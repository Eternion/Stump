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
using System.Linq;
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.BaseServer.Data;

namespace Stump.Tools.Proxy.Data
{
    public static class ZonesManager
    {
        private static readonly Dictionary<uint, string> m_zoneNameByMap = new Dictionary<uint, string>();

        public static Dictionary<int, string> ZonesName
        {
            get;
            private set;
        }

        public static IEnumerable<SubArea> Zones
        {
            get;
            private set;
        }

        public static void Initialize()
        {
            Zones = DataLoader.LoadData<SubArea>();
            ZonesName = new Dictionary<int, string>();

            foreach (SubArea subArea in Zones)
            {
                ZonesName.Add(subArea.id, DataLoader.GetI18NText((int) subArea.nameId));
            }
        }

        public static string GetZoneNameByMap(uint mapId)
        {
            if (m_zoneNameByMap.ContainsKey(mapId))
                return m_zoneNameByMap[mapId];

            SubArea zone = Zones.Where(entry => entry.mapIds.Contains(mapId)).FirstOrDefault();

            if (zone == null)
                return "Unknown Zone";

            string name = ZonesName[zone.id];

            m_zoneNameByMap.Add(mapId, name);

            return name;
        }
    }
}