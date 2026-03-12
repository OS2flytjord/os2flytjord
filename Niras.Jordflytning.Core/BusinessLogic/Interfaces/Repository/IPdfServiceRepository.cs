using System;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository
{
    public interface IPdfServiceRepository
    {
        bool UrlToPdf(Guid filnavn, string url);
        bool UrlToPdf(string filnavn, string url);

        bool HtmlToPdf(Guid filnavn, string html);
        bool HtmlToPdf(string filnavn, string html);
    }
}
