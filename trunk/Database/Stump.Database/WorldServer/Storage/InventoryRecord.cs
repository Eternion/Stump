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

namespace Stump.Database.WorldServer
{

    [AttributeDatabase(DatabaseService.WorldServer)]
    [ActiveRecord("inventories")]
    public sealed class InventoryRecord : ActiveRecordBase<InventoryRecord>
    {
        private IList<ItemRecord> m_items;

        [PrimaryKey(PrimaryKeyType.Identity, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [Property("Kamas", NotNull = true, Default = "0")]
        public long Kamas
        {
            get;
            set;
        }

        [HasMany(typeof(ItemRecord), Table = "inventories_items", ColumnKey = "InventoryId", Cascade=  ManyRelationCascadeEnum.Delete)]
        public IList<ItemRecord> Items
        {
            get { return m_items ?? new List<ItemRecord>(); }
            set { m_items = value; }
        }

    }
}