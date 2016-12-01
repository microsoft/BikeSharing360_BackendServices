CREATE TABLE [dbo].[Venues] (
    [Id]         INT              IDENTITY (1, 1) NOT NULL,
    [ExternalId] NVARCHAR (450)   NOT NULL,
    [Latitude]   NUMERIC (18, 10) NOT NULL,
    [Longitude]  NUMERIC (18, 10) NOT NULL,
    [Name]       NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_Venues] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AK_Venues_ExternalId] UNIQUE NONCLUSTERED ([ExternalId] ASC)
);

