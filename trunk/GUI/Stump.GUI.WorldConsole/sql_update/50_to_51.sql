ALTER TABLE characters
ADD COLUMN CreationDate datetime NULL AFTER Id,
ADD COLUMN LastUsage datetime NULL AFTER CreationDate;