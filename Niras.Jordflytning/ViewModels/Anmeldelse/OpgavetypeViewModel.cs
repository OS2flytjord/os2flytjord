using Niras.Jordflytning.Core.Models;
using System;

namespace Niras.Jordflytning.ViewModels.Anmeldelse
{
    public class OpgavetypeViewModel
    {
        public Guid Id { get; set; }
        public Guid KommuneId { get; set; }
        public int Kode { get; set; }
        public string Navn { get; set; }
        public decimal Timepris { get; set; }
        public DateTime? FraDato { get; set; }
        public DateTime? TilDato { get; set; }
        public int Afrunding { get; set; }
        public int Sortering { get; set; }

        public bool KraeverTidsregistrering => !(!Id.Equals(Guid.Empty) && Kode == 0);

        public static OpgavetypeViewModel Create(Opgavetype opgavetype)
        {
            return new OpgavetypeViewModel
            {
                Id = opgavetype.Id,
                KommuneId = opgavetype.Kommune.Id,
                Kode = opgavetype.Kode,
                Navn = opgavetype.Navn,
                Timepris = opgavetype.Timepris,
                FraDato = opgavetype.FraDato,
                TilDato = opgavetype.TilDato,
                Afrunding = opgavetype.Afrunding,
                Sortering = opgavetype.Sortering
            };
        }
    }
}