using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
  public class StatusAnmeldelseRepository : GenericRepository<StatusAnmeldelse>, IStatusAnmeldelseRepository
	{
    public StatusAnmeldelseRepository(IConfigService configService): base(configService)
		{

		}

	} 
}
