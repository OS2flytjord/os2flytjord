using System;

namespace Niras.Jordflytning.Core.Models.SoegeResultat
{
	/// <summary>
	/// A row from a search result
	/// </summary>
	public class SoegeResultatVognlaes2
	{

		public Guid VognlaesId { get; set; }
		public Guid AnmeldelseId { get; set; }

		/// <summary>
		/// Opgravningssted: angivelse af adressen, vejen, anden oprindelse 
		/// </summary>
		public string Opgravningssted { get; set; }
		
		/// <summary>
		/// Oprindelseskommune
		/// </summary>
		public string Oprindelseskommune { get; set; }
		
		/// <summary>
		/// Løbenummer
		/// </summary>
		public string Loebenummer { get; set; }
		
		/// <summary>
		/// Jordtype: ingen, fejesand, boremudder, stik og brud mv 
		/// </summary>
		public string Jordtype { get; set; }
		
		/// <summary>
		/// Forureningskategori: ren jord, let forurenet jord, anden klassifikation
		/// </summary>
		public string Forureningskategori { get; set; }
		
		/// <summary>
		/// Mængder: modtaget mængde i ton 
		/// </summary>
		public double JordmaengdeTons { get; set; }		
		
		/// <summary>
		/// Mængder: modtaget mængde i aksler 
		/// </summary>
		public int JordmaengdeAksler { get; set; }
		

		/// <summary>
		/// Modtageanlæg
		/// </summary>
		public string Modtageanlaeg { get; set; }
		
		/// <summary>
		/// Modtagedato
		/// </summary>
		public string Modtagedato { get; set; }

		/// <summary>
		/// Er vognlæsset blevet afvist
		/// </summary>
		public bool Afvist { get; set; }

		/// <summary>
		/// Begrundelse for afvisning
		/// </summary>
		public string AfvistNote { get; set; }

	}

}
