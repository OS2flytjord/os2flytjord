using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Niras.Jordflytning.Core.Interfaces.Business;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.Interfaces
{
  public interface IPersonBusiness : IGenericBusiness<Person>
  {
  
    IList<Person> SearchByEmail(string email);

  }
}
