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

namespace Stump.Database.Data.Quests
{
    [Serializable]
    [ActiveRecord("quest_objectives"),JoinedBase]
    [AttributeAssociatedFile("QuestObjectives")]
    public class QuestObjectiveRecord : DataBaseRecord<QuestObjectiveRecord>
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [Property("StepId")]
        public uint StepId
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

        [Property("Parameters", ColumnType = "Serializable")]
        public List<uint> Parameters
        {
            get;
            set;
        }

        [Property("Coords", ColumnType = "Serializable")]
        public Point Coords
        {
            get;
            set;
        }
    }
}