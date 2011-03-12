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
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses;

namespace Stump.Database.D2O
{
    [Serializable]
    [ActiveRecord("item_types")]
    public sealed class ItemTypeRecord : D2OBaseRecord<ItemTypeRecord>
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
        {
            get;
            set;
        }

        [Property("NameId")]
        public uint NameId
        {
            get;
            set;
        }

        [Property("SuperTypeId")]
        public uint SuperTypeId
        {
            get;
            set;
        }

        [Property("Plural")]
        public bool Plural
        {
            get;
            set;
        }

        [Property("Gender")]
        public uint Gender
        {
            get;
            set;
        }

        [Property("ZoneSize")]
        public uint ZoneSize
        {
            get;
            set;
        }

        [Property("ZoneShape")]
        public uint ZoneShape
        {
            get;
            set;
        }

        [Property("NeedUseConfirm")]
        public bool NeedUseConfirm
        {
            get;
            set;
        }
    }
}