-- Cleanup worlds_characters to avoid colissions
/*
SELECT COUNT(*) AS nbr_doublon, CharacterId FROM worlds_characters GROUP BY CharacterId HAVING COUNT(*) > 1;
*/

CREATE TABLE `GOOD_ID` (
`Id`  int(12) NOT NULL ,
`CharacterId`  int(12) NULL
);

CREATE TABLE `TMP_WC` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `CharacterId` int(11) NOT NULL,
  `AccountId` int(11) NOT NULL,
  `WorldId` int(11) NOT NULL,
  PRIMARY KEY (`Id`,`CharacterId`)
) ENGINE=MyISAM AUTO_INCREMENT=118187 DEFAULT CHARSET=utf8;

INSERT INTO GOOD_ID
select min(Id),
CharacterId
from worlds_characters
group by CharacterId;

INSERT INTO TMP_WC 
select a.Id,
b.CharacterId,
b.AccountId,
b.WorldId 
from GOOD_ID a
inner join worlds_characters b on b.Id=a.Id;