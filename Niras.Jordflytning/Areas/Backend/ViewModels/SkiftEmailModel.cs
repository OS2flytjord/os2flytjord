using System.ComponentModel.DataAnnotations;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;

namespace Niras.Jordflytning.Areas.Backend.ViewModels
{
    public class SkiftEmailModel
    {
        [Display(Name = "Nuværende email")]
        public string SoegEmail { get; set; }

        [Required(ErrorMessage = "Skal udfyldes")]
        public string FraEmail { get; set; }

        [Required(ErrorMessage = "Skal udfyldes")]
        [StringLength(40, ErrorMessage = "Email må højst være 40 tegn.")]
        [EmailAddress(ErrorMessage = "Ikke en gyldig emailadresse.")]
        [Display(Name = "Ny email")]
        public string TilEmail { get; set; }

        [Display(Name = "Opdater også kontaktperson-email på modtageanlæg")]
        public bool OpdaterKontaktpersonEmail { get; set; }

        /// <summary>Den fundne bruger, null hvis der ikke er søgt eller brugeren ikke findes.</summary>
        public BrugerEmailInfo Bruger { get; set; }

        public bool ErSoegt { get; set; }
        public string Besked { get; set; }
        public bool BeskedErFejl { get; set; }
    }
}
