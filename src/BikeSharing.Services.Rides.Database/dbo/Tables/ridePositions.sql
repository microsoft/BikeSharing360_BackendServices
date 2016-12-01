CREATE TABLE [dbo].[ridePositions] (
    [Id]        BIGINT           IDENTITY (1, 1) NOT NULL,
    [RideId]    INT              NOT NULL,
    [Latitude]  NUMERIC (18, 10) NOT NULL,
    [Longitude] NUMERIC (18, 10) NOT NULL,
    [TS]        DATETIME2 (7)    NOT NULL,
    CONSTRAINT [PK_ridePositions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ridePositions_rides] FOREIGN KEY ([RideId]) REFERENCES [dbo].[rides] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [ix_RidePositions_RideId]
    ON [dbo].[ridePositions]([RideId] ASC);

