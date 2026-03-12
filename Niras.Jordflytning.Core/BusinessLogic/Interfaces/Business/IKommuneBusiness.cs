using Niras.Jordflytning.Core.Models;
using System;
using System.Collections.Generic;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
	public interface IKommuneBusiness : IGenericBusiness<Kommune>
  {
    IList<Kommune> ReadAktiveKommuner();
    Kommune ReadKommune(Guid kommuneId);
    Kommune ReadKommuneByNavn(string kommunenavn);
	Guid GetKommuneIdByUserId(Guid userId);

  }
}
