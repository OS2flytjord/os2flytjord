using System;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
  public interface IVognlaesBusiness : IGenericBusiness<Vognlaes>
  {
    bool UpdateVognlaes(Guid vognlaesId, Vognlaes opdateretVognlaes);
    bool CreateVognlaes(Vognlaes vognlaes, Guid anmeldelseId, bool shouldCreateStikProeve, int? stikproeveBaas, bool ignoreStikProeve);
		DateTime? GetSenesteVognlaesTid(Anmeldelse anmeldelse);
	  bool AfvisVognlaes(Guid vognlaesId, string afvisNote);
  }
}
