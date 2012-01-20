using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Items
{
    public abstract class ItemEffectHandler : EffectHandler
    {
        protected ItemEffectHandler(EffectBase effect, Character target, Item item)
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

        public Item Item
        {
            get;
            protected set;
        }

        public bool Equiped
        {
            get { return Item.IsEquiped(); }
        }
    }
}