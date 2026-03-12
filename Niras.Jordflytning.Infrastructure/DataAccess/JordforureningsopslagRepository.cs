using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
  public class JordforureningsopslagRepository : GenericRepository<Jordforureningsopslag>, IJordforureningsopslagRepository
	{
    public JordforureningsopslagRepository(IConfigService configService)
      : base(configService)
		{

		}

	}
}
