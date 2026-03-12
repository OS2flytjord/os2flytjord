using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business 
{
	public interface IDokumenterBusiness : IGenericBusiness<Dokumenter>
	{
		List<Dokumenter> GetList(Guid modtagerAnlaegGuid);
		FileStream ReadDokumentation(Guid modtageAnlaegGuid, string fileName);
		bool SaveFile(Guid modtageAnlaegGuid, HttpPostedFileBase file);
	}
}
