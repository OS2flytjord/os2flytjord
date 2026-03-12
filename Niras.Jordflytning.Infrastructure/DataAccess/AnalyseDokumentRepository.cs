using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
	public class AnalyseDokumentRepository : GenericRepository<AnalyseDokument>, IAnalyseDokumentRepository
	{
    public AnalyseDokumentRepository(IConfigService configService): base(configService)
		{

		}

	}
}
