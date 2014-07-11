using System.Collections.Generic;
using System.Linq;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.World;
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

            
            Cell cell;
            if (!Fight.FindRandomFreeCell(this, out cell, false))
                return;

            Position = new ObjectPosition(TaxCollectorNpc.Map, cell, TaxCollectorNpc.Direction);

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

        public List<int> Items
        {
            get;
            private set;
        }

        public int Kamas
        {
            get;
            private set;
        }

        public override string GetMapRunningFighterName()
        {
            return TaxCollectorNpc.Name;
        }

        public override IFightResult GetFightResult(FightOutcomeEnum outcome)
        {
            return new TaxCollectorFightResult(this, outcome, Loot);
        }

        public TaxCollectorFightersInformation GetTaxCollectorFightersInformation()
        {
            var allies = Fight.State == FightState.Placement && Fight is FightPvT
                ? (Fight as FightPvT).DefendersQueue.Select(x => x.GetCharacterMinimalPlusLookInformations())
                : Team.Fighters.OfType<CharacterFighter>().Select(x => x.Character.GetCharacterMinimalPlusLookInformations());

            return new TaxCollectorFightersInformation(TaxCollectorNpc.GlobalId, allies,
                OpposedTeam.Fighters.OfType<CharacterFighter>().Select(x => x.Character.GetCharacterMinimalPlusLookInformations()));
        }

        public override FightTeamMemberInformations GetFightTeamMemberInformations()
        {
            return new FightTeamMemberTaxCollectorInformations(Id, TaxCollectorNpc.FirstNameId,
                TaxCollectorNpc.LastNameId, TaxCollectorNpc.Level, TaxCollectorNpc.Guild.Id,
                TaxCollectorNpc.GlobalId);
        }

        public override GameFightFighterInformations GetGameFightFighterInformations(WorldClient client = null)
        {
            return new GameFightTaxCollectorInformations(Id,
                Look.GetEntityLook(),
                GetEntityDispositionInformations(client),
                (sbyte)Team.Id,
                IsAlive(),
                GetGameFightMinimalStats(client),
                TaxCollectorNpc.FirstNameId,
                TaxCollectorNpc.LastNameId,
                TaxCollectorNpc.Level);
        }
    }
}
