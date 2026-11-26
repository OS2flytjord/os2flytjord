/*
Tildeler (eller fjerner) rollen MasterAdmin for en bruger.
MasterAdmin giver adgang til Support-menuen i backend (fx "Skift email").

@action: 0 = vis kun, 1 = tildel MasterAdmin, 2 = fjern MasterAdmin
Brugeren skal logge ud og ind igen før menuen vises.
*/
declare
	@emailAdress varchar(max) = 'aarhus.flytjord@gmail.com',
	@action int = 1,
	@brugerid int,
	@roleid int;

select @brugerid = BrugerId from dbo.Brugerprofil where BrugerNavn = @emailAdress
select @roleid = RoleId from dbo.webpages_Roles where RoleName = 'MasterAdmin'

if (@brugerid is null)
	select 'Ingen bruger med email = ' + @emailAdress as Fejl

-- Brugerens nuværende roller
select b.BrugerId, b.BrugerNavn, r.RoleName
from dbo.Brugerprofil b
left outer join dbo.webpages_UsersInRoles ur on ur.UserId = b.BrugerId
left outer join dbo.webpages_Roles r on r.RoleId = ur.RoleId
where b.BrugerId = @brugerid

if (@action = 1 and @brugerid is not null)
begin
	if not exists (select 1 from dbo.webpages_UsersInRoles where UserId = @brugerid and RoleId = @roleid)
	begin
		insert into dbo.webpages_UsersInRoles (UserId, RoleId) values (@brugerid, @roleid)
		select CAST(getDate() as varchar(max)) + ' : Tildelt MasterAdmin til bruger med Id = ' + CAST(@brugerid as varchar(max)) + ', email = ' + @emailAdress
	end
	else
		select 'Bruger har allerede MasterAdmin' as Info
end;

if (@action = 2 and @brugerid is not null)
begin
	delete from dbo.webpages_UsersInRoles where UserId = @brugerid and RoleId = @roleid
	if (@@ROWCOUNT > 0)
		select CAST(getDate() as varchar(max)) + ' : Fjernet MasterAdmin fra bruger med Id = ' + CAST(@brugerid as varchar(max)) + ', email = ' + @emailAdress
end;

-- Alle MasterAdmins
select b.BrugerId, b.BrugerNavn
from dbo.webpages_UsersInRoles ur
join dbo.Brugerprofil b on b.BrugerId = ur.UserId
where ur.RoleId = @roleid
order by b.BrugerNavn


/*
Ændringslog:
*/
select *
from Log l
join Delta d on d.LogId = l.Id
where l.PersonId is not null
order by l.Dato desc, d.Navn
