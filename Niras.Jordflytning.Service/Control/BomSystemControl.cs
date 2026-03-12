using System;
using System.Collections.Generic;
using Niras.Jordflytning.Service.DataAccess;
using Niras.Jordflytning.Service.Model;

namespace Niras.Jordflytning.Service.Control
{
	/// <summary>
	/// Håndtering af flow 
	/// (dvs. hvad skal der f.ex. ske, 
	/// når man creater et vognlæs)
	/// </summary>
	public class BomSystemControl
	{
		public List<BomAnmeldelse> GetBomAnmeldelser(string jordmodtagerAnlaegId, DateTime tidsStempel)
		{
			var dataAccess = new BomSystemDataAccess();
			var list = dataAccess.GetBomAnmeldelser(jordmodtagerAnlaegId, tidsStempel);
			return list;
		}

		public bool CreateVognlaes(IList<Vognlaes> vognlaesList)
		{
			var dataAccess = new BomSystemDataAccess();
			var retVal = dataAccess.CreateVognlaes(vognlaesList);
			return retVal;
		}

		public int CreateStikproeve(Vognlaes vognlaes)
		{
			var dataAccess = new BomSystemDataAccess();
			var retVal = dataAccess.CreateStikproeve(vognlaes);
			return retVal;
		}

	}
}