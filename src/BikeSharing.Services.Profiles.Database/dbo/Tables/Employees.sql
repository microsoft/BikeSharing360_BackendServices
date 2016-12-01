CREATE TABLE [dbo].[Employees]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Skype] NCHAR(100) NOT NULL, 
    [Name] NCHAR(100) NOT NULL, 
    [ServiceUrl] NCHAR(100) NULL, 
    [ConversationId] NCHAR(100) NULL, 
    [Latitude] FLOAT NULL, 
    [Longitude] FLOAT NULL, 
    [CustomerSkype] NCHAR(100) NULL, 
    [CustomerName] NCHAR(100) NULL, 
    [CustomerServiceUrl] NCHAR(100) NULL, 
    [CustomerConversationId] NCHAR(100) NULL, 
    [CustomerLatitude] FLOAT NULL, 
    [CustomerLongitude] FLOAT NULL, 
    [CustomerAddr] NCHAR(255) NULL
)
