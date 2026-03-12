select * from NLogEntries n 
where n.TimeStamp between '2014-06-10 14:36:00' and '2014-06-12 14:40:00'
order by TimeStamp

select id,KoertJordAlarm from anmeldelse where Nummer=2193
select * from Alarm where anmeldelseId='D303A03A-A42A-47DC-B9F6-AD5801461DFC'

select * from person where Mobiltelefon=29208185

select * from NLogEntries n 
where n.Message like '%CreateAlarmMaengdeKoertJord%' 
and n.TimeStamp between '2014-04-01 11:14:00' and '2014-04-01 11:16:00'

