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
using Castle.ActiveRecord;
using Stump.Database.Types;

namespace Stump.Database.Data.Misc
{
    [Serializable]
    [ActiveRecord("censored_contents")]
    [AttributeAssociatedFile("CensoredContents")]
    public sealed class CensoredContentRecord : DataBaseRecord<CensoredContentRecord>
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [Property("Lang")]
        public string Lang
        {
            get;
            set;
        }

        [Property("Type")]
        public int Type
        {
            get;
            set;
        }

        [Property("OldValue")]
        public int OldValue
        {
            get;
            set;
        }

        [Property("NewValue")]
        public int NewValue
        {
            get;
            set;
        }


    }
}