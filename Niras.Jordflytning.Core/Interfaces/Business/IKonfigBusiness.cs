using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niras.Jordflytning.Core.Interfaces.Business
{
  public interface IKonfigBusiness
  {
    string ReadKommuneKonfig(string key,Guid kommuneId);
  }
}
