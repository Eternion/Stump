



-- -----------------------------------------------------------
-- Entity Designer DDL Script for MySQL Server 4.1 and higher
-- -----------------------------------------------------------
-- Date Created: 06/26/2012 14:00:14
-- Generated from EDMX file: C:\Users\Bouh2\Desktop\Programming\C#\Project Stump (git)\trunk\Server\Stump.Server.WorldServer\Database\Model.edmx
-- Target version: 2.0.0.0
-- --------------------------------------------------

DROP DATABASE IF EXISTS `stump_world`;
CREATE DATABASE `stump_world`;
USE `stump_world`;

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- NOTE: if the constraint does not exist, an ignorable error will be reported.
-- --------------------------------------------------

--    ALTER TABLE `WorldAccountFriends` DROP CONSTRAINT `FK_WorldAccountFriend_WorldAccount`;
--    ALTER TABLE `WorldAccountFriends` DROP CONSTRAINT `FK_WorldAccountFriend_Friend`;
--    ALTER TABLE `WorldAccountIgnoreds` DROP CONSTRAINT `FK_WorldAccountIgnored_WorldAccount`;
--    ALTER TABLE `WorldAccountIgnoreds` DROP CONSTRAINT `FK_WorldAccountIgnored_Ignored`;
--    ALTER TABLE `WorldAccountStartupActions` DROP CONSTRAINT `FK_WorldAccountStartupAction_WorldAccount`;
--    ALTER TABLE `WorldAccountStartupActions` DROP CONSTRAINT `FK_WorldAccountStartupAction_StartupAction`;
--    ALTER TABLE `StartupActionsObjects` DROP CONSTRAINT `FK_StartupActionStartupActionObject`;
--    ALTER TABLE `BreedItems` DROP CONSTRAINT `FK_BreedBreedItem`;
--    ALTER TABLE `BreedSpells` DROP CONSTRAINT `FK_BreedBreedSpell`;
--    ALTER TABLE `CharacterSpells` DROP CONSTRAINT `FK_CharacterRecordCharacterSpell`;
--    ALTER TABLE `InteractiveSpawnInteractiveSkillRecord` DROP CONSTRAINT `FK_InteractiveSpawnInteractiveSkillRecord_InteractiveSpawn`;
--    ALTER TABLE `InteractiveSpawnInteractiveSkillRecord` DROP CONSTRAINT `FK_InteractiveSpawnInteractiveSkillRecord_InteractiveSkillRecord`;
--    ALTER TABLE `InteractivesSkills_SkillTemplateDependantRecord` DROP CONSTRAINT `FK_SkillTemplateDependantRecord_inherits_SkillRecord`;
--    ALTER TABLE `InteractivesSkills_SkillTeleportRecord` DROP CONSTRAINT `FK_SkillTeleportRecord_inherits_SkillTemplateDependantRecord`;
--    ALTER TABLE `InteractivesSkills_SkillZaapTeleportRecord` DROP CONSTRAINT `FK_SkillZaapTeleportRecord_inherits_SkillTemplateDependantRecord`;
--    ALTER TABLE `InteractivesSkills_SkillZaapSaveRecord` DROP CONSTRAINT `FK_SkillZaapSaveRecord_inherits_SkillTemplateDependantRecord`;

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------
SET foreign_key_checks = 0;
    DROP TABLE IF EXISTS `Accounts`;
    DROP TABLE IF EXISTS `Areas`;
    DROP TABLE IF EXISTS `Breeds`;
    DROP TABLE IF EXISTS `BreedItems`;
    DROP TABLE IF EXISTS `BreedSpells`;
    DROP TABLE IF EXISTS `Characters`;
    DROP TABLE IF EXISTS `CharacterJobs`;
    DROP TABLE IF EXISTS `CharacterSpells`;
    DROP TABLE IF EXISTS `Effects`;
    DROP TABLE IF EXISTS `ExperiencesEntries`;
    DROP TABLE IF EXISTS `Hints`;
    DROP TABLE IF EXISTS `Interactives`;
    DROP TABLE IF EXISTS `InteractivesCustomSkills`;
    DROP TABLE IF EXISTS `InteractivesSkills`;
    DROP TABLE IF EXISTS `InteractivesSpawns`;
    DROP TABLE IF EXISTS `InteractivesTemplatesSkills`;
    DROP TABLE IF EXISTS `Items`;
    DROP TABLE IF EXISTS `ItemsSelled`;
    DROP TABLE IF EXISTS `ItemsSets`;
    DROP TABLE IF EXISTS `ItemsTemplates`;
    DROP TABLE IF EXISTS `ItemsTypes`;
    DROP TABLE IF EXISTS `Jobs`;
    DROP TABLE IF EXISTS `Maps`;
    DROP TABLE IF EXISTS `MapsCellsTriggers`;
    DROP TABLE IF EXISTS `MapsCoordinates`;
    DROP TABLE IF EXISTS `MapsPositions`;
    DROP TABLE IF EXISTS `MapsReferences`;
    DROP TABLE IF EXISTS `Monsters`;
    DROP TABLE IF EXISTS `MonstersDrops`;
    DROP TABLE IF EXISTS `MonstersGrades`;
    DROP TABLE IF EXISTS `MonstersRaces`;
    DROP TABLE IF EXISTS `MonstersSpawns`;
    DROP TABLE IF EXISTS `MonstersSpawnsDungeon`;
    DROP TABLE IF EXISTS `MonstersSpells`;
    DROP TABLE IF EXISTS `MonstersSuperRaces`;
    DROP TABLE IF EXISTS `Npcs`;
    DROP TABLE IF EXISTS `NpcsActions`;
    DROP TABLE IF EXISTS `NpcsMessages`;
    DROP TABLE IF EXISTS `Npcs_Replies`;
    DROP TABLE IF EXISTS `NpcSpawns`;
    DROP TABLE IF EXISTS `Shortcuts`;
    DROP TABLE IF EXISTS `InteractiveSkillsTemplates`;
    DROP TABLE IF EXISTS `Spells`;
    DROP TABLE IF EXISTS `SpellsBomb`;
    DROP TABLE IF EXISTS `SpellsLevels`;
    DROP TABLE IF EXISTS `SpellsStates`;
    DROP TABLE IF EXISTS `SpellsTypes`;
    DROP TABLE IF EXISTS `StartupActions`;
    DROP TABLE IF EXISTS `StartupActionsObjects`;
    DROP TABLE IF EXISTS `SubAreas`;
    DROP TABLE IF EXISTS `SuperAreas`;
    DROP TABLE IF EXISTS `Texts`;
    DROP TABLE IF EXISTS `TextsUI`;
    DROP TABLE IF EXISTS `versions`;
    DROP TABLE IF EXISTS `WorldMaps`;
    DROP TABLE IF EXISTS `InteractivesSkills_SkillTemplateDependantRecord`;
    DROP TABLE IF EXISTS `InteractivesSkills_SkillTeleportRecord`;
    DROP TABLE IF EXISTS `InteractivesSkills_SkillZaapTeleportRecord`;
    DROP TABLE IF EXISTS `InteractivesSkills_SkillZaapSaveRecord`;
    DROP TABLE IF EXISTS `WorldAccountFriends`;
    DROP TABLE IF EXISTS `WorldAccountIgnoreds`;
    DROP TABLE IF EXISTS `WorldAccountStartupActions`;
    DROP TABLE IF EXISTS `InteractiveSpawnInteractiveSkillRecord`;
