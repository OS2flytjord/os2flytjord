using System;

namespace Niras.Jordflytning.Core.Models.SoegeResultat
{
	/// <summary>
	/// A row from a search result
	/// </summary>
	public class SoegeResultatOpslagsTavle
	{
		public Guid NoteId { get; set; }
		public string Besked { get; set; }
		public string DatoOprettet { get; set; }
		public string DatoAendret { get; set; }
		public string AendretAfNavn { get; set; }
		public Guid AendretAfId { get; set; }
		public string BetalerNavn { get; set; }
		public Guid BetalerId { get; set; }

		public DateTime SenesteDato { get; set; }
	}
}
