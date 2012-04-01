using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Handlers.Items;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Handlers.Basic;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Usables
{
    [EffectHandler(EffectsEnum.Effect_AddHealth)]
    public class RestoreHp : UsableEffectHandler
    {
        public RestoreHp(EffectBase effect, Character target, Item item)
            : base(effect, target, item)
        {
        }

        public override void Apply()
        {
            var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Item) as EffectInteger;

            if (integerEffect == null)
                return;

            if (Target.Stats.Health.DamageTaken == 0)
            {
                // health already to max
                BasicHandler.SendTextInformationMessage(Target.Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 225);
                return;
            }

            if (Target.Stats.Health.DamageTaken < integerEffect.Value)
            {
                integerEffect.Value = Target.Stats.Health.DamageTaken;
            }

            Target.Stats.Health.DamageTaken -= integerEffect.Value;

            // x hp restored
            Target.RefreshHealth();
            BasicHandler.SendTextInformationMessage(Target.Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 1, integerEffect.Value);
            Target.PlayEmote(EmotesEnum.EMOTE_EAT);
        }
    }
}