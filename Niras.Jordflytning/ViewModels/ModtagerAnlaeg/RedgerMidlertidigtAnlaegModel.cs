using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Niras.Jordflytning.Enums;

namespace Niras.Jordflytning.ViewModels.ModtagerAnlaeg
{
    public class RedigerMidlertidigtBrugerAnlaegModel
    {

        [Display(Name = "Navn på stedet")] // Optional
        public string Navn { get; set; }

        [Required(ErrorMessage = "Grundejer skal angives")]
        [Display(Name = "Grundejer")]
        public string Grundejer { get; set; }

        [Required(ErrorMessage = "Modtageradresse skal angives")]
        [Display(Name = "Modtageradresse")]
        public string Adresse { get; set; }

        [Required(ErrorMessage = "Postnummer skal angives")]
        [Display(Name = "Postnummer")]
        public int? Postnummer { get; set; }

        [Required(ErrorMessage = "By skal angives")]
        [Display(Name = "By")]
        public string Postdistrikt { get; set; }

        [Required(ErrorMessage = "Kommune skal angives")]
        [Display(Name = "Kommune")]
        public string Kommunekode { get; set; }
        public SelectList Kommuner { get; set; }
        
        [Required(ErrorMessage = "Kontaktperson skal angives")]
        [Display(Name = "Kontaktperson")]
        public string Kontaktperson { get; set; }

        [Required(ErrorMessage = "Telefonnummer skal angives")]
        [Display(Name = "Telefonnummer")]
        public int? Telefon { get; set; }

        [Display(Name = "E-mail")]
        [EmailAddress]
        public string Email { get; set; } // Optional

        [Display(Name = "CVR nummer")]
        public int? CVR { get; set; } // Optional

        [Display(Name = "Modtager affald")]
        public bool Affald { get; set; }

        [Display(Name = "Bemærkninger")]
        public string Bemaerkninger { get; set; }


        // --------------
        // Hidden values
        // --------------

        public int? Ejerlav { get; set; }
        public string Ejerlavsnavn { get; set; }
        public string Matrikelnummer { get; set; }
        public string EsrEjensomsnummer { get; set; }

        public Guid? ModtagerId { get; set; }
        public Guid? AnlaegId { get; set; }

        [Required(ErrorMessage = "JordKlassifikationTypeId skal angives")] // Debug message
        public Guid JordKlassifikationTypeId { get; set; }

        public MapBoundsModel MapBounds { get; set; }

        //public string MapStedLx { get; set; }
        //public string MapStedLy { get; set; }
        //public string MapStedUx { get; set; }
        //public string MapStedUy { get; set; }

        //public string ModtagerAnlaegWkt { get; set; }

        //Kort
        public string KortApiUrl { get; set; }
        public string KortPageSted { get; set; }
        public string KortPageModtagere { get; set; }
        public string KortSite { get; set; }

        public IList<string> InitFejl { get; set; }

        public int PostAttempts { get; set; }

        public SaveState SaveState { get; set; }

        public RedigerMidlertidigtBrugerAnlaegModel()
        {
            ModtagerId = Guid.Empty;
            AnlaegId = Guid.Empty;
            MapBounds = new MapBoundsModel();
        }

    }
    
}