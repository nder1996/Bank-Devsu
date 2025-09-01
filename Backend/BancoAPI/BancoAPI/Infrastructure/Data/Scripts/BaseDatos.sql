CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;
ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Personas` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Nombre` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Genero` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `Edad` int NOT NULL,
    `Identificacion` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `Direccion` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Telefono` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Personas` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Clientes` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Contrasena` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Estado` tinyint(1) NOT NULL,
    `PersonaId` bigint NOT NULL,
    CONSTRAINT `PK_Clientes` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Clientes_Personas_PersonaId` FOREIGN KEY (`PersonaId`) REFERENCES `Personas` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Cuentas` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `NumeroCuenta` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `TipoCuenta` int NOT NULL,
    `SaldoInicial` decimal(18,2) NOT NULL,
    `Estado` tinyint(1) NOT NULL,
    `ClienteId` bigint NOT NULL,
    CONSTRAINT `PK_Cuentas` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Cuentas_Clientes_ClienteId` FOREIGN KEY (`ClienteId`) REFERENCES `Clientes` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Movimientos` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `Fecha` datetime(6) NOT NULL,
    `TipoMovimiento` int NOT NULL,
    `Valor` decimal(18,2) NOT NULL,
    `Saldo` decimal(18,2) NOT NULL,
    `CuentaId` bigint NOT NULL,
    CONSTRAINT `PK_Movimientos` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Movimientos_Cuentas_CuentaId` FOREIGN KEY (`CuentaId`) REFERENCES `Cuentas` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_Clientes_PersonaId` ON `Clientes` (`PersonaId`);

CREATE INDEX `IX_Cuentas_ClienteId` ON `Cuentas` (`ClienteId`);

CREATE INDEX `IX_Movimientos_CuentaId` ON `Movimientos` (`CuentaId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250821215452_InitialCreate', '9.0.8');

CREATE TABLE `Reportes` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `ClienteId` bigint NOT NULL,
    `FechaInicio` datetime(6) NOT NULL,
    `FechaFin` datetime(6) NOT NULL,
    `Formato` int NOT NULL,
    `FechaGeneracion` datetime(6) NOT NULL,
    `RutaArchivo` varchar(500) CHARACTER SET utf8mb4 NULL,
    `NombreArchivo` varchar(100) CHARACTER SET utf8mb4 NULL,
    `Activo` tinyint(1) NOT NULL,
    CONSTRAINT `PK_Reportes` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Reportes_Clientes_ClienteId` FOREIGN KEY (`ClienteId`) REFERENCES `Clientes` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_Reportes_ClienteId` ON `Reportes` (`ClienteId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250830221605_AddReportesTable', '9.0.8');

COMMIT;