SET foreign_key_checks = 1;

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Accounts'

CREATE TABLE `Accounts` (
    `Id` int  NOT NULL,
    `Nickname` longtext  NOT NULL,
    `LastConnection` datetime  NULL,
    `LastIp` longtext  NULL,
    `ConnectedCharacter` int  NULL
);

-- Creating table 'Areas'

CREATE TABLE `Areas` (
    `Id` int  NOT NULL,
    `NameId` bigint  NULL,
    `SubAreaId` int  NULL,
    `ContainHouses` bool  NULL,
    `ContainPaddocks` bool  NULL,
    `Bounds` varbinary(100)  NULL
);

-- Creating table 'Breeds'

CREATE TABLE `Breeds` (
    `Id` int  NOT NULL,
    `AlternativeMaleSkinBin` varbinary(100)  NOT NULL,
    `AlternativeFemaleSkinBin` varbinary(100)  NOT NULL,
    `GameplayDescriptionId` int  NOT NULL,
    `ShortNameId` int  NOT NULL,
    `LongNameId` int  NOT NULL,
    `DescriptionId` int  NOT NULL,
    `MaleLookString` longtext  NOT NULL,
    `FemaleLookString` longtext  NOT NULL,
    `CreatureBonesId` bigint  NOT NULL,
    `MaleArtwork` int  NOT NULL,
    `FemaleArtwork` int  NOT NULL,
    `StatsPointsForStrengthBin` varbinary(100)  NOT NULL,
    `StatsPointsForIntelligenceBin` varbinary(100)  NOT NULL,
    `StatsPointsForChanceBin` varbinary(100)  NOT NULL,
    `StatsPointsForAgilityBin` varbinary(100)  NOT NULL,
    `MaleColorsBin` varbinary(100)  NOT NULL,
    `StatsPointsForVitalityBin` varbinary(100)  NOT NULL,
    `StatsPointsForWisdomBin` varbinary(100)  NOT NULL,
    `BreedSpellsIdBin` varbinary(100)  NOT NULL,
    `FemaleColorsBin` varbinary(100)  NOT NULL,
    `StartMap` int  NOT NULL,
    `StartCell` smallint  NOT NULL,
    `StartDirection` int  NOT NULL,
    `StartActionPoints` int  NOT NULL,
    `StartMovementPoints` int  NOT NULL,
    `StartHealthPoint` int  NOT NULL,
    `StartProspection` int  NOT NULL,
    `StartStatsPoints` int  NOT NULL,
    `StartSpellsPoints` int  NOT NULL,
    `StartStrength` smallint  NOT NULL,
    `StartVitality` smallint  NOT NULL,
    `StartWisdom` smallint  NOT NULL,
    `StartIntelligence` smallint  NOT NULL,
    `StartChance` smallint  NOT NULL,
    `StartAgility` smallint  NOT NULL,
    `StartLevel` tinyint unsigned  NOT NULL,
    `StartKamas` int  NOT NULL
);

-- Creating table 'BreedItems'

CREATE TABLE `BreedItems` (
    `Id` int  NOT NULL,
    `BreedId` int  NOT NULL,
    `ItemId` int  NOT NULL,
    `Amount` int  NOT NULL,
    `MaxEffects` bool  NOT NULL
);

-- Creating table 'BreedSpells'

CREATE TABLE `BreedSpells` (
    `Id` int AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `Spell` int  NULL,
    `ObtainLevel` int  NULL,
    `BreedId` int  NOT NULL
);

-- Creating table 'Characters'

CREATE TABLE `Characters` (
    `Id` int AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `CreationDate` datetime  NULL,
    `LastUsage` datetime  NULL,
    `Name` longtext  NOT NULL,
    `Breed` int  NOT NULL,
    `Sex` int  NOT NULL,
    `EntityLookString` longtext  NULL,
    `CustomEntityLookString` longtext  NULL,
    `CustomLookActivated` tinyint  NULL,
    `TitleId` bigint  NOT NULL,
    `TitleParam` longtext  NOT NULL,
    `HasRecolor` bool  NOT NULL,
    `HasRename` bool  NOT NULL,
    `CantBeAggressed` bool  NULL,
    `CantBeChallenged` bool  NULL,
    `CantTrade` bool  NULL,
    `CantBeAttackedByMutant` bool  NULL,
    `CantRun` bool  NULL,
    `ForceSlowWalk` bool  NULL,
    `CantMinimize` bool  NULL,
    `CantMove` bool  NULL,
    `CantAggress` bool  NULL,
    `CantChallenge` bool  NULL,
    `CantExchange` bool  NULL,
    `CantAttack` bool  NULL,
    `CantChat` bool  NULL,
    `CantBeMerchant` bool  NULL,
    `CantUseObject` bool  NULL,
    `CantUseTaxCollector` bool  NULL,
    `CantUseInteractive` bool  NULL,
    `CantSpeakToNpc` bool  NULL,
    `CantChangeZone` bool  NULL,
    `CantAttackMonster` bool  NULL,
    `CantWalk8Directions` bool  NULL,
    `MapId` int  NOT NULL,
    `CellId` smallint  NOT NULL,
    `DirectionInt` int  NOT NULL,
    `BaseHealth` int  NOT NULL,
    `DamageTaken` int  NOT NULL,
    `AP` int  NOT NULL,
    `MP` int  NOT NULL,
    `Prospection` int  NOT NULL,
    `Strength` smallint  NOT NULL,
    `Chance` smallint  NOT NULL,
    `Vitality` smallint  NOT NULL,
    `Wisdom` smallint  NOT NULL,
    `Intelligence` smallint  NOT NULL,
    `Agility` smallint  NOT NULL,
    `PermanentAddedStrength` smallint  NOT NULL,
    `PermanentAddedChance` smallint  NOT NULL,
    `PermanentAddedVitality` smallint  NOT NULL,
    `PermanentAddedWisdom` smallint  NOT NULL,
    `PermanentAddedIntelligence` smallint  NOT NULL,
    `PermanentAddedAgility` smallint  NOT NULL,
    `Kamas` int  NOT NULL,
    `CanRestat` bool  NOT NULL,
    `Experience` bigint  NOT NULL,
    `EnergyMax` smallint  NOT NULL,
    `Energy` smallint  NOT NULL,
    `StatsPoints` int  NOT NULL,
    `SpellsPoints` int  NOT NULL,
    `AlignmentSide` int  NULL,
    `AlignmentValue` tinyint  NULL,
    `Honor` int  NULL,
    `Dishonor` int  NULL,
    `PvPEnabled` bool  NULL,
    `KnownZaapsBin` varbinary(100)  NOT NULL,
    `SpawnMapId` int  NULL,
    `WarnOnConnection` bool  NULL,
    `WarnOnLevel` bool  NULL
);

