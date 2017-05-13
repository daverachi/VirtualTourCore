CREATE TABLE [dbo].[RegistrationCode]
(
	[Id] int IDENTITY (1, 1) NOT NULL, 
	[ClientId] int NULL,
    [Guid] UNIQUEIDENTIFIER NOT NULL, 
    [AvailableUsages] INT NOT NULL DEFAULT 2, 
	CreateUserID    INT NULL,
    CreateDate      DATETIME      NULL,
    UpdateUserID    INT			NULL,
    UpdateDate      DATETIME      NULL,
	CONSTRAINT PK_RegistrationCode PRIMARY KEY CLUSTERED (Id ASC),
	CONSTRAINT FK_RegistrationCode_CreateUser FOREIGN KEY (CreateUserID) REFERENCES dbo.SecurityUser (Id),
	CONSTRAINT FK_RegistrationCode_UpdateUser FOREIGN KEY (UpdateUserID) REFERENCES dbo.SecurityUser (Id),
	CONSTRAINT FK_RegistrationCode_ClientId FOREIGN KEY (ClientId) REFERENCES dbo.Client (Id),
)
