USE HotelDB;
GO

IF COL_LENGTH('dbo.Reserva', 'Observaciones') IS NULL
    ALTER TABLE Reserva ADD Observaciones NVARCHAR(200) NULL;
GO
