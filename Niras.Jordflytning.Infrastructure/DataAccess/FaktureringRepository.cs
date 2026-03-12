using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
    internal class FaktureringRepository : GenericRepository<FaktureringUdtraek>, IFaktureringRepository
    {
        public FaktureringRepository(IConfigService configService) : base(configService)
        {
        }
    }
}
