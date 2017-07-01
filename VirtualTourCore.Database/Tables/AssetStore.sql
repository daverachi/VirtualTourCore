CREATE TABLE [dbo].[AssetStore]
(
	[Id]	INT IDENTITY (1, 1) NOT NULL,
    ClientId		INT			  NOT NULL,
	Filename		VARCHAR (200) NOT NULL,
	Nickname		VARCHAR (200) NOT NULL,
	Path			VARCHAR (500) NOT NULL,
    FileType		NVARCHAR(50)  NOT NULL,
    CreateUserId    INT			  NOT NULL,
    CreateDate      DATETIME      NOT NULL,
    UpdateUserID    INT			  NULL,
    UpdateDate      DATETIME      NULL,
	CONSTRAINT PK_AssetStore PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT FK_AssetStore_Client FOREIGN KEY (ClientID) REFERENCES Client (Id),
	CONSTRAINT FK_AssetStore_CreateUser FOREIGN KEY (CreateUserId) REFERENCES SecurityUser (Id),
    CONSTRAINT FK_AssetStore_UpdateUser FOREIGN KEY (UpdateUserId) REFERENCES SecurityUser (Id),
)
