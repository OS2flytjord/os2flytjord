using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
	public class PersonRepository : GenericRepository<Person>, IPersonRepository
	{

		public PersonRepository(IConfigService configService)
			: base(configService)
		{
		}
	}
}
