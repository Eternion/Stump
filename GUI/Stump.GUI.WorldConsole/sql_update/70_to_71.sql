CREATE TABLE `breeds_items` (
`Id`  int NOT NULL ,
`Breed`  int NOT NULL ,
`ItemId`  int NOT NULL ,
`Amount`  int NOT NULL DEFAULT 1 ,
`MaxEffects`  tinyint NOT NULL DEFAULT 1 ,
PRIMARY KEY (`Id`)
)
;

RENAME TABLE breed_spells TO breeds_spells;
