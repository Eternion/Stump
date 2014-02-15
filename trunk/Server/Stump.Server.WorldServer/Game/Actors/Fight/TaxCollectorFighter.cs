using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public class TaxCollectorFighter : AIFighter
    {
        private readonly StatsFields m_stats;

        public TaxCollectorFighter(FightTeam team, TaxCollectorNpc taxCollector)
            : base(team, taxCollector.Guild.GetTaxCollectorSpells(), taxCollector.GlobalId)
        {
            TaxCollectorNpc = taxCollector;

            m_stats = new StatsFields(this);
            m_stats.Initialize(TaxCollectorNpc);
        }

        public TaxCollectorNpc TaxCollectorNpc
        {
            get;
            private set;
        }

        public override string Name
        {
            get { return TaxCollectorNpc.Name; }
        }

        public override ObjectPosition MapPosition
        {
            get { return TaxCollectorNpc.Position; }
        }

        public override byte Level
        {
            get { return TaxCollectorNpc.Level; }
        }

        public override StatsFields Stats
        {
            get { return m_stats; }
        }

        public override string GetMapRunningFighterName()
        {
            return TaxCollectorNpc.Name;
        }
    }
}
