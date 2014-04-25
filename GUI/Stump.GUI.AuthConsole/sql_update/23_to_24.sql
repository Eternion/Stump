ALTER TABLE `accounts`
ADD COLUMN `NewTokens` int(10) DEFAULT 0 NOT NULL AFTER `Tokens`;