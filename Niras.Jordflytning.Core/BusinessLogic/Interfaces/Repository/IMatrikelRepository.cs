using System.Collections.Generic;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository
{
  public interface IMatrikelRepository : IRepository<Matrikel>
    {
        IList<Matrikel> GetListFromWfsService(string wkt);
    }
}
