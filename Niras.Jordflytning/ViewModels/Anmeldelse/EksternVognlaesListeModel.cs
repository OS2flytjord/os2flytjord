using System;
using System.Collections.Generic;

namespace Niras.Jordflytning.ViewModels.Anmeldelse
{
  public class EksternVognlaesListeModel
  {
    public IList<EksternVognlaesModel> VognlaesListe { get; set; }
    public Guid AnmeldelseId { get; set; }
  }
}