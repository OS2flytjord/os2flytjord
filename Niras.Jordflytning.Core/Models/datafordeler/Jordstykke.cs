using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niras.Jordflytning.Core.Models.datafordeler
{
    public class Jordstykke
    {
        public string id_lokalId { get; set; }

        public string ejerlavLokalId { get; set; }

        public Ejerlav ejerlav { get; set; }

        public string matrikelnummer { get; set; }

    }
}
