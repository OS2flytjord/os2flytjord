declare
	@fromEmailAdress varchar(max) = 'info@ds-jord.dk',
	@toEmailAdress varchar(max) = 'CM@ds-jord.dk',
	@update int = 1



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

Sep 24 2026  7:30AM : Opdateret bruger med Id = 5126 fra email = info@ds-jord.dk, til email = CM@ds-jord.dk
Sep 24 2026  7:30AM : Opdateret person med Id = 1DDF4376-C32F-49D9-9305-75FC4B73A3F4 fra email = info@ds-jord.dk, til email = CM@ds-jord.dk
24/8-2026 : update person set Email = 'info@ds-jord.dk' where id = '1DDF4376-C32F-49D9-9305-75FC4B73A3F4' --Ændret email for person også....
21/8-2026 : update [dbo].[webpages_Membership] set IsConfirmed = 0 where UserId = 943 --hj@frisesdahl.dk sat inaktiv
18/8-2026 : Ændret fra Info@jord-ds.dk til info@ds-jord.dk (BrugerId : 5126)
18/8-2026 : update [dbo].[webpages_Membership] set IsConfirmed = 1 where UserId = 2451 --'mok@dge.dk'
*/


