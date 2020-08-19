CREATE USER 'hungrypizza'@'localhost' IDENTIFIED BY 'hungry123';
GRANT ALTER, SHOW VIEW, SHOW DATABASES, SELECT, PROCESS, EXECUTE, CREATE ROUTINE, CREATE, ALTER ROUTINE, CREATE VIEW, CREATE TEMPORARY TABLES, CREATE TABLESPACE, EVENT, DROP, DELETE, REFERENCES, INSERT, INDEX, CREATE USER, UPDATE, TRIGGER, LOCK TABLES, FILE, REPLICATION SLAVE, REPLICATION CLIENT, RELOAD, SUPER, SHUTDOWN  ON *.* TO 'hungrypizza'@'localhost' WITH GRANT OPTION;
FLUSH PRIVILEGES;


-- Copiando estrutura do banco de dados para hungrypizza
CREATE DATABASE IF NOT EXISTS `hungrypizza` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `hungrypizza`;

CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(95) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
);

CREATE TABLE `Clients` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(60) CHARACTER SET utf8mb4 NOT NULL,
    `CEP` varchar(60) CHARACTER SET utf8mb4 NOT NULL,
    `Address` varchar(60) CHARACTER SET utf8mb4 NOT NULL,
    `PhoneNumber` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Clients` PRIMARY KEY (`Id`)
);

CREATE TABLE `Flavours` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` longtext CHARACTER SET utf8mb4 NULL,
    `Price` decimal(65,30) NOT NULL,
    CONSTRAINT `PK_Flavours` PRIMARY KEY (`Id`)
);

CREATE TABLE `Orders` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `ClientId` int NOT NULL,
    `Address` longtext CHARACTER SET utf8mb4 NULL,
    `Date` DateTime NOT NULL,
    `Total` decimal(65,30) NOT NULL,
    CONSTRAINT `PK_Orders` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Orders_Clients_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `Clients` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `Pizzas` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Price` decimal(65,30) NOT NULL,
    `OrderId` int NULL,
    CONSTRAINT `PK_Pizzas` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Pizzas_Orders_OrderId` FOREIGN KEY (`OrderId`) REFERENCES `Orders` (`Id`) ON DELETE RESTRICT
);

CREATE TABLE `PizzaFlavours` (
    `PizzaId` int NOT NULL,
    `FlavourId` int NOT NULL,
    CONSTRAINT `PK_PizzaFlavours` PRIMARY KEY (`PizzaId`, `FlavourId`),
    CONSTRAINT `FK_PizzaFlavours_Flavours_FlavourId` FOREIGN KEY (`FlavourId`) REFERENCES `Flavours` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_PizzaFlavours_Pizzas_PizzaId` FOREIGN KEY (`PizzaId`) REFERENCES `Pizzas` (`Id`) ON DELETE CASCADE
);

CREATE INDEX `IX_Orders_ClientId` ON `Orders` (`ClientId`);

CREATE INDEX `IX_PizzaFlavours_FlavourId` ON `PizzaFlavours` (`FlavourId`);

CREATE INDEX `IX_Pizzas_OrderId` ON `Pizzas` (`OrderId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20200817223957_InitialMigration', '3.1.7');

INSERT INTO `Flavours` (`Name`, `Price`)
VALUES  ('3 Queijos', 50),
		('Frango com requeijão', 59.99),
		('Mussarela', 42.5),
		('Calabresa', 42.5),
		('Pepperoni', 55),
		('Portuguesa', 45),
		('Veggie', 59.99);
		
INSERT INTO `Clients` (`Name`, `CEP`, `Address`, `PhoneNumber`)
VALUES  ('Federico Alonso Noguera', '88034-100', 'Rua Pastor William Schisler Filho 1128', '+5548996867968');