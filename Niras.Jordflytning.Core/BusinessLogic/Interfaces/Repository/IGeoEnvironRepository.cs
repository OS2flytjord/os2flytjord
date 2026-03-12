using System.Collections.Generic;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.JordForurening;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository
{
	public interface IGeoEnvironRepository
	{
		List<string> Get(string ejerlav, string matrikelNummer, IList<JordKlassifikationType> jordKlassifikationTypeList, out string exceptionList);

		List<GeoEnvironRow> GetRows(string ejerlav, string matrikelNummer, IList<GeoEnvironKlassifikation> geoEnvironKlassifikationList, out string exceptionList);
		List<ForureningsOpslagResult> GetResult(string ejerlav, string ejerlavsnavn, string matrikelnr, IList<JordKlassifikationType> jordKlassifikationTypeList);
	}
}
