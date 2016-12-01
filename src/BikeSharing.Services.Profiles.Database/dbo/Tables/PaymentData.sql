CREATE TABLE [dbo].[PaymentData] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [CreditCard]     NVARCHAR (255) NULL,
    [CreditCardType] INT            NOT NULL,
    [ExpirationDate] DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_PaymentData] PRIMARY KEY CLUSTERED ([Id] ASC)
);

