using Stump.Server.WorldServer.Worlds.Actors.Interfaces;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Effects.Instances;
using Stump.Server.WorldServer.Worlds.Items;

namespace Stump.Server.WorldServer.Worlds.Effects.Handlers.Items
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