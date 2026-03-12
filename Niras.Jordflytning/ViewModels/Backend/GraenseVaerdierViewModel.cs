using System;

namespace Niras.Jordflytning.ViewModels.Backend
{
    public class GraenseVaerdierViewModel
    {
        public Guid Id { get; set; }
        public string Navn { get; set; }
        public decimal? Max { get; set; }
        public string Kode { get; set; }
	    public string EnhedNavn { get; set; }
    }
}