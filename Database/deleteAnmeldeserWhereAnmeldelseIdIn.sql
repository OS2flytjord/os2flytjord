--delete anmeldelse



delete d from Dokumentation d inner join Jord j on j.id = d.JordId
inner join anmeldelse a on j.Id = a.Id
where a.Id in('611C798A-DCF5-4777-B37F-5E1C63F3CC8C');

delete j from Jord j inner join anmeldelse a on j.Id = a.Id
where a.Id in('611C798A-DCF5-4777-B37F-5E1C63F3CC8C');

delete m from Matrikel m inner join Anmeldelse a on m.OprindelsesstedId = a.id
where a.Id in('611C798A-DCF5-4777-B37F-5E1C63F3CC8C');

delete o from Oprindelsessted o inner join anmeldelse a on o.Id = a.Id
where a.Id in('611C798A-DCF5-4777-B37F-5E1C63F3CC8C');

delete p from PlanlagteStikproever p inner join anmeldelse a on p.AnmeldelseId = a.Id
where a.Id in('611C798A-DCF5-4777-B37F-5E1C63F3CC8C');

delete s from StatusAnmeldelse s inner join anmeldelse a on s.AnmeldelseId = a.Id
where a.Id in('611C798A-DCF5-4777-B37F-5E1C63F3CC8C');

delete ss from StatusStikproeve ss inner join Stikproeve s on ss.StikproeveId =s.id
inner join Vognlaes v on v.Id = s.VognlaesId
inner join anmeldelse a on v.AnmeldelseId= a.Id
where a.Id in('611C798A-DCF5-4777-B37F-5E1C63F3CC8C');


delete s from Stikproeve s 
inner join Vognlaes v on v.Id = s.VognlaesId
inner join anmeldelse a on v.AnmeldelseId= a.Id
where a.Id in('611C798A-DCF5-4777-B37F-5E1C63F3CC8C');


delete v from Vognlaes v inner join anmeldelse a on v.AnmeldelseId= a.Id
where a.Id in('611C798A-DCF5-4777-B37F-5E1C63F3CC8C');

delete j from Jordforureningsopslag j 
inner join anmeldelse a on j.Id= a.Id
where a.Id in('611C798A-DCF5-4777-B37F-5E1C63F3CC8C');

delete al from Alarm al inner join Anmeldelse a on al.AnmeldelseId = a.Id
where a.Id in('611C798A-DCF5-4777-B37F-5E1C63F3CC8C');

delete ad from Advis ad inner join anmeldelse a on ad.AnmeldelseId = a.Id
where a.Id in('611C798A-DCF5-4777-B37F-5E1C63F3CC8C');

delete d from Delta d inner join Log l on d.LogId = l.Id
inner join Anmeldelse a on a.Id = l.AnmeldelseId
where a.Id in('611C798A-DCF5-4777-B37F-5E1C63F3CC8C');

delete l from Log l inner join Anmeldelse a on a.Id = l.AnmeldelseId
where a.Id in('611C798A-DCF5-4777-B37F-5E1C63F3CC8C');

delete i from Interesant i inner join Anmeldelse a on a.Id =i.AnmeldelseId
where a.Id in('611C798A-DCF5-4777-B37F-5E1C63F3CC8C');

delete k from Kommunikation k inner join Anmeldelse a on a.Id = k.AnmeldelseId
where a.Id in('611C798A-DCF5-4777-B37F-5E1C63F3CC8C');

delete a from Anmeldelse a where a.Id in('611C798A-DCF5-4777-B37F-5E1C63F3CC8C');

