create view OprindelsesstedMatr_Point as
select [id]
      ,[nummer]
      ,[OprindelsesstedKlassifikationTypeId]
      ,[OprindelsesstedKlassifikationType]
      ,[Adresse]
      ,[Postnummer]
      ,[PostDistrikt]
      ,[TidligereErhvervsAktivitet]
      ,[Kortlagt]
      ,[Geom]
      ,[OffvejUrl]
      ,[Beskrivelse]
      ,[AndenOprindJordTypeID]
from [dbo].[OprindelsesstedMatr]
where Geom.STGeometryType() = 'Point'

GO

