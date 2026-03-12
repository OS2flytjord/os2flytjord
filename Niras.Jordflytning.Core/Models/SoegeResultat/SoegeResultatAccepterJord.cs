using System;

namespace Niras.Jordflytning.Core.Models.SoegeResultat
{
	/// <summary>
	/// A row from a search result
	/// </summary>
	public class SoegeResultatAccepterJord
	{
		public Guid AnmeldelseId { get; set; }
		public Guid BetalerId { get; set; }

		public string AnmeldLoebeNr { get; set; }
		public string OprindelsesSted { get; set; }
		public string JordMaengde { get; set; }
		public string KoerselsPeriode { get; set; }
		public string AnmelderNavn { get; set; }		
		public string BetalerNavn { get; set; }

		public DateTime SenesteStatusDato { get; set; }

		public string Status { get; set; }
	}
}
