using System;
using System.Collections.Generic;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{

  public enum EnumStatusStikproeve
  {
    Planlagt = 1,
    OprettetAdHoc = 2,
    ModtagetHosModtageranlaeg =3,
    ProevetagerRekvireret=4,
    ProeveUdtaget=5,
    TildeltLaboratoriet=6,
    AnalyseUdfoert=7,
    AnalyseGodkendt=8,
    AnalyseAfvistPgaForureningskomponenter=9,
    AnalyseAfvistPgaAffald = 10,
    BaasToemt=11,
		UnderBehandling = 12,
  }

  public interface IStatusStikproeveBusiness
  {
		StatusStikproeve CreateStatus(EnumStatusStikproeve type, Person udfoertAfPerson);    
    bool IsAnalyseForetaget(ICollection<StatusStikproeve> statusStikproever);
		StatusStikproeve GodkendtAfvistUnderbehandling(IList<StatusStikproeve> statusStikproever);
		StatusStikproeve GetLastStatus(ICollection<StatusStikproeve> statusAnmeldelses);
	  StatusStikproeveType GetStatusType(Guid? stikproeveStatusGuid);
	  StatusStikproeve GetFirstStatus(ICollection<StatusStikproeve> statusStikproeve);
  }


}
