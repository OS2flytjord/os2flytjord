using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
  public class GraensevaerdierRepository : GenericRepository<Graensevaerdier>, IGraensevaerdierRepository
	{
		public GraensevaerdierRepository(IConfigService configService)
			: base(configService)
		{
		}

	}
}
