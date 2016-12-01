CREATE TABLE [dbo].[Profiles] (
    [Id]             INT              IDENTITY (1, 1) NOT NULL,
    [BirthDate]      DATETIME2 (7)    NULL,
    [FirstName]      NVARCHAR (255)   NULL,
    [Gender]         INT              NOT NULL,
    [LastName]       NVARCHAR (255)   NULL,
    [UserId]         INT              NOT NULL,
    [Email]          NVARCHAR (255)   NULL,
    [PaymentId]      INT              NULL,
    [FaceProfileId]  UNIQUEIDENTIFIER NULL,
    [VoiceProfileId] UNIQUEIDENTIFIER NULL,
    [Mobile]         NVARCHAR (255)   NULL,
    [Skype]          NVARCHAR (255)   NULL,
    [VoiceSecretPhrase] NVARCHAR(255) NULL,
    CONSTRAINT [PK_Profiles] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Profiles_PaymentData_PaymentId] FOREIGN KEY ([PaymentId]) REFERENCES [dbo].[PaymentData] ([Id]),
    CONSTRAINT [FK_Profiles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Profiles_UserId]
    ON [dbo].[Profiles]([UserId] ASC) WHERE ([UserId] IS NOT NULL);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Profiles_PaymentId]
    ON [dbo].[Profiles]([PaymentId] ASC) WHERE ([PaymentId] IS NOT NULL);

