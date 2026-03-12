using System;
using System.ComponentModel.DataAnnotations;

namespace Niras.Jordflytning.ViewModels.Anmeldelse
{
  public class TransportoerModel
  {
    public Guid Id { get; set; }

    [Display(Name = @"Miljøklasser på lastbiler")]
    public string LastbilMiljoeKlasserNavn { get; set; }
    
    public string Firma { get; set; }

    public string Navn { get; set; }
    
    public string Kontakt { get; set; }

    public string Adresse { get; set; }

    public string PostDistrikt { get; set; }
		
    public string PostNr { get; set; }

    public string Telefon { get; set; }

    public string Mobiltelefon { get; set; }

    public string Email { get; set; }
  }
}