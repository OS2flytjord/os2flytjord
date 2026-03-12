using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.BomSystem;
using Niras.Jordflytning.Library.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Niras.Jordflytning.Core.BusinessLogic
{
    public class BomSystemBusiness : IBomSystemBusiness
  {
    private readonly ILastbilRepository _lastbilRepository;
    private readonly IStatusAnmeldelseBusiness _statusAnmeldelseBusiness;
    private readonly IModtagerAnlaegBusiness _modtagerAnlaegBusiness;
    private readonly IVognlaesBusiness _vognlaesBusiness;
    private readonly IAnmeldelserBusiness _anmeldelserBusiness;
    private readonly IStatusStikproeveBusiness _statusStikproeveBusiness;

    private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.BomSystemBusiness");

    public BomSystemBusiness(
      IStatusAnmeldelseBusiness statusAnmeldelseBusiness,
      IModtagerAnlaegBusiness modtagerAnlaegBusiness,
      ILastbilRepository lastbilRepository,
      IVognlaesBusiness vognlaesBusiness,
      IAnmeldelserBusiness anmeldelserBusiness,
      IStatusStikproeveBusiness statusStikproeveBusiness)
    {
      _lastbilRepository = lastbilRepository;
      _statusAnmeldelseBusiness = statusAnmeldelseBusiness;
      _modtagerAnlaegBusiness = modtagerAnlaegBusiness;
      _vognlaesBusiness = vognlaesBusiness;
      _anmeldelserBusiness = anmeldelserBusiness;
      _statusStikproeveBusiness = statusStikproeveBusiness;
    }

        #region *** Public methods ***

        public BomAnmeldelseList GetBomAnmeldelser(int modtagerAnlaegId, DateTime tidsStempel)
        {
            string bomInfo = string.Format("Bomservice - Modtageranlæg: {0}, Input tidsStempel: {1}", modtagerAnlaegId, tidsStempel.ToLongTimeString());
            Logger.LogInfo(bomInfo);

            Logger.LogInfo(string.Format("Start: {0}", DateTime.Now.ToLongTimeString()));

            var anlist = _modtagerAnlaegBusiness.GetAnmeldelserOnModtageAnlaeg(modtagerAnlaegId, false, tidsStempel);

            var bomAnmeldList = new BomAnmeldelseList();
            var modtageAnlaegGuid = Guid.Empty;
            var now = DateTime.Now;
            var count = 0;
            foreach (var anmeldelse in anlist)
            {
                //TOK 9.6.2016: De midlertidige anmeldelser ved revision skal ikke med ud
                if (anmeldelse.RevisionAfAnmeldelse != null)
                    continue;

                // hent modtageanlæggets guid
                if (count < 1)
                    if (anmeldelse.ModtagerAnlaeg != null)
                        modtageAnlaegGuid = anmeldelse.ModtagerAnlaeg.Id;
                count++;

                // Giv de anmeldelser, hvor seneste statuskode er yngre end det givne timestamp
                // Hvis der fortages ændringer af Kommunen eller miljømedarbejder, skal ændringerne med ud til bomwebservice.
                bool hasStatusChange = _statusAnmeldelseBusiness.GetLastStatus(anmeldelse.AnmeldelseEffektivStatus, out _, out var senesteStatusTid) && senesteStatusTid > tidsStempel;

                // Giv de anmeldelser, hvor der er kommet vognlæs til anmeldelsen
                var hasVognlaesChange = false;
                var senesteVognLaesTid = _vognlaesBusiness.GetSenesteVognlaesTid(anmeldelse);
                if (senesteVognLaesTid != null && senesteVognLaesTid > tidsStempel)
                    hasVognlaesChange = true;

                // Giv kun anmeldelser, hvor der er lavet en planlagt stikprove senere end tidsstempel
                var hasStikproeveChange = false;
                var senesteStikproeveTid = GetSenesteStikproeveTid(anmeldelse);
                if (senesteStikproeveTid > tidsStempel)
                    hasStikproeveChange = true;

                // hvis der er sket ændringer på betaler-status, skal den med
                var hasBetalerChange = false;
                var senesteBetalerTid = GetSenesteBetalerTid(anmeldelse);
                if (senesteBetalerTid > tidsStempel)
                    hasBetalerChange = true;



                // hvis kørselslut dag er i dag 
                // og midnat er i perioden fra tidsstempel til nu (dvs. for at opdatere at det er sidste kørselsdag i dag )
                // og midnat næste dag er i perioden fra tidsstempel til nu (dvs. for at opdatere at at det ikke er sidste kørselsdag i dag), 
                // skal den med
                var addAnmeldelsePgaKoerselSlut = false;
                var koerselSlutDato = GetKoerselSlutDato(anmeldelse);
                var isKoerselOverskredetIdag = IsKoerselsperiodeOverskredetIdag(anmeldelse);

                if (isKoerselOverskredetIdag)
                {
                    // Efter midnat på kørselslutdatoen (dvs. om morgenen kl.00.00.01 på kørsels-slut-dagen)
                    var koerselSlutdagBegynd = new DateTime(koerselSlutDato.Year, koerselSlutDato.Month, koerselSlutDato.Day, 0, 0, 1);

                    // tag den med hvis koerselSlutdagBegynd er i perioden fra tidsstempel til nu
                    if (koerselSlutdagBegynd > tidsStempel && koerselSlutdagBegynd <= now)
                        addAnmeldelsePgaKoerselSlut = true;

                    // efter midnat dagen efter kørselslutdatoen (dvs. om morgenen næste dag kl. 00.00.01)
                    var koerselSlutdagEnd = new DateTime(koerselSlutDato.Year, koerselSlutDato.Month, koerselSlutDato.Day, 0, 0, 1);
                    koerselSlutdagEnd.AddDays(1);
                    // tag den med hvis koerselSlutdagEnd er i perioden fra tidsstempel til nu
                    if (koerselSlutdagEnd > tidsStempel && koerselSlutdagEnd <= now)
                        addAnmeldelsePgaKoerselSlut = true;
                }

                var addAnmeldelse = hasStatusChange || hasVognlaesChange || hasStikproeveChange || hasBetalerChange || addAnmeldelsePgaKoerselSlut;
                if (!addAnmeldelse)
                    continue;

                var bomAnmeldelse = MapToBomAnmeldelse(anmeldelse);
                bomAnmeldList.AnmeldelseList.Add(bomAnmeldelse);

                if (anmeldelse != null && bomAnmeldelse != null)
                    Logger.LogInfo(string.Format("Bomservice - Returneret til bom, Adresse: {0}, Lbnr: {1}", bomAnmeldelse.Oprindelsesadresse, anmeldelse.Nummer));
            }

            // Hvis ikke angivet, forsøg at finde modtageranlæg ud fra nummer til stikprøve:
            if (modtageAnlaegGuid == Guid.Empty)
                modtageAnlaegGuid = _modtagerAnlaegBusiness.ReadModtagerAnlaeg(modtagerAnlaegId).Id;

            if (modtageAnlaegGuid != Guid.Empty)
                bomAnmeldList.Stikproeve = ShouldTakeStikproeve(modtageAnlaegGuid);

            bomAnmeldList.TimeStamp = now;
            Logger.LogInfo(string.Format("Bomservice - Tidsstemple returneret til bom: {0}", bomAnmeldList.TimeStamp.ToLongTimeString()));

            Logger.LogInfo(string.Format("Slut: {0}", DateTime.Now.ToLongTimeString()));
            Logger.LogInfo(string.Format("Procestid: {0} msec", (DateTime.Now - now).TotalMilliseconds));
            return bomAnmeldList;
        }

        public List<Vognlaes> CreateVognlaes(IList<VognlaesBom> vognlaesList)
    {
      var list = new List<Vognlaes>();
      foreach (var vognlaesBom in vognlaesList)
      {
        Logger.LogInfo(
          string.Format("CreateVognlaes - Tid: {0}, AnmeldelseId: {1}, LastbilId: {2}, Stikproeve: {3}, StikproeveBaas: {4}"
          ,vognlaesBom.Tid.ToLongDateString() + " " + vognlaesBom.Tid.ToLongTimeString(), vognlaesBom.AnmeldelsesId,vognlaesBom.LastbilId,vognlaesBom.Stikproeve,vognlaesBom.StikproeveBaas));

        var vLaes = SaveVognlaes(vognlaesBom);
        list.Add(vLaes);
      }
      return list;
    }

    public List<KoereToej> GetKoeretoejer(DateTime tidsStempel)
    {
      //var lastbilList = (from lastbil in _lastbilRepository.Read() where lastbil.Redigeret > tidsStempel select lastbil).ToList();
      var lastbilList = (from lastbil in _lastbilRepository.Search(x=>x.Redigeret > tidsStempel) select lastbil).ToList();

      var koereToejList = MapLastbil(lastbilList);

      Logger.LogInfo(string.Format("GetKoeretoejer - Servertid: {0}, Bomtid: {1}, Antal køretøj: {2}", 
        DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongDateString(), 
        tidsStempel.ToShortDateString() +" " +tidsStempel.ToLongTimeString(),
        lastbilList.Count().ToString()
        ));
      return koereToejList;
    }

    #endregion *** Public methods ***

    #region *** Private methods ***

    private bool ShouldTakeStikproeve(Guid modtAnlaegId)
    {
      var shouldTakeStikproeve = _modtagerAnlaegBusiness.ShouldTakeStikproeve(modtAnlaegId);
      return shouldTakeStikproeve;
    }

    #region *** Map BomAnmeldelse ***

    private BomAnmeldelse MapToBomAnmeldelse(Anmeldelse anmeldelse)
    {
      var bomAnmeldelse = new BomAnmeldelse();

      // Anmeldelseslbrnr
      bomAnmeldelse.Anmeldelseslbrnr = (int)anmeldelse.Nummer;

      // ModtagerAnlaeg
      if (anmeldelse.ModtagerAnlaeg != null)
        bomAnmeldelse.JordmodtageranlægId = (int)anmeldelse.ModtagerAnlaeg.Nummer;

      // Jordtype
      if (anmeldelse.Jord != null && anmeldelse.Jord.JordKlassifikationType != null)
        bomAnmeldelse.Jordtype = anmeldelse.Jord.JordKlassifikationType.Kode;

      // Oprindelsesadresse
      if (anmeldelse.Oprindelsessted != null)
      {
        bomAnmeldelse.Oprindelsesadresse =
          anmeldelse.Oprindelsessted.Adresse + " " +
          anmeldelse.Oprindelsessted.Postnummer + " " +
          anmeldelse.Oprindelsessted.PostDistrikt;
      }

     
      bomAnmeldelse.BetalerGodkendt = _statusAnmeldelseBusiness.IsBetalerGodkendt(anmeldelse);
      bomAnmeldelse.BetalerSpaerret = _statusAnmeldelseBusiness.IsBetalerSpaerret(anmeldelse);
      bomAnmeldelse.Godkendt = _statusAnmeldelseBusiness.IsAnmeldelseAktiv(anmeldelse);
      bomAnmeldelse.UdtagNaesteLaesTilStikproeve = HasPlanlagtStikproeve(anmeldelse);
      bomAnmeldelse.JordmaengdeOverskredet = IsJordmaengdeOverskredet(anmeldelse);
      bomAnmeldelse.KoerselsperiodeOverskredet = IsKoerselsperiodeOverskredet(anmeldelse);
      bomAnmeldelse.KoerselsperiodeOverskredetSammedag = IsKoerselsperiodeOverskredetIdag(anmeldelse);
      bomAnmeldelse.KoerselStartDato = GetKoerselStartDato(anmeldelse);
      bomAnmeldelse.KoerselSlutDato = GetKoerselSlutDato(anmeldelse);

      /* 30/9-16 efter ønske fra Lars Gravco: firmanavn med ud hvis det findes */
      if (anmeldelse.Betaler != null && anmeldelse.Betaler.Person.Firmaoplysninger != null)
      {
          bomAnmeldelse.BetalerNavn = anmeldelse.Betaler.Person.Firmaoplysninger.Firmanavn;
      }
      else
        // Jf. JF-459
        bomAnmeldelse.BetalerNavn = anmeldelse.Betaler != null ?  String.Format("{0} {1}", anmeldelse.Betaler.Person.Navn,
            anmeldelse.Betaler.Person.Efternavn): "Ikke opgivet";

      return bomAnmeldelse;
    }

    private DateTime GetSenesteStikproeveTid(Anmeldelse anmeldelse)
    {
      var tid = DateTime.MinValue;

      foreach (var planlagteStikproever in anmeldelse.PlanlagteStikproever)
      {
        var senesteTid = DateTime.MinValue;
        if (planlagteStikproever.Stikproeve != null)
        {
          var statusStikproeve = _statusStikproeveBusiness.GetLastStatus(planlagteStikproever.Stikproeve.StatusStikproeve);
          senesteTid = statusStikproeve.Tid;
        }

        if (tid < senesteTid)
          tid = senesteTid;
      }
      return tid;
    }

    private static DateTime? GetSenesteBetalerTid(Anmeldelse anmeldelse)
    {
      DateTime? tid = null;

      if (anmeldelse.Betaler != null && anmeldelse.Betaler.StatusBetaler != null)
      {
        foreach (var statusBetaler in anmeldelse.Betaler.StatusBetaler)
        {
          if (statusBetaler.JordmodtagerId == anmeldelse.ModtagerAnlaeg.JordmodtagerId)
          {
            tid = statusBetaler.Redigeret;
          }
        }
      }
      return tid;
    }

    private static bool HasPlanlagtStikproeve(Anmeldelse anmeldelse)
    {
      var shouldTakeStikproeve = false;
      var plStik = anmeldelse.PlanlagteStikproever;

      if (plStik != null && plStik.Count > 0)
        shouldTakeStikproeve = true;

      return shouldTakeStikproeve;
    }

    private static bool IsJordmaengdeOverskredet(Anmeldelse anmeldelse)
    {
      var isOverskredet = false;
      if (anmeldelse != null && anmeldelse.Jord != null)
      {
        var aflJord = GetAfleveretJordMaengde(anmeldelse);
        var forvJord = anmeldelse.Jord.ForventetJordmaengdeTon;
        if (aflJord != null && forvJord != null)
        {
          if (aflJord > forvJord)
          {
            isOverskredet = true;
          }
        }
      }
      return isOverskredet;
    }

    private static DateTime GetKoerselSlutDato(Anmeldelse anmeldelse)
    {
      var korselsDato = DateTime.MaxValue;
      if (anmeldelse.Jord != null && anmeldelse.Jord.KoerselSlut != null)
      {
        korselsDato = (DateTime)anmeldelse.Jord.KoerselSlut;
      }
      return korselsDato;
    }

    private static DateTime GetKoerselStartDato(Anmeldelse anmeldelse)
    {
      var korselsDato = DateTime.MinValue;
      if (anmeldelse.Jord != null && anmeldelse.Jord.KoerselStart != null)
      {
        korselsDato = (DateTime)anmeldelse.Jord.KoerselStart;
      }
      return korselsDato;
    }

    private static bool IsKoerselsperiodeOverskredet(Anmeldelse anmeldelse)
    {
      var isOverskredet = false;
      if (anmeldelse.Jord != null && anmeldelse.Jord.KoerselSlut != null)
      {
        var endDate = (DateTime)anmeldelse.Jord.KoerselSlut;
        var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        if (today >= endDate)
        {
          isOverskredet = true;
        }
      }
      return isOverskredet;
    }

    private static bool IsKoerselsperiodeOverskredetIdag(Anmeldelse anmeldelse)
    {
      var isOverskredet = false;
      if (anmeldelse.Jord != null && anmeldelse.Jord.KoerselSlut != null)
      {
        var endDate = (DateTime)anmeldelse.Jord.KoerselSlut;
        var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        if (today == endDate)
        {
          isOverskredet = true;
        }
      }
      return isOverskredet;
    }

    #endregion *** Map BomAnmeldelse ***

    #region *** Map Vognlaes ***

    private Vognlaes SaveVognlaes(VognlaesBom vognlaesBom)
    {
      var anmeld = _anmeldelserBusiness.Search(x => x.Nummer == vognlaesBom.AnmeldelsesId).FirstOrDefault();

      if (anmeld == null)
        return null;

      if (anmeld.Nummer == 0)
        return null;

      if (anmeld.Id == Guid.Empty)
        return null;

      var vognlaes = new Vognlaes();
      vognlaes.Dato = vognlaesBom.Tid;
      vognlaes.MaengdeAksler = vognlaesBom.JordmaengdeAksler;
      var lastbil = _lastbilRepository.Search(x => x.Nummer == vognlaesBom.LastbilId).FirstOrDefault();
      if (lastbil != null)
        vognlaes.LastbilId = lastbil.Id;

      _vognlaesBusiness.CreateVognlaes(vognlaes, anmeld.Id, vognlaesBom.Stikproeve, vognlaesBom.StikproeveBaas, false);
      
      return vognlaes;
    }

    private static decimal? GetAfleveretJordMaengde(Anmeldelse an)
    {
      decimal? jordMaengde = new decimal(0.0);
      if (an != null)
        foreach (var vognlaese in an.Vognlaes)
        {
          jordMaengde = jordMaengde + vognlaese.MaengdeTon;
        }
      return jordMaengde;
    }

    #endregion *** Map Vognlaes ***

    #region *** Map KoereToej ***

    private static List<KoereToej> MapLastbil(List<Lastbil> lastbilList)
    {
      var koertojList = new List<KoereToej>();
      if (lastbilList != null)
      {
        foreach (var lastbil in lastbilList)
        {
          var koereToej = new KoereToej();
          koereToej.Fabrikat = lastbil.Fabrikat;
          koereToej.KoeretoejsId = (int)lastbil.Nummer;

          if (lastbil.MiljoeklasseType != null)
            koereToej.Miljoeklasse = lastbil.MiljoeklasseType.Navn;

          koereToej.Nummerplade = lastbil.Nummerplade;

          if (lastbil.Transportoer != null && lastbil.Transportoer.Person != null && lastbil.Transportoer.Person.Firmaoplysninger != null)
            koereToej.FirmaNavn = lastbil.Transportoer.Person.Firmaoplysninger.Firmanavn;

          koertojList.Add(koereToej);
        }
      }
      return koertojList;
    }

    #endregion *** Map KoereToej ***

    #endregion *** Private methods ***

  }
}