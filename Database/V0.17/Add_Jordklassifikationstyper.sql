USE [JordflytningTest150828v14]
GO

INSERT INTO [dbo].[JordKlassifikationType]
           ([Id]
		   ,[Navn]
           ,[Aktiv]
           ,[Sortering]
           ,[Priotering]
           ,[Kode]
           ,[LandsdelTypeId]
           ,[Tooltip])
     VALUES
           ('7A7D81C1-F377-4EB8-8468-E3AE16878E5D'
		   ,'Klasse 0'
           ,1
           ,5
           ,5
           ,8
           ,'0BEB86F2-8734-4B1F-B787-C9293FE7C7B8'
           ,'Ren jord')
GO


INSERT INTO [dbo].[JordKlassifikationType]
           ([Id]
		   ,[Navn]
           ,[Aktiv]
           ,[Sortering]
           ,[Priotering]
           ,[Kode]
           ,[LandsdelTypeId]
           ,[Tooltip])
     VALUES
           ('1B0774BC-2C76-40B9-B5C5-E5E67274E80A'
		   ,'Anden klasse'
           ,1
           ,6
           ,6
           ,9
           ,'0BEB86F2-8734-4B1F-B787-C9293FE7C7B8'
           ,'Anden klasse')
GO


