using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using Antlr.StringTemplate;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Core.BusinessLogic
{

    public class AdviseringBusiness : GenericBusiness<Advis>, IAdviseringBusiness
    {
        private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.AdviseringBusiness");

        private readonly IKodelisteBusiness _kodelisteBusiness;
        private readonly IAdvisRepository _advisRepository;
        private readonly IKommunikationBusiness _kommunikationBusiness;
        //private readonly IStatusAnmeldelseBusiness _statusAnmeldelseBusiness;
        private readonly IStatusBetalerBusiness _statusBetalerBusiness;
        private readonly IMailBusiness _mailBusiness;
        private readonly IPersonRepository _personRepository;
        private readonly IAnmeldelserRepository _anmeldelserRepository;
        private readonly IKommuneRepository _kommuneRepository;

        public AdviseringBusiness(
            IAnmeldelserRepository anmeldelserRepository,
            IPersonRepository personRepository,
            IKodelisteBusiness kodelisteBusiness,
            IAdvisRepository advisRepository,
            IUnitOfWork uow,
            IStatusBetalerBusiness statusBetalerBusiness,
            //IStatusAnmeldelseBusiness statusAnmeldelseBusiness,
            IKommunikationBusiness kommunikationBusiness,
            IKommuneRepository kommuneRepository,
            IMailBusiness mailBusiness)
            : base(advisRepository, uow)
        {
            _advisRepository = advisRepository;
            _kodelisteBusiness = kodelisteBusiness;
            _statusBetalerBusiness = statusBetalerBusiness;
            //_statusAnmeldelseBusiness = statusAnmeldelseBusiness;
            _kommunikationBusiness = kommunikationBusiness;
            _mailBusiness = mailBusiness;
            _personRepository = personRepository;
            _anmeldelserRepository = anmeldelserRepository;
            _kommuneRepository = kommuneRepository;
        }

        #region "System adviseringer"

        public bool SendBeskedTilJordmodtageranlæggetsKontaktperson(Anmeldelse anmeldelse)
        {

            if (anmeldelse.ModtagerAnlaeg != null && anmeldelse.ModtagerAnlaeg.Jordmodtager != null)
            {
                var bruger = _personRepository.Search(
                     p =>
                         p.Email == anmeldelse.ModtagerAnlaeg.KontaktpersonEmail &&
                         p.Firmaoplysninger != null &&
                         p.Firmaoplysninger.CVR == anmeldelse.ModtagerAnlaeg.Jordmodtager.CVR).FirstOrDefault();

                if (bruger != null && !bruger.JordmodtagerAdvis)
                {
                    var template = CreateMessage(null, anmeldelse.ModtagerAnlaeg.Jordmodtager.Id,
                        EnumAdvisSkabelon.KommuneGodkenderAnmeldelsen);
                    template.SetAttribute("firmanavn", anmeldelse.ModtagerAnlaeg.Navn);
                    template.SetAttribute("linkanmeldelse", GetAnmeldelseLink(anmeldelse.Id));
                    template.SetAttribute("adresse", anmeldelse.Oprindelsessted.Adresse);
                    template.SetAttribute("kommune", anmeldelse.Kommune.Navn);


                    var appSettings = ConfigurationManager.AppSettings;
                    var domain = appSettings["FlytJordDomain"];
                    template.SetAttribute("blanketUrl", string.Format("https://{0}/blanketter/{1}", domain, anmeldelse.Id));

                    var emailBody = template.ToString();
                    var emailSubject = "FlytJord - Anmeldelse godkendt af kommunen";

                    SendEmail(emailSubject, emailBody, anmeldelse.ModtagerAnlaeg.KontaktpersonEmail);
                }

                return true;
            }
            return false;
        }

        public bool SendBeskedTilRaadgiver(Anmeldelse anmeldelse, string raadgiverEmail, string besked, string afsender, string afsendertlfnr)
        {
            if (anmeldelse != null)
            {
                var template = CreateMessage(null, Guid.Empty, EnumAdvisSkabelon.TilRaadgiver);
                template.SetAttribute("afsender", afsender);
                template.SetAttribute("afsendertlfnr", afsendertlfnr);
                template.SetAttribute("besked", besked);
                template.SetAttribute("linkanmeldelse", GetAnmeldelseLink(anmeldelse.Id));

                var subject = string.Format("FlytJord.dk - meddelelse vedrørende {0}", anmeldelse.Oprindelsessted.Adresse);

                SendEmail(subject, template.ToString(), raadgiverEmail);
                return true;
            }
            return false;
        }

        public bool SendBeskedTilKommuneVedrAkutJordflytning(Anmeldelse anmeldelse, string kommuneEmail)
        {
            if (anmeldelse != null && !string.IsNullOrEmpty(kommuneEmail))
            {
                var samletbesked = "Følgende anmeldelse er oprettet som en akut jordflytning:<br><br>" +
                                   anmeldelse.Oprindelsessted.Adresse +
                                   "<br>Løbenr: " + anmeldelse.Nummer;

                SendEmail("Akut jordflytning - " + anmeldelse.Oprindelsessted.Adresse, samletbesked, kommuneEmail);
                return true;
            }
            return false;
        }

        public void SendAktivationEmail(BrugerProfil userProfile, string activationToken)
        {
            var appSettings = ConfigurationManager.AppSettings;
            var domaine = appSettings["FlytJordDomain"];

            var template = CreateMessage(null, Guid.Empty, EnumAdvisSkabelon.TilNyBruger);
            //Set attributes.
            var aktiverUrl = @"http://" + domaine + "/bruger/aktiver/" + activationToken;
            template.SetAttribute("aktiverUrl", aktiverUrl);
            template.SetAttribute("PasswordClearText", userProfile.PasswordClearText);
            const string subject = "FlytJord.dk - meddelse vedrørende aktivering af bruger";


            SendEmail(subject, template.ToString(), userProfile.BrugerNavn);
        }

        public void SendAktivationEmailVedOprettetAfAndenBruger(BrugerProfil userProfile, string activationToken)
        {
            var appSettings = ConfigurationManager.AppSettings;
            var domaine = appSettings["FlytJordDomain"];

            var template = CreateMessage(null, Guid.Empty, EnumAdvisSkabelon.TilNyBrugerOprettetAfAndenBruger);
            //Set attributes.
            var aktiverUrl = @"http://" + domaine + "/bruger/aktiver/" + activationToken;
            template.SetAttribute("aktiverUrl", aktiverUrl);
            template.SetAttribute("BrugerNavn", userProfile.BrugerNavn);
            template.SetAttribute("PasswordClearText", userProfile.PasswordClearText);
            const string subject = "FlytJord.dk - meddelse vedrørende oprettelse og aktivering af bruger";

            SendEmail(subject, template.ToString(), userProfile.BrugerNavn);
        }

        public bool SendGlemtPassword(string email, string kode)
        {
            const string emailSubject = "FlytJord - Her er dit nye password";
            var template = CreateMessage(null, Guid.Empty, EnumAdvisSkabelon.GlemtPassword);

            var appSettings = ConfigurationManager.AppSettings;
            var domaine = appSettings["FlytJordDomain"];

            template.SetAttribute("newPassword", kode);
            template.SetAttribute("profilurl", string.Format("http://{0}/Bruger/Profil", domaine));

            SendEmail(emailSubject, template.ToString(), email);
            return true;
        }

        public bool SendBeskedTilInteresenter(string besked, Anmeldelse anmeldelse, bool anmelder, bool transportoer,
                                              bool betaler, bool interessenter, bool sagsbehandler, Person afsender)
        {
            if (anmeldelse != null)
            {
                var template = CreateMessage(anmeldelse.Kommune.Kommunenr.ToString(CultureInfo.InvariantCulture), Guid.Empty,
                                             EnumAdvisSkabelon.TilInteressenterFraSagsbehandler);
                template.SetAttribute("adresse", anmeldelse.Oprindelsessted.Adresse);
                template.SetAttribute("besked", besked);
                template.SetAttribute("afsender", afsender.Navn + " " + afsender.Efternavn);

                template.SetAttribute("linkanmeldelse", GetAnmeldelseLink(anmeldelse.Id));
                template.SetAttribute("flytjordUrl", GetVisAnmeldelseLink(anmeldelse.Id));

                var emailBody = template.ToString();
                var b = new Besked { Tekst = emailBody, Tid = DateTime.Now };

                var persons = new List<Person>();
                if (anmelder && anmeldelse.Anmelder != null)
                {
                    persons.Add(anmeldelse.Anmelder.Person);
                    var k = new Kommunikation
                        {
                            Anmeldelse = anmeldelse,
                            Besked = b,
                            Person = afsender,
                            Person1 = anmeldelse.Anmelder.Person
                        };
                    _kommunikationBusiness.CreateKommunikation(k);
                }

                if (transportoer && anmeldelse.Transportoer != null)
                {
                    persons.Add(anmeldelse.Transportoer.Person);
                    var k = new Kommunikation
                        {
                            Anmeldelse = anmeldelse,
                            Besked = b,
                            Person = afsender,
                            Person1 = anmeldelse.Transportoer.Person
                        };
                    _kommunikationBusiness.CreateKommunikation(k);
                }

                if (betaler && anmeldelse.Betaler != null && anmeldelse.Betaler.Person != null)
                {
                    persons.Add(anmeldelse.Betaler.Person);
                    var k = new Kommunikation
                        {
                            Anmeldelse = anmeldelse,
                            Besked = b,
                            Person = afsender,
                            Person1 = anmeldelse.Betaler.Person
                        };
                    _kommunikationBusiness.CreateKommunikation(k);
                }

                if (sagsbehandler && anmeldelse.Sagsbehandler != null)
                {
                    persons.Add(anmeldelse.Sagsbehandler.Person);
                    var k = new Kommunikation
                        {
                            Anmeldelse = anmeldelse,
                            Besked = b,
                            Person = afsender,
                            Person1 = anmeldelse.Sagsbehandler.Person
                        };
                    _kommunikationBusiness.CreateKommunikation(k);
                }

                if (persons.Any())
                {
                    var emailSubject = string.Format("FlytJord.dk - meddelelse vedrørende {0}", anmeldelse.Oprindelsessted.Adresse);
                    if (interessenter && anmeldelse.Interesant != null)
                    {
                        //Af KVE - Nødløsning - Interessenter er ikke nødvendigvis oprettet i person tabellen. 
                        //Derfor kan jeg ikke oprettet poster i advis eller kommunikations tabellen.
                        //Derfor registrere jeg ikke at mailen sendes til interessenterne... 
                        foreach (var i in anmeldelse.Interesant)
                        {
                            SendEmail(emailSubject, emailBody, i.Email);
                        }
                    }
                    foreach (var p in persons)
                    {
                        var advis = new Advis
                            {
                                AdvisType = GetEmailAdvisType(),
                                Person = p,
                                Besked = b
                            };

                        Create(advis);
                        SendEmail(emailSubject, emailBody, p.Email);
                    }
                    return true;
                }
            }
            return false;
        }

        public bool SendHoerAndenKommune(string emne, string besked, Person modtagerPerson, Person afsenderPerson, Anmeldelse anmeldelse)
        {
            //var modtagerPerson = _personRepository.Search(p => p.Email == modtagerEmail).FirstOrDefault();
            var domain = ConfigurationManager.AppSettings["FlytJordDomain"];
            string url = String.Format("<a href=\"http://{0}/default/AndenKommuneSvar/?id={1}&afsenderEmail={2}&modtagerEmail={3}&afsenderId={4}&modtagerId={5}\">Godkend eller afvis via dette link</a>",
                domain, anmeldelse.Id, afsenderPerson.Email, modtagerPerson.Email, afsenderPerson.Id, modtagerPerson.Id);

            if (modtagerPerson != null && afsenderPerson != null)
            {
                var template = CreateMessage(anmeldelse.Kommune.Kommunenr.ToString(CultureInfo.InvariantCulture), Guid.Empty,
                                             EnumAdvisSkabelon.TilAndenKommuneGodkendAfvisAnlaeg);
                template.SetAttribute("adresse", anmeldelse.Oprindelsessted.Adresse + ", " + anmeldelse.Oprindelsessted.Postnummer + " " + anmeldelse.Oprindelsessted.PostDistrikt);

                template.SetAttribute("tilkommune", (anmeldelse.ModtagerAnlaeg.KommuneKode.HasValue ? _kodelisteBusiness.ReadKommuneByKode(anmeldelse.ModtagerAnlaeg.KommuneKode.Value).Navn : string.Empty));

                template.SetAttribute("fraadresse", anmeldelse.Oprindelsessted.Adresse + ", " + anmeldelse.Oprindelsessted.Postnummer + " " + anmeldelse.Oprindelsessted.PostDistrikt);
                template.SetAttribute("tiladresse", anmeldelse.ModtagerAnlaeg.Adresse + ", " + anmeldelse.ModtagerAnlaeg.Postnummer + " " + anmeldelse.ModtagerAnlaeg.PostDistrikt);

                template.SetAttribute("godkendafvislink", url);

                if (besked != "")
                    template.SetAttribute("supplerendeoplysninger", "Supplerende oplysninger:<br />" + besked);
                else
                    template.SetAttribute("supplerendeoplysninger", "");

                //template.SetAttribute("linkanmeldelse", GetAnmeldelseLink(anmeldelse.Id));
                template.SetAttribute("flytjordUrl", GetVisAnmeldelseLink(anmeldelse.Id));

                var emailBody = template.ToString();
                var b = new Besked { Tekst = emailBody, Tid = DateTime.Now };

                if (modtagerPerson != null)
                {
                    var k = new Kommunikation { Anmeldelse = anmeldelse, Besked = b, Person = afsenderPerson, Person1 = modtagerPerson };
                    _kommunikationBusiness.CreateKommunikation(k);
                }


                var emailSubject = string.Format("FlytJord.dk - Høring vedrørende godkendelse af midlertigt anlæg {0}", anmeldelse.ModtagerAnlaeg.Adresse);

                if (modtagerPerson == null)
                {
                    //TOK: Modtager er ikke nødvendigvis oprettet i Flytjord.dk. 
                    //Der kan i det tilfælde ikke oprettes poster i advis eller kommunikation tabellerne.
                    SendEmail(emailSubject, emailBody, modtagerPerson.Email);
                }
                else
                {
                    var advis = new Advis { AdvisType = GetHoerAndenKommuneAdvisType(), Person = modtagerPerson, Besked = b };

                    Create(advis);
                    SendEmail(emailSubject, emailBody, modtagerPerson.Email);
                }

                return true;

                //Af TOK - Modtager er ikke nødvendigvis oprettet i Flytjord.dk. 
                //Der kan i det tilfælde ikke oprettes poster i advis eller kommunikation tabellerne.
                /*if (modtagerPerson == null)
                    SendEmail(emailSubject, emailBody, modtagerEmail);
                else {
                    var b = new Besked { Tekst = emailBody, Tid = DateTime.Now };

                    //var _statusAnmeldelseBusiness = We statusAnmeldelseBusiness;
                    //anmeldelse.StatusAnmeldelse.Add(_statusAnmeldelseBusiness.CreateStatus(EnumStatusAnmeldelse.Afsendt, afsenderPerson));

                    var k = new Kommunikation { Anmeldelse = anmeldelse, Besked = b, Person = afsenderPerson, Person1 = modtagerPerson };
                    _kommunikationBusiness.CreateKommunikation(k);

                    var advis = new Advis { AdvisType = GetEmailAdvisType(), Person = modtagerPerson, Besked = b };
                    Create(advis);
                    SendEmail(emailSubject, emailBody, modtagerEmail);
                    
                    return true;
                }*/
                
            }
            return false;
        }


        public bool AndenKommuneSvar(Boolean godkendt, string besked, string modtagerEmail, string afsenderEmail, Guid anmeldelseId)
        {
            var afsenderPerson = _personRepository.Search(p => p.Email == modtagerEmail).FirstOrDefault();
            var modtagerPerson = _personRepository.Search(p => p.Email == afsenderEmail).FirstOrDefault();
            var anmeldelse = _anmeldelserRepository.Read(anmeldelseId);
            var kommuneNavn = _kommuneRepository.Search(p => p.Kommunenr == anmeldelse.ModtagerAnlaeg.KommuneKode).FirstOrDefault();
            string sKommuneNavn = (kommuneNavn == null ? "" : kommuneNavn.Navn);

            if (afsenderPerson != null)
            {
                var template = CreateMessage(anmeldelse.Kommune.Kommunenr.ToString(CultureInfo.InvariantCulture), Guid.Empty,
                                             EnumAdvisSkabelon.FraAndenKommuneGodkendAfvisAnlaeg);
                //template.SetAttribute("adresse", anmeldelse.Oprindelsessted.Adresse);
                if (godkendt)
                    template.SetAttribute("beslutning", sKommuneNavn + " Kommune godkender at jorden fra anmeldelse med lbnr " + 
                        anmeldelse.Nummer + " må modtages på adressen " + anmeldelse.ModtagerAnlaeg.Adresse + ", " + anmeldelse.ModtagerAnlaeg.Postnummer + " " + anmeldelse.ModtagerAnlaeg.PostDistrikt);
                else
                    template.SetAttribute("beslutning", sKommuneNavn + " Kommune afviser at jorden fra anmeldelse med lbnr " +
                       anmeldelse.Nummer + " må modtages på adressen " + anmeldelse.ModtagerAnlaeg.Adresse + ", " + anmeldelse.ModtagerAnlaeg.Postnummer + " " + anmeldelse.ModtagerAnlaeg.PostDistrikt);

                template.SetAttribute("besked", besked);
                template.SetAttribute("afsender", afsenderPerson.Navn + " " + afsenderPerson.Efternavn + " (" + afsenderPerson.Email + ")");

                var emailBody = template.ToString();
                var b = new Besked { Tekst = emailBody, Tid = DateTime.Now };

                if (modtagerPerson != null)
                {
                    var k = new Kommunikation { Anmeldelse = anmeldelse, Besked = b, Person = modtagerPerson, Person1 = afsenderPerson };
                    _kommunikationBusiness.CreateKommunikation(k);
                }


                var emailSubject = string.Format("FlytJord.dk - Svar vedrørende høring om godkendelse af midlertigt anlæg {0}", 
                    anmeldelse.ModtagerAnlaeg.Adresse + ", " + anmeldelse.ModtagerAnlaeg.Postnummer + " " + anmeldelse.ModtagerAnlaeg.PostDistrikt);

                if (modtagerPerson == null)
                    SendEmail(emailSubject, emailBody, afsenderEmail);
                else
                {
                    var advis = new Advis { AdvisType = GetSvarFraAndenKommuneAdvisType(), Person = modtagerPerson, Besked = b };

                    Create(advis);
                    SendEmail(emailSubject, emailBody, afsenderEmail);

                    return true;
                }
            }
            return false;
        }


        public bool SendBeskedTilBetalerAcceptereDuBetalingen(Anmeldelse anmeldelse)
        {
            if (anmeldelse != null && anmeldelse.Oprindelsessted != null && anmeldelse.Betaler != null && anmeldelse.Betaler.Person != null)
            {
                var template = CreateMessage(null, Guid.Empty, EnumAdvisSkabelon.TilBetalerAcceptereDuBetalingen);

                //Set attributes på template
                if (anmeldelse.Betaler != null && anmeldelse.Betaler.Person != null)
                    template.SetAttribute("navn", anmeldelse.Betaler.Person.Navn + " " + anmeldelse.Betaler.Person.Efternavn);

                if (anmeldelse.Anmelder != null && anmeldelse.Anmelder.Person != null)
                    template.SetAttribute("anmelder", anmeldelse.Anmelder.Person.Navn + " " + anmeldelse.Anmelder.Person.Efternavn);

                template.SetAttribute("oprindelsesstedadresse", anmeldelse.Oprindelsessted.Adresse);
                template.SetAttribute("acceptlink", GetAcceptLink(anmeldelse));
                template.SetAttribute("afvislink", GetAfvisLink(anmeldelse));
                template.SetAttribute("linkanmeldelse", GetAnmeldelseLink(anmeldelse.Id));

                var emailModtagere = new List<string>();
                emailModtagere.Add(anmeldelse.Betaler.Person.Email);
                var emailBody = template.ToString();
                var emailSubject = string.Format("FlytJord.dk - meddelelse vedrørende {0}", anmeldelse.Oprindelsessted.Adresse);

                foreach (var emailModtager in emailModtagere.Distinct())
                {
                    SendEmail(emailSubject, emailBody, emailModtager);
                }

                var advis = new Advis
                    {
                        AdvisType = GetEmailAdvisType(),
                        Anmeldelse = anmeldelse,
                        Person = anmeldelse.Betaler.Person,
                        Besked = new Besked { Tekst = emailBody, Tid = DateTime.Now }
                    };
                Create(advis);

                return true;
            }
            return false;
        }

        public bool SendAktiveretAnmeldelse(Anmeldelse anmeldelse)
        {
            if (anmeldelse != null)
            {
                var domain = ConfigurationManager.AppSettings["FlytJordDomain"];

                var template = CreateMessage(null, Guid.Empty, EnumAdvisSkabelon.AktiveretAnmeldelse);
                template.SetAttribute("adresse", anmeldelse.Oprindelsessted.Adresse);
                template.SetAttribute("blanketUrl", string.Format("https://{0}/blanketter/{1}", domain, anmeldelse.Id));
                template.SetAttribute("flytjordUrl", GetVisAnmeldelseLink(anmeldelse.Id));

                var emailBody = template.ToString();
                var emailSubject = string.Format("FlytJord.dk - meddelelse vedrørende {0}", anmeldelse.Oprindelsessted.Adresse);
                var persons = new List<Person>();
                var emailModtagere = new List<string>();
                persons.Add(anmeldelse.Anmelder.Person);
                emailModtagere.Add(anmeldelse.Anmelder.Person.Email);

                if (anmeldelse.Betaler != null && anmeldelse.Betaler.Person != null && anmeldelse.Anmelder.Person.Id != anmeldelse.Betaler.Person.Id)
                {
                    //hvis anmelder ikke er betaler
                    persons.Add(anmeldelse.Betaler.Person);
                    emailModtagere.Add(anmeldelse.Betaler.Person.Email);
                }

                //Transportør
                if (anmeldelse.Anmelder.Person.Id != anmeldelse.Transportoer.Person.Id) //hvis anmelder ikke er betaler
                {
                    persons.Add(anmeldelse.Transportoer.Person);
                    emailModtagere.Add(anmeldelse.Transportoer.Person.Email);
                }

                //Interessenter
                foreach (var i in anmeldelse.Interesant)
                {
                    emailModtagere.Add(i.Email);
                }

                //Modtageranlæg
                if (anmeldelse.ModtagerAnlaeg.Advis && !String.IsNullOrWhiteSpace(anmeldelse.ModtagerAnlaeg.KontaktpersonEmail))
                {
                    emailModtagere.Add(anmeldelse.ModtagerAnlaeg.KontaktpersonEmail);
                }

                var besked = new Besked { Tekst = emailBody, Tid = DateTime.Now };

                foreach (var emailModtager in emailModtagere.Distinct())
                {
                    SendEmail(emailSubject, emailBody, emailModtager);
                }

                foreach (var p in persons)
                {
                    var advis = new Advis
                        {
                            AdvisType = GetEmailAdvisType(),
                            Anmeldelse = anmeldelse,
                            Person = anmeldelse.Anmelder.Person,
                            Besked = besked
                        };
                    Create(advis);
                }
                return true;
            }
            return false;
        }

        public bool SendAktiveretRevideretAnmeldelse(Anmeldelse anmeldelse)
        {
            if (anmeldelse != null)
            {
                SendBeskedTilJordmodtageranlæggetsKontaktperson(anmeldelse);

                var template = CreateMessage(null, Guid.Empty, EnumAdvisSkabelon.AktiveretRevideretAnmeldelse);
                template.SetAttribute("adresse", anmeldelse.Oprindelsessted.Adresse);

                var domain = ConfigurationManager.AppSettings["FlytJordDomain"];
                template.SetAttribute("blanketUrl", string.Format("https://{0}/blanketter/{1}", domain, anmeldelse.Id));
                template.SetAttribute("flytjordUrl", GetVisAnmeldelseLink(anmeldelse.Id));

                var emailBody = template.ToString();
                var emailSubject = "FlytJord - Angående " + anmeldelse.Oprindelsessted.Adresse;

                var persons = new List<Person>();
                var emailModtagere = new List<string>();
                persons.Add(anmeldelse.Anmelder.Person);
                emailModtagere.Add(anmeldelse.Anmelder.Person.Email);
                if (anmeldelse.Betaler != null && anmeldelse.Betaler.Person != null && anmeldelse.Anmelder.Person.Id != anmeldelse.Betaler.Person.Id)
                {
                    //hvis anmelder ikke er betaler
                    persons.Add(anmeldelse.Betaler.Person);
                    emailModtagere.Add(anmeldelse.Betaler.Person.Email);
                }

                //Transportør
                if (anmeldelse.Anmelder.Person.Id != anmeldelse.Transportoer.Person.Id) //hvis anmelder ikke er betaler
                {
                    persons.Add(anmeldelse.Transportoer.Person);
                    emailModtagere.Add(anmeldelse.Transportoer.Person.Email);
                }

                //Interessenter
                foreach (var i in anmeldelse.Interesant)
                {
                    emailModtagere.Add(i.Email);
                }

                var besked = new Besked { Tekst = emailBody, Tid = DateTime.Now };

                foreach (var emailModtager in emailModtagere.Distinct())
                {
                    SendEmail(emailSubject, emailBody, emailModtager);
                }

                foreach (var p in persons)
                {
                    var advis = new Advis
                        {
                            AdvisType = GetEmailAdvisType(),
                            Anmeldelse = anmeldelse,
                            Person = anmeldelse.Anmelder.Person,
                            Besked = besked
                        };

                    Create(advis);
                }
                return true;
            }
            return false;
        }

        public bool SendAfsluttetAnmeldelse(Anmeldelse anmeldelse, bool isCaseWorker = false)
        {
            if (anmeldelse != null)
            {
                var domain = ConfigurationManager.AppSettings["FlytJordDomain"];
                var template = CreateMessage(null, Guid.Empty, EnumAdvisSkabelon.AfslutAnmeldelse);
                template.SetAttribute("adresse", anmeldelse.Oprindelsessted.Adresse);
                template.SetAttribute("blanketUrl", string.Format("https://{0}/blanketter/{1}", domain, anmeldelse.Id));
                template.SetAttribute("flytjordUrl", GetVisAnmeldelseLink(anmeldelse.Id));

                var emailBody = template.ToString();
                var emailSubject = string.Format("FlytJord.dk - meddelelse vedrørende {0}", anmeldelse.Oprindelsessted.Adresse);

                var persons = new List<Person>();
                var emailModtagere = new List<string>();

                // Advis til sagsbehandler. Tilføjet 9.6.2017 efter ønske fra Hvidovre Kommune.
                if (anmeldelse.Kommune != null && anmeldelse.Kommune.Kommunenr == 167 && anmeldelse.Sagsbehandler != null && anmeldelse.Sagsbehandler.Person != null)
                    emailModtagere.Add(anmeldelse.Sagsbehandler.Person.Email);

                // Jira JF-482 Advis om afslutning af anmeldelser skal ikke kunne fravælges af de forskellige parter. Hvis man er sagsbehandler.
                if (isCaseWorker)
                {
                    // Anmelder
                    persons.Add(anmeldelse.Anmelder.Person);
                    emailModtagere.Add(anmeldelse.Anmelder.Person.Email);

                    //hvis anmelder ikke er betaler
                    if (anmeldelse.Betaler != null && anmeldelse.Betaler.Person != null && anmeldelse.Anmelder.Person.Id != anmeldelse.Betaler.Person.Id)
                    {
                        persons.Add(anmeldelse.Betaler.Person);
                        emailModtagere.Add(anmeldelse.Betaler.Person.Email);
                    }

                    //Transportør
                    if (anmeldelse.Anmelder != null && anmeldelse.Anmelder.Person != null &&
              anmeldelse.Transportoer != null && anmeldelse.Transportoer.Person != null &&
              anmeldelse.Anmelder.Person.Id != anmeldelse.Transportoer.Person.Id) //hvis anmelder ikke er betaler
                    {
                        persons.Add(anmeldelse.Transportoer.Person);
                        emailModtagere.Add(anmeldelse.Transportoer.Person.Email);
                    }

                    //Modtageranlæg
                    if(!String.IsNullOrWhiteSpace(anmeldelse.ModtagerAnlaeg.KontaktpersonEmail))
                        emailModtagere.Add(anmeldelse.ModtagerAnlaeg.KontaktpersonEmail);
                }
                else
                {
                    // Anmelder
                    if (anmeldelse.Anmelder.Person.FrivilligeAdvis)
                    {
                        persons.Add(anmeldelse.Anmelder.Person);
                        emailModtagere.Add(anmeldelse.Anmelder.Person.Email);
                    }

                    //hvis anmelder ikke er betaler
                    if (anmeldelse.Betaler != null && anmeldelse.Betaler.Person != null && anmeldelse.Anmelder.Person.Id != anmeldelse.Betaler.Person.Id)
                    {
                        if (anmeldelse.Betaler.Person.FrivilligeAdvis)
                        {
                            persons.Add(anmeldelse.Betaler.Person);
                            emailModtagere.Add(anmeldelse.Betaler.Person.Email);
                        }
                    }

                    //Transportør
                    if (anmeldelse.Anmelder != null && anmeldelse.Anmelder.Person != null &&
              anmeldelse.Transportoer != null && anmeldelse.Transportoer.Person != null &&
              anmeldelse.Anmelder.Person.Id != anmeldelse.Transportoer.Person.Id) //hvis anmelder ikke er betaler
                    {
                        if (anmeldelse.Transportoer.Person.FrivilligeAdvis)
                        {
                            persons.Add(anmeldelse.Transportoer.Person);
                            emailModtagere.Add(anmeldelse.Transportoer.Person.Email);
                        }
                    }

                    //Modtageranlæg
                    if (anmeldelse.ModtagerAnlaeg.Advis)
                    {
                        if (!String.IsNullOrWhiteSpace(anmeldelse.ModtagerAnlaeg.KontaktpersonEmail))
                            emailModtagere.Add(anmeldelse.ModtagerAnlaeg.KontaktpersonEmail);
                    }
                }

                //Interessenter
                foreach (var i in anmeldelse.Interesant)
                {
                    emailModtagere.Add(i.Email);
                }

                var besked = new Besked { Tekst = emailBody, Tid = DateTime.Now };

                foreach (var emailModtager in emailModtagere.Distinct())
                {
                    SendEmail(emailSubject, emailBody, emailModtager);
                }

                foreach (var p in persons)
                {
                    var advis = new Advis
                        {
                            AdvisType = GetEmailAdvisType(),
                            Anmeldelse = anmeldelse,
                            Person = p,
                            Besked = besked
                        };
                    Create(advis);
                }

                return true;
            }
            return false;
        }

        public bool SendAlarmAdvis(Alarm alarm)
        {
            if (alarm != null && alarm.Anmeldelse != null && alarm.Anmeldelse.ModtagerAnlaeg != null && alarm.Anmeldelse.ModtagerAnlaeg.Jordmodtager != null)
            {
                var template = CreateMessage(null, alarm.Anmeldelse.ModtagerAnlaeg.Jordmodtager.Id, EnumAdvisSkabelon.AlarmKoertJord);
                template.SetAttribute("alarm", alarm.Besked.Tekst);
                template.SetAttribute("adresse", alarm.Anmeldelse.Oprindelsessted.Adresse);
                template.SetAttribute("linkanmeldelse", GetAnmeldelseLink(alarm.Anmeldelse.Id));

                //Når beskeden oprettes er teksten kun alarm teksten. 
                //Når alarmen/advis afsendes opdateres besked.tekst til html indholdet.
                alarm.Besked.Tekst = template.ToString();
                var emailSubject = string.Format("FlytJord.dk - meddelelse vedrørende {0}", alarm.Anmeldelse.Oprindelsessted.Adresse);

                var emailModtagere = new List<string>();
                emailModtagere.Add(alarm.Person.Email);

                //Interessenter - Hvis alarmen sendes til anmelder så sendes den også til interessenter. 
                if (alarm.Anmeldelse.Anmelder.Person.Id == alarm.Person.Id)
                {
                    foreach (var i in alarm.Anmeldelse.Interesant)
                    {
                        emailModtagere.Add(i.Email);
                    }
                }

                foreach (var emailModtager in emailModtagere)
                {
                    SendEmail(emailSubject, template.ToString(), emailModtager);
                }

                var advis = new Advis
                    {
                        Anmeldelse = alarm.Anmeldelse,
                        AdvisType = GetEmailAdvisType(),
                        Person = alarm.Person,
                        Besked = alarm.Besked
                    };

                Create(advis);
                return true;
            }
            return false;
        }

        public bool SendBeskedVedrBetalerAfvistAfBogholder(IList<Anmeldelse> anmeldelser)
        {
            if (anmeldelser != null && anmeldelser.Any())
            {
                var emailModtagere = new List<string>();

                foreach (var a in anmeldelser)
                {
                    if (a.ModtagerAnlaeg.Jordmodtager == null)
                        continue;

                    if (a.Anmelder != null && a.Anmelder.Person != null)
                        emailModtagere.Add(a.Anmelder.Person.Email);

                    if (a.Transportoer != null && a.Transportoer.Person != null)
                        emailModtagere.Add(a.Transportoer.Person.Email);

                    if (a.Betaler != null && a.Betaler.Person != null)
                        emailModtagere.Add(a.Betaler.Person.Email);

                    var betalernavn = "";

                    if (a.Betaler != null && a.Betaler.Person != null)
                    {
                        var firmanavn = "-";
                        if (a.Betaler.Person.Firmaoplysninger != null)
                            firmanavn = ", " + a.Betaler.Person.Firmaoplysninger.Firmanavn;

                        betalernavn = string.Format("{0} {1} {2}", a.Betaler.Person.Navn, a.Betaler.Person.Efternavn, firmanavn);
                    }

                    var template = CreateMessage(null, a.ModtagerAnlaeg.Jordmodtager.Id, EnumAdvisSkabelon.BeskedVedrBetalerAfvistAfBogholder);
                    template.SetAttribute("Betaleren", betalernavn);
                    template.SetAttribute("Jordmodtagernavn", a.ModtagerAnlaeg.Jordmodtager.Navn);
                    template.SetAttribute("Oprindelsessted", a.Oprindelsessted.Adresse);

                    var emailBody = template.ToString();
                    const string emailSubject = "FlytJord - Anmeldelse låses pga. betaler er blevet afvist";

                    var distinctModtagere = emailModtagere.Distinct();
                    foreach (var email in distinctModtagere)
                    {
                        SendEmail(emailSubject, emailBody, email);
                    }
                }
                return true;
            }
            return false;
        }

        #endregion

        #region "Jordmodtager adviseringer"

        public bool SendBeskedTilBetalerFraBogholder(string besked, Guid betalerId, Guid jordmodtagerId)
        {
            var statusBetaler = _statusBetalerBusiness.GetStatusBetaler(betalerId, jordmodtagerId);
            if (statusBetaler != null)
            {
                string statusbetaler;
                var template = CreateMessage(null, jordmodtagerId, EnumAdvisSkabelon.TilBetalerNytFraBogholder);
                template.SetAttribute("navn", statusBetaler.Betaler.Person.Navn + " " + statusBetaler.Betaler.Person.Efternavn);
                template.SetAttribute("tekst", besked);
                template.SetAttribute("jordmodtagernavn", statusBetaler.Jordmodtager.Navn);

                if (statusBetaler.Godkendt.HasValue)
                {
                    statusbetaler = statusBetaler.Godkendt.Value ? "Godkendt" : "Afvist";
                    template.SetAttribute("obsgodkend", "");
                }
                else
                {
                    statusbetaler = "Under behandling";
                }
                template.SetAttribute("statusbetaler", statusbetaler);

                var emailBody = template.ToString();
                var emailSubject = "FlytJord.dk - Nyt fra " + statusBetaler.Jordmodtager.Navn;

                SendEmail(emailSubject, emailBody, statusBetaler.Betaler.Person.Email);

                var advis = new Advis
                    {
                        AdvisType = GetEmailAdvisType(),
                        Person = statusBetaler.Betaler.Person,
                        Besked = new Besked { Tekst = emailBody, Tid = DateTime.Now }
                    };

                Create(advis);
                return true;
            }
            return false;
        }

        public bool SendBeskedTilAnmelderAfvistAfJordmodtager(Anmeldelse anmeldelse)
        {
            if (anmeldelse != null)
            {
                var template = CreateMessage(null, anmeldelse.ModtagerAnlaeg.Jordmodtager.Id, EnumAdvisSkabelon.TilAnmelderJordmodtagerAfviserAnmeldelsen);
                template.SetAttribute("linkanmeldelse", GetAnmeldelseLink(anmeldelse.Id));
                template.SetAttribute("adresse", anmeldelse.Oprindelsessted.Adresse);
                template.SetAttribute("aarsag", anmeldelse.AarsagAfvisning);
                template.SetAttribute("modtageranlaegnavn", anmeldelse.ModtagerAnlaeg.Navn);
                template.SetAttribute("flytjordUrl", GetVisAnmeldelseLink(anmeldelse.Id));

                var emailBody = template.ToString();
                var emailSubject = "FlytJord - Angående " + anmeldelse.Oprindelsessted.Adresse;
                var besked = new Besked { Tekst = emailBody, Tid = DateTime.Now };

                var persons = new List<Person>();
                var emailModtagere = new List<string>();

                //Anmelder
                persons.Add(anmeldelse.Anmelder.Person);
                emailModtagere.Add(anmeldelse.Anmelder.Person.Email);

                //Betaler
                if (anmeldelse.Betaler != null && anmeldelse.Betaler.Person != null && anmeldelse.Betaler.Person.FrivilligeAdvis)
                {
                    if (!emailModtagere.Contains(anmeldelse.Betaler.Person.Email))
                    {
                        persons.Add(anmeldelse.Betaler.Person);
                        emailModtagere.Add(anmeldelse.Betaler.Person.Email);
                    }
                }

                //Interessenter
                foreach (var i in anmeldelse.Interesant)
                {
                    emailModtagere.Add(i.Email);
                }

                foreach (var emailModtager in emailModtagere)
                {
                    SendEmail(emailSubject, emailBody, emailModtager);
                }

                foreach (var p in persons)
                {
                    var advis = new Advis
                        {
                            AdvisType = GetEmailAdvisType(),
                            Person = p,
                            Besked = besked,
                            Anmeldelse = anmeldelse
                        };
                    Create(advis);
                }
                return true;
            }
            return false;
        }

        public bool SendBeskedTilLabVedrStikproeven(Stikproeve stikproeve, Person person)
        {
            if (stikproeve != null)
            {
                var template = CreateMessage(null, stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager.Id, EnumAdvisSkabelon.TilLabProeveSkalAnalyses);
                var appSettings = ConfigurationManager.AppSettings;
                var domaine = appSettings["FlytJordDomain"];

                template.SetAttribute("stikproevenummer", stikproeve.Nummer);
                template.SetAttribute("LinkTilStikProeve", string.Format("http://{0}/backend/defaultbackend/index?fane=stikproeve&id={1}", domaine, stikproeve.Id));

                var emailBody = template.ToString();
                var emailSubject = "FlytJord - Stikprøvenr." + stikproeve.Nummer;

                SendEmail(emailSubject, emailBody, person.Email);
                return true;
            }
            return false;
        }

        public bool SendBeskedTilMiljoemedarbejderVedrStikproeven(Stikproeve stikproeve, IList<Person> persons)
        {
            //http://jira.niras.dk/browse/JF-418
            if (stikproeve != null)
            {
                var template = CreateMessage(null, stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager.Id, EnumAdvisSkabelon.TilMiljoemedarbejderVedrStikproeve);
                template.SetAttribute("stikproevenr", stikproeve.Nummer);

                var adresse = "";
                if (stikproeve.Vognlaes != null && stikproeve.Vognlaes.Anmeldelse != null &&
                    stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg != null &&
                    stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager != null)
                {
                    adresse = stikproeve.Vognlaes.Anmeldelse.Oprindelsessted.Adresse;
                }
                else if (stikproeve.PlanlagteStikproever != null && stikproeve.PlanlagteStikproever.Count == 1 &&
                         stikproeve.PlanlagteStikproever.First().Anmeldelse != null &&
                         stikproeve.PlanlagteStikproever.First().Anmeldelse.ModtagerAnlaeg != null &&
                         stikproeve.PlanlagteStikproever.First().Anmeldelse.ModtagerAnlaeg.Jordmodtager != null)
                {
                    adresse = stikproeve.PlanlagteStikproever.First().Anmeldelse.Oprindelsessted.Adresse;
                }
                template.SetAttribute("adresse", adresse);

                var emailBody = template.ToString();
                var emailSubject = "FlytJord - Stikprøvenr. " + stikproeve.Nummer;

                foreach (var p in persons)
                {
                    SendEmail(emailSubject, emailBody, p.Email);
                }
                return true;
            }
            return false;
        }

        public bool SendBeskedTilPladsmandVedrStikproeven(Stikproeve stikproeve, IList<Person> persons, EnumStatusStikproeve statusStikproeve)
        {
            if (stikproeve != null)
            {
                var template = CreateMessage(null, stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager.Id, EnumAdvisSkabelon.TilPladsmandNytOmStikproeve);

                template.SetAttribute("baasnr", stikproeve.Baas.HasValue ? stikproeve.Baas.Value.ToString(CultureInfo.InvariantCulture) : "-");

                var adresse = "";
                System.Guid anmeldelseGUID = System.Guid.Empty;
                if (stikproeve.Vognlaes != null && stikproeve.Vognlaes.Anmeldelse != null &&
                    stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg != null &&
                    stikproeve.Vognlaes.Anmeldelse.ModtagerAnlaeg.Jordmodtager != null)
                {
                    adresse = stikproeve.Vognlaes.Anmeldelse.Oprindelsessted.Adresse;
                    anmeldelseGUID = stikproeve.Vognlaes.Anmeldelse.Id;
                }
                else if (stikproeve.PlanlagteStikproever != null && stikproeve.PlanlagteStikproever.Count == 1 &&
                         stikproeve.PlanlagteStikproever.First().Anmeldelse != null &&
                         stikproeve.PlanlagteStikproever.First().Anmeldelse.ModtagerAnlaeg != null &&
                         stikproeve.PlanlagteStikproever.First().Anmeldelse.ModtagerAnlaeg.Jordmodtager != null)
                {
                    adresse = stikproeve.PlanlagteStikproever.First().Anmeldelse.Oprindelsessted.Adresse;
                    anmeldelseGUID = stikproeve.PlanlagteStikproever.First().Anmeldelse.Id;
                }
                template.SetAttribute("adresse", adresse);

                /* Ny tekst for afvisning efter aftale med KRK den 13.06.2014/TOK*/
                switch (statusStikproeve)
                {
                    case EnumStatusStikproeve.AnalyseAfvistPgaAffald:
                        {
                            template.SetAttribute("stikproevestatus",
                                "<p>Stikprøven viser, at jorden er mere forurenet end hvad der kan modtages på anlægget.</p><p>Du bliver i den nærmeste fremtid kontaktet vedrørende den videre håndtering af jordlæsset.");
                            break;
                        }
                    case EnumStatusStikproeve.AnalyseAfvistPgaForureningskomponenter:
                        {
                            template.SetAttribute("stikproevestatus",
                                "<p>Stikprøven viser, at jorden er mere forurenet end hvad der kan modtages på anlægget.</p><p>Du bliver i den nærmeste fremtid kontaktet vedrørende den videre håndtering af jordlæsset.");
                            break;
                        }
                    case EnumStatusStikproeve.AnalyseGodkendt:
                        {
                            template.SetAttribute("stikproevestatus", "Jorden herfra er godkendt.");
                            break;
                        }
                    default:
                        {
                            template.SetAttribute("stikproevestatus", "-");
                            break;
                        }
                }


                var appSettings = ConfigurationManager.AppSettings;
                var domaine = appSettings["FlytJordDomain"];
                template.SetAttribute("link", GetAnmeldelseLink(anmeldelseGUID));
                var emailBody = template.ToString();
                var emailSubject = "FlytJord - Angående stikprøve nr: " + stikproeve.Nummer;

                foreach (var p in persons)
                {
                    SendEmail(emailSubject, emailBody, p.Email);
                }
                return true;
            }
            return false;
        }

        public bool SendBeskedTilProevetagerTidTilJordproever(ModtagerAnlaeg modtagerAnlaeg, IList<Person> proevetagere, Stikproeve stikproeve)
        {
            if (modtagerAnlaeg != null)
            {
                var template = CreateMessage(null, modtagerAnlaeg.Jordmodtager.Id, EnumAdvisSkabelon.TilProeveTagerTidTilJordproever);
                template.SetAttribute("baas", stikproeve.Baas);
                template.SetAttribute("modtageranlaegnavn", modtagerAnlaeg.Navn);

                var emailBody = template.ToString();
                var emailSubject = "FlytJord - Stikprøvekontrol " + modtagerAnlaeg.Navn;

                foreach (var person in proevetagere)
                {
                    SendEmail(emailSubject, emailBody, person.Email);
                }
                return true;
            }
            return false;
        }

        #endregion

        #region "Kommune adviseringer"

        public bool SendBeskedTilAnmelderAfvistAfKommunen(Anmeldelse anmeldelse)
        {
            if (anmeldelse != null)
            {
                var template = CreateMessage(anmeldelse.Kommune.Kommunenr.ToString(CultureInfo.InvariantCulture), Guid.Empty,
                                             EnumAdvisSkabelon.TilAnmelderKommuneAfviserAnmeldelsen);

                template.SetAttribute("linkanmeldelse", GetAnmeldelseLink(anmeldelse.Id));
                template.SetAttribute("adresse", anmeldelse.Oprindelsessted.Adresse);
                template.SetAttribute("aarsag", anmeldelse.AarsagAfvisning);
                template.SetAttribute("navn", anmeldelse.Anmelder.Person.Navn + anmeldelse.Anmelder.Person.Efternavn);
                template.SetAttribute("flytjordUrl", GetVisAnmeldelseLink(anmeldelse.Id));

                var emailBody = template.ToString();
                var emailSubject = "FlytJord - Angående " + anmeldelse.Oprindelsessted.Adresse;
                var besked = new Besked { Tekst = emailBody, Tid = DateTime.Now };

                var persons = new List<Person>();
                var emailModtagere = new List<string>();

                //Anmelder
                persons.Add(anmeldelse.Anmelder.Person);
                emailModtagere.Add(anmeldelse.Anmelder.Person.Email);

                //Betaler
                if (anmeldelse.Betaler != null && anmeldelse.Betaler.Person != null && anmeldelse.Betaler.Person.FrivilligeAdvis)
                {
                    if (!emailModtagere.Contains(anmeldelse.Betaler.Person.Email))
                    {
                        persons.Add(anmeldelse.Betaler.Person);
                        emailModtagere.Add(anmeldelse.Betaler.Person.Email);
                    }
                }

                //Interessenter
                foreach (var i in anmeldelse.Interesant)
                {
                    emailModtagere.Add(i.Email);
                }

                foreach (var emailModtager in emailModtagere)
                {
                    SendEmail(emailSubject, emailBody, emailModtager);
                }

                foreach (var p in persons)
                {
                    var advis = new Advis
                        {
                            AdvisType = GetEmailAdvisType(),
                            Person = p,
                            Besked = besked,
                            Anmeldelse = anmeldelse
                        };
                    Create(advis);
                }
                return true;
            }
            return false;
        }

        public bool SendTilSagsbehandlerVedIndsendAnmeldelse(Anmeldelse anmeldelse, bool autoGodkendt)
        {
            if (anmeldelse == null)
                return false;

            var template = CreateMessage(anmeldelse.Kommune.Kommunenr.ToString(CultureInfo.InvariantCulture),
                Guid.Empty,
                EnumAdvisSkabelon.SendTilSagsbehandlerVedIndsendAnmeldelse);

            if (autoGodkendt)
            {
                template.SetAttribute("status",
                    String.Format("Anmeldelse med løbenr. {0} for adressen {1} er indsendt og autogodkendt.",
                        anmeldelse.Nummer, anmeldelse.Oprindelsessted.Adresse));
            }
            else
            {
                template.SetAttribute("status",
                    String.Format("Anmeldelse med løbenr. {0} for adressen {1} er indsendt og skal sagsbehandles.",
                        anmeldelse.Nummer, anmeldelse.Oprindelsessted.Adresse));
            }


            string subject = "FlytJord - Angående " + anmeldelse.Oprindelsessted.Adresse;
            string body = template.ToString();

            foreach (PersonKommune person in anmeldelse.Kommune.PersonKommune)
            {
                SendEmail(subject, body, person.Person.Email);

                var advis = new Advis
                {
                    AdvisType = GetEmailAdvisType(),
                    Person = person.Person,
                    Besked = new Besked { Tekst = body, Tid = DateTime.Now },
                    Anmeldelse = anmeldelse
                };
                Create(advis);
            }

            return true;
        }

        #endregion



        public string AdvisUdenHttp(string advis)
        {
            var s = advis;
            var i = 0;
            while (s.ToLower().Contains("http"))
            {
                i++;
                s = RemoveHttp(s);
                if (i > 20) //Jeg skal ikke have en uendelig løkke!
                    break;
            }
            return s;
        }

        public Advis GetAdvis(Guid id)
        {
            return _advisRepository.Read(id);
        }

        #region *** Private methods ***

        private static string RemoveHttp(string s)
        {
            if (s.ToLower().Contains("http"))
            {
                var startIndex = s.ToLower().IndexOf("http", StringComparison.Ordinal);
                var arrEndChar = new char[2];
                arrEndChar[0] = '"';
                arrEndChar[1] = '<';
                var endIndex = s.Substring(startIndex).IndexOfAny(arrEndChar);
                s = s.Remove(startIndex, endIndex);

                //s = s.Replace("href=\"\"", "href=\"\" target=\"_blank\"");
                s = s.Replace("href=\"\"", "");

                return s;
            }
            return s;
        }

        private static StringTemplate CreateMessage(string kommuneNr, Guid jordmodtagerId, EnumAdvisSkabelon skabelon)
        {
            try
            {
                StringTemplate template;
                string skabelonBaseDir;

                if (HttpContext.Current != null)
                    skabelonBaseDir = HttpContext.Current.Server.MapPath(@"~\App_Data\skabeloner");
                else
                    skabelonBaseDir = @"C:\projects\jordflytning\trunk\Niras.Jordflytning\App_Data\Skabeloner";

                switch (skabelon)
                {
                    //KOMMUNE
                    case EnumAdvisSkabelon.TilInteressenterFraSagsbehandler:
                    case EnumAdvisSkabelon.TilAnmelderKommuneAfviserAnmeldelsen:
                    case EnumAdvisSkabelon.SendTilSagsbehandlerVedIndsendAnmeldelse:
                    case EnumAdvisSkabelon.TilAndenKommuneGodkendAfvisAnlaeg:
                    case EnumAdvisSkabelon.FraAndenKommuneGodkendAfvisAnlaeg:
                        {
                            var skabelonOrganisationDir = Path.Combine(skabelonBaseDir, "Kommune", kommuneNr);
                            var templates = new StringTemplateGroup("FlytJord", skabelonOrganisationDir);
                            template = templates.GetInstanceOf(skabelon.ToString());
                            break;
                        }

                    //JORDMODTAGER
                    case EnumAdvisSkabelon.TilBetalerNytFraBogholder:
                    case EnumAdvisSkabelon.TilLabProeveSkalAnalyses:
                    case EnumAdvisSkabelon.TilMiljoemedarbejderVedrStikproeve:
                    case EnumAdvisSkabelon.TilPladsmandNytOmStikproeve:
                    case EnumAdvisSkabelon.TilProeveTagerTidTilJordproever:
                    case EnumAdvisSkabelon.AlarmKoertJord:
                    case EnumAdvisSkabelon.BeskedVedrBetalerAfvistAfBogholder:
                    case EnumAdvisSkabelon.TilAnmelderJordmodtagerAfviserAnmeldelsen:
                        {
                            var skabelonOrganisationDir = Path.Combine(skabelonBaseDir, "Jordmodtager", jordmodtagerId.ToString());
                            var templates = new StringTemplateGroup("FlytJord", skabelonOrganisationDir);
                            template = templates.GetInstanceOf(skabelon.ToString());
                            break;
                        }


                    //SYSTEM
                    //case EnumAdvisSkabelon.TilAnmelderOgTransAnmeldelsenErGodkendt:
                    case EnumAdvisSkabelon.TilBetalerAcceptereDuBetalingen:
                    //case EnumAdvisSkabelon.TilInteressenterAlarmPaaAnmeldelse:
                    case EnumAdvisSkabelon.TilNyBruger:
                    case EnumAdvisSkabelon.TilNyBrugerOprettetAfAndenBruger:
                    case EnumAdvisSkabelon.AktiveretAnmeldelse:
                    case EnumAdvisSkabelon.AktiveretRevideretAnmeldelse:
                    case EnumAdvisSkabelon.TilRaadgiver:
                    case EnumAdvisSkabelon.GlemtPassword:
                    case EnumAdvisSkabelon.KommuneGodkenderAnmeldelsen:
                    case EnumAdvisSkabelon.AfslutAnmeldelse:
                        {
                            var skabelonOrganisationDir = Path.Combine(skabelonBaseDir, "System");
                            var templates = new StringTemplateGroup("FlytJord", skabelonOrganisationDir);
                            template = templates.GetInstanceOf(skabelon.ToString());
                            break;
                        }

                    default:
                        {
                            throw new Exception("Skabelonen er ikke implementeret");
                        }
                }

                return template;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        private static string GetVisAnmeldelseLink(Guid anmeldelseId)
        {
            var domain = ConfigurationManager.AppSettings["FlytJordDomain"];
            var retVal = string.Format("http://{0}/Anmeldelser/VisAnmeldelse/{1}", domain, anmeldelseId);
            return retVal;
        }

        private static string GetAnmeldelseLink(Guid anmeldelseId)
        {
            var domain = ConfigurationManager.AppSettings["FlytJordDomain"];
            var retVal = string.Format("http://{0}/Anmeldelser/rediger?anmeldelseId={1}", domain, anmeldelseId);
            return retVal;
        }

        private static string GetAfvisLink(Anmeldelse anmeldelse)
        {
            var domain = ConfigurationManager.AppSettings["FlytJordDomain"];
            var retVal = string.Format(@"http://{0}/default/gateway?g1={1}&handling={2}&g2={3}", domain, anmeldelse.Id, EnumStatusAnmeldelse.BetalerAfviserBetalingen, anmeldelse.Betaler.Id);
            return retVal;
        }

        private static string GetAcceptLink(Anmeldelse anmeldelse)
        {
            var domain = ConfigurationManager.AppSettings["FlytJordDomain"];
            var retVal = string.Format(@"http://{0}/default/gateway?g1={1}&handling={2}&g2={3}", domain, anmeldelse.Id, EnumStatusAnmeldelse.BetalerAccepteretBetalingen, anmeldelse.Betaler.Id);
            return retVal;
        }

        private AdvisType GetEmailAdvisType()
        {
            var advisTypeEmail = (from at in _kodelisteBusiness.ReadAktiveAdvisTypes()
                                  where at.Kode == (short)EnumAdvisType.Email
                                  select at).FirstOrDefault();
            return advisTypeEmail;
        }

        private AdvisType GetHoerAndenKommuneAdvisType()
        {
            var advisTypeHoerAndenKommune = (from at in _kodelisteBusiness.ReadAktiveAdvisTypes()
                                             where at.Kode == (short)EnumAdvisType.HørAndenKommune
                                             select at).FirstOrDefault();
            return advisTypeHoerAndenKommune;
        }

        private AdvisType GetSvarFraAndenKommuneAdvisType()
        {
            var advisTypeSvarFraAndenKommune = (from at in _kodelisteBusiness.ReadAktiveAdvisTypes()
                                             where at.Kode == (short)EnumAdvisType.SvarfraAndenKommune
                                             select at).FirstOrDefault();
            return advisTypeSvarFraAndenKommune;
        }


        private void SendEmail(string subject, string content, string emailTo)
        {
            //Pga af at mailene ryger i spamfiltre, prøver vi nu mail webservicen som anvendes i Lucrative
            Logger.LogInfo("Email info:");
            Logger.LogInfo("MailTo: " + emailTo);
            Logger.LogInfo("Subject: " + subject);
            Logger.LogInfo("Body: " + content);

            _mailBusiness.SendEmail(subject, content, emailTo);
            Logger.LogInfo("Email afsendt");
            Logger.LogInfo("");
        }

        #endregion *** Private methods ***

    }
}
