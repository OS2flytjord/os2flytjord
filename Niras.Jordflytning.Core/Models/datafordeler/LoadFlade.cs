using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niras.Jordflytning.Core.Models.datafordeler
{
    public class LoadFlade
    {
        public string id_lokalId { get; set; }  

        public string jordstykkeLokalId { get; set; }

        public DatafordelerGeometry geometri { get; set; }
    }
}
