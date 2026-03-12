using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
  public class DokumenterRepository : GenericRepository<Dokumenter>, IDokumenterRepository
	{
		public DokumenterRepository(IConfigService configService)
			: base(configService)
		{
		}

	}
}
