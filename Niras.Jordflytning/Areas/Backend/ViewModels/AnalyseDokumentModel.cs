using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Areas.Backend.ViewModels
{
	public class AnalyseDokumentModel
	{
		public Guid Id { get; set; }
		public Guid StikproeveId { get; set; }
		public string Filnavn { get; set; }
		public Nullable<DateTime> Dato { get; set; }

		public AnalyseDokumentModel()
		{}

		public AnalyseDokumentModel(AnalyseDokument analyseDokument)
		{
			Id = analyseDokument.Id;
			StikproeveId = analyseDokument.StikproeveId;
			Filnavn = analyseDokument.Filnavn;
			Dato = analyseDokument.Dato;
		}

	}
}