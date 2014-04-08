ALTER TABLE accounts
ADD COLUMN `LastServer` int(11) DEFAULT NULL AFTER LastLogin;