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

namespace Stump.Database.Data.Spells
{
    [Serializable]
    [ActiveRecord("spell_types")]
    [AttributeAssociatedFile("SpellTypes")]
    public sealed class SpellTypeRecord : DataBaseRecord<SpellTypeRecord>
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
        {
            get;
            set;
        }

        [Property("LongNameId")]
        public uint LongNameId
        {
            get;
            set;
        }

        [Property("ShortNameId")]
        public uint ShortNameId
        {
            get;
            set;
        }
    }
}