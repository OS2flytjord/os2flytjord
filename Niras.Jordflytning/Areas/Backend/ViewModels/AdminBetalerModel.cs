using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Niras.Jordflytning.Areas.Backend.ViewModels
{
  public class AdminBetalerModel
  {
    public AdminBetalerModel()
    {
      StatusBetalerListe = new List<StatusBetalerModel>();
    }

    //Betaler / Person
    public string Firmanavn { get; set; }
    public string PersonNavn { get; set; }
    public string Adresse { get; set; }
    public decimal? Postnr { get; set; }
    public string PostDistrikt { get; set; }
    public decimal? Telefon { get; set; }
    public string Email { get; set; }
    public decimal? Cvr { get; set; }
    public string Pnummer { get; set; }
    public string EAN { get; set; }

    //Status Betaler
    public Guid  StatusBetalerId { get; set; }
    public bool? StatusBetalerGodkendt { get; set; }
    public string InternBemaerkning { get; set; }
    public bool Kernekunde { get; set; }

    [DisplayFormat(DataFormatString = "{0:d}")]
    public DateTime StatusRedigeret { get; set; }

    public IEnumerable<StatusBetalerModel> StatusBetalerListe { get; set; }
    
    public string BetalerId { get; set; }
    public string JordmodtagerId { get; set; } 

    //Rights
    public bool RightIsBogholder { get; set; }
  }
}