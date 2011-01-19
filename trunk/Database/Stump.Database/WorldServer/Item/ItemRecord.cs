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
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;

namespace Stump.Database.WorldServer
{
    [AttributeDatabase(DatabaseService.WorldServer)]
    [ActiveRecord("items"), JoinedBase]
    public class ItemRecord : ActiveRecordBase<ItemRecord>
    {
        private IList<byte[]> m_effects;

        [PrimaryKey(PrimaryKeyType.Native, "Guid")]
        public long Guid
        {
            get;
            set;
        }

        [Property("ItemId", NotNull = true)]
        public int ItemId
        {
            get;
            set;
        }

        [Property("Stack", NotNull=true, Default="0")]
        public uint Stack
        {
            get;
            set;
        }

        [Property("Position", NotNull=true, Default="63")]
        public CharacterInventoryPositionEnum Position
        {
            get;
            set;
        }

        [HasMany(typeof(ItemEffectRecord), Cascade= ManyRelationCascadeEnum.Delete)]
        public IList<byte[]> Effects
        {
            get { return m_effects ?? new List<byte[]>(); }
            set { m_effects = value; }
        }
    }
}