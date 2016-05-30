using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Usables
{
    [EffectHandler(EffectsEnum.Effect_LearnSmileyPack)]
    public class LearnSmileyPack : UsableEffectHandler
    {
        public LearnSmileyPack(EffectBase effect, Character target, BasePlayerItem item)
            : base(effect, target, item)
        {
        }

        protected override bool InternalApply()
        {
            var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Item) as EffectInteger;

            if (integerEffect == null)
                return false;

            if (Target.HasSmileyPack((SmileyPacksEnum)integerEffect.Value))
                return false;
            
            UsedItems = 1;

            Target.AddSmileyPack((SmileyPacksEnum)integerEffect.Value);

            return true;
        }
    }
}
