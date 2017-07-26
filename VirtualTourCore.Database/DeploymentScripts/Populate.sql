SET NOCOUNT ON
SET IDENTITY_INSERT [dbo].[ItemStatus] ON
MERGE INTO 
	dbo.ItemStatus 
WITH (HOLDLOCK) AS target
USING (VALUES
	(1, 'Public'),
	(2, 'Private'),
	(3, 'Deleted')
	) AS source ([Id], [Name])
    ON target.Id = source.Id
WHEN MATCHED 
THEN 
    UPDATE SET target.Name = source.Name
WHEN NOT MATCHED BY TARGET 
THEN
    INSERT (Id, Name)
    VALUES (source.Id, source.Name);
SET IDENTITY_INSERT [dbo].[ItemStatus] OFF
GO
SET NOCOUNT OFF
GO