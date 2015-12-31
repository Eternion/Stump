using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Handlers.Basic;
using System;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Usables
{
    [EffectHandler(EffectsEnum.Effect_RestoreEnergyPoints)]
    public class RestoreEnergy : UsableEffectHandler
    {
        public RestoreEnergy(EffectBase effect, Character target, BasePlayerItem item)
            : base(effect, target, item)
        {
        }

        public override bool Apply()
        {
            var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Item) as EffectInteger;

            if (integerEffect == null)
                return false;

            if (Target.Energy == Target.EnergyMax)
            {
                // energy already to max
                BasicHandler.SendTextInformationMessage(Target.Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 253);
                return false;
            }

            var energy = (int)(integerEffect.Value * NumberOfUses);
            if ((Target.EnergyMax - Target.Energy) < energy)
            {
                energy = Target.EnergyMax - Target.Energy;
            }

            UsedItems = (uint)Math.Ceiling((double)energy / integerEffect.Value);
            Target.Energy += (short)energy;

            // x energy restored
            Target.RefreshStats();
            BasicHandler.SendTextInformationMessage(Target.Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 92, energy);
            Target.PlayEmote(EmotesEnum.EMOTE_BOIRE_UNE_POTION);

            return true;
        }
    }
}
