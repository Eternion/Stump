-- ########################### ACCOUNTS ###########################

-- Cleanup accounts to remove accounts doesn't have stump_auth.accounts associated
/*
SELECT COUNT(*) FROM accounts WHERE Id NOT IN (SELECT Id FROM stump_auth.accounts);
*/
DELETE FROM accounts WHERE Id NOT IN (SELECT Id FROM stump_auth.accounts);

-- Cleanup accounts_relations to remove relations doesn't have accounts associated
/*
SELECT COUNT(*) FROM accounts_relations WHERE AccountId NOT IN (SELECT Id FROM accounts);
*/
DELETE FROM accounts_relations WHERE AccountId NOT IN (SELECT Id FROM accounts);

-- Cleanup accounts_relations to remove relations doesn't have target accounts associated
/*
SELECT COUNT(*) FROM accounts_relations WHERE TargetId NOT IN (SELECT Id FROM accounts);
*/
DELETE FROM accounts_relations WHERE TargetId NOT IN (SELECT Id FROM accounts);

-- ########################### CHARACTERS ###########################

-- Cleanup characters_items to remove items doesn't have character associated
/*
SELECT COUNT(*) FROM characters_items WHERE OwnerId NOT IN (SELECT Id FROM characters);
*/
DELETE FROM characters_items WHERE OwnerId NOT IN (SELECT Id FROM characters);

-- Cleanup characters_items_selled to remove items selled doesn't have character associated
/*
SELECT COUNT(*) FROM characters_items_selled WHERE OwnerId NOT IN (SELECT Id FROM characters);
*/
DELETE FROM characters_items_selled WHERE OwnerId NOT IN (SELECT Id FROM characters);

-- Cleanup characters_jobs to remove jobs doesn't have character associated
/*
SELECT COUNT(*) FROM characters_jobs WHERE OwnerId NOT IN (SELECT Id FROM characters);
*/
DELETE FROM characters_jobs WHERE OwnerId NOT IN (SELECT Id FROM characters);

-- Cleanup characters_shortcuts_items to remove items shortcuts doesn't have character associated
/*
SELECT COUNT(*) FROM characters_shortcuts_items WHERE OwnerId NOT IN (SELECT Id FROM characters);
*/
DELETE FROM characters_shortcuts_items WHERE OwnerId NOT IN (SELECT Id FROM characters);

-- Cleanup characters_shortcuts_items_presets to remove items shortcuts doesn't have character associated
/*
SELECT COUNT(*) FROM characters_shortcuts_items_presets WHERE OwnerId NOT IN (SELECT Id FROM characters);
*/
DELETE FROM characters_shortcuts_items_presets WHERE OwnerId NOT IN (SELECT Id FROM characters);

-- Cleanup characters_shortcuts_spells to remove spells shortcuts doesn't have character associated
/*
SELECT COUNT(*) FROM characters_shortcuts_spells WHERE OwnerId NOT IN (SELECT Id FROM characters);
*/
DELETE FROM characters_shortcuts_spells WHERE OwnerId NOT IN (SELECT Id FROM characters);

-- Cleanup characters_spells to remove characters spells doesn't have character associated
/*
SELECT COUNT(*) FROM characters_spells WHERE OwnerId NOT IN (SELECT Id FROM characters);
*/
DELETE FROM characters_spells WHERE OwnerId NOT IN (SELECT Id FROM characters);


-- ########################### GUILDS ###########################

-- Cleanup guild_members to remove guild members doesn't have character associated
/*
SELECT COUNT(*) FROM guild_members WHERE CharacterId NOT IN (SELECT Id FROM characters);
*/
DELETE FROM guild_members WHERE CharacterId NOT IN (SELECT Id FROM characters);

-- Cleanup guild_members doesn't have guild record associated
/*
SELECT COUNT(*) FROM guild_members WHERE GuildId NOT IN (SELECT Id FROM guilds);
*/
DELETE FROM guild_members WHERE GuildId NOT IN (SELECT Id FROM guilds);

-- Cleanup guilds to remove guilds doesn't have any guild members
/*
SELECT COUNT(*) FROM guilds WHERE Id NOT IN (SELECT GuildId FROM guild_members);
*/
DELETE FROM guilds WHERE Id NOT IN (SELECT GuildId FROM guild_members);

-- Find guilds to remove guilds doesn't have any guild members
SELECT COUNT(*) FROM guilds WHERE Id NOT IN (SELECT GuildId FROM guild_members WHERE RankId = '1');

-- Cleanup world_maps_taxcollector to remove taxCollectors doesn't have guild associated
/*
SELECT COUNT(*) FROM world_maps_taxcollector WHERE GuildId NOT IN (SELECT Id FROM guilds);
*/
DELETE FROM world_maps_taxcollector WHERE GuildId NOT IN (SELECT Id FROM guilds);

-- Cleanup taxcollectors_items to remove taxCollectors Items doesn't have guild associated
/*
SELECT COUNT(*) FROM taxcollectors_items WHERE OwnerId NOT IN (SELECT Id FROM guilds);
*/
DELETE FROM taxcollectors_items WHERE OwnerId NOT IN (SELECT Id FROM guilds);

-- Cleanup world_maps_merchant to remove merchants doesn't have character associated
/*
SELECT COUNT(*) FROM world_maps_merchant WHERE CharacterId NOT IN (SELECT Id FROM characters);
*/
DELETE FROM world_maps_merchant WHERE CharacterId NOT IN (SELECT Id FROM characters);