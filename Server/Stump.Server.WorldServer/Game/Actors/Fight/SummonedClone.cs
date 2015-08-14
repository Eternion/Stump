using System.Collections.Generic;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

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
            m_stats = caster.Stats.CloneAndChangeOwner(this);
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
            get { return (Caster is NamedFighter) ? ((NamedFighter)Caster).Name : "(no name)"; }
        }

        public override StatsFields Stats
        {
            get { return m_stats; }
        }

        public GameFightFighterNamedInformations GetGameFightFighterNamedInformations()
        {
            var casterInfos = Caster.GetGameFightFighterInformations();
            return new GameFightFighterNamedInformations(Id, casterInfos.look, GetEntityDispositionInformations(), casterInfos.teamId, IsAlive(), GetGameFightMinimalStats(), Name);
        }

        public override GameFightFighterInformations GetGameFightFighterInformations()
        {
            var casterInfos = Caster.GetGameFightFighterInformations();
            return new GameFightFighterInformations(Id, casterInfos.look, GetEntityDispositionInformations(), casterInfos.teamId, IsAlive(), GetGameFightMinimalStats());
        }

        public override FightTeamMemberInformations GetFightTeamMemberInformations()
        {
            return new FightTeamMemberInformations(Id);
        }
    }
}