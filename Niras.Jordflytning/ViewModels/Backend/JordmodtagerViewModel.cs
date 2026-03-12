using System;
using System.ComponentModel.DataAnnotations;

namespace Niras.Jordflytning.ViewModels.Backend
{
  public class JordmodtagerViewModel
	{

		[ScaffoldColumn(false)] // denne for at undgå at den bliver vist i kendo popup editor
		public Guid Id { get; set; }

		[Display(Name = @"Navn")]
		[Required(ErrorMessage = @"Navn er krævet.")]
    public string Navn { get; set; }

		[Display(Name = @"Adresse")]
		[Required(ErrorMessage = @"Adresse er krævet.")]
		public string Adresse { get; set; }

		[Display(Name = @"Postnummer")]
		[Required(ErrorMessage = @"Postnummer er krævet.")]
		public string Postnummer { get; set; }

		[Display(Name = @"By")]
		[Required(ErrorMessage = @"By er krævet.")]
		public string By { get; set; }

    [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = @"CVR nummeret skal være 8 cifre")]
    [Range(typeof(Decimal), "10000000", "99999999", ErrorMessage = @"CVR nummeret skal være 8 cifre")]
		[Display(Name = @"CVR")]
		[Required(ErrorMessage = @"CVR er krævet.")]
    
		public string Cvr { get; set; }

    [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = @"Telefonnummeret skal være 8 cifre")]
    [Range(typeof(Decimal), "10000000", "99999999", ErrorMessage = @"Telefonnummeret skal være 8 cifre")]
		[Display(Name = @"Telefon")]
		[Required(ErrorMessage = @"Telefon er krævet.")]
	  public string Telefon { get; set; }

		[Display(Name = @"Aktiv")]
	  public bool Aktiv { get; set; }

        [Display(Name = @"Jordmodtagerfirma med Flytjord abonnement ")]
        public bool JordmodtagerMedFlytjordAbonnement { get; set; }
	}
}