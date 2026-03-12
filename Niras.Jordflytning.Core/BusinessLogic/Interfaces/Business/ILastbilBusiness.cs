using System.Collections.Generic;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{

  public interface ILastbilBusiness : IGenericBusiness<Lastbil>
  {
    List<Lastbil> GetAktiveLastbilList();

  }
}
