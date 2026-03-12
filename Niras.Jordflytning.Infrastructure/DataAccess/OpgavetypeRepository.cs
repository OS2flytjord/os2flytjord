using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
    public class OpgavetypeRepository : GenericRepository<Opgavetype>, IOpgavetypeRepository
	{
        public OpgavetypeRepository(IConfigService configService) : base(configService)
        {
        }
    }
}
