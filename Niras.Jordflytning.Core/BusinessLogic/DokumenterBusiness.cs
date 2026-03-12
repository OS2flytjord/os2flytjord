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
  public class DokumenterBusiness : GenericBusiness<Dokumenter>, IDokumenterBusiness
  {
	  private readonly IDokumenterRepository _dokumenterRepository;
		private const string DokumentBaseDir = "DokumenterBaseFileDir";
		private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.DokumenterBusiness");

    public DokumenterBusiness(IDokumenterRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
    {
	    _dokumenterRepository = repository;
    }

	  public List<Dokumenter> GetList(Guid modtagerAnlaegGuid)
	  {
		  //var baseList = _dokumenterRepository.Read().Where(a => a.ModtagerAnlaegId == modtagerAnlaegGuid);
      var baseList = _dokumenterRepository.Search(a => a.ModtagerAnlaegId == modtagerAnlaegGuid);
		  var list = baseList.ToList();
		  return list;
	  }

		public FileStream ReadDokumentation(Guid modtageAnlaegGuid, string fileName)
		{
			if (modtageAnlaegGuid==Guid.Empty)
				return null;

			var appSettings = ConfigurationManager.AppSettings;
			var baseDir = appSettings[DokumentBaseDir];
			baseDir = Path.Combine(baseDir, modtageAnlaegGuid.ToString());
			var filePath = Path.Combine(baseDir, fileName);

			//Prøver at hente fil fra folder
			if (File.Exists(filePath))
				return new FileStream(filePath, FileMode.Open);
			
			return null;
		}

	  public bool SaveFile(Guid modtageAnlaegGuid, HttpPostedFileBase file)
	  {
		  var isOk = false;
		  try
		  {
			  var appSettings = ConfigurationManager.AppSettings;
			  var baseDir = appSettings[DokumentBaseDir];

			  baseDir = Path.Combine(baseDir, modtageAnlaegGuid.ToString());
			  var folderExists = Directory.Exists(baseDir);
			  if (!folderExists)
				  Directory.CreateDirectory(baseDir);

			  var fileName = Path.GetFileName(file.FileName);

			  if (!String.IsNullOrEmpty(fileName))
			  {
				  fileName = GetNewFileNameForDuplicates(fileName, baseDir);
				  var physicalPath = Path.Combine(baseDir, fileName);
				  file.SaveAs(physicalPath);

				  var dokument = new Dokumenter();
				  dokument.Filnavn = fileName;
				  dokument.Sti = baseDir;
				  dokument.ModtagerAnlaegId = modtageAnlaegGuid;

				  Create(dokument);

				  isOk = true;
			  }
		  }
		  catch (Exception exception)
		  {
				Logger.LogException("Fejl i SaveFile().", exception);
			  isOk = false;
		  }
			return isOk;
	  }

	  public string GetNewFileNameForDuplicates(string fileName, string baseDir)
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
				var count = 0;

				var fileExists = false;
				while (File.Exists(newFullPathTemp))
				{
					fileExists = true;
					timeStamp = string.Format("({0}-{1}-{2} {3}-{4}-{5})",
						DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
						DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

					filenameExclExt = ShortenFileName(fileName, fileNameCompleteMaxLength - 20, extension, filenameExclExt, timeStamp);
					newFullPathTemp = Path.Combine(baseDir, filenameExclExt + extension);

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
  }
} 
