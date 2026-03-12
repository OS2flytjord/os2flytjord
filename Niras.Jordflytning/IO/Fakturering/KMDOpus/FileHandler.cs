using System;
using System.Collections.Generic;
using System.IO;

namespace Niras.Jordflytning.IO.Fakturering.KMDOpus
{
    public class FileHandler
    {
        protected const string CURRENT_FORMAT_FOLDER_NAME = "KMDOpus";
        protected const string FILE_EXT = ".csv";

        public string ContentType => "text/csv";

        protected readonly Guid _kommuneId;
        protected readonly FileSystemHelper _fileSystemHelper;

        public FileHandler(Guid kommuneId)
        {
            _kommuneId = kommuneId;
            _fileSystemHelper = new FileSystemHelper(CURRENT_FORMAT_FOLDER_NAME, FILE_EXT, _kommuneId);
        }

        public IEnumerable<FileInfo> GetFiles()
        {
            return _fileSystemHelper.GetFiles();
        }

        public FileInfo GetFile(Guid fileId)
        {
            return _fileSystemHelper.GetFile(fileId);
        }

    }
}