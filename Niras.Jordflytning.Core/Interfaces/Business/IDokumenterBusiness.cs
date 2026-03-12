using System;
using System.Collections.Generic;
using System.IO;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.Interfaces.Business 
{
	public interface IDokumenterBusiness : IGenericBusiness<Dokumenter>
	{
		List<Dokumenter> GetList(Guid modtagerAnlaegGuid);
		FileStream ReadDokumentation(Guid modtageAnlaegGuid, string fileName);
		void Delete(Guid dokId);
	}
}
