using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;
using Stump.Server.WorldServer.Handlers.Context;

using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Others
{
    [EffectHandler(EffectsEnum.Effect_ReviveAndGiveHPToLastDiedAlly)]
    public class ReviveAndGiveHP : SpellEffectHandler
    {
        public ReviveAndGiveHP(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        public FightActor LastDeadFighter
        {
            get;
            private set;
        }

        protected override bool InternalApply()
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

        void ReviveActor(FightActor actor, int heal)
        {
            var cell = TargetedCell;
            if (!Fight.IsCellFree(cell))
                cell = Map.GetRandomAdjacentFreeCell(TargetedPoint, true);

            actor.Revive(heal, Caster);
            actor.SummoningEffect = this;
            actor.Position.Cell = cell;

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
    }
}
