/*
	JCC 1/10-2026 
		- Dette script ændrer opgavetypen der er tilknyttet til anmeldelser fra nogle gamle til de nuværende
		- Dette er en bug der skal rettes således det ikke er de gamle der bliver tilknyttet.
	
*/


--BEMÆRK: OpgavetypeId'erne er kommunespecifikke, pt. for Aarhus. DVS. til skal tilrettes. Ligeledes skal datoen tilrettes
--Opstartsgebyr
--Inden update kan denne SQL bruges lige at se hvad kommer til at blive opdateret

--Opgavetyper
select * from opgavetype ot
left outer join  kommune k on k.Id = ot.KommuneId
where KommuneNr = 751 --Århus
--where KommuneNr = 730 --Randers
--where KommuneNr = 265 --Roskilde

--Nedenstående er for Århus kommune

begin transaction updateopgave
/*
commit transaction updateopgave
rollback transaction updateopgave
*/

--Tid 2023/2024 (631.2 Kr. -> 959 Kr.) 28 stk.
select * from [dbo].[GebyrTidsregistrering] where [Dato] > '2026-01-01' and [OpgavetypeId] = 'E6E26074-EB97-4B55-BA9C-844A24374E5F'

update [dbo].[GebyrTidsregistrering] set [OpgavetypeId] = '31072201-ABF2-4CAA-BAF3-39DEA8649AFB' where [Dato] > '2026-01-01' and [OpgavetypeId] = 'E6E26074-EB97-4B55-BA9C-844A24374E5F'


--Opstartsgebyr 2023/2024 (151 Kr. -> 176 Kr.) 23 stk.
select * from [dbo].[GebyrTidsregistrering] where [Dato] > '2026-01-01' and [OpgavetypeId] = 'EF47579A-2D98-45A0-8463-E8E085B28261'

update [dbo].[GebyrTidsregistrering] set [OpgavetypeId] = '8B1EF466-F04F-4D80-8916-94328D8323CE' where [Dato] > '2026-01-01' and [OpgavetypeId] = 'EF47579A-2D98-45A0-8463-E8E085B28261'



--Tid 2025
select * from [dbo].[GebyrTidsregistrering] where [Dato] > '2026-01-01' and [OpgavetypeId] = '756F600A-F9EC-4D7C-8A4F-7AE026431242'

update [dbo].[GebyrTidsregistrering] set [OpgavetypeId] = '31072201-ABF2-4CAA-BAF3-39DEA8649AFB' where [Dato] > '2026-01-01' and [OpgavetypeId] = '756F600A-F9EC-4D7C-8A4F-7AE026431242'


--Opstartsgebyr 2025
select * from [dbo].[GebyrTidsregistrering] where [Dato] > '2026-01-01' and [OpgavetypeId] = '669553EC-ABDE-4A6B-8F01-FF611C9D4CFD'

update [dbo].[GebyrTidsregistrering] set [OpgavetypeId] = '8B1EF466-F04F-4D80-8916-94328D8323CE' where [Dato] > '2026-01-01' and [OpgavetypeId] = '669553EC-ABDE-4A6B-8F01-FF611C9D4CFD'

