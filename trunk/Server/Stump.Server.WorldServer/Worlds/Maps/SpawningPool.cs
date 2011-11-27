using System;
using System.Linq;
using System.Timers;
using NLog;
using Stump.Core.Threading;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Monsters;

namespace Stump.Server.WorldServer.Worlds.Maps
{
    public class SpawningPool
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Map m_map;
        private readonly SubArea m_subArea;
        private readonly Timer m_timer;
        private MonsterGroup m_nextGroup;
        private DateTime m_startTime;

        public event Action<SpawningPool> Enabled;

        private void NotifyEnabled()
        {
            var handler = Enabled;
            if (handler != null) handler(this);
        }

        public event Action<SpawningPool> Disabled;

        private void NotifyDisabled()
        {
            var handler = Disabled;
            if (handler != null) handler(this);
        }

        public event Action<SpawningPool, MonsterGroup> Spawned;

        private void NotifySpawned(MonsterGroup group)
        {
            var handler = Spawned;
            if (handler != null) handler(this, group);
        }

        public SpawningPool(uint time, Map map)
        {
            Time = time;
            m_map = map;

            m_timer = new Timer();
            m_timer.Elapsed += TimerElapsed;
            m_timer.Interval =  RandomTimer(time) * 1000;
            LimitReached = entry => m_map.GetMonsterSpawnsCount() >= m_map.GetMonsterSpawnsLimit();
        }

        public SpawningPool(uint time, SubArea subArea)
        {
            Time = time;
            m_subArea = subArea;

            m_timer = new Timer();
            m_timer.Elapsed += TimerElapsed;
            m_timer.Interval = RandomTimer(time) * 1000;
        }

        public Predicate<SpawningPool> LimitReached
        {
            get;
            set;
        }

        public uint Time
        {
            get;
            set;
        }

        public bool IsEnabled
        {
            get;
            private set;
        }

        public int SpawnTimeLeft
        {
            get
            {
                return ( TimeSpan.FromSeconds(m_timer.Interval) - (DateTime.Now - m_startTime) ).Seconds;
            }
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (m_map == null && m_subArea == null)
            {
                logger.Error("Spawning pool running without defined map or subarea ");
                return;
            }

            Map map;
            if (m_subArea != null)
            {
                var maps = m_subArea.Maps.ToArray();
                var rand = new AsyncRandom();

                map = maps[rand.Next(0, maps.Length)];
            }
            else
            {
                map = m_map;
            }

            var group = m_nextGroup ?? map.GenerateRandomMonsterGroup();
            m_nextGroup = null;

            if (group == null)
                return;

            map.Enter(group);
            NotifySpawned(group);

            if (LimitReached != null && LimitReached(this))
                Disable();

            m_timer.Interval = RandomTimer(Time) * 1000;
        }

        public void SetTimer(uint time)
        {
            Time = time;
            m_timer.Interval = RandomTimer(time) * 1000;
        }

        public void SetNextSpawnedGroup(MonsterGroup group)
        {
            m_nextGroup = group;
        }

        public void Enable()
        {
            m_startTime = DateTime.Now;
            m_timer.Start();

            IsEnabled = true;
            NotifyEnabled();
        }

        public void Disable()
        {
            m_timer.Stop();

            m_nextGroup = null;
            IsEnabled = false;

            NotifyDisabled();
        }

        private static uint RandomTimer(uint time)
        {
            var rand = new AsyncRandom();
            if (rand.Next(0, 2) == 0)
            {
                return (uint) (time - ( rand.NextDouble() * time / 4 ));
            }
            else
            {
                return (uint)( time + ( rand.NextDouble() * time / 4 ) );
            }
        }
    }
}