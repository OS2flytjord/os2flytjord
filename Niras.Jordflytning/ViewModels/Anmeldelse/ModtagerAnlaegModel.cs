using System;
using System.ComponentModel.DataAnnotations;

namespace Niras.Jordflytning.ViewModels.Anmeldelse
{
    public class ModtagerAnlaegModel
    {
        public Guid Id { get; set; }

        public string Navn { get; set; }

        public string By { get; set; }

        public string Postnummer { get; set; }

        public string Adresse { get; set; }

        public string Ejerlav { get; set; }
        public string Ejerlavsnavn { get; set; }
        public string Matrikelnr { get; set; }
        public string EsrEjendomsnummer { get; set; }

        public int? Afstand { get; set; }

        [Display(Name = @"Forureningskategori")]
        public string Forureningskategori { get; set; }

        public bool Affald { get; set; }

        public string Kommune { get; set; }

        public bool JF { get; set; }

        public bool CentraltOprettetMidlertidigtAnlaeg { get; set; }

        public bool AnmelderOprettetMidlertidigtAnlaeg { get; set; }

        [Display(Name = @"Dok.")]
        public string DocLink { get; set; }

        public string www { get; set; }

        public Guid? JordmodtagerId { get; set; }

        public string KortLink { get; set; }

        public string JordmodtagerNavn { get; set; }

        public string Label { get; set; }

        public string LabelJordModtager { get; set; }

        public string KontaktpersonNavn { get; set; }

        public decimal? KontaktpersonTlf { get; set; }

        public string KontaktpersonEmail { get; set; }

        public string Bemaerkning { get; set; }
    }
}