declare
	@anmeldelses_nummer int = 121346
	, @anmeldelsesId uniqueidentifier
	;

select @anmeldelsesId = id from Anmeldelse where Nummer = @anmeldelses_nummer;

select 
--Modtageranlæg
ma.KommuneKode, ma.Adresse, ma.Postnummer, ma.PostDistrikt, k.Navn
--Oprindelsessted
,os.Adresse, os.Postnummer, os.PostDistrikt

--Anmeldelse
,ak.Navn as AnmeldelsesKommuneNavn
, a.*
, p.*
, f.*
from Anmeldelse a
left outer join ModtagerAnlaeg ma on ma.id = a.ModtagerAnlaegId
left outer join Kommune k on k.Kommunenr = ma.KommuneKode
left outer join Oprindelsessted os on os.Id = a.Id
left outer join Kommune ak on ak.Id = a.KommuneId
left outer join Person p on p.Id = a.AnmelderId
left outer join Firmaoplysninger f on f.Id = p.Firmaoplysningerid
where a.id = @anmeldelsesId;

select k.Id, tilP.Email, fraP.Email, b.id, * from Kommunikation k
left outer join Person tilP on tilP.id = k.TilPerson
left outer join Person fraP on fraP.id = k.FraPerson
left outer join Besked b on b.Id = k.BeskedId
where Anmeldelseid = @anmeldelsesId



--select * from advis where beskedid = '050693B7-F222-41F5-9E27-3A6D2CEC62D7'