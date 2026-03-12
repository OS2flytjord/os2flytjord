using System;

namespace Niras.Jordflytning.Core.Models.SoegeResultat
{
	/// <summary>
	/// A row from a search result
	/// </summary>
	public class SoegeResultatBetaler
	{		
		public Guid PersonId { get; set; }
		public Guid BetalerId { get; set; }
		
		public string BetalerNavn { get; set; }
		public string FirmaNavn { get; set; }
		public string Dato { get; set; }

		public string Bemaerkning { get; set; }

		public Guid? JordmodtagerId { get; set; }

		public string KerneKunde { get; set; }
		public string Status { get; set; }


	}
}
