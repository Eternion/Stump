ALTER TABLE `accounts`
CHANGE COLUMN `Password` `PasswordHash`  varchar(32) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL AFTER `Login`;
UPDATE accounts SET `PasswordHash`= MD5(`PasswordHash`);