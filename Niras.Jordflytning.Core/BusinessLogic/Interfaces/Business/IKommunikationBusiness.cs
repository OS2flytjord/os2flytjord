using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
  public interface IKommunikationBusiness
  {
    void CreateKommunikation(Kommunikation k);

    void CreateKommunikation(Guid anmeldelseId, Besked besked, string modtagerEmail, string afsenderEmail, string hoerAnsvarligKommuneNr);
  }
}
