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
using Castle.ActiveRecord;

namespace Stump.Database.WorldServer
{
    [AttributeDatabase(DatabaseService.WorldServer)]
    [ActiveRecord("bidhouses_solditems")]
    public sealed class BidHouseSoldItemRecord : ActiveRecordBase<BidHouseSoldItemRecord>
    {

        [PrimaryKey(PrimaryKeyType.Identity, "Id")]
        public long Id
        {
            get;
            set;
        }

        [BelongsTo("BidHouseId", NotNull = true)]
        public BidHouseRecord BidHouse
        {
            get;
            set;
        }

        [Property("ItemId", NotNull=true)]
        public uint ItemId
        {
            get;
            set;
        }

        [Property("SoldPrice", NotNull=true)]
        public uint SoldPrice
        {
            get;
            set;
        }

    }
}