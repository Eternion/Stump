
using System;
using System.Collections.Concurrent;
using System.Linq;
using Stump.Database.Data.World;
using Stump.Server.WorldServer.Data;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.XmlSerialize;

namespace Stump.Server.WorldServer.Global
{
    public partial class World
    {
        public void Initialize()
        {
            logger.Info("Build World...");
            BuildWorld();

            logger.Info("Spawn World");
            SpawnWorld();
        }

        private void BuildWorld()
        {
            logger.Info("Build Continents...");
            BuildSuperAreas();

            logger.Info("Build Regions...");
            BuildAreas();

            logger.Info("Build Zones...");
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
            Continents = SuperAreaRecord.FindAll().ToDictionary(key => key.Id, value => new Continent(value));
            logger.Info("Loaded {0} Continents", Continents.Count);
        }

        private void BuildAreas()
        {
            Regions = AreaRecord.FindAll().ToDictionary(key => key.Id,
                                                        value => new Region(value, Continents[value.SuperAreaId]));
            logger.Info("Loaded {0} Regions", Regions.Count);
        }

        public void BuildSubAreas()
        {
            Zones = SubAreaRecord.FindAll().ToDictionary(key => key.Id, value => new Zone(value, Regions[value.AreaId]));
            logger.Info("Loaded {0} Zones", Zones.Count);
        }

        public void BuildMaps()
        {
            Maps =
                new ConcurrentDictionary<int, Map>(MapRecord.FindAll().ToDictionary(key => key.Id,
                                                                                    value =>
                                                                                    new Map(value, Zones[value.ZoneId])));
            logger.Info("Loaded {0} Maps", Maps.Count);
        }

        public void BuildTriggers()
        {
            foreach (CellTrigger trigger in MapLoader.LoadTriggers())
            {
                try
                {
                    trigger.Map.SpawnTrigger(trigger.Cell, trigger);
                }
                catch (Exception e)
                {
                    logger.Warn("Cannot add trigger on cell <id:{0}> : {1}", trigger.Cell.Id, e.Message);
                }
            }
        }

        public void BuildInteractiveObjects()
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