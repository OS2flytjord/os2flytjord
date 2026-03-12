using System;
using System.IO;
using System.Web;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
  public enum EnumDokumentationType
  {
    ØvrigDokumentation = 1,
    Situationsplan = 2,
    Forureningsundersøgelse = 3,
    AnalyserapportPdf = 4,
    AnmeldelseAndenKommune = 5,
    HistoriskRedegørelse = 6,
    AnalyserapportExcel = 7,
    Vejesedler=8,
    GeotekniskUndersøgelse=9,
    Jordhåndteringsplan=10
  }

  public interface IDokumentationBusiness : IGenericBusiness<Dokumentation>
  {
    /// <summary>
    /// Laves der en revision af anmeldelsen skal dokumentation flyttes over på anmeldelsen som revideres.
    /// I første omgang flyttes den til temp mappe med brugerens id. Ved efterfølgende gem flyttes 
    /// den til en mappe navn som anmeldelsesid'et
    /// </summary>
    bool CopyDokumentationToTempFolder(Guid personId, Guid anmeldelsesId);

    bool MoveTempDokumentationToAnmeldelseFolder(Guid personId, Guid anmeldelsesId);
    
		bool SaveTempDokument(string baseDir, string personId, HttpPostedFileBase file);

    bool RemoveTempDirectory(string personId);
    FileStream ReadDokumentation(Guid personId, Guid anmeldelseId, string filNavn);
    bool RemoveDokumentation(Guid personId, Guid anmeldelseId, string filNavn, Guid dokumentationId);


		Dokumentation CreateTempDokument(string personId, string anmeldId, HttpPostedFileBase file, Guid dokumentationTypeId, DateTime metadataDato);
	  string GetNewFileNameForDuplicates(string fileName, string baseDir, string anmeldDir);

    bool CopyDokumentationFromRevAnmeldelseToOprindeligAnmeldelse(Guid revAnmeldelseId, Guid oprindAnmeldelseId);
  }
}