-- Creating table 'CharacterJobs'

CREATE TABLE `CharacterJobs` (
    `Id` int  NOT NULL,
    `JobId` int  NULL,
    `Owner` int  NULL,
    `Experience` bigint  NULL
);

-- Creating table 'CharacterSpells'

CREATE TABLE `CharacterSpells` (
    `Id` int AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `OwnerId` int  NOT NULL,
    `SpellId` int  NOT NULL,
    `Level` tinyint  NOT NULL,
    `Position` tinyint unsigned  NOT NULL
);

-- Creating table 'Effects'

CREATE TABLE `Effects` (
    `Id` bigint  NOT NULL,
    `DescriptionId` bigint  NULL,
    `IconId` bigint  NULL,
    `Characteristic` int  NULL,
    `Category` bigint  NULL,
    `Operator` longtext  NULL,
    `ShowInTooltip` bool  NULL,
    `UseDice` bool  NULL,
    `ForceMinMax` bool  NULL,
    `Boost` bool  NULL,
    `Active` bool  NULL,
    `ShowInSet` bool  NULL,
    `BonusType` int  NULL,
    `UseInFight` bool  NULL
);

-- Creating table 'ExperiencesEntries'

CREATE TABLE `ExperiencesEntries` (
    `Level` tinyint unsigned  NOT NULL,
    `CharacterExp` bigint  NULL,
    `GuildExp` bigint  NULL,
    `MountExp` bigint  NULL,
    `AlignmentHonor` int  NULL
);

-- Creating table 'Hints'

CREATE TABLE `Hints` (
    `Id` int  NOT NULL,
    `CategoryId` bigint  NULL,
    `Gfx` bigint  NULL,
    `NameId` bigint  NULL,
    `MapId` bigint  NULL,
    `RealMapId` bigint  NULL
);

-- Creating table 'Interactives'

CREATE TABLE `Interactives` (
    `Id` int  NOT NULL,
    `NameId` bigint  NULL,
    `ActionId` int  NULL,
    `DisplayTooltip` bool  NULL
);

-- Creating table 'InteractivesCustomSkills'

CREATE TABLE `InteractivesCustomSkills` (
    `InteractiveId` int  NOT NULL,
    `SkillId` int  NOT NULL
);

-- Creating table 'InteractivesSkills'

CREATE TABLE `InteractivesSkills` (
    `Id` int AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `Duration` bigint  NULL
);

-- Creating table 'InteractivesSpawns'

CREATE TABLE `InteractivesSpawns` (
    `Id` int AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `TemplateId` int  NULL,
    `ElementId` int  NOT NULL,
    `MapId` int  NOT NULL
);

-- Creating table 'InteractivesTemplatesSkills'

CREATE TABLE `InteractivesTemplatesSkills` (
    `InteractiveId` int  NOT NULL,
    `SkillId` int  NOT NULL
);

-- Creating table 'Items'

CREATE TABLE `Items` (
    `Id` int  NOT NULL,
    `RecognizerType` longtext  NOT NULL,
    `Item1` int  NOT NULL,
    `Stack` int  NOT NULL,
    `Effects` varbinary(100)  NOT NULL,
    `Owner` int  NULL,
    `Position` int  NOT NULL,
    `IsLinkedToOwner` tinyint  NOT NULL
);

-- Creating table 'ItemsSelled'

CREATE TABLE `ItemsSelled` (
    `Id` int AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `RecognizerType` longtext  NOT NULL,
    `ItemId` int  NULL,
    `Npc_NpcShopId` int  NULL,
    `Npc_CustomPrice` float  NULL,
    `Npc_BuyCriterion` longtext  NULL,
    `Npc_MaxStats` tinyint  NULL
);

-- Creating table 'ItemsSets'

CREATE TABLE `ItemsSets` (
    `Id` bigint  NOT NULL,
    `Items` varbinary(100)  NULL,
    `NameId` bigint  NULL,
    `BonusIsSecret` bool  NULL,
    `BonusEffects` varbinary(100)  NOT NULL
);

-- Creating table 'ItemsTemplates'

CREATE TABLE `ItemsTemplates` (
    `Id` int  NOT NULL,
    `RecognizerType` longtext  NOT NULL,
    `Weight` bigint  NULL,
    `RealWeight` bigint  NULL,
    `NameId` bigint  NULL,
    `TypeId` bigint  NULL,
    `DescriptionId` bigint  NULL,
    `IconId` bigint  NULL,
    `Level` bigint  NULL,
    `Cursed` bool  NULL,
    `UseAnimationId` int  NULL,
    `Usable` bool  NULL,
    `Targetable` bool  NULL,
    `Price` float  NULL,
    `TwoHanded` bool  NULL,
    `Etheral` bool  NULL,
    `ItemSetId` int  NULL,
    `Criteria` longtext  NULL,
    `HideEffects` bool  NULL,
    `AppearanceId` bigint  NULL,
    `BonusIsSecret` bool  NULL,
    `PossibleEffects` varbinary(100)  NULL,
    `FavoriteSubAreasBonus` bigint  NULL,
    `ApCost` int  NULL,
    `MinRange` int  NULL,
    `WeaponRange` int  NULL,
    `CastInLine` bool  NULL,
    `CastInDiagonal` bool  NULL,
    `CastTestLos` bool  NULL,
    `CriticalHitProbability` int  NULL,
    `CriticalHitBonus` int  NULL,
    `CriticalFailureProbability` int  NULL,
    `IsLinkedToOwner` tinyint  NOT NULL
);

