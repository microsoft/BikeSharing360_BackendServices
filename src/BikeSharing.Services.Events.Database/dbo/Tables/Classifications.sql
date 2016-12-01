CREATE TABLE [dbo].[Classifications] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [ExternalId] NVARCHAR (450) NOT NULL,
    [Name]       NVARCHAR (MAX) NULL,
    [Type]       INT            NOT NULL,
    CONSTRAINT [PK_Classifications] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AK_Classifications_ExternalId] UNIQUE NONCLUSTERED ([ExternalId] ASC)
);

