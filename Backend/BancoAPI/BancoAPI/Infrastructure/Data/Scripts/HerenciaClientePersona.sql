-- Script para implementar herencia Cliente -> Persona
-- Paso 1: Agregar columna ClienteId y llenarla con el valor actual de Id
USE railway;

-- Agregar columna ClienteId 
ALTER TABLE Clientes ADD COLUMN ClienteId BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY AFTER Id;

-- Copiar datos existentes de Id a ClienteId (temporalmente)
UPDATE Clientes SET ClienteId = Id;

-- Paso 2: Actualizar foreign keys para usar ClienteId
-- Primero deshabilitamos foreign key checks
SET FOREIGN_KEY_CHECKS = 0;

-- Actualizar tabla Cuentas para referenciar ClienteId en lugar de ClienteId (el FK actual)
-- Note: Esto asume que ya tienes ClienteId en Cuentas - si no es así, necesitas agregarlo primero

-- Si necesitas crear la columna ClienteId en Cuentas (verificar primero si existe)
-- ALTER TABLE Cuentas ADD COLUMN ClienteId BIGINT;
-- UPDATE Cuentas SET ClienteId = (SELECT Id FROM Clientes WHERE Clientes.Id = Cuentas.ClienteId);

-- Rehabilitar foreign key checks
SET FOREIGN_KEY_CHECKS = 1;

-- Paso 3: Agregar nuevas foreign keys
ALTER TABLE Clientes 
ADD CONSTRAINT FK_Clientes_Personas_Id 
FOREIGN KEY (Id) REFERENCES Personas(Id) ON DELETE CASCADE;

-- Opcional: Quitar la columna PersonaId si ya no se necesita
-- ALTER TABLE Clientes DROP COLUMN PersonaId;

SELECT 'Migración completada exitosamente' AS status;