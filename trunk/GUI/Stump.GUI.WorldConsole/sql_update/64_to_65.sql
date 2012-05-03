ALTER TABLE `characters`
ADD COLUMN `CustomEntityLook`  text NULL AFTER `EntityLook`,
ADD COLUMN `CustomLookActivated`  tinyint NULL AFTER `CustomEntityLook`;

