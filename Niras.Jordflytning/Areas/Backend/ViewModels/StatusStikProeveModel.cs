using System;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Areas.Backend.ViewModels
{
	public class StatusStikproeveModel
	{

		public Guid Id { get; set; }
		public Guid? StikproeveId { get; set; }
		public Guid? PersonId { get; set; }
		public Guid? StatusStikproeveTypeId { get; set; }
		public DateTime Tid { get; set; }
		public String PersonNavn { get; set; }
		public StatusStikproeveTypeModel StatusStikproeveType { get; set; }		


		public StatusStikproeveModel(StatusStikproeve statusStikproeve)
		{
			Id = statusStikproeve.Id;
			StikproeveId = statusStikproeve.StikproeveId;
			PersonId = statusStikproeve.PersonId;
			StatusStikproeveTypeId = statusStikproeve.StatusStikproeveTypeId;
			Tid = statusStikproeve.Tid;
			StatusStikproeveType = new StatusStikproeveTypeModel(statusStikproeve.StatusStikproeveType);
			if (statusStikproeve.Person != null) 
				PersonNavn = statusStikproeve.Person.Navn + " " + statusStikproeve.Person.Efternavn;
		}

	}
}