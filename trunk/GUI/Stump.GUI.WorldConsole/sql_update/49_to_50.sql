ALTER TABLE monsters_grades
ADD COLUMN TackleEvade smallint NULL AFTER MovementPoints,
ADD COLUMN TackleBlock smallint NULL AFTER TackleEvade;