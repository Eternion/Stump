using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Items
{
    [EffectHandler(EffectsEnum.Effect_AddTitle)]
    public class TitleEffectHandler : ItemEffectHandler
    {
        public TitleEffectHandler(EffectBase effect, Character target, BasePlayerItem item)
            : base(effect, target, item)
        {
        }

        public TitleEffectHandler(EffectBase effect, Character target, ItemSetTemplate itemSet, bool apply) 
            : base(effect, target, itemSet, apply)
        {
        }

        protected override bool InternalApply()
        {
            var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Item) as EffectInteger;

            if (integerEffect == null)
                return false;

            if (Operation == HandlerOperation.APPLY)
                Target.AddTitle(integerEffect.Value);
            else
                Target.RemoveTitle(integerEffect.Value);

            return true;
        }
    }
}