-- Creating table 'ItemsTypes'

CREATE TABLE `ItemsTypes` (
    `Id` int  NOT NULL,
    `NameId` bigint  NULL,
    `SuperTypeId` bigint  NULL,
    `Plural` bool  NULL,
    `Gender` bigint  NULL,
    `RawZone` longtext  NULL,
    `NeedUseConfirm` bool  NULL
);

-- Creating table 'Jobs'

CREATE TABLE `Jobs` (
    `Id` int  NOT NULL,
    `NameId` bigint  NULL,
    `SpecializationOfId` int  NULL,
    `IconId` int  NULL,
    `ToolIds` varbinary(100)  NULL
);

-- Creating table 'Maps'

CREATE TABLE `Maps` (
    `Id` int  NOT NULL,
    `BlueCells` varbinary(100)  NULL,
    `RedCells` varbinary(100)  NULL,
    `Version` bigint  NULL,
    `RelativeId` bigint  NULL,
    `MapType` int  NULL,
    `SubAreaId` int  NULL,
    `TopNeighbourId` int  NULL,
    `BottomNeighbourId` int  NULL,
    `LeftNeighbourId` int  NULL,
    `RightNeighbourId` int  NULL,
    `ClientTopNeighbourId` int  NULL,
    `ClientBottomNeighbourId` int  NULL,
    `ClientLeftNeighbourId` int  NULL,
    `ClientRightNeighbourId` int  NULL,
    `ShadowBonusOnEntities` int  NULL,
    `UseLowpassFilter` bool  NULL,
    `UseReverb` bool  NULL,
    `PresetId` int  NULL,
    `CompressedCells` varbinary(100)  NOT NULL,
    `CompressedElements` varbinary(100)  NOT NULL
);

-- Creating table 'MapsCellsTriggers'

CREATE TABLE `MapsCellsTriggers` (
    `Id` int AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `RecognizerType` longtext  NOT NULL,
    `CellId` smallint  NULL,
    `MapId` int  NULL,
    `TriggerType` int  NULL,
    `Condition` longtext  NULL,
    `DestinationCellId` smallint  NULL,
    `DestinationMapId` int  NULL
);

-- Creating table 'MapsCoordinates'

CREATE TABLE `MapsCoordinates` (
    `Id` int AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `CompressedCoords` bigint  NULL,
    `Ids` varbinary(100)  NULL
);

-- Creating table 'MapsPositions'

CREATE TABLE `MapsPositions` (
    `Id` int  NOT NULL,
    `PosX` int  NULL,
    `PosY` int  NULL,
    `Outdoor` bool  NULL,
    `SubAreaId` int  NULL,
    `Capabilities` int  NULL,
    `WorldMap` int  NULL,
    `Sounds` varbinary(100)  NULL,
    `NameId` int  NULL,
    `HasPriorityOnWorldmap` bool  NULL
);

-- Creating table 'MapsReferences'

CREATE TABLE `MapsReferences` (
    `Id` int  NOT NULL,
    `MapId` bigint  NULL,
    `CellId` int  NULL
);

-- Creating table 'Monsters'

CREATE TABLE `Monsters` (
    `Id` int  NOT NULL,
    `NameId` bigint  NULL,
    `GfxId` bigint  NULL,
    `Race` int  NULL,
    `MinDroppedKamas` int  NULL,
    `MaxDroppedKamas` int  NULL,
    `Look` longtext  NULL,
    `UseSummonSlot` bool  NULL,
    `UseBombSlot` bool  NULL,
    `CanPlay` bool  NULL,
    `IsBoss` bool  NULL
);

-- Creating table 'MonstersDrops'

CREATE TABLE `MonstersDrops` (
    `Id` int AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `MonsterOwnerId` int  NOT NULL,
    `ItemId` smallint  NOT NULL,
    `DropLimit` int  NOT NULL,
    `DropRate` double  NOT NULL,
    `RollsCounter` int  NOT NULL,
    `ProspectingLock` int  NOT NULL
);

-- Creating table 'MonstersGrades'

CREATE TABLE `MonstersGrades` (
    `Id` int AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `Wisdom` int  NULL,
    `Grade` bigint  NULL,
    `GradeXp` int  NULL,
    `MonsterId` int  NULL,
    `Level` bigint  NULL,
    `PaDodge` int  NULL,
    `PmDodge` int  NULL,
    `EarthResistance` int  NULL,
    `AirResistance` int  NULL,
    `FireResistance` int  NULL,
    `WaterResistance` int  NULL,
    `NeutralResistance` int  NULL,
    `LifePoints` int  NULL,
    `ActionPoints` int  NULL,
    `MovementPoints` int  NULL,
    `TackleEvade` smallint  NULL,
    `TackleBlock` smallint  NULL,
    `Strength` smallint  NULL,
    `Chance` smallint  NULL,
    `Vitality` smallint  NULL,
    `Intelligence` smallint  NULL,
    `Agility` smallint  NULL,
    `Stats` varbinary(100)  NULL
);

-- Creating table 'MonstersRaces'

CREATE TABLE `MonstersRaces` (
    `Id` int  NOT NULL,
    `SuperRaceId` int  NULL,
    `NameId` bigint  NULL
);

-- Creating table 'MonstersSpawns'

CREATE TABLE `MonstersSpawns` (
    `Id` int AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `MapId` int  NULL,
    `SubAreaId` int  NULL,
    `MonsterId` int  NOT NULL,
    `Frenquency` double  NOT NULL,
    `MinGrade` int  NOT NULL,
    `MaxGrade` int  NOT NULL
);

-- Creating table 'MonstersSpawnsDungeon'

