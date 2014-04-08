ALTER TABLE `interactives_skills`
MODIFY COLUMN `MapId`  int(11) NULL AFTER `Duration`,
MODIFY COLUMN `CellId`  int(11) NULL AFTER `MapId`,
MODIFY COLUMN `Direction`  int(11) NULL AFTER `CellId`;
ALTER TABLE `npcs_replies`
ADD COLUMN `MapId`  int(11) NULL AFTER `NextMessageId`,
ADD COLUMN `CellId`  int(11) NULL AFTER `MapId`,
ADD COLUMN `Direction`  int(11) NULL AFTER `CellId`;