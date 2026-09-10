/*
select * from Firmaoplysninger
where Firmanavn like 'Roskilde%'

select * from BrugerProfil
where BrugerNavn = 'cladwa@roskilde.dk'

select * from  [dbo].[webpages_Membership] 
where UserId = 5020

select * from [dbo].[Person]
where BrugerId = 5020

select * from Jordmodtager
where Navn like 'SCT%'

select * from Firmaoplysninger
where Firmanavn like 'SCT%'
*/

--JCC 21/82026 efter forespørgelse fra Clara-Marie Dyreborg Waight fra Roskilde kommune.
update Firmaoplysninger set EAN = '5790002524370' where Firmanavn like 'SCT%'