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
using System.Linq;
using System.Collections.Generic;
using NLog;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.IO;
using Stump.BaseCore.Framework.Utils;
using Stump.DofusProtocol.Enums;
using Stump.Server.DataProvider.Data.Areas;
using Stump.Server.DataProvider.Data.Interactives;
using Stump.Server.DataProvider.Data.Map;
using Stump.Server.DataProvider.Data.SubAreas;
using Stump.Server.DataProvider.Data.SuperAreas;
using Stump.Server.WorldServer.Data;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.World.Zones;
using Stump.Server.WorldServer.XmlSerialize;
using System.Drawing;

namespace Stump.Server.WorldServer.World
{
    public partial class World : Singleton<World>
    {

        /// <summary>
        /// Welcome message
        /// </summary>
        [Variable]
        public static string MessageOfTheDay = "Bienvenue. Ce serveur est propulsé par Stump.";

        private readonly Logger logger = LogManager.GetCurrentClassLogger();


        public void Initialize()
        {
            logger.Info("Build World...");
            BuildWorld();

            logger.Info("Spawn World");
            SpawnWorld();
        }

        private void BuildWorld()
        {
            logger.Info("Build SuperAreas...");
            BuildSuperAreas();

            logger.Info("Build Areas...");
            BuildAreas();

            logger.Info("Build SubAreas...");
            BuildSubAreas();

            logger.Info("Build Maps...");
            BuildMaps();

            logger.Info("Build Triggers...");
            BuildTriggers();

            logger.Info("Build Interactives...");
            BuildInteractiveObjects();

            logger.Info("Build Houses");
            //TODO

            logger.Info("Build Paddocks");
            //TODO

            logger.Info("Build BidHouses");
            //TODO
        }

        private void BuildSuperAreas()
        {
            m_superAreas=SuperAreaTemplateManager.Instance.Select(t => new SuperArea(t)).ToDictionary(s => s.Id);
            logger.Info("Loaded {0} SuperAreas", m_superAreas.Count);
        }

        private void BuildAreas()
        {
            foreach (var areaTemplate in AreaTemplateManager.Instance)
            {
                var superArea = GetSuperArea(areaTemplate.SuperAreaId);
                if (superArea != null)
                {
                    var area = new Area(areaTemplate, superArea);
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
            foreach (var subAreaTemplate in SubAreaTemplateManager.Instance)
            {
                var area = GetArea(subAreaTemplate.AreaId);
                if (area != null)
                {
                    var subArea = new SubArea(subAreaTemplate, area);
                    area.SubAreas.Add(subArea.Id, subArea);
                    m_subAreas.Add(subArea.Id, subArea);
                }
                else
                    logger.Warn("SubArea {0} belongs to unexistant Area {1}", subAreaTemplate.id, subAreaTemplate.areaId);
            }
            logger.Info("Loaded {0} SubAreas", m_areas.Count);
        }

        public void BuildMaps()
        {
            var mapTemplates = MapTemplateManager.Instance.GetAll();

            var consoleProcent = new ConsoleProcent();
            var count = mapTemplates.Count();
            var i = 0;

            foreach (var mapTemplate in mapTemplates)
            {
                if (mapTemplate.Position != Point.Empty)
                {
                    if (m_subAreas.ContainsKey(mapTemplate.SubAreaId))
                    {
                        var subArea = GetSubArea(mapTemplate.SubAreaId);

                        var map = new Map(mapTemplate, subArea);

                        subArea.Maps.Add(map.Id, map);
                        m_maps.Add(map.Id, map);

                        if (map.MapType == MapTypeEnum.OUTDOOR)
                            m_outdoorMapsByCoordinates.Add(map.Position, map);
                        else
                            m_indoorMapsByCoordinates.Add(map.Position, map);

                        consoleProcent.Update(i++ / count);
                    }
                    else
                    {
                        logger.Warn("Map {0} belongs to an innexistant subArea :{1}", mapTemplate.Id, mapTemplate.SubAreaId);
                    }
                }
                else
                {
                    logger.Warn("Map {0} doesn't have a position", mapTemplate.Id);
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

        public void SpawnMonsters()
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