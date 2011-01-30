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
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global;

namespace Stump.Server.WorldServer.World.Zones
{
    public class Area
    {

        public Area(int id, string name, SuperArea superArea)
        {
            Id = id;
            Name = name;
            SuperArea = superArea;
        }

        public readonly int Id;

        public readonly string Name;

        public readonly SuperArea SuperArea;

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