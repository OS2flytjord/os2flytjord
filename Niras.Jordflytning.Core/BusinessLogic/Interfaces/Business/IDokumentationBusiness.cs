using System;
using System.Collections.Generic;
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

    /// <summary>
    /// Flytter de angivne filer fra personens temp-mappe til anmeldelsens mappe.
    /// </summary>
    bool MoveTempDokumentationToAnmeldelseFolder(Guid personId, Guid anmeldelsesId, IEnumerable<string> filnavne);

		bool SaveTempDokument(string baseDir, string personId, HttpPostedFileBase file);

    /// <summary>
    /// Sletter forældede filer i personens temp-mappe. Filer, der kan høre til en åben anmeldelse, bevares.
    /// </summary>
    bool CleanupTempDirectory(string personId);
    FileStream ReadDokumentation(Guid personId, Guid anmeldelseId, string filNavn);
    bool RemoveDokumentation(Guid personId, Guid anmeldelseId, string filNavn, Guid dokumentationId);


		Dokumentation CreateTempDokument(string personId, string anmeldId, HttpPostedFileBase file, Guid dokumentationTypeId, DateTime metadataDato);
	  string GetNewFileNameForDuplicates(string fileName, string baseDir, string anmeldDir);

    bool CopyDokumentationFromRevAnmeldelseToOprindeligAnmeldelse(Guid revAnmeldelseId, Guid oprindAnmeldelseId);
  }
}
