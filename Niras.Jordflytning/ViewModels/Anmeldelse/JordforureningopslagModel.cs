using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Niras.Jordflytning.ViewModels.Anmeldelse
{
  public class JordforureningopslagModel
  {
    public DateTime Tid { get; set; }
    public bool V2 { get; set; }
    public bool V1 { get; set; }
    public bool OmkAnalysePligt { get; set; }
    public bool OmkLet { get; set; }
    public bool OmkRen { get; set; }
    public string KommunesMiljoeDatabaseTekst { get; set; }
    public bool KommunesMiljoeDatabase { get; set; }
    public Guid JordKlassifikationType { get; set; }
  }
}