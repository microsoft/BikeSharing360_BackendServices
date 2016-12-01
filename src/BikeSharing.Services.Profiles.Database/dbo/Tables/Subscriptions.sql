CREATE TABLE [dbo].[Subscription] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [ExpiresOn] DATETIME2 (7) NULL,
    [Status]    INT           NOT NULL,
    [Type]      INT           NOT NULL,
    [UserId]    INT           NOT NULL,
    CONSTRAINT [PK_Subscriptions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Subscriptions_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Subscription_UserId]
    ON [dbo].[Subscription]([UserId] ASC);

