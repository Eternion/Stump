using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Usables
{
    [DefaultEffectHandler]
    public class DefaultUsableEffectHandler : UsableEffectHandler
    {
        public DefaultUsableEffectHandler(EffectBase effect, Character target, PlayerItem item)
            : base(effect, target, item)
        {
        }

        public override bool Apply()
        {
            return true;
        }
    }
}