using System;
using Niras.Jordflytning.Core.Models;
using System.Collections.Generic;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
	public interface IAnmelderBusiness : IGenericBusiness<Anmelder>
	{
	    Anmelder ReadAnmelder(Guid personId);
	    IList<Anmelder> ReadAnmeldereBypassEf();
	}
}