CREATE TABLE `MonstersSpawnsDungeon` (
    `Id` int AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `MapId` int  NOT NULL,
    `SerializedMonsterGroup` varbinary(100)  NOT NULL,
    `TeleportEvent` bool  NULL,
    `TeleportMapId` int  NULL,
    `TeleportCell` smallint  NULL,
    `Direction` int  NULL
);

-- Creating table 'MonstersSpells'

CREATE TABLE `MonstersSpells` (
    `Id` int AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `MonsterGradeId` int  NULL,
    `SpellId` int  NOT NULL,
    `Level` tinyint  NOT NULL
);

-- Creating table 'MonstersSuperRaces'

CREATE TABLE `MonstersSuperRaces` (
    `Id` int  NOT NULL,
    `NameId` bigint  NULL
);

-- Creating table 'Npcs'

CREATE TABLE `Npcs` (
    `Id` int  NOT NULL,
    `NameId` bigint  NULL,
    `DialogMessagesId` varbinary(100)  NULL,
    `DialogRepliesId` varbinary(100)  NULL,
    `ActionsId` varbinary(100)  NULL,
    `Gender` bigint  NULL,
    `Look` longtext  NULL,
    `SpecialArtworkId` smallint  NULL,
    `TokenShop` bool  NULL
);

-- Creating table 'NpcsActions'

CREATE TABLE `NpcsActions` (
    `Id` bigint AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `RecognizerType` longtext  NOT NULL,
    `Npc` int  NULL,
    `Condition` longtext  NULL,
    `Talk_MessageId` int  NULL,
    `Shop_Token` int  NULL,
    `Shop_CanSell` tinyint  NULL,
    `Shop_MaxStats` tinyint  NULL
);

-- Creating table 'NpcsMessages'

CREATE TABLE `NpcsMessages` (
    `Id` int  NOT NULL,
    `MessageId` bigint  NULL,
    `MessageParams` longtext  NULL
);

-- Creating table 'Npcs_Replies'

CREATE TABLE `Npcs_Replies` (
    `Id` int AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `RecognizerType` longtext  NOT NULL,
    `Reply` int  NULL,
    `Message` int  NULL,
    `Criteria` longtext  NULL,
    `Teleport_Map` int  NULL,
    `Teleport_Cell` int  NULL,
    `Teleport_Direction` int  NULL,
    `Dialog_NextMessageId` int  NULL,
    `UseItem_Item` int  NULL,
    `UseItem_Amount` bigint  NULL
);

-- Creating table 'NpcSpawns'

CREATE TABLE `NpcSpawns` (
    `Id` bigint AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `Npc` int  NULL,
    `Map` int  NOT NULL,
    `Cell` int  NOT NULL,
    `Direction` int  NOT NULL,
    `Look` longtext  NULL
);

-- Creating table 'Shortcuts'

CREATE TABLE `Shortcuts` (
    `Id` int AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `RecognizerType` longtext  NOT NULL,
    `Owner` int  NULL,
    `Slot` int  NULL,
    `SpellId` smallint  NULL,
    `ItemTemplateId` int  NULL,
    `ItemGuid` int  NULL
);

-- Creating table 'InteractiveSkillsTemplates'

CREATE TABLE `InteractiveSkillsTemplates` (
    `Id` int AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `NameId` bigint  NULL,
    `ParentJobId` int  NULL,
    `IsForgemagus` bool  NULL,
    `ModifiableItemType` int  NULL,
    `GatheredRessourceItem` int  NULL,
    `CraftableItemIdsBin` varbinary(100)  NULL,
    `InteractiveId` int  NULL,
    `UseAnimation` longtext  NULL,
    `IsRepair` bool  NULL,
    `Cursor` int  NULL,
    `AvailableInHouse` bool  NULL,
    `LevelMin` int  NULL
);

-- Creating table 'Spells'

CREATE TABLE `Spells` (
    `Id` int  NOT NULL,
    `NameId` bigint  NULL,
    `DescriptionId` bigint  NULL,
    `TypeId` bigint  NULL,
    `ScriptParams` longtext  NULL,
    `ScriptParamsCritical` longtext  NULL,
    `ScriptId` int  NULL,
    `ScriptIdCritical` int  NULL,
    `IconId` int  NULL,
    `SpellLevels` varbinary(100)  NULL,
    `UseParamCache` bool  NULL
);

-- Creating table 'SpellsBomb'

CREATE TABLE `SpellsBomb` (
    `Id` int  NOT NULL,
    `ChainReactionSpellId` int  NULL,
    `ExplodSpellId` int  NULL,
    `WallId` int  NULL,
    `InstantSpellId` int  NULL,
    `ComboCoeff` int  NULL
);

-- Creating table 'SpellsLevels'

CREATE TABLE `SpellsLevels` (
    `Id` bigint  NOT NULL,
    `SpellId` int  NULL,
    `SpellBreed` bigint  NULL,
    `ApCost` bigint  NULL,
    `Range` bigint  NULL,
    `CastInLine` bool  NULL,
    `CastInDiagonal` bool  NULL,
    `CastTestLos` bool  NULL,
    `CriticalHitProbability` bigint  NULL,
    `StatesRequired` varbinary(100)  NULL,
    `CriticalFailureProbability` bigint  NULL,
    `NeedFreeCell` bool  NULL,
    `NeedFreeTrapCell` bool  NULL,
    `NeedTakenCell` bool  NULL,
    `RangeCanBeBoosted` bool  NULL,
    `MaxStack` int  NULL,
    `MaxCastPerTurn` bigint  NULL,
    `MaxCastPerTarget` bigint  NULL,
    `MinCastInterval` bigint  NULL,
    `InitialCooldown` bigint  NULL,
    `GlobalCooldown` int  NULL,
    `MinPlayerLevel` bigint  NULL,
    `CriticalFailureEndsTurn` bool  NULL,
    `HideEffects` bool  NULL,
    `Hidden` bool  NULL,
    `MinRange` bigint  NULL,
    `StatesForbidden` varbinary(100)  NULL,
    `Effects` varbinary(100)  NULL,
    `CriticalEffect` varbinary(100)  NULL
);

-- Creating table 'SpellsStates'

CREATE TABLE `SpellsStates` (
    `Id` int  NOT NULL,
    `NameId` bigint  NULL,
    `PreventsSpellCast` bool  NULL,
    `PreventsFight` bool  NULL,
    `Critical` bool  NULL
);

-- Creating table 'SpellsTypes'

