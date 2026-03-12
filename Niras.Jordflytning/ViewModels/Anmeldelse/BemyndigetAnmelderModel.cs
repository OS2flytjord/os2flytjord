using System;

namespace Niras.Jordflytning.ViewModels.Anmeldelse
{
  public class BemyndigetAnmelderModel
  {
    public Guid Id { get; set; }

    public String Firma { get; set; }
    public String Navn { get; set; }
    public String Adresse { get; set; }
    public String Postnr { get; set; }
    public string Telefon { get; set; }
    public string CVR { get; set; }
    public string PNr { get; set; }
    public string Kontakt { get; set; }
    public string Mobil { get; set; }
    public string Email { get; set; }


  }
}