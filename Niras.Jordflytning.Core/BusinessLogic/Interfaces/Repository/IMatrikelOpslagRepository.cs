using System.Collections.Generic;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.MatrikelOpslag;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository
{
    public interface IMatrikelOpslagRepository
    {
        MatrikelOpslagResultat Get(string ejerlavkode, string matrikelnr);
        MatrikelOpslagResultat Get(string wkt);
        string GetEsrEjendomsnummer(string ejerlavkode, string matrikelnr);
        IList<Matrikel> GetListFromRestService(string wkt);
    }
}
