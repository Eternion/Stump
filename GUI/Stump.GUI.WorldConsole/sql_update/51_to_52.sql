ALTER TABLE `maps`
CHANGE COLUMN `TopNeighbourId` `ClientTopNeighbourId`  int(11) NULL DEFAULT NULL AFTER `SubAreaId`,
CHANGE COLUMN `BottomNeighbourId` `ClientBottomNeighbourId`  int(11) NULL DEFAULT NULL AFTER `ClientTopNeighbourId`,
CHANGE COLUMN `LeftNeighbourId` `ClientLeftNeighbourId`  int(11) NULL DEFAULT NULL AFTER `ClientBottomNeighbourId`,
CHANGE COLUMN `RightNeighbourId` `ClientRightNeighbourId`  int(11) NULL DEFAULT NULL AFTER `ClientLeftNeighbourId`,
ADD COLUMN `TopNeighbourId`  int(11) NULL DEFAULT NULL AFTER `ClientRightNeighbourId`,
ADD COLUMN `BottomNeighbourId`  int(11) NULL DEFAULT NULL AFTER `TopNeighbourId`,
ADD COLUMN `LeftNeighbourId`  int(11) NULL DEFAULT NULL AFTER `BottomNeighbourId`,
ADD COLUMN `RightNeighbourId`  int(11) NULL DEFAULT NULL AFTER `LeftNeighbourId`;

