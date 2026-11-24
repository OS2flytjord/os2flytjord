using System.Collections.Generic;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
    /// <summary>
    /// Supportfunktioner til MasterAdmin: skift af en brugers email (login) og aktivering/deaktivering.
    /// </summary>
    public interface IBrugerSupportBusiness
    {
        /// <summary>
        /// Finder bruger ud fra email (BrugerNavn). Returnerer null hvis brugeren ikke findes.
        /// </summary>
        BrugerEmailInfo HentBruger(string email);

        /// <summary>
        /// Skifter email på en bruger. BrugerProfil.BrugerNavn og Person.Email opdateres samtidig,
        /// og ændringen logges i Log/Delta.
        /// </summary>
        /// <param name="fraEmail">Nuværende email (BrugerNavn).</param>
        /// <param name="tilEmail">Ny email.</param>
        /// <param name="opdaterKontaktpersonEmail">Opdater også KontaktpersonEmail på modtageanlæg der har den gamle email.</param>
        /// <param name="udfoertAf">Brugernavn på den der udfører ændringen (til log).</param>
        SupportResultat SkiftEmail(string fraEmail, string tilEmail, bool opdaterKontaktpersonEmail, string udfoertAf);

        /// <summary>
        /// Aktiverer eller deaktiverer en bruger (webpages_Membership.IsConfirmed). Ændringen logges i Log/Delta.
        /// En deaktiveret bruger kan ikke logge ind.
        /// </summary>
        /// <param name="email">Brugerens email (BrugerNavn).</param>
        /// <param name="aktiveret">true = aktivér, false = deaktivér.</param>
        /// <param name="udfoertAf">Brugernavn på den der udfører ændringen (til log).</param>
        SupportResultat SaetAktiveret(string email, bool aktiveret, string udfoertAf);
    }

    public class BrugerEmailInfo
    {
        public int BrugerId { get; set; }
        public string BrugerNavn { get; set; }
        public string PersonEmail { get; set; }
        public string Navn { get; set; }
        public bool? PersonAktiv { get; set; }
        /// <summary>webpages_Membership.IsConfirmed. null hvis brugeren ikke har en membership-række.</summary>
        public bool? Aktiveret { get; set; }
        public bool HarMembership { get; set; }
        public int FejledeLoginforsoeg { get; set; }
        public IList<string> Roller { get; set; }
        public IList<string> ModtageanlaegMedKontaktpersonEmail { get; set; }
    }

    public class SupportResultat
    {
        public bool Succes { get; set; }
        public string Besked { get; set; }
    }
}
