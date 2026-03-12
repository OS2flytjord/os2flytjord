using System;
using System.Collections.Generic;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.Interfaces.Business
{
  public interface ISoegBusiness
  {
    List<SoegeResultat> GetAktiveAnmeldelser(Guid brugerId);
		List<SoegeResultat> GetAfsluttedeAnmeldelser(Guid brugerId);
		List<SoegeResultat> GetFremSendteAnmeldelser(Guid brugerId);
		List<SoegeResultat> GetIkkeFremSendteAnmeldelser(Guid brugerId);

		List<SoegeResultat> GetAllAnmeldelser(Guid brugerId);
		List<SoegeResultat> GetAnmeldelserBy(decimal? loebeNr, string adresse, DateTime? foerDate, DateTime? efterDate, Guid? transportoerId, Guid? anmelderId, Guid? modtagerId, Guid brugerId, bool isKommune);
		
		List<SoegeResultat> GetMineAnmeldelser(Guid sagsbehandlerId);
	  List<SoegeResultat> GetAnmeldelserDerKraeverHandling(Guid kommuneGuid);
  }
}
