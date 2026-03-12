using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;


namespace Niras.Jordflytning.Infrastructure.DataAccess
{
	public class webpages_RolesRepository : GenericRepository<webpages_Roles>, Iwebpages_RolesRepository
	{
		public webpages_RolesRepository(IConfigService configService)
			: base(configService)
		{

		}

	}
}
