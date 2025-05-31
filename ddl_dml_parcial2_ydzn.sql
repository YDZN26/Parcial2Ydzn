CREATE DATABASE Parcial2Ydzn;
GO

USE [master];
GO
CREATE LOGIN [usrparcial2] WITH PASSWORD = N'12345678',
	DEFAULT_DATABASE = [Parcial2Ydzn],
	CHECK_EXPIRATION = OFF,
	CHECK_POLICY = ON
GO

USE [Parcial2Ydzn];
GO
CREATE USER [usrparcial2] FOR LOGIN [usrparcial2];
GO
ALTER ROLE [db_owner] ADD MEMBER [usrparcial2];
GO


CREATE TABLE Serie (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    titulo VARCHAR(250) NOT NULL,
    sinopsis VARCHAR(5000) NULL,
    director VARCHAR(100) NULL,
    episodios INT NOT NULL,
    fechaEstreno DATE NOT NULL,
    estado SMALLINT NOT NULL DEFAULT 1
);