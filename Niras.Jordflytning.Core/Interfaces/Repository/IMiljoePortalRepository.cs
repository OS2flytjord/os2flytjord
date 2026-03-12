using Niras.Jordflytning.Core.Models;
using System.Collections.Generic;
using Niras.Jordflytning.Core.Models.JordForurening;

namespace Niras.Jordflytning.Core.Interfaces.Repository
{
	public interface IMiljoePortalRepository
	{

		ForureningsOpslag Read(Oprindelsessted oprindelsessted, IList<JordKlassifikationType> jordKlassifikationTypeList);
		List<ForureningsOpslagResult> GetResult(Oprindelsessted oprSted, IList<JordKlassifikationType> jordKlassifikationTypeList);

	}
}
