--delete from PersonKommune where PersonId='B413D39F-AF88-43A4-9AC0-C15F72718685'
--delete from Person where Id='B413D39F-AF88-43A4-9AC0-C15F72718685'
select a.id from Anmeldelse a 
where 
a.AnmelderId in (
'E9E314A6-137D-4FDF-AC81-7D5F436D6C67'
);

select a.id from Anmeldelse a 
where 
a.BetalerId in (
'E9E314A6-137D-4FDF-AC81-7D5F436D6C67'
);


------------------------------

delete from StatusBetaler
where BetalerId in (
'E9E314A6-137D-4FDF-AC81-7D5F436D6C67'
);

delete from BogholderOpslagstavle
where BetalerId in (
'E9E314A6-137D-4FDF-AC81-7D5F436D6C67'
);

delete from BemyndigedeAnmeldere
where BetalerId in (
'E9E314A6-137D-4FDF-AC81-7D5F436D6C67'
);

delete from Anmelder where id in (
'E9E314A6-137D-4FDF-AC81-7D5F436D6C67'
);

delete from Betaler where id in (
'E9E314A6-137D-4FDF-AC81-7D5F436D6C67'
);

delete from Advis where PersonId in 
('E9E314A6-137D-4FDF-AC81-7D5F436D6C67'
);

delete from Person where Id ='E9E314A6-137D-4FDF-AC81-7D5F436D6C67';

