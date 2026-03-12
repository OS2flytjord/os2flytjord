/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [Id]
      ,[CVR]
      ,[PNummer]
      ,[Firmanavn]
      ,[Adresse]
      ,[Postnummer]
      ,[Postdistikt]
      ,[Telefon]
      ,[EAN]
  FROM [Jordflytning].[dbo].[Firmaoplysninger]
  where Firmanavn='Vognmandsfirmaet Verner Franck Jensen'
  
  select * from Person where FirmaoplysningerId='D5EBB9DB-B4CE-4742-AD1D-0D5DA4F13E65'
  
  select * from transportoer where id='F2B562CF-FA2D-4BF9-9D04-6D41CDACFDD1'
  select * from Lastbil where TransportoerId='F2B562CF-FA2D-4BF9-9D04-6D41CDACFDD1'
  
  select * from lastbil where Nummer=1581
  select * from lastbil where Nummer=1571
  
  
  
 set identity_insert Lastbil On
  GO
  
  insert into Lastbil (TransportoerId,MiljoeklasseTypeId,Fabrikat,Nummerplade,Bemærkning,aktiv,Nummer,Redigeret)
  values ('F2B562CF-FA2D-4BF9-9D04-6D41CDACFDD1','E030ED2C-13A4-4A1F-BBAD-83A864729BD6'	,'mercedes','cf 88 219','Oprettet af NIRAS',1,1571,GETDATE());
  go
  set identity_insert Lastbil Off
  GO
  
  select * from Vognlaes where LastbilId =(select Id from Lastbil where Nummer =1571)
  
  update Vognlaes 
  set LastbilId='B33032E7-2F69-4F0D-8FB7-35BA7E0FF779'
  where ID in ('5E39871F-D5E0-4AA7-BD58-1E34287513F5','F87F35E4-F28A-41C9-B1A3-B8D71B1749E9');
  
  delete from lastbil where Nummer =1571