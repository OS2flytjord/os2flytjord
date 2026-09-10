exec dbo.[SP_SELECT_Anmeldelse_Soegning]
@KommuneId = 'ff8aee8d-3512-4e7e-94a4-51b8beb3c732',
@ModtagerAnlaegXmlList = null,
@FoerDate = null,
@EfterDate = null,
@LoebeNr = 122281,
@ModtagerId = null,
@TransportoerId = null,
@AnmelderId = null,
@Forureningskategori = null


--kRÆVER HANDLING

declare	
	@kommunid uniqueidentifier = 'ff8aee8d-3512-4e7e-94a4-51b8beb3c732' --Den kommune som login user er tilknyttet
	, @nummer int = 122281

select * from (
select a.Nummer
,(select top 1 Kode from StatusAnmeldelse sa
left outer join StatusAnmeldelseType sat on sat.Id = sa.StatusAnmeldelseTypeId
where sa.AnmeldelseId = a.Id
order by sa.Tid desc
) as StatusAnmeldelseKode
,
(SELECT    
    latest.DateTimeField    
FROM dbo.AnmeldelseEffektivStatus AS aes
OUTER APPLY
(
    SELECT TOP (1)
        v.DateTimeField,
        v.DateTimeValue AS LatestDateTime
    FROM (VALUES
        ('Revideret',                aes.Revideret),
        ('Afsendt',                  aes.Afsendt),
        ('Oprettet',                 aes.Oprettet)
    ) AS v(DateTimeField, DateTimeValue)
    WHERE v.DateTimeValue IS NOT NULL
    ORDER BY v.DateTimeValue DESC
) AS latest
WHERE aes.AnmeldelseId = a.Id)
as LatestAnmeldelseValueField
, 
(SELECT    
    latest.DateTimeField    
FROM dbo.AnmeldelseEffektivStatus AS aes
OUTER APPLY
(
    SELECT TOP (1)
        v.DateTimeField,
        v.DateTimeValue AS LatestDateTime
    FROM (VALUES
        ('UnderBehandling',          aes.UnderBehandling),
        ('Afvist',                   aes.Afvist),
        ('Godkendt',                 aes.Godkendt)        
    ) AS v(DateTimeField, DateTimeValue)
    WHERE v.DateTimeValue IS NOT NULL
    ORDER BY v.DateTimeValue DESC
) AS latest
WHERE aes.AnmeldelseId = a.Id)
as LatestKommuneValueField

, aes.*
from anmeldelse a
left outer join AnmeldelseEffektivStatus aes on aes.AnmeldelseId = a.Id
where a.KommuneId = @kommunid and a.Nummer > 0 
) q
where (q.Nummer = @nummer)
--where q.StatusAnmeldelseKode = 11
order by q.Nummer desc

select * from StatusAnmeldelse sa
left outer join StatusAnmeldelseType sat on sat.Id = sa.StatusAnmeldelseTypeId
where sa.AnmeldelseId = '45FF9223-1335-40C7-A9B5-F3ABBD782645'
order by sa.Tid desc

--insert into StatusAnmeldelse
--(Id,AnmeldelseId,StatusAnmeldelseTypeId,PersonId,Tid)
--values
--(NEWID(), '45FF9223-1335-40C7-A9B5-F3ABBD782645', 'FE83DA7A-9593-4321-8291-20FC32A5E854', 'C650AFB1-E5DE-4777-883F-2903FA1F9C44', GETDATE())
--update AnmeldelseEffektivStatus set Afsendt = GETDATE() where AnmeldelseId = '45FF9223-1335-40C7-A9B5-F3ABBD782645'
--update AnmeldelseEffektivStatus set Afsendt = Gemt where AnmeldelseId = '45FF9223-1335-40C7-A9B5-F3ABBD782645'
--update StatusAnmeldelse set Tid =DATEADD(HOUR, 2, GETDATE()) where Id = '67A34A60-2532-4215-A917-9318B366409B' 


--delete from StatusAnmeldelse where Id in ('4E8A4577-81B4-46DE-8B83-DFD15577BC92','8116D856-EFDF-4FFB-85F1-7D662FCE3B31')