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
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Data;
using Stump.Server.WorldServer.Data;
using Stump.Server.WorldServer.Global.Maps;
using AreaTemplate = Stump.DofusProtocol.D2oClasses.Area;
using SubAreaTemplate = Stump.DofusProtocol.D2oClasses.SubArea;
using SuperAreaTemplate = Stump.DofusProtocol.D2oClasses.SuperArea;

namespace Stump.Server.WorldServer.Global
{
    public partial class World
    {
        public void BuildWorld()
        {
            logger.Info("Loading Continents...");
            LoadContinents();

            logger.Info("Loading Regions...");
            LoadRegions();

            logger.Info("Loading Zones...");
            LoadZones();

            logger.Info("Loading Maps...");
            LoadMaps();
        }

        #region Maps/Zones/Region/Continent

        public void LoadMaps()
        {
            try
            {
                foreach (Map map in MapLoader.LoadMaps())
                {
                    map.ParentSpace = GetZone((int) map.ZoneId);
                    map.ParentSpace.Childrens.Add(map);

                    Maps.Add(map.Id, map);
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Error on parsing map :" + ex);
            }

            foreach (Zone zone in Zones.Values)
            {
                zone.Maps = GetMaps(zone.MapsId);
                zone.CustomWorldMaps = GetMaps(zone.CustomWorldMapsId);
            }
        }


        public void LoadContinents()
        {
            var parentareaslist = new List<SuperAreaTemplate>();
            DataLoader.LoadData(ref parentareaslist);

            foreach (SuperAreaTemplate parentarea in parentareaslist)
            {
                var continent = new Continent
                    {
                        Id = parentarea.id,
                        Name = Enum.GetName(typeof (ContinentIdEnum), parentarea.id),
                        ParentSpace = null
                    };

                // We could consider World as top of top world space but it's doesn't make sense at some levels.
                Continents.Add(continent.Id, continent);
            }
        }

        public void LoadRegions()
        {
            foreach (AreaTemplate areatemplate in DataLoader.LoadDataById<AreaTemplate>(entry => entry.id))
            {
                if (areatemplate != null)
                {
                    var region = new Region
                        {
                            Id = areatemplate.id,
                            ParentSpace = Continents[areatemplate.superAreaId]
                        };

                    region.ParentSpace.Childrens.Add(region);
                    region.Name = Enum.GetName(typeof (RegionIdEnum), region.Id);
                    Regions.Add(region.Id, region);
                }
            }
        }

        public void LoadZones()
        {
            foreach (SubAreaTemplate subArea in DataLoader.LoadDataById<SubAreaTemplate>(entry => entry.id))
            {
                if (subArea != null)
                {
                    var zone = new Zone
                        {
                            Id = subArea.id,
                            Name = Enum.GetName(typeof (ZoneIdEnum), subArea.id),
                            MapsId = subArea.mapIds.ToArray(),
                            CustomWorldMapsId = subArea.mapIds.ToArray(),
                            ParentSpace = Regions[subArea.areaId]
                        };

                    zone.ParentSpace.Childrens.Add(zone);
                    Zones.Add(zone.Id, zone);
                }
            }
        }

        #endregion
    }
}