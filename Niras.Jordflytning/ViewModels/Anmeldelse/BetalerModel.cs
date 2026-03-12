using System;
using System.ComponentModel.DataAnnotations;

namespace Niras.Jordflytning.ViewModels.Anmeldelse
{
  public class BetalerModel
  {
    public Guid Id { get; set; }
    public String Firma { get; set; }
    public String Navn { get; set; }
    public String Adresse { get; set; }
    [Display(Name="Postnr.")]
    public String Postnr { get; set; }
    public string Telefon { get; set; }
    public string CVR { get; set; }
    [Display(Name = "P-nummer")]
    public string PNr { get; set; }

    public string Kontakt { get; set; }
    [Display(Name = "Mobil tlf.")]
    public string Mobil { get; set; }

    public string Email { get; set; }


  }
}