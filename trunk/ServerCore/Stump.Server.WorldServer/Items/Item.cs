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
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Effects;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global;

namespace Stump.Server.WorldServer.Items
{
    public class Item : IOwned
    {
        #region Fields

        private readonly CharacterItemRecord m_record;

        private List<EffectBase> m_effects;
        private CharacterInventoryPositionEnum m_position;
        private uint m_stack;
        protected ItemTemplate m_template;

        #endregion

        #region Constructors

        public Item(Entity owner, Item item)
            : this(owner, item, item.Stack)
        {
        }

        public Item(Entity owner, Item item, uint stack)
            : this(owner, item.Template, item.Guid, item.Position, stack, item.Effects)
        {
        }

        public Item(Entity owner, ItemTemplate template, long guid)
            : this(owner, template, guid, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
        {
        }

        public Item(Entity owner, ItemTemplate template, long guid, CharacterInventoryPositionEnum position)
            : this(owner, template, guid, position, 1)
        {
        }

        public Item(Entity owner, ItemTemplate template, long guid, CharacterInventoryPositionEnum position, uint stack)
            : this(owner, template, guid, position, stack, new List<EffectBase>())
        {
        }

        public Item(Entity owner, ItemTemplate template, long guid, CharacterInventoryPositionEnum position, uint stack,
                    List<EffectBase> effects)
        {
            Owner = owner;
            m_template = template;
            Guid = guid;
            m_position = position;
            m_stack = stack;
            m_effects = effects;
            m_record = new CharacterItemRecord // create the associated record
                {
                    Guid = guid,
                    OwnerId = owner.Id,
                    ItemId = template.Id,
                    Stack = stack,
                    Position = position,
                    Effects = GetEffectsSerialized()
                };
        }

        public Item(Entity owner, CharacterItemRecord record)
        {
            m_record = record;

            if (m_record.OwnerId != owner.Id)
                throw new Exception(
                    string.Format(
                        "This item <guid:{0}> don't own to the given Entity <id:{1}> but to this Entity <id:{2}>",
                        m_record.Guid, owner.Id, m_record.OwnerId));

            Owner = owner;
            Guid = m_record.Guid;
            m_template = ItemManager.GetTemplate(m_record.ItemId);
            m_stack = m_record.Stack;
            m_position = m_record.Position;
            m_effects = GetEffectsUnSerialized(m_record.Effects);
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
            return (compared.Owner == Owner &&
                    compared.ItemId == ItemId &&
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
            return (compared.Owner == Owner &&
                    compared.ItemId == ItemId &&
                    compared.Position == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED &&
                    compared.Position == Position &&
                    compared.Effects.CompareEnumerable(Effects));
        }

        public void StackItem(uint amount)
        {
            if (m_position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
                return;

            m_stack += amount;
        }

        public void UnStackItem(uint amount)
        {
            if (m_position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
                return;

            m_stack -= amount;
        }

        public ObjectItem ToNetworkItem()
        {
            return new ObjectItem(
                (uint) Position,
                (uint) ItemId,
                0, // todo : power rate
                false, // todo : over max
                m_effects.Select(entry => entry.ToNetworkEffect()).ToList(),
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
            get { return m_template; }
        }

        public long Guid
        {
            get;
            private set;
        }

        public int ItemId
        {
            get { return m_template.Id; }
        }

        public uint Stack
        {
            get { return m_stack; }
            set { m_stack = value; }
        }

        public CharacterInventoryPositionEnum Position
        {
            get { return m_position; }
            set { m_position = value; }
        }

        public List<EffectBase> Effects
        {
            get { return m_effects; }
            set { m_effects = value; }
        }

        public Entity Owner
        {
            get;
            set;
        }

        #endregion

        #region Database Features

        public void Save()
        {
            World.Instance.TaskPool.EnqueueTask(SaveNow);
        }

        internal void SaveNow()
        {
            m_record.Guid = Guid;
            m_record.ItemId = m_template.Id;
            m_record.Position = m_position;
            m_record.Stack = m_stack;
            m_record.OwnerId = Owner.Id;
            m_record.Effects = GetEffectsSerialized();

            m_record.Save();
        }

        public void Create()
        {
            World.Instance.TaskPool.EnqueueTask(m_record.Create);
        }

        public void Delete()
        {
            World.Instance.TaskPool.EnqueueTask(m_record.Delete);
        }

        internal List<byte[]> GetEffectsSerialized()
        {
            return m_effects.Select(EffectBase.Serialize).ToList();
        }

        internal List<EffectBase> GetEffectsUnSerialized(List<byte[]> buffers)
        {
            return buffers.Select(EffectBase.DeSerialize).ToList();
        }

        #endregion
    }
}