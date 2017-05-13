CREATE TABLE [dbo].[SecurityUserClients]
(
	[Id] INT IDENTITY (1, 1) NOT NULL, 
	[SecurityUserId] INT NOT NULL, 
    [ClientId] INT NOT NULL, 
	[CreateUserID]    INT NULL,
    [CreateDate]      DATETIME      NULL,
    [UpdateUserID]    INT			NULL,
    [UpdateDate]      DATETIME      NULL,
    CONSTRAINT PK_SecurityUserClient PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_SecurityUserClients_SecurityUser FOREIGN KEY ([SecurityUserID]) REFERENCES dbo.SecurityUser (Id),
    CONSTRAINT FK_SecurityUserClients_CreateUser FOREIGN KEY ([CreateUserID]) REFERENCES dbo.SecurityUser (Id),
    CONSTRAINT FK_SecurityUserClients_UpdateUser FOREIGN KEY ([UpdateUserID]) REFERENCES dbo.SecurityUser (Id),
    CONSTRAINT FK_SecurityUserClients_Client FOREIGN KEY (ClientId) REFERENCES dbo.Client (Id),
)
