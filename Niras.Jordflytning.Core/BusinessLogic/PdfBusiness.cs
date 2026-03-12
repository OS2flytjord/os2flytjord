using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using System;
using System.Net;
using System.Web;

namespace Niras.Jordflytning.Core.BusinessLogic
{
    public class PdfBusiness : IPdfBusiness
    {
        private readonly IPdfServiceRepository _pdfServiceRepository;

        public PdfBusiness(IPdfServiceRepository pdfServiceRepository)
        {
            _pdfServiceRepository = pdfServiceRepository;
        }

        private string UrlToHtml(string url)
        {
            try
            {
                using (var client = new WebClient())
                {
                    // Todays Chrome: (2024-09-04)
                    client.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/128.0.0.0 Safari/537.36";
                    client.Encoding = System.Text.Encoding.UTF8;
                    var result = client.DownloadString(url);
                    return result;
                }
            }
            catch { }
            return string.Empty;
        }

        public void CreateAnmeldelseBlanket(Guid anmeldelseId)
        {
            var domain = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            var url = $"{domain}/anmeldelser/blanket?anmeldelseId={anmeldelseId}&printStyle=1";
            if (domain.IndexOf("localhost") > -1)
            {
                var html = UrlToHtml(url);
                _pdfServiceRepository.HtmlToPdf(anmeldelseId, html);
            }
            else
                _pdfServiceRepository.UrlToPdf(anmeldelseId, url);
        }

        public void CreateHistorikKommunikationPdf(Guid anmeldelseId)
        {
            var domain = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            var url = $"{domain}/anmeldelser/HistorikKommunikation?anmeldelseId={anmeldelseId}";
            if (domain.IndexOf("localhost") > -1)
            {
                var html = UrlToHtml(url);
                _pdfServiceRepository.HtmlToPdf($"{anmeldelseId.ToString()}_hk", html);
            }
            else
                _pdfServiceRepository.UrlToPdf($"{anmeldelseId.ToString()}_hk", url);
        }
    }
}
