Alter table Jord
alter column IntaktJord bit not null;


Alter table Jord
add AntalProever numeric(3, 0) NULL;


drop index AndenOprindJordType.IDX_AndenOprindJordType_1_FK ;
ALTER TABLE AndenOprindJordType
DROP CONSTRAINT Oprindelsessted_AndenOprindJordType;
alter table AndenOprindJordType
drop column OprindelsesstedId;

Alter table Oprindelsessted
add AndenOprindJordTypeID Uniqueidentifier;
Alter Table Oprindelsessted
ADD FOREIGN KEY (AndenOprindJordTypeID)
REFERENCES AndenOprindJordType(Id);
Create index IDX_Oprindelsessted_2_FK on Oprindelsessted(AndenOprindJordTypeID);

alter table Konfig
alter column Value nvarchar(1000);

alter table Dokumentation
add OprindelsesDato datetime not null;


--Renaming StatusAnmeldelseType
sp_RENAME 'StatusAnmeldelseType.[Sortering]' , 'Ident', 'COLUMN'

--Dokumentation
ALTER TABLE Dokumentation
DROP COLUMN sti;