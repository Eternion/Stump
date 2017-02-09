using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Items
{
    public abstract class ItemEffectHandler : EffectHandler
    {
        HandlerOperation? m_operation;

        public enum HandlerOperation
        {
            APPLY,
            UNAPPLY,
        }

        protected ItemEffectHandler(EffectBase effect, Character target, BasePlayerItem item)
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

        public BasePlayerItem Item
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
            set;
        }

        public bool Equiped
        {
            get
            {
                return Item != null && Item.IsEquiped();
            }
        }

        public bool Boost
        {
            get
            {
                return Item != null && Item.Template.Type.SuperType == ItemSuperTypeEnum.SUPERTYPE_BOOST;
            }
        }

        public HandlerOperation Operation
        {
            get
            {
                return m_operation ?? (Equiped || ItemSetApply ? HandlerOperation.APPLY : HandlerOperation.UNAPPLY);
            }
            set { m_operation = value; }
        }

    }
}