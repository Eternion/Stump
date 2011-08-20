using Stump.Server.WorldServer.Worlds.Actors.Interfaces;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Effects.Instances;
using Stump.Server.WorldServer.Worlds.Items;

namespace Stump.Server.WorldServer.Worlds.Effects.Handlers.Items
{
    public abstract class ItemEffectHandler 
    {
        protected ItemEffectHandler(Character target, Item item, EffectBase effect)
        {
            Target = target;
            Item = item;
            Effect = effect;
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

        public EffectBase Effect
        {
            get;
            protected set;
        }

        public virtual void Apply()
        {
        }
    }
}