using System.Data.Spatial;
using Niras.Jordflytning.Core.Models.JordForurening;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository
{
    public interface IKulturarvRepository
	{

        KonfliktSoegningLeverandoerResultat KonfliktSoegningMidlertidigModtager(DbGeometry geom);

	}
}
