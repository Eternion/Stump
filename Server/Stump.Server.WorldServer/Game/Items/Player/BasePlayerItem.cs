using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Cache;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Actors.RolePlay;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Handlers.Items;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Maps;

namespace Stump.Server.WorldServer.Game.Items.Player
{
    public abstract class BasePlayerItem : PersistantItem<PlayerItemRecord>
    {
        #region Fields

        public Character Owner
        {
            get;
        }


        #endregion

        #region Constructors

        protected BasePlayerItem(Character owner, PlayerItemRecord record)
            : base(record)
        {
            m_objectItemValidator = new ObjectValidator<ObjectItem>(BuildObjectItem);

            Owner = owner;

            if (IsEquiped() && Template.FavoriteSubAreas.Length > 0)
                Owner.EnterMap += OnEnterMap;

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

        public bool IsTokenItem() => Inventory.ActiveTokens && Template.Id == Inventory.TokenTemplateId;

        public virtual bool IsUsable() => Template.Usable;

        public bool IsEquiped() => Position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED;

        public virtual bool CanEquip() => true;

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
            IsDeleted = true;
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
            if (!unequip && Template.FavoriteSubAreas.Length > 0)
            {
                Owner.EnterMap += OnEnterMap;
            }
            else if (unequip && Template.FavoriteSubAreas.Length > 0)
            {
                Owner.EnterMap -= OnEnterMap;
            }
            return true;
        }

        private void OnEnterMap(RolePlayActor actor, Map map)
        {
            CheckFavoriteSubAreas();
        }

        private void CheckFavoriteSubAreas()
        {
            if (Owner.LastMap != null && Template.FavoriteSubAreas.Contains((uint)Owner.LastMap.SubArea.Id) &&
                !Template.FavoriteSubAreas.Contains((uint)Owner.Map.SubArea.Id))
            {
                Owner.Inventory.ApplyItemEffects(this, false, ItemEffectHandler.HandlerOperation.UNAPPLY, 1 + Template.FavoriteSubAreasBonus/100d);
                Owner.Inventory.ApplyItemEffects(this, true, ItemEffectHandler.HandlerOperation.APPLY, 1);
            }
            if ((Owner.LastMap != null && !Template.FavoriteSubAreas.Contains((uint)Owner.LastMap.SubArea.Id)) &&
                Template.FavoriteSubAreas.Contains((uint)Owner.Map.SubArea.Id))
            {
                // Votre $item%1 se sent plus fort(e) dans cette zone, ses bonus sont augmentés temporairement de %2%%.
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 233, Template.Id, Template.FavoriteSubAreasBonus);
                Owner.Inventory.ApplyItemEffects(this, false, ItemEffectHandler.HandlerOperation.UNAPPLY, 1);
                Owner.Inventory.ApplyItemEffects(this, true, ItemEffectHandler.HandlerOperation.APPLY, 1 + Template.FavoriteSubAreasBonus/100d);
            }
        }

        public virtual bool CanFeed(BasePlayerItem item) => false;

        public virtual bool Feed(BasePlayerItem food) => false;

        public virtual bool CanDrop(BasePlayerItem item) => false;

        public virtual bool Drop(BasePlayerItem dropOnItem) => false;

        public void OnObjectModified()
        {
            Record.IsDirty = true;
        }

        public void OnEffectApplied(bool apply)
        {
            EffectApplied = apply;
        }

        public bool EffectApplied
        {
            get;
            private set;
        }

        public virtual ActorLook UpdateItemSkin(ActorLook characterLook)
        {
            if (AppearanceId > 0)
            {
                var look = characterLook.GetRiderLook() ?? characterLook;

                if (IsEquiped())
                    look.AddSkin((short)AppearanceId);
                else
                    look.RemoveSkin((short)AppearanceId);
            }

            return characterLook;
        }

        #region ObjectItem

        private readonly ObjectValidator<ObjectItem> m_objectItemValidator;

        protected virtual ObjectItem BuildObjectItem()
        {
            return new ObjectItem(
                (byte) Position,
                (short) Template.Id,
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


        public ObjectItemNotInContainer GetObjectItemNotInContainer()
        {
            return new ObjectItemNotInContainer(
                (short)Template.Id,
                Effects.Where(entry => !entry.Hidden).Select(entry => entry.GetObjectEffect()),
                Guid,
                (int)Stack);
        }

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

        public sealed override ItemTemplate Template
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
            set
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
                var appearanceId = Template.AppearanceId;

                var effect = Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_Appearance || x.EffectId == EffectsEnum.Effect_Apparence_Wrapper);
                if (effect != null)
                {
                    var itemId = ((EffectInteger)effect).Value;
                    var item = ItemManager.Instance.TryGetTemplate(itemId);

                    if (item != null)
                        appearanceId = item.AppearanceId;
                }

                return appearanceId;
            }
        }

        private EffectInteger m_sinkEffect;

        public short PowerSink
        {
            get {
                var sinkEffect = m_sinkEffect ?? (m_sinkEffect = Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_PowerSink) as EffectInteger);

                return sinkEffect?.Value ?? 0;
            }
            set
            {
                var sinkEffect = m_sinkEffect ?? (m_sinkEffect = Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_PowerSink) as EffectInteger);

                if (sinkEffect != null)
                {
                    if (value == 0)
                    {
                        Effects.Remove(sinkEffect);
                        m_sinkEffect = null;
                    }
                    else
                        sinkEffect.Value = value;
                }
                else if (value != 0)
                {
                    Effects.Add(m_sinkEffect = new EffectInteger(EffectsEnum.Effect_PowerSink, value));
                }

                Invalidate();
            }
        }

        public virtual int Weight
        {
            get { return (int) (Template.RealWeight*Stack); }
        }

        public double CurrentSubAreaBonus
        {
            get { return Template.FavoriteSubAreas.Contains((uint) Owner.SubArea.Id) ? Template.FavoriteSubAreasBonus : 0; }
        }

        public bool IsDeleted
        {
            get;
            private set;
        }

        #endregion

        public override string ToString()
        {
            return $"{Stack} x \"{Template.Name}\" ({Template.Id}) (Gid : {Guid})";
        }
    }
}