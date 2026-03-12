using System.Collections.Generic;
using Niras.Jordflytning.Core.Models.SoegeResultat;

namespace Niras.Jordflytning.Areas.Backend.ViewModels
{
	public class StartBogholderViewModel
	{
		public List<SoegeResultatBetaler> KraeverHandlingList { get; set; }
		public List<SoegeResultatOpslagsTavle> OpslagstavleList { get; set; }
	}
}