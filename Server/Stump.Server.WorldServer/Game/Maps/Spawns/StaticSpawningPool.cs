using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Maps.Spawns
{
    public class StaticSpawningPool : SpawningPoolBase
    {
        [Variable(true)]
        public static int StaticSpawnsInterval = 3;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly List<MonsterStaticSpawn> m_spawns = new List<MonsterStaticSpawn>();
        private Queue<MonsterStaticSpawn> m_spawnsQueue = new Queue<MonsterStaticSpawn>();
        private readonly Dictionary<MonsterGroup, MonsterStaticSpawn> m_groupsSpawn = new Dictionary<MonsterGroup, MonsterStaticSpawn>();

        private readonly object m_locker = new object();

        public StaticSpawningPool(Map map)
            : this(map, StaticSpawnsInterval)
        {
        }

        public StaticSpawningPool(Map map, int interval)
            : base(map, interval)
        {
        }

        public ReadOnlyCollection<MonsterStaticSpawn> Spawns
        {
            get { return m_spawns.AsReadOnly(); }
        }

        public void AddSpawn(MonsterStaticSpawn spawn)
        {
            lock (m_locker)
            {
                m_spawns.Add(spawn);
                m_spawnsQueue.Enqueue(spawn);
            }
        }

        public void RemoveSpawn(MonsterStaticSpawn spawn)
        {
            lock (m_locker)
            {
                m_spawns.Remove(spawn);

                var asList = m_spawnsQueue.ToList();
                if (asList.Remove(spawn))
                    m_spawnsQueue = new Queue<MonsterStaticSpawn>(asList);
            }
        }

        protected override bool IsLimitReached()
        {
            return m_spawnsQueue.Count == 0;
        }

        protected override int GetNextSpawnInterval()
        {
            return Interval*1000;
        }

        protected override MonsterGroup DequeueNextGroupToSpawn()
        {
            if (!Map.CanSpawnMonsters())
            {
                StopAutoSpawn();
                return null;
            }

            lock (m_locker)
            {
                if (m_spawnsQueue.Count == 0)
                {
                    return null;
                }

                var spawn = m_spawnsQueue.Dequeue();

                var group = new MonsterGroup(Map.GetNextContextualId(), new ObjectPosition(Map, spawn.Cell, (DirectionsEnum)spawn.Direction), this);
                foreach (var monsterGrade in spawn.GroupMonsters)
                {
                    group.AddMonster(new Monster(monsterGrade, group));
                }

                m_groupsSpawn.Add(group, spawn);

                return group;
            }
        }

        protected override void OnGroupUnSpawned(MonsterGroup monster)
        {
            lock (m_locker)
            {
                if (!m_groupsSpawn.ContainsKey(monster))
                {
                    logger.Error("Group {0} (Map {1}) was not bind to a dungeon spawn", monster.Id, Map.Id);
                }
                else
                {
                    var spawn = m_groupsSpawn[monster];

                    if (m_spawns.Contains(spawn))
                        m_spawnsQueue.Enqueue(spawn);
                }
            }

            base.OnGroupUnSpawned(monster);
        }
    }
}
