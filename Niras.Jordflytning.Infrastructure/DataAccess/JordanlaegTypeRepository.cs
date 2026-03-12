using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
	public class JordanlaegTypeRepository : GenericRepository<JordanlaegType>, IJordanlaegTypeRepository
	{
		public JordanlaegTypeRepository(IConfigService configService): base(configService)
		{

		}

	}
}
