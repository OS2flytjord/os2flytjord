using System;
using System.Collections.Generic;
using System.Data.Spatial;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.Interfaces.Business
{
	public interface IAnmeldelserBusiness : IGenericBusiness<Anmeldelse>
	{
    void GemAnmeldelse(Anmeldelse anmeldelse, Person udfoertAfPerson);
    void GemAnmeldelseOgSetStatus(Anmeldelse anmeldelse, Person udfoertAfPerson, EnumStatusAnmeldelse status);
    ICollection<Dokumentation> HentHistoriskeDokumenter(DbGeometry dbGeometry);
	  void SetAnmeldelseStatus(Guid anmeldelseId, Person udfoertAfPerson, EnumStatusAnmeldelse status);
	  //IEnumerable<Anmeldelse> Read();

	}
}
