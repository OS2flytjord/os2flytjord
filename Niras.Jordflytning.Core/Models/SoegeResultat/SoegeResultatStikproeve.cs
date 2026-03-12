using System;

namespace Niras.Jordflytning.Core.Models.SoegeResultat
{
	/// <summary>
	/// A row from a search result
	/// </summary>
	public class SoegeResultatStikproeve
	{
		public Guid StikproeveId { get; set; }

		/// <summary>
		/// Den seneste gang status er ændret
		/// </summary>
		public DateTime SenesteStatusDato { get; set; }		
	
		public string ModtageAnlaegNavn { get; set; }
		public decimal? BaasNr { get; set; }
    public string ProeveTagerNavn { get; set; }
		public string AnalyseretAf { get; set; }
		public string TransportoerNavn { get; set; }
		public string Status { get; set; }

		public decimal LoebeNummer { get; set; }
	  
    public decimal AnmeldelseLoebenr { get; set; }
	  public string Adresse { get; set; }

	}
}
