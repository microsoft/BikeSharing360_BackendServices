CREATE EXTERNAL TABLE [dbo].[Classifications] (
    [Id] INT NOT NULL,
    [ExternalId] NVARCHAR (450) NOT NULL,
    [Name] NVARCHAR (MAX) NULL,
    [Type] INT NOT NULL
)
    WITH (
    DATA_SOURCE = [EventsReferenceData],
    SCHEMA_NAME = N'dbo',
    OBJECT_NAME = N'Classifications'
    );

