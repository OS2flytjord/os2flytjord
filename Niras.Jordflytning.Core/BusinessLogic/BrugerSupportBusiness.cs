using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Core.BusinessLogic
{
    /// <summary>
    /// Supportfunktioner til MasterAdmin. Erstatter Scripts\Support\User_Change_Email.sql
    /// og manuel opdatering af webpages_Membership.IsConfirmed.
    /// </summary>
    public class BrugerSupportBusiness : IBrugerSupportBusiness
    {
        private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.BrugerSupportBusiness");

        // Person.Email er nvarchar(40) - BrugerProfil.BrugerNavn er længere, så Person.Email er begrænsningen.
        private const int MaksEmailLaengde = 40;
        // Delta.Navn er nvarchar(40)
        private const int MaksDeltaNavnLaengde = 40;

        private readonly IBrugereRepository _brugerRepo;
        private readonly IPersonRepository _personRepo;
        private readonly IModtagerAnlaegRepository _modtagerAnlaegRepo;
        private readonly ILogRepository _logRepo;
        private readonly Iwebpages_MembershipRepository _membershipRepo;

        public BrugerSupportBusiness(
            IBrugereRepository brugerRepo,
            IPersonRepository personRepo,
            IModtagerAnlaegRepository modtagerAnlaegRepo,
            ILogRepository logRepo,
            Iwebpages_MembershipRepository membershipRepo)
        {
            _brugerRepo = brugerRepo;
            _personRepo = personRepo;
            _modtagerAnlaegRepo = modtagerAnlaegRepo;
            _logRepo = logRepo;
            _membershipRepo = membershipRepo;
        }

        public BrugerEmailInfo HentBruger(string email)
        {
            email = Normaliser(email);
            if (string.IsNullOrEmpty(email))
                return null;

            var bruger = _brugerRepo.Search(b => b.BrugerNavn == email).FirstOrDefault();
            if (bruger == null)
                return null;

            var person = _personRepo.Search(p => p.BrugerId == bruger.BrugerId).FirstOrDefault();
            var membership = _membershipRepo.Search(m => m.UserId == bruger.BrugerId).FirstOrDefault();

            return new BrugerEmailInfo
            {
                BrugerId = bruger.BrugerId,
                BrugerNavn = bruger.BrugerNavn,
                PersonEmail = person != null ? person.Email : null,
                Navn = person != null ? (person.Navn + " " + person.Efternavn).Trim() : null,
                PersonAktiv = person != null ? person.Aktiv : (bool?)null,
                HarMembership = membership != null,
                Aktiveret = membership != null ? membership.IsConfirmed : null,
                FejledeLoginforsoeg = membership != null ? membership.PasswordFailuresSinceLastSuccess : 0,
                Roller = bruger.webpages_Roles.Select(r => r.RoleName).OrderBy(r => r).ToList(),
                ModtageanlaegMedKontaktpersonEmail = _modtagerAnlaegRepo
                    .Search(m => m.KontaktpersonEmail == bruger.BrugerNavn)
                    .Select(m => m.Navn)
                    .ToList()
            };
        }

        public SupportResultat SkiftEmail(string fraEmail, string tilEmail, bool opdaterKontaktpersonEmail, string udfoertAf)
        {
            fraEmail = Normaliser(fraEmail);
            tilEmail = Normaliser(tilEmail);

            if (string.IsNullOrEmpty(fraEmail) || string.IsNullOrEmpty(tilEmail))
                return Fejl("Både nuværende og ny email skal udfyldes.");

            if (string.Equals(fraEmail, tilEmail, StringComparison.Ordinal))
                return Fejl("Ny email er den samme som den nuværende.");

            if (tilEmail.Length > MaksEmailLaengde)
                return Fejl(string.Format("Ny email må højst være {0} tegn.", MaksEmailLaengde));

            if (!new EmailAddressAttribute().IsValid(tilEmail))
                return Fejl("Ny email er ikke en gyldig emailadresse.");

            var bruger = _brugerRepo.Search(b => b.BrugerNavn == fraEmail).FirstOrDefault();
            if (bruger == null)
                return Fejl(string.Format("Der findes ingen bruger med email '{0}'.", fraEmail));

            // Databasens collation er case-insensitive, så et skift der kun ændrer store/små bogstaver
            // finder brugeren selv - derfor udelukkes brugeren selv i tjekket.
            var brugerId = bruger.BrugerId;
            if (_brugerRepo.Search(b => b.BrugerNavn == tilEmail && b.BrugerId != brugerId).Any())
                return Fejl(string.Format("Der findes allerede en bruger med email '{0}'.", tilEmail));

            if (_personRepo.Search(p => p.Email == tilEmail && p.BrugerId != brugerId).Any())
                return Fejl(string.Format("Der findes allerede en person med email '{0}'.", tilEmail));

            var person = _personRepo.Search(p => p.BrugerId == brugerId).FirstOrDefault();

            var log = new Log { Dato = DateTime.Now, PersonId = person != null ? person.Id : (Guid?)null };
            log.Delta.Add(NyDelta("BrugerNavn (BrugerId " + brugerId + ")", bruger.BrugerNavn, tilEmail));
            bruger.BrugerNavn = tilEmail;

            if (person != null)
            {
                log.Delta.Add(NyDelta("Person.Email", person.Email, tilEmail));
                person.Email = tilEmail;
            }

            var antalAnlaeg = 0;
            if (opdaterKontaktpersonEmail)
            {
                foreach (var anlaeg in _modtagerAnlaegRepo.Search(m => m.KontaktpersonEmail == fraEmail).ToList())
                {
                    log.Delta.Add(NyDelta("KontaktpersonEmail", anlaeg.Navn + ": " + anlaeg.KontaktpersonEmail, anlaeg.Navn + ": " + tilEmail));
                    anlaeg.KontaktpersonEmail = tilEmail;
                    antalAnlaeg++;
                }
            }

            log.Delta.Add(NyDelta("Email skiftet af", null, udfoertAf));
            _logRepo.Add(log);

            try
            {
                // Alle repositories deler samme DataContext pr. request, så ændringerne gemmes i én SaveChanges (én transaktion).
                _logRepo.Commit();
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return Fejl("Ændringen kunne ikke gemmes: " + (e.InnerException ?? e).Message);
            }

            var besked = string.Format("Email ændret fra '{0}' til '{1}'.", fraEmail, tilEmail);
            if (person == null)
                besked += " Bemærk: brugeren havde ingen tilknyttet person.";
            if (antalAnlaeg > 0)
                besked += string.Format(" KontaktpersonEmail opdateret på {0} modtageanlæg.", antalAnlaeg);

            return new SupportResultat { Succes = true, Besked = besked };
        }

        public SupportResultat SaetAktiveret(string email, bool aktiveret, string udfoertAf)
        {
            email = Normaliser(email);
            if (string.IsNullOrEmpty(email))
                return Fejl("Email skal udfyldes.");

            var bruger = _brugerRepo.Search(b => b.BrugerNavn == email).FirstOrDefault();
            if (bruger == null)
                return Fejl(string.Format("Der findes ingen bruger med email '{0}'.", email));

            var brugerId = bruger.BrugerId;
            var membership = _membershipRepo.Search(m => m.UserId == brugerId).FirstOrDefault();
            if (membership == null)
                return Fejl("Brugeren har ingen login-konto (webpages_Membership) og kan ikke aktiveres.");

            var foer = membership.IsConfirmed == true;
            if (foer == aktiveret)
                return Fejl(aktiveret ? "Brugeren er allerede aktiveret." : "Brugeren er allerede deaktiveret.");

            var person = _personRepo.Search(p => p.BrugerId == brugerId).FirstOrDefault();

            var log = new Log { Dato = DateTime.Now, PersonId = person != null ? person.Id : (Guid?)null };
            log.Delta.Add(NyDelta("IsConfirmed (BrugerId " + brugerId + ")", foer ? "Aktiveret" : "Ikke aktiveret", aktiveret ? "Aktiveret" : "Ikke aktiveret"));
            log.Delta.Add(NyDelta(aktiveret ? "Aktiveret af" : "Deaktiveret af", null, udfoertAf));

            membership.IsConfirmed = aktiveret;
            _logRepo.Add(log);

            try
            {
                _logRepo.Commit();
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return Fejl("Ændringen kunne ikke gemmes: " + (e.InnerException ?? e).Message);
            }

            return new SupportResultat
            {
                Succes = true,
                Besked = string.Format(aktiveret ? "Brugeren '{0}' er aktiveret og kan nu logge ind." : "Brugeren '{0}' er deaktiveret og kan ikke længere logge ind.", bruger.BrugerNavn)
            };
        }

        private static Delta NyDelta(string navn, string foer, string efter)
        {
            if (navn.Length > MaksDeltaNavnLaengde)
                navn = navn.Substring(0, MaksDeltaNavnLaengde);
            return new Delta { Navn = navn, Foer = foer, Efter = efter };
        }

        private static string Normaliser(string email)
        {
            return email == null ? null : email.Trim();
        }

        private static SupportResultat Fejl(string besked)
        {
            return new SupportResultat { Succes = false, Besked = besked };
        }
    }
}
