using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Items
{
    public abstract class ItemEffectHandler : EffectHandler
    {
        public enum HandlerOperation
        {
            APPLY,
            UNAPPLY,
        }

        protected ItemEffectHandler(EffectBase effect, Character target, PlayerItem item)
            : base (effect)
        {
            Target = target;
            Item = item;
        }

        protected ItemEffectHandler(EffectBase effect, Character target, ItemSetTemplate itemSet, bool apply)
            : base (effect)
        {
            Target = target;
            ItemSet = itemSet;
            ItemSetApply = apply;
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

        public ItemSetTemplate ItemSet
        {
            get;
            protected set;
        }

        public bool ItemSetApply
        {
            get;
            protected set;
        }

        public bool Equiped
        {
            get
            {
                return Item != null && Item.IsEquiped();
            }
        }

        public HandlerOperation Operation
        {
            get
            {
                return Equiped || ItemSetApply ? HandlerOperation.APPLY : HandlerOperation.UNAPPLY;
            }
        }

    }
}