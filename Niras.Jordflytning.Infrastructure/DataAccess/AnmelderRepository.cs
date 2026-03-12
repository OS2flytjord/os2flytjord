using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
	public class AnmelderRepository : GenericRepository<Anmelder>, IAnmelderRepository
	{
		public AnmelderRepository(IConfigService configService): base(configService)
		{

		}

	}
}