using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Library.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.Objects;
using System.Data.Spatial;
using System.Linq;

namespace Niras.Jordflytning.Core.BusinessLogic
{
    public class AnmeldelserBusiness : GenericBusiness<Anmeldelse>, IAnmeldelserBusiness
    {
        private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.AnmeldelserBusiness");

        private readonly IAnmeldelserRepository _anmeldelserRepository;
        private readonly IInteresantRepository _interesantRepository;
        private readonly IFaktureringRepository _faktureringRepository;
        private readonly IStatusAnmeldelseBusiness _statusAnmeldelseBusiness;
        private readonly IDokumentationBusiness _dokumentationBusiness;
        private readonly IAdviseringBusiness _adviseringBusiness;
        private readonly IKonfigBusiness _konfigBusiness;
        private readonly IBetalerBusiness _betalerBusiness;
        private readonly IJordmodtagerBusiness _jordmodtagerBusiness;
        private readonly IPdfBusiness _pdfBusiness;
        private readonly IAlarmBusiness _alarmBusiness;
        private readonly ILogBusiness _logBusiness;
        private readonly IStatusBetalerBusiness _statusBetalerBusiness;
        private readonly IKodelisteBusiness _kodelisteBusiness;
        private readonly IBrugereBusiness _brugereBusiness;

        public AnmeldelserBusiness(
          IAnmeldelserRepository anmeldelserRepository,
          IInteresantRepository interesantRepository,
          IFaktureringRepository faktureringRepository,
          IUnitOfWork uow,
          IStatusAnmeldelseBusiness statusAnmeldelseBusiness,
          IDokumentationBusiness dokumentationBusiness,
          IAdviseringBusiness adviseringBusiness,
          IKonfigBusiness konfigBusiness,
          IBetalerBusiness betalerBusiness,
          IJordmodtagerBusiness jordmodtagerBusiness,
          IPdfBusiness pdfBusiness,
          IAlarmBusiness alarmBusiness,
          ILogBusiness logBusiness,
          IStatusBetalerBusiness statusBetalerBusiness,
          IKodelisteBusiness kodelisteBusiness,
          IBrugereBusiness brugereBusiness
          )
            : base(anmeldelserRepository, uow)
        {
            _anmeldelserRepository = anmeldelserRepository;
            _interesantRepository = interesantRepository;
            _faktureringRepository = faktureringRepository;
            _statusAnmeldelseBusiness = statusAnmeldelseBusiness;
            _dokumentationBusiness = dokumentationBusiness;
            _adviseringBusiness = adviseringBusiness;
            _konfigBusiness = konfigBusiness;
            _betalerBusiness = betalerBusiness;
            _jordmodtagerBusiness = jordmodtagerBusiness;
            _pdfBusiness = pdfBusiness;
            _alarmBusiness = alarmBusiness;
            _logBusiness = logBusiness;
            _statusBetalerBusiness = statusBetalerBusiness;
            _kodelisteBusiness = kodelisteBusiness;
            _brugereBusiness = brugereBusiness;
        }

        #region *** Public Methods ***

        public Anmeldelse GetAnmeldelse(Guid id)
        {
            return _anmeldelserRepository.Read(id);
        }

        public IEnumerable<Anmeldelse> GetAnmeldelserOnModtageAnlaeg(Guid modtageAnlaegId)
        {
            var startTime = DateTime.Now;

            //var list = (from anmeldelse in _anmeldelserRepository.Read() where anmeldelse.ModtagerAnlaegId == modtageAnlaegId select anmeldelse).ToList();

            //var list = (from anmeldelse in _anmeldelserRepository.SearchInclude(x => x.ModtagerAnlaegId == modtageAnlaegId, EnumIncludeTables.StatusAnmeldelse_StatusAnmeldelseType) select anmeldelse).ToList();
            //Logger.LogInfo("GetAnmeldelserOnModtageAnlaeg split 1 time: " + (DateTime.Now - startTime).TotalSeconds + " seconds.");


            //TODO - OVERVEJ om Afsluttet kan indgå før

            //Laver filtreringen efter database opslaget da EF ved .search() brokker sig over at GetStatusAnmeldelseTyperEfterRevideretAfAnmelder ikke må indgå i EF kaldet.
            //var list2 =
            //  list.Where(
            //    a =>
            //    _statusAnmeldelseBusiness.GetStatusAnmeldelseTyperEfterRevideretAfAnmelder(a.StatusAnmeldelse)
            //                             .All(x => x.StatusAnmeldelseType.Kode != (short)EnumStatusAnmeldelse.Afsluttet)).ToList();

            //Logger.LogInfo("GetAnmeldelserOnModtageAnlaeg call time: " + (DateTime.Now - startTime).TotalSeconds + " seconds.");


            var list = _anmeldelserRepository.Search(x => x.ModtagerAnlaegId == modtageAnlaegId
              && (!x.StatusAnmeldelse.Any(s => s.StatusAnmeldelseType.Kode == (short)EnumStatusAnmeldelse.Afsluttet || s.StatusAnmeldelseType.Kode == (short)EnumStatusAnmeldelse.RevideretAfAnmelder) //Alm anmeldeser: Der må ikke være nogen status afsluttet eller status revideret
              || //eller
              x.StatusAnmeldelse.Where(s => s.StatusAnmeldelseType.Kode == (short)EnumStatusAnmeldelse.RevideretAfAnmelder).OrderByDescending(t => t.Tid).Select(tt => tt.Tid).FirstOrDefault() >
              x.StatusAnmeldelse.Where(s => s.StatusAnmeldelseType.Kode == (short)EnumStatusAnmeldelse.Afsluttet).OrderByDescending(t => t.Tid).Select(tt => tt.Tid).FirstOrDefault()
                //hvis der findes en status revideret af anmelder som er nyere end status anmeldelse
              )).ToList();
            Logger.LogInfo("GetAnmeldelserOnModtageAnlaeg time: " + (DateTime.Now - startTime).TotalSeconds + " seconds.");


            return list;
        }

        public IEnumerable<Anmeldelse> GetAnmeldelserOnModtageAnlaegIncludingVognlaesStikproever(Guid modtageAnlaegId)
        {
            //OBS Der søges i alle anmeldelser også dem som er afsluttet.
            var list =
              (from anmeldelse in _anmeldelserRepository.SearchInclude(x => x.ModtagerAnlaegId == modtageAnlaegId, EnumIncludeTables.Vognlaes_Stikproeve_StatusStikproeve_StatusStikproeveType)
               select anmeldelse).ToList();

            return list;
        }


        public IEnumerable<Anmeldelse> GetUsersAktuelleStikproeverOnModtageAnlaegIncludingVognlaesStikproever(Guid modtageAnlaegId)
        {
            //OBS Der søges i alle anmeldelser også dem som er afsluttet.
            var list =
              (from anmeldelse in _anmeldelserRepository.SearchInclude(x => x.ModtagerAnlaegId == modtageAnlaegId
                && (x.Vognlaes.Any(v => v.Stikproeve.Any())) //Der skal være en stikprøve.
                , EnumIncludeTables.Vognlaes_Stikproeve_StatusStikproeve_StatusStikproeveType)
               select anmeldelse).ToList();

            return list;
        }

        public void GemAnmeldelseOgSetStatus(Anmeldelse anmeldelse, Person udfoertAfPerson, EnumStatusAnmeldelse status)
        {
            anmeldelse.StatusAnmeldelse = _statusAnmeldelseBusiness.Read(anmeldelse.Id);
            var s = _statusAnmeldelseBusiness.CreateStatus(EnumStatusAnmeldelse.Gemt, udfoertAfPerson);
            anmeldelse.StatusAnmeldelse.Add(s);
            Create(anmeldelse);
        }

        public void GemAnmeldelseFjernFlagSagsbehandler(Anmeldelse anmeldelse)
        {
            anmeldelse.FlagSagsbehandler = null;
            Create(anmeldelse);
        }

