using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Niras.Jordflytning.ViewModels.Mobile
{
	public class MobileAnmeldelseListModel
	{

		public string Status { get; set; }
		public Guid Id { get; set; }
		public string Visningstekst { get; set; }
	}
}