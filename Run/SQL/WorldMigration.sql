-- UPDATE Items AppearanceId
UPDATE stump_world.items_templates item1 JOIN stump_world_old.items_templates item2 ON (item1.Id = item2.Id) SET item1.AppearanceId = item2.AppearanceId;
UPDATE stump_world.items_templates_weapons item1 JOIN stump_world_old.items_templates_weapons item2 ON (item1.Id = item2.Id) SET item1.AppearanceId = item2.AppearanceId;

-- UPDATE interactives_spawns TemplateId
UPDATE stump_world.interactives_spawns io1 JOIN stump_world_old.interactives_spawns io2 ON (io1.Id = io2.Id) SET io1.TemplateId = io2.TemplateId;

-- ClEANUP interactives_spawns_skills
DELETE FROM interactives_spawns_skills WHERE InteractiveSpawnId NOT IN (SELECT Id FROM interactives_spawns);
DELETE FROM interactives_skills WHERE Id NOT IN (SELECT SkillId FROM interactives_spawns_skills) AND Type = 'Teleport';

-- RESET maps
UPDATE stump_data.maps SET TopNeighbourId = -1 WHERE TopNeighbourId = 0;
UPDATE stump_data.maps SET BottomNeighbourId = -1 WHERE BottomNeighbourId = 0;
UPDATE stump_data.maps SET LeftNeighbourId = -1 WHERE LeftNeighbourId = 0;
UPDATE stump_data.maps SET RightNeighbourId = -1 WHERE RightNeighbourId = 0;

UPDATE stump_data.maps SET TopNeighbourCellId = -1 WHERE TopNeighbourCellId = 0;
UPDATE stump_data.maps SET BottomNeighbourCellId = -1 WHERE BottomNeighbourCellId = 0;
UPDATE stump_data.maps SET LeftNeighbourCellId = -1 WHERE LeftNeighbourCellId = 0;
UPDATE stump_data.maps SET RightNeighbourCellId = -1 WHERE RightNeighbourCellId = 0;

-- UPDATE maps
UPDATE stump_data.maps maps1 JOIN stump_data.maps_old maps2 ON (maps1.Id = maps2.Id) SET maps1.TopNeighbourId = maps2.TopNeighbourId;
UPDATE stump_data.maps maps1 JOIN stump_data.maps_old maps2 ON (maps1.Id = maps2.Id) SET maps1.BottomNeighbourId = maps2.BottomNeighbourId;
UPDATE stump_data.maps maps1 JOIN stump_data.maps_old maps2 ON (maps1.Id = maps2.Id) SET maps1.LeftNeighbourId = maps2.LeftNeighbourId;
UPDATE stump_data.maps maps1 JOIN stump_data.maps_old maps2 ON (maps1.Id = maps2.Id) SET maps1.RightNeighbourId = maps2.RightNeighbourId;

UPDATE stump_data.maps maps1 JOIN stump_data.maps_old maps2 ON (maps1.Id = maps2.Id) SET maps1.TopNeighbourCellId = maps2.TopNeighbourCellId;
UPDATE stump_data.maps maps1 JOIN stump_data.maps_old maps2 ON (maps1.Id = maps2.Id) SET maps1.BottomNeighbourCellId = maps2.BottomNeighbourCellId;
UPDATE stump_data.maps maps1 JOIN stump_data.maps_old maps2 ON (maps1.Id = maps2.Id) SET maps1.LeftNeighbourCellId = maps2.LeftNeighbourCellId;
UPDATE stump_data.maps maps1 JOIN stump_data.maps_old maps2 ON (maps1.Id = maps2.Id) SET maps1.RightNeighbourCellId = maps2.RightNeighbourCellId;

UPDATE stump_data.maps maps1 JOIN stump_data.maps_old maps2 ON (maps1.Id = maps2.Id) SET maps1.BlueCellsBin = maps2.BlueCellsBin;
UPDATE stump_data.maps maps1 JOIN stump_data.maps_old maps2 ON (maps1.Id = maps2.Id) SET maps1.RedCellsBin = maps2.RedCellsBin;

UPDATE stump_data.maps maps1 JOIN stump_data.MapScrollActions mapScroll ON (maps1.Id = mapScroll.Id) SET maps1.TopNeighbourId = mapScroll.TopMapId WHERE mapScroll.TopMapId != 0;
UPDATE stump_data.maps maps1 JOIN stump_data.MapScrollActions mapScroll ON (maps1.Id = mapScroll.Id) SET maps1.BottomNeighbourId = mapScroll.BottomMapId WHERE mapScroll.BottomMapId != 0;
UPDATE stump_data.maps maps1 JOIN stump_data.MapScrollActions mapScroll ON (maps1.Id = mapScroll.Id) SET maps1.LeftNeighbourId = mapScroll.LeftMapId WHERE mapScroll.LeftMapId != 0;
UPDATE stump_data.maps maps1 JOIN stump_data.MapScrollActions mapScroll ON (maps1.Id = mapScroll.Id) SET maps1.RightNeighbourId = mapScroll.RightMapId WHERE mapScroll.RightMapId != 0;

-- UPDATE mounts_templates
UPDATE stump_world.mounts_templates mounts1 JOIN stump_world_old.mounts_templates mounts2 ON (mounts1.Id = mounts2.Id) SET mounts1.ScrollId = mounts2.ScrollId;