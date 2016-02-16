using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Maps.Cells;
using System.Linq;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
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
            caster.DamageInflicted += OnDamageInflicted;

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
            damage.Amount = int.MaxValue;
            base.OnBeforeDamageInflicted(damage);
        }

        protected override void OnDead(FightActor killedBy, bool passTurn = true)
        {
            Fight.TurnStarted -= OnTurnStarted;
            Summoner.DamageInflicted -= OnDamageInflicted;

            base.OnDead(killedBy, passTurn);

            ActionsHandler.SendGameActionFightVanishMessage(Fight.Clients, this, this);

            if (!Summoner.Summons.Any(x => x is SummonedImage))
                Summoner.SetInvisibilityState(GameActionFightInvisibilityStateEnum.VISIBLE);
        }

        void OnDamageInflicted(FightActor actor, Damage damage)
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
