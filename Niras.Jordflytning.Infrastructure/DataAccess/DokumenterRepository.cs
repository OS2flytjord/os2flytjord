using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
  public class DokumentationRepository : GenericRepository<Dokumentation>, IDokumentationRepository
	{
    public DokumentationRepository(IConfigService configService) : base(configService)
		{

		}

	}
}
