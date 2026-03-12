drop view MapModtagerAnlaeg
go

create view MapModtagerAnlaeg as 
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
Modtageranlaeg m,
Jordanlaegtype jat,
Jordklassifikationtype jkt 
where 
	m.JordanlaegTypeId = jat.Id
and m.JordKlassifikationTypeId = jkt.Id
and m.aktiv=1
go