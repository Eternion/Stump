ALTER TABLE `accounts`
ADD COLUMN `RecordVersion` int(10) DEFAULT 1 NOT NULL;

ALTER TABLE `connections`
ADD COLUMN `RecordVersion` int(10) DEFAULT 1 NOT NULL;