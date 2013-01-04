DELETE FROM interactives_skills;
ALTER TABLE `interactives_skills` AUTO_INCREMENT=1;
DELETE FROM interactives_templates_skills;
ALTER TABLE `interactives_templates_skills` AUTO_INCREMENT=1;
DELETE FROM interactives_spawns_skills;
ALTER TABLE `interactives_spawns_skills` AUTO_INCREMENT=1;

-- zaaps
INSERT INTO interactives_skills (Type) VALUES ('ZaapTeleport');
INSERT INTO interactives_templates_skills VALUES (16, (SELECT last_insert_id() FROM interactives_skills));