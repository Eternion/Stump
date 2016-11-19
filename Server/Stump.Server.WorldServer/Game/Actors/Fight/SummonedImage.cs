using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Maps.Cells;
using System.Linq;
using Stump.Server.WorldServer.Handlers.Actions;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public class SummonedImage : SummonedClone
    {
        readonly bool m_initialized;

        public SummonedImage(int id, FightActor caster, Cell cell)
            : base(id, caster, cell)
        {
            m_stats.Health.DamageTaken = caster.Stats.Health.DamageTaken;

            Fight.TurnStarted += OnTurnStarted;
            caster.DamageInflicted += OnCasterDamageInflicted;

            m_initialized = true;
        }

        public override bool CanPlay() => false;

        void OnTurnStarted(IFight fight, FightActor player)
        {
            if (player == Summoner)
                Die();

            if (player != this)
                return;

            PassTurn();
        }

        protected override void OnBeforeDamageInflicted(Damage damage)
        {
            Die();

            damage.Amount = 0;
            base.OnBeforeDamageInflicted(damage);
        }

        protected override void OnDead(FightActor killedBy, bool passTurn = true)
        {
            Fight.TurnStarted -= OnTurnStarted;
            Summoner.DamageInflicted -= OnCasterDamageInflicted;

            using (Fight.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL))
            {
                ActionsHandler.SendGameActionFightVanishMessage(Fight.Clients, Summoner, this);

                Summoner.RemoveSummon(this);

                if (!Summoner.Summons.Any(x => x is SummonedImage))
                    Summoner.SetInvisibilityState(GameActionFightInvisibilityStateEnum.VISIBLE);
            }
        }

        void OnCasterDamageInflicted(FightActor actor, Damage damage)
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
