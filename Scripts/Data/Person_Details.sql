declare
	@email varchar(max) = 'Hans.k@loegtved.dk'

	,@personId varchar(max)
	;


select @personId = id from Person where Email = @email

select * from Person where Id = @personId

select * from dbo.Brugerprofil b
left outer join [dbo].[webpages_Membership] wm on wm.UserId = b.BrugerId
where BrugerNavn = @email

select atype.Navn, b.Tekst, * from Kommunikation k
left outer join Besked b on b.Id = k.BeskedId
left outer join Advis a on a.BeskedId = b.Id
left outer join AdvisType atype on atype.Id = a.AdvisTypeId
where TilPerson = @personId or FraPerson = @personId