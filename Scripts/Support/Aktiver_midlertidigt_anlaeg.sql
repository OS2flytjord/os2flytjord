/*
	JCC 17/11-2026
		- Anmelder kan ikke revidere anmeldelse 123759: "Det tilknyttede modtageanlæg er inaktivt".
		  Modtageranlægget er oprettet af anmelder som midlertidigt anlæg. Sådanne anlæg oprettes med Aktiv = 0
		  og bliver kun aktiveret, når den anden kommune svarer via AndenKommuneSvar.
		  Revisionsknappen spærres når ModtagerAnlaeg.Aktiv = 0 eller Jordmodtager.Aktiv = 0
		  (AnmeldelserController.CreateOpretModel).
		  Anlæg med AnmelderOprettetMidlertidigtAnlaeg = 1 vises ikke i backend og ikke i anlægslister for andre anmeldere,
		  så aktivering påvirker kun anmeldelser der allerede peger på anlægget.

	JCC 18/11-2026
		- Kørt på drift miljø
*/

declare
	@anmeldelses_nummer int = 123759
	, @modtagerAnlaegId uniqueidentifier
	, @jordmodtagerId uniqueidentifier
	;

select @modtagerAnlaegId = a.ModtagerAnlaegId
from Anmeldelse a
where a.Nummer = @anmeldelses_nummer;

select @jordmodtagerId = ma.JordmodtagerId
from ModtagerAnlaeg ma
where ma.Id = @modtagerAnlaegId;

-- Kontrol før rettelse
select ma.Id, ma.Navn, ma.Adresse, ma.Postnummer, ma.Aktiv, ma.AktivFra, ma.AktivTil
	, ma.AnmelderOprettetMidlertidigtAnlaeg, ma.CentraltOprettetMidlertidigtAnlaeg
	, jm.Id as JordmodtagerId, jm.Navn as JordmodtagerNavn, jm.Aktiv as JordmodtagerAktiv
from ModtagerAnlaeg ma
left outer join Jordmodtager jm on jm.Id = ma.JordmodtagerId
where ma.Id = @modtagerAnlaegId;

-- Andre anmeldelser der bruger samme anlæg (bør kun være denne og dens revisioner)
select a.Nummer, a.Id, a.RevisionAfAnmeldelse
from Anmeldelse a
where a.ModtagerAnlaegId = @modtagerAnlaegId;

/*
begin tran;

update ModtagerAnlaeg set Aktiv = 1 where Id = @modtagerAnlaegId and AnmelderOprettetMidlertidigtAnlaeg = 1;
update Jordmodtager  set Aktiv = 1 where Id = @jordmodtagerId;

-- commit tran;
-- rollback tran;

*/

