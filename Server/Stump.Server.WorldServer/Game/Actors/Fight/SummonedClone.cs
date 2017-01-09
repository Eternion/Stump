using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;
using System.Linq;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public class SummonedClone : SummonedFighter
    {
        protected readonly StatsFields m_stats;

        public SummonedClone(int id, FightActor caster, Cell cell)
            : base(id, caster.Team, new List<Spell>(), caster, cell)
        {
            Caster = caster;
            Look = caster.Look.Clone();
            m_stats = new StatsFields(this);
            m_stats.InitializeFromStats(caster.Stats);
            ResetUsedPoints();
        }

        public FightActor Caster
        {
            get;
        }

        public override ObjectPosition MapPosition => Position;

        public override string GetMapRunningFighterName() => Name;

        public override byte Level => Caster.Level;

        public override string Name => (Caster is NamedFighter) ? ((NamedFighter)Caster).Name : "(no name)";

        public override StatsFields Stats => m_stats;

        public override GameFightFighterInformations GetGameFightFighterInformations(WorldClient client = null)
        {
            var casterInfos = Caster.GetGameFightFighterInformations();

            if (casterInfos is GameFightCharacterInformations)
            {
                var characterInfos = casterInfos as GameFightCharacterInformations;

                return new GameFightCharacterInformations(Id, casterInfos.look, GetEntityDispositionInformations(), casterInfos.teamId,
                    0, IsAlive(), GetGameFightMinimalStats(), MovementHistory.GetEntries(2).Select(x => x.Cell.Id).ToArray(),
                    characterInfos.name, characterInfos.status, characterInfos.level, characterInfos.alignmentInfos, characterInfos.breed, characterInfos.sex);
            }

            return new GameFightFighterInformations(Id, casterInfos.look, GetEntityDispositionInformations(), casterInfos.teamId,
                0, IsAlive(), GetGameFightMinimalStats(), MovementHistory.GetEntries(2).Select(x => x.Cell.Id).ToArray());
        }

        public override FightTeamMemberInformations GetFightTeamMemberInformations() => new FightTeamMemberInformations(Id);
    }
}