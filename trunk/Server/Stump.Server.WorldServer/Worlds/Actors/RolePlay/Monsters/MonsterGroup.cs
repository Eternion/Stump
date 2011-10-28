using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Fights;
using Stump.Server.WorldServer.Worlds.Maps.Cells;
using Stump.Server.WorldServer.Worlds.Maps.Pathfinding;

namespace Stump.Server.WorldServer.Worlds.Actors.RolePlay.Monsters
{
    public sealed class MonsterGroup : RolePlayActor
    {
        private readonly List<Monster> m_monsters = new List<Monster>();

        public MonsterGroup(int id, ObjectPosition position)
        {
            ContextualId = id;
            Position = position;
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

        public override bool CanMove()
        {
            return true;
        }

        public override bool IsMoving()
        {
            return false;
        }

        public override bool StartMove(MovementPath movementPath)
        {
            if (!CanMove())
                return false;

            Position = movementPath.End;
            List<short> keys = movementPath.GetServerMovementKeys();

            Map.ForEach(entry => ContextHandler.SendGameMapMovementMessage(entry.Client, keys, this));

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

            Fights.Fight fight = FightManager.Instance.Create(Map, FightTypeEnum.FIGHT_TYPE_PvM);

            fight.RedTeam.AddFighter(character.CreateFighter(fight.RedTeam));

            foreach (MonsterFighter monster in CreateFighters(fight.BlueTeam))
                fight.BlueTeam.AddFighter(monster);

            fight.StartPlacementPhase();
        }

        public IEnumerable<MonsterFighter> CreateFighters(FightTeam team)
        {
            Map.Leave(this);

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
        }

        public void DecrementBonus(short bonus)
        {
            AgeBonus -= bonus;
        }

        public IEnumerable<Monster> GetMonsters()
        {
            return m_monsters;
        }

        public IEnumerable<Monster> GetMonstersWithoutLeader()
        {
            return m_monsters.Where(entry => entry != Leader);
        }

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            return new GameRolePlayGroupMonsterInformations(Id,
                                                            Leader.Look,
                                                            GetEntityDispositionInformations(),
                                                            Leader.Template.Id,
                                                            (sbyte) Leader.Grade.GradeId,
                                                            GetMonstersWithoutLeader().Select(entry => entry.GetMonsterInGroupInformations()),
                                                            AgeBonus,
                                                            -1,
                                                            false);
        }
    }
}