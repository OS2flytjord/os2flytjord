using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
	public class JordmodtagerRepository : GenericRepository<Jordmodtager>, IJordmodtagerRepository
	{
		public JordmodtagerRepository(IConfigService configService)	: base(configService)
		{

		}

	}
}
