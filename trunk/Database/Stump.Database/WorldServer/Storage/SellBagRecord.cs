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
using Stump.Database.Types;

namespace Stump.Database.WorldServer
{
    [ActiveRecord("sellbags")]
    public sealed class SellBagRecord : WorldRecord<SellBagRecord>
    {
        private IList<PricedItemRecord> m_items;

        [PrimaryKey(PrimaryKeyType.Foreign, "CharacterId")]
        public uint CharacterId { get; set; }

        [OneToOne]
        public CharacterRecord Character { get; set; }

        [Property("Capacity")]
        public uint Capacity { get; set; }

        [HasMany(typeof (PricedItemRecord), Table = "sellbags_items", ColumnKey = "CharacterId")]
        public IList<PricedItemRecord> PricedItems
        {
            get { return m_items ?? new List<PricedItemRecord>(); }
            set { m_items = value; }
        }
    }
}