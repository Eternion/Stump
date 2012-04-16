using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Stump.Core.Collections;
using Stump.Core.Extensions;
using Stump.Core.Threading;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;

namespace Stump.Server.WorldServer.Game.Maps.Spawns
{
    public class ClassicalSpawningPool : SpawningPoolBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected enum GroupSize
        {
            Small = 0,
            Medium = 1,
            Big = 2
        }

        protected Dictionary<GroupSize, Tuple<int, int>> GroupSizes = new Dictionary<GroupSize, Tuple<int, int>>()
        {
            {GroupSize.Small, Tuple.Create(1, 2)},
            {GroupSize.Medium, Tuple.Create(3, 5)},
            {GroupSize.Big, Tuple.Create(6, 8)},
        };

        private readonly object m_locker = new object();
        private readonly MonsterGroup[] m_groupsBySize = new MonsterGroup[3];
        private readonly Queue<GroupSize> m_groupsToSpawn = new Queue<GroupSize>();

        public ClassicalSpawningPool(Map map)
            : base(map)
        {
            RandomQueue();
        }

        public ClassicalSpawningPool(Map map, int interval)
            : base(map, interval)
        {
            RandomQueue();
        }

        private void RandomQueue()
        {
            var array = Enum.GetValues(typeof(GroupSize));
            
            foreach (var size in array.Cast<GroupSize>().Shuffle())
            {
                m_groupsToSpawn.Enqueue(size);
            }
        }

        /// <summary>
        /// 1-2 Group
        /// </summary>
        public MonsterGroup SmallGroup
        {
            get { return m_groupsBySize[(int) GroupSize.Small]; }
        }

        /// <summary>
        /// 3 - 5 group
        /// </summary>
        public MonsterGroup MediumGroup
        {
            get
            {
                return m_groupsBySize[(int)GroupSize.Medium];
            }
        }
        
        /// <summary>
        /// 6 - 8 group
        /// </summary>
        public MonsterGroup BigGroup
        {
            get
            {
                return m_groupsBySize[(int)GroupSize.Big];
            }
        }

        protected override bool IsLimitReached()
        {
            return m_groupsToSpawn.Count == 0;
        }

        protected override MonsterGroup DequeueNextGroupToSpawn()
        {
            lock (m_locker)
            {
                if (m_groupsToSpawn.Count == 0)
                {
                    return null;
                }

                var size = m_groupsToSpawn.Dequeue();

                return m_groupsBySize[(int) size] = Map.GenerateRandomMonsterGroup(GroupSizes[size].Item1, GroupSizes[size].Item2);
            }
        }

        protected override int GetNextSpawnInterval()
        {
            var rand = new AsyncRandom();
            if (rand.Next(0, 2) == 0)
            {
                return (int) ( (Interval - ( rand.NextDouble() * Interval / 4 )) * 1000 );
            }

            return (int) ( (Interval + ( rand.NextDouble() * Interval / 4 )) * 1000 );
        }

        protected override void OnGroupUnSpawned(MonsterGroup monster)
        {
            lock (m_locker)
            {
                if (monster == SmallGroup)
                {
                    m_groupsBySize[(int) GroupSize.Small] = null;
                    m_groupsToSpawn.Enqueue(GroupSize.Small);
                }

                if (monster == MediumGroup)
                {
                    m_groupsBySize[(int)GroupSize.Medium] = null;
                    m_groupsToSpawn.Enqueue(GroupSize.Medium);
                }

                if (monster == BigGroup)
                {
                    m_groupsBySize[(int)GroupSize.Big] = null;
                    m_groupsToSpawn.Enqueue(GroupSize.Big);
                }
            }

            base.OnGroupUnSpawned(monster);
        }
    }
}