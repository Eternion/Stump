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
using NHibernate.Criterion;

namespace Stump.Database
{

    [AttributeDatabase(DatabaseService.WorldServer)]
    [ActiveRecord("accounts_house"), JoinedBase]
    public class HouseRecord : ActiveRecordBase<HouseRecord>
    {

        [PrimaryKey(PrimaryKeyType.Assigned, "HouseId")]
        public uint HouseId
        {
            get;
            set;
        }
        
        [BelongsTo("AccountId", NotNull = true)]
        public WorldAccountRecord Account
        {
            get;
            set;
        }

        [Property("Price", NotNull = false)]
        public uint Price
        {
            get;
            set;
        }

        [Property("Password", NotNull = false)]
        public uint Password
        {
            get;
            set;
        }


    }
}