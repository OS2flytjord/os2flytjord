using System.Collections.Generic;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.SoegeResultat;

namespace Niras.Jordflytning.Areas.Backend.ViewModels
{
	public class StartMiljoemedarbejderViewModel
	{
		#region Constructor

		#endregion

		#region Properties

		public List<SoegeResultatStikproeve> AktuelleStikproeverList { get; set; }
		
		public List<SoegeResultatAccepterJord> AccepterJordList { get; set; }

		#endregion

	}
}