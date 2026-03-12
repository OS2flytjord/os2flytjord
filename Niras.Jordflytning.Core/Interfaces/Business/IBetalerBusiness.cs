using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.Interfaces.Business
{
	public interface IBetalerBusiness : IGenericBusiness<Betaler>
	{
	  Betaler ReadBetaler(Guid personId);

	}
}
