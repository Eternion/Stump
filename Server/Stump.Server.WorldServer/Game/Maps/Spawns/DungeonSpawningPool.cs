using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Monster = Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters.Monster;

namespace Stump.Server.WorldServer.Game.Maps.Spawns
{
    public class DungeonSpawningPool : SpawningPoolBase
    {
        [Variable(true)]
        public static int DungeonSpawnsInterval = 30;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly object m_locker = new object();
        private readonly List<MonsterDungeonSpawn> m_spawns = new List<MonsterDungeonSpawn>();
        private Queue<MonsterDungeonSpawn> m_spawnsQueue = new Queue<MonsterDungeonSpawn>();
        private readonly Dictionary<MonsterGroup, MonsterDungeonSpawn> m_groupsSpawn = new Dictionary<MonsterGroup, MonsterDungeonSpawn>(); 

        public DungeonSpawningPool(Map map)
            : this(map, DungeonSpawnsInterval)
        {
        }

        public DungeonSpawningPool(Map map, int interval)
            : base(map, interval)
        {
        }

        public ReadOnlyCollection<MonsterDungeonSpawn> Spawns
        {
            get { return m_spawns.AsReadOnly(); }
        }

        public void AddSpawn(MonsterDungeonSpawn spawn)
        {
            lock (m_locker)
            {
                m_spawns.Add(spawn);
                m_spawnsQueue.Enqueue(spawn);
            }
        }

        public void RemoveSpawn(MonsterDungeonSpawn spawn)
        {
            lock (m_locker)
            {
                m_spawns.Remove(spawn);

                var asList = m_spawnsQueue.ToList();
                if (asList.Remove(spawn))
                    m_spawnsQueue = new Queue<MonsterDungeonSpawn>(asList);
            }
        }

        protected override bool IsLimitReached()
        {
            return m_spawnsQueue.Count == 0;
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
                    logger.Error("SpawningPool Map = {0} try to spawn a monser but m_groupsToSpawn is empty", Map.Id);
                    return null;
                }

                var spawn = m_spawnsQueue.Dequeue();

                var cell = spawn.CellId == null ? Map.GetRandomFreeCell() : Map.Cells[(int)spawn.CellId];

                var group = new MonsterGroup(Map.GetNextContextualId(), new ObjectPosition(Map, cell, Map.GetRandomDirection()), this);
                foreach (var monsterGrade in spawn.GroupMonsters)
                {
                    group.AddMonster(new Monster(monsterGrade, group));
                }

                m_groupsSpawn.Add(group, spawn);

                return group;
            }
        }

        protected override int GetNextSpawnInterval()
        {
            return Interval * 1000;
        }

        protected override void OnGroupSpawned(MonsterGroup group)
        {
            group.EnterFight += OnGroupEnterFight;

            base.OnGroupSpawned(group);
        }

        private void OnGroupEnterFight(MonsterGroup group, Character character)
        {
            group.EnterFight -= OnGroupEnterFight;
            group.Fight.WinnersDetermined += OnWinnersDetermined;
        }

        private void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            fight.WinnersDetermined -= OnWinnersDetermined;

            if (draw)
                return;

            // if players didn't win they don't get teleported
            if (!(winners is FightPlayerTeam) || !(losers is FightMonsterTeam))
                return;

            var group = ((MonsterFighter) losers.Leader).Monster.Group;

            if (!m_groupsSpawn.ContainsKey(@group))
            {
                logger.Error("Group {0} (Map {1}) has ended his fight but is not register in the pool", @group.Id, Map.Id);
                return;
            }

            var spawn = m_groupsSpawn[@group];

            if (!spawn.TeleportEvent)
                return;

            var pos = spawn.GetTeleportPosition();

            foreach (var fighter in winners.GetAllFighters<CharacterFighter>())
            {
                fighter.Character.NextMap = pos.Map;
                fighter.Character.Cell = pos.Cell;
                fighter.Character.Direction = pos.Direction;

                
                m_groupsSpawn.Remove(group);
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