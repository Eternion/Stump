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
using NLog;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Data;
using Stump.Server.WorldServer.Data;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.World.Zones.Map;
using Stump.Server.WorldServer.XmlSerialize;
using Point = System.Drawing.Point;

namespace Stump.Server.WorldServer.World
{
    public partial class World
    {

        /// <summary>
        /// Welcome message
        /// </summary>
        [Variable]
        public static string MessageOfTheDay = "Bienvenue. Ce serveur est propulsé par Stump.";

        private readonly Logger logger = LogManager.GetCurrentClassLogger();


        public void Initialize()
        {
            logger.Info("Building World...");
            BuildWorld();
            logger.Info("Spawning World");
            SpawnWorld();
        }


        public void BuildWorld()
        {
            logger.Info("Building SuperAreas...");
            BuildSuperAreas();

            logger.Info("Building Areas...");
            BuildAreas();

            logger.Info("Building SubAreas...");
            BuildSubAreas();

            logger.Info("Building Maps...");
            BuildMaps();

            logger.Info("Building Cell Triggers...");
            BuildTriggers();

            logger.Info("Building Interactive Objects...");
            BuildInteractiveObjects();

            logger.Info("Building Houses");
            //TODO

            logger.Info("Building Paddocks");
            //TODO

            logger.Info("Building BidHouses");
            //TODO
        }

        public void BuildSuperAreas()
        {
            foreach (var data in DataLoader.LoadData<SuperArea>())
            {
                var superArea = new Zones.SuperArea(data.id, DataLoader.GetI18NText(data.nameId), this);
                m_superAreas.Add(superArea.Id, superArea);
            }
            logger.Info("Loaded {0} SuperAreas", m_superAreas.Count);
        }

        public void BuildAreas()
        {
            foreach (var data in DataLoader.LoadData<Area>())
            {
                var superArea = GetSuperArea(data.superAreaId);
                if (superArea != null)
                {
                    var area = new Zones.Area(data.id, DataLoader.GetI18NText(data.nameId), superArea);
                    superArea.Areas.Add(area.Id, area);
                    m_areas.Add(area.Id, area);
                }
                else
                    logger.Warn("Area {0} belongs to unexistant SuperArea {1} !", data.id, data.superAreaId);
            }
            logger.Info("Loaded {0} Areas", m_areas.Count);
        }

        public void BuildSubAreas()
        {
            foreach (var data in DataLoader.LoadData<SubArea>())
            {
                var area = GetArea(data.areaId);
                if (area != null)
                {
                    var subArea = new Zones.SubArea(data.id, DataLoader.GetI18NText(data.nameId), area);
                    area.SubAreas.Add(subArea.Id, subArea);
                    m_subAreas.Add(subArea.Id, subArea);
                }
                else
                    logger.Warn("SubArea {0} belongs to unexistant Area {1}", data.id, data.areaId);
            }
            logger.Info("Loaded {0} SubAreas", m_areas.Count);
        }

        public void BuildMaps()
        {
            var consoleProcent = new ConsoleProcent();
            int count = MapLoader.GetMapFilesCount();
            int i = 0;
            IDictionary<int, MapPosition> mapsPosition = MapLoader.LoadMapPositions();
            foreach (var data in MapLoader.LoadMaps())
            {
                if (mapsPosition.ContainsKey(data.Id))
                {
                    if (m_subAreas.ContainsKey((int)data.SubAreaId))
                    {
                        var pos = mapsPosition[data.Id];
                        var subArea = GetSubArea((int)data.SubAreaId);
                        var point = new Point(pos.posX, pos.posY);

                        var map = new Map((uint)data.Id, data.MapType, point, subArea);

                        subArea.Maps.Add((int)map.Id, map);
                        m_maps.Add((int)map.Id, map);

                        if (map.MapType == MapTypeEnum.OUTDOOR)
                            m_outdoorMapsByCoordinates.Add(point, map);
                        else
                            m_indoorMapsByCoordinates.Add(point, map);

                        consoleProcent.Update(i++ / count);
                    }
                    else
                    {
                        logger.Warn("Map {0} belongs to an innexistant subArea :{1}", data.Id, data.SubAreaId);
                    }
                }
                else
                {
                    logger.Warn("Map {0} doesn't have a position", data.Id);
                }
            }
            consoleProcent.End();
        }

        public void BuildTriggers()
        {
            foreach (CellTrigger trigger in MapLoader.LoadTriggers())
            {
                try
                {
                    trigger.Map.AddTrigger(trigger.Cell, trigger);
                }
                catch (Exception e)
                {
                    logger.Warn("Cannot add trigger on cell <id:{0}> : {1}", trigger.Cell.Id, e.Message);
                }
            }
        }

        public void BuildInteractiveObjects()
        {
            foreach (InteractiveElementSerialized interactiveObject in InteractiveObjectLoader.LoadsInteractiveObjects())
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


        public void SpawnWorld()
        {
            logger.Info("Spawn Npcs...");
            SpawnNpcs();

            logger.Info("Spawn Monsters...");
            //TODO

            logger.Info("Spawn Prisms");
            //TODO

            logger.Info("Spawn Collectors");
            //TODO
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

        public void  SpawnMonsters()
        {
            
        }

        public void SpawnPrisms()
        {
            
        }

        public void SpawnCollectors()
        {
            
        }

    }
}