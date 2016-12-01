CREATE TABLE [dbo].[Users] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [LastLogin] DATETIME2 (7)  NOT NULL,
    [UserName]  NVARCHAR (255) NULL,
    [Password]  NVARCHAR (255) NULL,
    [TenantId]  INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Users_Tenants_TenantId] FOREIGN KEY ([TenantId]) REFERENCES [dbo].[Tenants] ([Id]) ON DELETE CASCADE
);






GO
CREATE NONCLUSTERED INDEX [IX_Users_TenantId]
    ON [dbo].[Users]([TenantId] ASC);

