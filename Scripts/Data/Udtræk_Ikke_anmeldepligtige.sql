/*
Udtræk: Anmeldelser der ikke er anmeldepligtige og hvor der er sagt nej til gebyrpligt
Bestilt af: Mette Erbou Hansen, Aarhus Kommune (mail "SV: Udtræk - FlytJord")

Kriterier fra mailen:
  1. Forureningskategorien under 'Jorden' er Kategori 1 (ren jord)
  2. Der er ikke tjek i nogen forureningsoplysninger under 'Resultat af jordforureningsopslag'
     (V2, V1, OMK Analysepligt, OMK Kat. 2, OMK Kat. 1, Kommunens miljødatabase)
  3. Under 'Gebyr og tidsregistrering' er 'Gebyrpligtig' = Nej og 'Frigivet til fakturering' = Nej

Periode: hele 2025 (oprindelig forespørgsel). Ret @fraDato/@tilDato efter behov.
Revisioner: styres med @medRevisioner (1 = med, 0 = uden). Kolonnen ErRevision viser hvilke der er revisioner.
*/

declare
	@kommuneNavn varchar(max) = 'Aarhus'
	, @fraDato datetime = '20250101'	--inklusiv
	, @tilDato datetime = '20260101'	--eksklusiv
	, @medRevisioner bit = 1
	, @kategori1 uniqueidentifier = '1426C54A-6C06-4970-AC52-327D013751C7' --Kategori 1 (Ren jord)
	, @kommuneId uniqueidentifier
	;

select @kommuneId = Id from Kommune where [Navn] = @kommuneNavn;

with Udtraek as (
	select
		a.Nummer as Loebenummer
		, case when a.RevisionAfAnmeldelse is null then 'Nej' else 'Ja' end as ErRevision
		, ra.Nummer as RevisionAfLoebenummer
		, aes.Oprettet
		, aes.Godkendt
		, aes.Afvist
		--Oprindelsessted
		, os.Adresse as OprindelsesstedAdresse, os.Postnummer as OprindelsesstedPostnummer, os.PostDistrikt as OprindelsesstedPostdistrikt
		--Modtager
		, ma.Nummer as ModtagerNummer, ma.Navn as ModtagerNavn
		--Jorden
		, jkt.Navn as Forureningskategori
		, j.ForventetJordmaengdeTon
		--Jordforureningsopslag
		, jfo.Tid as ForureningsopslagTid
		--Gebyr
		, case g.Gebyrpligtig when 1 then 'Ja' when 0 then 'Nej' end as Gebyrpligtig
		, g.BeslutningsDato
		, case g.FrigivetTilFakturering when 1 then 'Ja' else 'Nej' end as FrigivetTilFakturering
	from dbo.Anmeldelse a
	left outer join dbo.AnmeldelseEffektivStatus aes on aes.AnmeldelseId = a.Id
	left outer join dbo.Anmeldelse ra on ra.Id = a.RevisionAfAnmeldelse
	left outer join dbo.Jord j on j.Id = a.Id
	left outer join dbo.JordKlassifikationType jkt on jkt.Id = j.JordKlassifikationTypeId
	left outer join dbo.Jordforureningsopslag jfo on jfo.Id = a.Id
	left outer join dbo.Gebyr g on g.Id = a.GebyrId
	left outer join dbo.Oprindelsessted os on os.Id = a.Id
	left outer join dbo.ModtagerAnlaeg ma on ma.Id = a.ModtagerAnlaegId
	where a.KommuneId = @kommuneId
	and a.Nummer > 0
	and aes.Oprettet >= @fraDato and aes.Oprettet < @tilDato
	and (@medRevisioner = 1 or a.RevisionAfAnmeldelse is null)
	--1. Kategori 1 (ren jord)
	and j.JordKlassifikationTypeId = @kategori1
	--2. Ingen tjek i resultat af jordforureningsopslag (manglende opslag tælles som ingen tjek)
	and isnull(jfo.V2, 0) = 0
	and isnull(jfo.V1, 0) = 0
	and isnull(jfo.OmkAnalysepligt, 0) = 0
	and isnull(jfo.OmkLet, 0) = 0	--OMK Kat. 2
	and isnull(jfo.OmkRen, 0) = 0	--OMK Kat. 1
	and isnull(jfo.KommunensMiljoeDb, '') = ''
	--3. Gebyrpligtig = Nej og Frigivet til fakturering = Nej
	and g.Gebyrpligtig = 0
	and g.FrigivetTilFakturering = 0
)
select * from Udtraek
order by Oprettet;

--Optælling
select
	count(*) as Antal
	, sum(case when a.RevisionAfAnmeldelse is null then 1 else 0 end) as AntalUdenRevisioner
	, sum(case when a.RevisionAfAnmeldelse is not null then 1 else 0 end) as AntalRevisioner
from dbo.Anmeldelse a
left outer join dbo.AnmeldelseEffektivStatus aes on aes.AnmeldelseId = a.Id
left outer join dbo.Jord j on j.Id = a.Id
left outer join dbo.Jordforureningsopslag jfo on jfo.Id = a.Id
left outer join dbo.Gebyr g on g.Id = a.GebyrId
where a.KommuneId = @kommuneId
and a.Nummer > 0
and aes.Oprettet >= @fraDato and aes.Oprettet < @tilDato
and (@medRevisioner = 1 or a.RevisionAfAnmeldelse is null)
and j.JordKlassifikationTypeId = @kategori1
and isnull(jfo.V2, 0) = 0
and isnull(jfo.V1, 0) = 0
and isnull(jfo.OmkAnalysepligt, 0) = 0
and isnull(jfo.OmkLet, 0) = 0
and isnull(jfo.OmkRen, 0) = 0
and isnull(jfo.KommunensMiljoeDb, '') = ''
and g.Gebyrpligtig = 0
and g.FrigivetTilFakturering = 0;