using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;


namespace Niras.Jordflytning.Infrastructure.DataAccess
{
  public class SagsbehandlerRepository : GenericRepository<Sagsbehandler>, ISagsbehandlerRepository
	{
    public SagsbehandlerRepository(IConfigService configService)
      : base(configService)
		{

		}

	}
}
