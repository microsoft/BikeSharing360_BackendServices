CREATE TABLE [dbo].[Events] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [EndTime]    DATETIME2 (7)  NULL,
    [ExternalId] NVARCHAR (450) NOT NULL,
    [GenreId]    INT            NOT NULL,
    [ImagePath]  NVARCHAR (MAX) NULL,
    [Name]       NVARCHAR (MAX) NULL,
    [SegmentId]  INT            NOT NULL,
    [StartTime]  DATETIME2 (7)  NULL,
    [SubGenreId] INT            NOT NULL,
    [VenueId]    INT            NOT NULL,
    CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Events_Classifications_GenreId] FOREIGN KEY ([GenreId]) REFERENCES [dbo].[Classifications] ([Id]),
    CONSTRAINT [FK_Events_Classifications_SegmentId] FOREIGN KEY ([SegmentId]) REFERENCES [dbo].[Classifications] ([Id]),
    CONSTRAINT [FK_Events_Classifications_SubGenreId] FOREIGN KEY ([SubGenreId]) REFERENCES [dbo].[Classifications] ([Id]),
    CONSTRAINT [FK_Events_Venues_VenueId] FOREIGN KEY ([VenueId]) REFERENCES [dbo].[Venues] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [AK_Events_ExternalId] UNIQUE NONCLUSTERED ([ExternalId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Events_GenreId]
    ON [dbo].[Events]([GenreId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Events_SegmentId]
    ON [dbo].[Events]([SegmentId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Events_SubGenreId]
    ON [dbo].[Events]([SubGenreId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Events_VenueId]
    ON [dbo].[Events]([VenueId] ASC);

