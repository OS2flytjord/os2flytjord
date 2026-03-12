using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Foolproof;

namespace Niras.Jordflytning.ViewModels
{
	/// <summary>
	/// View model class for sandboxing, i.e. debug, proof of concept etc.
	/// The model is used in sandboxing views and partial views.
	/// </summary>
	public class SandboxViewModel
	{
		public SandboxViewModel()
		{
			Vip = false;
		}

		#region Validation properties

		[Display(Name="Navn")]
		[Required(ErrorMessage = "Navn skal udfyldes")]
		public String Name { get; set; }

		[Display(Name = "Fødselsdato")]
		[Required(ErrorMessage = "Fødselsdato skal udfyldes")]
		public DateTime BirthDate { get; set; }

		[Display(Name = "VIP")]
		public Boolean Vip { get; set; }

		[Display(Name = "VIP nummer")]
		[RequiredIfTrue("Vip", ErrorMessage = "Som VIP skal du udfylde dit VIP nr.")]
		public Int32? VipNumber { get; set; }

		[Display(Name = "Beløb")]
		[RequiredIfTrue("Vip", ErrorMessage = "Som VIP skal du udfylde beløb")]
		public Decimal? Amount { get; set; }

		[Display(Name = "Fritekst")]
		public String Value { get; set; }

		#endregion
	}
}