using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niras.Jordflytning.Core.Interfaces.Business
{
  public interface IPdfBusiness
  {
    void CreateAnmeldelseBlanket(Guid anmeldelseId);
  }
}
