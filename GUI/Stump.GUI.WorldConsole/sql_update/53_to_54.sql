ALTER TABLE `items_set`
ADD COLUMN `BonusEffects`  longblob NULL AFTER `BonusIsSecret`;

ALTER TABLE `characters`
ADD COLUMN `KnownZaaps`  longblob NOT NULL AFTER `PvPEnabled`;

