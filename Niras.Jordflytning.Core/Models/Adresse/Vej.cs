using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niras.Jordflytning.Core.Models.Adresse
{
  public class Vej
  {
    public Vej()
    {
      
    }

    public string href { get; set; }
    public string kode { get; set; }
    public string navn { get; set; }
    public Postnummer postnummer { get; set; }
    public Kommune kommune { get; set; }
    public etrs89koordinat etrs89koordinat { get; set; }
    public wgs84koordinat wgs84Koordinat { get; set; }
    

  }
}
