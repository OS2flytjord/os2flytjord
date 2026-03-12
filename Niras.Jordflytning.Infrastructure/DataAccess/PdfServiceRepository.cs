using System;
using System.Configuration;
using System.IO;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
    public class PdfServiceRepository : IPdfServiceRepository
    {
        private static readonly ILogger logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Infrastructure.DataAccess.PdfServiceRepository");
        private readonly string _username;
        private readonly string _apiKey;
        private readonly string _targetPath;

        public PdfServiceRepository()
        {
            _username = ConfigurationManager.AppSettings["pdfCrowdUsername"];
            _apiKey = ConfigurationManager.AppSettings["pdfCrowdApiKey"];
            _targetPath = ConfigurationManager.AppSettings["BlanketBaseFileDir"];
            if (!Directory.Exists(_targetPath))
                Directory.CreateDirectory(_targetPath);
        }

        public bool UrlToPdf(Guid filnavn, string url)
        {
            return UrlToPdf(filnavn.ToString(), url);
        }

        public bool UrlToPdf(string filnavn, string url)
        {
            try
            {
                pdfcrowd.HtmlToPdfClient client = new pdfcrowd.HtmlToPdfClient(_username, _apiKey);
                var destination = Path.Combine(_targetPath, $"{filnavn}.pdf");
                using (var stream = new FileStream(destination, FileMode.Create))
                    client.convertUrlToStream(url, stream);
                return true;
            }
            catch (pdfcrowd.Error ex)
            {
                logger.LogException(ex);
                return false;
            }
        }

        public bool HtmlToPdf(Guid filnavn, string html)
        {
            return HtmlToPdf(filnavn.ToString(), html);
        }

        public bool HtmlToPdf(string filnavn, string html)
        {
            try
            {
                pdfcrowd.HtmlToPdfClient client = new pdfcrowd.HtmlToPdfClient(_username, _apiKey);
                var destination = Path.Combine(_targetPath, $"{filnavn}.pdf");
                using (var stream = new FileStream(destination, FileMode.Create))
                    client.convertStringToStream(html, stream);
                return true;
            }
            catch (pdfcrowd.Error ex)
            {
                logger.LogException(ex);
                return false;
            }
        }

    }
}