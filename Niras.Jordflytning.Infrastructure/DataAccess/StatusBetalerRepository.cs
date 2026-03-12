using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;


namespace Niras.Jordflytning.Infrastructure.DataAccess
{
	public class StatusBetalerRepository : GenericRepository<StatusBetaler>, IStatusBetalerRepository
	{
		public StatusBetalerRepository(IConfigService configService) : base(configService)
		{

		}

	}
}
