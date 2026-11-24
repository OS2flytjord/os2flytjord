declare
	@fromEmailAdress varchar(max) = 'mp@fjpas.dk',
	@password varchar(max) = 'AOQvBreu2tcUQ9VrgXchY/Jn7SdMJpx9k1xCwSL+AK3IgLwqg/bcB0vMVpdy4iXUIA==', --f1y1j0rd
	@update int = 1,
	@brugerid int;


select @brugerid = b.BrugerId from dbo.Brugerprofil b
left outer join [dbo].[webpages_Membership] wm on wm.UserId = b.BrugerId
where BrugerNavn = @fromEmailAdress

select * from dbo.Brugerprofil b
left outer join [dbo].[webpages_Membership] wm on wm.UserId = b.BrugerId
where BrugerNavn = @fromEmailAdress

select * from dbo.Person where Email = @fromEmailAdress

if (@update = 1)
begin
	
	update [dbo].[webpages_Membership] set PasswordFailuresSinceLastSuccess = 0, LastPasswordFailureDate = null, [Password] = @password  where UserId = @brugerid

	select CAST(getDate() as varchar(max)) + ' : Sat password til f1y1j0rd for bruger med Id = ' + CAST(@brugerid as varchar(max)) + ' fra email = ' + @fromEmailAdress

end;


select * from dbo.Brugerprofil b
left outer join [dbo].[webpages_Membership] wm on wm.UserId = b.BrugerId
where BrugerNavn = @fromEmailAdress

select * from dbo.Person where Email = @fromEmailAdress


/*
Ændringslog:
Nov 10 2026  6:03AM : Sat password til f1y1j0rd for bruger med Id = 2751 fra email = Hans.k@loegtved.dk
Oct 16 2026  5:39AM : Sat password til f1y1j0rd for bruger med Id = 1623 fra email = mp@fjpas.dk
*/


