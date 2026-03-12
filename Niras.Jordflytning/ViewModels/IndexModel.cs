using System.Collections.Generic;
using Niras.Jordflytning.Core;
using Niras.Jordflytning.Entities;

namespace Niras.Jordflytning.ViewModels
{
	public class IndexModel
	{
		public Menu Menu { get; set; }

		public string CurrentRoles { get; set; }
		public List<string> RoleList { get; set; }
	  public string StikproeveFaneTekst { get; set; }
    public string StikproeveDeepLink { get; set; }
	}
}