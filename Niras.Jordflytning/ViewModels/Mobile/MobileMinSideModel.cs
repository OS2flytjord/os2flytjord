using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Niras.Jordflytning.ViewModels.Mobile
{
	public class MobileMinSideModel
	{

		public IList<MobileAnmeldelseListModel> AktiveAnmeldelser { get; set; }
		public IList<MobileAnmeldelseListModel> FremsendteAnmeldelser { get; set; }
		public IList<MobileAnmeldelseListModel> AfsluttedeAnmeldelser { get; set; }
		public IList<MobileAnmeldelseListModel> IkkeFremsendteAnmeldelser { get; set; }

		public Guid ValgtAnmeldelseId { get; set; }

	}
}