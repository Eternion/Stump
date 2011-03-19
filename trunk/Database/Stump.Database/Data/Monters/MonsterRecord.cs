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

namespace Stump.Database.Data.Monters
{
    [Serializable]
    [ActiveRecord("monsters")]
    [AttributeAssociatedFile("Monsters")]
    public sealed class MonsterRecord : DataBaseRecord<MonsterRecord>
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

        [Property("GfxId")]
        public uint GfxId
        {
            get;
            set;
        }

        [Property("Race")]
        public int Race
        {
            get;
            set;
        }

        [Property("Grades",ColumnType="Serializable")]
        public List<MonsterGrade> Grades
        {
            get;
            set;
        }

        [Property("Look")]
        public string Look
        {
            get;
            set;
        }

        [Property("UseSummonSlot")]
        public bool UseSummonSlot
        {
            get;
            set;
        }

        [Property("UseBombSlot")]
        public bool UseBombSlot
        {
            get;
            set;
        }

        [Property("CanPlay")]
        public bool CanPlay
        {
            get;
            set;
        }
    }
}