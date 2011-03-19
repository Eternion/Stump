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

namespace Stump.Database.Data.Spells
{
    [Serializable]
    [ActiveRecord("spells")]
    [AttributeAssociatedFile("Spells")]
    public sealed class SpellRecord : DataBaseRecord<SpellRecord>
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

        [Property("DescriptionId")]
        public uint DescriptionId
        {
            get;
            set;
        }

        [Property("TypeId")]
        public uint TypeId
        {
            get;
            set;
        }

        [Property("ScriptParams")]
        public string ScriptParams
        {
            get;
            set;
        }

        [Property("ScriptParamsCritical")]
        public string ScriptParamsCritical
        {
            get;
            set;
        }

        [Property("ScripId")]
        public int ScriptId
        {
            get;
            set;
        }

        [Property("ScriptIdCritical")]
        public int ScriptIdCritical
        {
            get;
            set;
        }

        [Property("IconId")]
        public uint IconId
        {
            get;
            set;
        }

        [Property("SpellLevel", ColumnType = "Serializable")]
        public List<uint> SpellLevel
        {
            get;
            set;
        }

        [Property("UseParamCache")]
        public bool UseParamCache
        {
            get;
            set;
        }
    }
}