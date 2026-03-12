using System;
using System.Collections.Generic;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.Interfaces.Business
{
  public interface ISoegBusinessBetaler
  {
	  List<SoegeResultatBetaler> GetBetalerSomKrvHandling(Guid brugerId);
  }
}
