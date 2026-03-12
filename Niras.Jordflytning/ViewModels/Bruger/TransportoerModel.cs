using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.ViewModels.GridBindingModels;

namespace Niras.Jordflytning.ViewModels.Bruger
{
	public class TransportoerModel
	{
		#region properties

		[Display(Name = "Transportør")]
		public bool ErTransportoer { get; set; }
		public Guid Id { get; set; }
		public String Navn { get; set; }
		public List<LastbilModel> MineLastbiler { get; set; }
		//public IEnumerable<SelectListItem> Miljoeklasser { get; set; }
		public String SelectedMiljoeKlasse { get; set; }

		#endregion

		#region constructors

		public TransportoerModel(Transportoer transportoer)
		{
			Navn = transportoer.Person.Navn + " " + transportoer.Person.Efternavn;
			Id = transportoer.Id;
			MineLastbiler = transportoer.Lastbil.Select(x => new LastbilModel(x)).ToList();
		}


		public TransportoerModel()
		{
			//MineLastbiler = new List<LastbilModel>();
			//SelectedMiljoeKlasse = "0";

			//var miljoeKlasser = new List<SelectListItem>();
			//miljoeKlasser.Add(new SelectListItem { Text = "Klasse 1", Value = "0", Selected = true });
			//miljoeKlasser.Add(new SelectListItem { Text = "Klasse 2", Value = "1" });
			//miljoeKlasser.Add(new SelectListItem { Text = "Klasse 3", Value = "2" });
			//miljoeKlasser.Add(new SelectListItem { Text = "Klasse 4", Value = "3" });

			//IEnumerable<SelectListItem> selectList =
			//					from m in miljoeKlasser
			//					select new SelectListItem
			//										 {
			//												 Selected = (m.Value == SelectedMiljoeKlasse),
			//												 Text = m.Text,
			//												 Value = m.Value
			//										 };

			//Miljoeklasser = selectList;
		}

		#endregion
	}
}
