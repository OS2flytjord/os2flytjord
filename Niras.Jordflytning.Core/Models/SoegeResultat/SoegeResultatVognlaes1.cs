using System;
using System.ComponentModel.DataAnnotations;

namespace Niras.Jordflytning.Core.Models.SoegeResultat
{
	/// <summary>
	/// A row from a search result
	/// </summary>
	public class SoegeResultatVognlaes1
	{
		public Guid VognlaesId { get; set; }

		public string Dato { get; set; }
		public string BetalerNavn { get; set; }
		public string ModtageAnlaegNavn { get; set; }
		public string TransportoerNavn { get; set; }
		public string JordmaengdeTons { get; set; }
		public string JordmaengdeAksler { get; set; }
		public string Status { get; set; }
		public Guid? AnmeldelseId { get; set; }
		public string Adresse { get; set; }
		public DateTime? VognlaesDato { get; set; }

		public bool Afvist { get; set; }

		[StringLength(256, ErrorMessage = @"Begrundelse må højst være 256 lang.")]
		public string AfvistNote { get; set; }
	}
}
