using System;
using System.Collections.Generic;
using Niras.Jordflytning.Core.Models.SoegeResultat;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{

	public interface ISoegAdminBusiness
  {
	  List<SoegeResultatBetaler> GetBetalereBy(
		  string firmaNavn,
		  string navn,
			int isGodkendt,
		  int kerneKunde,
		  Guid brugerId);

	  List<SoegeResultatStikproeve> GetStikproeverBy(
			DateTime? paramFoerDato, 
			DateTime? paramEfterDato, 
			decimal? paramStikproeveNummer, 
			Guid? paramStikproeveStatusGuid, 
			Guid personId);

		List<SoegeResultatVognlaes1> GetVognlaesBy(DateTime? vognlaesFoerDato, DateTime? vognlaesEfterDato, EnumVognlaesReturnType vognlaesReturnType, Guid personId);
		List<SoegeResultatVognlaes2> GetVognlaesBy(DateTime? vognlaesFoerDato, DateTime? vognlaesEfterDato, Guid personId);
		List<SoegeResultatStikproeve> GetStikproeverDerErAktive(Guid personId);
		List<SoegeResultatFakturaExport> GetVognlaesFaktura(List<Guid> vognlaesGuidList);
  }
}
