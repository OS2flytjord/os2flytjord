using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
    public class webpages_MembershipRepository : GenericRepository<webpages_Membership>, Iwebpages_MembershipRepository
    {
        public webpages_MembershipRepository(IConfigService configService)
            : base(configService)
        {
        }
    }
}
