using System;
using System.Collections.Generic;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.SoegeResultat;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Core.BusinessLogic
{
  public class SoegBusinessStikproeve : ISoegBusinessStikproeve
  {
    private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.SoegBusinessStikproeve");
    private readonly IAnmeldelserBusiness _anmeldelserBusiness;
    private readonly IStatusStikproeveBusiness _statusStikproeveBusiness;
    private readonly IStikproeveBusiness _stikproeveBusiness;

    public SoegBusinessStikproeve(IAnmeldelserBusiness anmeldelserBusiness, IStatusStikproeveBusiness statusStikproeveBusiness, IStikproeveBusiness stikproeveBusiness)
    {
      _anmeldelserBusiness = anmeldelserBusiness;
      _statusStikproeveBusiness = statusStikproeveBusiness;
      _stikproeveBusiness = stikproeveBusiness;
    }

    #region *** Public methods ***

    public List<SoegeResultatStikproeve> GetAktuelleStikproeveListForLab(Guid labPersonId)
    {
      var startTime = DateTime.Now;
      var soegeResultatList = new List<SoegeResultatStikproeve>();

      //Henter stikprøver tildelt en lab, hvor analysen ikke er udført og prøven er udtaget.
      var stikproeverForLab = _stikproeveBusiness.GetStikproeverForLabPerson(labPersonId);

      soegeResultatList.AddRange(MapToSoegeResultatList(stikproeverForLab.ToList()));

      var timeUsed = DateTime.Now - startTime;
      Logger.LogInfo("GetAktuelleStikproeveListForLab call time: " + timeUsed.TotalSeconds + " seconds.");
      return soegeResultatList;

    }

    public List<SoegeResultatStikproeve> GetAktuelleStikproeveList(Guid userId)
    {
      var startTime = DateTime.Now;

      var soegeResultatList = new List<SoegeResultatStikproeve>();
      //var anmeldelseList_old = _anmeldelserBusiness.GetUsersAnmeldelserIncludingVognlaesStikproever(userId).ToList();
      var anmeldelseList = _anmeldelserBusiness.GetUsersAktuelleStikproever(userId).ToList(); //Her laver vi en filtrering i databasen, hvor det kun er de anmeldelser der har en stikprøve eller planlagt stikprøve som hentes.
     
      Logger.LogInfo("GetAktuelleStikProever split time: " + (DateTime.Now - startTime).TotalSeconds + " seconds.");

      var stikList = new List<Stikproeve>();

      foreach (var anmeldelse in anmeldelseList)
      {
        foreach (var vognlaes in anmeldelse.Vognlaes)
        {
          foreach (var stikproeve in vognlaes.Stikproeve)
          {
            var addToResultList = false;
            if (stikproeve == null)
              continue;

            if (stikproeve.StatusStikproeve == null || stikproeve.StatusStikproeve.Count < 1)
            {
              addToResultList = true;
            }
            else
            {
              var firstOrDefault = stikproeve.StatusStikproeve.OrderByDescending(x => x.Tid).FirstOrDefault();
              if (firstOrDefault != null && firstOrDefault.StatusStikproeveType.Kode != (short)EnumStatusStikproeve.BaasToemt)
                addToResultList = true;
            }

            if (addToResultList)
            {
              stikList.Add(stikproeve);
            }
          }
        }
      }
      
      Logger.LogInfo("GetAktuelleStikProever split time: " + (DateTime.Now - startTime).TotalSeconds + " seconds.");
      
      soegeResultatList.AddRange(MapToSoegeResultatList(stikList.ToList()));

      var timeUsed = DateTime.Now - startTime;
      Logger.LogInfo("GetAktuelleStikProever call time: " + timeUsed.TotalSeconds + " seconds.");
      return soegeResultatList;
    }

    #endregion *** Public methods ***


    #region *** Private methods ***

    private List<SoegeResultatStikproeve> MapToSoegeResultatList(List<Stikproeve> stikProeveList)
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

          if (stikproeve.Vognlaes != null &&
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

          if (stikproeve.Vognlaes != null && stikproeve.Vognlaes.Anmeldelse != null && 
						stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg != null)
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
          else if (stikproeve.PlanlagteStikproever != null && 
						stikproeve.PlanlagteStikproever.Any() &&
            stikproeve.PlanlagteStikproever.First().Anmeldelse != null && 
						stikproeve.PlanlagteStikproever.First().Anmeldelse.Oprindelsessted != null
            )
          {
            var postnummer = "";
            if (stikproeve.PlanlagteStikproever.First().Anmeldelse.Oprindelsessted.Postnummer.HasValue)
            {
              postnummer = ", " +
                           stikproeve.PlanlagteStikproever.First()
                                     .Anmeldelse.Oprindelsessted.Postnummer.Value.ToString() + " " +
                           stikproeve.PlanlagteStikproever.First().Anmeldelse.Oprindelsessted.PostDistrikt;
            }
          }

          var stat = _statusStikproeveBusiness.GetLastStatus(stikproeve.StatusStikproeve);
          if (stat != null)
          {
            soegeResultat.Status = stat.StatusStikproeveType.Navn;
            soegeResultat.SenesteStatusDato = stat.Tid;
          }

          if (String.IsNullOrEmpty(soegeResultat.Status))
            soegeResultat.Status = " - ";

          soegeResultatList.Add(soegeResultat);
        }
      // sort by dato
      soegeResultatList = (soegeResultatList.OrderByDescending(res => res.SenesteStatusDato)).ToList();

      return soegeResultatList;
    }


    #endregion *** Private methods ***


  }
}
