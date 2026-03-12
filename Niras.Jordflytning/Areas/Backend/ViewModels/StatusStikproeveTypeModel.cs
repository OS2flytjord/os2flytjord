using System;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Areas.Backend.ViewModels
{
	public class StatusStikproeveTypeModel
	{

		public StatusStikproeveTypeModel(StatusStikproeveType statusStikproeveType)
		{
			Id = statusStikproeveType.Id;
			Navn = statusStikproeveType.Navn;
			Sortering = statusStikproeveType.Sortering;
			Aktiv = statusStikproeveType.Aktiv;
			Kode = statusStikproeveType.Kode;
		}

		public Guid Id { get; set; }
		public string Navn { get; set; }
		public decimal? Sortering { get; set; }
		public bool Aktiv { get; set; }
		public short Kode { get; set; }    
	}

}