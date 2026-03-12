using System;
using System.Collections.Generic;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic
{
  public class StatusBetalerBusiness : GenericBusiness<StatusBetaler>, IStatusBetalerBusiness
  {  
    private readonly IStatusBetalerRepository _statusBetalerRepository;

    public StatusBetalerBusiness(IStatusBetalerRepository statusBetalerRepository, IUnitOfWork uow ) : base(statusBetalerRepository, uow)
    {
      _statusBetalerRepository = statusBetalerRepository;
    }

    /// <summary>
    /// Anmode om godkendelse hos jordmodtage ved afsendelse af anmeldelse.
    /// </summary>
    /// <returns>True hvis betaleren ikke er i badstandings hos jordmodtagerfirmaet.</returns>
    public bool AnmodOmGodkendelseHosJordmodtager(Betaler betaler, Jordmodtager jordmodtager)
    {
      if (betaler == null)
        return true; // Betaleren kan godt være null, hvis modtagernanlæget ikke anvender FlytJord. I disse tilfælde giver det ikke mening at registrere en betaler til anmeldelsen.


      var statusBetaler =
        (from sb in _statusBetalerRepository
					 .Search(x => 
						 x.Betaler.Id == betaler.Id && 
						 x.Jordmodtager.Id == jordmodtager.Id)
         select sb).FirstOrDefault();
      if (statusBetaler != null)
      {
	      return !statusBetaler.Godkendt.HasValue || statusBetaler.Godkendt.Value;
      }
	    //Betaleren findes ikke i statusBetaler - Han må oprettes.      
	    var sp = new StatusBetaler();
	    sp.Betaler = betaler;
	    sp.Jordmodtager = jordmodtager;
	    sp.Redigeret = DateTime.Now;
	    Create(sp);
	    return true;
    }

    /// <summary>
    /// Henter en liste af statusbetaler. Listen skal ikke indeholde statusbetaler oplysningerne for jordmodtagerfirmaet
    /// </summary>
    public IList<StatusBetaler> GetStatusBetalerForBetaler(Betaler betaler, Guid jordmodtagerId)
    {
	    var list =
		    (from sb in _statusBetalerRepository
			     .Search(x =>
			             x.Betaler.Id == betaler.Id &&
			             x.Godkendt.HasValue &&
			             x.Jordmodtager.Id != jordmodtagerId)
		     select sb)
			    .OrderBy(f => f.Redigeret).ToList();

	    return list;
    }

	  /// <summary>
	  /// Er den givne betaler godkendt af modtageanlægget
	  /// </summary>
	  public bool IsBetalerGodkendtForJordmodtager(Guid jordmodtagerId, Guid betalerId)
	  {
		  var isGodkendt = false;

		  var statBetal = GetStatusBetaler(betalerId, jordmodtagerId);
		  if (statBetal != null && statBetal.Godkendt == true)
			  isGodkendt = true;

		  return isGodkendt;
	  }

	  /// <summary>
	  /// Er den givne betaler spærret af og for jordmodtagerfirmaet
	  /// </summary>
	  public bool IsBetalerSpaerretForJordmodtager(Guid jordmodtagerId, Guid betalerId)
	  {
		  var isSpaerret = false;

		  var statBetal = GetStatusBetaler(betalerId, jordmodtagerId);
		  if (statBetal != null && statBetal.Godkendt == false)
			  isSpaerret = true;

		  return isSpaerret;
	  }

		/// <summary>
		/// True = Godkendt
		/// False = Afvist
		/// Null = Under behandling
		/// </summary>
	  public bool? GetGodkendtStatusForBogholder(Anmeldelse anmeldelse)
	  {
			bool? godkendtStatus = null;

		  if (anmeldelse.ModtagerAnlaeg != null && anmeldelse.ModtagerAnlaeg.Jordmodtager != null && anmeldelse.BetalerId != null)
		  {
			  var jordmodtagerId = anmeldelse.ModtagerAnlaeg.Jordmodtager.Id;
			  var betalerId = (Guid) anmeldelse.BetalerId;

			  var statBetal = GetStatusBetaler(betalerId, jordmodtagerId);
			  if (statBetal != null)
				  godkendtStatus = statBetal.Godkendt;
		  }
		  return godkendtStatus;
	  }

	  public StatusBetaler GetStatusBetaler(Guid betalerId, Guid jordmodtagerId)
    {
			var retVal = 
				(from sb in _statusBetalerRepository
					 .Search(x => 
						 x.Betaler.Id == betalerId && 
						 x.Jordmodtager.Id == jordmodtagerId) 
				 select sb)
				 .FirstOrDefault();

	    return retVal;
    }
  }
}
