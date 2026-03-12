USE [Jordflytning_STAGE]
GO

INSERT INTO [dbo].[AdvisType]
           ([Id]
           ,[Navn]
           ,[Aktiv]
           ,[Sortering]
           ,[Kode])
     VALUES
           ('8be72185-8f96-4d5b-b576-1283d816d846'
           ,'Hør anden kommune'
           ,1
           ,3
           ,3)

INSERT INTO [dbo].[AdvisType]
           ([Id]
           ,[Navn]
           ,[Aktiv]
           ,[Sortering]
           ,[Kode])
     VALUES
           ('728f161c-fdfd-47a2-9751-5d56362695ee'
           ,'Svar fra anden kommune'
           ,1
           ,4
           ,4)
GO


