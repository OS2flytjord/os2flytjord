using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niras.Jordflytning.Core.Models.MatrikelOpslag
{
  public class Polygon
  {
    public string type { get; set; }
    public List<List<List<String>>> coordinates { get; set; }
  }
}
