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

namespace Stump.Database.D2O
{
    [Serializable]
    [ActiveRecord("emoticons")]
    public sealed class EmoticonRecord : D2OBaseRecord<EmoticonRecord>
    {

        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public uint Id
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

        [Property("ShortcutId")]
        public uint ShortcutId
        {
            get;
            set;
        }

        [Property("DefaultAnim")]
        public string DefaultAnim
        {
            get;
            set;
        }

        [Property("Instant")]
        public bool Instant
        {
            get;
            set;
        }

        [Property("EightDirections")]
        public bool EightDirections
        {
            get;
            set;
        }

        [Property("Aura")]
        public bool Aura
        {
            get;
            set;
        }

        [Property("Anims",ColumnType="Serializable")]
        public List<string> Anims
        {
            get;
            set;
        }

        [Property("Cooldown")]
        public uint Cooldown
        {
            get;
            set;
        }
    }
}