// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.BaseCore.Framework.Extensions;
using Stump.Database;
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Effects;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global;

namespace Stump.Server.WorldServer.Items
{
    public class Item
    {
        #region Fields

        private readonly ItemRecord m_record;

        #endregion

        #region Constructors

        public Item(Item item, uint stack)
            : this(item.Template, item.Guid, item.Position, stack, item.Effects)
        {
        }

        public Item(ItemTemplate template, long guid)
            : this(template, guid, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
        {
        }

        public Item(ItemTemplate template, long guid, CharacterInventoryPositionEnum position)
            : this(template, guid, position, 1)
        {
        }

        public Item(ItemTemplate template, long guid, CharacterInventoryPositionEnum position, uint stack)
            : this(template, guid, position, stack, new List<EffectBase>())
        {
        }

        public Item(ItemTemplate template, long guid, CharacterInventoryPositionEnum position, uint stack,
                    List<EffectBase> effects)
        {
            Template = template;
            Position = position;
            Stack = stack;
            Effects = effects;
            m_record = new ItemRecord // create the associated record
                {
                    Guid = guid,
                    ItemId = template.Id,
                    Stack = stack,
                    Position = position,
                    Effects = GetEffectsSerialized()
                };
        }

        public Item(ItemRecord record)
        {
            m_record = record;
          
            Template = ItemManager.GetTemplate(m_record.ItemId);
            Stack = m_record.Stack;
            Position = m_record.Position;
            Effects = GetEffectsUnSerialized(m_record.Effects);
        }

        internal Item(ItemTemplate template, CharacterInventoryPositionEnum position, uint stack,
                      List<EffectBase> effects)
        {
            Template = template;
            Position = position;
            Stack = stack;
            Effects = effects;

            m_record = new ItemRecord // create the associated record
                       {
                           Guid = -1, // unassigned guid. ITEM CANNOT BE USED !
                           ItemId = template.Id,
                           Stack = stack,
                           Position = position,
                           Effects = GetEffectsSerialized()
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
            return (compared.ItemId == ItemId &&
                    compared.Position == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED &&
                    compared.Effects.CompareEnumerable(Effects));
        }

        /// <summary>
        ///   Check if the given item must be stacked with the actual item
        /// </summary>
        /// <param name = "compared"></param>
        /// <returns></returns>
        public bool MustStackWith(Item compared)
        {
            return (compared.ItemId == ItemId &&
                    compared.Position == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED &&
                    compared.Position == Position &&
                    compared.Effects.CompareEnumerable(Effects));
        }

        public void StackItem(uint amount)
        {
            if (Position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
                return;

            Stack += amount;
        }

        public void UnStackItem(uint amount)
        {
            if (Position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
                return;

            Stack -= amount;
        }

        public ObjectItem ToNetworkItem()
        {
            return new ObjectItem(
                (uint) Position,
                (uint) ItemId,
                0, // todo : power rate
                false, // todo : over max
                Effects.Select(entry => entry.ToNetworkEffect()).ToList(),
                (uint) Guid,
                Stack);
        }

        public override string ToString()
        {
            return string.Format("Item \"{0}\" <Id:{1}>", Enum.GetName(typeof (ItemIdEnum), ItemId), ItemId);
        }

        #endregion

        #region Properties

        public ItemTemplate Template
        {
            get;
            protected set;
        }

        public long Guid
        {
            get
            {
                return m_record.Guid;
            }
            internal set
            {
                m_record.Guid = value;
            }
        }

        public int ItemId
        {
            get { return Template.Id; }
        }

        public uint Stack
        {
            get;
            set;
        }

        public CharacterInventoryPositionEnum Position
        {
            get;
            set;
        }

        public List<EffectBase> Effects
        {
            get;
            set;
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
            World.Instance.TaskPool.EnqueueTask(SaveNow);
        }

        internal void SaveNow()
        {
            if (CanBeSave)
            {
                m_record.ItemId = Template.Id;
                m_record.Position = Position;
                m_record.Stack = Stack;
                m_record.Effects = GetEffectsSerialized();

                m_record.Save();
            }
        }

        internal void Create()
        {
            m_record.Create();
        }

        public void Delete()
        {
            World.Instance.TaskPool.EnqueueTask(m_record.Delete);
        }

        internal List<byte[]> GetEffectsSerialized()
        {
            return Effects.Select(EffectBase.Serialize).ToList();
        }

        internal List<EffectBase> GetEffectsUnSerialized(IList<byte[]> buffers)
        {
            return buffers.Select(EffectBase.DeSerialize).ToList();
        }

        #endregion
    }
}