using System;
using System.Collections.Generic;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
	public interface ITransportoerBusiness : IGenericBusiness<Transportoer>
  {
    IList<Transportoer> ReadAktiveTransportoerer();
    IList<Transportoer> ReadTidligereAktiveAnvendteTransportoerer(Guid personid);
    Transportoer ReadTransportoer(Guid personId);
	Transportoer ReadTransportoer(decimal? cvr);
    IList<Transportoer> ReadTransportoererBypassEf();
  }
}
