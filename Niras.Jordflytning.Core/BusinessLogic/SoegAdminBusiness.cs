using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.SoegeResultat;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Core.BusinessLogic
{


  public enum EnumVognlaesReturnType
  {
    Normal = 0,
    UdtraekTilOkonimiSystem = 1,
    UdtraekTilMaegnder = 2
  }


  //MasterAdmin	søg alt og ingen filtrering		
  //KommuneAdmin	ej søg		
  //JordmodtagerAdmin	ej søg		

  //Sagsbehandler:	kun søg på anmeldelser, og filtrer på kommuneid
  //Bogholder: 	filtrer på tilknyttede jordmodtagerfirmaer 
  //Miljømedarbejder	filtrer på tilknyttede jordmodtagerfirmaer
  //Pladsmand	filtrer på tilknyttede jordmodtagerfirmaer
  //Prøvetager	filtrer på tilknyttede jordmodtagerfirmaer

  //Laboratorie:	kun stikprøver og kun tilknyttede jordmodtagerfirmaer 

  public class SoegAdminBusiness : ISoegAdminBusiness
  {
    private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.SoegAdminBusiness");
    private readonly IStatusBetalerRepository _statusBetalerRepository;
    private readonly IVognlaesRepository _vognlaesRepository;
    private readonly IPersonJordmodtagerRepository _personJordmodtagerRepository;
    private readonly IAnmeldelserBusiness _anmeldelserBusiness;
    private readonly IStatusStikproeveBusiness _statusStikproeveBusiness;

    public SoegAdminBusiness(
      IStatusBetalerRepository statusBetalerRepository,
      IVognlaesRepository vognlaesRepository,
      IPersonJordmodtagerRepository personJordmodtagerRepository,
      IAnmeldelserBusiness anmeldelserBusiness,
      IStatusStikproeveBusiness statusStikproeveBusiness)
    {
      _statusBetalerRepository = statusBetalerRepository;
      _vognlaesRepository = vognlaesRepository;
      _personJordmodtagerRepository = personJordmodtagerRepository;
      _anmeldelserBusiness = anmeldelserBusiness;
      _statusStikproeveBusiness = statusStikproeveBusiness;
    }

    #region *** Public methods ***

    public List<SoegeResultatBetaler> GetBetalereBy(string firmaNavn, string navn, int isGodkendt, int kerneKunde, Guid brugerId)
    {
      var startTime = DateTime.Now;

      //TODO - KVE Performance, her laves filtreringen på serveren og alle betalerne hentes fra databasen.
      var allresult = _statusBetalerRepository.Read();

      var usersJordmodtagerId = GetUsersJordmodtagerId(brugerId);
      allresult = allresult.Where(x => usersJordmodtagerId.Contains(x.JordmodtagerId));

      if (!String.IsNullOrEmpty(firmaNavn))
      {
        allresult = allresult.Where(x =>
                                    x.Betaler.Person.Firmaoplysninger != null &&
                                    (x.Betaler.Person != null &&
                                     (x.Betaler != null && x.Betaler.Person.Firmaoplysninger.Firmanavn.ToUpperInvariant().Contains(firmaNavn.ToUpperInvariant()))));
      }

      if (!String.IsNullOrEmpty(navn))
      {
        allresult = allresult.Where(x => (x.Betaler.Person.Navn + " " + x.Betaler.Person.Efternavn).ToUpperInvariant().Contains(navn.ToUpperInvariant()));
      }

      if (kerneKunde != 0)
      {
        bool? kKunde = null;

        switch (kerneKunde)
        {
          case 1:
            kKunde = true;
            break;
          case 2:
            kKunde = false;
            break;
        }
        allresult = allresult.Where(x => x.KerneKunde == kKunde);
      }

      if (isGodkendt != 0)
      {
        bool? gk = null;

        switch (isGodkendt)
        {
          case 1:
            gk = true;
            break;
          case 2:
            gk = false;
            break;
        }
        allresult = allresult.Where(x => x.Godkendt == gk);
      }
      List<StatusBetaler> betalereList;

      try
      {
        betalereList = allresult.ToList();
      }
      catch (Exception exception)
      {
        Logger.LogException("Error in method: GetBetalereBy", exception);
        betalereList = new List<StatusBetaler>();
      }

      var soegeResultatList = MapToSoegeResultatList(betalereList);

      var timeUsed = DateTime.Now - startTime;
      Logger.LogInfo("GetBetalereBy call time: " + timeUsed.TotalSeconds + " seconds.");
      return soegeResultatList;
    }

    public List<SoegeResultatStikproeve> GetStikproeverBy(DateTime? paramFoerDato, DateTime? paramEfterDato, decimal? paramStikproeveNummer,
                                                          Guid? paramStikproeveStatusGuid, Guid personId)
    {
      var startTime = DateTime.Now;
      var stikList = new List<Stikproeve>();

      var allresult = _anmeldelserBusiness.GetUsersAnmeldelserIncludingVognlaesStikproever(personId);

      var hasNummerSearch = !(paramStikproeveNummer == null || paramStikproeveNummer == 0);
      var hasStatusSearch = !(paramStikproeveStatusGuid == Guid.Empty || paramStikproeveStatusGuid == null);
      var hasDateSearch = !(paramFoerDato == null && paramEfterDato == null);
      var stikListAll = new List<Stikproeve>();

      foreach (var anmeldelse in allresult)
      {
        foreach (var vognlaes in anmeldelse.Vognlaes)
        {
          foreach (var stikproeve in vognlaes.Stikproeve)
          {
            stikListAll.Add(stikproeve);
          }
        }
      }

      foreach (var anmeldelse in allresult)
      {
        foreach (var plStik in anmeldelse.PlanlagteStikproever)
        {
          if (plStik.Stikproeve != null)
            stikListAll.Add(plStik.Stikproeve);
        }
      }

      foreach (var stikproeve in stikListAll)
      {
        var addToResultList = false;

        if ((paramStikproeveNummer == null || paramStikproeveNummer == 0) && paramFoerDato == null &&
            paramEfterDato == null && paramStikproeveStatusGuid == Guid.Empty)
        {
          addToResultList = true;
        }
        else
        {
          var hasSameStatus = false;
          var hasSameNummer = false;
          var isInDateRange = false;

          // hent seneste status på stikprøven
          var lastStikproeveStatus = _statusStikproeveBusiness.GetLastStatus(stikproeve.StatusStikproeve);

          // Status parameter
          if (hasStatusSearch)
          {
            if (lastStikproeveStatus.StatusStikproeveTypeId == paramStikproeveStatusGuid)
              hasSameStatus = true;
          }

          // Dato parametre
          if (hasDateSearch)
          {
            // hent seneste status på stikprøven
            var oprettetStikproeveStatus = _statusStikproeveBusiness.GetFirstStatus(stikproeve.StatusStikproeve);
            if (oprettetStikproeveStatus != null)
              isInDateRange = IsInDateTimeRange(paramFoerDato, paramEfterDato, oprettetStikproeveStatus.Tid);
          }

          // Stikprøvenummer parameter 
          if (hasNummerSearch)
          {
            if (stikproeve.Nummer.ToString(CultureInfo.InvariantCulture).Contains(paramStikproeveNummer.ToString()))
              hasSameNummer = true;
          }

          // regn ud om stikprøven skal taget med i søgeresultatet
          if (hasStatusSearch && !hasNummerSearch && !hasDateSearch)
          {
            if (hasSameStatus)
              addToResultList = true;
          }
          else if (!hasStatusSearch && hasNummerSearch && !hasDateSearch)
          {
            if (hasSameNummer)
              addToResultList = true;
          }
          else if (!hasStatusSearch && !hasNummerSearch && hasDateSearch)
          {
            if (isInDateRange)
              addToResultList = true;
          }

          else if (hasStatusSearch && hasNummerSearch && !hasDateSearch)
          {
            if (hasSameStatus && hasSameNummer)
              addToResultList = true;
          }
          else if (hasStatusSearch && !hasNummerSearch)
          {
            if (hasSameStatus && isInDateRange)
              addToResultList = true;
          }
          else if (!hasStatusSearch && hasNummerSearch)
          {
            if (hasSameNummer && isInDateRange)
              addToResultList = true;
          }

          else if (hasStatusSearch)
          {
            if (hasSameStatus && hasSameNummer && isInDateRange)
              addToResultList = true;
          }
        }

        if (addToResultList)
        {
          stikList.Add(stikproeve);
        }
      }


      List<Stikproeve> stikproeveList;
      try
      {
        stikproeveList = stikList.ToList();
      }
      catch (Exception exception)
      {
        Logger.LogException("Error in method: GetStikproeverBy", exception);
        stikproeveList = new List<Stikproeve>();
      }

      var soegeResultatList = MapToSoegeResultatStikproeveList(stikproeveList);

      var timeUsed = DateTime.Now - startTime;
      Logger.LogInfo("GetStikproeverBy call time: " + timeUsed.TotalSeconds + " seconds.");
      return soegeResultatList;
    }

    /// <summary>
    /// Eksport af vognlæs til økonomisystem skal ikke tage de vognlæs med, 
    /// som er udtaget til stikprøve. 
    /// Vognlæs som har været udtaget til stikprøve, 
    /// skal anvende datoen for "bås tømt", 
    /// når vognlæsene udtrækkes til økonomisystmet. 
    /// </summary>
    public List<SoegeResultatVognlaes1> GetVognlaesBy(DateTime? vognlaesFoerDato, DateTime? vognlaesEfterDato, EnumVognlaesReturnType vognlaesReturnType, Guid personId)
    {
      var startTime = DateTime.Now;
      var anmeldList = _anmeldelserBusiness.GetUsersAnmeldelserIncludingVognlaesStikproever(personId).ToList();
      var vognList = new List<Vognlaes>();

      switch (vognlaesReturnType)
      {
        case EnumVognlaesReturnType.Normal:
          FindVognlaes(vognlaesFoerDato, vognlaesEfterDato, anmeldList, vognList);
          break;

        case EnumVognlaesReturnType.UdtraekTilOkonimiSystem:
          FindVognlaesTilOkonomiSystem(vognlaesFoerDato, vognlaesEfterDato, anmeldList, vognList);
          break;
      }

      List<Vognlaes> vognlaesList;
      try
      {
        vognlaesList = vognList.ToList();
      }
      catch (Exception exception)
      {
        Logger.LogException("Error in method: GetVognlaesBy", exception);
        vognlaesList = new List<Vognlaes>();
      }

      var soegeResultatList = MapToSoegeResultatVognlaes1List(vognlaesList);

      var timeUsed = DateTime.Now - startTime;
      Logger.LogInfo("GetVognlaesBy call time: " + timeUsed.TotalSeconds + " seconds.");
      return soegeResultatList;
    }

    public List<SoegeResultatVognlaes2> GetVognlaesBy(DateTime? vognlaesFoerDato, DateTime? vognlaesEfterDato, Guid personId)
    {
      var startTime = DateTime.Now;
      var anmeldList = _anmeldelserBusiness.GetUsersAnmeldelserIncludingVognlaesStikproever(personId).ToList();
      var vognList = new List<Vognlaes>();

      FindVognlaes(vognlaesFoerDato, vognlaesEfterDato, anmeldList, vognList);

      List<Vognlaes> vognlaesList;
      try
      {
        vognlaesList = vognList.ToList();
      }
      catch (Exception exception)
      {
        Logger.LogException("Error in method: GetVognlaesBy", exception);
        vognlaesList = new List<Vognlaes>();
      }

      var soegeResultatList = MapToSoegeResultatVognlaes2List(vognlaesList);

      var timeUsed = DateTime.Now - startTime;
      Logger.LogInfo("GetVognlaesBy call time: " + timeUsed.TotalSeconds + " seconds.");
      return soegeResultatList;
    }

    public List<SoegeResultatStikproeve> GetStikproeverDerErAktive(Guid personId)
    {
      var startTime = DateTime.Now;

      var anmeldList = _anmeldelserBusiness.GetUsersAnmeldelserIncludingVognlaesStikproever(personId).ToList();
      var stikList = new List<Stikproeve>();

      foreach (var anmeldelse in anmeldList)
      {
        foreach (var vognlaes in anmeldelse.Vognlaes)
        {
          foreach (var stikproeve in vognlaes.Stikproeve)
          {
            stikList.Add(stikproeve);
          }
        }
      }
      var allresult = from stikproeve in stikList
                      where !(stikproeve.StatusStikproeve.Any(c => c.StatusStikproeveType.Kode == (decimal)EnumStatusStikproeve.BaasToemt))
                        //&& (stikproeve.StatusStikproeve.Any(c => c.StatusStikproeveType.Kode == (decimal)EnumStatusStikproeve.Planlagt))
                      && stikproeve.StatusStikproeve.Count > 1
                      select stikproeve;

      List<Stikproeve> stikproeveList;
      try
      {
        stikproeveList = allresult.ToList();
      }
      catch (Exception exception)
      {
        Logger.LogException("Error in method: GetStikproeverDerErAktive", exception);
        stikproeveList = new List<Stikproeve>();
      }

      var soegeResultatList = MapToSoegeResultatStikproeveList(stikproeveList);

      var timeUsed = DateTime.Now - startTime;
      Logger.LogInfo("GetStikproeverDerErAktive call time: " + timeUsed.TotalSeconds + " seconds.");
      return soegeResultatList;
    }

    public List<SoegeResultatFakturaExport> GetVognlaesFaktura(List<Guid> vognlaesGuidList)
    {
      //var vognLaesList = from vl in _vognlaesRepository.Read()
      //                   where vognlaesGuidList.Contains(vl.Id)
      //                   select vl;
      var vognLaesList = from vl in _vognlaesRepository.Search(x => vognlaesGuidList.Contains(x.Id))
                         select vl;

      var returnList = MapSoegeResultatFakturaExport(vognLaesList);

      return returnList;
    }

    #endregion *** Public methods ***

    #region *** Private methods ***

    private static void FindVognlaesTilOkonomiSystem(DateTime? vognlaesFoerDato, DateTime? vognlaesEfterDato, List<Anmeldelse> anmeldList, List<Vognlaes> vognList)
    {
      foreach (var anmeldelse in anmeldList)
      {
        foreach (var vognlaes in anmeldelse.Vognlaes)
        {
          var vognlaesDato = RemoveTimeFromDateTime(vognlaes.Dato);
          var ignoreVognlaes = false;

          // har vognlæsseet stikprøve
          var stprv = (from st in vognlaes.Stikproeve select st).FirstOrDefault();

          if (stprv != null)
          {
            // find bås tømt dato
            var stikStatus = (from stat in stprv.StatusStikproeve
                              where stat.StatusStikproeveType.Kode == (short)EnumStatusStikproeve.BaasToemt
                              select stat).FirstOrDefault();

            // Hvis båsen er tømt, skal vognlæsset med; men med statusdatoen, som søgedato
            if (stikStatus != null)
              vognlaesDato = stikStatus.Tid;
            else
              ignoreVognlaes = true;
          }

          if (ignoreVognlaes)
            continue;

          var addToResultList = IsInDateTimeRange(vognlaesFoerDato, vognlaesEfterDato, vognlaesDato);

          if (addToResultList)
            vognList.Add(vognlaes);
        }
      }
    }

    private static void FindVognlaes(DateTime? vognlaesFoerDato, DateTime? vognlaesEfterDato, List<Anmeldelse> anmeldList, List<Vognlaes> vognList)
    {
      foreach (var anmeldelse in anmeldList)
      {
        foreach (var vognlaes in anmeldelse.Vognlaes)
        {
          var addToResultList = IsInDateTimeRange(vognlaesFoerDato, vognlaesEfterDato, vognlaes.Dato);
          if (addToResultList)
            vognList.Add(vognlaes);
        }
      }
    }

    private static DateTime? RemoveTimeFromDateTime(DateTime? dateTime)
    {
      DateTime? date = null;
      if (dateTime != null)
      {
        date = dateTime.Value.Date;
      }
      return date;
    }

    private List<Guid?> GetUsersJordmodtagerId(Guid brugerId)
    {
      //var persJordList = _personJordmodtagerRepository.Read().Where(x => x.PersonId == brugerId);
      var persJordList = _personJordmodtagerRepository.Search(x => x.PersonId == brugerId);

      var resList = new List<Guid?>();
      foreach (var personJordmodtager in persJordList.ToList())
      {
        var id = new Guid?(personJordmodtager.JordmodtagerId);
        resList.Add(id);

      }
      return resList;
    }

    private static List<SoegeResultatBetaler> MapToSoegeResultatList(List<StatusBetaler> betalerList)
    {
      var size = 0;
      if (betalerList != null)
      {
        size = betalerList.Count;
      }
      var soegeResultatList = new List<SoegeResultatBetaler>(size);

      if (betalerList != null)
        foreach (var betaler in betalerList)
        {
          var soegeResultat = new SoegeResultatBetaler();
          soegeResultat.BetalerId = betaler.Id;

          if (betaler.Betaler != null && betaler.Betaler.Person != null)
            soegeResultat.PersonId = betaler.Betaler.Person.Id;

          soegeResultat.JordmodtagerId = betaler.JordmodtagerId;
          soegeResultat.Dato = betaler.Redigeret.ToShortDateString();
          soegeResultat.Bemaerkning = betaler.Bemaerkning;

          switch (betaler.KerneKunde)
          {
            case null:
              soegeResultat.KerneKunde = "-";
              break;
            case true:
              soegeResultat.KerneKunde = "Ja";
              break;
            case false:
              soegeResultat.KerneKunde = "Nej";
              break;
          }

          switch (betaler.Godkendt)
          {
            case null:
              soegeResultat.Status = "Anmoder om godkendelse";
              break;
            case true:
              soegeResultat.Status = "Godkendt";
              break;
            case false:
              soegeResultat.Status = "Afvist";
              break;
          }

          if (betaler.Betaler != null && betaler.Betaler.Person != null)
          {
            soegeResultat.BetalerNavn = betaler.Betaler.Person.Navn + " " + betaler.Betaler.Person.Efternavn;
            if (betaler.Betaler.Person.Firmaoplysninger != null)
            {
              soegeResultat.FirmaNavn = betaler.Betaler.Person.Firmaoplysninger.Firmanavn;
            }
          }
          soegeResultatList.Add(soegeResultat);
        }

      // sort by dato
      soegeResultatList = (soegeResultatList.OrderByDescending(res => res.Dato)).ToList();

      return soegeResultatList;
    }

    private static List<SoegeResultatStikproeve> MapToSoegeResultatStikproeveList(List<Stikproeve> stikProeveList)
    {
      var size = 0;
      if (stikProeveList != null)
        size = stikProeveList.Count;

      var soegeResultatList = new List<SoegeResultatStikproeve>(size);

      if (stikProeveList != null)
        foreach (var stikproeve in stikProeveList)
        {
          var soegeResultat = new SoegeResultatStikproeve();
          soegeResultat.StikproeveId = stikproeve.Id;

          soegeResultat.LoebeNummer = stikproeve.Nummer;

          if (
            stikproeve.Vognlaes != null &&
            stikproeve.Vognlaes.Lastbil != null &&
            stikproeve.Vognlaes.Lastbil.Transportoer != null &&
            stikproeve.Vognlaes.Lastbil.Transportoer.Person != null &&
            stikproeve.Vognlaes.Lastbil.Transportoer.Person.Firmaoplysninger != null)
          {
            soegeResultat.TransportoerNavn = stikproeve.Vognlaes.Lastbil.Transportoer.Person.Firmaoplysninger.Firmanavn;
          }

          if (stikproeve.Person != null)
            soegeResultat.AnalyseretAf = stikproeve.Person.Navn + " " + stikproeve.Person.Efternavn;

          if (stikproeve.Baas != null)
            soegeResultat.BaasNr = stikproeve.Baas;

          if (stikproeve.Vognlaes != null && stikproeve.Vognlaes.Anmeldelse != null && stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg != null)
            soegeResultat.ModtageAnlaegNavn = stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Navn;

          if (stikproeve.Person != null)
            soegeResultat.ProeveTagerNavn = stikproeve.Person.Navn + " " + stikproeve.Person.Efternavn;

          if (stikproeve.Vognlaes != null && stikproeve.Vognlaes.Anmeldelse != null &&
              stikproeve.Vognlaes.Anmeldelse.Oprindelsessted != null)
          {
            var postnummer = "";
            if (stikproeve.Vognlaes.Anmeldelse.Oprindelsessted.Postnummer.HasValue)
            {
              postnummer = ", " + stikproeve.Vognlaes.Anmeldelse.Oprindelsessted.Postnummer.Value + " " +
                           stikproeve.Vognlaes.Anmeldelse.Oprindelsessted.PostDistrikt;
            }

            soegeResultat.Adresse = stikproeve.Vognlaes.Anmeldelse.Oprindelsessted.Adresse + postnummer;
            soegeResultat.AnmeldelseLoebenr = stikproeve.Vognlaes.Anmeldelse.Nummer;

          }
          else if (
            stikproeve.PlanlagteStikproever != null &&
            stikproeve.PlanlagteStikproever.Any() &&
            stikproeve.PlanlagteStikproever.First().Anmeldelse != null &&
            stikproeve.PlanlagteStikproever.First().Anmeldelse.Oprindelsessted != null
            )
          {
            var postnummer = "";
            if (stikproeve.PlanlagteStikproever.First().Anmeldelse.Oprindelsessted.Postnummer.HasValue)
            {
              postnummer = ", " + stikproeve.PlanlagteStikproever.First().Anmeldelse.Oprindelsessted.Postnummer.Value + " " +
                           stikproeve.PlanlagteStikproever.First().Anmeldelse.Oprindelsessted.PostDistrikt;
            }

            soegeResultat.Adresse = stikproeve.PlanlagteStikproever.First().Anmeldelse.Oprindelsessted.Adresse + postnummer;
            soegeResultat.AnmeldelseLoebenr = stikproeve.PlanlagteStikproever.First().Anmeldelse.Nummer;
          }



          if (stikproeve.StatusStikproeve != null)
          {
            var status = stikproeve.StatusStikproeve.OrderByDescending(a => a.Tid).FirstOrDefault();
            if (status != null && status.StatusStikproeveType != null)
            {
              soegeResultat.Status = status.StatusStikproeveType.Navn;
              soegeResultat.SenesteStatusDato = status.Tid;
            }
            else
            {
              soegeResultat.Status = "Fejl: Ingen status!";
            }
          }

          soegeResultatList.Add(soegeResultat);
        }

      // sort by lbnr desc
      soegeResultatList = (soegeResultatList.OrderByDescending(res => res.LoebeNummer)).ToList();

      return soegeResultatList;
    }

    private List<SoegeResultatVognlaes1> MapToSoegeResultatVognlaes1List(List<Vognlaes> vognlaesList)
    {
      var size = 0;
      if (vognlaesList != null)
        size = vognlaesList.Count;

      var soegeResultatList = new List<SoegeResultatVognlaes1>(size);

      if (vognlaesList != null)
      {
        var count = 0;
        foreach (var vognlaes in vognlaesList)
        {
          var soegeResultat = new SoegeResultatVognlaes1();

          soegeResultat.VognlaesId = vognlaes.Id;
          soegeResultat.AnmeldelseId = vognlaes.AnmeldelseId;
          soegeResultat.Adresse = vognlaes.Anmeldelse.Oprindelsessted.Adresse;


          soegeResultat.BetalerNavn = vognlaes.Anmeldelse.Betaler.Person.Navn + " " + vognlaes.Anmeldelse.Betaler.Person.Efternavn;
          soegeResultat.Dato = vognlaes.Dato.ToShortDateString();
          soegeResultat.VognlaesDato = vognlaes.Dato;

          if (vognlaes.MaengdeAksler != null)
            soegeResultat.JordmaengdeAksler = String.Format("{0:0}", vognlaes.MaengdeAksler);

          if (vognlaes.MaengdeTon != null)
            soegeResultat.JordmaengdeTons = (String.Format("{0:0.##}", vognlaes.MaengdeTon)).Replace(".", ",");

          soegeResultat.ModtageAnlaegNavn = vognlaes.Anmeldelse.ModtagerAnlaeg.Navn;

          var firstOrDefault = vognlaes.Stikproeve.FirstOrDefault();
          if (firstOrDefault != null)
          {
            var stat = _statusStikproeveBusiness.GetLastStatus(firstOrDefault.StatusStikproeve);
            if (stat != null)
            {
              soegeResultat.Status = stat.StatusStikproeveType.Navn;
            }
          }
          if (String.IsNullOrEmpty(soegeResultat.Status))
            soegeResultat.Status = " - ";

          if (vognlaes.Lastbil != null &&
              vognlaes.Lastbil.Transportoer != null &&
              vognlaes.Lastbil.Transportoer.Person != null &&
              vognlaes.Lastbil.Transportoer.Person.Firmaoplysninger != null)
          {
            soegeResultat.TransportoerNavn =
              vognlaes.Lastbil.Transportoer.Person.Firmaoplysninger.Firmanavn + " (" + vognlaes.Lastbil.Nummerplade + ")";
          }
          else
          {
            soegeResultat.TransportoerNavn = "(Fejl i data: Intet firma!)";
          }

          soegeResultat.Afvist = vognlaes.Afvist;
          soegeResultat.AfvistNote = vognlaes.AfvistNote;

          soegeResultatList.Add(soegeResultat);
        }
      }
      // sort by dato
      soegeResultatList = (soegeResultatList.OrderByDescending(res => res.VognlaesDato)).ToList();

      return soegeResultatList;
    }

    private static List<SoegeResultatVognlaes2> MapToSoegeResultatVognlaes2List(List<Vognlaes> vognlaesList)
    {
      var size = 0;
      if (vognlaesList != null)
        size = vognlaesList.Count;

      var soegeResultatList = new List<SoegeResultatVognlaes2>(size);

      if (vognlaesList != null)
        foreach (var vognlaes in vognlaesList)
        {
          var soegeResultat = new SoegeResultatVognlaes2();

          soegeResultat.VognlaesId = vognlaes.Id;
          soegeResultat.Modtagedato = vognlaes.Dato.ToShortDateString();

          if (vognlaes.MaengdeAksler != null)
            soegeResultat.JordmaengdeAksler = (int)vognlaes.MaengdeAksler;

          if (vognlaes.MaengdeTon != null)
            soegeResultat.JordmaengdeTons = (double)vognlaes.MaengdeTon;

          soegeResultat.AnmeldelseId = vognlaes.Anmeldelse.Id;

          if (vognlaes.Anmeldelse.Oprindelsessted != null)
          {
            soegeResultat.Opgravningssted = vognlaes.Anmeldelse.Oprindelsessted.Adresse;

            if (vognlaes.Anmeldelse.Oprindelsessted.AndenOprindJordType != null)
              soegeResultat.Jordtype = vognlaes.Anmeldelse.Oprindelsessted.AndenOprindJordType.Navn;
          }

          if (vognlaes.Anmeldelse.Jord != null)
            if (vognlaes.Anmeldelse.Jord.JordKlassifikationType != null)
              soegeResultat.Forureningskategori = vognlaes.Anmeldelse.Jord.JordKlassifikationType.Navn;

          soegeResultat.Loebenummer = vognlaes.Anmeldelse.Nummer.ToString(CultureInfo.InvariantCulture);
          if (vognlaes.Anmeldelse.ModtagerAnlaeg != null)
            soegeResultat.Modtageanlaeg = vognlaes.Anmeldelse.ModtagerAnlaeg.Navn;

          soegeResultat.Oprindelseskommune = vognlaes.Anmeldelse.Kommune.Navn;

          soegeResultat.Afvist = vognlaes.Afvist;
          soegeResultat.AfvistNote = vognlaes.AfvistNote;

          soegeResultatList.Add(soegeResultat);
        }

      // sort by dato
      soegeResultatList = (soegeResultatList.OrderByDescending(res => res.Modtagedato)).ToList();

      return soegeResultatList;
    }

    private static List<SoegeResultatFakturaExport> MapSoegeResultatFakturaExport(IEnumerable<Vognlaes> vognLaesList)
    {
      var returnList = new List<SoegeResultatFakturaExport>();
      foreach (var vognlaes in vognLaesList)
      {
        var sr = new SoegeResultatFakturaExport();

        // Vognlæs
        sr.VognlaesId = vognlaes.Id;
        sr.Dato = vognlaes.Dato.ToShortDateString();
        sr.OrderByDato = vognlaes.Dato;

        sr.LoebeNummer = vognlaes.Anmeldelse.Nummer.ToString();

        if (vognlaes.MaengdeAksler != null)
          sr.MaengdeAksler = (int)vognlaes.MaengdeAksler;

        if (vognlaes.MaengdeTon != null)
          sr.MaengdeTons = vognlaes.MaengdeTon.ToString();

        if (vognlaes.Lastbil != null)
          sr.Lastbil = vognlaes.Lastbil.Nummerplade;

        // Oprindelsessted postdistrikt+nummer
        if (vognlaes.Anmeldelse.Oprindelsessted != null && vognlaes.Anmeldelse.Oprindelsessted.Postnummer != null)
          sr.Postnr = vognlaes.Anmeldelse.Oprindelsessted.Postnummer + " " + vognlaes.Anmeldelse.Oprindelsessted.PostDistrikt;

        // Adresse
        if (vognlaes.Anmeldelse.Oprindelsessted != null)
          sr.Adresse = vognlaes.Anmeldelse.Oprindelsessted.Adresse;

        // Betaler
        if (vognlaes.Anmeldelse.Betaler != null)
        {
          sr.BetalerNavn = vognlaes.Anmeldelse.Betaler.Person.Navn + " " + vognlaes.Anmeldelse.Betaler.Person.Efternavn;
          sr.BetalerId = vognlaes.Anmeldelse.Betaler.Person.Id;
          if (vognlaes.Anmeldelse.Betaler.Person.Firmaoplysninger != null)
          {
            sr.BetalerFirma = vognlaes.Anmeldelse.Betaler.Person.Firmaoplysninger.Firmanavn;
            sr.Cvr = vognlaes.Anmeldelse.Betaler.Person.Firmaoplysninger.CVR.ToString(CultureInfo.InvariantCulture);
            sr.Pnummer = vognlaes.Anmeldelse.Betaler.Person.Firmaoplysninger.PNummer;
          }
        }

        // Anmelder
        if (vognlaes.Anmeldelse.Anmelder != null)
        {
          sr.AnmelderNavn = vognlaes.Anmeldelse.Anmelder.Person.Navn + " " + vognlaes.Anmeldelse.Anmelder.Person.Efternavn;
          sr.AnmelderId = vognlaes.Anmeldelse.Anmelder.Person.Id;
          if (vognlaes.Anmeldelse.Anmelder.Person.Firmaoplysninger != null)
            sr.AnmelderFirma = vognlaes.Anmeldelse.Anmelder.Person.Firmaoplysninger.Firmanavn;
        }

        // Modtageanlæg
        if (vognlaes.Anmeldelse.ModtagerAnlaeg != null)
        {
          sr.ModtagerAnlaegId = vognlaes.Anmeldelse.ModtagerAnlaeg.Id;
          sr.ModtagerAnlaegNavn = vognlaes.Anmeldelse.ModtagerAnlaeg.Navn;
          sr.ModtagerAnlaegNummer = vognlaes.Anmeldelse.ModtagerAnlaeg.Nummer.ToString(CultureInfo.InvariantCulture);
        }

        // Transportør
        if (vognlaes.Lastbil != null && vognlaes.Lastbil.Transportoer != null &&
          vognlaes.Lastbil.Transportoer.Person != null && vognlaes.Lastbil.Transportoer.Person.Firmaoplysninger != null)
        {
          sr.TransportoerNavn = vognlaes.Lastbil.Transportoer.Person.Navn + " " + vognlaes.Lastbil.Transportoer.Person.Efternavn;
          sr.TransportoerFirma = vognlaes.Lastbil.Transportoer.Person.Firmaoplysninger.Firmanavn;
        }


        sr.BeskedTilJordmodtager = vognlaes.Anmeldelse.BemaerkningTilJordmodtager;
        sr.AnmeldersSagsnummer = vognlaes.Anmeldelse.AnmelderSagsnummer;

        if (vognlaes.Anmeldelse.Betaleringsoplysning != null && vognlaes.Anmeldelse.Betaleringsoplysning.Count > 0)
        {
          sr.EanNummer = vognlaes.Anmeldelse.Betaleringsoplysning.FirstOrDefault().EAN;
          sr.FakturaEmails = vognlaes.Anmeldelse.Betaleringsoplysning.FirstOrDefault().SendesTilEmail;
        }

        if (string.IsNullOrEmpty(sr.EanNummer))
        {
          //EanNummer er også registreret på tabellen Betalingsoplysninger, men der efter ½ års drift ikke indsat nogle rækker i tabellen...
          //Så derfor laver jeg denne her løsning.
          if (vognlaes.Anmeldelse.Betaler != null && vognlaes.Anmeldelse.Betaler.Person.Firmaoplysninger != null && vognlaes.Anmeldelse.Betaler.Person.Firmaoplysninger.EAN.HasValue)
          {
            sr.EanNummer = vognlaes.Anmeldelse.Betaler.Person.Firmaoplysninger.EAN.Value.ToString();
          }

        }

        returnList.Add(sr);
      }
      // sort by dato
      returnList = (returnList.OrderByDescending(res => res.OrderByDato)).ToList();
      return returnList;
    }

    /// <summary>
    /// Checker om en dato er indenfor en given periode
    /// </summary>
    private static bool IsInDateTimeRange(DateTime? foerDato, DateTime? efterDato, DateTime? dateToCheck)
    {
      // fjern klokkeslet
      var parameterFoer = RemoveTimeFromDateTime(foerDato);
      var parameterEfter = RemoveTimeFromDateTime(efterDato);

      var addToResultList = false;
      // hvis intet er opgivet skal alt med
      if (foerDato == null && efterDato == null)
        addToResultList = true;
      else
      {
        var theDateToCheck = RemoveTimeFromDateTime(dateToCheck);
        if (foerDato != null && efterDato != null)
        {
          if (theDateToCheck <= parameterFoer && theDateToCheck >= parameterEfter)
            addToResultList = true;
        }
        else
        {
          if (foerDato != null)
          {
            if (theDateToCheck <= parameterFoer)
              addToResultList = true;
          }
          if (efterDato != null)
          {
            if (theDateToCheck >= parameterEfter)
              addToResultList = true;
          }
        }
      }
      return addToResultList;
    }


    #endregion *** Private methods ***
  }
}
