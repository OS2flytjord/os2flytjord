using System;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
    public interface IPdfBusiness
    {
        void CreateAnmeldelseBlanket(Guid anmeldelseId);
        void CreateHistorikKommunikationPdf(Guid anmeldelseId);
    }
}
