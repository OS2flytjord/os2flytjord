using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niras.Jordflytning.Core.Models.MatrikelOpslag
{
    public static class DawaClasses
    {

        public class AdresseMini
        {
            public string id { get; set; }
            public int status { get; set; }
            public string vejkode { get; set; }
            public string vejnavn { get; set; }
            public string husnr { get; set; }
            public object etage { get; set; }
            public object dør { get; set; }
            public object supplerendebynavn { get; set; }
            public string postnr { get; set; }
            public string postnrnavn { get; set; }
            public string kommunekode { get; set; }
            public string adgangsadresseid { get; set; }
            public double x { get; set; }
            public double y { get; set; }
        }

        public class EjerlavInfo
        {
            public string tekst { get; set; }
            public Ejerlav ejerlav { get; set; }
        }

        public class Ejerlav
        {
            public int kode { get; set; }
            public string navn { get; set; }
            public string href { get; set; }
        }

        public class Kommune
        {
            public string href { get; set; }
            public string kode { get; set; }
        }

        public class Region
        {
            public string href { get; set; }
            public string kode { get; set; }
        }

        public class Sogn
        {
            public string href { get; set; }
            public string kode { get; set; }
        }

        public class Retskreds
        {
            public string href { get; set; }
            public string kode { get; set; }
        }

        public class Jordstykke
        {
            public string ændret { get; set; }
            public int geo_version { get; set; }
            public string geo_ændret { get; set; }
            public string matrikelnr { get; set; }
            public string href { get; set; }
            public Ejerlav ejerlav { get; set; }
            public Kommune kommune { get; set; }
            public Region region { get; set; }
            public Sogn sogn { get; set; }
            public Retskreds retskreds { get; set; }
            public string esrejendomsnr { get; set; }
            public string sfeejendomsnr { get; set; }
        }

    }
}
