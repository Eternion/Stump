ALTER TABLE `experiences`
ADD COLUMN `GuildExp`  bigint(20) NULL DEFAULT NULL AFTER `CharacterExp`,
ADD COLUMN `MountExp`  bigint(20) NULL DEFAULT NULL AFTER `GuildExp`,
ADD COLUMN `AlignmentHonor`  smallint UNSIGNED NULL DEFAULT NULL AFTER `MountExp`;

ALTER TABLE `characters`
ADD COLUMN `AlignmentSide`  int NOT NULL AFTER `SpellsPoints`,
ADD COLUMN `AlignmentValue`  tinyint NOT NULL AFTER `AlignmentSide`,
ADD COLUMN `Honor`  smallint UNSIGNED NOT NULL AFTER `AlignmentValue`,
ADD COLUMN `Dishonor`  smallint UNSIGNED NOT NULL AFTER `Honor`,
ADD COLUMN `PvPEnabled`  tinyint NOT NULL AFTER `Dishonor`;

UPDATE `experiences` SET AlignmentHonor="0" WHERE `Level`="1";
UPDATE `experiences` SET AlignmentHonor="500" WHERE `Level`="2";
UPDATE `experiences` SET AlignmentHonor="1500" WHERE `Level`="3";
UPDATE `experiences` SET AlignmentHonor="3000" WHERE `Level`="4";
UPDATE `experiences` SET AlignmentHonor="5000" WHERE `Level`="5";
UPDATE `experiences` SET AlignmentHonor="7500" WHERE `Level`="6";
UPDATE `experiences` SET AlignmentHonor="10000" WHERE `Level`="7";
UPDATE `experiences` SET AlignmentHonor="12500" WHERE `Level`="8";
UPDATE `experiences` SET AlignmentHonor="15000" WHERE `Level`="9";
UPDATE `experiences` SET AlignmentHonor="17500" WHERE `Level`="10";
