using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Core.BusinessLogic
{
  public class DokumentationBusiness : GenericBusiness<Dokumentation>, IDokumentationBusiness
  {
    private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.DokumentationBusiness");

    /// <summary>
    /// Hvor gamle filer i en persons temp-mappe skal være, før de ryddes op.
    /// Dokumentlisten ligger i Session, så filer ældre end dette kan ikke længere høre til en åben anmeldelse.
    /// </summary>
    private static readonly TimeSpan TempFilMaksAlder = TimeSpan.FromDays(2);

    private readonly IDokumentationRepository _dokumentationRepository;
    private readonly IKodelisteBusiness _kodelisteBusiness;

    public DokumentationBusiness(IDokumentationRepository repository, IKodelisteBusiness kodelisteBusiness, IUnitOfWork unitOfWork)
      : base(repository, unitOfWork)
    {
      _dokumentationRepository = repository;
      _kodelisteBusiness = kodelisteBusiness;
    }

    #region *** Public Methods ***

    public bool CopyDokumentationToTempFolder(Guid personId, Guid anmeldelsesId)
    {
      var appSettings = ConfigurationManager.AppSettings;
      var targetBaseDir = appSettings["TempBaseFileDir"];
      var sourceBaseDir = appSettings["DokumentationBaseFileDir"];
      if (!Directory.Exists(Path.Combine(sourceBaseDir, anmeldelsesId.ToString())))
        return false;

      var dokumentationsFiles = Directory.GetFiles(Path.Combine(sourceBaseDir, anmeldelsesId.ToString()), "*.*", SearchOption.AllDirectories).ToList();

      var lSDirectoryName = Path.Combine(targetBaseDir, personId.ToString());
      var lDDirInfo = new DirectoryInfo(lSDirectoryName);
      if (lDDirInfo.Exists == false)
        Directory.CreateDirectory(lSDirectoryName);

      foreach (var file in dokumentationsFiles)
      {
        var mFile = new FileInfo(file);
        var target = Path.Combine(lSDirectoryName, mFile.Name);
        //Overskriv evt. gammel fil med samme navn, så revisionen får den rigtige fil med.
        mFile.CopyTo(target, true);
        File.SetCreationTimeUtc(target, DateTime.UtcNow);
      }

      return false;
    }

    public bool CopyDokumentationFromRevAnmeldelseToOprindeligAnmeldelse(Guid revAnmeldelseId, Guid oprindAnmeldelseId)
    {
      var appSettings = ConfigurationManager.AppSettings;
      var dokumentationBaseFileDir = appSettings["DokumentationBaseFileDir"];

      if (!Directory.Exists(Path.Combine(dokumentationBaseFileDir, revAnmeldelseId.ToString())))
        Directory.CreateDirectory(Path.Combine(dokumentationBaseFileDir, revAnmeldelseId.ToString()));

      if (!Directory.Exists(Path.Combine(dokumentationBaseFileDir, oprindAnmeldelseId.ToString())))
        Directory.CreateDirectory(Path.Combine(dokumentationBaseFileDir, oprindAnmeldelseId.ToString()));
      

      var destinationDir = Path.Combine(dokumentationBaseFileDir, oprindAnmeldelseId.ToString());
      var sourceDir = Path.Combine(dokumentationBaseFileDir, revAnmeldelseId.ToString());

      var revDokumentationsFiles = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories).ToList();
      foreach (var file in revDokumentationsFiles)
      {
        var mFile = new FileInfo(file);
        if (new FileInfo(Path.Combine(destinationDir, mFile.Name)).Exists == false) //to remove name collusion
        {
          mFile.CopyTo(Path.Combine(destinationDir, mFile.Name));
        }
      }
      return true;
    }

    /// <summary>
    /// Flytter de filer, som anmeldelsen refererer til, fra personens temp-mappe til anmeldelsens mappe.
    /// Kun filer i <paramref name="filnavne"/> flyttes. Andre filer i temp-mappen kan høre til en anden
    /// anmeldelse, som brugeren arbejder på samtidig (fx i en anden fane), og skal blive liggende.
    /// </summary>
    public bool MoveTempDokumentationToAnmeldelseFolder(Guid personId, Guid anmeldelsesId, IEnumerable<string> filnavne)
    {
      var appSettings = ConfigurationManager.AppSettings;
      var sourceDir = Path.Combine(appSettings["TempBaseFileDir"], personId.ToString());
      var targetDir = Path.Combine(appSettings["DokumentationBaseFileDir"], anmeldelsesId.ToString());

      if (!Directory.Exists(sourceDir) || filnavne == null)
        return false;

      var navne = filnavne.Where(f => !string.IsNullOrEmpty(f)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
      if (!navne.Any())
        return false;

      if (!Directory.Exists(targetDir))
        Directory.CreateDirectory(targetDir);

      foreach (var filnavn in navne)
      {
        var source = Path.Combine(sourceDir, Path.GetFileName(filnavn));
        var target = Path.Combine(targetDir, Path.GetFileName(filnavn));

        if (!File.Exists(source))
        {
          if (!File.Exists(target))
            Logger.LogWarning(string.Format("Dokument '{0}' til anmeldelse {1} findes hverken i temp-mappen for person {2} eller i anmeldelsens mappe.", filnavn, anmeldelsesId, personId));
          continue;
        }

        if (File.Exists(target)) //Filen er allerede flyttet
          continue;

        File.Move(source, target);
      }

      return true;
    }

    public bool SaveTempDokument(string baseDir, string personId, HttpPostedFileBase file)
    {
      var personDir = Path.Combine(baseDir, personId);
      if (!Directory.Exists(personDir))
        Directory.CreateDirectory(personDir);

      // Some browsers send file names with full path. This needs to be stripped.
      var fileName = Path.GetFileName(file.FileName);
      var physicalPath = Path.Combine(personDir, fileName);

      file.SaveAs(physicalPath);

      return File.Exists(physicalPath);
    }

    public bool RemoveDokumentation(Guid personId, Guid anmeldelseId, string filNavn, Guid dokumentationId)
    {
      var appSettings = ConfigurationManager.AppSettings;
      var personBaseDir = appSettings["TempBaseFileDir"];
      var anmeldelseBaseDir = appSettings["DokumentationBaseFileDir"];

      //Prøver at hente fil fra anmeldelse folder
      var anmeldelseFilePath = Path.Combine(anmeldelseBaseDir, anmeldelseId.ToString(), filNavn);
      if (File.Exists(anmeldelseFilePath))
      {
        //Slet fra fildrev
        File.Delete(anmeldelseFilePath);

        //Slet fra database
        _dokumentationRepository.Delete(_dokumentationRepository.Read(dokumentationId));
        _dokumentationRepository.Commit(); //_uow.Commit();


        var fi2 = new FileInfo(anmeldelseFilePath);
        return !fi2.Exists;
      }
      //Prøver så fra person folder. Her ligger filerne, indtil anmeldelsen bliver gemt.
      var tempFilePath = Path.Combine(personBaseDir, personId.ToString(), filNavn);
      if (File.Exists(tempFilePath))
      {
        File.Delete(tempFilePath);

        var fi2 = new FileInfo(tempFilePath);
        return !fi2.Exists;

        //Hvis filen ligger her er den ikke registreret i databasen.

      }

      return false;

    }

    public Dokumentation CreateTempDokument(string personId, string anmeldId, HttpPostedFileBase file, Guid dokumentationTypeId, DateTime metadataDato)
    {
      var appSettings = ConfigurationManager.AppSettings;

      var tempDir = Path.Combine(appSettings["TempBaseFileDir"], personId);
      var anmeldDir = Path.Combine(appSettings["DokumentationBaseFileDir"], anmeldId);

      if (!Directory.Exists(tempDir))
        Directory.CreateDirectory(tempDir);

      var dokument = new Dokumentation();

      // Some browsers send file names with full path. This needs to be stripped.

      var fileName = Path.GetFileName(file.FileName);
      if (fileName != null)
      {
        fileName = GetNewFileNameForDuplicates(fileName, tempDir, anmeldDir);
        var physicalPath = Path.Combine(tempDir, fileName);
        file.SaveAs(physicalPath);

        dokument.Id = Guid.NewGuid();
        dokument.Filnavn = Path.GetFileName(physicalPath);
        dokument.DokumentationType = _kodelisteBusiness.ReadDokumentationType(dokumentationTypeId);
        dokument.OprindelsesDato = metadataDato;
      }
      return dokument;
    }

    /// <summary>
    /// Rydder op i personens temp-mappe. Kun filer ældre end <see cref="TempFilMaksAlder"/> slettes.
    /// Mappen må ikke tømmes helt, da brugeren kan have en anden anmeldelse åben (fx i en anden fane),
    /// hvis uploadede filer ligger her, indtil anmeldelsen gemmes.
    /// </summary>
    public bool CleanupTempDirectory(string personId)
    {
      var appSettings = ConfigurationManager.AppSettings;
      var sourcePersonpath = Path.Combine(appSettings["TempBaseFileDir"], personId);
      if (!Directory.Exists(sourcePersonpath))
        return false;

      var graense = DateTime.UtcNow - TempFilMaksAlder;
      foreach (var file in Directory.GetFiles(sourcePersonpath, "*.*", SearchOption.AllDirectories))
      {
        try
        {
          var fi = new FileInfo(file);
          var senest = fi.CreationTimeUtc > fi.LastWriteTimeUtc ? fi.CreationTimeUtc : fi.LastWriteTimeUtc;
          if (senest < graense)
            fi.Delete();
        }
        catch (IOException ex)
        {
          Logger.LogException("Kunne ikke rydde op i temp-fil " + file, ex);
        }
        catch (UnauthorizedAccessException ex)
        {
          Logger.LogException("Kunne ikke rydde op i temp-fil " + file, ex);
        }
      }
      return true;
    }

    public FileStream ReadDokumentation(Guid personId, Guid anmeldelseId, string filNavn)
    {
      var appSettings = ConfigurationManager.AppSettings;

      //Prøver at hente fil fra anmeldelse folder
      var anmeldelseBaseDir = appSettings["DokumentationBaseFileDir"];
      var anmeldelseFilePath = Path.Combine(anmeldelseBaseDir, anmeldelseId.ToString(), filNavn);
      if (File.Exists(anmeldelseFilePath))
      {
        return new FileStream(anmeldelseFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
      }

      //Prøver så fra person folder. Her ligger filerne, indtil anmeldelsen bliver gemt.
      var personBaseDir = appSettings["TempBaseFileDir"];
      var tempFilePath = Path.Combine(personBaseDir, personId.ToString(), filNavn);
      if (File.Exists(tempFilePath))
      {
        return new FileStream(tempFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
      }

      return null;
    }

    #endregion *** Public Methods ***

    #region *** Priavte Methods ***

    /// <summary>
    /// Generates a new filename for duplicate filenames.
    /// and for filenames over 100 char in length
    /// </summary>
    public string GetNewFileNameForDuplicates(string fileName, string baseDir, string anmeldDir)
    {
      const int fileNameCompleteMaxLength = 100;

      if (!String.IsNullOrEmpty(fileName))
      {
        var filenameExclExt = Path.GetFileNameWithoutExtension(fileName);
        var extension = Path.GetExtension(fileName);
        var timeStamp = "";

        // first handle length
        filenameExclExt = ShortenFileName(fileName, fileNameCompleteMaxLength, extension, filenameExclExt, timeStamp);

        // Secondly handle dublicates in both temp and permanent filenames

        var newFullPathTemp = Path.Combine(baseDir, filenameExclExt + extension);
        var anmeldFullPath = Path.Combine(anmeldDir, filenameExclExt + extension);
        var count = 0;

        var fileExists = false;
        while (File.Exists(newFullPathTemp) || File.Exists(anmeldFullPath))
        {
          fileExists = true;
          timeStamp = string.Format("({0}-{1}-{2} {3}-{4}-{5})",
            DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
            DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

          filenameExclExt = ShortenFileName(fileName, fileNameCompleteMaxLength - 20, extension, filenameExclExt, timeStamp);
          newFullPathTemp = Path.Combine(baseDir, filenameExclExt + extension);
          anmeldFullPath = Path.Combine(anmeldDir, filenameExclExt + extension);

          count++;
          if (count > 50)
          {
            fileExists = false;
            timeStamp = "XXXX";
            break;
          }
        }
        if (fileExists)
          timeStamp = "";

        fileName = string.Format("{0}{1}{2}", filenameExclExt, timeStamp, extension);
      }
      return fileName;
    }

    private static string ShortenFileName(string fileName, int fileNameCompleteMaxLength, string extension, string filenameExclExt, string timestamp)
    {
      if (fileName.Length > fileNameCompleteMaxLength)
      {
        var extLength = 0;
        if (!String.IsNullOrEmpty(extension))
          extLength = extension.Length;

        if (!String.IsNullOrEmpty(filenameExclExt))
        {
          var newFileNameLength = fileNameCompleteMaxLength - extLength;
          filenameExclExt = filenameExclExt.Substring(0, newFileNameLength);
        }
      }
      if (!String.IsNullOrEmpty(timestamp))
      {
        filenameExclExt = filenameExclExt + timestamp;
      }
      return filenameExclExt;
    }

    #endregion *** Priavte Methods ***

  }
}
