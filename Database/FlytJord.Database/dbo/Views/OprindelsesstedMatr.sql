


CREATE view [dbo].[OprindelsesstedMatr] as
select 
o.id
,a.nummer
, o.OprindelsesstedKlassifikationTypeId
, t.Navn As OprindelsesstedKlassifikationType
, o.Adresse
, o.Postnummer
, o.PostDistrikt
, o.TidligereErhvervsAktivitet
, o.Kortlagt
, case
    when o.Geom.STGeometryType() = 'Point' and m.id is not null then m.Geom
	else o.Geom
  end as Geom
, o.OffvejUrl
, o.Beskrivelse
, o.AndenOprindJordTypeID
from Oprindelsessted o
left outer join OprindelsesstedKlassifikationType t on t.id = o.OprindelsesstedKlassifikationTypeId
left outer join Anmeldelse a on a.id = o.Id
left outer join Matrikel m on m.OprindelsesstedId = o.Id
where o.id = (
	select top 1 
	o2.id
	from Oprindelsessted o2
	where o2.Adresse = o.Adresse
	and o2.Postnummer = o.Postnummer)

GO

