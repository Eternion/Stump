-- Sort top 200 Orbes owners(Players)
SET @OrbeId = 20000;

SELECT acc.Login, ch.Name, SUM(chi.Stack)
FROM characters_items chi INNER JOIN characters ch INNER JOIN stump_auth_backup.worlds_characters wc INNER JOIN stump_auth_backup.accounts acc
WHERE chi.ItemId = @OrbeId AND chi.OwnerId = ch.Id AND wc.CharacterId = chi.OwnerId AND acc.Id = wc.AccountId AND acc.UserGroupId = 1
GROUP BY chi.OwnerId
ORDER BY SUM(chi.Stack) DESC
LIMIT 200;