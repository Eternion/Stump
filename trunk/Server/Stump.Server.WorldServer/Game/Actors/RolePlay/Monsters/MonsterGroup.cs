using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Attributes;
using Stump.Core.Timers;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters
{
    public sealed class MonsterGroup : RolePlayActor
    {
        [Variable(true)]
        public static int MinMoveInterval = 20;

        [Variable(true)]
        public static int MaxMoveInterval = 40;

        [Variable(true)]
        public static int StarsBonusInterval = 300;

        [Variable(true)]
        public static short StarsBonusIncrementation = 2;

        [Variable(true)]
        public static short StarsBonusLimit = 300;

        public const short ClientStarsBonusLimit = 200;

        public event Action<MonsterGroup, Character> EnterFight;

        private readonly List<Monster> m_monsters = new List<Monster>();

        public MonsterGroup(int id, ObjectPosition position)
        {
            ContextualId = id;
            Position = position;
        }

        public Fights.Fight Fight
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

        public Monster Leader
        {
            get;
            private set;
        }

        public short AgeBonus
        {
            get;
            private set;
        }

        internal TimerEntry MoveTimer
        {
            get;
            set;
        }

        internal TimerEntry StarsTimer
        {
            get;
            set;
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

        public override bool Teleport(ObjectPosition destination)
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

            Fights.Fight fight = FightManager.Instance.CreatePvMFight(Map);

            fight.RedTeam.AddFighter(character.CreateFighter(fight.RedTeam));

            foreach (MonsterFighter monster in CreateFighters(fight.BlueTeam))
                fight.BlueTeam.AddFighter(monster);

            Fight = fight;

            fight.StartPlacement();

            OnEnterFight(character);
        }

        private void OnEnterFight(Character character)
        {
            var handler = EnterFight;
            if (handler != null)
                EnterFight(this, character);
        }

        public IEnumerable<MonsterFighter> CreateFighters(FightTeam team)
        {
            return m_monsters.Select(monster => monster.CreateFighter(team));
        }

        public void AddMonster(Monster monster)
        {
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

        public void IncrementBonus(short bonus)
        {
            AgeBonus += bonus;

            if (AgeBonus > StarsBonusLimit)
                AgeBonus = StarsBonusLimit;
        }

        public void DecrementBonus(short bonus)
        {
            AgeBonus -= bonus;

            if (AgeBonus < 0)
                AgeBonus = 0;
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

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            return new GameRolePlayGroupMonsterInformations(Id,
                                                            Leader.Look,
                                                            GetEntityDispositionInformations(),
                                                            Leader.Template.Id,
                                                            (sbyte) Leader.Grade.GradeId,
                                                            GetMonstersWithoutLeader().Select(entry => entry.GetMonsterInGroupInformations()),
                                                            AgeBonus > ClientStarsBonusLimit ? ClientStarsBonusLimit : AgeBonus,
                                                            -1,
                                                            false);
        }

        protected override void OnDisposed()
        {
            base.OnDisposed();
        }

        public override string ToString()
        {
            return string.Format("{0} monsters ({1})", m_monsters.Count, Id);
        }
    }
}