CREATE TABLE `SpellsTypes` (
    `Id` int  NOT NULL,
    `LongNameId` bigint  NULL,
    `ShortNameId` bigint  NULL
);

-- Creating table 'StartupActions'

CREATE TABLE `StartupActions` (
    `Id` int AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `Title` longtext  NOT NULL,
    `Text` longtext  NOT NULL,
    `DescUrl` longtext  NOT NULL,
    `PictureUrl` longtext  NOT NULL
);

-- Creating table 'StartupActionsObjects'

CREATE TABLE `StartupActionsObjects` (
    `Id` bigint AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `ItemTemplate` bigint  NOT NULL,
    `Amount` int  NOT NULL,
    `MaxEffects` bool  NOT NULL,
    `StartupActionId` int  NOT NULL
);

-- Creating table 'SubAreas'

CREATE TABLE `SubAreas` (
    `Id` int  NOT NULL,
    `NameId` bigint  NULL,
    `AreaId` int  NULL,
    `AmbientSounds` varbinary(100)  NULL,
    `MapIds` varbinary(100)  NULL,
    `Bounds` varbinary(100)  NULL,
    `Shape` varbinary(100)  NULL,
    `CustomWorldMap` varbinary(100)  NULL,
    `PackId` int  NULL,
    `Difficulty` tinyint unsigned  NOT NULL,
    `SpawnsLimit` int  NULL,
    `CustomSpawnInterval` bigint  NULL
);

-- Creating table 'SuperAreas'

CREATE TABLE `SuperAreas` (
    `Id` int  NOT NULL,
    `NameId` bigint  NULL,
    `WorldmapId` bigint  NULL
);

-- Creating table 'Texts'

CREATE TABLE `Texts` (
    `Id` bigint AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `French` longtext  NULL,
    `English` longtext  NULL,
    `German` longtext  NULL,
    `Spanish` longtext  NULL,
    `Italian` longtext  NULL,
    `Japanish` longtext  NULL,
    `Dutsh` longtext  NULL,
    `Portugese` longtext  NULL,
    `Russish` longtext  NULL
);

-- Creating table 'TextsUI'

CREATE TABLE `TextsUI` (
    `Id` bigint AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `Name` longtext  NULL,
    `French` longtext  NULL,
    `English` longtext  NULL,
    `German` longtext  NULL,
    `Spanish` longtext  NULL,
    `Italian` longtext  NULL,
    `Japanish` longtext  NULL,
    `Dutsh` longtext  NULL,
    `Portugese` longtext  NULL,
    `Russish` longtext  NULL
);

-- Creating table 'versions'

CREATE TABLE `versions` (
    `Revision` bigint  NOT NULL,
    `UpdateDate` datetime  NULL,
    `DofusVersion` longtext  NULL
);

-- Creating table 'WorldMaps'

CREATE TABLE `WorldMaps` (
    `Id` int  NOT NULL,
    `OrigineX` int  NULL,
    `OrigineY` int  NULL,
    `MapWidth` double  NULL,
    `MapHeight` double  NULL,
    `HorizontalChunck` bigint  NULL,
    `VerticalChunck` bigint  NULL,
    `ViewableEverywhere` bool  NULL,
    `MinScale` double  NULL,
    `MaxScale` double  NULL,
    `StartScale` double  NULL,
    `CenterX` int  NULL,
    `CenterY` int  NULL,
    `TotalWidth` int  NULL,
    `TotalHeight` int  NULL,
    `Zoom` varbinary(100)  NULL
);

-- Creating table 'InteractivesSkills_SkillTemplateDependantRecord'

CREATE TABLE `InteractivesSkills_SkillTemplateDependantRecord` (
    `Id` int  NOT NULL
);

-- Creating table 'InteractivesSkills_SkillTeleportRecord'

CREATE TABLE `InteractivesSkills_SkillTeleportRecord` (
    `MapId` int  NULL,
    `CellId` int  NULL,
    `Direction` int  NULL,
    `Condition` longtext  NULL,
    `Id` int  NOT NULL
);

-- Creating table 'InteractivesSkills_SkillZaapTeleportRecord'

CREATE TABLE `InteractivesSkills_SkillZaapTeleportRecord` (
    `Id` int  NOT NULL
);

-- Creating table 'InteractivesSkills_SkillZaapSaveRecord'

CREATE TABLE `InteractivesSkills_SkillZaapSaveRecord` (
    `Id` int  NOT NULL
);

-- Creating table 'WorldAccountFriends'

CREATE TABLE `WorldAccountFriends` (
    `Friends_Id` int  NOT NULL,
    `FriendWith_Id` int  NOT NULL
);

-- Creating table 'WorldAccountIgnoreds'

CREATE TABLE `WorldAccountIgnoreds` (
    `IgnoredAccounts_Id` int  NOT NULL,
    `IgnoredBy_Id` int  NOT NULL
);

-- Creating table 'WorldAccountStartupActions'

CREATE TABLE `WorldAccountStartupActions` (
    `Accounts_Id` int  NOT NULL,
    `StartupActions_Id` int  NOT NULL
);

-- Creating table 'SpawnInteractiveSkills'

CREATE TABLE `SpawnInteractiveSkills` (
    `SpawnInteractiveSkill_Skill_Id` int  NOT NULL,
    `CustomSkills_Id` int  NOT NULL
);



-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on `Id` in table 'Accounts'

ALTER TABLE `Accounts`
ADD CONSTRAINT `PK_Accounts`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'Areas'

ALTER TABLE `Areas`
ADD CONSTRAINT `PK_Areas`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'Breeds'

ALTER TABLE `Breeds`
ADD CONSTRAINT `PK_Breeds`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'BreedItems'

ALTER TABLE `BreedItems`
ADD CONSTRAINT `PK_BreedItems`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'CharacterJobs'

ALTER TABLE `CharacterJobs`
ADD CONSTRAINT `PK_CharacterJobs`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'Effects'

