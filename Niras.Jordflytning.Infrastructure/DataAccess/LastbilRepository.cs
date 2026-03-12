using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
	public class LastbilRepository : GenericRepository<Lastbil>, ILastbilRepository
	{
		public LastbilRepository(IConfigService configService)
			: base(configService)
		{
		}
	}
}
