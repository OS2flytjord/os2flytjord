using System;
using System.IO;
using System.Web;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Core.BusinessLogic
{
  public class AnalyseDokumentBusiness : GenericBusiness<AnalyseDokument>, IAnalyseDokumentBusiness
  {
    private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.AnalyseDokumentBusiness");
    private readonly IAnalyseDokumentRepository _analyseDokumentRepository;

    public AnalyseDokumentBusiness(IAnalyseDokumentRepository analyseDokumentRepository, IUnitOfWork uow)
      : base(analyseDokumentRepository, uow)
    {
      _analyseDokumentRepository = analyseDokumentRepository;
    }

    public bool SaveAnalyseDokument(string stikproeveId, HttpPostedFileBase file)
    {

      var appSettings = System.Configuration.ConfigurationManager.AppSettings;
      var targetBaseDir = appSettings["AnalyseDokumentBaseFileDir"];


      String l_sDirectoryName = Path.Combine(targetBaseDir, stikproeveId.ToString());
      DirectoryInfo l_dDirInfo = new DirectoryInfo(l_sDirectoryName);
      if (l_dDirInfo.Exists == false)
      {
        Directory.CreateDirectory(l_sDirectoryName);
      }
      bool uniqueFilename = false;
      string uniqueFilename_ = file.FileName;

      while (!uniqueFilename)
      {
        FileInfo mFile = new FileInfo(Path.Combine(l_sDirectoryName, uniqueFilename_));
        if (new FileInfo(l_dDirInfo + "\\" + mFile.Name).Exists == false)
        {
          //mFile.MoveTo(l_dDirInfo + "\\" + mFile.Name);
          uniqueFilename = true;

        }
        else // else part handles giving the file a new name if name conflicts with an existing file.
        {
          string random = Guid.NewGuid().ToString().Split('-')[0];
          uniqueFilename_ = random + "_" + file.FileName;
        }
      }
      // Some browsers send file names with full path. This needs to be stripped.
      //    var fileName = Path.GetFileName(file.FileName);
      var physicalPath = Path.Combine(l_sDirectoryName, uniqueFilename_);

      file.SaveAs(physicalPath);
      if (File.Exists(physicalPath))
      {
        AnalyseDokument ad = new AnalyseDokument();
        ad.Dato = DateTime.Now;
        ad.Filnavn = uniqueFilename_;
        ad.StikproeveId = new Guid(stikproeveId);
        Create(ad);
      }

      return File.Exists(physicalPath);
    }

    public FileStream ReadAnalyseDokument(Guid stikproeveId, string filNavn)
    {
      var appSettings = System.Configuration.ConfigurationManager.AppSettings;
      var personBaseDir = appSettings["TempBaseFileDir"];
      var stikproeveBaseDir = appSettings["AnalyseDokumentBaseFileDir"];

      //Prøver at hente fil fra anmeldelse folder
      var stikproeveFilePath = Path.Combine(stikproeveBaseDir, stikproeveId.ToString(), filNavn);
      if (File.Exists(stikproeveFilePath))
      {
        return new FileStream(stikproeveFilePath, FileMode.Open);
      }
      else
      {
        return null;
      }
    }

    public bool RemoveAnalyseDokument(Guid stikproeveId, string filNavn, Guid analyseDokumentId)
    {
      var appSettings = System.Configuration.ConfigurationManager.AppSettings;

      var stikproeveBaseDir = appSettings["AnalyseDokumentBaseFileDir"];

      //Prøver at hente fil fra anmeldelse folder
      var stikproeveFilePath = Path.Combine(stikproeveBaseDir, stikproeveId.ToString(), filNavn);
      if (File.Exists(stikproeveFilePath))
      {
        //Slet fra fildrev
        File.Delete(stikproeveFilePath);

        //Slet fra database
        _analyseDokumentRepository.Delete(_analyseDokumentRepository.Read(analyseDokumentId));
        SaveChanges();

      }
      FileInfo fi2 = new FileInfo(stikproeveFilePath);
      return !fi2.Exists;

    }
  }

}

