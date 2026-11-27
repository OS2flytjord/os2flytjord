

CREATE view [dbo].[MapModtagerAnlaeg] as 
select
row_number() over(order by m.Id) as Eid,
m.id,
m.JordanlaegTypeId,
m.JordKlassifikationTypeId,
m.Navn,
m.Adresse,
m.Postnummer,
m.PostDistrikt,
m.Ejerlav,
m.Matrikelnr,
m.www,
m.OffentligBemaerkning,
m.Affald,
jat.navn as Jordanlaegtype,
jkt.navn as Jordklassifikationtype,
m.Geom


from 
Modtageranlaeg m 
left outer join Jordanlaegtype jat on m.JordanlaegTypeId = jat.Id
left outer join Jordklassifikationtype jkt on m.JordKlassifikationTypeId = jkt.Id
where 
m.aktiv=1

GO

