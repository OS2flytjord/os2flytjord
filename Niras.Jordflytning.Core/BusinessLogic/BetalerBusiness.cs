using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using System;
using System.Linq;

namespace Niras.Jordflytning.Core.BusinessLogic
{
    public class BetalerBusiness :  GenericBusiness<Betaler>, IBetalerBusiness   
  {
    private readonly IBetalerRepository _betalerRepo;
    private readonly IBrugereBusiness _brugereBusiness;
    private readonly IBemyndigedeAnmeldereRepository _bemyndigedeAnmeldereRepository;
    private readonly IStatusBetalerBusiness _statusBetalerBusiness;

    public BetalerBusiness(
			IBetalerRepository betalerRepo, 
			IBrugereBusiness brugereBusiness,
			IUnitOfWork uow, 
			IBemyndigedeAnmeldereRepository bemyndigedeAnmeldereRepository,
      IStatusBetalerBusiness statusBetalerBusiness
			): base(betalerRepo, uow)
    {
      _betalerRepo = betalerRepo;
      _brugereBusiness = brugereBusiness;
      _bemyndigedeAnmeldereRepository = bemyndigedeAnmeldereRepository;
      _statusBetalerBusiness = statusBetalerBusiness;
    }

    public Betaler ReadBetaler(Guid personId)
    {
      var b = _betalerRepo.Search(x => x.Id == personId).FirstOrDefault(); //.Read(personId); Read(x) threw exceptions!

      if (b != null)
        return b;

      //Opret betaler
      b = new Betaler();
      b.Id = personId;
      b.Person = _brugereBusiness.ReadPerson(personId);
      return b;
    }

    public bool IsAnmelderBemyndigetByBetaler(Anmelder anmelder, Betaler betaler)
    {
      var res =
        (from b in _bemyndigedeAnmeldereRepository.Search(x => x.Anmelder.Person.Id == anmelder.Person.Id && x.Betaler.Person.Id == betaler.Person.Id) select b)
          .FirstOrDefault();
      
      return res != null;
    }

    public bool CheckAutoBetalerAccepterBetaling(Anmeldelse anmeldelse)
    {
      if (anmeldelse != null && anmeldelse.Betaler != null && anmeldelse.Anmelder != null)
      {
        //Kernekunder er jordmodtagerfirmaets betroede kunder, så betaler behøves ikke at acceptere betalingen via adviset
        var kerneKunde = false;
        if (anmeldelse.ModtagerAnlaeg != null && anmeldelse.ModtagerAnlaeg.Jordmodtager != null &&
            anmeldelse.ModtagerAnlaeg.Jordmodtager.AnvenderJF.HasValue &&
            anmeldelse.ModtagerAnlaeg.Jordmodtager.AnvenderJF.Value)
        {
          var statusBetaler = _statusBetalerBusiness.GetStatusBetaler(anmeldelse.Betaler.Id, anmeldelse.ModtagerAnlaeg.Jordmodtager.Id);
          if (statusBetaler != null &&
              statusBetaler.Godkendt.HasValue && statusBetaler.Godkendt.Value &&
              statusBetaler.KerneKunde.HasValue && statusBetaler.KerneKunde.Value)

            kerneKunde = true;
        }


        if (anmeldelse.Betaler.Id != anmeldelse.Anmelder.Id)
        {
          //Og Anmelderen ikke er betaler
          if (kerneKunde)
          {
            //Hvis anmelderen er bemyndiget af betaleren.
            if (IsAnmelderBemyndigetByBetaler(anmeldelse.Anmelder, anmeldelse.Betaler))
              return true;
          }
        }
        else
        {
          //Anmelder er også betaler
          if (kerneKunde)
            return true;
        }

      }
      return false;
    }
  }
}
