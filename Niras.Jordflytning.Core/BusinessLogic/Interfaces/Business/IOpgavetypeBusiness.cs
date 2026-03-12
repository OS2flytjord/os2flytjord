using Niras.Jordflytning.Core.Models;
using System;
using System.Collections.Generic;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
    public interface IOpgavetypeBusiness : IGenericBusiness<Opgavetype>
    {

        IEnumerable<Opgavetype> ForKommune(Guid kommuneId);

    }
}
