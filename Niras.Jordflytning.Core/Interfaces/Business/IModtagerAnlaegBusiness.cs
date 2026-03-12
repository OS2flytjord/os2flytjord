using System;
using System.Collections.Generic;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.Interfaces.Business
{
	public interface IModtagerAnlaegBusiness : IGenericBusiness<ModtagerAnlaeg>
  {
    IList<ModtagerAnlaeg> ReadAktiveModtagerAnlaeg();
    IList<ModtagerAnlaeg> ReadAktiveModtagerAnlaeg(bool anvenderJf, Guid jordKlassifikationType);
    
    
		ModtagerAnlaeg ReadModtagerAnlaeg(Guid id);

    IList<ModtagerAnlaeg> ReadTidligereAktiveAnvendteModtagerAnlaeg(Guid personid,bool anvenderJf,Guid jordKlassifikationType);
    IList<ModtagerAnlaeg> ReadAktiveNaermesteModtagerAnlaeg(System.Data.Spatial.DbGeometry geometry, bool anvenderJf, Guid jordKlassifikationType);

		void RemoveGraenseVaerdi(Guid modtageAnlaegGuid, Guid guid);
		void AddGraenseVaerdi(Guid modtageAnlaegGuid, Graensevaerdier graensevaerdier);
  }
}
