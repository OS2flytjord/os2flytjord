
--Tjek for om email findes i 

select * from dbo.Brugerprofil b
left outer join [dbo].[webpages_Membership] wm on wm.UserId = b.BrugerId
where BrugerNavn = 'info@ds-jord.dk'

select * from dbo.Person where email = 'info@ds-jord.dk'

update person set Email = 'info@ds-jord.dk' where id = '1DDF4376-C32F-49D9-9305-75FC4B73A3F4'

/*
Ændringslog:

24/8-2026 : update person set Email = 'info@ds-jord.dk' where id = '1DDF4376-C32F-49D9-9305-75FC4B73A3F4' --Ændret email for person også....
21/8-2026 : update [dbo].[webpages_Membership] set IsConfirmed = 0 where UserId = 943 --hj@frisesdahl.dk sat inaktiv
18/8-2026 : Ændret fra Info@jord-ds.dk til info@ds-jord.dk (BrugerId : 5126)
18/8-2026 : update [dbo].[webpages_Membership] set IsConfirmed = 1 where UserId = 2451 --'mok@dge.dk'


*/


