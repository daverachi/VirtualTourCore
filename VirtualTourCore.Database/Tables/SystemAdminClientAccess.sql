CREATE TABLE [dbo].[SystemAdminClientAccess]
(
	[Id] INT IDENTITY (1, 1) NOT NULL, 
	[ClientId] INT NOT NULL,
	[GrantsSystemAdminAccess] Bit NOT NULL,
	[CreateUserID]    INT NULL,
    [CreateDate]      DATETIME      NULL,
    [UpdateUserID]    INT			NULL,
    [UpdateDate]      DATETIME      NULL,
    CONSTRAINT PK_SystemAdminClientAccess PRIMARY KEY CLUSTERED (Id ASC),
	CONSTRAINT FK_SystemAdminClientAccess_CreateUser FOREIGN KEY ([CreateUserID]) REFERENCES dbo.SecurityUser (Id),
    CONSTRAINT FK_SystemAdminClientAccess_UpdateUser FOREIGN KEY ([UpdateUserID]) REFERENCES dbo.SecurityUser (Id),
    CONSTRAINT FK_SystemAdminClientAccess_Client FOREIGN KEY ([ClientID]) REFERENCES dbo.Client (Id),
)
