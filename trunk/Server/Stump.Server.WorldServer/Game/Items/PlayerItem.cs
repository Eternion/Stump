using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Cache;
using Stump.Core.Collections;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Server.WorldServer.Game.Items
{
    public class PlayerItem : Item<PlayerItemRecord>
    {
        #region Fields

        public Character Owner
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        public PlayerItem(Character owner, PlayerItemRecord record)
            : base(record)
        {
            m_objectItemValidator = new ObjectValidator<ObjectItem>(BuildObjectItem);

            Owner = owner;
            Position = Record.Position;
        }

        public PlayerItem(Character owner, int guid, ItemTemplate template, CharacterInventoryPositionEnum position,
                          int stack,
                          List<EffectBase> effects)
        {
            m_objectItemValidator = new ObjectValidator<ObjectItem>(BuildObjectItem);
            Owner = owner;

            Record = new PlayerItemRecord // create the associated record
                         {
                             Id = guid,
                             OwnerId = owner.Id,
                             Template = template,
                             Stack = stack,
                             Position = position,
                             Effects = effects,
                             New = true
                         };
        }

        #endregion

        #region Functions

        public void ChangeOwner(Character newOwner)
        {
            Owner = newOwner;
            Record.OwnerId = newOwner.Id;
        }

        public bool AreConditionFilled(Character character)
        {
            try
            {
                if (Template.CriteriaExpression == null)
                    return true;

                return Template.CriteriaExpression.Eval(character);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///   Check if the given item can be stacked with the actual item (without comparing his position)
        /// </summary>
        /// <param name = "compared"></param>
        /// <returns></returns>
        public bool IsStackableWith(PlayerItem compared)
        {
            return (compared.Template.Id == Template.Id &&
                    compared.Position == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED &&
                    compared.Effects.CompareEnumerable(Effects));
        }

        /// <summary>
        ///   Check if the given item must be stacked with the actual item
        /// </summary>
        /// <param name = "compared"></param>
        /// <returns></returns>
        public bool MustStackWith(PlayerItem compared)
        {
            return (compared.Template.Id == Template.Id &&
                    compared.Position == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED &&
                    compared.Position == Position &&
                    compared.Effects.CompareEnumerable(Effects));
        }

        public bool IsLinked()
        {
            if (Template.IsLinkedToOwner)
                return true;

            if (Template.Type.SuperType == ItemSuperTypeEnum.SUPERTYPE_QUEST)
                return true;

            if (IsTokenItem())
                return true;

            return false;
        }

        public bool IsTokenItem()
        {
            return Inventory.ActiveTokens && Template.Id == Inventory.TokenTemplateId;
        }

        public bool IsUsable()
        {
            return Template.Usable;
        }

        public bool IsEquiped()
        {
            return Position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED;
        }

        #region ObjectItem

        private readonly ObjectValidator<ObjectItem> m_objectItemValidator;

        private ObjectItem BuildObjectItem()
        {
            return new ObjectItem(
                (byte) Position,
                (short) Template.Id,
                0, // todo : power rate
                false, // todo : over max
                Effects.Where(entry => !entry.Hidden).Select(entry => entry.GetObjectEffect()),
                Guid,
                Stack);
        }

        public ObjectItem GetObjectItem()
        {
            return m_objectItemValidator;
        }

        /// <summary>
        /// Call it each time you modify part of the item
        /// </summary>
        public void Invalidate()
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

        public override int Stack
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

        public CharacterInventoryPositionEnum Position
        {
            get { return Record.Position; }
            set
            {
                Record.Position = value;
                Invalidate();
            }
        }

        public int Weight
        {
            get { return (int) (Template.RealWeight*Stack); }
        }

        #endregion
    }
}