using Niras.Jordflytning.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niras.Jordflytning.Core.Interfaces.Business
{
	public interface IKommuneBusiness : IGenericBusiness<Kommune>
  {
    IList<Kommune> ReadAktiveKommuner();
    Kommune ReadKommune(Guid kommuneId);

  }
}
