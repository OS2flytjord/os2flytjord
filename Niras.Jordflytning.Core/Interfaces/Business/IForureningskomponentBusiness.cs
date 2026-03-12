using System.Collections.Generic;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.Interfaces.Business
{
	public interface IForureningskomponentBusiness : IGenericBusiness<Forureningskomponent>
  {

    IList<Forureningskomponent> ReadAktiveForureningskomponenter();

	}
}