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
using System.Collections.Generic;
using Stump.BaseCore.Framework.Collections;
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Enums;
using Stump.Server.DataProvider.Data.SubAreas;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.World.Entities.Characters;

namespace Stump.Server.WorldServer.World.Zones
{
    public class SubArea
    {

        public SubArea(SubAreaTemplate template, SubAreaRecord record, Area area)
        {
            Template = template;
            Record = record;
            Area = area;
        }

        public readonly SubAreaTemplate Template;

        public readonly SubAreaRecord Record;

        public int Id
        {
            get { return Template.Id; }
        }

        public string Name
        {
            get { return Template.Name; }
            set { Template.Name = value; }
        }

        public readonly Area Area;

        public Dictionary<int, Map> Maps
        {
            get;
            set;
        }

        public AlignmentSideEnum AlignmentSide
        {
            get { return (AlignmentSideEnum)Record.AlignmentSide; }
            set { Record.AlignmentSide = (int)value; }
        }

        private ConcurrentList<Character> m_characters = new ConcurrentList<Character>();
        public ConcurrentList<Character> Characters
        { get { return m_characters; } }


        public void Save()
        {
                Record.AlignmentSide = (int)AlignmentSide;
                Record.Prism = Prism.Record;
                Record.SaveAndFlush();
        }

        public override string ToString()
        {
            return string.Format("SubArea {0} (Id: {1})", Name, Id);
        }

    }
}