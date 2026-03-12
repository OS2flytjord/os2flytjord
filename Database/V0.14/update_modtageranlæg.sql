USE [JordflytningTest150828v14]
GO
update ModtagerAnlaeg
set KommuneKode = CAST(['Sheet 1$'].[KOMMUNE_NR] AS INT)
from ['Sheet 1$'] inner join ModtagerAnlaeg a on  CAST(['Sheet 1$'].POSTNR AS INT) = a.Postnummer

update modtageranlaeg set Postnummer=8370  where kommunekode is null