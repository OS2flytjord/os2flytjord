select
  sta.Navn,
  a.Tid,
  p.Navn + '' + p.Efternavn
from dbo.StatusAnmeldelse a
LEFT JOIN dbo.Person p
ON a.PersonId = p.Id
LEFT JOIN dbo.StatusAnmeldelseType sta
ON a.StatusAnmeldelseTypeId = sta.Id
where 
  a.AnmeldelseId = '437D29C6-DBA1-4ED5-895B-CF964A1D0F7D'
order by a.Tid desc