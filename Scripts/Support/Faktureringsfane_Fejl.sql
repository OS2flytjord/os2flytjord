/*
Fejlen opstår fordi datoen FrigivetTilFaktureringDato var null
Løsning:
Sætter FrigivetTilFaktureringDato lige med beslutningsdato i gebyr tabel.
*/

select * from Kommune where Navn like 'Aarhu%'

select g.FrigivetTilFaktureringDato, g.Id, BeslutningsDato, * from dbo.Anmeldelse a
left outer join dbo.Gebyr g on a.GebyrId = g.Id
where a.KommuneId  = 'A15D5888-BC70-4206-871E-A47A0C05DECC'
and g.Id is not null
and g.Gebyrpligtig = 1
and g.FrigivetTilFakturering = 1
and g.FaktureringUdtraekId is null
order by g.FrigivetTilFaktureringDato

update dbo.Gebyr set FrigivetTilFaktureringDato = BeslutningsDato where Id in ('AC232A44-949F-4DCD-B36E-3FDF3281CA8A')


/*
Log:

JCC 21/8-2026 : update dbo.Gebyr set FrigivetTilFaktureringDato = BeslutningsDato where Id in ('AC232A44-949F-4DCD-B36E-3FDF3281CA8A')

*/