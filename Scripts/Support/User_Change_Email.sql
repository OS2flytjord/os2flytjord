declare
	@fromEmailAdress varchar(max) = 'mcgedved@gmail.com',
	@toEmailAdress varchar(max) = 'mc@g-e-f.dk',
	@update int = 0

--update [dbo].[webpages_Membership] set PasswordFailuresSinceLastSuccess = 0, LastPasswordFailureDate = null where UserId = 1623

--Tjek for om email findes i 

select * from dbo.Brugerprofil b
left outer join [dbo].[webpages_Membership] wm on wm.UserId = b.BrugerId
where BrugerNavn = @fromEmailAdress

select * from dbo.Person where Email = @fromEmailAdress

if (@update = 1)
begin
	
	declare
		@brugerid int,
		@personId uniqueidentifier;

	select @brugerid = b.BrugerId from dbo.Brugerprofil b
	left outer join [dbo].[webpages_Membership] wm on wm.UserId = b.BrugerId
	where BrugerNavn = @fromEmailAdress
	
	select @personId = id from dbo.Person where Email = @fromEmailAdress

	
	update BrugerProfil set BrugerNavn = @toEmailAdress where BrugerId = @brugerid
	update person set Email = @toEmailAdress where id = @personId

	select CAST(getDate() as varchar(max)) + ' : Opdateret bruger med Id = ' + CAST(@brugerid as varchar(max)) + ' fra email = ' + @fromEmailAdress + ', til email = ' + @toEmailAdress
	union
	select  CAST(getDate() as varchar(max)) + ' : Opdateret person med Id = ' + CAST(@personId as varchar(max)) + ' fra email = ' + @fromEmailAdress + ', til email = ' + @toEmailAdress

end;


select * from dbo.Brugerprofil b
left outer join [dbo].[webpages_Membership] wm on wm.UserId = b.BrugerId
where BrugerNavn = @toEmailAdress

select * from dbo.Person where Email = @toEmailAdress


/*
Ændringslog:
Nov 23 2026  7:14AM : Opdateret bruger med Id = 950 fra email = mcgedved@gmail.com, til email = mc@g-e-f.dk
Nov 23 2026  7:14AM : Opdateret person med Id = 5CC70DF8-1ACA-4532-9537-F0C721FE7C52 fra email = mcgedved@gmail.com, til email = mc@g-e-f.dk
15/10-2026 : update [dbo].[webpages_Membership] set PasswordFailuresSinceLastSuccess = 0, LastPasswordFailureDate = null where UserId = 1623
Oct  9 2026  9:27AM : Opdateret bruger med Id = 1059 fra email = jansimonsentransport@hotmail.com, til email = jansimonsentransport@outlook.dk
Oct  9 2026  9:27AM : Opdateret person med Id = BE927D09-0310-40C1-81AC-67EA8184E672 fra email = jansimonsentransport@hotmail.com, til email = jansimonsentransport@outlook.dk
Sep 24 2026  7:30AM : Opdateret bruger med Id = 5126 fra email = info@ds-jord.dk, til email = CM@ds-jord.dk
Sep 24 2026  7:30AM : Opdateret person med Id = 1DDF4376-C32F-49D9-9305-75FC4B73A3F4 fra email = info@ds-jord.dk, til email = CM@ds-jord.dk
24/8-2026 : update person set Email = 'info@ds-jord.dk' where id = '1DDF4376-C32F-49D9-9305-75FC4B73A3F4' --Ændret email for person også....
21/8-2026 : update [dbo].[webpages_Membership] set IsConfirmed = 0 where UserId = 943 --hj@frisesdahl.dk sat inaktiv
18/8-2026 : Ændret fra Info@jord-ds.dk til info@ds-jord.dk (BrugerId : 5126)
18/8-2026 : update [dbo].[webpages_Membership] set IsConfirmed = 1 where UserId = 2451 --'mok@dge.dk'
*/


