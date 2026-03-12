using System.Collections.Generic;

namespace Niras.Jordflytning.ViewModels.Placering
{
    public class PlaceringsdataModel
    {
        public string Ejerlav { get; set; }
        public string Ejerlavsnavn { get; set; }
        public string Matrikelnummer { get; set; }
        public string Wkt { get; set; }
        public string EsrEjendomsnummer { get; set; }

        // Matrikler der er inkluderet i tegningen
        public IList<MatrikelInfoModel> Matrikler { get; set; }

        public PlaceringsdataModel()
        {
            Matrikler = new List<MatrikelInfoModel>();
        }

    }
}