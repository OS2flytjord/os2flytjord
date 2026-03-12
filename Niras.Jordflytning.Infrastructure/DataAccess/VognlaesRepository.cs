using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
	public class VognlaesRepository : GenericRepository<Vognlaes>, IVognlaesRepository
	{
		public VognlaesRepository(IConfigService configService) : base(configService)
		{
		}
	}
}
