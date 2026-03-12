using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
  public class FirmaoplysningerRepository : GenericRepository<Firmaoplysninger>, IFirmaoplysningerRepository
	{
		public FirmaoplysningerRepository(IConfigService configService)
      : base(configService)
		{
		}
	}
}
