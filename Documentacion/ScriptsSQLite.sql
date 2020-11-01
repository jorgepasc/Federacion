
-- -----------------------------------------------------
-- Table mydb.Administrador
-- -----------------------------------------------------
CREATE TABLE Administrador (
  IdAdministrador INT NOT NULL UNIQUE IDENTITY(1, 1),
  DNI VARCHAR(9) NOT NULL,
  Nombre VARCHAR(20) NULL,
  Apellidos VARCHAR(50) NULL,
  NumTelefono VARCHAR(15) NULL,  
  PRIMARY KEY (IdAdministrador));


-- -----------------------------------------------------
-- Table mydb.Arbitro
-- -----------------------------------------------------
CREATE TABLE Arbitro (
  IdArbitro INT NOT NULL IDENTITY(1, 1),
  Alias VARCHAR(20) NOT NULL UNIQUE,
  DNI VARCHAR(9) NULL,
  Nombre VARCHAR(20) NULL,
  Apellidos VARCHAR(50) NULL,
  NumTelefono VARCHAR(15) NULL,
  PRIMARY KEY (IdArbitro));


-- -----------------------------------------------------
-- Table mydb.Nomina
-- -----------------------------------------------------
CREATE TABLE Nomina (
  IdNomina INT NOT NULL IDENTITY(1, 1),
  Fecha DATETIME NOT NULL,
  Total DECIMAL(5,2) NOT NULL,
  IdArbitro INT NOT NULL,
  PRIMARY KEY (IdNomina),
  CONSTRAINT fk_Nomina_Arbitro
    FOREIGN KEY (IdArbitro)
    REFERENCES Arbitro (IdArbitro)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);


-- -----------------------------------------------------
-- Table mydb.Categoria
-- -----------------------------------------------------
CREATE TABLE Categoria (
  IdCategoria INT NOT NULL IDENTITY(1, 1),
  Descripcion VARCHAR(45) NOT NULL,
  TarifaArb1 DECIMAL(5,2) NOT NULL,
  TarifaArb2 DECIMAL(5,2) NULL,
  TarifaArb3 DECIMAL(5,2) NULL,
  TarifaAnotador DECIMAL(5,2) NOT NULL,
  TarifaCrono DECIMAL(5,2) NULL,
  Tarifa24 DECIMAL(5,2) NULL,
  PRIMARY KEY (IdCategoria));


-- -----------------------------------------------------
-- Table mydb.Partido
-- -----------------------------------------------------
CREATE TABLE Partido (
  IdPartido INT NOT NULL IDENTITY(1, 1),
  Fecha DATETIME NOT NULL,
  EquipoLocal VARCHAR(45) NOT NULL,
  EquipoVisitante VARCHAR(45) NOT NULL,
  IdCategoria INT NOT NULL,
  Ubicacion VARCHAR(200) NOT NULL,
  Observaciones VARCHAR(200),
  PRIMARY KEY (IdPartido),
  CONSTRAINT fk_Partido_Categoria
    FOREIGN KEY (IdCategoria)
    REFERENCES Categoria (IdCategoria)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);

-- -----------------------------------------------------
-- Table mydb.PartidoArbitrado
-- -----------------------------------------------------
CREATE TABLE PartidoArbitrado (
  IdPartidoArbitrado INT NOT NULL IDENTITY(1, 1),
  IdPartido INT NOT NULL,
  IdArbitro INT NOT NULL,
  Funcion VARCHAR(45) NOT NULL,
  Importe DECIMAL(5,2),
  PRIMARY KEY (IdPartidoArbitrado),
  CONSTRAINT fk_PartidoArbitrado_Partido
    FOREIGN KEY (IdPartido)
    REFERENCES Partido (IdPartido)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_PartidoArbitrado_Arbitro
    FOREIGN KEY (IdArbitro)
    REFERENCES Arbitro (IdArbitro)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);
	
-- -----------------------------------------------------
-- Table mydb.Disponibilidad
-- -----------------------------------------------------
CREATE TABLE Disponibilidad (
  IdDisponibilidad INT NOT NULL IDENTITY(1, 1),
  Disponibilidad XML NOT NULL,
  Fecha DATETIME NOT NULL,  
  IdArbitro INT NOT NULL,
  PRIMARY KEY (IdDisponibilidad),
  CONSTRAINT fk_Disponibilidad_Arbitro
    FOREIGN KEY (IdArbitro)
    REFERENCES Arbitro (IdArbitro)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);



	
