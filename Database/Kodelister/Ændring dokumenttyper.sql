use Jordflytning;

update DokumentationType set Navn = 'Analyserapport (PDF)' where Id = '36A1F939-98D4-4151-9656-754385A9DD8A';

update DokumentationType set Navn = 'Analyserapport (Excel)' where Id = 'D720EEEB-3AEF-4624-AE9C-D35124A385E7';

update DokumentationType set Navn = 'Øvrig dokumentation' where Id = 'C272FAAA-5CB9-47BD-9089-043838D930E4';

update DokumentationType set Navn = 'Anmeldelse, anden kommune' where Id = 'E19D6576-F9A0-49A0-8863-7A2D4E26EA48';

update DokumentationType set Navn = 'Forureningsundersøgelse' where Id = 'ED903BDF-47FB-4C63-A2A9-6154A9131CE8';

insert into DokumentationType ([Navn],[Aktiv],[Sortering],[Kode]) values ('Geoteknisk undersøgelse', 1, NULL, 9);

insert into DokumentationType ([Navn],[Aktiv],[Sortering],[Kode]) values ('Jordhåndteringsplan', 1, NULL, 10);