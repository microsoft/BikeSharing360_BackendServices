CREATE TABLE [dbo].[Tenants] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (255) NULL,
    CONSTRAINT [PK_Tenants] PRIMARY KEY CLUSTERED ([Id] ASC)
);

