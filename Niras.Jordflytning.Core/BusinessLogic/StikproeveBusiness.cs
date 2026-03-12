using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Library.Logging;
using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Linq;

namespace Niras.Jordflytning.Core.BusinessLogic
{
    public class StikproeveBusiness : GenericBusiness<Stikproeve>, IStikproeveBusiness
  {
    private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.StikproeveBusiness");

    private readonly IStikproeveRepository _stikproeveRepo;
    private readonly IEnhedTypeRepository _enhedTypeRepository;
    private readonly IStatusStikproeveBusiness _statusStikproeveBusiness;
    private readonly IBrugereBusiness _brugerBusiness;


    public StikproeveBusiness(
      IStikproeveRepository stikproeveRepository,
      IStatusStikproeveBusiness statusStikproeveBusiness,
      IBrugereBusiness brugerBusiness,
      IUnitOfWork uow,
      IEnhedTypeRepository enhedTypeRepository
      )
      : base(stikproeveRepository, uow)
    {
      _stikproeveRepo = stikproeveRepository;
      _enhedTypeRepository = enhedTypeRepository;
      _statusStikproeveBusiness = statusStikproeveBusiness;
      _brugerBusiness = brugerBusiness;

    }

    public Enhed ReadEnhed(Guid enhedId)
    {
      return _enhedTypeRepository.Read(enhedId);
    }

    public bool SkalProevetagerAdviseres(Stikproeve stikproeve, ModtagerAnlaeg modtagerAnlaeg)
    {
      if (modtagerAnlaeg.AnvenderJF && modtagerAnlaeg.AntalBaase.HasValue && modtagerAnlaeg.AntalBaase > 0 && stikproeve != null)
      {
        if (modtagerAnlaeg.AntalBaase.Value == stikproeve.Baas | Math.Floor(modtagerAnlaeg.AntalBaase.Value / 2) == stikproeve.Baas)
        {
          //Har et modtageranlæg 20 båse. Så skal proevetageren adviseres, når bås 20 eller 10 er blevet fyldt
          //Har modtageranlæget 15 båse, så skal prøvetageren adviseres, når bås 15 eller 7 er blevet fyldt
          return true;
        }
      }
      return false;
    }

    public IList<AnalyseDokument> HentAnalyseDokumenter(DbGeometry matrikel, Guid excludeAnmeldelseId)
    {
      var stikp = _stikproeveRepo.Search(x =>
        x.Vognlaes.Anmeldelse.Oprindelsessted.Geom.Intersects(matrikel) ||
        (x.Vognlaes.Anmeldelse.Oprindelsessted.Matrikel.FirstOrDefault(m => m.Geom.Intersects(matrikel)) != null));

      IList<AnalyseDokument> analyseDokument = new List<AnalyseDokument>();
      foreach (var s in stikp)
      {
	      if (s.Vognlaes == null || s.Vognlaes.Anmeldelse == null || s.Vognlaes.Anmeldelse.Id == excludeAnmeldelseId) 
					continue;

	      foreach (var ad in s.AnalyseDokument)
	      {
		      analyseDokument.Add(ad);
	      }
      }
	    return analyseDokument;
    }

    public void KnytVognlæsTilPlanlagtStikprøve(Vognlaes vognlaes, int? baasNummer, PlanlagteStikproever planlagteStikproeve)
    {
      //Stikprøven er oprettet men vognlæset skal knyttes til stikprøven
      var stikproeve = planlagteStikproeve.Stikproeve;
      stikproeve.Vognlaes = vognlaes;
      stikproeve.Baas = baasNummer;

      //håndterer at currentuser er null, når det er bomservicen som kalder denne metode.
      var person = _brugerBusiness.GetCurrentLoggedOnPerson();
      var statusStikproeve = _statusStikproeveBusiness.CreateStatus(EnumStatusStikproeve.OprettetAdHoc, person);
      stikproeve.StatusStikproeve.Add(statusStikproeve);

      //Der skal ikke være nogen planlagtstikprøve nu når der er knyttet et vognlæs på stikprøven
      stikproeve.PlanlagteStikproever = null;

      Create(stikproeve);
    }

    /// <summary>
    /// Henter stikprøver tildelt en lab, hvor analysen ikke er udført og prøven er udtaget.
    /// </summary>
    public IList<Stikproeve> GetStikproeverForLabPerson(Guid labPersonId)
    {
      var stikp = _stikproeveRepo.Search(s => 
				s.LabPersonId == labPersonId
				&& s.StatusStikproeve.Any(ss =>ss.StatusStikproeveType.Kode ==(short)EnumStatusStikproeve.ProeveUdtaget)
				&& s.StatusStikproeve.Any(ss => ss.StatusStikproeveType.Kode ==(short)EnumStatusStikproeve.AnalyseUdfoert) == false
        ).ToList();
      return stikp;
    }

	  public Stikproeve GetStikproeveOnVognlaes(Vognlaes vognlaes)
	  {
		  Stikproeve stikproeve = null;
		  if (vognlaes != null && vognlaes.Stikproeve != null)
		  {
			  stikproeve = vognlaes.Stikproeve.FirstOrDefault();
		  }
		  return stikproeve;
	  }

	  public void CreateStikProeve(Vognlaes vognlaes, int? baasNummer)
    {
      try
      {
        var stikproeve = new Stikproeve();
        stikproeve.Vognlaes = vognlaes;
        stikproeve.Baas = baasNummer;

        //håndterer at currentuser er null, når det er bomservicen som kalder denne metode.
        var person = _brugerBusiness.GetCurrentLoggedOnPerson();

        //Når det ikke er en planlagt stikprøve skal statusstikprøvetypen "ModtagetHosModtageranlaeg" anvendes.
        var statusStikproeve = _statusStikproeveBusiness.CreateStatus(EnumStatusStikproeve.ModtagetHosModtageranlaeg, person);

        stikproeve.StatusStikproeve.Add(statusStikproeve);
        Create(stikproeve);
      }
      catch (Exception e)
      {
        Logger.LogException(e);
        throw;
      }
    }

  }
}
