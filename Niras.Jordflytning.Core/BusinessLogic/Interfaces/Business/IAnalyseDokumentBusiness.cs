using System;
using System.IO;
using System.Web;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
	public interface IAnalyseDokumentBusiness : IGenericBusiness<AnalyseDokument>
	{
		bool SaveAnalyseDokument(string stikproeveId, HttpPostedFileBase file);
		FileStream ReadAnalyseDokument(Guid stikproeveId, string filNavn);
		bool RemoveAnalyseDokument(Guid stikproeveId, string filNavn, Guid analyseDokumentId);
	}
}
