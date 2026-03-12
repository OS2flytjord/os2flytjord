using System;
using System.Data.Spatial;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.JordForurening;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
    public interface IForureningsOpslagBusiness
    {
        //ForureningsOpslag GetJordForurening(Oprindelsessted oprindelsessted, Guid kommuneGuid);
        ForureningsOpslag2 Read(Oprindelsessted oprSted, Guid kommuneGuid);
        KonfliktSoegningResultater KonfliktSoegningMidlertidigModtager(DbGeometry geom);
    }
}
