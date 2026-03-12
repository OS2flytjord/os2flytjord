using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
	public class PlanlagteStikproeverRepository : GenericRepository<PlanlagteStikproever>, IPlanlagteStikproeverRepository
	{
		public PlanlagteStikproeverRepository(IConfigService configService)	: base(configService)
		{

		}
	}
}
