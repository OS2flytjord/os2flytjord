using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.ViewModels.GridBindingModels
{
	public class LastbilModel
	{		
		
		[ScaffoldColumn(false)]
		public Guid Id { get; set; }

		[ScaffoldColumn(false)]
		public Guid TransportoerId { get; set; }

		[UIHint("GridForeignKey")]
		[Display(Name = "Miljøklasse")]
		public Guid MiljoeklasseTypeId { get; set; }

		public string Fabrikat { get; set; }
		public string Nummerplade { get; set; }
		public string Bemærkning { get; set; }
		public bool Aktiv { get; set; }

		public virtual ICollection<Vognlaes> Vognlaes { get; set; }
		
		public LastbilModel()
		{
			Vognlaes = new HashSet<Vognlaes>();
		}

		public LastbilModel(Lastbil lastbil)
		{
			Vognlaes = new HashSet<Vognlaes>();
			Id = lastbil.Id;
			TransportoerId = lastbil.TransportoerId;
			MiljoeklasseTypeId = lastbil.MiljoeklasseTypeId;
			Fabrikat = lastbil.Fabrikat;
			Bemærkning = lastbil.Bemærkning;
			Nummerplade = lastbil.Nummerplade;
			Aktiv = lastbil.Aktiv;
		}

		public Lastbil ToLastbil()
		{
			var res = new Lastbil();
			res.Vognlaes = Vognlaes;
			res.TransportoerId = TransportoerId;
			res.Nummerplade = Nummerplade;
			res.MiljoeklasseTypeId = MiljoeklasseTypeId;
			res.Id = Id;
			res.Fabrikat = Fabrikat;
			res.Bemærkning = Bemærkning;
			res.Aktiv = Aktiv;
			return res;
		}
	}
}