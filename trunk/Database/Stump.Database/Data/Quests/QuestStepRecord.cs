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
    [ActiveRecord("quest_steps")]
    [AttributeAssociatedFile("QuestSteps")]
    public sealed class QuestStepRecord : DataBaseRecord<QuestStepRecord>
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [Property("QuestId")]
        public uint QuestId
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

        [Property("DialogId")]
        public int DialogId
        {
            get;
            set;
        }

        [Property("OptimalLevel")]
        public uint OptimalLevel
        {
            get;
            set;
        }

        [Property("ExperienceReward")]
        public uint ExperienceReward
        {
            get;
            set;
        }

        [Property("KamasReward")]
        public uint KamasReward
        {
            get;
            set;
        }

        [Property("ItemsReward", ColumnType = "Serializable")]
        public List<List<uint>> ItemsReward
        {
            get;
            set;
        }

        [Property("EmotesReward", ColumnType = "Serializable")]
        public List<uint> EmotesReward
        {
            get;
            set;
        }

        [Property("JobsReward", ColumnType = "Serializable")]
        public List<uint> JobsReward
        {
            get;
            set;
        }

        [Property("SpellsReward", ColumnType = "Serializable")]
        public List<uint> SpellsReward
        {
            get;
            set;
        }

        [Property("ObjectiveIds", ColumnType = "Serializable")]
        public List<uint> ObjectiveIds
        {
            get;
            set;
        }
    }
}