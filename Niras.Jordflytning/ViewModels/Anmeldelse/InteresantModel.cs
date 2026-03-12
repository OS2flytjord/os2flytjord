using System;
using System.ComponentModel.DataAnnotations;

namespace Niras.Jordflytning.ViewModels.Anmeldelse
{
  public class InteresantModel
  {
    [ScaffoldColumn(false)]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "*")]
    [StringLength(100, ErrorMessage = @"Navnet er for lang.")]
    public string Navn { get; set; }

    [Required(ErrorMessage = "*")]
    [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Email adressen er ikke gyldig.")]
    [StringLength(100, ErrorMessage = @"Email adressen er for lang.")]
    public string Email { get; set; }
  }
}