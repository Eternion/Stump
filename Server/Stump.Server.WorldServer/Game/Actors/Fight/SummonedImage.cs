using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public class SummonedImage : SummonedClone
    {
        private readonly bool m_initialized;

        public SummonedImage(int id, FightActor caster, Cell cell)
            : base(id, caster, cell)
        {
            Frozen = true;
            m_stats.Health.DamageTaken = caster.Stats.Health.DamageTaken;

            Fight.TurnStarted += OnTurnStarted;
            caster.DamageInflicted += OnDamageInflicted;

            m_initialized = true;
        }

        public override bool IsVisibleInTimeline
        {
            get { return false; }
        }

        private void OnTurnStarted(IFight fight, FightActor player)
        {
            if (player == Summoner)
                Die();

            if (player != this)
                return;

            PassTurn();
        }

        protected override void OnBeforeDamageInflicted(Damage damage)
        {
            damage.Amount = LifePoints;
            base.OnBeforeDamageInflicted(damage);
        }

        protected override void OnDead(FightActor killedBy, bool passTurn = true)
        {
            Fight.TurnStarted -= OnTurnStarted;
            Summoner.DamageInflicted -= OnDamageInflicted;

            base.OnDead(killedBy, passTurn);
        }

        private void OnDamageInflicted(FightActor actor, Damage damage)
        {
            Die();
        }

        protected override void OnPositionChanged(ObjectPosition position)
        {
            base.OnPositionChanged(position);

            if (m_initialized)
                Die();
        }
    }
}
