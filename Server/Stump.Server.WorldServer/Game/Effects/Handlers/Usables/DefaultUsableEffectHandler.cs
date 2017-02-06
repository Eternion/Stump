using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Usables
{
    [DefaultEffectHandler]
    public class DefaultUsableEffectHandler : UsableEffectHandler
    {
        public DefaultUsableEffectHandler(EffectBase effect, Character target, BasePlayerItem item)
            : base(effect, target, item)
        {
        }

        protected override bool InternalApply()
        {
            return true;
        }
    }
}