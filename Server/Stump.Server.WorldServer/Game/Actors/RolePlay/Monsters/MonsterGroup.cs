using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;
using Stump.Server.WorldServer.Game.Maps.Spawns;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters
{
    public sealed class MonsterGroup : RolePlayActor, IContextDependant, IAutoMovedEntity
    {
        [Variable(true)]
        public static int StarsBonusInterval = 300;

        [Variable(true)]
        public static short StarsBonusIncrementation = 2;

        [Variable(true)]
        public static short StarsBonusLimit = 200;

        public const short ClientStarsBonusLimit = 200;

        public event Action<MonsterGroup, Character> EnterFight;
        public event Action<MonsterGroup, IFight> ExitFight;

        private readonly List<Monster> m_monsters = new List<Monster>();

        public MonsterGroup(int id, ObjectPosition position, SpawningPoolBase spawningPool = null)
        {
            ContextualId = id;
            Position = position;
            CreationDate = DateTime.Now;
            SpawningPool = spawningPool;
        }

        public IFight Fight
        {
            get;
            private set;
        }

        public override int Id
        {
            get { return ContextualId; }
            protected set { ContextualId = value; }
        }

        public int ContextualId
        {
            get;
            private set;
        }

        public SpawningPoolBase SpawningPool
        {
            get;
            set;
        }

        public GroupSize GroupSize
        {
            get;
            set;
        }

        public Monster Leader
        {
            get;
            private set;
        }

        public short AgeBonus
        {
            get
            {
                var bonus = ( DateTime.Now - CreationDate ).TotalSeconds / ((double)StarsBonusInterval / StarsBonusIncrementation);

                if (bonus > StarsBonusLimit)
                    bonus = StarsBonusLimit;

                return (short) bonus;
            }
        }
        
        public DateTime NextMoveDate
        {
            get;
            set;
        }

        public DateTime LastMoveDate
        {
            get;
            private set;
        }

        public DateTime CreationDate
        {
            get;
            private set;
        }
        public override bool CanMove()
        {
            return true;
        }

        public override bool IsMoving()
        {
            return false;
        }

        public override bool StartMove(Path movementPath)
        {
            if (!CanMove() || movementPath.IsEmpty())
                return false;

            Position = movementPath.EndPathPosition;
            var keys = movementPath.GetServerPathKeys();

            Map.ForEach(entry => ContextHandler.SendGameMapMovementMessage(entry.Client, keys, this));

            // monsters movements are instants
            StopMove();
            LastMoveDate = DateTime.Now;

            return true;
        }

        public override bool StopMove()
        {
            return false;
        }

        public override bool StopMove(ObjectPosition currentObjectPosition)
        {
            return false;
        }

        public override bool MoveInstant(ObjectPosition destination)
        {
            return false;
        }

        public override bool Teleport(ObjectPosition destination, bool performCheck = true)
        {
            return false;
        }

        public void FightWith(Character character)
        {
            if (character.Map != Map)
                return;

            Map.Leave(this);

            if (Map.GetBlueFightPlacement().Length < m_monsters.Count)
            {
                character.SendServerMessage("Cannot start fight : Not enough fight placements");
                return;
            }

            var reason = character.CanAttack(this);
            if (reason != FighterRefusedReasonEnum.FIGHTER_ACCEPTED)
            {                
                ContextHandler.SendChallengeFightJoinRefusedMessage(character.Client, character, reason);
                return;
            }

            var fight = FightManager.Instance.CreatePvMFight(Map);

            fight.ChallengersTeam.AddFighter(character.CreateFighter(fight.ChallengersTeam));

            foreach (var monster in CreateFighters(fight.DefendersTeam))
                fight.DefendersTeam.AddFighter(monster);

            Fight = fight;

            fight.StartPlacement();

            OnEnterFight(character);

            Fight.FightEnded += OnFightEnded;
        }

        private void OnFightEnded(IFight fight)
        {
            OnExitFight(fight);
        }

        private void OnEnterFight(Character character)
        {
            var handler = EnterFight;
            if (handler != null)
                handler(this, character);
        }
        private void OnExitFight(IFight fight)
        {
            Fight = null;

            var handler = ExitFight;
            if (handler != null) handler(this, fight);
        }

        public IEnumerable<MonsterFighter> CreateFighters(FightMonsterTeam team)
        {
            return m_monsters.Select(monster => monster.CreateFighter(team));
        }

        public void AddMonster(Monster monster)
        {
            monster.SetMonsterGroup(this);
            m_monsters.Add(monster);

            if (m_monsters.Count == 1)
                Leader = monster;

            Map.Refresh(this);
        }

        public void RemoveMonster(Monster monster)
        {
            m_monsters.Remove(monster);

            if (m_monsters.Count == 0)
                Leader = null;

            Map.Refresh(this);
        }

        public IEnumerable<Monster> GetMonsters()
        {
            return m_monsters;
        }

        public IEnumerable<Monster> GetMonstersWithoutLeader()
        {
            return m_monsters.Where(entry => entry != Leader);
        }

        public int Count()
        {
            return m_monsters.Count;
        }

        public override GameContextActorInformations GetGameContextActorInformations(Character character)
        {
            return new GameRolePlayGroupMonsterInformations(Id,
                                                            Leader.Look.GetEntityLook(),
                                                            GetEntityDispositionInformations(),
                                                            GetGroupMonsterStaticInformations(),
                                                            AgeBonus > ClientStarsBonusLimit ? ClientStarsBonusLimit : AgeBonus,
                                                            0,
                                                            -1,
                                                            false);
        }

        public GroupMonsterStaticInformations GetGroupMonsterStaticInformations()
        {
            return new GroupMonsterStaticInformations(Leader.GetMonsterInGroupLightInformations(), GetMonstersWithoutLeader().Select(entry => entry.GetMonsterInGroupInformations()));
        }

        public override string ToString()
        {
            return string.Format("{0} monsters ({1})", m_monsters.Count, Id);
        }

    }
}