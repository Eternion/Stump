using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Usables
{
    [EffectHandler(EffectsEnum.Effect_GiveKamas)]
    public class GiveKamas : UsableEffectHandler
    {
        public GiveKamas(EffectBase effect, Character target, BasePlayerItem item) : base(effect, target, item)
        {
        }

        public override bool Apply()
        {
            var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Item) as EffectInteger;

            if (integerEffect == null)
                return false;

            var kamasAmount = (int)(integerEffect.Value * NumberOfUses);

            UsedItems = NumberOfUses;
            Target.Inventory.AddKamas(kamasAmount);

            //BasicHandler.SendTextInformationMessage(Target.Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 1, heal);

            return true;
        }
    }
}
