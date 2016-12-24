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
            // fix pets effect values
            var weightEffect = EffectManager.Instance.GetTemplate((short) EffectsEnum.Effect_IncreaseWeight);
            weightEffect.Characteristic = (int) CharacteristicEnum.WEIGHT;
        }
    }
}