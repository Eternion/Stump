



-- -----------------------------------------------------------
-- Entity Designer DDL Script for MySQL Server 4.1 and higher
-- -----------------------------------------------------------
-- Date Created: 06/16/2012 23:47:58
-- Generated from EDMX file: C:\Users\Bouh2\Desktop\Programming\C#\Project Stump (git)\trunk\Server\Stump.Server.AuthServer\Database\Model.edmx
-- Target version: 2.0.0.0
-- --------------------------------------------------

DROP DATABASE IF EXISTS `stump_auth`;
CREATE DATABASE `stump_auth`;
USE `stump_auth`;

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- NOTE: if the constraint does not exist, an ignorable error will be reported.
-- --------------------------------------------------

--    ALTER TABLE `Subscriptions` DROP CONSTRAINT `FK_AccountSubscription`;
--    ALTER TABLE `Sanctions` DROP CONSTRAINT `FK_AccountSanction`;
--    ALTER TABLE `WorldCharacters` DROP CONSTRAINT `FK_AccountWorldCharacter`;
--    ALTER TABLE `WorldCharactersDeleted` DROP CONSTRAINT `FK_AccountWorldCharacterDeleted`;
--    ALTER TABLE `Connections` DROP CONSTRAINT `FK_AccountConnection`;

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------
SET foreign_key_checks = 0;
    DROP TABLE IF EXISTS `Accounts`;
    DROP TABLE IF EXISTS `Connections`;
    DROP TABLE IF EXISTS `IpBans`;
    DROP TABLE IF EXISTS `Sanctions`;
    DROP TABLE IF EXISTS `Subscriptions`;
    DROP TABLE IF EXISTS `Texts`;
    DROP TABLE IF EXISTS `TextsUI`;
    DROP TABLE IF EXISTS `Worlds`;
    DROP TABLE IF EXISTS `WorldCharacters`;
    DROP TABLE IF EXISTS `WorldCharactersDeleted`;
SET foreign_key_checks = 1;

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Accounts'

CREATE TABLE `Accounts` (
    `Id` int AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `Login` longtext  NOT NULL,
    `PasswordHash` longtext  NOT NULL,
    `Nickname` longtext  NOT NULL,
    `RoleAsInt` int  NOT NULL,
    `AvailableBreedsFlag` bigint  NOT NULL,
    `Ticket` longtext  NULL,
    `SecretQuestion` longtext  NOT NULL,
    `SecretAnswer` longtext  NOT NULL,
    `Lang` longtext  NOT NULL,
    `Email` longtext  NOT NULL,
    `CreationDate` datetime  NOT NULL,
    `Tokens` int  NOT NULL,
    `NewTokens` int  NOT NULL,
    `LastVote` datetime  NULL,
    `RecordVersion` int  NULL
);

-- Creating table 'Connections'

CREATE TABLE `Connections` (
    `Id` bigint AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `Date` datetime  NOT NULL,
    `Ip` longtext  NOT NULL,
    `AccountId` int  NOT NULL,
    `WorldId` int  NULL
);

-- Creating table 'IpBans'

CREATE TABLE `IpBans` (
    `Id` bigint AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `IPAsString` longtext  NOT NULL,
    `Date` datetime  NOT NULL,
    `Duration` time  NULL,
    `BanReason` longtext  NULL,
    `BannedBy` int  NULL
);

-- Creating table 'Sanctions'

CREATE TABLE `Sanctions` (
    `Id` bigint AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `Date` datetime  NOT NULL,
    `Duration` time  NULL,
    `BanReason` longtext  NULL,
    `AccountId` int  NOT NULL,
    `BannedBy` int  NULL
);

-- Creating table 'Subscriptions'

CREATE TABLE `Subscriptions` (
    `Id` bigint AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `BuyDate` datetime  NOT NULL,
    `Duration` time  NULL,
    `PaymentType` longtext  NULL,
    `AccountId` int  NOT NULL
);

-- Creating table 'Texts'

CREATE TABLE `Texts` (
    `Id` int AUTO_INCREMENT PRIMARY KEY NOT NULL,
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
    `Id` int AUTO_INCREMENT PRIMARY KEY NOT NULL,
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

-- Creating table 'Worlds'

CREATE TABLE `Worlds` (
    `Id` int  NOT NULL,
    `Name` longtext  NOT NULL,
    `RequireSubscription` bool  NOT NULL,
    `RequiredRoleAsInt` int  NOT NULL,
    `Completion` int  NOT NULL,
    `ServerSelectable` bool  NOT NULL,
    `CharCapacity` int  NOT NULL,
    `StatusAsInt` int  NOT NULL,
    `CharsCount` int  NULL
);

-- Creating table 'WorldCharacters'

CREATE TABLE `WorldCharacters` (
    `Id` bigint AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `CharacterId` int  NOT NULL,
    `AccountId` int  NOT NULL,
    `WorldId` int  NOT NULL
);

-- Creating table 'WorldCharactersDeleted'

CREATE TABLE `WorldCharactersDeleted` (
    `Id` bigint AUTO_INCREMENT PRIMARY KEY NOT NULL,
    `CharacterId` int  NOT NULL,
    `DeletionDate` datetime  NOT NULL,
    `AccountId` int  NOT NULL,
    `WorldId` int  NOT NULL
);



-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on `Id` in table 'Worlds'

ALTER TABLE `Worlds`
ADD CONSTRAINT `PK_Worlds`
    PRIMARY KEY (`Id` );



-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on `AccountId` in table 'Subscriptions'

ALTER TABLE `Subscriptions`
ADD CONSTRAINT `FK_AccountSubscription`
    FOREIGN KEY (`AccountId`)
    REFERENCES `Accounts`
        (`Id`)
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountSubscription'

CREATE INDEX `IX_FK_AccountSubscription` 
    ON `Subscriptions`
    (`AccountId`);

-- Creating foreign key on `AccountId` in table 'Sanctions'

ALTER TABLE `Sanctions`
ADD CONSTRAINT `FK_AccountSanction`
    FOREIGN KEY (`AccountId`)
    REFERENCES `Accounts`
        (`Id`)
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountSanction'

CREATE INDEX `IX_FK_AccountSanction` 
    ON `Sanctions`
    (`AccountId`);

-- Creating foreign key on `AccountId` in table 'WorldCharacters'

ALTER TABLE `WorldCharacters`
ADD CONSTRAINT `FK_AccountWorldCharacter`
    FOREIGN KEY (`AccountId`)
    REFERENCES `Accounts`
        (`Id`)
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountWorldCharacter'

CREATE INDEX `IX_FK_AccountWorldCharacter` 
    ON `WorldCharacters`
    (`AccountId`);

-- Creating foreign key on `AccountId` in table 'WorldCharactersDeleted'

ALTER TABLE `WorldCharactersDeleted`
ADD CONSTRAINT `FK_AccountWorldCharacterDeleted`
    FOREIGN KEY (`AccountId`)
    REFERENCES `Accounts`
        (`Id`)
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountWorldCharacterDeleted'

CREATE INDEX `IX_FK_AccountWorldCharacterDeleted` 
    ON `WorldCharactersDeleted`
    (`AccountId`);

-- Creating foreign key on `AccountId` in table 'Connections'

ALTER TABLE `Connections`
ADD CONSTRAINT `FK_AccountConnection`
    FOREIGN KEY (`AccountId`)
    REFERENCES `Accounts`
        (`Id`)
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountConnection'

CREATE INDEX `IX_FK_AccountConnection` 
    ON `Connections`
    (`AccountId`);

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
