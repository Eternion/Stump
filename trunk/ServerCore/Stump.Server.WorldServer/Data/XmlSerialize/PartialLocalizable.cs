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
using System.Xml.Serialization;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.XmlSerialize
{
    public abstract class PartialLocalizable
    {
        private Map m_map;

        protected PartialLocalizable()
        {
        }

        protected PartialLocalizable(uint mapId)
        {
            MapId = mapId;
        }

        public uint MapId
        {
            get;
            set;
        }

        [XmlIgnore]
        public Map Map
        {
            get
            {
                if (m_map == null)
                    LoadMapFromWorld();

                return m_map;
            }
            private set { m_map = value; }
        }

        public void LoadMapFromWorld()
        {
            Map = World.Instance.GetMap(MapId);

            if (Map == null)
                throw new NullReferenceException(string.Format("Map {0} doesn't exist", MapId));
        }
    }
}