using System.Collections.Generic;

namespace Niras.Jordflytning.ViewModels.Placering
{

    // Denne model bliver aldrig gemt, men kun serialiseret og deserialiseret, i forbindelse med indtegning af område.
    // Dvs. at det rent faktisk kun er selve data property der bruges.
    public class IndtegnetPlaceringModel
    {

        // Integningens indhold
        public PlaceringsdataModel Data { get; set; }

        public string Adresse { get; set; }
        public int? Ejerlav { get; set; }
        public string Ejerlavsnavn { get; set; }
        public string Matrikelnummer { get; set; }
        public string EsrEjensomsnummer { get; set; }

        //Kort
        public string KortApiUrl { get; set; }
        public string KortPageSted { get; set; }
        public string KortPageModtagere { get; set; }
        public string KortSite { get; set; }

        public MapBoundsModel MapBounds { get; set; }

        public IList<string> InitFejl { get; set; }

        // Værdi der sættes serverside, der angiver om data er opdateret.
        // Bruges til at sende besked tilbage til den kaldende part.
        public bool Opdateret { get; set; }
        
        public IndtegnetPlaceringModel()
        {
            Data = new PlaceringsdataModel();
            MapBounds = new MapBoundsModel();
            InitFejl = new List<string>();
        }

    }

    
    
}