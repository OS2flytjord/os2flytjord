using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Niras.Jordflytning.ViewModels.Anmeldelse;

namespace Niras.Jordflytning.ViewModels.Mobile
{
	public class MobileAnmeldelseModel
	{

		public Core.Models.Anmeldelse Anmeldelse { get; set; }
		[Display(Name = "Indeholder jorden affald?")]
		public bool IndeholderJordenAffald { get; set; }

		[Display(Name = "Affaldstype")]
		public string Affaldstype { get; set; }
		[Display(Name = "Andet affald")]
		public string AndetAffald { get; set; }

		public string Forueningskategori { get; set; }

		public IEnumerable<SelectListItem> JordklassifikationTypeListe { get; set; }

		[Display(Name = "Almindelig jordflytning")]
		public bool AlmJordflytning { get; set; }

		[Display(Name = "Betaler")]
		public string HvemBetaler { get; set; }
		public string AndenBetaler { get; set; }


		public bool AnvenderFJ { get; set; }
		public string SelectedTransportoerId { get; set; }
		public string SelectedModtagerAnlaegId { get; set; }
		public string SelectedTransportoerId1 { get; set; }
		public string SelectedModtagerAnlaegId1 { get; set; }
		public string DatoFra { get; set; }
		public string DatoTil { get; set; }

		public IList<TransportoerModel> Transportoere { get; set; }
		public IList<ModtagerAnlaegModel> ModtagerAnlaeg { get; set; }

		public bool kanIndsende { get; set; }
        public bool LockAnmeldelse { get; set; }

	}
}