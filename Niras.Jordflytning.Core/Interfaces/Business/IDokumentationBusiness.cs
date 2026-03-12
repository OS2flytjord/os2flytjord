using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.Interfaces.Business 
{
	public interface IDokumentationBusiness : IGenericBusiness<Dokumentation>
	{
	  bool MoveTempDokumentationToAnmeldelseFolder(Guid PersonId, Guid AnmeldelsesId);
    bool SaveTempDokument(string baseDir, string personId, HttpPostedFileBase file);
    //bool RemoveTempDokument(string baseDir, string personId, string filename);
	  bool RemoveTempDirectory(string baseDir, string personId);
    FileStream ReadDokumentation(Guid personId, Guid anmeldelseId, string filNavn);
    bool RemoveDokumentation(Guid personId,Guid anmeldelseId, string filNavn,Guid dokumentationId);


	}
}
