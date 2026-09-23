
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niras.Jordflytning.Core.Models.datafordeler
{
    public class NavngivenVej
    {
        public string administreresAfKommune { get; set; }
        public VejnavneBeliggenhed vejnavnebeliggenhed_vejnavnelinje { get; set; }
    }

    public class NavngivenVejPosternummer
    {
        public string navngivenVej { get; set; }
    }

    public class VejnavneBeliggenhed
    {
        public string wkt { get; set; }
    }
}
