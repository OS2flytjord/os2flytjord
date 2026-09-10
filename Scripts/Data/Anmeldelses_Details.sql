declare
	@anmeldelses_nummer int = 122493
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



from Anmeldelse a
left outer join ModtagerAnlaeg ma on ma.id = a.ModtagerAnlaegId
left outer join Kommune k on k.Kommunenr = ma.KommuneKode
left outer join Oprindelsessted os on os.Id = a.Id
left outer join Kommune ak on ak.Id = a.KommuneId
where a.id = @anmeldelsesId;

