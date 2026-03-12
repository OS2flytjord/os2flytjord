USE [JordflytningTest150828v14]
GO




INSERT INTO [dbo].[JordanlaegType]
           ([id]
		   ,[Navn]
           ,[Sortering]
           ,[Aktiv]
           ,[Kode])
     VALUES
           ('80de8384-49e0-471a-8b48-99c608dc140a'
		   ,'DEPONERING'
           ,1
           ,1
           ,0)
GO

update [dbo].[JordanlaegType] set sortering = 2 where id = 'D8D3BD7A-1217-4368-8B47-79C200324E4B'
update [dbo].[JordanlaegType] set sortering = 3 where id = 'B28E7D07-11AC-4653-830F-499D5CD5AD4A'
update [dbo].[JordanlaegType] set sortering = 4 where id = '9DC3BA67-A2FB-433A-914D-E5CA14A5422C'
GO

INSERT INTO [dbo].[JordanlaegType]
           ([id]
		   ,[Navn]
           ,[Sortering]
           ,[Aktiv]
           ,[Kode])
     VALUES
           ('6dc446ef-9575-4d26-9599-075a68a1d9e8'
		   ,'KARTERING'
           ,5
           ,1
           ,0)
GO

update [dbo].[JordanlaegType] set sortering = 6 where id = '16E5B54E-E5CD-46FC-8548-8AF50E496E2C'
update [dbo].[JordanlaegType] set sortering = 7 where id = 'F16C80CA-E8B1-41B5-956A-D51E1F826915'
update [dbo].[JordanlaegType] set sortering = 8 where id = '3E5E55F5-C49F-42F1-BF19-F652FC4EB3AB'
GO

INSERT INTO [dbo].[JordanlaegType]
           ([id]
		   ,[Navn]
           ,[Sortering]
           ,[Aktiv]
           ,[Kode])
     VALUES
           ('f2abfd71-6dcd-418a-803a-1245f6d12452'
		   ,'TILLADELSE EFTER MBL § 19'
           ,9
           ,1
           ,0)
GO


INSERT INTO [dbo].[JordanlaegType]
           ([id]
		   ,[Navn]
           ,[Sortering]
           ,[Aktiv]
           ,[Kode])
     VALUES
           ('0255aeb4-a1cf-492e-91c4-27c307a8e727'
		   ,'ANDET'
           ,10
           ,1
           ,0)
GO