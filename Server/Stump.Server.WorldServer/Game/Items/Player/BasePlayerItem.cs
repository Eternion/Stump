using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Cache;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Server.WorldServer.Game.Items.Player
{
    public abstract class BasePlayerItem : PersistantItem<PlayerItemRecord>
    {
        #region Fields

        public Character Owner
        {
            get;
            private set;
        }


        #endregion

        #region Constructors

        protected BasePlayerItem(Character owner, PlayerItemRecord record)
            : base(record)
        {
            m_objectItemValidator = new ObjectValidator<ObjectItem>(BuildObjectItem);

            Owner = owner;
        }

        #endregion

        #region Functions

        public virtual bool AreConditionFilled(Character character)
        {
            try
            {
                return Template.CriteriaExpression == null ||
                    Template.CriteriaExpression.Eval(character);
            }
            catch
            {
                return false;
            }
        }

        public virtual bool IsLinkedToAccount()
        {
            if (Template.IsLinkedToOwner)
                return true;

            if (Template.Type.SuperType == ItemSuperTypeEnum.SUPERTYPE_QUEST)
                return true;

            if (IsTokenItem())
                return true;

            return Effects.Any(x => x.EffectId == EffectsEnum.Effect_NonExchangeable_982);
        }

        public virtual bool IsLinkedToPlayer()
        {
            if (Template.IsLinkedToOwner)
                return true;

            if (Template.Type.SuperType == ItemSuperTypeEnum.SUPERTYPE_QUEST)
                return true;

            if (IsTokenItem())
                return true;

            return Effects.Any(x => x.EffectId == EffectsEnum.Effect_NonExchangeable_981);
        }

        public bool IsTokenItem()
        {
            return Inventory.ActiveTokens && Template.Id == Inventory.TokenTemplateId;
        }

        public virtual bool IsUsable()
        {
            return Template.Usable;
        }

        public virtual bool IsEquiped()
        {
            return Position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True whenever the item can be added</returns>
        public virtual bool OnAddItem()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True whenever the item can be removed</returns>
        public virtual bool OnRemoveItem()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <param name="targetCell"></param>
        /// <param name="target"></param>
        /// <returns>Returns the amount of items to remove</returns>
        public virtual uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            if (amount < 0)
                throw new ArgumentException("amount < 0", "amount");

            uint removed = 0;
            foreach (var handler in Effects.Select(effect => EffectManager.Instance.GetUsableEffectHandler(effect, target ?? Owner, this)))
            {
                handler.NumberOfUses = (uint)amount;
                handler.TargetCell = targetCell;

                if (handler.Apply())
                    removed = Math.Max(handler.UsedItems, removed);
            }

            return removed;
        }

        public virtual bool OnEquipItem(bool unequip)
        {
            return true;
        }

        public virtual bool AllowFeeding
        {
            get
            {
                return false;
            }
        }

        public virtual bool Feed(BasePlayerItem food)
        {
            return false;
        }

        public virtual bool AllowDropping
        {
            get
            {
                return false;
            }
        }

        public virtual bool Drop(BasePlayerItem dropOnItem)
        {
            return false;
        }

        public void OnObjectModified()
        {
            Record.IsDirty = true;
        }

        #region ObjectItem

        private readonly ObjectValidator<ObjectItem> m_objectItemValidator;

        protected virtual ObjectItem BuildObjectItem()
        {
            return new ObjectItem(
                (byte) Position,
                (short) Template.Id,
                0, // todo : power rate
                false, // todo : over max
                Effects.Where(entry => !entry.Hidden).Select(entry => entry.GetObjectEffect()),
                Guid,
                (int)Stack);
        }

        public override ObjectItem GetObjectItem()
        {
            return m_objectItemValidator;
        }

        /// <summary>
        /// Call it each time you modify part of the item
        /// </summary>
        public virtual void Invalidate()
        {
            m_objectItemValidator.Invalidate();
        }

        #endregion

        #endregion

        #region Properties

        public override int Guid
        {
            get { return base.Guid; }
            protected set
            {
                base.Guid = value;
                Invalidate();
            }
        }

        public override ItemTemplate Template
        {
            get { return base.Template; }
            protected set
            {
                base.Template = value;
                Invalidate();
            }
        }

        public override uint Stack
        {
            get { return base.Stack; }
            set
            {
                base.Stack = value;
                Invalidate();
            }
        }


        public override List<EffectBase> Effects
        {
            get { return base.Effects; }
            protected set
            {
                base.Effects = value;
                Invalidate();
            }
        }

        public virtual CharacterInventoryPositionEnum Position
        {
            get { return Record.Position; }
            set
            {
                Record.Position = value;
                Invalidate();
            }
        }

        public virtual uint AppearanceId
        {
            get
            {
                return Template.AppearanceId;
            }
        }

        public virtual int Weight
        {
            get { return (int) (Template.RealWeight*Stack); }
        }

        #endregion
    }
}