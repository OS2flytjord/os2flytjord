using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
    public class GebyrTidsregistreringRepository : GenericRepository<GebyrTidsregistrering>, IGebyrTidsregistreringRepository
    {
        public GebyrTidsregistreringRepository(IConfigService configService)
            : base(configService)
        {
        }
    }
}
