using System.Collections.Generic;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public class SummonedClone : SummonedFighter
    {
        private readonly StatsFields m_stats;

        public SummonedClone(int id, FightActor caster, Cell cell)
            : base(id, caster.Team, new List<Spell>(), caster, cell)
        {
            Caster = caster;
            Look = caster.Look.Clone();
            m_stats = new StatsFields(caster);
            var fighter = caster as CharacterFighter;
            if (fighter != null)
                m_stats.Initialize(fighter.Character.Record);
            else
                m_stats.Initialize(((MonsterFighter) caster).Monster.Grade);
        }

        public FightActor Caster
        {
            get;
            private set;
        }

        public override ObjectPosition MapPosition
        {
            get { return Position; }
        }

        public override string GetMapRunningFighterName()
        {
            return Name;
        }

        public override byte Level
        {
            get { return Caster.Level; }
        }

        public override string Name
        {
            get { return (Caster is NamedFighter) ? ((NamedFighter)Caster).Name : "Unknown"; }
        }

        public override StatsFields Stats
        {
            get { return m_stats; }
        }
    }
}