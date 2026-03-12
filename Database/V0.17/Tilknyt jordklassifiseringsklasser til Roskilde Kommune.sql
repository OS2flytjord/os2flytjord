USE [JordflytningTest150828v14]
GO

/* Tilknyt "Klasse 0" og "Anden klasse" til Roskilde Kommune */
INSERT INTO [dbo].[KommuneJordklassifikation]
           ([Id]
		   ,[JordKlassifikationTypeId]
           ,[KommuneId]
           ,[Aktiv])
     VALUES
           ('BC63AC54-D4CE-4DE4-8C1F-3853DD8F4FB5'
		   ,'7A7D81C1-F377-4EB8-8468-E3AE16878E5D'
           ,'FF8AEE8D-3512-4E7E-94A4-51B8BEB3C732'
           ,1)
GO

INSERT INTO [dbo].[KommuneJordklassifikation]
           ([Id]
		   ,[JordKlassifikationTypeId]
           ,[KommuneId]
           ,[Aktiv])
     VALUES
           ('C06DD048-A759-46AA-ABE1-03CF8AA24491'
		   ,'1B0774BC-2C76-40B9-B5C5-E5E67274E80A'
           ,'FF8AEE8D-3512-4E7E-94A4-51B8BEB3C732'
           ,1)
GO

