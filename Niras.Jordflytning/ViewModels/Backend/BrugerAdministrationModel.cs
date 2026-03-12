using System.Collections.Generic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using Niras.Jordflytning.Infrastructure.Common;
using Niras.Jordflytning.ViewModels.GridBindingModels;

namespace Niras.Jordflytning.ViewModels.Backend
{
	public class BrugerAdministrationModel
	{
		
		public BrugerAdministrationModel()
		{
			ISecurityProvider securityProvider = new WebSecurityProvider();
		    VisKommuneRoller = securityProvider.CurrentUser.IsInRole("KommuneAdmin");
		    VisJordmodtagerRoller = securityProvider.CurrentUser.IsInRole("JordmodtagerAdmin");
		}

		public IList<BrugerProfilGridModel> Brugere { get; set; }
		public bool VisUnderskrift { get; set; }
		public bool VisKommuneRoller { get; set; }
		public bool VisJordmodtagerRoller { get; set; }
		public string Brugersoegning { get; set; }

	}
}