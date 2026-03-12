using System;
using System.Collections.Generic;
using Niras.Jordflytning.Core.Models.SoegeResultat;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
	public interface ISoegBusinessOpslagsTavle
  {
	  List<SoegeResultatOpslagsTavle> GetOpslag(Guid brugerId);
  }
}
