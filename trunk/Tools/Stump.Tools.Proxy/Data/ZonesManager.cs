
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Classes.world;
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