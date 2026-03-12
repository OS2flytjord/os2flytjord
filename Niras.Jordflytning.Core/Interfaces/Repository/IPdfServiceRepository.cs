using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niras.Jordflytning.Core.Interfaces.Repository
{
  public interface IPdfServiceRepository
  {
    /// <summary>
    /// Interface til DKPlan Pdf service
    /// Opsætning i config indeholder: 
    /// - Bruger opsat på PDF serveren hvis konfiguration bestemmer hvor filen skal placeres.
    /// - Password til PDF service
    /// - Url til PDF webservice
    /// </summary>
    /// <param name="filnavn">Filnavnet</param>
    /// <param name="url">Url på webside som skal laves til PDFDok.</param>
    bool UrlToPdfAsync(Guid filnavn, string url);
  }
}
