using System;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public class SummonedTurret : SummonedFighter
    {
        protected readonly StatsFields m_stats;

        public SummonedTurret(int id, FightActor summoner, MonsterGrade template, Cell cell)
            : base(id, summoner.Team, template.Spells, summoner, cell)
        {
            Caster = summoner;
            Monster = template;
            m_stats = summoner.Stats.CloneAndChangeOwner(this);
        }

        public FightActor Caster
        {
            get;
            private set;
        }

        public MonsterGrade Monster
        {
            get;
            private set;
        }

        public override string Name
        {
            get { return Monster.Template.Name; }
        }

        public override ObjectPosition MapPosition
        {
            get { return Position; }
        }

        public override byte Level
        {
            get { return (byte)Monster.Level; }
        }

        public override ActorLook Look
        {
            get { return Monster.Template.EntityLook; }
        }

        public override StatsFields Stats
        {
            get { return m_stats; }
        }

        public override string GetMapRunningFighterName()
        {
            return Name;
        }
    }
}
