using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.SoegeResultat;

namespace Niras.Jordflytning.Areas.Backend.ViewModels
{
	public class StartSagsbehandlerViewModel
	{
		#region Constructor

		public StartSagsbehandlerViewModel()
		{
		}

		#endregion

		#region Properties

		public List<SoegeResultat> MineAnmeldelserList { get; set; }
		public List<SoegeResultat> KraeverHandlingList { get; set; }

		#endregion

	}
}