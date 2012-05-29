ALTER TABLE `characters`
ADD COLUMN `PermanentAddedStrength`  smallint NOT NULL AFTER `Agility`,
ADD COLUMN `PermanentAddedChance`  smallint NOT NULL AFTER `PermanentAddedStrength`,
ADD COLUMN `PermanentAddedVitality`  smallint NOT NULL AFTER `PermanentAddedChance`,
ADD COLUMN `PermanentAddedWisdom`  smallint NOT NULL AFTER `PermanentAddedVitality`,
ADD COLUMN `PermanentAddedIntelligence`  smallint NOT NULL AFTER `PermanentAddedWisdom`,
ADD COLUMN `PermanentAddedAgility`  smallint NOT NULL AFTER `PermanentAddedIntelligence`;

