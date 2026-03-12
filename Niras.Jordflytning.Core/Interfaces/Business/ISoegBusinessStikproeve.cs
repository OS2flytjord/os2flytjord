using System;
using System.Collections.Generic;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.Interfaces.Business
{
	public interface ISoegBusinessStikproeve
	{
		List<SoegeResultatStikproeve> GetAktuelleStikproeveList(Guid stikproveId);
	}
}
