using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Usables
{
    public abstract class UsableEffectHandler : EffectHandler
    {
        protected UsableEffectHandler(EffectBase effect, Character target, PlayerItem item)
            : base (effect)
        {
            Target = target;
            Item = item;
        }

        public Character Target
        {
            get;
            protected set;
        }

        public PlayerItem Item
        {
            get;
            protected set;
        }
    }
}