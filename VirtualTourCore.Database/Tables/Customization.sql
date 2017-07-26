CREATE TABLE [dbo].[Customization]
(
	Id	INT IDENTITY (1, 1) NOT NULL,
	CustomCSS		NVARCHAR(MAX) NULL,
	CustomJS		NVARCHAR(MAX) NULL,
    CreateUserId    INT			  NOT NULL,
    CreateDate      DATETIME      NOT NULL,
    UpdateUserID    INT			  NULL,
    UpdateDate      DATETIME      NULL,
	CONSTRAINT FK_Customization_CreateUser FOREIGN KEY (CreateUserId) REFERENCES SecurityUser (Id),
    CONSTRAINT FK_Customization_UpdateUser FOREIGN KEY (UpdateUserId) REFERENCES SecurityUser (Id),
	CONSTRAINT PK_Customization PRIMARY KEY CLUSTERED (Id ASC),
)
