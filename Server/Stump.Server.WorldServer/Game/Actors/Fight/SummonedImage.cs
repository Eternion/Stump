using System.Collections.Generic;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public class SummonedImage : SummonedFighter
    {
        private readonly StatsFields m_stats;
        private readonly bool m_initialized;

        public SummonedImage(int id, FightActor caster, Cell cell)
            : base(id, caster.Team, new List<Spell>(), caster, cell)
        {
            Caster = caster;
            Look = caster.Look.Clone();
            m_stats = caster.Stats.CloneAndChangeOwner(this);
            Frozen = true;

            Fight.TurnStarted += OnTurnStarted;
            caster.DamageInflicted += OnDamageInflicted;

            m_initialized = true;
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
            get { return Summoner.GetMapRunningFighterName(); }
        }

        public override bool IsVisibleInTimeline
        {
            get { return false; }
        }

        public override StatsFields Stats
        {
            get { return m_stats; }
        }

        private void OnTurnStarted(IFight fight, FightActor player)
        {
            if (player == Summoner)
                InflictDirectDamage(LifePoints, Caster);

            if (player != this)
                return;

            PassTurn();
        }

        protected override void OnBeforeDamageInflicted(Damage damage)
        {
            damage.Amount = LifePoints;
            base.OnBeforeDamageInflicted(damage);
        }

        private void OnDamageInflicted(FightActor actor, Damage damage)
        {
            InflictDirectDamage(LifePoints, Caster);
        }

        protected override void OnPositionChanged(ObjectPosition position)
        {
            base.OnPositionChanged(position);

            if (m_initialized)
                InflictDirectDamage(LifePoints, Caster);
        }

        public override GameFightFighterInformations GetGameFightFighterInformations()
        {
            var casterInfos = Caster.GetGameFightFighterInformations();
            return new GameFightFighterNamedInformations(Id, casterInfos.look, GetEntityDispositionInformations(), casterInfos.teamId, 0, casterInfos.alive, casterInfos.stats, Name, new PlayerStatus());
        }

        public override FightTeamMemberInformations GetFightTeamMemberInformations()
        {
            return new FightTeamMemberInformations(Id);
        }
    }
}
