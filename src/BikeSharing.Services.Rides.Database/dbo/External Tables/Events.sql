CREATE EXTERNAL TABLE [dbo].[Events] (
    [Id] INT NOT NULL,
    [EndTime] DATETIME2 (7) NULL,
    [ExternalId] NVARCHAR (450) NOT NULL,
    [GenreId] INT NOT NULL,
    [ImagePath] NVARCHAR (MAX) NULL,
    [Name] NVARCHAR (MAX) NULL,
    [SegmentId] INT NOT NULL,
    [StartTime] DATETIME2 (7) NULL,
    [SubGenreId] INT NOT NULL,
    [VenueId] INT NOT NULL
)
    WITH (
    DATA_SOURCE = [EventsReferenceData],
    SCHEMA_NAME = N'dbo',
    OBJECT_NAME = N'events'
    );

