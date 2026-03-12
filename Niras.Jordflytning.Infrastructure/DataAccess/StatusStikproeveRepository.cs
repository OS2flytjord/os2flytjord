using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
  public class StatusStikproeveRepository : GenericRepository<StatusStikproeve>, IStatusStikproeveRepository
	{
    public StatusStikproeveRepository(IConfigService configService): base(configService)
		{

		}

	} 
}
