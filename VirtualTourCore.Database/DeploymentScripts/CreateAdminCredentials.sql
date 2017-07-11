-- todo make this idempotent merge script


INSERT INTO [dbo].[RegistrationCode]
           (Guid, AvailableUsages)
     VALUES
           (NEWID(), -999)
GO
INSERT INTO [dbo].[SecurityUser]
           ([Username]
           ,[Email]
           ,[PasswordHash]
           ,[PhoneNumber]
           ,[FirstName]
           ,[LastName]
           ,[RegistrationCodeId]
		   ,[IsSystemAdmin])
     VALUES
           ('MasterAdmin'
		   ,'david.ruhlemann@tallan.com'
		   ,'sha1:64000:18:gRYvqFhb3ooehdqtjN/tzs8XpH2RmYuf:McmEZUFuMn1qMiuCHe4IKMvx' -- password is: V8c38Q7wr5p%=5=5CfY+r+V8GZQu4Dqv
		   ,'8609934956'
		   ,'David'
		   ,'Ruhlemann'
		   ,(select Id from RegistrationCode where AvailableUsages = -999)
		   ,1)
GO
