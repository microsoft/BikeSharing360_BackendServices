CREATE TABLE [dbo].[stations]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(64) NOT NULL, 
    [Latitude] NUMERIC(18, 10) NOT NULL, 
    [Longitude] NUMERIC(18, 10) NOT NULL, 
    [Slots] NUMERIC(4) NOT NULL DEFAULT 30
)
