using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace Niras.Jordflytning.IO.Fakturering
{
    public class FileSystemHelper
    {
        private const string BASE_PATH_KEY = "FaktureringBaseFileDir";

        private readonly DirectoryInfo _mainDirectory;
        private readonly string _fileExtension;

        public FileSystemHelper(string workingDirectory, string fileExtenstion, Guid kommuneId)
        {
            if (string.IsNullOrWhiteSpace(workingDirectory))
                throw new ArgumentNullException(nameof(workingDirectory));

            if (string.IsNullOrWhiteSpace(fileExtenstion))
                throw new ArgumentNullException(nameof(fileExtenstion));

            _fileExtension = fileExtenstion[0] == '.' ? fileExtenstion : $".{fileExtenstion}";

            _mainDirectory = new DirectoryInfo(Path.Combine(ConfigurationManager.AppSettings[BASE_PATH_KEY], workingDirectory, kommuneId.ToString()));
            if (!_mainDirectory.Exists)
                _mainDirectory.Create();
        }

        public IEnumerable<FileInfo> GetFiles()
        {
            return _mainDirectory.Parent.GetFiles($"*{_fileExtension}", SearchOption.AllDirectories)
                .OrderByDescending(x => x.DirectoryName)
                .ThenByDescending(x => x.CreationTimeUtc)
                .ToArray();
        }

        private string ContructFileName(Guid fileId)
        {
            return Path.Combine(_mainDirectory.FullName, $"{fileId}{_fileExtension}");
        }

        public FileInfo CreateFile(Action<StreamWriter> contentWriter, Encoding encoding, Guid fileId)
        {
            var fn = ContructFileName(fileId);
            var file = new FileInfo(fn);
            using (var stream = new StreamWriter(file.FullName, false, encoding))
            {
                contentWriter(stream);
            }                
            return file;
        }

        public FileInfo CreateFile(Action<StreamWriter> contentWriter, Guid fileId)
        {
            return CreateFile(contentWriter, Encoding.UTF8, fileId);
        }

        public FileInfo GetFile(Guid fileId)
        {
            return new FileInfo(Path.Combine(_mainDirectory.FullName, $"{fileId}{_fileExtension}"));
        }

    }
}