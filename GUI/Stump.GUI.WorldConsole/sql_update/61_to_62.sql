ALTER TABLE `items_selled`
CHANGE COLUMN `NpcShopId` `Npc_NpcShopId`  int(11) NULL DEFAULT NULL AFTER `ItemId`,
CHANGE COLUMN `CustomPrice` `Npc_CustomPrice`  float NULL DEFAULT NULL AFTER `Npc_NpcShopId`,
CHANGE COLUMN `BuyCriterion` `Npc_BuyCriterion`  varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL AFTER `Npc_CustomPrice`,
ADD COLUMN `Npc_MaxStats`  tinyint NULL AFTER `Npc_BuyCriterion`;

ALTER TABLE `npcs_actions`
MODIFY COLUMN `Shop_Token`  int(11) NULL AFTER `Talk_MessageId`,
MODIFY COLUMN `Shop_CanSell`  tinyint(4) NULL DEFAULT 1 AFTER `Shop_Token`,
ADD COLUMN `Shop_MaxStats`  tinyint NULL AFTER `Shop_CanSell`;

