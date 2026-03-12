using System;
using System.Collections.Generic;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.BomSystem;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
	public interface IBomSystemBusiness
	{
		BomAnmeldelseList GetBomAnmeldelser(int modtagerAnlaegId, DateTime tidsStempel);
		List<Vognlaes> CreateVognlaes(IList<VognlaesBom> vognlaesList);
		List<KoereToej> GetKoeretoejer(DateTime tidsStempel);
	}
}
