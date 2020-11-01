-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `mydb` DEFAULT CHARACTER SET utf8 ;
-- -----------------------------------------------------
-- Schema restaurante
-- -----------------------------------------------------
USE `mydb` ;

-- -----------------------------------------------------
-- Table `mydb`.`Administrador`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Administrador` (
  `DNIl` VARCHAR(9) NOT NULL,
  `IdAdministrador` INT NOT NULL,
  `Nombre` VARCHAR(20) NULL,
  `Apellidos` VARCHAR(50) NULL,
  `NumTelefono` VARCHAR(15) NULL,
  UNIQUE INDEX `IdAdministrador_UNIQUE` (`IdAdministrador` ASC) VISIBLE,
  PRIMARY KEY (`DNIl`),
  UNIQUE INDEX `DNIl_UNIQUE` (`DNIl` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Arbitro`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Arbitro` (
  `DNIl` VARCHAR(9) NOT NULL,
  `NumLicencia` INT NOT NULL,
  `Alias` VARCHAR(20) NOT NULL,
  `Nombre` VARCHAR(20) NOT NULL,
  `Apellidos` VARCHAR(50) NULL,
  `NumTelefono` VARCHAR(15) NOT NULL,
  UNIQUE INDEX `IdAdministrador_UNIQUE` (`NumLicencia` ASC) VISIBLE,
  PRIMARY KEY (`NumLicencia`),
  UNIQUE INDEX `DNIl_UNIQUE` (`DNIl` ASC) VISIBLE,
  UNIQUE INDEX `Alias_UNIQUE` (`Alias` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Nomina`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Nomina` (
  `idNomina` INT NOT NULL,
  `Fecha` DATETIME NOT NULL,
  `Total` DECIMAL NOT NULL,
  `Arbitro_NumLicencia` INT NOT NULL,
  PRIMARY KEY (`idNomina`),
  UNIQUE INDEX `idNomina_UNIQUE` (`idNomina` ASC) VISIBLE,
  INDEX `fk_Nomina_Arbitro1_idx` (`Arbitro_NumLicencia` ASC) VISIBLE,
  CONSTRAINT `fk_Nomina_Arbitro1`
    FOREIGN KEY (`Arbitro_NumLicencia`)
    REFERENCES `mydb`.`Arbitro` (`NumLicencia`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Categoria`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Categoria` (
  `IdCategoria` INT NOT NULL,
  `Descripcion` VARCHAR(45) NOT NULL,
  `TarifaArb1` DECIMAL NOT NULL,
  `TarifaArb2` DECIMAL NULL,
  `TarifaArb3` DECIMAL NULL,
  `TarifaAnotador` DECIMAL NOT NULL,
  `TarifaCrono` DECIMAL NULL,
  `Tarifa24` DECIMAL NULL,
  PRIMARY KEY (`IdCategoria`),
  UNIQUE INDEX `IdTarifa_UNIQUE` (`IdCategoria` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Partido`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Partido` (
  `IdPartido` INT NOT NULL,
  `Fecha` DATETIME NOT NULL,
  `EquipoLocal` VARCHAR(45) NOT NULL,
  `EquipoVisitante` VARCHAR(45) NOT NULL,
  `IdCategoria` INT NOT NULL,
  PRIMARY KEY (`IdPartido`),
  UNIQUE INDEX `IdPartido_UNIQUE` (`IdPartido` ASC) VISIBLE,
  INDEX `fk_Partido_Categoria1_idx` (`IdCategoria` ASC) VISIBLE,
  CONSTRAINT `fk_Partido_Categoria1`
    FOREIGN KEY (`IdCategoria`)
    REFERENCES `mydb`.`Categoria` (`IdCategoria`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Disponibilidad`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Disponibilidad` (
  `IdDisponibilidad` INT NOT NULL,
  `Disponibilidad` JSON NOT NULL,
  `Fecha` DATETIME NOT NULL,
  `NumLicencia` INT NOT NULL,
  PRIMARY KEY (`IdDisponibilidad`),
  UNIQUE INDEX `IdDisponibilidad_UNIQUE` (`IdDisponibilidad` ASC) VISIBLE,
  INDEX `fk_Disponibilidad_Arbitro1_idx` (`NumLicencia` ASC) VISIBLE,
  CONSTRAINT `fk_Disponibilidad_Arbitro1`
    FOREIGN KEY (`NumLicencia`)
    REFERENCES `mydb`.`Arbitro` (`NumLicencia`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`PartidoArbitrado`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`PartidoArbitrado` (
  `IdPartido` INT NOT NULL,
  `NumLicencia` INT NOT NULL,
  `Funcion` VARCHAR(45) NOT NULL,
  `Importe` DECIMAL NOT NULL,
  PRIMARY KEY (`NumLicencia`, `IdPartido`),
  INDEX `fk_Partido_has_Arbitro_Arbitro1_idx` (`NumLicencia` ASC) VISIBLE,
  INDEX `fk_Partido_has_Arbitro_Partido1_idx` (`IdPartido` ASC) VISIBLE,
  UNIQUE INDEX `IdPartido_UNIQUE` (`IdPartido` ASC) VISIBLE,
  UNIQUE INDEX `NumLicencia_UNIQUE` (`NumLicencia` ASC) VISIBLE,
  CONSTRAINT `fk_Partido_has_Arbitro_Partido1`
    FOREIGN KEY (`IdPartido`)
    REFERENCES `mydb`.`Partido` (`IdPartido`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Partido_has_Arbitro_Arbitro1`
    FOREIGN KEY (`NumLicencia`)
    REFERENCES `mydb`.`Arbitro` (`NumLicencia`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
