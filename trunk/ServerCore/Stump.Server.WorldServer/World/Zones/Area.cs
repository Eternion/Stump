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
using Stump.Server.BaseServer.Network;
using Stump.Server.DataProvider.Data.Areas;
using Stump.Server.WorldServer.World.Entities.Characters;

namespace Stump.Server.WorldServer.World.Zones
{
    public class Area
    {

        public Area(AreaTemplate template, SuperArea superArea)
        {
            Template = template;
            SuperArea = superArea;
        }

        public readonly AreaTemplate Template;

        public readonly SuperArea SuperArea;

        public int Id
        {
            get { return Template.Id; }
        }

        public string Name
        {
            get { return Template.Name; }
            set { Template.Name = value; }
        }

        public Dictionary<int, SubArea> SubAreas
        {
            get;
            set;
        }

        private ConcurrentList<Character> m_characters = new ConcurrentList<Character>();
        public ConcurrentList<Character> Characters
        { get { return m_characters; } }

        public override string ToString()
        {
            return string.Format("Area {0} (Id: {1})",Name, Id);
        }

    }
}