ALTER TABLE `Effects`
ADD CONSTRAINT `PK_Effects`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Level` in table 'ExperiencesEntries'

ALTER TABLE `ExperiencesEntries`
ADD CONSTRAINT `PK_ExperiencesEntries`
    PRIMARY KEY (`Level` );

-- Creating primary key on `Id` in table 'Hints'

ALTER TABLE `Hints`
ADD CONSTRAINT `PK_Hints`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'Interactives'

ALTER TABLE `Interactives`
ADD CONSTRAINT `PK_Interactives`
    PRIMARY KEY (`Id` );

-- Creating primary key on `InteractiveId`, `SkillId` in table 'InteractivesCustomSkills'

ALTER TABLE `InteractivesCustomSkills`
ADD CONSTRAINT `PK_InteractivesCustomSkills`
    PRIMARY KEY (`InteractiveId`, `SkillId` );

-- Creating primary key on `InteractiveId`, `SkillId` in table 'InteractivesTemplatesSkills'

ALTER TABLE `InteractivesTemplatesSkills`
ADD CONSTRAINT `PK_InteractivesTemplatesSkills`
    PRIMARY KEY (`InteractiveId`, `SkillId` );

-- Creating primary key on `Id` in table 'Items'

ALTER TABLE `Items`
ADD CONSTRAINT `PK_Items`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'ItemsSets'

ALTER TABLE `ItemsSets`
ADD CONSTRAINT `PK_ItemsSets`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'ItemsTemplates'

ALTER TABLE `ItemsTemplates`
ADD CONSTRAINT `PK_ItemsTemplates`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'ItemsTypes'

ALTER TABLE `ItemsTypes`
ADD CONSTRAINT `PK_ItemsTypes`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'Jobs'

ALTER TABLE `Jobs`
ADD CONSTRAINT `PK_Jobs`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'Maps'

ALTER TABLE `Maps`
ADD CONSTRAINT `PK_Maps`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'MapsPositions'

ALTER TABLE `MapsPositions`
ADD CONSTRAINT `PK_MapsPositions`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'MapsReferences'

ALTER TABLE `MapsReferences`
ADD CONSTRAINT `PK_MapsReferences`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'Monsters'

ALTER TABLE `Monsters`
ADD CONSTRAINT `PK_Monsters`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'MonstersRaces'

ALTER TABLE `MonstersRaces`
ADD CONSTRAINT `PK_MonstersRaces`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'MonstersSuperRaces'

ALTER TABLE `MonstersSuperRaces`
ADD CONSTRAINT `PK_MonstersSuperRaces`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'Npcs'

ALTER TABLE `Npcs`
ADD CONSTRAINT `PK_Npcs`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'NpcsMessages'

ALTER TABLE `NpcsMessages`
ADD CONSTRAINT `PK_NpcsMessages`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'Spells'

ALTER TABLE `Spells`
ADD CONSTRAINT `PK_Spells`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'SpellsBomb'

ALTER TABLE `SpellsBomb`
ADD CONSTRAINT `PK_SpellsBomb`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'SpellsLevels'

ALTER TABLE `SpellsLevels`
ADD CONSTRAINT `PK_SpellsLevels`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'SpellsStates'

ALTER TABLE `SpellsStates`
ADD CONSTRAINT `PK_SpellsStates`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'SpellsTypes'

ALTER TABLE `SpellsTypes`
ADD CONSTRAINT `PK_SpellsTypes`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'SubAreas'

ALTER TABLE `SubAreas`
ADD CONSTRAINT `PK_SubAreas`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'SuperAreas'

ALTER TABLE `SuperAreas`
ADD CONSTRAINT `PK_SuperAreas`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Revision` in table 'versions'

ALTER TABLE `versions`
ADD CONSTRAINT `PK_versions`
    PRIMARY KEY (`Revision` );

-- Creating primary key on `Id` in table 'WorldMaps'

ALTER TABLE `WorldMaps`
ADD CONSTRAINT `PK_WorldMaps`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'InteractivesSkills_SkillTemplateDependantRecord'

ALTER TABLE `InteractivesSkills_SkillTemplateDependantRecord`
ADD CONSTRAINT `PK_InteractivesSkills_SkillTemplateDependantRecord`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'InteractivesSkills_SkillTeleportRecord'

ALTER TABLE `InteractivesSkills_SkillTeleportRecord`
ADD CONSTRAINT `PK_InteractivesSkills_SkillTeleportRecord`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'InteractivesSkills_SkillZaapTeleportRecord'

