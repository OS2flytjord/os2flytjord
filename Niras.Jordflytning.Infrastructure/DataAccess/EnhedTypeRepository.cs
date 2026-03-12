using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;


namespace Niras.Jordflytning.Infrastructure.DataAccess
{
  public class EnhedTypeRepository : GenericRepository<Enhed>, IEnhedTypeRepository
	{
		public EnhedTypeRepository(IConfigService configService)
      : base(configService)
		{

		}

	}

}
