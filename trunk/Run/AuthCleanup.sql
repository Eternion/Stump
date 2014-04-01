-- Cleanup worlds_characters to avoid colissions
/*
SELECT COUNT(*) AS nbr_doublon, CharacterId FROM worlds_characters GROUP BY CharacterId HAVING COUNT(*) > 1;
*/

CREATE TABLE GOOD_ID (id number, 

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

TRUNCATE TABLE worlds_characters;
INSERT INTO worlds_characters
select * from TMP_WC;