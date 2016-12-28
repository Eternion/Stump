using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Plugins.DefaultPlugin.Items
{
    public static class ItemsFix
    {
        [Initialization(typeof(ItemManager), Silent = true)]
        public static void ApplyFix()
        {
            foreach (var template in ItemManager.Instance.GetTemplates())
            {
                template.PossibleEffects.RemoveAll(x => x.EffectId == (uint)EffectsEnum.Effect_Exchangeable);
                template.Effects.RemoveAll(x => x.EffectId == EffectsEnum.Effect_Exchangeable);
            }

            // fix pets effect values
            var weightEffect = EffectManager.Instance.GetTemplate((short) EffectsEnum.Effect_IncreaseWeight);
            weightEffect.Characteristic = (int) CharacteristicEnum.WEIGHT;
        }
    }
}