using NLog;
using Stump.Core.Attributes;
using Stump.Core.Collections;
using Stump.Core.Threading;
using Stump.Core.Timers;
using Stump.Server.BaseServer.Benchmark;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Point = System.Drawing.Point;

namespace Stump.Server.WorldServer.Game.Maps
{
    public class Area : IContextHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Variable]
        public static readonly int DefaultUpdateDelay = 50;

        public static int[] UpdatePriorityMillis =
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
        private readonly PriorityQueueB<TimedTimerEntry> m_timers = new PriorityQueueB<TimedTimerEntry>(new TimedTimerComparer());
        private readonly List<TimedTimerEntry> m_pausedTimers = new List<TimedTimerEntry>();
        private readonly ManualResetEvent m_stoppedAsync = new ManualResetEvent(false);
        protected internal AreaRecord Record;
        private int m_currentThreadId;
        private bool m_isUpdating;
        private DateTime m_lastUpdateTime;
        private bool m_running;
        private int m_updateDelay;
        private TimedTimerEntry m_checkDCtimer;
        private Task m_currentTask;

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

        public Dictionary<Point, List<Map>> MapsByPosition
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

        public int TimersCount
        {
            get { return m_timers.Count; }
        }

        public List<IMessage> MessageQueue
        {
            get { return m_messageQueue.ToList(); }
        }

        public int MsgQueueCount
        {
            get { return m_messageQueue.Count; }
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
            get;
            private set;
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
            get;
            private set;
        }

        public bool IsDisposed
        {
            get;
            private set;
        }

        public int CurrentThreadId
        {
            get { return m_currentThreadId; }
        }

        #region IContextHandler Members

        public bool IsInContext
        {
            get { return Thread.CurrentThread.ManagedThreadId == m_currentThreadId; }
        }

        public void AddMessage(Action action)
        {
            AddMessage((Message)action);
        }

        public void AddMessage(IMessage msg)
        {
            // make sure, Map is running
            // Start();
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
            if (Thread.CurrentThread.ManagedThreadId == m_currentThreadId || !IsRunning)
                return;

            Stop();
            throw new InvalidOperationException(string.Format("Context needed in Area '{0}'", this));
        }

        #endregion IContextHandler Members

        public event Action<Area> Started;

        private void OnStarted()
        {
            m_checkDCtimer = CallPeriodically((int)TimeSpan.FromMinutes(5).TotalMilliseconds, CheckDC);

            var handler = Started;
            if (handler != null)
                handler(this);
        }

        public event Action<Area> Stopped;

        private void OnStopped()
        {
            var handler = Stopped;
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
            Stop(false);
        }

        public void Stop(bool wait)
        {
            if (!m_running)
                return;

            lock (m_objects)
            {
                if (!m_running)
                    return;

                m_running = false;

                if (wait && m_currentThreadId != 0)
                    m_stoppedAsync.WaitOne(TimeSpan.FromSeconds(5));

                logger.Info("Area '{0}' stopped", this);
            }
        }

        public void RegisterTimer(TimedTimerEntry timer)
        {
            ExecuteInContext(() =>
            {
                if (!timer.Enabled)
                    timer.Start();

                m_timers.Push(timer);
            });
        }

        public void UnregisterTimer(TimedTimerEntry timer)
        {
            EnsureContext();
            m_timers.Remove(timer);
        }

        public TimedTimerEntry CallDelayed(int delay, Action action)
        {
            var timer = new TimedTimerEntry
            {
                Interval = -1,
                Delay = delay,
                Action = action
            };

            timer.Start();
            RegisterTimer(timer);
            return timer;
        }

        public TimedTimerEntry CallPeriodically(int interval, Action action)
        {
            var timer = new TimedTimerEntry
            {
                Interval = interval,
                Action = action
            };

            timer.Start();
            RegisterTimer(timer);
            return timer;
        }

        private void UpdateCallback(object state)
        {
            if ((IsDisposed || !IsRunning) ||
                (Interlocked.CompareExchange(ref m_currentThreadId, Thread.CurrentThread.ManagedThreadId, 0) != 0))
            {
                logger.Info($"Area {this} exit callback since it's disposed");
                return;
            }

            var updateStart = DateTime.Now;
            var updateDelta = (int)((updateStart - m_lastUpdateTime).TotalMilliseconds);
            long messageProcessTime = 0;
            long timerProcessingTime = 0;
            var timerProcessed = 0;
            var processedMessages = new List<BenchmarkEntry>();
            try
            {
                var sw = Stopwatch.StartNew();
                IMessage msg;
                while (m_messageQueue.TryDequeue(out msg))
                {
                    var swMsg = Stopwatch.StartNew();
                    try
                    {
                        msg.Execute();
                        swMsg.Stop();
                        if (BenchmarkManager.Enable && swMsg.Elapsed.TotalMilliseconds > 50)
                            processedMessages.Add(BenchmarkEntry.Create(msg.ToString(), swMsg.Elapsed, "area", Id));
                    }
                    catch (Exception ex)
                    {
                        swMsg.Stop();
                        logger.Error("Exception raised when processing Message in {0} : {1}.", this, ex);
                        if (BenchmarkManager.Enable)
                            processedMessages.Add(BenchmarkEntry.Create(msg.ToString(), swMsg.Elapsed, "area", Id, "exception", ex));
                    }
                }
                sw.Stop();
                messageProcessTime = sw.ElapsedMilliseconds;

                m_isUpdating = true;

                foreach (var timer in m_pausedTimers.Where(timer => timer.Enabled))
                {
                    m_timers.Push(timer);
                }

                sw = Stopwatch.StartNew();
                TimedTimerEntry peek;
                while ((peek = m_timers.Peek()) != null && peek.NextTick <= DateTime.Now)
                {
                    var timer = m_timers.Pop();

                    if (!timer.Enabled)
                    {
                        if (!timer.IsDisposed)
                            m_pausedTimers.Add(timer);
                    }
                    else
                    {
                        try
                        {
                            var swMsg = Stopwatch.StartNew();
                            timer.Trigger();
                            swMsg.Stop();
                            
                            if (BenchmarkManager.Enable && swMsg.Elapsed.TotalMilliseconds > 20)
                                processedMessages.Add(BenchmarkEntry.Create(timer.ToString(), swMsg.Elapsed, "area", Id));

                            if (timer.Enabled)
                                m_timers.Push(timer);

                            timerProcessed++;
                        }
                        catch (Exception ex)
                        {
                            logger.Error("Exception raised when processing TimerEntry in {0} : {1}.", this, ex);
                        }
                    }
                }
                sw.Stop();
                timerProcessingTime = sw.ElapsedMilliseconds;
            }
            finally
            {
                try
                {
                    // we updated the map, so set our last update time to now
                    m_lastUpdateTime = updateStart;
                    TickCount++;
                    m_isUpdating = false;

                    // get the time, now that we've finished our update callback
                    var updateEnd = DateTime.Now;
                    var newUpdateDelta = updateEnd - updateStart;

                    // weigh old update-time 9 times and new update-time once
                    AverageUpdateTime = ((AverageUpdateTime * 9) + (float)(newUpdateDelta).TotalMilliseconds) / 10;

                    // make sure to unset the ID *before* enqueuing the task in the ThreadPool again
                    Interlocked.Exchange(ref m_currentThreadId, 0);
                    var callbackTimeout = (int)(m_updateDelay - newUpdateDelta.TotalMilliseconds);
                    if (callbackTimeout < 0)
                    {
                        // even if we are in a hurry: For the sake of load-balance we have to give control back to the ThreadPool
                        callbackTimeout = 0;
                        logger.Debug("Area '{0}' update lagged ({1}ms) (msg:{2}ms, timers:{3}ms, timerProc:{4}/{5})",
                            this, (int)newUpdateDelta.TotalMilliseconds, messageProcessTime, timerProcessingTime, timerProcessed, m_timers.Count);
                        foreach (var msg in processedMessages.OrderByDescending(x => x.Timestamp).Take(15))
                        {
                            logger.Debug(msg);
                        }

                        BenchmarkManager.Instance.AddRange(processedMessages.OrderByDescending(x => x.Timestamp).Take(15));
                    }

                    if (!m_running)
                        m_stoppedAsync.Set();
                    else
                        m_currentTask = Task.Factory.StartNewDelayed(callbackTimeout, UpdateCallback, this);
                }
                catch (Exception ex)
                {
                    logger.Error("Area {0}. Could not recall callback !! Exception {1}", this, ex);
                }
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

            if (!(obj is Character))
                return;

            m_characters.Add((Character)obj);

            if (!IsRunning)
                Start();
        }

        public void Leave(WorldObject obj)
        {
            m_objects.Remove(obj);

            if (!(obj is Character))
                return;

            m_characters.Remove((Character)obj);

            if (m_characters.Count <= 0 && IsRunning)
                Stop();
        }

        public void CheckDC()
        {
            var count = m_characters.RemoveAll(x => !x.IsLoggedIn);
            m_objects.RemoveAll(x => x is Character && !((Character)x).IsLoggedIn);

            if (count > 0)
            {
                logger.Warn("{0} disconnected characters removed from {1}", count, this);
            }
        }

        public void SpawnMapsLater()
        {
            AddMessage(SpawnMaps);
        }

        public void SpawnMaps()
        {
            EnsureContext();

            foreach (var map in Maps.Where(map => map.MonsterSpawnsCount > 0))
            {
                map.EnableClassicalMonsterSpawns();
            }
        }

        public void CallOnAllCharacters(Action<Character> action)
        {
            ExecuteInContext(() =>
                                 {
                                     foreach (var chr in m_characters)
                                     {
                                         action(chr);
                                     }
                                 });
        }

        internal void AddSubArea(SubArea subArea)
        {
            m_subAreas.Add(subArea);
            m_maps.AddRange(subArea.Maps);

            foreach (var map in subArea.Maps)
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
            m_maps.RemoveAll(delegate (Map entry)
            {
                if (!subArea.Maps.Contains(entry))
                    return false;

                if (!m_mapsByPoint.ContainsKey(entry.Position))
                    return true;

                var list = m_mapsByPoint[entry.Position];
                list.Remove(entry);

                if (list.Count <= 0)
                    m_mapsByPoint.Remove(entry.Position);

                return true;
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
            return !m_mapsByPoint.ContainsKey(position) ? new Map[0] : m_mapsByPoint[position].ToArray();
        }

        public Map[] GetMaps(Point position, bool outdoor)
        {
            return !m_mapsByPoint.ContainsKey(position) ? new Map[0] : m_mapsByPoint[position].Where(entry => entry.Outdoor == outdoor).ToArray();
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
            if (Thread.CurrentThread.ManagedThreadId != m_currentThreadId)
                return;

            Stop();
            throw new InvalidOperationException($"Context prohibitted in Area '{this}'");
        }

        public void EnsureNotUpdating()
        {
            if (!m_isUpdating)
                return;

            Stop();
            throw new InvalidOperationException($"Area '{this}' is updating");
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Id);
        }
    }
}