using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;


namespace Niras.Jordflytning.Infrastructure.DataAccess
{
	public class BogholderOpslagstavleRepository : GenericRepository<BogholderOpslagstavle>, IBogholderOpslagstavleRepository
	{
    public BogholderOpslagstavleRepository(IConfigService configService)
      : base(configService)
		{

		}

	}
}
