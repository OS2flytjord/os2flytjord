/*
Hej Jacob

Tak for udtrækket.
Efter det indledende arbejde med data har vi konstateret, at der er behov for et udvidet udtræk, da der er nogle flere parametre vi gerne ville have med, som ville gøre det hele lidt lettere for os.

Hvad vi skal bruge:
Alle anmeldelser som både er udenfor kategori og intakt jord de sidste 3 år

Informationer der skal med i udtrækket:
Løbenummer
Dato oprettet
Adresse (oprindelsessted)
Modtager
Jordmægde (t)
Om der er udtaget prøver eller det er kartering
Antal prøver i alt

Vi håber du har mulighed for at generere et sådan udtræk.


Med venlig hilsen

Jakob Skovsgaard
Miljøsagsbehandler

T + 45 4185 5801
E  sjaw@aarhus.dk



From: Jakob Washington Skovsgaard <sjaw@aarhus.dk> 
Sent: 9. oktober 2026 11:22
To: Jacob Christiansen (JCC) <JCC@NIRAS.DK>
Cc: Info-support <Info-support@NIRAS.DK>; Mette Erbou Hansen <meeh@aarhus.dk>
Subject: Liste - udtræk fra Flytjord

Hej Jacob
Vi har til en opgave, vi arbejder med i Jordgruppen i Aarhus, brug for en liste med anmeldelser i Flytjord.
Det drejer sig om anmeldelser af jordflytninger i Aarhus Kommune, som er registreret som 
•	Uden for kategori
•	Intakt jord
•	I perioden 1/1 2024 til i dag

Vil du lave udtrækket og sende det til mig?
Pft.

Med venlig hilsen

Jakob Skovsgaard
Miljøsagsbehandler

T + 45 4185 5801
E  sjaw@aarhus.dk
*/

declare
	@kommuneNavn varchar(max) = 'Aarhus'
	, @oprettetEfter datetime = '20240101'
	, @JordklassifikationsType uniqueidentifier = '08DD6EB4-9F10-4078-A1FC-56B2931839D8' --Uden for kategori
	, @intaktJord int = 1
	, @kommuneId uniqueidentifier
	;

select @kommuneId = id from kommune where [Navn] = @kommuneNavn

select a.Nummer, ae.Oprettet 
, os.Adresse, os.Postnummer, os.Postdistrikt  --Oprindelsessted
, m.Nummer as ModtagerNummer, m.Navn as ModtagerNavn, m.Adresse as ModtagerAdresse, m.Postnummer as ModtagerPostNummer, m.PostDistrikt as ModtagerPostDistrikt --Modtager
,j.IntaktJord, jkt.Navn as JordKlasificering, j.ForventetJordmaengdeTon as ForventetJordMaengde
, j.Jordproever as UdtagetJordProeve, j.AntalProever as JordAntalProever 
from Anmeldelse a
left outer join [dbo].[AnmeldelseEffektivStatus] ae on ae.AnmeldelseId = a.Id
left outer join dbo.Jord j on j.Id = a.Id
left outer join dbo.Oprindelsessted os on os.Id = a.Id
left outer join [dbo].[JordKlassifikationType] jkt on jkt.Id = j.[JordKlassifikationTypeId]
left outer join dbo.ModtagerAnlaeg m on m.Id = a.ModtagerAnlaegId
where KommuneId = @kommuneId
and ae.Oprettet >= @oprettetEfter
and j.JordKlassifikationTypeid = @JordklassifikationsType
and j.IntaktJord = @intaktJord
order by ae.Oprettet desc



