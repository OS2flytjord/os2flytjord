using Niras.Jordflytning.Core.Models;
using System;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository
{
	public interface IBrugereRepository : IRepository<BrugerProfil>
	{
		BrugerProfil Read(Int32 userId);

		BrugerProfil Read(String userName);

	}
}
