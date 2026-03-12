using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
	public class PersonJordmodtagerRepository : GenericRepository<PersonJordmodtager>, IPersonJordmodtagerRepository
	{
		public PersonJordmodtagerRepository(IConfigService configService)	: base(configService)
		{

		}
	}
}
