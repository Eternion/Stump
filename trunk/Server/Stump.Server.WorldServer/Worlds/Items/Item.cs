using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Worlds.Effects.Instances;

namespace Stump.Server.WorldServer.Worlds.Items
{
    public class Item
    {
        #region Fields

        internal readonly ItemRecord Record;

        #endregion

        #region Constructors

        internal Item(Item item, int stack)
            : this(item.Template, item.Guid, item.Position, stack, item.Effects)
        {
        }

        internal Item(ItemTemplate template, int guid)
            : this(template, guid, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
        {
        }

        internal Item(ItemTemplate template, int guid, CharacterInventoryPositionEnum position)
            : this(template, guid, position, 1)
        {
        }

        internal Item(ItemTemplate template, int guid, CharacterInventoryPositionEnum position, int stack)
            : this(template, guid, position, stack, new List<EffectBase>())
        {
        }

        internal Item(ItemTemplate template, int guid, CharacterInventoryPositionEnum position, int stack,
                    List<EffectBase> effects)
        {
            Template = template;
            Position = position;
            Stack = stack;
            Effects = effects;
            Record = new ItemRecord // create the associated record
            {
                Guid = guid,
                ItemId = template.Id,
                Stack = stack,
                Position = position,
                Effects = effects
            };
        }

        internal Item(ItemRecord record)
        {
            Record = record;

            Template = ItemManager.Instance.GetTemplate(Record.ItemId);
            Stack = Record.Stack;
            Position = Record.Position;
            Effects = new List<EffectBase>(Record.Effects);
        }

        internal Item(ItemTemplate template, CharacterInventoryPositionEnum position, int stack,
                      List<EffectBase> effects)
        {
            Template = template;

            Record = new ItemRecord // create the associated record
            {
                Guid = -1, // unassigned guid. ITEM CANNOT BE USED !
                ItemId = template.Id,
                Stack = stack,
                Position = position,
                Effects = effects
            };
        }

        #endregion

        #region Functions

        /// <summary>
        ///   Check if the given item can be stacked with the actual item (without compare his position)
        /// </summary>
        /// <param name = "compared"></param>
        /// <returns></returns>
        public bool IsStackableWith(Item compared)
        {
            return ( compared.ItemId == ItemId &&
                    compared.Position == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED &&
                    compared.Effects.CompareEnumerable(Effects) );
        }

        /// <summary>
        ///   Check if the given item must be stacked with the actual item
        /// </summary>
        /// <param name = "compared"></param>
        /// <returns></returns>
        public bool MustStackWith(Item compared)
        {
            return ( compared.ItemId == ItemId &&
                    compared.Position == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED &&
                    compared.Position == Position &&
                    compared.Effects.CompareEnumerable(Effects) );
        }

        public void StackItem(uint amount)
        {
            if (Position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
                return;

            Stack += (int)amount;
        }

        public void UnStackItem(uint amount)
        {
            if (Position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
                return;

            Stack -= (int)amount;
        }

        public bool IsEquiped()
        {
            return Position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED;
        }

        public ObjectItem GetObjectItem()
        {
            return new ObjectItem(
                (byte) Position,
                ItemId,
                0, // todo : power rate
                false, // todo : over max
                Effects.Select(entry => entry.GetObjectEffect()),
                Guid,
                Stack);
        }

        public override string ToString()
        {
            return string.Format("Item \"{0}\" <Id:{1}>", Enum.GetName(typeof(ItemIdEnum), ItemId), ItemId);
        }

        #endregion

        #region Properties

        public ItemTemplate Template
        {
            get;
            protected set;
        }

        public int Guid
        {
            get
            {
                return Record.Guid;
            }
            internal set
            {
                Record.Guid = value;
            }
        }

        public short ItemId
        {
            get
            {
                return (short) Template.Id;
            }
        }

        public int Stack
        {
            get { return Record.Stack; }
            set { Record.Stack = value; }
        }

        public CharacterInventoryPositionEnum Position
        {
            get
            {
                return Record.Position;
            }
            set
            {
                Record.Position = value;
            }
        }

        public List<EffectBase> Effects
        {
            get
            {
                return Record.Effects;
            }
            private set
            {
                Record.Effects = value;
            }
        }

        internal bool CanBeSave
        {
            get
            {
                return Guid != -1;
            }
        }

        #endregion

        #region Database Features

        public void Save()
        {
            WorldServer.Instance.IOTaskPool.EnqueueTask(SaveNow);
        }

        public void SaveNow()
        {
            if (!CanBeSave)
                return;

            Record.ItemId = ItemId;
            Record.Save();
        }

        internal void Create()
        {
            Record.Create();
        }

        public void Delete()
        {
            WorldServer.Instance.IOTaskPool.EnqueueTask(Record.Delete);
        }

        #endregion
    }

}