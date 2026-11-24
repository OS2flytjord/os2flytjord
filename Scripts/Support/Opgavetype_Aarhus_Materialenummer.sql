/*
	JCC 24/11-2026
		- Retter KMDOpusMaterialeVareNummer på Opstartsgebyr 2026 for Aarhus Kommune (751).
		- Ved oprettelse af 2026-satserne fik Opstartsgebyr materialenummer 108082 (Sagsbehandling-jordflytning)
		  i stedet for 108080 (Adm.gebyr pr. anmeldelse). Derfor kom Opstartsgebyr og Administrativ tid
		  på samme postering i KMD Opus-udtrækket.
		- Korrekte materialenumre for Aarhus:
			108080 : Adm.gebyr pr.anmeldelse      (Opstartsgebyr, Kode 0)
			108082 : Sagsbehandling-jordflytning  (Administrativ tid, Kode 1)
		- Rettelsen påvirker kun udtræk fremover. Allerede udlæste fakturaer i 2026 har Opstartsgebyr på 108082.
*/

--Opgavetyper for Aarhus før rettelse
select k.KommuneNr, ot.Id, ot.Kode, ot.Navn, ot.Timepris, ot.FraDato, ot.TilDato,
       ot.KMDOpusMaterialeVareNummer, ot.KMDOpusPSPElementNummer
from Opgavetype ot
join Kommune k on k.Id = ot.KommuneId
--where k.KommuneNr = 751 --Århus
order by ot.Kode, ot.FraDato


begin transaction fixmateriale
/*
commit transaction fixmateriale
rollback transaction fixmateriale
*/

--Opstartsgebyr 2026 (108082 -> 108080) 1 stk.
update Opgavetype
set KMDOpusMaterialeVareNummer = 108080
where 
  --Id = '8B1EF466-F04F-4D80-8916-94328D8323CE' and 
  Kode = 0
  and KMDOpusMaterialeVareNummer = 108082


--Kontrol: Kode 0 skal have 108080 og Kode 1 skal have 108082
select k.KommuneNr, ot.Id, ot.Kode, ot.Navn, ot.FraDato, ot.TilDato, ot.KMDOpusMaterialeVareNummer
from Opgavetype ot
join Kommune k on k.Id = ot.KommuneId
where k.KommuneNr = 751 --Århus
order by ot.Kode, ot.FraDato