CREATE TABLE Usuario (
	IdUsuario INT NOT NULL IDENTITY(1, 1),
	IdArbitro INT,
	IdAdministrador INT,
	Usuario VARCHAR(9) NOT NULL UNIQUE,
	Password VARCHAR(20) NOT NULL,
	IsAdmin bit
	PRIMARY KEY (IdUsuario),
	CONSTRAINT FK_Usuario_Arbitro
	FOREIGN KEY (IdArbitro)
	REFERENCES Arbitro(IdArbitro),
	CONSTRAINT FK_Usuario_Administrador
	FOREIGN KEY (IdAdministrador)
	REFERENCES Administrador(IdAdministrador)
)

-- CREATE TABLE Funcion(
	-- IdFuncion INT NOT NULL IDENTITY(1, 1),
	-- Descripcion VARCHAR(50) NOT NULL,
	-- ColumnaTarifa VARCHAR(50) NOT NULL
	-- PRIMARY KEY (IdFuncion)
-- )

-- INSERT INTO Administrador VALUES ('11223344A', 'Luis', 'Admin', '976976976')

-- INSERT INTO Arbitro values ('JORGE', '12345678A', 'Jorge', 'Pascual Cases', '123456789')
-- INSERT INTO Arbitro values ('PASCUAL', '12345678B', 'Pepe', 'Pascual', '111222333')
 --INSERT INTO Arbitro values ('PEREA', '12345678C', 'Fran', 'Perea', '666777888')
 --INSERT INTO Arbitro values ('BLASCO', '12345678C', 'Nuria', 'Blasco', '686868686')
 --INSERT INTO Arbitro values ('SANCHEZ', '12345678E', 'Luis', 'Sanchez', '655555556')
 --INSERT INTO Arbitro values ('MAR', '12345678E', 'Mar', 'Rojo', '612121212

-- INSERT INTO Usuario VALUES (1, null,  'jorge', 'pass', 0)
-- INSERT INTO Usuario VALUES (2, null, 'pepe', '123', 0)
-- INSERT INTO Usuario VALUES (3, null,  'perea', '1', 0)
-- INSERT INTO Usuario VALUES (4, null,  'blasco', '1', 0)
-- INSERT INTO Usuario VALUES (5, null,  'sanchez', '1', 0)
-- INSERT INTO Usuario VALUES (null, 1, 'admin', 'admin', 1)

-- INSERT INTO Categoria values ('Social Oro', 35, 27.50, null, 14.5, 17, 17)
-- INSERT INTO Categoria values ('Social Plata', 32, 25, null, 13.5, 15, 15)
-- INSERT INTO Categoria values ('Social Bronce', 30, null, null, 15, null, null)

-- INSERT INTO Partido VALUES ('20190818 12:00:00', 'Doctor Azua', 'Boscos', 1, 'Pabellón CesarAugusto. Calle Asín y Palacios, 5', null)
-- INSERT INTO Partido VALUES ('20190818 09:15:00', 'Helios', 'CBZ', 1, 'Pabellón Helios. Avda. Ranillas, 2', 'Hora oficial de comienzo del partido: 09:25')
-- INSERT INTO Partido VALUES ('20190821 18:00:00', 'Solteros', 'Casados', 3, 'Pabellón Dominicos, Pza. San Francisco', null)

-- INSERT INTO PartidoArbitrado values(1, 1, 'Arbitro Principal', null)
-- INSERT INTO PartidoArbitrado values(1, 2, 'Arbitro Auxiliar', null)
-- INSERT INTO PartidoArbitrado values(1, 3, 'Arbitro Ayudante', null)
-- INSERT INTO PartidoArbitrado values(1, 4, 'Anotador', null)
-- INSERT INTO PartidoArbitrado values(1, 5, 'Cronometrador', null)
-- INSERT INTO PartidoArbitrado values(1, 6, '24', null)
-- INSERT INTO PartidoArbitrado values(3, 2, 'Arbitro Principal', null)
-- INSERT INTO PartidoArbitrado values(3, 1, 'Anotador', null)


-- drop Table Administrador
-- drop Table Usuario
-- drop Table Nomina
-- drop Table PartidoArbitrado
-- drop Table Disponibilidad
-- drop Table Partido
-- drop Table Categoria
-- drop Table Arbitro


