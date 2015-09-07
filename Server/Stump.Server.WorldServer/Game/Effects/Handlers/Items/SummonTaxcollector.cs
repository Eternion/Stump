using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Items
{
    [EffectHandler(EffectsEnum.Effect_SummonTaxcollector)]
    public class SummonTaxcollector : ItemEffectHandler
    {
        public SummonTaxcollector(EffectBase effect, Character target, BasePlayerItem item)
            : base(effect, target, item)
        {
        }

        public override bool Apply()
        {
            if (Target.Guild == null)
                return false;

            TaxCollectorManager.Instance.AddTaxCollectorSpawn(Target);

            return true;
        }
    }
}
