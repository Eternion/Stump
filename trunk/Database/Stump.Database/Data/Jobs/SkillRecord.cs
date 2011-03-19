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

namespace Stump.Database.Data.Jobs
{
    [Serializable]
    [ActiveRecord("skills")]
    [AttributeAssociatedFile("Skills")]
    public sealed class SkillRecord : DataBaseRecord<SkillRecord>
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

        [Property("ParentJobId")]
        public int ParentJobId
        {
            get;
            set;
        }

        [Property("IsForgemagus")]
        public bool IsForgemagus
        {
            get;
            set;
        }

        [Property("ModifiableItemType")]
        public int ModifiableItemType
        {
            get;
            set;
        }

        [Property("GatheredRessourceItem")]
        public int GatheredRessourceItem
        {
            get;
            set;
        }

        [Property("CraftableItemIds", ColumnType = "Serializable")]
        public List<int> CraftableItemIds
        {
            get;
            set;
        }

        [Property("InteractiveId")]
        public int InteractiveId
        {
            get;
            set;
        }

        [Property("UseAnimation")]
        public string UseAnimation
        {
            get;
            set;
        }

        [Property("IsRepair")]
        public bool IsRepair
        {
            get;
            set;
        }

        [Property("SkillCursor")]
        public int Cursor
        {
            get;
            set;
        }

        [Property("AvailableInHouse")]
        public int AvailableInHouse
        {
            get;
            set;
        }
    }
}