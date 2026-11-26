/*
	JCC 20/10-2026
		- Modtageranlæg for jordmodtageren Jørgen Rasmussen Gruppen bliver ikke vist mere i FlytJord
		
		Mail til Klaus fra MariagerFjordKommune.
		"
		Angående modtageranlæg fra Jørgen Rasmussen Gruppen kan jeg se i databasen at der er sat et kryds i feltet AnmelderOprettetMidlertidigtAnlæg og det er derfor det ikke bliver vist mere.
		Jeg har fjernet dette flag således de nu vises i FlytJord, jeg kan dog ikke finde ud af hvordan dette kryds er sat, har du nogen ide til hvor og hvem der kunne sætte et sådan kryds?
		"

		"
		From: Jordgruppen, Mariagerfjord Kommune <jord@mariagerfjord.dk> 
		Sent: 20. oktober 2026 14:37
		To: Jacob Christiansen (JCC) <JCC@NIRAS.DK>
		Cc: Mette Erbou Hansen <meeh@aarhus.dk>
		Subject: SV: FlytJord

		Hej igen.
 
		Nu kommer jeg lige med en teori: 
 
		Kan du/I se om JGR nogensinde HAR været oprettet som en ”fast” jordmodtager? Det virker jo næsten som om alle nedenstående kommer fra enkeltanmeldelser, hvor anmelder har oprettet dem som midlertidigt anlæg hver gang…
 
		Nå, men lige meget hvad, så er korrekt matr.nr. 20bf, jf. godkendelsen https://dma.mst.dk/vis-sag/2308608. Så vælg bare en af 20BF’erne som aktiv.
 
 
		Venlig hilsen
 
		Claus Hallingdal Bloch
		Miljøsagsbehandler, jordforurening og virksomhedsmiljø
		"



		- Kørt på drift
*/


update ModtagerAnlaeg set AnmelderOprettetMidlertidigtAnlaeg = 1 where id in 
(select ma.id from Jordmodtager jm
left outer join ModtagerAnlaeg ma on ma.JordmodtagerId = jm.Id
where jm.Postnummer = 9220 and jm.Adresse = 'Rørdalsvej 238')


update ModtagerAnlaeg set AnmelderOprettetMidlertidigtAnlaeg = 0 where id = '0839C736-104B-4630-9897-AC3C8C9BAF09'
