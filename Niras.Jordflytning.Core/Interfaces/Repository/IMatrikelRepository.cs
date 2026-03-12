using System.Collections.Generic;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.Interfaces.Repository
{
    public interface IMatrikelRepository
    {
        IList<Matrikel> GetList(string wkt);
    }
}
