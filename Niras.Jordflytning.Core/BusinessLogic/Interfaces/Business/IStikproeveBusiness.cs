using System;
using System.Collections.Generic;
using System.Data.Spatial;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
	public interface IStikproeveBusiness : IGenericBusiness<Stikproeve>
	{

    /// <summary>
    /// Prøvetager skal adviseres, når halvdelen af båsene er fyldt med jord.
    /// </summary>
    bool SkalProevetagerAdviseres(Stikproeve stikproeve, ModtagerAnlaeg modtagerAnlaeg);
	 
		Enhed ReadEnhed(Guid enhedId);

    IList<AnalyseDokument> HentAnalyseDokumenter(DbGeometry matrikel,Guid excludeAnmeldelseId);

		void CreateStikProeve(Vognlaes vognlaes, int? baasNummer);

	  void KnytVognlæsTilPlanlagtStikprøve(Vognlaes vognlaes, int? baasNummer, PlanlagteStikproever planlagteStikproeve);

	  IList<Stikproeve> GetStikproeverForLabPerson(Guid labPersonId);
		Stikproeve GetStikproeveOnVognlaes(Vognlaes vognlaes);
	}
}
