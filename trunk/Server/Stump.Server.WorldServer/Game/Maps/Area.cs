using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Collections;
using Stump.Core.Threading;
using Stump.Core.Timers;
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Point = System.Drawing.Point;

namespace Stump.Server.WorldServer.Game.Maps
{
    public class Area : IContextHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Variable]
        public static readonly int DefaultUpdateDelay = 50;

        public static int[] UpdatePriorityMillis =
            new[]
                {
                    10000, // Inactive
                    3000, // Background
                    1000, // VeryLowPriority
                    6000, // LowPriority
                    300, // Active
                    0 // HighPriority
                };

        private readonly List<Character> m_characters = new List<Character>();


        private readonly List<Map> m_maps = new List<Map>();
        private readonly LockFreeQueue<IMessage> m_messageQueue = new LockFreeQueue<IMessage>();
        private readonly List<MonsterSpawn> m_monsterSpawns = new List<MonsterSpawn>();

        private readonly List<WorldObject> m_objects = new List<WorldObject>();
        private readonly List<SubArea> m_subAreas = new List<SubArea>();
        private readonly Dictionary<Point, List<Map>> m_mapsByPoint = new Dictionary<Point, List<Map>>();
        private readonly List<TimerEntry> m_timers = new List<TimerEntry>();
        protected internal AreaRecord Record;
        private float m_avgUpdateTime;
        private int m_currentThreadId;
        private bool m_isUpdating;
        private DateTime m_lastUpdateTime;
        private bool m_running;
        private int m_tickCount;
        private int m_updateDelay;

        public Area(AreaRecord record)
        {
            Record = record;
            m_updateDelay = DefaultUpdateDelay;
        }

        public int Id
        {
            get { return Record.Id; }
        }

        public string Name
        {
            get { return Record.Name; }
        }

        public IEnumerable<SubArea> SubAreas
        {
            get { return m_subAreas; }
        }

        public IEnumerable<Map> Maps
        {
            get { return m_maps; }
        }


        public Dictionary<System.Drawing.Point, List<Map>> MapsByPosition
        {
            get
            {
                return m_mapsByPoint;
            }
        }

        public SuperArea SuperArea
        {
            get;
            internal set;
        }

        public int ObjectCount
        {
            get { return m_objects.Count; }
        }

        /// <summary>
        /// Don't modify the List.
        /// </summary>
        public List<Character> Characters
        {
            get
            {
                EnsureContext();
                return m_characters;
            }
        }

        public int CharacterCount
        {
            get { return m_characters.Count; }
        }

        public bool IsRunning
        {
            get { return m_running; }
            set
            {
                if (m_running == value)
                    return;

                if (value)
                    Start();
                else
                    Stop();
            }
        }

        public int TickCount
        {
            get { return m_tickCount; }
        }

        public int UpdateDelay
        {
            get { return m_updateDelay; }
            set { Interlocked.Exchange(ref m_updateDelay, value); }
        }

        public DateTime LastUpdateTime
        {
            get { return m_lastUpdateTime; }
        }

        public bool IsUpdating
        {
            get { return m_isUpdating; }
        }

        public float AverageUpdateTime
        {
            get { return m_avgUpdateTime; }
        }

        public bool IsDisposed
        {
            get;
            private set;
        }

        #region IContextHandler Members

        public bool IsInContext
        {
            get { return Thread.CurrentThread.ManagedThreadId == m_currentThreadId; }
        }

        public void AddMessage(Action action)
        {
            AddMessage((Message) action);
        }

        public void AddMessage(IMessage msg)
        {
            // make sure, Map is running
            Start();
            m_messageQueue.Enqueue(msg);
        }

        public bool ExecuteInContext(Action action)
        {
            if (!IsInContext)
            {
                AddMessage(new Message(action));
                return false;
            }

            action();
            return true;
        }

        public void EnsureContext()
        {
            if (Thread.CurrentThread.ManagedThreadId != m_currentThreadId && IsRunning)
            {
                Stop();
                throw new InvalidOperationException(string.Format("Context needed in Area '{0}'", this));
            }
        }

        #endregion

        public event Action<Area> Started;

        private void OnStarted()
        {
            Action<Area> handler = Started;
            if (handler != null)
                handler(this);
        }

        public event Action<Area> Stopped;

        private void OnStopped()
        {
            Action<Area> handler = Stopped;
            if (handler != null)
                handler(this);
        }

        public void Start()
        {
            if (m_running)
                return;

            lock (m_objects)
            {
                if (m_running)
                    return;

                m_running = true;

                logger.Info("Area '{0}' started", this);

                Task.Factory.StartNewDelayed(m_updateDelay, UpdateCallback, this);

                SpawnMapsLater();

                m_lastUpdateTime = DateTime.Now;

                OnStarted();
            }
        }

        public void Stop()
        {
            if (!m_running)
                return;

            lock (m_objects)
            {
                if (!m_running)
                    return;

                m_running = false;

                logger.Info("Area '{0}' stopped", this);
            }
        }

        public void RegisterTimer(TimerEntry timer)
        {
            EnsureContext();
            m_timers.Add(timer);
        }

        public void UnregisterTimer(TimerEntry timer)
        {
            EnsureContext();
            m_timers.Remove(timer);
        }

        public void RegisterTimerLater(TimerEntry timer)
        {
            m_messageQueue.Enqueue(new Message(() => RegisterTimer(timer)));
        }

        public void UnregisterTimerLater(TimerEntry timer)
        {
            m_messageQueue.Enqueue(new Message(() => UnregisterTimer(timer)));
        }

        public TimerEntry CallDelayed(int millis, Action action)
        {
            var timer = new TimerEntry();
            timer.Action = delay =>
                               {
                                   action();
                                   UnregisterTimerLater(timer);
                               };
            timer.Start(millis, 0);
            RegisterTimerLater(timer);
            return timer;
        }

        public TimerEntry CallPeriodically(int seconds, Action action)
        {
            var timer = new TimerEntry
                            {
                                Action = (delay => { action(); })
                            };
            timer.Start(seconds, seconds);
            RegisterTimerLater(timer);
            return timer;
        }

        private void UpdateCallback(object state)
        {
            if ((IsDisposed || !IsRunning) || (Interlocked.CompareExchange(ref m_currentThreadId, Thread.CurrentThread.ManagedThreadId, 0) != 0))
                return;


            DateTime updateStart = DateTime.Now;
            var updateDelta = (int) ((updateStart - m_lastUpdateTime).TotalMilliseconds);

            try
            {
                IMessage msg;
                while (m_messageQueue.TryDequeue(out msg))
                {
                    try
                    {
                        msg.Execute();
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Exception raised when processing Message in {0} : {1}.", this, ex);
                    }
                }

                m_isUpdating = true;

                foreach (TimerEntry timerEntry in m_timers)
                {
                    try
                    {
                        timerEntry.Update(updateDelta);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Exception raised when processing TimerEntry in {0} : {1}.", this, ex);
                        UnregisterTimerLater(timerEntry);
                    }
                }

                // we copy to allow manipulations on the list
                foreach (WorldObject obj in m_objects.GetRange(0, m_objects.Count))
                {
                    if (obj.IsDisposed) // disposed items should not be in the list
                    {
                        m_objects.Remove(obj);
                        continue;
                    }

                    if (obj.IsTeleporting)
                        continue;

                    // todo : use priority
                    UpdatePriority priority = UpdatePriority.HighPriority;

                    try
                    {
                        int minObjUpdateDelta = UpdatePriorityMillis[(int) priority];
                        var objUpdateDelta = (int) ((updateStart - obj.LastUpdateTime).TotalMilliseconds);

                        if (objUpdateDelta >= minObjUpdateDelta)
                        {
                            obj.LastUpdateTime = updateStart;
                            obj.Update(objUpdateDelta);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Exception raised when updating object '{0}' : {1}", obj, this);

                        if (obj is Character)
                        {
                            ((Character) obj).Client.Disconnect();
                        }
                        else
                        {
                            obj.Delete();
                        }
                    }
                }
            }
            finally
            {
                // we updated the map, so set our last update time to now
                m_lastUpdateTime = updateStart;
                m_tickCount++;
                m_isUpdating = false;

                // get the time, now that we've finished our update callback
                DateTime updateEnd = DateTime.Now;
                TimeSpan newUpdateDelta = updateEnd - updateStart;

                // weigh old update-time 9 times and new update-time once
                m_avgUpdateTime = ((m_avgUpdateTime*9) + (float) (newUpdateDelta).TotalMilliseconds)/10;


                // make sure to unset the ID *before* enqueuing the task in the ThreadPool again
                Interlocked.Exchange(ref m_currentThreadId, 0);
                var callbackTimeout = (int) (m_updateDelay - newUpdateDelta.TotalMilliseconds);
                if (callbackTimeout < 0)
                {
                    // even if we are in a hurry: For the sake of load-balance we have to give control back to the ThreadPool
                    callbackTimeout = 0;
                    logger.Debug("Area '{0}' update lagged ({1}ms)", this, (int) newUpdateDelta.TotalMilliseconds);
                }

                Task.Factory.StartNewDelayed(callbackTimeout, UpdateCallback, this);
            }
        }

        public void Dispose()
        {
            IsDisposed = true;

            if (IsRunning)
                Stop();
        }

        public void Enter(WorldObject obj)
        {
            m_objects.Add(obj);

            if (obj is Character)
            {
                m_characters.Add((Character) obj);

                if (!IsRunning)
                    Start();
            }
        }

        public void Leave(WorldObject obj)
        {
            m_objects.Remove(obj);

            if (obj is Character)
            {
                m_characters.Remove((Character) obj);

                if (m_characters.Count <= 0 && IsRunning)
                    Stop();
            }
        }

        public void SpawnMapsLater()
        {
            AddMessage(SpawnMaps);
        }

        public void SpawnMaps()
        {
            EnsureContext();

            foreach (var map in Maps)
            {
                if (!map.SpawnEnabled && map.MonsterSpawnsCount > 0)
                    map.EnableClassicalMonsterSpawns();
            }
        }

        public void CallOnAllCharacters(Action<Character> action)
        {
            ExecuteInContext(() =>
                                 {
                                     foreach (Character chr in m_characters)
                                     {
                                         action(chr);
                                     }
                                 });
        }

        internal void AddSubArea(SubArea subArea)
        {
            m_subAreas.Add(subArea);
            m_maps.AddRange(subArea.Maps);

            foreach (Map map in subArea.Maps)
            {
                if (!m_mapsByPoint.ContainsKey(map.Position))
                    m_mapsByPoint.Add(map.Position, new List<Map>());

                m_mapsByPoint[map.Position].Add(map);
            }

            subArea.Area = this;
        }

        internal void RemoveSubArea(SubArea subArea)
        {
            m_subAreas.Remove(subArea);
            m_maps.RemoveAll(delegate(Map entry)
            {
                if (subArea.Maps.Contains(entry))
                {
                    if (m_mapsByPoint.ContainsKey(entry.Position))
                    {
                        var list = m_mapsByPoint[entry.Position];
                        list.Remove(entry);

                        if (list.Count <= 0)
                            m_mapsByPoint.Remove(entry.Position);
                    }

                    return true;
                }

                return false;
            });

            subArea.Area = null;
        }

        public Map[] GetMaps(int x, int y)
        {
            return GetMaps(new Point(x, y));
        }

        public Map[] GetMaps(int x, int y, bool outdoor)
        {
            return GetMaps(new Point(x, y), outdoor);
        }

        public Map[] GetMaps(Point position)
        {
            if (!m_mapsByPoint.ContainsKey(position))
                return new Map[0];

            return m_mapsByPoint[position].ToArray();
        }

        public Map[] GetMaps(Point position, bool outdoor)
        {
            if (!m_mapsByPoint.ContainsKey(position))
                return new Map[0];

            return m_mapsByPoint[position].Where(entry => entry.Outdoor == outdoor).ToArray();
        }


        public void AddMonsterSpawn(MonsterSpawn spawn)
        {
            m_monsterSpawns.Add(spawn);

            foreach (var subArea in SubAreas)
            {
                subArea.AddMonsterSpawn(spawn);
            }
        }

        public void RemoveMonsterSpawn(MonsterSpawn spawn)
        {
            m_monsterSpawns.Remove(spawn);

            foreach (var subArea in SubAreas)
            {
                subArea.RemoveMonsterSpawn(spawn);
            }
        }

        public ReadOnlyCollection<MonsterSpawn> MonsterSpawns
        {
            get
            {
                return m_monsterSpawns.AsReadOnly();
            }
        }

        public void EnsureNoContext()
        {
            if (Thread.CurrentThread.ManagedThreadId == m_currentThreadId)
            {
                Stop();
                throw new InvalidOperationException(string.Format("Context prohibitted in Area '{0}'", this));
            }
        }

        public void EnsureNotUpdating()
        {
            if (m_isUpdating)
            {
                Stop();
                throw new InvalidOperationException(string.Format("Area '{0}' is updating", this));
            }
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Id);
        }
    }
}