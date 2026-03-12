--Jordklassifikation ny navngivning
update JordKlassifikationType set
Navn = 'Udenfor kategori',
Tooltip='Kraftig forurenet jord'
where id='08DD6EB4-9F10-4078-A1FC-56B2931839D8';

update JordKlassifikationType set
Navn = 'Kategori 2',
Tooltip='Let forurenet jord'
where id='6C0C7044-411B-4C02-906A-61B6B78EB189';

update JordKlassifikationType set
Navn = 'Kategori 1',
Tooltip='Ren jord'
where id='1426C54A-6C06-4970-AC52-327D013751C7';


update JordKlassifikationType set
Navn = 'Klasse 4',
Tooltip='Kraftig forurenet jord'
where id='6224EEA2-6691-4A4E-B7CF-8783B8799D81';

update JordKlassifikationType set
Navn = 'Klasse 3',
Tooltip='Let forurenet jord'
where id='6DDF3824-0896-404D-B339-7B0C4986FDA1';

update JordKlassifikationType set
Navn = 'Klasse 2',
Tooltip='Let forurenet jord'
where id='2CBF384F-9965-4574-8BA2-F92889F0E101';

update JordKlassifikationType set
Navn = 'Klasse 1',
Tooltip='Ren jord'
where id='C8904329-DBBC-4816-AE81-4433659D9044';

update LandsdelType
set Navn = 'OMK'
where Id='1FDC87BB-41E7-462C-8051-274C7DB14787'
