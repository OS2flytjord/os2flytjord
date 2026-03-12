using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;


namespace Niras.Jordflytning.Infrastructure.DataAccess
{
  public class KommuneJordklassifikationRepository : GenericRepository<KommuneJordklassifikation>, IKommuneJordklassifikationRepository
	{
    public KommuneJordklassifikationRepository(IConfigService configService)
      : base(configService)
		{

		}

	}
}
