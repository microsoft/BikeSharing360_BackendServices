CREATE EXTERNAL TABLE [dbo].[Venues] (
    [Id] INT NOT NULL,
    [ExternalId] NVARCHAR (450) NOT NULL,
    [Latitude] NUMERIC (18, 10) NOT NULL,
    [Longitude] NUMERIC (18, 10) NULL,
    [Name] NVARCHAR (MAX) NULL
)
    WITH (
    DATA_SOURCE = [EventsReferenceData],
    SCHEMA_NAME = N'dbo',
    OBJECT_NAME = N'venues'
    );

