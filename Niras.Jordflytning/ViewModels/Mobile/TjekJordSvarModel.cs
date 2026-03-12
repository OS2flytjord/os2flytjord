using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Niras.Jordflytning.Core.Models.JordForurening;

namespace Niras.Jordflytning.ViewModels.Mobile
{
	public class TjekJordSvarModel
	{

		public string AnmelderpligtigJordTekst { get; set; }
		public string ForureningsStatus { get; set; }
		public bool AnmelderpligtigJord { get; set; }
		public string FejlBesked { get; set; }
		public string Forureningskode { get; set; }
    public IList<ForureningsOpslagResult> ForureningOpslagResultatList { get; set; }
    
	}
}