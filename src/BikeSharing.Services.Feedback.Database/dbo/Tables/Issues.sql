CREATE TABLE [dbo].[Issues] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [Description] NVARCHAR (1024) NULL,
    [IssueType]   INT             NOT NULL,
    [Latitude]    FLOAT (53)      NOT NULL,
    [Longitude]   FLOAT (53)      NOT NULL,
    [Solved]      BIT             NOT NULL,
    [StopId]      INT             NULL,
    [UserId]      INT             NOT NULL,
    [UtcTime]     DATETIME2 (7)   NOT NULL,
    [BikeId]      INT             DEFAULT ((0)) NOT NULL,
    [Title]       NVARCHAR (1024) NULL,
    CONSTRAINT [PK_Issues] PRIMARY KEY CLUSTERED ([Id] ASC)
);





