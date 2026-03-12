using Niras.Jordflytning.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Niras.Jordflytning.Core.Interfaces.Repository
{
	public interface IPersonRepository : IRepository<Person>
	{
		Person ReadByBrugerId(Int32 brugerId);
	}
}