ALTER TABLE `InteractivesSkills_SkillZaapTeleportRecord`
ADD CONSTRAINT `PK_InteractivesSkills_SkillZaapTeleportRecord`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Id` in table 'InteractivesSkills_SkillZaapSaveRecord'

ALTER TABLE `InteractivesSkills_SkillZaapSaveRecord`
ADD CONSTRAINT `PK_InteractivesSkills_SkillZaapSaveRecord`
    PRIMARY KEY (`Id` );

-- Creating primary key on `Friends_Id`, `FriendWith_Id` in table 'WorldAccountFriends'

ALTER TABLE `WorldAccountFriends`
ADD CONSTRAINT `PK_WorldAccountFriends`
    PRIMARY KEY (`Friends_Id`, `FriendWith_Id` );

-- Creating primary key on `IgnoredAccounts_Id`, `IgnoredBy_Id` in table 'WorldAccountIgnoreds'

ALTER TABLE `WorldAccountIgnoreds`
ADD CONSTRAINT `PK_WorldAccountIgnoreds`
    PRIMARY KEY (`IgnoredAccounts_Id`, `IgnoredBy_Id` );

-- Creating primary key on `Accounts_Id`, `StartupActions_Id` in table 'WorldAccountStartupActions'

ALTER TABLE `WorldAccountStartupActions`
ADD CONSTRAINT `PK_WorldAccountStartupActions`
    PRIMARY KEY (`Accounts_Id`, `StartupActions_Id` );

-- Creating primary key on `SpawnInteractiveSkill_Skill_Id`, `CustomSkills_Id` in table 'SpawnInteractiveSkills'

ALTER TABLE `SpawnInteractiveSkills`
ADD CONSTRAINT `PK_SpawnInteractiveSkills`
    PRIMARY KEY (`SpawnInteractiveSkill_Skill_Id`, `CustomSkills_Id` );



-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on `Friends_Id` in table 'WorldAccountFriends'

ALTER TABLE `WorldAccountFriends`
ADD CONSTRAINT `FK_WorldAccountFriend_WorldAccount`
    FOREIGN KEY (`Friends_Id`)
    REFERENCES `Accounts`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating foreign key on `FriendWith_Id` in table 'WorldAccountFriends'

ALTER TABLE `WorldAccountFriends`
ADD CONSTRAINT `FK_WorldAccountFriend_Friend`
    FOREIGN KEY (`FriendWith_Id`)
    REFERENCES `Accounts`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_WorldAccountFriend_Friend'

CREATE INDEX `IX_FK_WorldAccountFriend_Friend` 
    ON `WorldAccountFriends`
    (`FriendWith_Id`);

-- Creating foreign key on `IgnoredAccounts_Id` in table 'WorldAccountIgnoreds'

ALTER TABLE `WorldAccountIgnoreds`
ADD CONSTRAINT `FK_WorldAccountIgnored_WorldAccount`
    FOREIGN KEY (`IgnoredAccounts_Id`)
    REFERENCES `Accounts`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating foreign key on `IgnoredBy_Id` in table 'WorldAccountIgnoreds'

ALTER TABLE `WorldAccountIgnoreds`
ADD CONSTRAINT `FK_WorldAccountIgnored_Ignored`
    FOREIGN KEY (`IgnoredBy_Id`)
    REFERENCES `Accounts`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_WorldAccountIgnored_Ignored'

CREATE INDEX `IX_FK_WorldAccountIgnored_Ignored` 
    ON `WorldAccountIgnoreds`
    (`IgnoredBy_Id`);

-- Creating foreign key on `Accounts_Id` in table 'WorldAccountStartupActions'

ALTER TABLE `WorldAccountStartupActions`
ADD CONSTRAINT `FK_WorldAccountStartupAction_WorldAccount`
    FOREIGN KEY (`Accounts_Id`)
    REFERENCES `Accounts`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating foreign key on `StartupActions_Id` in table 'WorldAccountStartupActions'

ALTER TABLE `WorldAccountStartupActions`
ADD CONSTRAINT `FK_WorldAccountStartupAction_StartupAction`
    FOREIGN KEY (`StartupActions_Id`)
    REFERENCES `StartupActions`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_WorldAccountStartupAction_StartupAction'

CREATE INDEX `IX_FK_WorldAccountStartupAction_StartupAction` 
    ON `WorldAccountStartupActions`
    (`StartupActions_Id`);

-- Creating foreign key on `StartupActionId` in table 'StartupActionsObjects'

ALTER TABLE `StartupActionsObjects`
ADD CONSTRAINT `FK_StartupActionStartupActionObject`
    FOREIGN KEY (`StartupActionId`)
    REFERENCES `StartupActions`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_StartupActionStartupActionObject'

CREATE INDEX `IX_FK_StartupActionStartupActionObject` 
    ON `StartupActionsObjects`
    (`StartupActionId`);

-- Creating foreign key on `BreedId` in table 'BreedItems'

ALTER TABLE `BreedItems`
ADD CONSTRAINT `FK_BreedBreedItem`
    FOREIGN KEY (`BreedId`)
    REFERENCES `Breeds`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BreedBreedItem'

CREATE INDEX `IX_FK_BreedBreedItem` 
    ON `BreedItems`
    (`BreedId`);

-- Creating foreign key on `BreedId` in table 'BreedSpells'

ALTER TABLE `BreedSpells`
ADD CONSTRAINT `FK_BreedBreedSpell`
    FOREIGN KEY (`BreedId`)
    REFERENCES `Breeds`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_BreedBreedSpell'

CREATE INDEX `IX_FK_BreedBreedSpell` 
    ON `BreedSpells`
    (`BreedId`);

-- Creating foreign key on `OwnerId` in table 'CharacterSpells'

ALTER TABLE `CharacterSpells`
ADD CONSTRAINT `FK_CharacterRecordCharacterSpell`
    FOREIGN KEY (`OwnerId`)
    REFERENCES `Characters`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CharacterRecordCharacterSpell'

CREATE INDEX `IX_FK_CharacterRecordCharacterSpell` 
    ON `CharacterSpells`
    (`OwnerId`);

-- Creating foreign key on `SpawnInteractiveSkill_Skill_Id` in table 'SpawnInteractiveSkills'

ALTER TABLE `SpawnInteractiveSkills`
ADD CONSTRAINT `FK_SpawnInteractiveSkill_InteractiveSpawn`
    FOREIGN KEY (`SpawnInteractiveSkill_Skill_Id`)
    REFERENCES `InteractivesSpawns`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating foreign key on `CustomSkills_Id` in table 'SpawnInteractiveSkills'

ALTER TABLE `SpawnInteractiveSkills`
ADD CONSTRAINT `FK_SpawnInteractiveSkill_Skill`
    FOREIGN KEY (`CustomSkills_Id`)
    REFERENCES `InteractivesSkills`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SpawnInteractiveSkill_Skill'

CREATE INDEX `IX_FK_SpawnInteractiveSkill_Skill` 
    ON `SpawnInteractiveSkills`
    (`CustomSkills_Id`);

-- Creating foreign key on `Id` in table 'InteractivesSkills_SkillTemplateDependantRecord'

ALTER TABLE `InteractivesSkills_SkillTemplateDependantRecord`
ADD CONSTRAINT `FK_SkillTemplateDependantRecord_inherits_SkillRecord`
    FOREIGN KEY (`Id`)
    REFERENCES `InteractivesSkills`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating foreign key on `Id` in table 'InteractivesSkills_SkillTeleportRecord'

ALTER TABLE `InteractivesSkills_SkillTeleportRecord`
ADD CONSTRAINT `FK_SkillTeleportRecord_inherits_SkillTemplateDependantRecord`
    FOREIGN KEY (`Id`)
    REFERENCES `InteractivesSkills_SkillTemplateDependantRecord`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating foreign key on `Id` in table 'InteractivesSkills_SkillZaapTeleportRecord'

ALTER TABLE `InteractivesSkills_SkillZaapTeleportRecord`
ADD CONSTRAINT `FK_SkillZaapTeleportRecord_inherits_SkillTemplateDependantRecord`
    FOREIGN KEY (`Id`)
    REFERENCES `InteractivesSkills_SkillTemplateDependantRecord`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating foreign key on `Id` in table 'InteractivesSkills_SkillZaapSaveRecord'

ALTER TABLE `InteractivesSkills_SkillZaapSaveRecord`
ADD CONSTRAINT `FK_SkillZaapSaveRecord_inherits_SkillTemplateDependantRecord`
    FOREIGN KEY (`Id`)
    REFERENCES `InteractivesSkills_SkillTemplateDependantRecord`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
