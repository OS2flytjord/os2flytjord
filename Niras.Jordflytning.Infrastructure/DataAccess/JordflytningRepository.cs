using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
  public class JordflytningTypeRepository : GenericRepository<JordflytningType>, IJordflytningTypeRepository
  {
    public JordflytningTypeRepository(IConfigService configService): base(configService)
    {

    }

  }
}
