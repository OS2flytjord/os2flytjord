using System;
using System.ComponentModel.DataAnnotations;

namespace Niras.Jordflytning.Areas.Backend.ViewModels
{
  public class BogholderOpslagstavleModel
  {
    public Guid? Id { get; set; }
    public string Tekst { get; set; }
    [DisplayFormat(DataFormatString = "{0:d}")]
    public DateTime Tid { get; set; }
    public string UdfoertAf { get; set; }
  }
}