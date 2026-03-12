using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Niras.Jordflytning.ViewModels.Anmeldelse
{
  public class VejModel
  {
    public string displayname { get; set; }
    public string vejnavn { get; set; }
    public string vejkode { get; set; }
    public string postnr { get; set; }
		public string label { get; set; }
    public bool  aktivKommune { get; set; }
  }
}