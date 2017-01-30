-- UPDATE Items AppearanceId
UPDATE dev_238_data.Items item1 JOIN dev_uptodate_data.Items item2 ON (item1.Id = item2.Id) SET item1.AppearanceId = item2.AppearanceId;
UPDATE dev_238_data.Weapons item1 JOIN dev_uptodate_data.Weapons item2 ON (item1.Id = item2.Id) SET item1.AppearanceId = item2.AppearanceId;

-- UPDATE interactives_spawns TemplateId
UPDATE dev_238_world.interactives_spawns io1 JOIN dev_uptodate_world.interactives_spawns io2 ON (io1.Id = io2.Id) SET io1.TemplateId = io2.TemplateId;

-- ClEANUP interactives_spawns_skills
DELETE FROM interactives_spawns_skills WHERE InteractiveSpawnId NOT IN (SELECT Id FROM interactives_spawns);
DELETE FROM interactives_skills WHERE Id NOT IN (SELECT SkillId FROM interactives_spawns_skills) AND Type = 'Teleport';

-- RESET world_maps
UPDATE world_maps SET TopNeighbourId = -1 WHERE TopNeighbourId = 0;
UPDATE world_maps SET BottomNeighbourId = -1 WHERE BottomNeighbourId = 0;
UPDATE world_maps SET LeftNeighbourId = -1 WHERE LeftNeighbourId = 0;
UPDATE world_maps SET RightNeighbourId = -1 WHERE RightNeighbourId = 0;

UPDATE world_maps SET TopNeighbourCellId = NULL WHERE TopNeighbourCellId = 0;
UPDATE world_maps SET BottomNeighbourCellId = NULL WHERE BottomNeighbourCellId = 0;
UPDATE world_maps SET LeftNeighbourCellId = NULL WHERE LeftNeighbourCellId = 0;
UPDATE world_maps SET RightNeighbourCellId = NULL WHERE RightNeighbourCellId = 0;

-- UPDATE world_maps
UPDATE dev_238_world.world_maps maps1 JOIN dev_uptodate_world.world_maps maps2 ON (maps1.Id = maps2.Id) SET maps1.TopNeighbourId = maps2.TopNeighbourId;
UPDATE dev_238_world.world_maps maps1 JOIN dev_uptodate_world.world_maps maps2 ON (maps1.Id = maps2.Id) SET maps1.BottomNeighbourId = maps2.BottomNeighbourId;
UPDATE dev_238_world.world_maps maps1 JOIN dev_uptodate_world.world_maps maps2 ON (maps1.Id = maps2.Id) SET maps1.LeftNeighbourId = maps2.LeftNeighbourId;
UPDATE dev_238_world.world_maps maps1 JOIN dev_uptodate_world.world_maps maps2 ON (maps1.Id = maps2.Id) SET maps1.RightNeighbourId = maps2.RightNeighbourId;

UPDATE dev_238_world.world_maps maps1 JOIN dev_uptodate_world.world_maps maps2 ON (maps1.Id = maps2.Id) SET maps1.TopNeighbourCellId = maps2.TopNeighbourCellId;
UPDATE dev_238_world.world_maps maps1 JOIN dev_uptodate_world.world_maps maps2 ON (maps1.Id = maps2.Id) SET maps1.BottomNeighbourCellId = maps2.BottomNeighbourCellId;
UPDATE dev_238_world.world_maps maps1 JOIN dev_uptodate_world.world_maps maps2 ON (maps1.Id = maps2.Id) SET maps1.LeftNeighbourCellId = maps2.LeftNeighbourCellId;
UPDATE dev_238_world.world_maps maps1 JOIN dev_uptodate_world.world_maps maps2 ON (maps1.Id = maps2.Id) SET maps1.RightNeighbourCellId = maps2.RightNeighbourCellId;

UPDATE dev_238_world.world_maps maps1 JOIN dev_uptodate_world.world_maps maps2 ON (maps1.Id = maps2.Id) SET maps1.BlueCellsCSV = maps2.BlueCellsCSV;
UPDATE dev_238_world.world_maps maps1 JOIN dev_uptodate_world.world_maps maps2 ON (maps1.Id = maps2.Id) SET maps1.RedCellsCSV = maps2.RedCellsCSV;

UPDATE dev_238_world.world_maps maps1 JOIN dev_238_data.MapScrollActions mapScroll ON (maps1.Id = mapScroll.Id) SET maps1.TopNeighbourId = mapScroll.TopMapId WHERE mapScroll.TopMapId != 0;
UPDATE dev_238_world.world_maps maps1 JOIN dev_238_data.MapScrollActions mapScroll ON (maps1.Id = mapScroll.Id) SET maps1.BottomNeighbourId = mapScroll.BottomMapId WHERE mapScroll.BottomMapId != 0;
UPDATE dev_238_world.world_maps maps1 JOIN dev_238_data.MapScrollActions mapScroll ON (maps1.Id = mapScroll.Id) SET maps1.LeftNeighbourId = mapScroll.LeftMapId WHERE mapScroll.LeftMapId != 0;
UPDATE dev_238_world.world_maps maps1 JOIN dev_238_data.MapScrollActions mapScroll ON (maps1.Id = mapScroll.Id) SET maps1.RightNeighbourId = mapScroll.RightMapId WHERE mapScroll.RightMapId != 0;

-- UPDATE mounts_templates
UPDATE dev_238_world.mounts_templates mounts1 JOIN dev_uptodate_world.mounts_templates mounts2 ON (mounts1.Id = mounts2.Id) SET mounts1.ScrollId = mounts2.ScrollId;

-- UPDATE monsters_spawns
UPDATE dev_238_world.monsters_spawns monsters1 JOIN dev_uptodate_world.monsters_spawns monsters2 ON (monsters1.MonsterId = monsters2.MonsterId AND monsters1.SubAreaId = monsters2.SubAreaId) SET monsters1.IsDisabled = monsters2.IsDisabled;

-- CLEANUP monsters_spells
DELETE FROM monsters_spells WHERE SpellId = -1;
