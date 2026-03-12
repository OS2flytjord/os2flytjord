Alter table person
alter column Aktiv bit not null;

Alter table ModtagerAnlaeg
add Affald bit;
Update ModtagerAnlaeg set Affald=1;
Alter table ModtagerAnlaeg
alter column Affald bit not null;


Update ModtagerAnlaeg set AnvenderJF=0 where AnvenderJF is null ;
Alter table ModtagerAnlaeg
alter column AnvenderJF bit not null;

Alter table ModtagerAnlaeg
alter column Aktiv bit not null;

-- Retter fejl i Matrikel tabellen hvor newid er sat på Herred og ikke Id.
ALTER TABLE [dbo].[Matrikel] ADD  CONSTRAINT [DEF_Matrikel_Id]  DEFAULT (newid()) FOR [Id]
ALTER TABLE [dbo].[Matrikel] DROP CONSTRAINT [DEF_Matrikel_Herred]

alter table oprindelsessted
drop column Matrikelnr;

alter table oprindelsessted
drop column Ejerlav;


--Alter Kommune
Alter table Kommune
add Kommunenr smallint;
--Opdatere test data
Update Kommune set Kommunenr = 751 where Id='9DCD3EA6-696D-4939-AF01-FDB3369D3194';--Århus Kommune
Update Kommune set Kommunenr = 101 where Id='DEC0C70B-78B7-4B2F-8487-2FF762ECB735';--København Kommune
Update Kommune set Kommunenr = 846 where Id='91A6A417-5F8B-4767-9B56-14F557A22212';--Mariagerfjord Kommune
--
Alter table Kommune
Alter column Kommunenr smallint not null;

--Alter JordKlassifikationType
--update ModtagerAnlaeg set JordKlassifikationTypeId=null
--delete from JordKlassifikationType
Alter table JordKlassifikationType
Add MiljoeportalOmkKey smallint not null;
Alter table JordKlassifikationType
Add MiljoeportalOmkNavn varchar(50) not null;
Alter table JordKlassifikationType
Add Priotering smallint not null;
--Indlæs JordklassifikationTyperne igen (04_kodelister.sql)
--Tildel random modtageranlaeg til jordklassifikationttype.(04_kodelister.sql)
