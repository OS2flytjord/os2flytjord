using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.Interfaces.Business
{
	public interface ITransportoerBusiness : IGenericBusiness<Transportoer>
  {
    IList<Transportoer> ReadAktiveTransportoerer();
    //IList<Transportoer> ReadAktiveTransportoerer(int filtreringsmetode);
    IList<Transportoer> ReadTidligereAktiveAnvendteTransportoerer(Guid personid);
    Transportoer ReadTransportoer(Guid personId);
		//void SaveLastbil(Lastbil lastbil);
		//void DeleteLastbil(Lastbil lastbil);
		//Lastbil ReadLastbil(Guid guid);
  }
}
