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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.BaseServer.Data;

namespace Stump.Tools.Proxy.Data
{
    public static class ZonesManager
    {
        private static readonly ConcurrentDictionary<uint, string> m_regionNameByMap = new ConcurrentDictionary<uint, string>();

        public static Dictionary<int, string> RegionsName
        {
            get;
            private set;
        }

        public static Dictionary<int, SubArea> Zones
        {
            get;
            private set;
        }


        public static Dictionary<int, Area> Regions
        {
            get;
            private set;
        }

        public static Dictionary<int, MapPosition> Maps
        {
            get;
            private set;
        }

        public static void Initialize()
        {
            Zones = DataLoader.LoadDataByIdAsDictionary<int, SubArea>(entry => entry.id);
            Regions = DataLoader.LoadDataByIdAsDictionary<int, Area>(entry => entry.id);
            Maps = DataLoader.LoadDataByIdAsDictionary<int, MapPosition>(entry => entry.id);

            RegionsName = new Dictionary<int, string>();

            foreach (Area area in Regions.Values)
            {
                RegionsName.Add(area.id, DataLoader.GetI18NText((int)area.nameId));
            }
        }

        public static string GetRegionNameByMap(uint mapId)
        {
            if (m_regionNameByMap.ContainsKey(mapId))
                return m_regionNameByMap[mapId];

            Area region;
            try
            {
                region = Regions[Zones[Maps[(int) mapId].subAreaId].areaId];
            }
            catch
            {
                region = null;
            }

            if (region == null)
                return "Unknown Zone";

            string name = RegionsName[region.id];

            m_regionNameByMap.TryAdd(mapId, name);

            return name;
        }
    }
}