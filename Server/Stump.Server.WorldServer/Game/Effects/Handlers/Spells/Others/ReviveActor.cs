using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Others
{
    [EffectHandler(EffectsEnum.Effect_ReviveAndGiveHPToLastDiedAlly)]
    public class ReviveAndGiveHP : SpellEffectHandler
    {
        public ReviveAndGiveHP(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public FightActor LastDeadFighter
        {
            get;
            private set;
        }

        public override bool Apply()
        {
            var integerEffect = GenerateEffect();

            if (integerEffect == null)
                return false;

            LastDeadFighter = Caster.Team.GetLastDeadFighter();

            if (LastDeadFighter == null)
                return false;

            ReviveActor(LastDeadFighter, integerEffect.Value);

            return true;
        }

        private void ReviveActor(FightActor actor, int heal)
        {
            HealHpPercent(actor, heal);
            actor.Position.Cell = TargetedCell;

            ActionsHandler.SendGameActionFightReviveMessage(Fight.Clients, Caster, actor);
            ContextHandler.SendGameFightTurnListMessage(Fight.Clients, Fight);

            Caster.Dead += OnCasterDead;
        }

        public void OnCasterDead(FightActor actor, FightActor killer)
        {
            if (LastDeadFighter != null && LastDeadFighter.IsAlive())
                LastDeadFighter.Die();

            Caster.Dead -= OnCasterDead;
        }

        private void HealHpPercent(FightActor actor, int percent)
        {
            var healAmount = (int)(actor.MaxLifePoints * (percent / 100d));

            if (healAmount <= 0)
                healAmount = 1;

            actor.Heal(healAmount, Caster, false);
        }
    }
}
