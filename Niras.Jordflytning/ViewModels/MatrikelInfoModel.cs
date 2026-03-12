using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Niras.Jordflytning.ViewModels
{
    public class MatrikelInfoModel
    {

        public string Ejerlav { get; set; }
        public string Ejerlavsnavn { get; set; }
        public string Matrikelnummer { get; set; }
        public string Wkt { get; set; }
        public string EsrEjendomsnummer { get; set; }

        public MatrikelInfoModel()
        {
        }

        public MatrikelInfoModel(string ejerlav, string ejerlavsnavn, string matrikelnummer, string wkt, string esrEjendomsnummer)
        {
            Ejerlav = ejerlav;
            Ejerlavsnavn = ejerlavsnavn;
            Matrikelnummer = matrikelnummer;
            Wkt = wkt;
            EsrEjendomsnummer = esrEjendomsnummer;
        }

    }
}