CREATE TABLE [dbo].[bikes]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [SerialNumber] VARCHAR(16) NULL, 
    [InCirculationSince] DATETIME2 NOT NULL, 
    [StationId] INT NULL,
	CONSTRAINT [FK_bikes_ToStation] FOREIGN KEY ([StationId]) REFERENCES [stations]([Id]),
)
