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
using System.Threading;
using System.Threading.Tasks;
using Stump.BaseCore.Framework.IO;
using Stump.BaseCore.Framework.Utils;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Data;
using Stump.Server.WorldServer.Data;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.XmlSerialize;
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

            logger.Info("Loading Cell Triggers...");
            LoadTriggers();

            logger.Info("Spawn Interactive Objects...");
            SpawnInteractiveObjects();

            logger.Info("Spawn Npcs...");
            SpawnNpcs();

            logger.Info("Setup WorldElement Lazy Loading");
            SetupLazyLoading();
        }

        public void LoadMaps()
        {
            try
            {
                int count = MapLoader.GetMapFilesCount();

                var consoleProcent = new ConsoleProcent();
                IDictionary<int, MapPosition> mapsPositions = MapLoader.LoadMapPositions();
                Parallel.ForEach(MapLoader.LoadMaps(), map =>
                {
                    map.ParentSpace = GetZone((int) map.ZoneId);

                    if (mapsPositions.ContainsKey(map.Id))
                        map.SetMapPosition(mapsPositions[map.Id]);

                    var continent = (Continent)map.ParentSpace.ParentSpace.ParentSpace;

                    retry:
                    try
                    {
                        map.ParentSpace.Childrens.Add(map);
                    }
                    catch
                    {
                        // ugly way ...
                        goto retry;
                    }

                    while (!Maps.TryAdd(map.Id, map))
                    {
                        // if cannot add the map we change the current thread
                        Thread.Sleep(1);
                    }

                    if (map.Outdoor && !continent.MapsByPosition.ContainsKey(map.Position))
                        while (!continent.MapsByPosition.TryAdd(map.Position, map))
                        {
                            // if cannot add the element we change the current thread
                            Thread.Sleep(1);
                        }

                    consoleProcent.Update((int) (((double) Maps.Count/count)*100));
                });

                consoleProcent.End();
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

        public void LoadTriggers()
        {
            foreach (CellTrigger trigger in MapLoader.LoadTriggers())
            {
                try
                {
                    trigger.Map.AddTrigger(trigger.Cell, trigger);
                }
                catch (Exception e)
                {
                    logger.Error("Cannot add trigger on cell <id:{0}> : {1}", trigger.Cell.Id, e.Message);
                }
            }
        }

        public void LoadContinents()
        {
            IEnumerable<SuperAreaTemplate> parentareaslist = DataLoader.LoadData<SuperAreaTemplate>();

            foreach (SuperAreaTemplate parentarea in parentareaslist)
            {
                var continent = new Continent
                    {
                        Id = parentarea.id,
                        Name = DataLoader.GetI18NText((int) parentarea.nameId),
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

        public void SpawnInteractiveObjects()
        {
            foreach (InteractiveElementSerialized interactiveObject in InteractiveObjectLoader.LoadsInteractiveObjects()
                )
            {
                try
                {
                    interactiveObject.Map.SpawnInteractiveObject(interactiveObject.InteractiveElement);
                }
                catch (Exception e)
                {
                    logger.Error("Cannot spawn object <id:{0}> : {1}", interactiveObject.InteractiveElement.elementId,
                                 e.Message);
                }
            }

            foreach (SkillInstanceSerialized skill in InteractiveObjectLoader.LoadSkills())
            {
                try
                {
                    InteractiveObject interactiveObject = skill.Map.GetInteractiveObject(skill.ElementId);

                    interactiveObject.Skills.Add(skill.SkillInstance.Id, skill.SkillInstance.Skill);
                }
                catch (Exception e)
                {
                    logger.Error("Cannot assign skill <id:{0}> : {1}", skill.SkillInstance.Id, e.Message);
                }
            }
        }

        public void SpawnNpcs()
        {
            foreach (NpcSerialized npcSpawnInfo in NpcLoader.LoadSpawnData())
            {
                try
                {
                    npcSpawnInfo.Map.SpawnNpc(npcSpawnInfo.NpcInformations);
                }
                catch (Exception e)
                {
                    logger.Error("Cannot spawn Npc <id:{0}> : {1}", npcSpawnInfo.NpcInformations.npcId, e.Message);
                }
            }
        }

        public void SetupLazyLoading()
        {
            const string text = "{0} was added in the {1}Folder, load it ?";

            /* Npcs */
            FileWatcher.RegisterFileCreation(NpcLoader.NpcsDir,f=>
                {
                    if (WorldServer.Instance.ConsoleInterface.AskForSomething(string.Format(text,f,"Npc"), 20))
                    {
                        //TODO
                    }
                });

            /* InteractiveObject */
            FileWatcher.RegisterFileCreation(InteractiveObjectLoader.InteractiveObjectsDir, f =>
            {
                if (WorldServer.Instance.ConsoleInterface.AskForSomething(string.Format(text, f, "InteractiveObject"), 20))
                {
                    //TODO
                }
            });

            /* Trigger */
            FileWatcher.RegisterFileCreation(MapLoader.CellTriggersDir, f =>
            {
                if (WorldServer.Instance.ConsoleInterface.AskForSomething(string.Format(text, f, "Trigger"), 20))
                {
                    //TODO
                }
            });

            //TODO
        }
    }
}