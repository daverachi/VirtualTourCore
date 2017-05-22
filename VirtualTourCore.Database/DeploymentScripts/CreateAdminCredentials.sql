-- todo make this idempotent merge script

INSERT INTO [dbo].[RegistrationCode]
           (Guid, AvailableUsages)
     VALUES
           (NEWID(), 1)
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
		   ,(select Id from RegistrationCode where GUID = '99681bc1-99f2-4e99-a57b-60964e7c66b0')
		   ,1)
GO
