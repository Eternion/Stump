ALTER TABLE worlds
ADD COLUMN `Status` int(11) DEFAULT 1 AFTER CharCapacity,
ADD COLUMN `CharsCount` int(11) DEFAULT NULL AFTER `Status`;

ALTER TABLE accounts
ADD COLUMN `Points` int(11) DEFAULT NULL,
ADD COLUMN `LastVote` datetime DEFAULT NULL;