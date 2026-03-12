--Opret ny Jordmodtageranlægadministrator

--1) Opret bruger på ekstern side.

--2) Manuel knyt rettigheder og organisation til person
select * from BrugerProfil b where b.BrugerNavn='adminbgstones@flytjord.dk'
--BrugerId=96
select * from webpages_Membership where UserId=109;

insert into dbo.webpages_UsersInRoles (UserId,RoleId) values(109,3);
update webpages_Membership set IsConfirmed=1 where UserId=109;

--3) Relatation mellem person og jordmodtager
select * from Jordmodtager 
where anvenderJF =1 
order by navn-- Tjekker lige om jordmotageren er aktiv eller skal den være det.

update Jordmodtager set AnvenderJF=1 where Id='0F5950E3-7E0D-4900-889C-A63534D68648'

select id from Person where BrugerId=109;
--27062E94-B758-4802-8C50-F22CBEBCFA40
insert into PersonJordmodtager (PersonId,JordmodtagerId)
values ('B96343E3-DC83-4386-B629-9887005C5F0D','0F5950E3-7E0D-4900-889C-A63534D68648');

