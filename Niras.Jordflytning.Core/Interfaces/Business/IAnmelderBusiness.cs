using System;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.Interfaces.Business
{
	public interface IAnmelderBusiness 
	{

	  Anmelder ReadAnmelder(Guid personId);

	}
}
