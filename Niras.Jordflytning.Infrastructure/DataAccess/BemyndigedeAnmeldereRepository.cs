using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;


namespace Niras.Jordflytning.Infrastructure.DataAccess
{
  public class BemyndigedeAnmeldereRepository : GenericRepository<BemyndigedeAnmeldere>, IBemyndigedeAnmeldereRepository
	{
    public BemyndigedeAnmeldereRepository(IConfigService configService)
      : base(configService)
		{

		}

	}
}
