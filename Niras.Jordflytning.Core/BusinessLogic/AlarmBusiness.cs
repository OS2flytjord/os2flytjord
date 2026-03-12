using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Core.BusinessLogic
{
  public class AlarmBusiness : GenericBusiness<Alarm>, IAlarmBusiness
  {
    private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.AlarmBusiness");
    private readonly IAdviseringBusiness _adviseringBusiness;
    private readonly IAlarmRepository _alarmRepository;
    private readonly IBeskedRepository _beskedRepository;

    public AlarmBusiness(IUnitOfWork uow, IAlarmRepository alarmRepository, IAdviseringBusiness adviseringBusiness, IBeskedRepository beskedRepository)
      : base(alarmRepository, uow)
    {
      _adviseringBusiness = adviseringBusiness;
      _alarmRepository = alarmRepository;
      _beskedRepository = beskedRepository;
    }


    public IList<Alarm> CreateAlarmMaengdeKoertJord(Anmeldelse anmeldelse)
    {
      var alarmerPersonlig = new List<Alarm>();
      var alarmerAnmeldelse = new List<Alarm>();

      var alarmTekst = "% af jorden er nu kørt til " + anmeldelse.ModtagerAnlaeg.Navn;

      if (anmeldelse.ModtagerAnlaeg != null && anmeldelse.ModtagerAnlaeg.AnvenderJF)
      {
        //Anmelder - Alarm angivet på profilen
        if (anmeldelse.Anmelder != null && anmeldelse.Anmelder.Person != null && anmeldelse.Anmelder.Person.KoertJordAlarm.HasValue)//anmeldelse.Anmelder.Person.KoertJordAlarm.HasValue så er vil han have personligalarmer
        {
          var aAnmelder = new Alarm();
          aAnmelder.KoertJordAlarm = anmeldelse.Anmelder.Person.KoertJordAlarm.Value;
          aAnmelder.Person = anmeldelse.Anmelder.Person;
          aAnmelder.Besked = new Besked { Tekst = anmeldelse.Anmelder.Person.KoertJordAlarm + alarmTekst, Tid = DateTime.Now };
          alarmerPersonlig.Add(aAnmelder);
        }

        //Anmelder - Alarm angivet anmeldelsen 
        if (anmeldelse.KoertJordAlarm.HasValue // der skal være angivet en procentsats på kommunikationsfanen ved oprettelse af anmeldelsen.
          && anmeldelse.Anmelder != null && anmeldelse.Anmelder.Person != null && anmeldelse.Anmelder.Person.FrivilligeAdvis //Anmelder skal ønske frivillig advis
          ) 
        {
          var a = new Alarm();
          a.KoertJordAlarm = anmeldelse.KoertJordAlarm.Value;
          a.Person = anmeldelse.Anmelder.Person;
          a.Besked = new Besked { Tekst = anmeldelse.KoertJordAlarm.Value + alarmTekst, Tid = DateTime.Now };
          alarmerAnmeldelse.Add(a);
        }

        //Betaler - Alarm angivet på profilen
        if (anmeldelse.Betaler != null && anmeldelse.Betaler.Person != null && anmeldelse.Betaler.Person.KoertJordAlarm.HasValue)
        {
          var findesAlarmTilPerson = (from a in alarmerPersonlig where a.Person == anmeldelse.Betaler.Person select a.Person).FirstOrDefault();
          if (findesAlarmTilPerson == null)
          {
            
            var aBetaler = new Alarm();
            aBetaler.KoertJordAlarm = anmeldelse.Betaler.Person.KoertJordAlarm.Value;
            aBetaler.Person = anmeldelse.Betaler.Person;
            aBetaler.Besked = new Besked { Tekst = anmeldelse.Betaler.Person.KoertJordAlarm + alarmTekst, Tid = DateTime.Now };

            alarmerPersonlig.Add(aBetaler);
          }
        }

        //Betaler - Alarm angivet anmeldelsen 
        if (anmeldelse.KoertJordAlarm.HasValue // der skal være angivet en procentsats på kommunikationsfanen ved oprettelse af anmeldelsen.
          && anmeldelse.Betaler != null && anmeldelse.Betaler.Person != null && anmeldelse.Betaler.Person.FrivilligeAdvis //Betaler skal ønske frivillig advis
          )
        {
          var findesAnmeldelseAlarmTilPerson = (from a in alarmerAnmeldelse where a.Person == anmeldelse.Betaler.Person select a.Person).FirstOrDefault();
          if (findesAnmeldelseAlarmTilPerson == null)
          {
            var a = new Alarm();
            a.KoertJordAlarm = anmeldelse.KoertJordAlarm.Value;
            a.Person = anmeldelse.Betaler.Person;
            a.Besked = new Besked { Tekst = anmeldelse.KoertJordAlarm + alarmTekst, Tid = DateTime.Now };

            alarmerAnmeldelse.Add(a);
          }
        }

        //Transportør - Alarm på profilen
        if (anmeldelse.Transportoer != null && anmeldelse.Transportoer.Person != null && anmeldelse.Transportoer.Person.KoertJordAlarm.HasValue)
        {
          var findesAlarmTilPerson = (from a in alarmerPersonlig where a.Person == anmeldelse.Transportoer.Person select a.Person).FirstOrDefault();
          if (findesAlarmTilPerson == null)
          {
            //Alarm angivet på profilen
            var aTrans = new Alarm();
            aTrans.KoertJordAlarm = anmeldelse.Transportoer.Person.KoertJordAlarm.Value;
            aTrans.Person = anmeldelse.Transportoer.Person;
            aTrans.Besked = new Besked { Tekst = anmeldelse.Transportoer.Person.KoertJordAlarm + alarmTekst, Tid = DateTime.Now };

            alarmerPersonlig.Add(aTrans);
          }
        }

        //Transportør - Alarm angivet på anmeldelsen
        if (anmeldelse.KoertJordAlarm.HasValue // der skal være angivet en procentsats på kommunikationsfanen ved oprettelse af anmeldelsen.
          && anmeldelse.Transportoer != null && anmeldelse.Transportoer.Person != null && anmeldelse.Transportoer.Person.FrivilligeAdvis //Transportoer skal ønske frivillig advis
          )
        {
          var findesAnmeldelseAlarmTilPerson = (from a in alarmerAnmeldelse where a.Person == anmeldelse.Transportoer.Person select a.Person).FirstOrDefault();
          if (findesAnmeldelseAlarmTilPerson == null)
          {
            //Alarm angivet anmeldelsen   
            var a = new Alarm();
            a.KoertJordAlarm = anmeldelse.KoertJordAlarm.Value;
            a.Person = anmeldelse.Transportoer.Person;
            a.Besked = new Besked { Tekst = anmeldelse.KoertJordAlarm + alarmTekst, Tid = DateTime.Now };

            alarmerAnmeldelse.Add(a);
          }
        }
      }

      
      alarmerPersonlig.AddRange(alarmerAnmeldelse); // Alermerne slåes sammen.
      var outAlarm = new List<Alarm>();
      foreach (var alarm in alarmerPersonlig)
      {
        //Vi tjekker om der findes en alarm til samme person med samme procentsats.
        if (!outAlarm.Any(a => a.Person.Id == alarm.Person.Id && a.KoertJordAlarm == alarm.KoertJordAlarm))
        {
          //I såfald tilføjer vi den til output listen
          outAlarm.Add(alarm);
        }
      }

      return outAlarm;
    }

    /// <summary>
    /// Kaldes af bomsystem, når der kommer et vognlæs fra bomsystemet 
    /// eller hvis der tilføjes et vognlæs manuelt.
    /// </summary>
    public void SendKoertJordAlarmer(Anmeldelse anmeldelse)
    {
      //Tjekker om der er alarmer som skal sendes
      var koertJord = (
        from v in anmeldelse.Vognlaes
        select v.MaengdeTon).Sum();

      if (koertJord.Value <= 0 || anmeldelse.Jord == null || !anmeldelse.Jord.ForventetJordmaengdeTon.HasValue)
        return;

      foreach (var alarm in anmeldelse.Alarm)
      {
	      var procentDelivered = (koertJord.Value / anmeldelse.Jord.ForventetJordmaengdeTon) * 100;
				var shouldSendAlarm = (procentDelivered >= alarm.KoertJordAlarm);
				if (!shouldSendAlarm)
          continue;

        if (alarm.Udfoert.HasValue) //Tjek for om alarmen allerede er sendt engang
          continue;

        //Sender advis, da der er kørt mere jord end alarmer er sat til.
	      var alarmIsSend = _adviseringBusiness.SendAlarmAdvis(alarm);

				if (!alarmIsSend)
          continue;

        alarm.Udfoert = DateTime.Now;
        Create(alarm);
      }
    }

    public void DeleteAlarm(Alarm alarm)
    {
      var a = _alarmRepository.Read(alarm.Id);
	    if (a == null) return;

	    if (a.Besked != null)
		    _beskedRepository.Delete(a.Besked);

	    _alarmRepository.Delete(a);
	    SaveChanges();
    }
  }
}
