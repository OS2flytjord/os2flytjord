using System;
using System.Collections.Generic;
using Niras.Jordflytning.Core.Models.SoegeResultat;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
    public interface ISoegBusiness
    {
        List<SoegeResultat> GetAktiveAnmeldelser(Guid brugerId, bool iskommune);
	    List<SoegeResultat> GetAfsluttedeAnmeldelser(Guid brugerId);
	    List<SoegeResultat> GetFremSendteAnmeldelser(Guid brugerId, bool iskommune);
	    List<SoegeResultat> GetIkkeFremSendteAnmeldelser(Guid brugerId, bool iskommune);

	    List<SoegeResultat> GetAnmeldelserBy(decimal? loebeNr, string adresse, DateTime? foerDate, DateTime? efterDate, Guid? transportoerId, Guid? anmelderId, Guid? modtagerId, Guid brugerId, bool getKunMine, bool inclAfsluttede);
	    
        List<SoegeResultat> GetAnmeldelserBy(decimal? loebeNr, string adresse, DateTime? foerDate,
            DateTime? efterDate, Guid brugerId, bool isKommune, bool inclAfsluttede, Guid? modtagerId,
            Guid? transportoerId, Guid? anmelderId, Guid? andenOprindJordTypeId,
            string andenOprindBeskrivelse, string forureningsKategori);
		
	    List<SoegeResultat> GetMineAnmeldelser(Guid sagsbehandlerId);
	    List<SoegeResultat> GetAnmeldelserDerKraeverHandling(Guid userId);
	    List<SoegeResultat> GetIkkeMeldtAfsluttetAnmeldelser(Guid userId, bool isKommune);
	    List<SoegeResultat> GetIkkeAfsluttedeAnmeldelser(Guid brugerId, bool isKommune);

        List<SoegeResultat> GetAnmelderAlarmAnmeldelser(Guid brugerId);
    }
}
