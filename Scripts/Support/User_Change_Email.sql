declare
	@fromEmailAdress varchar(max) = 'johss@c.dk',
	@toEmailAdress varchar(max) = 'mail@johs-s.dk',
	@update int = 1,
	@fuldSoegning int = 0 -- 1 = søg efter fra-email i alle tekstkolonner i databasen (tager ca. 2 min.), 0 = kun kendte felter

--update [dbo].[webpages_Membership] set PasswordFailuresSinceLastSuccess = 0, LastPasswordFailureDate = null where UserId = 1623

--Tjek for om email findes i

select * from dbo.Brugerprofil b
left outer join [dbo].[webpages_Membership] wm on wm.UserId = b.BrugerId
where BrugerNavn = @fromEmailAdress

select * from dbo.Person where Email = @fromEmailAdress

-------------------------------------------------------------------------------
-- Tjek: hvor står @fromEmailAdress direkte i databasen (som tekst, ikke som henvisning til personen)?
-- Søger i ALLE tekstkolonner i alle tabeller (LIKE '%email%', så også lister som 'a@x.dk;b@y.dk' findes).
-- Kun BrugerProfil.BrugerNavn og Person.Email opdateres af scriptet - resten skal vurderes manuelt.
-- Kendte felter: ModtagerAnlaeg.KontaktpersonEmail (bruges til opslag af kontaktperson/advis),
--                Interesant.Email (interessenter på anmeldelser), Betaleringsoplysning.SendesTilEmail.
-------------------------------------------------------------------------------
begin
	declare
		@soeg nvarchar(max) = N'%' + replace(replace(replace(@fromEmailAdress, '[', '[[]'), '%', '[%]'), '_', '[_]') + N'%',
		@sql nvarchar(max),
		@skema sysname, @tabel sysname, @kolonne sysname, @fuldtNavn nvarchar(300);

	if object_id('tempdb..#EmailFund') is not null drop table #EmailFund
	create table #EmailFund (Tabel nvarchar(300), Kolonne sysname, Antal int)

	declare kol cursor local fast_forward for
		select s.name, t.name, c.name
		from sys.columns c
		join sys.tables t on t.object_id = c.object_id
		join sys.schemas s on s.schema_id = t.schema_id
		join sys.types ty on ty.user_type_id = c.user_type_id
		where ty.name in ('varchar', 'nvarchar', 'char', 'nchar', 'text', 'ntext')
		  and (c.max_length = -1 or c.max_length >= 6) -- kortere kolonner kan ikke indeholde en email
		  and t.name not in ('NLogEntries') -- teknisk applikationslog (millioner af rækker) - gør søgningen meget langsom

	open kol
	fetch next from kol into @skema, @tabel, @kolonne
	while @@FETCH_STATUS = 0 and @fuldSoegning = 1
	begin
		set @sql = N'insert into #EmailFund select @t, @k, count(*) from ' + quotename(@skema) + N'.' + quotename(@tabel)
		         + N' where ' + quotename(@kolonne) + N' like @soeg having count(*) > 0'
		set @fuldtNavn = @skema + N'.' + @tabel
		exec sp_executesql @sql, N'@t nvarchar(300), @k sysname, @soeg nvarchar(max)', @fuldtNavn, @kolonne, @soeg
		fetch next from kol into @skema, @tabel, @kolonne
	end
	close kol
	deallocate kol

	select 'Findes i (fra email): ' + @fromEmailAdress as Info, Tabel, Kolonne, Antal,
		case
			when Tabel = 'dbo.BrugerProfil' and Kolonne = 'BrugerNavn' then 'Opdateres af scriptet'
			when Tabel = 'dbo.Person' and Kolonne = 'Email' then 'Opdateres af scriptet'
			when Tabel in ('dbo.Log', 'dbo.Delta') then 'Historik - skal ikke rettes'
			else 'OBS: opdateres IKKE af scriptet - vurder manuelt'
		end as Handling
	from #EmailFund
	order by Tabel, Kolonne

	-- Detaljer for de kendte tekstfelter
	select 'ModtagerAnlaeg' as Tabel, Id, Navn, KontaktpersonEmail from dbo.ModtagerAnlaeg where KontaktpersonEmail = @fromEmailAdress
	select 'Interesant' as Tabel, i.Id, i.AnmeldelseId, i.Navn, i.Email from dbo.Interesant i where i.Email = @fromEmailAdress
	select 'Betaleringsoplysning' as Tabel, b.Id, b.AnmeldelseId, b.SendesTilEmail from dbo.Betaleringsoplysning b where b.SendesTilEmail like @soeg

	-- Konflikt: er den nye email allerede i brug?
	if exists (select 1 from dbo.Brugerprofil where BrugerNavn = @toEmailAdress)
		or exists (select 1 from dbo.Person where Email = @toEmailAdress)
		select 'ADVARSEL: ' + @toEmailAdress + ' er allerede i brug som BrugerNavn eller Person.Email - ret ikke uden at undersøge!' as Konflikt
end

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
Nov 24 2026 10:13AM : Opdateret bruger med Id = 283 fra email = johss@c.dk, til email = mail@johs-s.dk
Nov 24 2026 10:13AM : Opdateret person med Id = 83DCE105-E338-4A76-A1D3-05417F2925F7 fra email = johss@c.dk, til email = mail@johs-s.dk
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