        /// <summary>
        /// Dokumenter som er tilknyttet anmeldelse skal ikke vises i den historiske liste
        /// </summary>
        public IList<Dokumentation> HentHistoriskeDokumenter(DbGeometry dbGeometry, Guid excludeAnmeldelseId)
        {
            //var anmeldelser = Read().Where(x => x.Jord.Dokumentation.FirstOrDefault() != null).Where(x => x.Oprindelsessted.Geom.Intersects(dbGeometry) || (x.Oprindelsessted.Matrikel.FirstOrDefault(m => m.Geom.Intersects(dbGeometry)) != null));
            var anmeldelser = _anmeldelserRepository.Search( x => (x.Oprindelsessted.Geom.Intersects(dbGeometry) && !x.Oprindelsessted.Geom.Touches(dbGeometry)) ||
                                                          (x.Oprindelsessted.Matrikel.FirstOrDefault(m => m.Geom.Intersects(dbGeometry) && !m.Geom.Touches(dbGeometry)) != null )

            );

            IList<Dokumentation> dokumentationer = new List<Dokumentation>();

            try
            {
                foreach (var anmeldelse in anmeldelser)
                {
                    //Historiske dokumenter fra reviderede anmeldelser skal ikke medtages
                    //Dokumenter på anmeldelsen som vises skal ikke vises i den historiske liste af dokumenter.
                    if (!anmeldelse.RevisionAfAnmeldelse.HasValue & anmeldelse.Id != excludeAnmeldelseId)
                    {
                        foreach (var dokumentation in anmeldelse.Jord.Dokumentation)
                        {
                            dokumentationer.Add(dokumentation);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }

            return dokumentationer;
        }

        public bool AfslutAnmeldelse(Guid anmeldelseId, Person udfoertAfPerson, IEnumerable<Dokumentation> analyseDocs = null)
        {
            var a = Read(anmeldelseId);
            if (a != null)
            {
               a.StatusAnmeldelse = _statusAnmeldelseBusiness.Read(a.Id);
                var s = _statusAnmeldelseBusiness.CreateStatus(EnumStatusAnmeldelse.Afsluttet, udfoertAfPerson);
                a.StatusAnmeldelse.Add(s);

                if (analyseDocs != null) {
                    //Hvis der er tilføjet analysedokumenter, så skal anmeldes markeres så den kommer på kræver handlinglisten i den interne applikation.
                    a.FlagSagsbehandler = DateTime.Now;

                    foreach (var doc in analyseDocs)
                        a.Jord.Dokumentation.Add(doc);
                }

                Create(a);

                _pdfBusiness.CreateAnmeldelseBlanket(a.Id);

                //SetAnmeldelseStatus(anmeldelseId, udfoertAfPerson, EnumStatusAnmeldelse.Afsluttet);

                var isCaseWorker = false;
                if (udfoertAfPerson != null)
                    isCaseWorker = udfoertAfPerson.Sagsbehandler != null;

                _adviseringBusiness.SendAfsluttetAnmeldelse(a, isCaseWorker);
                return true;
            }

            return false;
        }

        public bool DokumentationPaakraevet(Jordforureningsopslag jordforureningsopslag, Guid jordklassifikationFraJordforureningOpslag,
                                            Guid jordklassifikationValgAfBruger, decimal? forventetJordmængde, int kommunenr)
        {
            if (jordforureningsopslag != null && jordklassifikationFraJordforureningOpslag != Guid.Empty && jordklassifikationValgAfBruger != Guid.Empty && forventetJordmængde.HasValue)
            {

                //Hvis ikke brugeren har lavet nogle ændringer.
                if (jordklassifikationFraJordforureningOpslag == jordklassifikationValgAfBruger)
                    return false;

                var jkFraOpslag = _kodelisteBusiness.ReadJordKlassifikationType(jordklassifikationFraJordforureningOpslag);
                var jkFraBruger = _kodelisteBusiness.ReadJordKlassifikationType(jordklassifikationValgAfBruger);

                //Opklassificering
                if (jkFraOpslag.Priotering > jkFraBruger.Priotering) //Mindst prioteringstal er den værste klassificering.
                {
                    return false;
                }

                //Nedklassificering
                if (kommunenr == 751) //Århus Kommune
                {
                    //Nedklassificering fra OMK Analysepligt til kat 2. Der må ikke være V2,V1 eller KommunesMiljødatabase.
                    if (
                      jordforureningsopslag.V2 == false &&
                      jordforureningsopslag.V1 == false &&
                      string.IsNullOrEmpty(jordforureningsopslag.KommunensMiljoeDb) &&
                      jordforureningsopslag.OmkAnalysepligt &&
                      jkFraBruger.Kode == (short)EnumJordklassifikation.Kategori2)
                    {
                        if (forventetJordmængde <= 18)
                        {
                            return false;
                        }
                    }
                }
                else //Andre kommuner.
                {
                    //Nedklassificering fra OMK Analysepligt til kat 2
                    if (
                      jordforureningsopslag.V2 == false &&
                      jordforureningsopslag.V1 == false &&
                      string.IsNullOrEmpty(jordforureningsopslag.KommunensMiljoeDb) &&
                      jordforureningsopslag.OmkAnalysepligt &&
                      jkFraBruger.Kode == (short)EnumJordklassifikation.Kategori2)
                    {
                        if (forventetJordmængde <= 1)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            return false;
        }

        public void DeleteDokumentation(Anmeldelse anmeldelse)
        {
            if (anmeldelse != null && anmeldelse.Jord != null && anmeldelse.Jord.Dokumentation != null && anmeldelse.Jord.Dokumentation.Any())
            {
                var listOfGuids = anmeldelse.Jord.Dokumentation.Select(x => x.Id).ToList();
                foreach (var id in listOfGuids)
                {
                    var doc = _dokumentationBusiness.Read(id);
                    if (doc != null)
                        _dokumentationBusiness.Delete(doc);
                }
                //OBS der laves ikke SaveChanges /commit, da interssanterne slettes i forbindelse med oprettelse af en anmeldelse.
            }
        }

        public void DeleteInteressanter(Anmeldelse anmeldelse)
        {
            if (anmeldelse != null && anmeldelse.Interesant != null && anmeldelse.Interesant.Any())
            {
                var listOfGuids = anmeldelse.Interesant.Select(x => x.Id).ToList();
                foreach (var id in listOfGuids)
                {
                    var interesant = _interesantRepository.Read(id);
                    if (interesant != null)
                        _interesantRepository.Delete(interesant);
                }
                //OBS der laves ikke SaveChanges /commit, da interssanterne slettes i forbindelse med oprettelse af en anmeldelse.
            }
        }

        public void SetAnmeldelseStatus(Guid anmeldelseId, Person udfoertAfPerson, EnumStatusAnmeldelse status)
        {
            var anmeldelse = _anmeldelserRepository.Read(anmeldelseId);
            if (anmeldelse != null)
            {
                anmeldelse.StatusAnmeldelse = _statusAnmeldelseBusiness.Read(anmeldelse.Id);
                var s = _statusAnmeldelseBusiness.CreateStatus(status, udfoertAfPerson);
                anmeldelse.StatusAnmeldelse.Add(s);
                Create(anmeldelse);

                /*if (status == EnumStatusAnmeldelse.AfvistAfKommunen)
                {
                    //Start PDF genering af blanket
                    var startTime = DateTime.Now;
                    _pdfBusiness.CreateAnmeldelseBlanket(anmeldelseId);
                    var timeUsed = DateTime.Now - startTime;
                    Logger.LogInfo("PDF service call time: " + timeUsed.TotalSeconds + " seconds.");
                }*/
            }
        }

        public List<FaktureringUdtraek> GetGebyrFaktureringUdtraekListe(Guid kommuneId, int skip, int take)
        {
            return _faktureringRepository
                .Search(x => x.Gebyrer.Any(y => y.Anmeldelse.KommuneId == kommuneId))
                .OrderByDescending(x => x.OprettetDato)
                .Skip(skip)
                .Take(take)
                .ToList();
        }

        public FaktureringUdtraek OpretAnmeldelseGebyrFaktureringUdtraek(IEnumerable<Guid> anmeldelseIdListe, DateTime tidspunkt, Guid sagsbehandlerId, string navn, DateTime skaeringsdato)
        {
            var sagsbehandler = _brugereBusiness.ReadSagsbehandler(sagsbehandlerId);
            var anmeldelser = _anmeldelserRepository
                .Search(x => anmeldelseIdListe.Contains(x.Id) && x.Gebyr != null && x.Gebyr.FrigivetTilFakturering && EntityFunctions.TruncateTime(x.Gebyr.FrigivetTilFaktureringDato) <= skaeringsdato.Date)
                .ToList();
            var udtraek = new FaktureringUdtraek
            {
                Id = Guid.NewGuid(),
                OprettetDato = tidspunkt,
                Navn = navn,
                Skaeringsdato = skaeringsdato.Date,
                OprettetAfSagsbehandler = sagsbehandler
            };
            if (anmeldelser.Any())
            {
                foreach (var anmeldelse in anmeldelser)
                    anmeldelse.Gebyr.FaktureringUdtraek = udtraek;
            }
            SaveChanges();
            return udtraek;
        }

        public FaktureringUdtraek RedigerAnmeldelseGebyrFaktureringUdtraek(Guid id, string navn)
        {
            var udtraek = GetGebyrFaktureringUdtraek(id);
            udtraek.Navn = navn;
            SaveChanges();
            return udtraek;
        }

        public void OverfoerAnmeldelseGebyrFaktureringUdtraek(Guid faktureringUdtraekId, DateTime tidspunkt, Guid sagsbehandlerId)
        {
            var sagsbehandler = _brugereBusiness.ReadSagsbehandler(sagsbehandlerId);
            var faktureringUdtraek = _faktureringRepository.Read(faktureringUdtraekId);
            if (faktureringUdtraek != null)
            {
                faktureringUdtraek.OverfoertDato = tidspunkt;
                faktureringUdtraek.OverfoertAfSagsbehandler = sagsbehandler;
            }
            SaveChanges();
        }

        public void SletAnmeldelseGebyrFaktureringUdtraek(Guid faktureringUdtraekId)
        {
            var faktureringUdtraek = _faktureringRepository.Read(faktureringUdtraekId);
            if (faktureringUdtraek != null)
                _faktureringRepository.Delete(faktureringUdtraek);
            SaveChanges();
        }

        public bool CheckAutoKommuneGodkendAnmeldelse(Anmeldelse anmeldelse, Kommune kommune)
        {
            // Se om kommunen overskriver en eventuel autogodkendelse:
            if (anmeldelse.ModtagerAnlaeg.OphaevAutoGodkendAnmKom)
                return false;

            //Jord fra anden kommune til et FJ modtageranlæg, så skal den sættes autogodkendt af kommunen
            //TOK: Er pr. 23.5.2016 flyttet op som det første der tjekkes (da der ikke foretages forureningsopslag på offentlig vej)
            if (anmeldelse.ModtagerAnlaeg.KommuneKode != null &&
                  anmeldelse.Kommune.Kommunenr != anmeldelse.ModtagerAnlaeg.KommuneKode && !anmeldelse.Kommune.Aktiv &&
                  anmeldelse.ModtagerAnlaeg.AnvenderJF)
            {
                return true;
            }

            //Tjek om forholdene er i orden for at systemet kan godkende anmeldelsen
            if (anmeldelse.Jordforureningsopslag == null | kommune == null)
                return false;

            if (!String.IsNullOrEmpty(anmeldelse.BemaerkningTilKommune) ||
                !String.IsNullOrEmpty(anmeldelse.BemaerkningTilAnmeldelse) ||
                !String.IsNullOrEmpty(anmeldelse.BemaerkningTilJordmodtager) ||
                !String.IsNullOrEmpty(anmeldelse.Jord.TidligereErhvervsaktivitet) ||
                !String.IsNullOrEmpty(anmeldelse.Jord.JordarbejdeBeskrivelse) ||
                !String.IsNullOrEmpty(anmeldelse.Jord.Bemaerkning)) //Jira 457
                return false;

            //HVIS Materiale-/jordtype er valgt på 'Ejendom' eller 'OffentligVej' så skal der manual godkendelse til!
            if (anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType != null &&
                (anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType.Kode ==
                 (short)EnumOprindelsesstedKlassifikationType.Ejendom ||
                 anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType.Kode ==
                 (short)EnumOprindelsesstedKlassifikationType.OffentligVej) && anmeldelse.Oprindelsessted.AndenOprindJordTypeID != null)
            {
                return false;
            }

          //Hvis Jorden kommer fra offentlig vej må den ikke godkendes automatisk
            //Oprindelsested - Anden oprindelse
            else if ((anmeldelse != null && anmeldelse.Oprindelsessted != null && anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType != null &&
              anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType.Kode == (short)EnumOprindelsesstedKlassifikationType.OffentligVej))
            {
                return false;
            }

            //Intakt jord og nedklassificering 
            //Hvis jorden stammer fra et OMK-ejendom (lige meget hvilken kategori) og er angivet som intakt jord kan den intakte jord bortskaffe som ren jord. Det kræver dog en tilladelse fra anvisningsmyndigheden jf. 1.5 
            if (anmeldelse.Jord != null && anmeldelse.Jord.IntaktJord
              && anmeldelse.Jordforureningsopslag != null
              )
            {
                if (anmeldelse.Jordforureningsopslag.JordKlassifikationType == null && anmeldelse.Jordforureningsopslag.JordKlassifikationTypeId != null && anmeldelse.Jordforureningsopslag.JordKlassifikationTypeId != Guid.Empty)
                {
                    //Vi henter jordklassifikationen fra databasen.
                    anmeldelse.Jordforureningsopslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(anmeldelse.Jordforureningsopslag.JordKlassifikationTypeId);
                    if (anmeldelse.Jordforureningsopslag.JordKlassifikationType == null)
                    {
                        //Der er noget galt
                        return false;
                    }
                }

                if (anmeldelse.Jordforureningsopslag.OmkAnalysepligt | anmeldelse.Jordforureningsopslag.OmkLet | anmeldelse.Jordforureningsopslag.OmkRen)
                {
                    //Det er en OMK ejendom
                    var jkFraOpslag = _kodelisteBusiness.ReadJordKlassifikationType(anmeldelse.Jordforureningsopslag.JordKlassifikationType.Id);
                    var jkFraBruger = _kodelisteBusiness.ReadJordKlassifikationType(anmeldelse.Jord.JordKlassifikationType.Id);

                    if (jkFraOpslag.Priotering < jkFraBruger.Priotering) //Mindst prioteringstal er den værste klassificering.
                    {
                        //Jorden er blevet nedklassificeret
                        return false;
                    }
                }
            }

            //Oprindelsested - Anden oprindelse
            if (anmeldelse != null && anmeldelse.Oprindelsessted != null && anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType != null &&
              anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType.Kode == (short)EnumOprindelsesstedKlassifikationType.AndenOprindelse)
            {
                return false;
            }

            //Kommuner som ikke anvender Flytjord
            if (!kommune.Aktiv) //En kommune som ikke anvender FlytJord
            {
                //Hvis der er et link til anmeldelse i andet system
                if (anmeldelse.Jord != null && !string.IsNullOrEmpty(anmeldelse.Jord.LinkTilGodkendtAnmeldelse))
                    return true;
                //Hvis der er vedhæftet et dokumnt af typen godkendt anmeldelse
                if (anmeldelse.Jord != null && anmeldelse.Jord.Dokumentation != null && anmeldelse.Jord.Dokumentation.Any())
                {
                    var res = anmeldelse.Jord.Dokumentation.FirstOrDefault(d => d.DokumentationType.Kode == (short)EnumDokumentationType.AnmeldelseAndenKommune);
                    if (res != null) return true;
                }

                //Hvis der ikke er anmelderpligt så kan systemet sætte anmelderstatusen til godkendt af kommunen
                if (anmeldelse.Jordforureningsopslag != null)
                {
                    if (anmeldelse.Jordforureningsopslag.OmkAnalysepligt | anmeldelse.Jordforureningsopslag.OmkLet |
                        anmeldelse.Jordforureningsopslag.OmkRen | anmeldelse.Jordforureningsopslag.V1 |
                        anmeldelse.Jordforureningsopslag.V2)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            //ÅRHUS REGLER
            if (kommune.Kommunenr == 751)
            {
                //1.6 Godkendelse fra kommunen Ved AKUT jordflytning. Se supplerende diagram på GUIEkstern
                //Regelsæt 1.6 er vigtigere end 1.5
                if (anmeldelse.Jord != null && anmeldelse.Jord.JordflytningType != null && anmeldelse.Jord.JordflytningType.Kode == (short)EnumJordflytningType.Akut)
                {
                    return true;
                }

                //1.7. Godkendelse fra kommunen når forureningsgraden dokumenteres OG forureningsgraden nedgraderes
                //Tjek på om Jordklassifikationen er blevet ændret
                if (anmeldelse.Jord != null && anmeldelse.Jord.JordKlassifikationType != null && anmeldelse.Jordforureningsopslag.JordKlassifikationType != null
                    && anmeldelse.Jord.JordKlassifikationType.Id != anmeldelse.Jordforureningsopslag.JordKlassifikationType.Id)
                {
                    var jkFraOpslag = _kodelisteBusiness.ReadJordKlassifikationType(anmeldelse.Jordforureningsopslag.JordKlassifikationType.Id);
                    var jkFraBruger = _kodelisteBusiness.ReadJordKlassifikationType(anmeldelse.Jord.JordKlassifikationType.Id);


                    if (jkFraOpslag.Priotering > jkFraBruger.Priotering) //Mindst prioteringstal er den værste klassificering.
                    {
                        //Opklassificering - Alle anmeldelser hvor jorden opklassificeres skal behandles manuelt af kommunen.
                        return false;
                    }

                    //Nedklassificering
                    if (anmeldelse.Jord.Dokumentation != null && anmeldelse.Jord.Dokumentation.Count > 0)
                    {
                        //V2, V1, Geoenviron, Omk analysepligt, Omk let
                        if (anmeldelse.Jordforureningsopslag.V2 | anmeldelse.Jordforureningsopslag.V1 | !string.IsNullOrEmpty(anmeldelse.Jordforureningsopslag.KommunensMiljoeDb))
                            return false;

                        if (anmeldelse.Jordforureningsopslag.OmkAnalysepligt) //Her har vi undtagelsen
                        {
                            if (jkFraBruger.Kode == (short)EnumJordklassifikation.Kategori2 && anmeldelse.Jord.ForventetJordmaengdeTon <= 18)
                            {
                                return true;
                            }
                            return false; //Hvis nedklassificering til Kategori 1 jord (ren jord), Opklassificering sker før dette.
                        }

                        if (anmeldelse.Jordforureningsopslag.OmkLet) //Her kan jorden kun nedklassificeres til Kategori 1 jord som er ren jord. Her er mængde ligegyldig.
                        {
                            return false;
                        }

                        //Omk ren
                        if (anmeldelse.Jordforureningsopslag.OmkRen) //Burde ikke kunne nedklassificere OMKren da det er Kategori 1, men pyt.
                        {
                            return true;
                        }

                        //Ingen hits i jordopslaget (Negativ). //Burde ikke kunne nedklassificere Negativ da det er Kategori 1, men pyt.
                        return true;
                    }
                    //Vi går videre med at tjekke 1.5 eller 1.8 hvis 1.7 ikke returnere noget.
                }


                //Regel 1.5 Godkendelse forureningsgraden ikke dokumenteres
                //Regel 1.8. Godkendelse fra kommunen når forureningsgraden dokumenteres OG forureningsgraden IKKE nedgraderes.
                //Reglerne 1.5 og 1.8 er de samme derfor laves der ikke tjek på om der er vedhæftet dokumentation eller ej. 
                //Istedet for håndteres regelsæt 1.7 før 1.5 og 1.8

                if (anmeldelse.Jord != null)
                {
                    //V2, V1, Geoenviron
                    if (anmeldelse.Jordforureningsopslag.V2 | anmeldelse.Jordforureningsopslag.V1 | !string.IsNullOrEmpty(anmeldelse.Jordforureningsopslag.KommunensMiljoeDb))
                        return false;

                    //Omk Analysepligt
                    if (anmeldelse.Jordforureningsopslag.OmkAnalysepligt)
                    {
                        if (anmeldelse.Jord.ForventetJordmaengdeTon.HasValue && anmeldelse.Jord.ForventetJordmaengdeTon.Value <= 18)
                        {
                            return true;
                        }
                        return false;
                    }

                    //Omk Let
                    if (anmeldelse.Jordforureningsopslag.OmkLet)
                    {
                        if (anmeldelse.Jord.ForventetJordmaengdeTon.HasValue && anmeldelse.Jord.ForventetJordmaengdeTon.Value <= 150)
                        {
                            return true;
                        }
                        return false;
                    }

                    //Omk ren
                    if (anmeldelse.Jordforureningsopslag.OmkRen)
                    {
                        if (anmeldelse.Jord.ForventetJordmaengdeTon.HasValue && anmeldelse.Jord.ForventetJordmaengdeTon.Value <= 150)
                        {
                            return true;
                        }
                        return false;
                    }

                    //Ingen hits i jordopslaget (Negativ)
                    return true;

                }

            }
            else if (kommune.Kommunenr == 746) //Skanderborg kommune
            {
                //1.6 Godkendelse fra kommunen Ved AKUT jordflytning. Se supplerende diagram på GUIEkstern
                //Regelsæt 1.6 er vigtigere end 1.5
                if (anmeldelse.Jord != null && anmeldelse.Jord.JordflytningType != null &&
                    anmeldelse.Jord.JordflytningType.Kode == (short)EnumJordflytningType.Akut)
                {
                    return true;
                }

                //1.7. Godkendelse fra kommunen når forureningsgraden dokumenteres OG forureningsgraden nedgraderes
                //Tjek på om Jordklassifikationen er blevet ændret
                if (anmeldelse.Jord != null && anmeldelse.Jord.JordKlassifikationType != null &&
                    anmeldelse.Jordforureningsopslag.JordKlassifikationType != null
                    &&
                    anmeldelse.Jord.JordKlassifikationType.Id != anmeldelse.Jordforureningsopslag.JordKlassifikationType.Id)
                {
                    var jkFraOpslag =
                        _kodelisteBusiness.ReadJordKlassifikationType(
                            anmeldelse.Jordforureningsopslag.JordKlassifikationType.Id);
                    var jkFraBruger =
                        _kodelisteBusiness.ReadJordKlassifikationType(anmeldelse.Jord.JordKlassifikationType.Id);


                    if (jkFraOpslag.Priotering > jkFraBruger.Priotering)
                    //Mindst prioteringstal er den værste klassificering.
                    {
                        //Opklassificering - Alle anmeldelser hvor jorden opklassificeres skal behandles manuelt af kommunen.
                        return false;
                    }

                    //Nedklassificering
                    if (anmeldelse.Jord.Dokumentation != null && anmeldelse.Jord.Dokumentation.Count > 0)
                    //Hvis der er dokumentation er den nedklassificeret i dette tilfælde
                    {
                        //V2, V1, Geoenviron, Omk analysepligt, Omk let
                        if (anmeldelse.Jordforureningsopslag.V2 | anmeldelse.Jordforureningsopslag.V1 |
                            !string.IsNullOrEmpty(anmeldelse.Jordforureningsopslag.KommunensMiljoeDb))
                            return false;

                        if (anmeldelse.Jordforureningsopslag.OmkAnalysepligt) //Her har vi undtagelsen
                        {
                            if (jkFraBruger.Kode == (short)EnumJordklassifikation.Kategori2 &&
                                anmeldelse.Jord.ForventetJordmaengdeTon <= 4000)
                            {
                                return true;
                            }
                            return false;
                            //Hvis nedklassificering til Kategori 1 jord (ren jord), Opklassificering sker før dette.
                        }

                        if (anmeldelse.Jordforureningsopslag.OmkLet)
                        //Her kan jorden kun nedklassificeres til Kategori 1 jord som er ren jord. Her er mængde ligegyldig.
                        {
                            return false;
                        }

                        //Omk ren
                        if (anmeldelse.Jordforureningsopslag.OmkRen)
                        //Burde ikke kunne nedklassificere OMKren da det er Kategori 1, men pyt.
                        {
                            return true;
                        }

                        //Ingen hits i jordopslaget (Negativ). //Burde ikke kunne nedklassificere Negativ da det er Kategori 1, men pyt.
                        return true;
                    }
                    //Vi går videre med at tjekke 1.5 eller 1.8 hvis 1.7 ikke returnere noget.
                }

                //Regel 1.5 Godkendelse forureningsgraden ikke dokumenteres
                //Regel 1.8. Godkendelse fra kommunen når forureningsgraden dokumenteres OG forureningsgraden IKKE nedgraderes.
                //Reglerne 1.5 og 1.8 er de samme derfor laves der ikke tjek på om der er vedhæftet dokumentation eller ej. 
                //Istedet for håndteres regelsæt 1.7 før 1.5 og 1.8

                if (anmeldelse.Jord != null)
                {
                    //V2, V1, Geoenviron
                    if (anmeldelse.Jordforureningsopslag.V2 | anmeldelse.Jordforureningsopslag.V1 |
                        !string.IsNullOrEmpty(anmeldelse.Jordforureningsopslag.KommunensMiljoeDb))
                        return false;

                    //Omk Analysepligt
                    if (anmeldelse.Jordforureningsopslag.OmkAnalysepligt)
                    {
                        if (anmeldelse.Jord.ForventetJordmaengdeTon.HasValue &&
                            anmeldelse.Jord.ForventetJordmaengdeTon.Value <= 18)
                        {
                            return true;
                        }
                        return false;
                    }

                    //Omk Let
                    if (anmeldelse.Jordforureningsopslag.OmkLet)
                    {
                        if (anmeldelse.Jord.ForventetJordmaengdeTon.HasValue &&
                            anmeldelse.Jord.ForventetJordmaengdeTon.Value <= 150)
                        {
                            return true;
                        }
                        return false;
                    }

                    //Omk ren
                    if (anmeldelse.Jordforureningsopslag.OmkRen)
                    {
                        if (anmeldelse.Jord.ForventetJordmaengdeTon.HasValue &&
                            anmeldelse.Jord.ForventetJordmaengdeTon.Value <= 150)
                        {
                            return true;
                        }
                        return false;
                    }

                    //Ingen hits i jordopslaget (Negativ)
                    return true;
                }
            }

            return false;
        }

        public bool AutoKommuneGodkend(Anmeldelse anmeldelse, Kommune kommune)
        {
            if (anmeldelse == null)
                return false;

            //Tjekker om anmeldelsen kan godkendes automatisk
            var kanAnmeldelseGodkendesAutomatisk = CheckAutoKommuneGodkendAnmeldelse(anmeldelse, kommune);
            if (kanAnmeldelseGodkendesAutomatisk) //Jira 457
            {
                SetAnmeldelseStatus(anmeldelse.Id, null, EnumStatusAnmeldelse.GodkendtAfKommunen);
                if (anmeldelse.RevisionAfAnmeldelse == null) {
                    _adviseringBusiness.SendBeskedTilJordmodtageranlæggetsKontaktperson(anmeldelse);
                }

                return true;
            }

            //Special case Akut -->send mail
            if (anmeldelse.Jord == null || anmeldelse.Jord.JordflytningType == null || anmeldelse.Jord.JordflytningType.Kode != (short)EnumJordflytningType.Akut)
                return false;

            var komEmail = _konfigBusiness.ReadKommuneKonfig(EnumKonfigKey.AkutJordflytningEmail.ToString(), kommune.Id);
            _adviseringBusiness.SendBeskedTilKommuneVedrAkutJordflytning(anmeldelse, komEmail);
            return true;
        }

        public bool CheckAutoJordmodtagerAccepterJord(Anmeldelse anmeldelse)
        {
            if (anmeldelse != null)
            {
                if (anmeldelse.ModtagerAnlaeg != null && anmeldelse.ModtagerAnlaeg.AnvenderJF && anmeldelse.ModtagerAnlaeg.AutoGodkend)
                {
                    //// Se om kommunen overskriver en eventuel autogodkendelse:
                    //if (anmeldelse.ModtagerAnlaeg.OphaevAutoGodkendAnmKom)
                    //    return false;

                    //Henter guid fra web.config med id på Århus havns modtageranlæg. Her gælder specielle godkendelse krav.
                    var aarhusHavnModtagerAnlaegGuidListe = new List<Guid>();
                    var aarhusHavnModtagerAnlaegIds = ConfigurationManager.AppSettings["AarhusHavnModtagerAnlaegIds"];
                    var arrAarhusHavnModtagerAnlaegIds = aarhusHavnModtagerAnlaegIds.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var modtagerAnlaegId in arrAarhusHavnModtagerAnlaegIds)
                    {
                        Guid mGuid;
                        if (Guid.TryParse(modtagerAnlaegId, out mGuid))
                            aarhusHavnModtagerAnlaegGuidListe.Add(mGuid);
                    }

                    //Århus havn regelsæt 
                    if (aarhusHavnModtagerAnlaegGuidListe.Contains(anmeldelse.ModtagerAnlaeg.Id))
                    {
                        if (anmeldelse.Kommune.Kommunenr == 751) //Århus
                        {
                            //Speciel aftale mellem Århus Kommune og Århus Havn
                            //Godkender Kommune godkender Havnen!
                            return true;
                        }
                        //Hvis jorden kommer fra en anden kommune end Århus Kommune, 
                        //så skal Miljømedarbejderen behandle anmeldelsen.
                        return false;
                    }

                    //Reglsæt for øvrige modtageranlæg som anvender FlytJord og hvor Modtageranlaeg.Autogodkend er sat til 1 i databasen.

                    if (anmeldelse.Jordforureningsopslag == null)
                        return false;

                    if (anmeldelse.Jord != null)
                    {
                        //2.4 Godkendelse af jordmodtager når forureningsgraden dokumenteres OG forureningsgraden nedgraderes
                        //Tjek på om Jordklassifikationen er blevet ændret
                        if (anmeldelse.Jord != null &&
                            anmeldelse.Jord.JordKlassifikationType != null &&
                            anmeldelse.Jordforureningsopslag.JordKlassifikationType != null &&
                            anmeldelse.Jord.JordKlassifikationType.Id != anmeldelse.Jordforureningsopslag.JordKlassifikationType.Id)
                        {

                            //Opklassificering
                            var jkFraOpslag = _kodelisteBusiness.ReadJordKlassifikationType(anmeldelse.Jordforureningsopslag.JordKlassifikationType.Id);
                            var jkFraBruger = _kodelisteBusiness.ReadJordKlassifikationType(anmeldelse.Jord.JordKlassifikationType.Id);
                            if (jkFraOpslag.Priotering > jkFraBruger.Priotering) //Mindst prioteringstal er den værste klassificering.
                            {
                                //Opklassificering - Alle anmeldelser hvor jorden opklassificeres skal behandles manuelt af jordmodtagerfirmaet.
                                return false;
                            }

                            if (anmeldelse.Jord.Dokumentation.Count > 0)
                            {
                                //V2, V1, Geoenviron, Omk analysepligt, Omk let
                                if (anmeldelse.Jordforureningsopslag.V2 | anmeldelse.Jordforureningsopslag.V1
                                    | !string.IsNullOrEmpty(anmeldelse.Jordforureningsopslag.KommunensMiljoeDb)
                                    | anmeldelse.Jordforureningsopslag.OmkAnalysepligt | anmeldelse.Jordforureningsopslag.OmkLet
                                  )
                                    return false;

                                //Omk ren
                                if (anmeldelse.Jordforureningsopslag.OmkRen)
                                {
                                    return true;
                                }
                                //Ingen hits i jordopslaget (Negativ)
                                return true;
                            }
                            //Vi går videre med at tjekke 2.3 eller 2.5 hvis 2.4 ikke returnere noget.
                        }
                        //2.3 Godkendelse af jordmodtager når forureningsgraden ikke dokumenteres
                        //2.5 Godkendelse af jordmodtager når forureningsgraden dokumenteres OG forureningsgraden IKKE nedgraderes
                        if (anmeldelse.Jordforureningsopslag != null)
                        {
                            if (anmeldelse.Jordforureningsopslag.V2 | anmeldelse.Jordforureningsopslag.V1)
                            {
                                return false;
                            }
                            //Alt alt andet kan autogodkendes.
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void AutoJordmodtagerAccepterJord(Anmeldelse anmeldelse)
        {
            //Tjekker om anmeldelsen kan accepteres automatisk af jordmodtagerfirmaets milljømedarbejder.
            var kanJordmodtagerAcceptereJordenAutomatisk = CheckAutoJordmodtagerAccepterJord(anmeldelse);
            if (kanJordmodtagerAcceptereJordenAutomatisk)
                SetAnmeldelseStatus(anmeldelse.Id, null, EnumStatusAnmeldelse.JordmodtagerAcceptererJorden);
        }

        public void AutoBetalerAccepterBetaling(Anmeldelse anmeldelse)
        {
            if (anmeldelse == null || anmeldelse.ModtagerAnlaeg == null || !anmeldelse.ModtagerAnlaeg.AnvenderJF) return;

            //Betaleren skal godkende, hvis jordmodtagerfirmaet anvender FlytJord
            if (_betalerBusiness.CheckAutoBetalerAccepterBetaling(anmeldelse))
                SetAnmeldelseStatus(anmeldelse.Id, null, EnumStatusAnmeldelse.BetalerAccepteretBetalingen);
            else
            {
                var advisering = false;
                if (anmeldelse.RevisionAfAnmeldelse.HasValue)
                {
                    if (anmeldelse.Anmeldelse2.Jord.ForventetJordmaengdeTon != anmeldelse.Jord.ForventetJordmaengdeTon)
                    {
                        advisering = true;
                    }
                    else
                    {
                        SetAnmeldelseStatus(anmeldelse.Id, null, EnumStatusAnmeldelse.BetalerAccepteretBetalingen);
                    }
                }
                else
                {
                    //Advisering
                    advisering = true;
                }

                if (advisering)
                    _adviseringBusiness.SendBeskedTilBetalerAcceptereDuBetalingen(anmeldelse);
            }
        }

        public List<Anmeldelse> GetUsersAnmeldelser(Guid userId)
        {
            var jordmodtagerList = _jordmodtagerBusiness.GetJordModtagerListForUser(userId);
            var list = new List<Anmeldelse>();
            foreach (var jordmodtager in jordmodtagerList)
            {
                foreach (var modtagerAnlaeg in jordmodtager.ModtagerAnlaeg)
                {
                    var anmlist = GetAnmeldelserOnModtageAnlaeg(modtagerAnlaeg.Id);
                    list.AddRange(anmlist);
                }
            }
            return list;
        }

        public List<Anmeldelse> GetUsersAnmeldelserIncludingVognlaesStikproever(Guid userId)
        {
            var jordmodtagerList = _jordmodtagerBusiness.GetJordModtagerListForUser(userId);
            var list = new List<Anmeldelse>();
            foreach (var jordmodtager in jordmodtagerList)
            {
                foreach (var modtagerAnlaeg in jordmodtager.ModtagerAnlaeg)
                {
                    var anmlist = GetAnmeldelserOnModtageAnlaegIncludingVognlaesStikproever(modtagerAnlaeg.Id);
                    list.AddRange(anmlist);
                }
            }
            return list;
        }

        public List<Anmeldelse> GetUsersAktuelleStikproever(Guid userId)
        {/////
            var jordmodtagerList = _jordmodtagerBusiness.GetJordModtagerListForUser(userId);
            var list = new List<Anmeldelse>();
            foreach (var jordmodtager in jordmodtagerList)
            {
                foreach (var modtagerAnlaeg in jordmodtager.ModtagerAnlaeg)
                {
                    var anmlist = GetUsersAktuelleStikproeverOnModtageAnlaegIncludingVognlaesStikproever(modtagerAnlaeg.Id);
                    list.AddRange(anmlist);
                }
            }
            return list;
        }


        public IEnumerable<Anmeldelse> GetKommunesAktiveAnmeldelser(Guid kommuneId, decimal? nummer)
        {
            var anmeldList = _anmeldelserRepository.Search(x => x.KommuneId == kommuneId && x.Nummer > 0 && ((nummer != null && x.Nummer == nummer) || (nummer == null && x.Nummer > 0)) &&

           /* (!x.StatusAnmeldelse.Any(s => s.StatusAnmeldelseType.Kode == (short)EnumStatusAnmeldelse.Afsluttet | s.StatusAnmeldelseType.Kode == (short)EnumStatusAnmeldelse.RevideretAfAnmelder) //Alm anmeldeser: Der må ikke være nogen status afsluttet eller status revideret
            | //eller
            x.StatusAnmeldelse.Where(s => s.StatusAnmeldelseType.Kode == (short)EnumStatusAnmeldelse.RevideretAfAnmelder).OrderByDescending(t => t.Tid).Select(tt => tt.Tid).FirstOrDefault() >
            x.StatusAnmeldelse.Where(s => s.StatusAnmeldelseType.Kode == (short)EnumStatusAnmeldelse.Afsluttet).OrderByDescending(t => t.Tid).Select(tt => tt.Tid).FirstOrDefault()
                //hvis der findes en status revideret af anmelder som er nyere end status anmeldelse
            )*/
                /* TOK 25-05-2016: Kan sandsynligvis erstattes med dette */
            (x.StatusAnmeldelse.OrderByDescending(s => s.Tid).FirstOrDefault(s => 
                    s.StatusAnmeldelseType.Kode == (short)EnumStatusAnmeldelse.Afsluttet) == null));

            return anmeldList;
        }

        public FaktureringUdtraek GetGebyrFaktureringUdtraek(Guid id)
        {
            return _faktureringRepository.Read(id);
        }

        public IEnumerable<Anmeldelse> GetKommunesFakturerbareAnmeldelser(Guid kommuneId)
        {
            var anmeldelser = _anmeldelserRepository.Search(x => 
                x.KommuneId == kommuneId && 
                x.Gebyr != null && 
                x.Gebyr.Gebyrpligtig == true && 
                x.Gebyr.FrigivetTilFakturering && 
                x.Gebyr.FaktureringUdtraek == null
            );
            return anmeldelser;
        }

        public IEnumerable<Anmeldelse> GetKommunesAnmeldelser(Guid kommuneId)
        {
            //var anmeldList = _anmeldelserRepository.Read().Where(x => x.KommuneId == kommuneId);
            //var anmeldList = _anmeldelserRepository.ReadIncludeStatusAnmeldelse().Where(x => x.KommuneId == kommuneId);
            var anmeldList = _anmeldelserRepository.Search(x => x.KommuneId == kommuneId);

            return anmeldList;
        }

        public void CreateAlarmMaengdeKoertJord(Anmeldelse anmeldelse)
        {
            if (anmeldelse == null) return;
            var alarms = _alarmBusiness.CreateAlarmMaengdeKoertJord(anmeldelse);

            if (alarms == null || alarms.Count <= 0) return;
            if (anmeldelse.Alarm != null && anmeldelse.Alarm.Any())
            {
                //Hvis der er nogle eksisterende alarmer skal de gamle slettes før de nye knyttes til anmeldelsen.
                //Ellers har vi nogle alarmer uden nogen anmeldelser, hvilket man får FK constraint fejl.
                var alarmliste = (from x in anmeldelse.Alarm select x).ToList();
                foreach (var alarm in alarmliste)
                {
                    _alarmBusiness.DeleteAlarm(alarm);
                }
            }
            anmeldelse.Alarm = alarms;
            Create(anmeldelse);
        }

        public int AktiverAnmeldelserIfmBogholderAcceptererBetaler(Guid jordmodtagerId, Guid betalerId)
        {
            var antalAnmeldelserSomBliverAktiveret = 0;
            //Finder anmeldelser til betaler
            var anmeldelser = (from a in _anmeldelserRepository.Search(x =>
                                                                       x.Betaler != null &&
                                                                       x.Betaler.Id == betalerId &&
                                                                       x.ModtagerAnlaeg != null &&
                                                                       x.ModtagerAnlaeg.Jordmodtager != null &&
                                                                       x.ModtagerAnlaeg.Jordmodtager.Id == jordmodtagerId)
                               select a).ToList();

            foreach (var anmeldelse in anmeldelser)
            {
                if (_statusAnmeldelseBusiness.IsAnmeldelseAktiv(anmeldelse) || !_statusAnmeldelseBusiness.IsAnmeldelseReadyToBeAktiv(anmeldelse))
                    continue;

                if (AktiverAnmeldelse(anmeldelse.Id, null))
                    antalAnmeldelserSomBliverAktiveret += 1;
            }
            return antalAnmeldelserSomBliverAktiveret;
        }

        public bool AktiverAnmeldelse(Guid anmeldelsesId, Person udfoertAf)
        {
            var a = Read(anmeldelsesId);
            if (a != null)
            {
                //Tjek om den er klar til frigivelse
                var ready = _statusAnmeldelseBusiness.IsAnmeldelseReadyToBeAktiv(a);
                if (ready)
                {
                    //Hvis revideret anmeldelse, så skal original anmeldelse opdateres med værdier fra rev anmeldelse
                    if (a.RevisionAfAnmeldelse.HasValue)
                    {
                        //Merge revideret anmeldelse med oprindelig anmeldelse
                        var statusMerge = MergeRevideretAnmeldelse(a, udfoertAf);
                        if (statusMerge)
                        {
                            //Afslut revideret anmeldelse.
                            //Her skal der ikke sendes noget advis om at anmeldelsen er afsluttet
                            SetAnmeldelseStatus(a.Id, udfoertAf, EnumStatusAnmeldelse.Afsluttet);

                            //Rev. anmeldelse erstattes af den nu opdaterede oprindelige anmeldelse
                            anmeldelsesId = a.RevisionAfAnmeldelse.Value;
                            a = Read(anmeldelsesId);

                            //Sæt statusAnmeldelse til frigivet.
                            SetAnmeldelseStatus(anmeldelsesId, udfoertAf, EnumStatusAnmeldelse.AnmeldelseAktiv);

                            //Start PDF genering af blanket
                            var startTime = DateTime.Now;
                            _pdfBusiness.CreateAnmeldelseBlanket(anmeldelsesId);
                            var timeUsed = DateTime.Now - startTime;
                            Logger.LogInfo("PDF service call time: " + timeUsed.TotalSeconds + " seconds.");

                            //Send Advisering
                            _adviseringBusiness.SendAktiveretRevideretAnmeldelse(a);
                        }
                    }
                    else
                    {
                        //Sæt statusAnmeldelse til frigivet.
                        SetAnmeldelseStatus(anmeldelsesId, udfoertAf, EnumStatusAnmeldelse.AnmeldelseAktiv);

                        //Start PDF genering af blanket
                        var startTime = DateTime.Now;
                        _pdfBusiness.CreateAnmeldelseBlanket(anmeldelsesId);
                        var timeUsed = DateTime.Now - startTime;
                        Logger.LogInfo("PDF service call time: " + timeUsed.TotalSeconds + " seconds.");

                        //Send Advisering
                        _adviseringBusiness.SendAktiveretAnmeldelse(a);
                    }






                    return true;
                }
            }
            return false;
        }

        public void CheckForKoertJordAlarmer(Anmeldelse anmeldelse)
        {
            _alarmBusiness.SendKoertJordAlarmer(anmeldelse);
        }

        public Anmeldelse CloneForLogingPurpose(Anmeldelse a)
        {
            var anmeldelse = a.ShallowCopy();

            //Sted
            if (a.Oprindelsessted != null)
            {
                anmeldelse.Oprindelsessted = a.Oprindelsessted.ShallowCopy();
                if (a.Oprindelsessted.Matrikel != null && a.Oprindelsessted.Matrikel.Count > 0)
                {
                    var matrikler = new List<Matrikel>();

                    foreach (var m in a.Oprindelsessted.Matrikel)
                    {
                        var matr = m.ShallowCopy();
                        matrikler.Add(matr);
                    }
                    anmeldelse.Oprindelsessted.Matrikel = matrikler;
                }
            }

            //Jord
            if (a.Jord != null)
            {
                anmeldelse.Jord = a.Jord.ShallowCopy();
            }

            //Modtageranlæg
            if (a.ModtagerAnlaeg != null)
            {
                anmeldelse.ModtagerAnlaeg = new ModtagerAnlaeg { Id = a.ModtagerAnlaeg.Id };
            }

            //Transportoer
            if (a.Transportoer != null)
            {
                anmeldelse.Transportoer = new Transportoer { Id = a.Transportoer.Id };
            }

            //Betaler
            if (a.Betaler != null)
            {
                anmeldelse.Betaler = new Betaler { Id = a.Betaler.Id };
            }
            return anmeldelse;
        }

        public Anmeldelse ModifyRevisionAnmeldelse(Anmeldelse anmeldelse)
        {
            anmeldelse.RevisionAfAnmeldelse = new Guid(anmeldelse.Id.ToString());

            #region "Id'er fjernes"

            ////////////////////////////////////////////////////////////
            //Id'er fjernes, så anmeldelsen oprettes og ikke opdateres.
            anmeldelse.Id = Guid.Empty;

            anmeldelse.Oprindelsessted.Id = Guid.Empty;
            anmeldelse.Oprindelsessted.Anmeldelse = null;

            if (anmeldelse.Oprindelsessted.Matrikel != null)
            {

                foreach (var m in anmeldelse.Oprindelsessted.Matrikel)
                {
                    m.Id = Guid.Empty;
                    m.Oprindelsessted = null;
                }
            }

            if (anmeldelse.Jord != null)
            {
                anmeldelse.Jord.Id = Guid.Empty;
                anmeldelse.Jord.Anmeldelse = null;
            }

            //Jordforureningopslag
            if (anmeldelse.Jordforureningsopslag != null)
            {
                anmeldelse.Jordforureningsopslag.Id = Guid.Empty;
                anmeldelse.Jordforureningsopslag.Anmeldelse = null;
            }


            //Dokumentation
            if (anmeldelse.Jord != null && anmeldelse.Jord.Dokumentation != null && anmeldelse.Jord.Dokumentation.Any())
            {
                //anmeldelse.Jord.Dokumentation = new Collection<Dokumentation>();
                foreach (var dokumentation in anmeldelse.Jord.Dokumentation)
                {
                    dokumentation.Id = Guid.Empty;
                    dokumentation.Jord = null;
                    //Dokumenationen filerne flyttes senere.
                }
            }

            if (anmeldelse.Betaleringsoplysning != null && anmeldelse.Betaleringsoplysning.Any())
            {
                foreach (var betaleringsoplysning in anmeldelse.Betaleringsoplysning)
                {
                    betaleringsoplysning.Id = Guid.Empty;
                    betaleringsoplysning.Anmeldelse = null;
                }
            }

            if (anmeldelse.Interesant != null && anmeldelse.Interesant.Any())
            {
                foreach (var interesant in anmeldelse.Interesant)
                {
                    interesant.Id = Guid.Empty;
                    interesant.Anmeldelse = null;
                }
            }

            #endregion


            return anmeldelse;
        }

        public bool RemoveVognlaes(Guid anmeldelsesId, Guid vognlaesId)
        {
            var anmeldelse = Read(anmeldelsesId);
            if (anmeldelse != null)
            {
                var vognlaes = (from v in anmeldelse.Vognlaes where v.Id == vognlaesId select v).FirstOrDefault();
                if (vognlaes != null)
                {
                    if (vognlaes.Stikproeve == null)
                        return true; //vognlæsset skal ikke kunne slettes, hvis der er en stikprøve på det.

                    anmeldelse.Vognlaes.Remove(vognlaes);
                    SaveChanges();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Bogholderen afviser betaler. Denne funktion finder de aktive anmeldelser som betaler er betaler for, hvis parter som skal orienteres herom
        /// </summary>
        public IList<Anmeldelse> GetBetalersAktiveAnmeldelserHosJordmodtager(Guid betalerId, Guid jordmodtagerId)
        {
            // Der må ikke være nogle statusAnmeldelse afsluttet efter nogle eventulle statusAnmeldelse RevideretAfAnmelder.
            return _anmeldelserRepository
                .Search(x =>
                    x.Betaler != null && 
                    x.Betaler.Id == betalerId && 
                    x.ModtagerAnlaeg != null && 
                    x.ModtagerAnlaeg.Jordmodtager != null && 
                    x.ModtagerAnlaeg.Jordmodtager.Id == jordmodtagerId &&
                    x.AnmeldelseEffektivStatus.Revideret == null && 
                    x.AnmeldelseEffektivStatus.Afsluttet == null
                )
                .ToList();
        }

        /// <summary>
        /// Hvis der findes en revideret anmeldelse, som ikke er afsluttet, til denne anmeldelse.
        /// </summary>
        public Anmeldelse IsThereARevideretAnmeldelseBaseOnThisAnmeldelse(Guid parentAnmeldelseGuid)
        {
            var revAnmeldelses = _anmeldelserRepository
                .Search(a => a.RevisionAfAnmeldelse == parentAnmeldelseGuid)
                .ToList();

            foreach (var revAnmeldelse in revAnmeldelses)
            {
                if (!_statusAnmeldelseBusiness.IsAnmeldelseAfsluttet(revAnmeldelse))
                    return revAnmeldelse;
            }
            return null;
        }

        /// <summary>
        /// Afslut anmeldelser som er n antal uger efter kørselsdato
        /// </summary>
        public bool AfslutGamleAnmeldelser(int antalUger)
        {
            int antalDerBlevAfsluttet = 0;

            if (antalUger < 0) //Brugeren må ikke afslutte anmeldelser som ikke er overskredet.
                return false;


            var isOk = true;
            try
            {
                var antalDays = antalUger * 7;
                var today = DateTime.Now;
                var sletDate = today.AddDays(-antalDays);

                // find alle gamle anmeldelser
                //var list = (from an in _anmeldelserRepository.Read() where an.Jord != null && an.Jord.KoerselSlut < sletDate select an).ToList();
                var list = (from an in _anmeldelserRepository.Search(x => x.Jord != null && x.Jord.KoerselSlut < sletDate) select an).ToList();

                //Der skal en ToList på...  "When you iterate over a LINQ result, 
                //you can't make any changes until the iteration has finished" 
                //http://stackoverflow.com/questions/15363242/entity-framework-new-transaction-is-not-allowed-because-there-are-other-threads

                foreach (var anmeldelse in list)
                {

                    //if ((anmeldelse.Vognlaes == null || anmeldelse.Vognlaes.Count == 0) && anmeldelse.TotalMaengdeJordIkkeFJ == null)
                    //    continue;



                    var isAktiv = _statusAnmeldelseBusiness.IsAnmeldelseAktiv(anmeldelse);
                    if (isAktiv)
                    {
                        //Afslutter anmeldelse og sender advis
                        AfslutAnmeldelse(anmeldelse.Id, null);
                        antalDerBlevAfsluttet++;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogException("Fejl i AfslutGamleAnmeldelser(): ", e);
                isOk = false;
            }
            Logger.LogInfo(antalDerBlevAfsluttet + " anmeldelser blev auto-afsluttet.");
            return isOk;
        }

        public void IndsendAnmeldelseAutomatiskeProcedurer(Anmeldelse a, Person personSomErLoggetInd)
        {
            if (personSomErLoggetInd == null)
                return;

            //Set status
            SetAnmeldelseStatus(a.Id, personSomErLoggetInd, EnumStatusAnmeldelse.Afsendt);

            //Kalder funktionalitet som godkender anmeldelsen for sagsbehandleren, hvis forskellige regler er opfyldt.
            var autoGodkendt = AutoKommuneGodkend(a, a.Kommune);
            // Skanderborg og Mariagerfjord ønsker at der sendes mail til kommunens FJ brugere når en anmeldelse bliver indsendt af anmelder
            if (a.Kommune.Kommunenr == 746 || a.Kommune.Kommunenr == 846)
            {
                _adviseringBusiness.SendTilSagsbehandlerVedIndsendAnmeldelse(a, autoGodkendt);
            }


            //Hvis systemet på vegne af kommunen godkendte anmeldelsen, så kalder vi funktionaliteten 
            //som godkender anmeldelsen på vegne af jordmodtagerfirmaet, hvis forskellige regler er opfyldt.
            var aa = Read(a.Id);
            var lastKommuneStatusAnmeldelseType = _statusAnmeldelseBusiness.GetLastStatusTypeForKommune(aa.AnmeldelseEffektivStatus);
            if (lastKommuneStatusAnmeldelseType != EnumStatusAnmeldelse.Ukendt && lastKommuneStatusAnmeldelseType == EnumStatusAnmeldelse.GodkendtAfKommunen)
            {
                //Hvis anmelderen er bemyndiget af betaleren til at lave anmeldelser som betaleren betaler.
                //Dette tjek laves kun ved afsend af anmeldelsen. 
                AutoBetalerAccepterBetaling(aa);

                //Tjekker og udfør og jordmodtagerfirmaet kan acceptere jorden
                AutoJordmodtagerAccepterJord(aa);
            }


            var aaa = Read(a.Id);
            if (!_statusAnmeldelseBusiness.IsAnmeldelseAktiv(aaa) && _statusAnmeldelseBusiness.IsAnmeldelseReadyToBeAktiv(aaa))
            {
                //Alle parter har givet grønt lys til anmeldelsen kan sættes til aktiv og blanketten kan sendes til transportør og anmelder.
                AktiverAnmeldelse(aaa.Id, null);
            }
            else
            { 
                //TOK: Tilføjet 20.11.2017 efter ønske fra Øst kommuner til brug i forb. med flytninger til godkendte anlæg
                _pdfBusiness.CreateAnmeldelseBlanket(a.Id);
            }
        }

        public bool MergeRevideretAnmeldelse(Anmeldelse revA, Person personSomErLoggetInd)
        {
            if (revA.RevisionAfAnmeldelse.HasValue == false)
                return false;

            //Load oprindelig anmeldelse
            var oprindeligAnmeldelse = Read(revA.RevisionAfAnmeldelse.Value);
            var anmeldelseFoerMerge = CloneForLogingPurpose(oprindeligAnmeldelse);

            //Flet data - kopi funktion objekt for objekt
            //Anmeldlelse
            oprindeligAnmeldelse = AttributeCopyAnmeldelse(revA, oprindeligAnmeldelse);
            //Oprindelsested
            oprindeligAnmeldelse.Oprindelsessted = AttributeCopyOprindelsessted(revA.Oprindelsessted, oprindeligAnmeldelse.Oprindelsessted);
            //Matrikel
            oprindeligAnmeldelse.Oprindelsessted.Matrikel = new Collection<Matrikel>();
            foreach (var mi in revA.Oprindelsessted.Matrikel)
            {
                var mo = new Matrikel();
                mo = AttributeCopyMatrikel(mi, mo);
                oprindeligAnmeldelse.Oprindelsessted.Matrikel.Add(mo);
            }
            //Jord
            oprindeligAnmeldelse.Jord = AttributeCopyJord(revA.Jord, oprindeligAnmeldelse.Jord);
            //Dokumentation
            oprindeligAnmeldelse.Jord.Dokumentation = new Collection<Dokumentation>();
            foreach (var di in revA.Jord.Dokumentation)
            {
                var doko = new Dokumentation();
                doko = AttributeCopyDokumenation(di, doko);
                oprindeligAnmeldelse.Jord.Dokumentation.Add(doko);
            }

            //De fysiske filer skal flyttes over i mappen tilhørende den oprindelige anmeldelse.
            _dokumentationBusiness.CopyDokumentationFromRevAnmeldelseToOprindeligAnmeldelse(revA.Id, revA.RevisionAfAnmeldelse.Value);

            //Jordforureningskategori
            if (revA.Jordforureningsopslag != null)
            {
                var jordforurningOpslagO = new Jordforureningsopslag();
                oprindeligAnmeldelse.Jordforureningsopslag = AttributeCopyJordforureningsopslag(revA.Jordforureningsopslag, jordforurningOpslagO);
            }

            //StatusAnmeldelse - Tilføjer til de eksisterende
            foreach (var sai in revA.StatusAnmeldelse)
            {
                if (sai != null && sai.StatusAnmeldelseType != null && sai.StatusAnmeldelseType.Kode != (short)EnumStatusAnmeldelse.Afsluttet)
                {
                    //Vi må ikke overføre statusanmeldeelser af typen Afsluttet, da den oprindelige anmeldelse hermed vil bliver afsluttet. 
                    //Man kan komme ud for sådan et tilfælde, hvis betaleren acceptere betalingen for en revideret anmeldelse flere gange. http://jira.niras.dk/browse/JF-405

                    var sao = new StatusAnmeldelse();
                    oprindeligAnmeldelse.StatusAnmeldelse.Add(AttributeCopyStatusAnmeldelse(sai, sao));
                }

            }

            //Advis - Tilføjer til de eksisterende
            foreach (var ai in revA.Advis)
            {
                var ao = new Advis();
                oprindeligAnmeldelse.Advis.Add(AttributeCopyStatusAdvis(ai, ao));
            }

            //Log (incl delta) - tilføjes eksisterende poster
            foreach (var li in revA.Log)
            {
                var lo = new Log();
                oprindeligAnmeldelse.Log.Add(AttributeCopyStatusLog(li, lo));
            }

            //Status anmeldelse gem
            oprindeligAnmeldelse.StatusAnmeldelse.Add(_statusAnmeldelseBusiness.CreateStatus(EnumStatusAnmeldelse.Gemt, personSomErLoggetInd));

            //Gem oprindelig anmeldelse
            GemAnmeldelse(oprindeligAnmeldelse, personSomErLoggetInd, anmeldelseFoerMerge);

            return true;
        }

        /// <summary>
        /// Opretter en anmeldelse på baggrund af en eksisterende anmeldelse.
        /// Anvendes, når der er ændringer til en aktiv anmeldelse - En eksisterende anmeldelse bliver altså "Revideret"
        /// Den originale anmeldelse bevares og låses. Den reviderede anmeldelse behandles som en alm anmeldelse. Når den reviderede anmeldelse godkendes, merges den sammen med den orginale anmeldelse.
        /// </summary>
        /// <param name="a">Anmeldelsen som clones</param>
        /// <param name="personSomErLoggetInd"></param>
        /// <returns>Anmeldelsen som kan revideres</returns>
        public Anmeldelse CreateRevisionAfAnmeldelse(Anmeldelse a, Person personSomErLoggetInd)
        {
            if (a == null)
                return null;
            var originalAnmeldelseId = new Guid(a.Id.ToString());

            //Id'er fjernes så anmeldelse oprettes og ikke opdateres
            a = ModifyRevisionAnmeldelse(a);

            //StatusAnmeldelse
            a.StatusAnmeldelse.Add(_statusAnmeldelseBusiness.CreateStatus(EnumStatusAnmeldelse.RevideretAfAnmelder, personSomErLoggetInd));

            //Flyt dokumentation filer til person mappe
            _dokumentationBusiness.CopyDokumentationToTempFolder(personSomErLoggetInd.Id, originalAnmeldelseId);

            return a;
        }

        public void GemAnmeldelse(Anmeldelse nyanmeldelse, Person udfoertAfPerson, Anmeldelse gammelanmeldelse)
        {
            if (nyanmeldelse.Id == Guid.Empty)
            {
                //Sætter status oprettet
                var s = _statusAnmeldelseBusiness.CreateStatus(EnumStatusAnmeldelse.Oprettet, udfoertAfPerson);
                nyanmeldelse.StatusAnmeldelse.Add(s);

                //Log ændringer i anmeldelsen - Tilføj ændring i log tabel.
                //Anvende JKL's serialization metode fra Husdyr
                //http://stackoverflow.com/questions/4951233/compare-two-objects-and-find-the-differences
            }
            else
            {
                if (nyanmeldelse.StatusAnmeldelse != null && nyanmeldelse.StatusAnmeldelse.Any())
                {
                    //eksisterende statusAnmeldelser på anmeldelsen
                    var eksisterendeStatusAnmeldelser = _statusAnmeldelseBusiness.Read(nyanmeldelse.Id);

                    //Merge eksisterendeStatusAnmeldelser og nye statusAnmeldelse
                    foreach (var esa in eksisterendeStatusAnmeldelser)
                    {
                        if (!nyanmeldelse.StatusAnmeldelse.Contains(esa))
                            nyanmeldelse.StatusAnmeldelse.Add(esa);
                    }
                    var s = _statusAnmeldelseBusiness.CreateStatus(EnumStatusAnmeldelse.Gemt, udfoertAfPerson);
                    nyanmeldelse.StatusAnmeldelse.Add(s);
                }

                //Log ændringer på anmeldelsen
                var l = _logBusiness.ChangesOnAnmeldelse(gammelanmeldelse, nyanmeldelse);
                if (l != null && l.Delta != null)
                    nyanmeldelse.Log.Add(l);
            }

            Create(nyanmeldelse);
            
            if (nyanmeldelse.Id == Guid.Empty)
                return;

            var g = Guid.Empty;
            if (udfoertAfPerson != null)
                g = udfoertAfPerson.Id;
            //Flyt filer fra temp folder hvis de findes til foldernavn med anmeldelsesGuid 
            _dokumentationBusiness.MoveTempDokumentationToAnmeldelseFolder(g, nyanmeldelse.Id);
        }

        public Dokumentation AttributeCopyDokumenation(Dokumentation i, Dokumentation o)
        {
            o.DokumentationType = i.DokumentationType;
            o.Filnavn = i.Filnavn;
            o.OprindelsesDato = i.OprindelsesDato;
            return o;
        }

        public Matrikel AttributeCopyMatrikel(Matrikel i, Matrikel o)
        {
            o.Dato = i.Dato;
            o.Ejerlav = i.Ejerlav;
            o.Ejerlavsnavn = i.Ejerlavsnavn;
            o.Geom = i.Geom;
            o.Herred = i.Herred;
            o.Matrikelnr = i.Matrikelnr;
            o.Sogn = i.Sogn;

            return o;
        }

        #endregion *** Public Methods ***

        #region *** Private Methods ***

        private static Anmeldelse AttributeCopyAnmeldelse(Anmeldelse i, Anmeldelse o)
        {
            o.BemaerkningTilAnmeldelse = i.BemaerkningTilAnmeldelse;
            o.BemaerkningTilKommune = i.BemaerkningTilKommune;
            o.BemaerkningTilJordmodtager = i.BemaerkningTilJordmodtager;
            o.AarsagAfvisning = i.AarsagAfvisning;
            o.BemarkningInternKommune = i.BemarkningInternKommune;
            o.KoertJordAlarm = i.KoertJordAlarm;
            o.AnmelderSagsnummer = i.AnmelderSagsnummer;
            o.FlagSagsbehandler = i.FlagSagsbehandler;
            o.Kommune = i.Kommune;
            o.ModtagerAnlaeg = i.ModtagerAnlaeg;
            o.Transportoer = i.Transportoer;
            o.Betaler = i.Betaler;
            return o;
        }

        private static Oprindelsessted AttributeCopyOprindelsessted(Oprindelsessted i, Oprindelsessted o)
        {
            o.OprindelsesstedKlassifikationType = i.OprindelsesstedKlassifikationType;
            o.Adresse = i.Adresse;
            o.Postnummer = i.Postnummer;
            o.PostDistrikt = i.PostDistrikt;
            o.TidligereErhvervsAktivitet = i.TidligereErhvervsAktivitet;
            o.Kortlagt = i.Kortlagt;
            o.Geom = i.Geom;
            o.OffvejUrl = i.OffvejUrl;
            o.Beskrivelse = i.Beskrivelse;
            o.AndenOprindJordType = i.AndenOprindJordType;
            return o;
        }

        private static Jord AttributeCopyJord(Jord i, Jord o)
        {
            o.AffaldType = i.AffaldType;
            o.AkutBaggrund = i.AkutBaggrund;
            o.AndenAffaldType = i.AndenAffaldType;
            o.AntalProever = i.AntalProever;
            o.Bemaerkning = i.Bemaerkning;
            o.ForventetJordmaengdeTon = i.ForventetJordmaengdeTon;
            o.IntaktJord = i.IntaktJord;
            o.JordKlassifikationType = i.JordKlassifikationType;
            o.JordarbejdeBeskrivelse = i.JordarbejdeBeskrivelse;
            o.JordflytningType = i.JordflytningType;
            o.Jordproever = i.Jordproever;
            o.JordproeverFoer = i.JordproeverFoer;
            o.KoerselSlut = i.KoerselSlut;
            o.KoerselStart = i.KoerselStart;
            o.LinkTilGodkendtAnmeldelse = i.LinkTilGodkendtAnmeldelse;
            o.MiljoeTekniskTilsyn = i.MiljoeTekniskTilsyn;
            o.StraksGodkendJordhaandteringsplan = o.StraksGodkendJordhaandteringsplan;
            o.TidligereErhvervsaktivitet = i.TidligereErhvervsaktivitet;
            return o;
        }

        private static StatusAnmeldelse AttributeCopyStatusAnmeldelse(StatusAnmeldelse i, StatusAnmeldelse o)
        {
            o.Person = i.Person;
            o.StatusAnmeldelseType = i.StatusAnmeldelseType;
            o.Tid = i.Tid;
            return o;
        }

        private static Advis AttributeCopyStatusAdvis(Advis i, Advis o)
        {
            o.AdvisType = i.AdvisType;
            o.Besked = i.Besked;
            o.Person = i.Person;
            return o;
        }

        private static Log AttributeCopyStatusLog(Log i, Log o)
        {
            o.Dato = i.Dato;
            foreach (var di in i.Delta)
            {
                var deltaO = new Delta();
                o.Delta.Add(AttributeCopyStatusDelta(di, deltaO));
            }
            o.Person = i.Person;
            return o;
        }

        private static Delta AttributeCopyStatusDelta(Delta i, Delta o)
        {
            o.Efter = i.Efter;
            o.Foer = i.Foer;
            o.Navn = i.Navn;
            return o;
        }

        private static Jordforureningsopslag AttributeCopyJordforureningsopslag(Jordforureningsopslag i, Jordforureningsopslag o)
        {
            if (i != null && o != null)
            {
                o.JordKlassifikationTypeId = i.JordKlassifikationTypeId;
                o.JordKlassifikationType = i.JordKlassifikationType;
                o.KommunensMiljoeDb = i.KommunensMiljoeDb;
                o.OmkLet = i.OmkLet;
                o.OmkRen = i.OmkRen;
                o.Tid = i.Tid;
                o.V1 = i.V1;
                o.V2 = i.V2;
            }
            return o;
        }

        #endregion *** private Methods ***

    }
}
