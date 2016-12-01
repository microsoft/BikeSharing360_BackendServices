CREATE TABLE [dbo].[rides]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Duration] INT NULL, 
    [Start] DATETIME2 NULL, 
    [Stop] DATETIME2 NULL, 
    [StartStationId] INT NOT NULL, 
    [EndStationId] INT NULL, 
    [BikeId] INT NOT NULL, 
    [UserId] INT NOT NULL, 
    [EventType] INT NULL, 
    [EventId] INT NULL, 
    [GeoDistance] INT NULL, 
    [EventName] VARCHAR(512) NULL, 
    CONSTRAINT [FK_rides_ToStartStation] FOREIGN KEY ([StartStationId]) REFERENCES [stations]([Id]),
	CONSTRAINT [FK_rides_ToEndStation] FOREIGN KEY ([EndStationId]) REFERENCES [stations]([Id]),
	CONSTRAINT [FK_rides_ToBike] FOREIGN KEY ([BikeId]) REFERENCES [bikes]([Id])
)
