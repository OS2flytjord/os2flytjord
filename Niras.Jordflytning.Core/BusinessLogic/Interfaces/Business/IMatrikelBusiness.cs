using System.Collections.Generic;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.MatrikelOpslag;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
    public interface IMatrikelBusiness
    {

        MatrikelOpslagResultat GetMatrikel(string ejerlavkode, string matrikelnummer);
        IList<Matrikel> ReadMatrikler(string wkt);
        void DeleteMatrikler(Anmeldelse anmeldelse);
        string GetEsrEjendomsnummer(string ejerlavkode, string matrikelnr);
    }
}
