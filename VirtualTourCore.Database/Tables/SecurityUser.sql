CREATE TABLE [dbo].[SecurityUser]
(
	[Id] INT IDENTITY (1, 1) NOT NULL, 
	[Username] NVARCHAR(40) NOT NULL, 
    [Email] NVARCHAR(60) NOT NULL, 
    [PasswordHash] NVARCHAR(MAX) NOT NULL, 
    [PhoneNumber] NVARCHAR(50) NULL, 
    [FirstName] NVARCHAR(50) NULL, 
    [LastName] NVARCHAR(50) NULL, 
    [RegistrationCodeId] INT NOT NULL,
	[IsSystemAdmin] BIT NOT NULL DEFAULT (0), 
    CONSTRAINT PK_SecurityUserId PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_SecurityUser_RegistrationCode FOREIGN KEY ([RegistrationCodeId]) REFERENCES dbo.RegistrationCode (Id),
)
