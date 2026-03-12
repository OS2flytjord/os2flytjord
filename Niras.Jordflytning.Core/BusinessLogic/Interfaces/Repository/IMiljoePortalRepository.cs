using Niras.Jordflytning.Core.Models;
using System.Collections.Generic;
using System.Data.Spatial;
using Niras.Jordflytning.Core.Models.JordForurening;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository
{
	public interface IMiljoePortalRepository
	{

		//ForureningsOpslag Read(Oprindelsessted oprindelsessted, IList<JordKlassifikationType> jordKlassifikationTypeList);
		List<ForureningsOpslagResult> GetResult(Oprindelsessted oprSted, IList<JordKlassifikationType> jordKlassifikationTypeList);
        KonfliktSoegningLeverandoerResultat KonfliktSoegningMidlertidigModtager(DbGeometry geom);

	}
}
