using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
	public class AffaldTypeRepository : GenericRepository<AffaldType>, IAffaldTypeRepository
	{
    public AffaldTypeRepository(IConfigService configService): base(configService)
		{

		}

	}
}
