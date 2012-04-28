ALTER TABLE `npcs_actions`
ADD COLUMN `Shop_Token`  int NULL AFTER `Talk_MessageId`,
ADD COLUMN `Shop_CanSell`  tinyint NULL AFTER `Shop_Token`;

