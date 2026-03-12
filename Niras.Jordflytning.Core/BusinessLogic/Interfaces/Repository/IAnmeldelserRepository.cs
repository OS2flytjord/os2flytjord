using Niras.Jordflytning.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository
{
    public interface IAnmeldelserRepository : IRepository<Anmeldelse>
    {
        //IEnumerable<Anmeldelse> ReadIncludeStatusAnmeldelse();
        IEnumerable<Anmeldelse> ReadIncludeVognlaesStikproeve();
        IEnumerable<Anmeldelse> SearchInclude(Expression<Func<Anmeldelse, bool>> predicate, EnumIncludeTables includeTables);

        IEnumerable<Anmeldelse> SearchBypassEF(Guid? KommuneId, string ModtagerAnlaegXmlList, DateTime? foerDate,
            DateTime? efterDate, decimal? loebeNr, Guid? modtagerId, Guid? transportoerId, Guid? anmelderId, string forureningsKategori);
    }

    public enum EnumIncludeTables
    {
        //StatusAnmeldelse_StatusAnmeldelseType,
        Vognlaes_Stikproeve_StatusStikproeve_StatusStikproeveType
    }
}
