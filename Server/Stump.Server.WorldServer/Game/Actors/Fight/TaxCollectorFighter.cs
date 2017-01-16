using System.Collections.Generic;
using System.Linq;
using NLog.LayoutRenderers;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public sealed class TaxCollectorFighter : AIFighter
    {
        private readonly StatsFields m_stats;

        public TaxCollectorFighter(FightTeam team, TaxCollectorNpc taxCollector)
            : base(team, taxCollector.Guild.GetTaxCollectorSpells(), taxCollector.GlobalId)
        {
            Id = Fight.GetNextContextualId();
            TaxCollectorNpc = taxCollector;
            Look = TaxCollectorNpc.Look.Clone();
            Items = TaxCollectorNpc.Bag.SelectMany(x => Enumerable.Repeat(x.Template.Id, (int) x.Stack))
                            .Shuffle()
                            .ToList();
            Kamas = TaxCollectorNpc.GatheredKamas;

            m_stats = new StatsFields(this);
            m_stats.Initialize(TaxCollectorNpc);
            
            if (!Fight.FindRandomFreeCell(this, out var cell, false))
                return;

            Position = new ObjectPosition(TaxCollectorNpc.Map, cell, TaxCollectorNpc.Direction);

        }

        public TaxCollectorNpc TaxCollectorNpc
        {
            get;
        }

        public override string Name => TaxCollectorNpc.Name;

        public override ObjectPosition MapPosition => TaxCollectorNpc.Position;

        public override byte Level => TaxCollectorNpc.Level;

        public override StatsFields Stats => m_stats;

        public List<int> Items
        {
            get;
        }

        public int Kamas
        {
            get;
        }

        public override string GetMapRunningFighterName() => TaxCollectorNpc.Name;

        public override IFightResult GetFightResult(FightOutcomeEnum outcome) => new TaxCollectorFightResult(this, outcome, Loot);

        public TaxCollectorFightersInformation GetTaxCollectorFightersInformation()
        {
            var allies = Fight.State == FightState.Placement && Fight is FightPvT pvtFight
                ? pvtFight.DefendersQueue.Select(x => x.GetCharacterMinimalPlusLookInformations())
                : Team.Fighters.OfType<CharacterFighter>().Select(x => x.Character.GetCharacterMinimalPlusLookInformations());

            return new TaxCollectorFightersInformation(TaxCollectorNpc.GlobalId, allies,
                OpposedTeam.Fighters.OfType<CharacterFighter>().Select(x => x.Character.GetCharacterMinimalPlusLookInformations()));
        }

        public override GameFightFighterLightInformations GetGameFightFighterLightInformations(WorldClient client = null) => new GameFightFighterTaxCollectorLightInformations(
                true,
                IsAlive(),
                Id,
                0,
                Level,
                (sbyte)BreedEnum.TAX_COLLECTOR,
                TaxCollectorNpc.FirstNameId,
                TaxCollectorNpc.LastNameId);

        public override FightTeamMemberInformations GetFightTeamMemberInformations() => new FightTeamMemberTaxCollectorInformations(Id, TaxCollectorNpc.FirstNameId,
                TaxCollectorNpc.LastNameId, TaxCollectorNpc.Level, TaxCollectorNpc.Guild.Id,
                TaxCollectorNpc.GlobalId);

        public override GameFightFighterInformations GetGameFightFighterInformations(WorldClient client = null) => new GameFightTaxCollectorInformations(
                Id,
                Look.GetEntityLook(),
                GetEntityDispositionInformations(client),
                (sbyte)Team.Id,
                0,
                IsAlive(),
                GetGameFightMinimalStats(client),
                new short[0],
                TaxCollectorNpc.FirstNameId,
                TaxCollectorNpc.LastNameId,
                TaxCollectorNpc.Level);
    }
}
