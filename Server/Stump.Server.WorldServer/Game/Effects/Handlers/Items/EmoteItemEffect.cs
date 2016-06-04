using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Items
{
    [EffectHandler(EffectsEnum.Effect_LearnEmote)]
    public class EmoteItemEffect : ItemEffectHandler
    {
        public EmoteItemEffect(EffectBase effect, Character target, BasePlayerItem item)
            : base(effect, target, item)
        {
        }

        public EmoteItemEffect(EffectBase effect, Character target, ItemSetTemplate itemSet, bool apply)
            : base(effect, target, itemSet, apply)
        {
        }

        protected override bool InternalApply()
        {
            var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Item) as EffectInteger;

            if (integerEffect == null)
                return false;

            if (Operation == HandlerOperation.APPLY)
                Target.AddEmote((EmotesEnum)integerEffect.Value);
            else
                Target.RemoveEmote((EmotesEnum)integerEffect.Value);

            return true;
        }
    }
}