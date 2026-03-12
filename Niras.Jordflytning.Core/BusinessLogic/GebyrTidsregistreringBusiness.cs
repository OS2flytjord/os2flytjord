using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using System.IO;

namespace Niras.Jordflytning.Core.BusinessLogic
{
    public class GebyrTidsregistreringBusiness : GenericBusiness<GebyrTidsregistrering>, IGebyrTidsregistreringBusiness
    {
        public GebyrTidsregistreringBusiness(IGebyrTidsregistreringRepository repo, IUnitOfWork unitOfWork) : base(repo, unitOfWork)
        {
        }
    }
}
