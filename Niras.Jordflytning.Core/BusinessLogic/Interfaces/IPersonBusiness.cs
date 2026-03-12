using System;
using System.Collections.Generic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces
{
  public interface IPersonBusiness : IGenericBusiness<Person>
  {
		Person ReadByBrugerId(Int32 brugerId);
    IList<Person> SearchByEmail(string email);

	  List<Person> GetAndrePersonerIfirma(Guid brugerId);
  }
}
