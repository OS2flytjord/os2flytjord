SELECT * FROM [dbo].[Forureningskomponent] where Kode in ('347', '2455')

insert into [dbo].[Forureningskomponent]
(Id, [Navn], Udloebsdato, Sortering, Kode)
values
(NEWID(), 'Tin', null, null, '347')

insert into [dbo].[Forureningskomponent]
(Id, [Navn], Udloebsdato, Sortering, Kode)
values
(NEWID(), 'Phenoler', null, null, '2455')