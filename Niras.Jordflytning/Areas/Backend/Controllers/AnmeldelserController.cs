using DocumentFormat.OpenXml.Drawing.Charts;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Niras.Jordflytning.Areas.Backend.ViewModels;
using Niras.Jordflytning.Core;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.JordForurening;
using Niras.Jordflytning.Library.Logging;
using Niras.Jordflytning.ViewModels.Anmeldelse;
using Niras.Jordflytning.ViewModels.Backend;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Spatial;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Niras.Jordflytning.Areas.Backend.Controllers
{
    [Authorize]
    public class AnmeldelserController : Controller
    {
        private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Areas.Backend.Controllers.AnmeldelserController");

        #region *** Variables ***
        private const int Epsg25832 = 25832;

        private const string OprindelsesTypeAndenOprGuid = ApplicationConstants.OprindelsesTypeAndenOprGuid;
        private readonly Guid _jordFlytTypeAlmtGuid = ApplicationConstants.JordFlytTypeAlmtGuid;
        private readonly Guid _jordFlytTypeAkutGuid = ApplicationConstants.JordFlytTypeAkutGuid;
        private readonly Guid _jordFlytTypeStraksGuid = ApplicationConstants.JordFlytTypeStraksGuid;

        private readonly IAdviseringBusiness _adviseringBusiness;
        private readonly IAnalyseDokumentBusiness _analyseDokumentBusiness;
        private readonly IAnmeldelserBusiness _anmeldelserBusiness;
        private readonly IBetalerBusiness _betalerBusiness;
        private readonly IBrugereBusiness _brugerBusiness;
        private readonly IDokumentationBusiness _dokumentationBusiness;
        private readonly IDokumenterBusiness _dokumenterBusiness;
        private readonly IForureningsOpslagBusiness _forureningOpslagBusiness;
        private readonly IForureningskomponentBusiness _forureningskomponentBusiness;
        private readonly IKodelisteBusiness _kodelisteBusiness;
        private readonly IRepository<Lastbil> _lastbilRepo;
        private readonly ILogBusiness _logBusiness;
        private readonly IMatrikelBusiness _matrikelBusiness;
        private readonly IModtagerAnlaegBusiness _modtagerAnlaegBusiness;
        private readonly IPersonBusiness _personBusiness;
        private readonly IPlanlagteStikproeverBusiness _planlagteStikproeverBusiness;
        private readonly ISecurityProvider _securityProvider;
        private readonly IStatusAnmeldelseBusiness _statusAnmeldelseBusiness;
        private readonly IStatusBetalerBusiness _statusBetalerBusiness;
        private readonly IStatusStikproeveBusiness _statusStikproeveBusiness;
        private readonly IStikproeveBusiness _stikproeveBusiness;
        private readonly ITransportoerBusiness _transportoerBusiness;
        private readonly IVognlaesBusiness _vognlaesBusiness;
        private readonly IKommuneBusiness _kommuneBusiness;
        private readonly IPdfBusiness _pdfBusiness;
        private readonly IOpgavetypeBusiness _opgavetypeBusiness;
        private readonly IGebyrTidsregistreringBusiness _gebyrTidsregistreringBusiness;

        #endregion *** Variables ***

        #region *** Constructors ***

        public AnmeldelserController(
          IForureningskomponentBusiness forureningskomponentBusiness,
          IKodelisteBusiness kodelisteBusiness,
          IAnmeldelserBusiness anmeldelserBusiness,
          IModtagerAnlaegBusiness modtagerAnlaegBusiness,
          ITransportoerBusiness transportoerBusiness,
          IPersonBusiness personBusiness,
          IForureningsOpslagBusiness forureningOpslagBusiness,
          ISecurityProvider securityProvider,
          IBrugereBusiness brugerBusiness,
          IDokumentationBusiness dokumentationBusiness,
          IBetalerBusiness betalerBusiness,
          IMatrikelBusiness matrikelBusiness,
          IStatusAnmeldelseBusiness statusAnmeldelseBusiness,
          IRepository<Lastbil> lastbilRepo,
          IPlanlagteStikproeverBusiness planlagteStikproeverBusiness,
          IStatusBetalerBusiness statusBetalerBusiness,
          IStatusStikproeveBusiness statusStikproeveBusiness,
          IDokumenterBusiness dokumenterBusiness,
          IAdviseringBusiness adviseringBusiness,
          ILogBusiness logBusiness,
          IVognlaesBusiness vognlaesBusiness,
          IAnalyseDokumentBusiness analyseDokumentBusiness,
          IStikproeveBusiness stikproeveBusiness,
          IKommuneBusiness kommuneBusiness,
          IPdfBusiness pdfBusiness,
          IOpgavetypeBusiness opgavetypeBusiness,
          IGebyrTidsregistreringBusiness gebyrTidsregistreringBusiness
          )
        {
            _forureningskomponentBusiness = forureningskomponentBusiness;
            _anmeldelserBusiness = anmeldelserBusiness;
            _kodelisteBusiness = kodelisteBusiness;
            _transportoerBusiness = transportoerBusiness;
            _modtagerAnlaegBusiness = modtagerAnlaegBusiness;
            _personBusiness = personBusiness;
            _forureningOpslagBusiness = forureningOpslagBusiness;
            _securityProvider = securityProvider;
            _brugerBusiness = brugerBusiness;
            _dokumentationBusiness = dokumentationBusiness;
            _betalerBusiness = betalerBusiness;
            _matrikelBusiness = matrikelBusiness;
            _statusAnmeldelseBusiness = statusAnmeldelseBusiness;
            _lastbilRepo = lastbilRepo;
            _planlagteStikproeverBusiness = planlagteStikproeverBusiness;
            _statusBetalerBusiness = statusBetalerBusiness;
            _statusStikproeveBusiness = statusStikproeveBusiness;
            _dokumenterBusiness = dokumenterBusiness;
            _adviseringBusiness = adviseringBusiness;
            _logBusiness = logBusiness;
            _vognlaesBusiness = vognlaesBusiness;
            _analyseDokumentBusiness = analyseDokumentBusiness;
            _stikproeveBusiness = stikproeveBusiness;
            _kommuneBusiness = kommuneBusiness;
            _pdfBusiness = pdfBusiness;
            _opgavetypeBusiness = opgavetypeBusiness;
            _gebyrTidsregistreringBusiness = gebyrTidsregistreringBusiness;
        }

        #endregion *** Constructors  ***

        #region *** Init ***

        public ActionResult Besked(string anmeldelseId)
        {
            var model = new KommunikationModel();
            Guid ga;
            if (Guid.TryParse(anmeldelseId, out ga))
                model.AnmeldelseId = ga;
            return View(model);
        }

        public ActionResult Index(string anmeldelseId)
        {
            Session["Matrikler_" + anmeldelseId] = null;
            Session["docList_" + anmeldelseId] = null;
            Session["Forureningskomponenter_" + anmeldelseId] = null;
            Session["docList_" + anmeldelseId] = null;
            Session["Jordforureningsopslag_" + anmeldelseId] = null;
            Session["anmeldelse_kvittering"] = null;
            Session[$"GebyrTidsregistrering_{anmeldelseId}"] = null;

            var model = new SagsbehandlingModel();
            Guid g;
            if (Guid.TryParse(anmeldelseId, out g))
                model.AnmeldelseModel.Anmeldelse = _anmeldelserBusiness.Read(g);

            model = LoadLister(model);
            model = InitModel(model);
            ModelState.Clear();
            return View(model);
        }

        private SagsbehandlingModel InitModel(SagsbehandlingModel m)
        {
            try
            {
                if (m == null || m.AnmeldelseModel == null || m.AnmeldelseModel.Anmeldelse == null)
                    return null;

                var anmeldelseModel = m.AnmeldelseModel;
                var anmeldelse = m.AnmeldelseModel.Anmeldelse;

                //USER RIGHTS
                //Knapper
                m.KnapPlanlaegStikproeve = true;
                m.KnapAfslutAnmeldelse = true;
                m.KnapGemSynlig = true;
                m.KnapVisAnmeldelse = true;

                //Anmeldelse afsluttet
                if (_statusAnmeldelseBusiness.IsAnmeldelseAfsluttet(anmeldelse))
                {
                    //Anmeldelse er afsluttet
                    m.KnapPlanlaegStikproeve = false;
                    m.KnapAfslutAnmeldelse = false;
                    m.KnapGemSynlig = false;
                    m.RightStedAllReadonly = true;
                    m.RightJordenAllReadonly = true;
                    m.RightModtagerTransportoerAllReadonly = true;
                    m.RightBetalerAllReadonly = true;
                    m.RightKommunikationAllReadonly = true;
                    m.RightSagsbehandlingReadonly = true;
                    m.RightTilKoertJordReadonly = true;
                }
                else
                {
                    //Anmeldelse er ikke afsluttet. Dvs. readonly
                    //sagsbehandler og miljømedarbejder må rette alt på nær betaler oplysningerne. 
                    //Øvrige roller må ikke rette
                    if (_securityProvider.CurrentUser.IsInRole("Sagsbehandler") || _securityProvider.CurrentUser.IsInRole("Miljømedarbejder"))
                    {
                        m.RightStedAllReadonly = false;
                        m.RightJordenAllReadonly = false;
                        m.RightModtagerTransportoerAllReadonly = false;
                        m.RightBetalerAllReadonly = true;
                        m.RightKommunikationAllReadonly = false;
                        m.RightSagsbehandlingReadonly = false;
                    }
                    else
                    {
                        m.RightStedAllReadonly = true;
                        m.RightJordenAllReadonly = true;
                        m.RightModtagerTransportoerAllReadonly = true;
                        m.RightBetalerAllReadonly = true;
                        m.RightKommunikationAllReadonly = true;
                        m.RightSagsbehandlingReadonly = true;
                    }
                }

                var person = GetPerson();
                var personHosJordmodager = _brugerBusiness.ReadJordmodtagerPerson(anmeldelse.ModtagerAnlaeg.Jordmodtager.Id, person.Id);

                //Miljømedarbejder Hos Jordmodtager 
                //(Gælder også scenaiet hvor Århus Kommunes sagsbehandler også varetager Miljømedarbejder rollen for Århus Havn.)
                //Hvis personen findes så er miljømedarbejder hos jordmodtager.
                if (_securityProvider.CurrentUser.IsInRole("Miljømedarbejder"))
                    m.MiljømedarbejderHosJordmodtager = (personHosJordmodager != null);
                else
                    m.MiljømedarbejderHosJordmodtager = false;

                //Fanerne stikprøve og tilkørt jord skal kun være synlig for folk tilhørende Jordmodtager som anvenderJF=true
                m.RightStikproeveVognlaesVisible = personHosJordmodager != null && anmeldelseModel != null && anmeldelse.ModtagerAnlaeg != null;

                //Sagsbehandler Hos Kommunen
                //Hvis personen findes så er sagsbehandleren hos kommune.
                if (_securityProvider.CurrentUser.IsInRole("Sagsbehandler"))
                {
                    var personKommune = _brugerBusiness.ReadKommunePerson(anmeldelse.Kommune.Id, person.Id);
                    m.SagsbehandlerHosKommunen = (personKommune != null);
                }
                else
                {
                    m.SagsbehandlerHosKommunen = false;
                }

                //Revision
                if (anmeldelse.RevisionAfAnmeldelse.HasValue)
                {
                    var revA = _anmeldelserBusiness.Read(anmeldelse.RevisionAfAnmeldelse.Value);
                    m.RevisionAfAnmeldelseAdresse = revA.Oprindelsessted.Adresse + ", " + revA.Oprindelsessted.Postnummer + " " + revA.Oprindelsessted.PostDistrikt;
                }

                m = InitHeader(m);
                m.AnmeldelseModel = InitAnmeldelseModel(anmeldelseModel);
                m = InitSagsbehandlingModel(m);
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
            return m;
        }


        private SagsbehandlingModel InitHeader(SagsbehandlingModel m)
        {
            if (m.AnmeldelseModel.Anmeldelse != null)
            {
                //HEADER
                if (m.AnmeldelseModel.Anmeldelse.Anmelder != null && m.AnmeldelseModel.Anmeldelse.Anmelder.Person != null)
                {
                    m.HeaderAnmelderNavn = m.AnmeldelseModel.Anmeldelse.Anmelder.Person.Navn + " " + m.AnmeldelseModel.Anmeldelse.Anmelder.Person.Efternavn;
                    m.HeaderAnmelderPersonId = m.AnmeldelseModel.Anmeldelse.Anmelder.Person.Id;
                }

                if (m.AnmeldelseModel.Anmeldelse.Betaler != null && m.AnmeldelseModel.Anmeldelse.Betaler.Person != null)
                {
                    m.HeaderBetalerNavn = m.AnmeldelseModel.Anmeldelse.Betaler.Person.Navn + " " + m.AnmeldelseModel.Anmeldelse.Betaler.Person.Efternavn;
                    m.HeaderBetalerPersonId = m.AnmeldelseModel.Anmeldelse.Betaler.Person.Id;
                }


                if (m.AnmeldelseModel.Anmeldelse.ModtagerAnlaeg != null)
                {
                    m.HeaderModtagerAnlaegNavn = m.AnmeldelseModel.Anmeldelse.ModtagerAnlaeg.Navn;
                    m.ModtagerAnlaegBemaerking = m.AnmeldelseModel.Anmeldelse.ModtagerAnlaeg.Bemaerkning;
                }

                if (m.AnmeldelseModel.Anmeldelse.StatusAnmeldelse != null)
                {
                    var oprettet = m.AnmeldelseModel.Anmeldelse.AnmeldelseEffektivStatus.Oprettet;
                    m.HeaderIndsendt = oprettet != null ? (oprettet.Value.ToShortDateString() + " " + oprettet.Value.ToLongTimeString()) : " - ";
                }

                if (m.AnmeldelseModel.Anmeldelse.Sagsbehandler != null)
                {
                    m.HeaderSagsbehandler = m.AnmeldelseModel.Anmeldelse.Sagsbehandler.Person.Navn + " " + m.AnmeldelseModel.Anmeldelse.Sagsbehandler.Person.Efternavn;
                    m.HeaderSagsbehandlerPersonId = m.AnmeldelseModel.Anmeldelse.Sagsbehandler.Person.Id;
                }

                if (m.AnmeldelseModel.Anmeldelse != null)
                    m.HeaderLoebenr = m.AnmeldelseModel.Anmeldelse.Nummer.ToString(CultureInfo.InvariantCulture);

                if (m.AnmeldelseModel.Anmeldelse != null)
                {
                    m.HeaderTilkoertJord = m.AnmeldelseModel.Anmeldelse.JordmaengdeKoert;
                }
                else
                {
                    m.HeaderTilkoertJord = "-";
                }

                if (m.AnmeldelseModel.Anmeldelse != null && (m.AnmeldelseModel.Anmeldelse.Transportoer != null && m.AnmeldelseModel.Anmeldelse.Transportoer.Person.Firmaoplysninger != null))
                {
                    m.HeaderTransportoerNavn = m.AnmeldelseModel.Anmeldelse.Transportoer.Person.Firmaoplysninger.Firmanavn;
                    m.HeaderTransportoerPersonId = m.AnmeldelseModel.Anmeldelse.Transportoer.Person.Id;
                }
            }
            return m;
        }

        private OpretModel InitAnmeldelseModel(OpretModel opretModel)
        {
            var appSettings = ConfigurationManager.AppSettings;

            var person = GetPerson();
            var aktuelSagsbehandler = GetSagsbehandler(person.Id);
            opretModel.AktuelSagsbehandler = SagsbehandlerViewModel.Create(aktuelSagsbehandler.Person);

            opretModel.KortApiUrl = appSettings["KortApiUrl"];
            opretModel.KortPageModtagere = appSettings["KortPageModtagere"];
            opretModel.KortPageSted = appSettings["KortPageSted"];
            opretModel.KortSite = appSettings["KortSite"];

            //Sted
            opretModel.StedKommuneListe = GetAktiveKommuner(opretModel.selectedAndenOprindelsesstedKommune);
            opretModel.AndenOprindJordTypeListe = GetAndenOprindJordtyper(opretModel.SelectedAndenOprindJordTypeID);
            opretModel.AdresseOpslagViewMode = true;

            // JordTypeLister
            var ejendomJordTypeListe = new List<SelectListItem>();
            var vejJordTypeListe = new List<SelectListItem>();
            var andenJordTypeListe = new List<SelectListItem>();

            foreach (var jordType in opretModel.AndenOprindJordTypeListe)
            {
                switch (jordType.Text)
                {
                    case "Boremudder":
                        ejendomJordTypeListe.Add(jordType);
                        vejJordTypeListe.Add(jordType);
                        andenJordTypeListe.Add(jordType);
                        break;
                    case "Fejesand":
                        andenJordTypeListe.Add(jordType);
                        break;
                    case "Jord fra ledningsarbejde":
                        ejendomJordTypeListe.Add(jordType);
                        vejJordTypeListe.Add(jordType);
                        andenJordTypeListe.Add(jordType);
                        break;
                    case "Jord fra midlertidig oplag":
                        ejendomJordTypeListe.Add(jordType);
                        andenJordTypeListe.Add(jordType);
                        break;
                    case "Jord fra stik og brud":
                        ejendomJordTypeListe.Add(jordType);
                        vejJordTypeListe.Add(jordType);
                        andenJordTypeListe.Add(jordType);
                        break;
                    case "Sand fra regnvandsbrønd":
                        vejJordTypeListe.Add(jordType);
                        andenJordTypeListe.Add(jordType);
                        break;
                    case "Sandfangssand":
                        ejendomJordTypeListe.Add(jordType);
                        vejJordTypeListe.Add(jordType);
                        andenJordTypeListe.Add(jordType);
                        break;
                    case "Sediment fra regnvandsbassin":
                        ejendomJordTypeListe.Add(jordType);
                        andenJordTypeListe.Add(jordType);
                        break;
                    case "Sediment fra sø / dam":
                        ejendomJordTypeListe.Add(jordType);
                        andenJordTypeListe.Add(jordType);
                        break;
                    default:
                        break;
                }
            }

            opretModel.EjendomJordTypeListe = ejendomJordTypeListe;
            opretModel.VejJordTypeListe = vejJordTypeListe;
            opretModel.AndenJordTypeListe = andenJordTypeListe;

            //KORT
            if (opretModel.Anmeldelse != null && opretModel.Anmeldelse.Oprindelsessted != null &&
                opretModel.Anmeldelse.Oprindelsessted.Geom != null)
            {
                opretModel.OprindelsesstedWkt = opretModel.Anmeldelse.Oprindelsessted.Geom.WellKnownValue.WellKnownText;
                opretModel.MapStedLx = opretModel.GetMapBoundLowerX(opretModel.OprindelsesstedWkt);
                opretModel.MapStedLy = opretModel.GetMapBoundLowerY(opretModel.OprindelsesstedWkt);
                opretModel.MapStedUx = opretModel.GetMapBoundUpperX(opretModel.OprindelsesstedWkt);
                opretModel.MapStedUy = opretModel.GetMapBoundUpperY(opretModel.OprindelsesstedWkt);
            }

            if (opretModel.Anmeldelse != null && opretModel.Anmeldelse.Oprindelsessted != null &&
                opretModel.Anmeldelse.Oprindelsessted.Matrikel != null &&
                opretModel.Anmeldelse.Oprindelsessted.Matrikel.Count > 0)
            {
                var gc = "";
                foreach (var m in opretModel.Anmeldelse.Oprindelsessted.Matrikel)
                {
                    gc += m.Geom.WellKnownValue.WellKnownText + ",";
                }
                gc = gc.Substring(0, gc.Length - 1);
                opretModel.MatrikelWkt = "GEOMETRYCOLLECTION(" + gc + ")";

                //Der anvendes session til matrikler, da matriklerne ikke bindes noget noget i brugerfladen.
                //Session["Matrikler_" + opretModel.Anmeldelse.Id.ToString()] = opretModel.Anmeldelse.Oprindelsessted.Matrikel;
            }
            else
            {
                opretModel.MatrikelWkt = "";
            }

            //JORD
            opretModel.DokumentationTypeListe = GetAktiveDokumentationTypeLister();
            /*
          var kommune = (from k in _kodelisteBusiness.ReadAktiveKommune()
                         where k.Navn == "Aarhus"
                         select k).FirstOrDefault();
             * */
            if (opretModel.Anmeldelse != null && opretModel.Anmeldelse.Kommune != null)
            {
                opretModel.JordklassifikationTypeListe = GetJordKlassifikationType(opretModel.Anmeldelse.Kommune.Id);
                var renjordKlassifikationTypeId = (from f in _kodelisteBusiness.ReadAktiveJordKlassifikationTypesForLandsdel(opretModel.Anmeldelse.Kommune.Id)
                                                   select f).OrderByDescending(x => x.Priotering)
                                                  .FirstOrDefault();
                if (renjordKlassifikationTypeId != null)
                    opretModel.JordRenJordKlassifikationTypeId = renjordKlassifikationTypeId.Id.ToString();

                opretModel.OprindelsesKommuneAnvenderFlytJord = opretModel.Anmeldelse.Kommune.Aktiv;
            }

            if (opretModel.Anmeldelse != null)
                opretModel.IndeholderAffald = (!string.IsNullOrEmpty(opretModel.Anmeldelse.Jord.AndenAffaldType) | opretModel.Anmeldelse.Jord.AffaldType != null);

            opretModel.AffaldTypeListe = GetAktiveAffaldTyper(opretModel.selectedAffaldType);
            opretModel.JordflytningTypeListe = _kodelisteBusiness.ReadAktiveJordflytningTypes();

            if (opretModel.Anmeldelse != null && opretModel.Anmeldelse.Jord != null &&
                opretModel.Anmeldelse.Jord.Dokumentation.Count > 0)
                Session["docList_" + opretModel.Anmeldelse.Id] = opretModel.Anmeldelse.Jord.Dokumentation;

            //opretModel.ForureningOpslagJordklassifikationTypeId anvendes ved opret anmeldelse til hold værdien fra opslag fra eksterne services.
            //Denne hentes fra databasen her på backend anmeldelse
            if (opretModel.Anmeldelse != null && opretModel.Anmeldelse.Jord != null &&
                opretModel.Anmeldelse.Jord.JordKlassifikationType != null)
            {
                opretModel.ForureningOpslagJordklassifikationTypeId = opretModel.Anmeldelse.Jord.JordKlassifikationType.Id;
                opretModel.SelectedJordklassifikationType = opretModel.Anmeldelse.Jord.JordKlassifikationType.Id;
                opretModel.SelectedJordklassifikationTypeVedAndenOprindelse = opretModel.Anmeldelse.Jord.JordKlassifikationType.Id;
            }

            //ModtagerAnlaeg og Transportør
            Guid mGuid;
            if (Guid.TryParse(opretModel.SelectedModtagerAnlaegId, out mGuid))
            {
                var ml = new List<ModtagerAnlaeg>();
                var m = _modtagerAnlaegBusiness.ReadModtagerAnlaeg(mGuid);
                ml.Add(m);
                if (opretModel.Anmeldelse != null)
                    if (opretModel.Anmeldelse.Oprindelsessted != null)
                        opretModel.SelectedModtagerAnlaeg = ConvertModtagerAnlaegToModel(ml, opretModel.Anmeldelse.Oprindelsessted.Geom);
            }
            Guid tGuid;
            if (Guid.TryParse(opretModel.SelectedTransportoerId, out tGuid))
            {
                var tl = new List<Transportoer>();
                var t = _transportoerBusiness.ReadTransportoer(tGuid);
                tl.Add(t);
                opretModel.SelectedTransportoer = ConvertTransportoerToModel(tl);
            }

            //Betaler
            if (opretModel.BetalerSelectedIndex == 0 && opretModel.Anmeldelse != null &&
                opretModel.Anmeldelse.Anmelder != null)
            {
                var betaler = _betalerBusiness.ReadBetaler(opretModel.Anmeldelse.Anmelder.Id);
                var pp = new List<Person> { betaler.Person };
                var pm = ConvertPersonToModel(pp);
                opretModel.SelectedBetaler = pm;
            }
            else if (opretModel.BetalerSelectedIndex == 1)
            {
                Guid ggTrans;
                if (Guid.TryParse(opretModel.SelectedTransportoerId, out ggTrans))
                {
                    var betaler = _betalerBusiness.ReadBetaler(ggTrans);
                    var pp = new List<Person> { betaler.Person };
                    var pm = ConvertPersonToModel(pp);
                    opretModel.SelectedBetaler = pm;
                }
            }
            else if (opretModel.BetalerSelectedIndex == 2 && opretModel.Anmeldelse != null &&
                     opretModel.Anmeldelse.Betaler != null && opretModel.Anmeldelse.Betaler.Id != Guid.Empty)
            {
                var betaler = _betalerBusiness.ReadBetaler(opretModel.Anmeldelse.Betaler.Id);
                var pp = new List<Person> { betaler.Person };
                var pm = ConvertPersonToModel(pp);
                opretModel.SelectedBetaler = pm;
            }
            else
            {
                if (opretModel.Anmeldelse != null)
                {
                    var p = _personBusiness.Read(opretModel.Anmeldelse.Anmelder.Id);
                    var pp = new List<Person> { p };
                    var pm = ConvertPersonToModel(pp);
                    opretModel.SelectedBetaler = pm;
                }
                opretModel.SelectedBetalerIndex = 0;
            }

            if (opretModel.Gebyr == null)
                opretModel.Gebyr = GebyrViewModel.Create(opretModel.Anmeldelse.Gebyr);
            
            return opretModel;
        }

        private SagsbehandlingModel InitSagsbehandlingModel(SagsbehandlingModel m)
        {
            // Sæt den valgte "ukendt transportør" bruger
            string ukendtTransportoerId = null;
            try
            {
                ukendtTransportoerId = ConfigurationManager.AppSettings["UkendtTransportoerId"];
                var ukendtTransportoer = _transportoerBusiness.ReadTransportoer(Guid.Parse(ukendtTransportoerId));
                m.UkendtTransportoer = ConvertTransportoerToModel(ukendtTransportoer);
            }
            catch (Exception ex)
            {
                throw new Exception($"Kunne ikke finde den krævede \"Ukendt transportør\" bruger ud fra værdien \"{ukendtTransportoerId}\"", ex);
            }
            //Sagsbehandlere
            if (m.AnmeldelseModel != null && m.AnmeldelseModel.Anmeldelse != null &&
              m.AnmeldelseModel.Anmeldelse.Kommune != null)
            {
                var lstSagsb = new List<Person>();
                var sagsbehandlerGuid = Guid.Empty;
                if (m.AnmeldelseModel != null && m.AnmeldelseModel.Anmeldelse != null &&
                    m.AnmeldelseModel.Anmeldelse.Kommune != null)
                {
                    lstSagsb.AddRange(_brugerBusiness.ReadAktiveSagsbehandlere(m.AnmeldelseModel.Anmeldelse.Kommune.Id));
                    if (m.AnmeldelseModel.Anmeldelse.Sagsbehandler != null)
                    {
                        sagsbehandlerGuid = m.AnmeldelseModel.Anmeldelse.Sagsbehandler.Id;
                        if (!lstSagsb.Contains(m.AnmeldelseModel.Anmeldelse.Sagsbehandler.Person))
                        {
                            //Sagsbehandleren er ikke aktiv mere. Vi må hente ham igen
                            var inaktivPerson = _personBusiness.Read(m.AnmeldelseModel.Anmeldelse.Sagsbehandler.Id);
                            lstSagsb.Add(inaktivPerson);
                        }
                    }
                }

                m.SagsbehandlerListe = ConvertToSagsbehandlerModel(lstSagsb, sagsbehandlerGuid);

                if (m.AnmeldelseModel.Anmeldelse.Sagsbehandler != null)
                    m.SelectedSagsbehandlerId = m.AnmeldelseModel.Anmeldelse.Sagsbehandler.Id.ToString();
            }
            else
            {
                m.SagsbehandlerListe = new List<SelectListItem>();
            }

            //Modtageranlægoplysninger
            if (m.AnmeldelseModel != null && m.AnmeldelseModel.Anmeldelse != null &&
                m.AnmeldelseModel.Anmeldelse.ModtagerAnlaeg != null &&
                m.AnmeldelseModel.Anmeldelse.ModtagerAnlaeg.Graensevaerdier != null)
            {
                m.ModtagerAnlaegForureningskomponentListe =
                  ConvertGraenseVaerdiListModel(m.AnmeldelseModel.Anmeldelse.ModtagerAnlaeg);
            }

            if (m.AnmeldelseModel != null && m.AnmeldelseModel.Anmeldelse != null && m.AnmeldelseModel.Anmeldelse.ModtagerAnlaeg != null)
            {
                var list = _dokumenterBusiness.GetList(m.AnmeldelseModel.Anmeldelse.ModtagerAnlaeg.Id);
                m.ModtagerAnlaegDokumentListe = list.Select(d => new DokumenterViewModel
                {
                    Id = d.Id,
                    FilNavn = d.Filnavn,
                    ModtageAnlaegId = (Guid)(d.ModtagerAnlaegId != null ? d.ModtagerAnlaegId : Guid.Empty),
                    FilSti = d.Sti
                });
            }

            //Historisk dokumatation
            if (m.AnmeldelseModel != null && m.AnmeldelseModel.Anmeldelse != null && m.AnmeldelseModel.Anmeldelse.Oprindelsessted.Matrikel != null)
            {
                var historiskDokumenter = new List<HistoriskDokumentationModel>();
                foreach (var matrikel in m.AnmeldelseModel.Anmeldelse.Oprindelsessted.Matrikel)
                {
                    if (matrikel.Geom != null)
                    {
                        var dd =
                            ConvertDokumentationToModel(_anmeldelserBusiness.HentHistoriskeDokumenter(matrikel.Geom,
                                m.AnmeldelseModel.Anmeldelse.Id));

                        var analyseDokumneter = _stikproeveBusiness.HentAnalyseDokumenter(matrikel.Geom,
                            m.AnmeldelseModel.Anmeldelse.Id);
                        var ad = ConvertAnalyseDokumentToModel(analyseDokumneter);

                        historiskDokumenter.AddRange(dd);
                        historiskDokumenter.AddRange(ad);
                    }
                }
                historiskDokumenter.Sort((p1, p2) => -p1.DatoData.CompareTo(p2.DatoData));
                m.HistoriskDokumentationListe = historiskDokumenter;
            }

            //StatusAnmeldelse - Kommune
            if (m.AnmeldelseModel != null)
            {
                if (m.AnmeldelseModel.Anmeldelse != null)
                {
                    var lastKommuneStatus = _statusAnmeldelseBusiness.GetLastStatusTypeForKommune(m.AnmeldelseModel.Anmeldelse.AnmeldelseEffektivStatus);
                    if (lastKommuneStatus != EnumStatusAnmeldelse.Ukendt)
                        m.SelectedKommuneStatusKode = (int)lastKommuneStatus;
                }
            }
            m.StatusKommuneListe = ConvertToStatusAnmeldelseModel(
              _statusAnmeldelseBusiness.GetStatusTypesForKommune(), m.SelectedKommuneStatusKode);

            //StatusAnmeldelse - Jordmodtager
            if (m.AnmeldelseModel != null)
            {
                if (m.AnmeldelseModel.Anmeldelse != null)
                {
                    var lastJordmodtagerStatus = _statusAnmeldelseBusiness.GetLastStatusTypeForJordmodtager(m.AnmeldelseModel.Anmeldelse.AnmeldelseEffektivStatus);
                    if (lastJordmodtagerStatus != EnumStatusAnmeldelse.Ukendt)
                        m.SelectedJordmodtagerStatusKode = (int)lastJordmodtagerStatus;
                }
            }

            m.StatusJordmodtagerListe = ConvertToStatusAnmeldelseModel(
              _statusAnmeldelseBusiness.GetStatusTypesForJordmodtager(), m.SelectedJordmodtagerStatusKode);

            //StatusAnmeldelse - Betaler
            var lastBetalerStatus = _statusAnmeldelseBusiness.GetLastStatusTypeForBetaler(m.AnmeldelseModel.Anmeldelse.AnmeldelseEffektivStatus);
            m.SelectedBetalerStatusKode = (int)lastBetalerStatus;
            m.StatusBetalerListe = ConvertToStatusAnmeldelseModel(_statusAnmeldelseBusiness.GetStatusTypesForBetaler(), m.SelectedBetalerStatusKode);

            //VOGNLÆS
            if (m.AnmeldelseModel != null && m.AnmeldelseModel.Anmeldelse != null)
                m.VognlaesListe = ConvertVognlaesToModel(m.AnmeldelseModel.Anmeldelse.Vognlaes).OrderByDescending(v => v.DatoBom).ToList();

            //Jordforureningopslag
            m.JordforureningopslagListe = ConvertJordforureningOpsalgToModel(m.AnmeldelseModel.Anmeldelse.Jordforureningsopslag);

            var opgavetypeListe = _opgavetypeBusiness.ForKommune(m.AnmeldelseModel.Anmeldelse.KommuneId.Value)
                .OrderBy(x => x.Sortering)
                .Select(OpgavetypeViewModel.Create)
                .ToList();
            opgavetypeListe.Insert(0, new OpgavetypeViewModel { Navn = "Vælg opgavetype" });
            m.OpgavetyperListe = opgavetypeListe;
;

            return m;
        }

        /// <summary>
        /// Initaliser lister eller kodelister som kun skal gøres een gang
        /// </summary>
        private SagsbehandlingModel LoadLister(SagsbehandlingModel s)
        {

            if (s.AnmeldelseModel != null && s.AnmeldelseModel.Anmeldelse != null)
            {
                //SAGSBEHANDLING
                //Forureningskomponenter - listen anvender ajax og dataene skal først i databasen når man trykker gem.
                if (s.AnmeldelseModel.Anmeldelse.Jord.JordForureningskomponent != null)
                {
                    var fk = (from jfk in s.AnmeldelseModel.Anmeldelse.Jord.JordForureningskomponent
                              select jfk.Forureningskomponent).ToList();
                    Session["Forureningskomponenter_" + s.AnmeldelseModel.Anmeldelse.Id] = fk;
                }
                if (Session["Forureningskomponenter_" + s.AnmeldelseModel.Anmeldelse.Id] == null)
                    Session["Forureningskomponenter_" + s.AnmeldelseModel.Anmeldelse.Id] = new List<Forureningskomponent>();

                //STED 
                //Kodelister
                if (s.AnmeldelseModel.Anmeldelse.Oprindelsessted != null &&
                    s.AnmeldelseModel.Anmeldelse.Oprindelsessted.AndenOprindJordType != null &&
                    s.AnmeldelseModel.Anmeldelse.Oprindelsessted.AndenOprindJordType.Id != Guid.Empty)
                {
                    s.AnmeldelseModel.SelectedAndenOprindJordTypeID = s.AnmeldelseModel.Anmeldelse.Oprindelsessted.AndenOprindJordType.Id.ToString();
                    s.AnmeldelseModel.SelectedEjendomMaterialeTypeID =
                        s.AnmeldelseModel.Anmeldelse.Oprindelsessted.AndenOprindJordType.Id.ToString();
                    s.AnmeldelseModel.SelectedOffentligVejMaterialeTypeID =
                        s.AnmeldelseModel.Anmeldelse.Oprindelsessted.AndenOprindJordType.Id.ToString();
                }
                if (s.AnmeldelseModel.Anmeldelse.Kommune != null)
                    s.AnmeldelseModel.selectedAndenOprindelsesstedKommune = s.AnmeldelseModel.Anmeldelse.Kommune.Id.ToString();

                //JORDEN
                if (s.AnmeldelseModel.Anmeldelse.Jord != null && s.AnmeldelseModel.Anmeldelse.Jord.AffaldType != null &&
                    s.AnmeldelseModel.Anmeldelse.Jord.AffaldType.Id != Guid.Empty)
                {
                    s.AnmeldelseModel.selectedAffaldType = s.AnmeldelseModel.Anmeldelse.Jord.AffaldType.Id.ToString();
                }

                if (s.AnmeldelseModel.Anmeldelse.Jord != null && s.AnmeldelseModel.Anmeldelse.Jord.Dokumentation.Count > 0)
                {
                    Session["docList_" + s.AnmeldelseModel.Anmeldelse.Id] = s.AnmeldelseModel.Anmeldelse.Jord.Dokumentation;
                }
                // TRANSPORTØR MODTAGERANLAEG
                // TransportørId og ModtagerAnlægId anvendes ikke på model.anmeldelse.transportoer og model.anmeldelse.modtageranlaeg, 
                // da disse vil kræve at modtageranlæg og transportør skal være valgt før man kan gemme.
                // Istedet anvendes selectedTransportørId og selectedModtagerAnlaegId
                if (s.AnmeldelseModel.Anmeldelse.ModtagerAnlaeg != null && s.AnmeldelseModel.Anmeldelse.ModtagerAnlaeg.Id != Guid.Empty)
                {
                    s.AnmeldelseModel.SelectedModtagerAnlaegId = s.AnmeldelseModel.Anmeldelse.ModtagerAnlaeg.Id.ToString();
                }

                if (s.AnmeldelseModel.Anmeldelse.Transportoer != null && s.AnmeldelseModel.Anmeldelse.Transportoer.Id != Guid.Empty)
                    s.AnmeldelseModel.SelectedTransportoerId = s.AnmeldelseModel.Anmeldelse.Transportoer.Id.ToString();
                                
            }
            return s;
        }

        #endregion *** Init ***

        #region *** Convert ***

        private static IEnumerable<VognlaesModel> ConvertVognlaesToModel(ICollection<Vognlaes> vognlaeses)
        {
            var res = (from v in vognlaeses
                       select new VognlaesModel
                       {
                           Id = v.Id,
                           DatoBom = v.Dato,
                           JordmaengdeAksler = v.MaengdeAksler,
                           JordmaengdeTon = v.MaengdeTon,
                           Afvist = v.Afvist,
                           AfvistNote = v.AfvistNote,
                           Transportoer = v.Lastbil.Transportoer.Person.Firmaoplysninger != null
                            ? v.Lastbil.Transportoer.Person.Firmaoplysninger.Firmanavn + Environment.NewLine + "(" + v.Lastbil.Nummerplade + ")"
                            : "(Fejl i data: Intet firma!)"
                       }
                      );
            return res;
        }

        private static IEnumerable<JordforureningopslagModel> ConvertJordforureningOpsalgToModel(Jordforureningsopslag jfo)
        {
            var res = new List<JordforureningopslagModel>();
            if (jfo != null)
            {
                var j = new JordforureningopslagModel();
                if (jfo.JordKlassifikationType != null)
                    j.JordKlassifikationType = jfo.JordKlassifikationType.Id;
                j.KommunesMiljoeDatabase = (!string.IsNullOrEmpty(jfo.KommunensMiljoeDb));
                j.KommunesMiljoeDatabaseTekst = jfo.KommunensMiljoeDb;
                j.OmkAnalysePligt = jfo.OmkAnalysepligt;
                j.OmkLet = jfo.OmkLet;
                j.OmkRen = jfo.OmkRen;
                j.V1 = jfo.V1;
                j.V2 = jfo.V2;
                j.Tid = jfo.Tid;

                res.Add(j);
            }
            return res;
        }

        private static IEnumerable<SelectListItem> ConvertToStatusAnmeldelseModel(IList<StatusAnmeldelseType> statusAnmeldelseTypes, int selectedStatusAnmeldelseTypeKode)
        {
            var res = new List<SelectListItem>((from s in statusAnmeldelseTypes
                                                select new SelectListItem
                                                {
                                                    Selected = (s.Kode == selectedStatusAnmeldelseTypeKode),
                                                    Text = s.Navn,
                                                    Value = s.Kode.ToString(CultureInfo.InvariantCulture)
                                                }));
            res.Add(new SelectListItem { Text = @"Kræver handling", Value = "0" });
            return res.OrderBy(f => f.Value);
        }

        private static IEnumerable<SelectListItem> ConvertToSagsbehandlerModel(IList<Person> persons, Guid selectedPersonId)
        {
            var res = (from p in persons
                       select new SelectListItem
                       {
                           Selected = (selectedPersonId == p.Id),
                           Text = p.Navn + @" " + p.Efternavn,
                           Value = p.Id.ToString()
                       }).ToList();

            var valgtSagsbehandler = (from pp in persons
                                      where pp.Id == selectedPersonId
                                      select pp).FirstOrDefault();
            if (valgtSagsbehandler == null)
            {
                res.Insert(0, new SelectListItem { Text = @"Vælg sagsbehandler", Value = "0", Selected = true });
            }
            return res;
        }

        private static IEnumerable<ForureningskomponentModel> ConvertForureningkomponentsToForureningskomponenterModel(IEnumerable<Forureningskomponent> fk)
        {
            var res = (from g in fk
                       select new ForureningskomponentModel
                       {
                           Navn = g.Navn,
                           Kode = g.Kode,
                           Id = g.Id,
                       });
            return res;
        }

        private static TransportoerModel ConvertTransportoerToModel(Transportoer transportoer)
        {
            return new TransportoerModel
            {
                Firma = transportoer.Person.Firmaoplysninger != null ? transportoer.Person.Firmaoplysninger.Firmanavn : "(Fejl i data: Intet firma!)",
                Id = transportoer.Id,
                Telefon = transportoer.Person.Firmaoplysninger != null ? transportoer.Person.Telefon.ToString() : "(Fejl i data: Intet firma!)",
                LastbilMiljoeKlasserNavn =
                (String.Join(", ",
                            transportoer.Lastbil.ToList()
                            .Select(x => x.MiljoeklasseType.Navn)
                            .Distinct()
                            .OrderBy(n => n)
                            .ToList())),
                PostDistrikt = transportoer.Person.Firmaoplysninger != null ? transportoer.Person.Firmaoplysninger.Postdistikt : "(Fejl i data: Intet firma!)",
                PostNr = transportoer.Person.Firmaoplysninger != null ? transportoer.Person.Firmaoplysninger.Postnummer.ToString() : "(Fejl i data: Intet firma!)",
                Kontakt = transportoer.Person.Firmaoplysninger != null ? transportoer.Person.Navn + " " + transportoer.Person.Efternavn : "",
                Email = transportoer.Person.Email,
                Adresse = transportoer.Person.Firmaoplysninger != null ? transportoer.Person.Firmaoplysninger.Adresse : "(Fejl i data: Intet firma!)",
                Mobiltelefon = transportoer.Person.Mobiltelefon.HasValue ? transportoer.Person.Mobiltelefon.Value.ToString() : ""
            };
        }

        private static IEnumerable<TransportoerModel> ConvertTransportoerToModel(IList<Transportoer> transportoers)
        {
            return transportoers.Select(ConvertTransportoerToModel);
        }

        private static IEnumerable<BetalerModel> ConvertPersonToModel(IList<Person> persons)
        {
            var res = from p in persons
                      select new BetalerModel
                      {
                          Id = p.Id,
                          Adresse = p.Adresse,
                          CVR =
                          p.Firmaoplysninger != null
                            ? Convert.ToInt32(p.Firmaoplysninger.CVR).ToString(CultureInfo.InvariantCulture)
                            : "",
                          Firma = p.Firmaoplysninger != null ? p.Firmaoplysninger.Firmanavn : "",
                          Kontakt = p.Firmaoplysninger != null ? p.Navn + " " + p.Efternavn : "",
                          Mobil = p.Mobiltelefon.ToString(),
                          Navn = p.Navn,
                          PNr = p.Firmaoplysninger != null ? p.Firmaoplysninger.PNummer : "",
                          Postnr = p.Postnummer + " " + p.Postdistrikt,
                          Telefon = p.Telefon.ToString(),
                          Email = p.Email
                      };
            return res;
        }

        private static List<HistoriskDokumentationModel> ConvertDokumentationToModel(IList<Dokumentation> docs)
        {
            var res = (from d in docs
                       select new HistoriskDokumentationModel
                       {
                           Id = d.Id,
                           FilUrl = "Skal ikke bruges",
                           FilNavn = d.Filnavn,
                           FilNavnUrlEncoded = HttpUtility.UrlEncode(d.Filnavn),
                           Type = d.DokumentationType.Navn,
                           AnmeldelseId = d.Jord.Anmeldelse.Id,
                           StikproeveId = Guid.Empty,
                           DatoData = d.OprindelsesDato
                       }).ToList();
            return res;
        }

        private static ICollection<HistoriskDokumentationModel> ConvertAnalyseDokumentToModel(IList<AnalyseDokument> docs)
        {
            var res = (from d in docs
                       select new HistoriskDokumentationModel
                       {
                           Id = d.Id,
                           FilUrl = "Skal ikke bruges",
                           FilNavnUrlEncoded = HttpUtility.UrlEncode(d.Filnavn),
                           FilNavn = d.Filnavn,
                           AnmeldelseId = Guid.Empty,
                           Type = "Analysedokument",
                           StikproeveId = d.Stikproeve.Id,
                           DatoData = d.Dato,
                       }).ToList();
            return res;
        }

        private Anmeldelse ConvertSagsbehandlingsoplysningerToAnmeldelse(Anmeldelse a, SagsbehandlingModel sagsbehandlingModel)
        {
            //Sagsbehandler
            Guid gSagsb;
            if (Guid.TryParse(sagsbehandlingModel.SelectedSagsbehandlerId, out gSagsb))
                a.Sagsbehandler = _brugerBusiness.ReadSagsbehandler(gSagsb);

            //Forureningskomponenter
            a.Jord.JordForureningskomponent.Clear();
            var fks = _forureningskomponentBusiness.ReadAktiveForureningskomponenter();
            if (Session["Forureningskomponenter_" + a.Id] != null)
            {
                var fksession = (List<Forureningskomponent>)Session["Forureningskomponenter_" + a.Id];
                foreach (var forureningskomponent in fksession)
                {
                    var id = forureningskomponent.Id;
                    var f = (from ff in fks
                             where ff.Id == id
                             select ff).FirstOrDefault();

                    //Så er jeg sikker på jeg får objektet fra databasen med alle oplysninger og ikke det fra sessionen. Bare for en sikkerhedsskyld. KVE
                    if (f != null)
                        a.Jord.JordForureningskomponent.Add(new JordForureningskomponent { Forureningskomponent = f, Jord = a.Jord });
                }
            }

            //Bmæærkninger
            a.BemaerkningTilAnmeldelse = sagsbehandlingModel.AnmeldelseModel.Anmeldelse.BemaerkningTilAnmeldelse;
            a.AarsagAfvisning = sagsbehandlingModel.AnmeldelseModel.Anmeldelse.AarsagAfvisning;
            a.BemaerkningTilJordmodtager = sagsbehandlingModel.AnmeldelseModel.Anmeldelse.BemaerkningTilJordmodtager;
            a.BemaerkningTilKommune = sagsbehandlingModel.AnmeldelseModel.Anmeldelse.BemaerkningTilKommune;
            a.BemarkningInternKommune = sagsbehandlingModel.AnmeldelseModel.Anmeldelse.BemarkningInternKommune;

            return a;
        }

        private static List<GraenseVaerdierViewModel> ConvertGraenseVaerdiListModel(ModtagerAnlaeg ma)
        {
            var res = (from g in ma.Graensevaerdier.OrderBy(g => g.Forureningskomponent.Navn)
                       select new GraenseVaerdierViewModel
                       {
                           Navn = g.Forureningskomponent.Navn,
                           Kode = g.Forureningskomponent.Kode,
                           Id = g.Forureningskomponent.Id,
                           Max = g.Max,
                           EnhedNavn = g.Enhed.Navn
                       });
            return res.ToList();
        }

        private Anmeldelse ConvertAnmeldelseToAnmeldelse(Anmeldelse a, OpretModel opretModel)
        {
            if (a != null)
            {
                //OPRINDELSESTED
                a.Oprindelsessted.OprindelsesstedKlassifikationType = _kodelisteBusiness.ReadOprindelsesstedKlassifikationType(
                    opretModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType.Id);
                a.Oprindelsessted.Adresse = opretModel.Anmeldelse.Oprindelsessted.Adresse;
                a.Oprindelsessted.Postnummer = opretModel.Anmeldelse.Oprindelsessted.Postnummer;
                a.Oprindelsessted.PostDistrikt = opretModel.Anmeldelse.Oprindelsessted.PostDistrikt;
                a.Oprindelsessted.TidligereErhvervsAktivitet = opretModel.Anmeldelse.Oprindelsessted.TidligereErhvervsAktivitet;
                a.Oprindelsessted.Kortlagt = opretModel.Anmeldelse.Oprindelsessted.Kortlagt; //Bruges ikke i brugerfladen
                a.Oprindelsessted.OffvejUrl = opretModel.Anmeldelse.Oprindelsessted.OffvejUrl;

                a.Oprindelsessted.AndenOprindJordType = null;
                Guid selectedEjendomMaterialeTypeID;
                if (Guid.TryParse(opretModel.SelectedEjendomMaterialeTypeID, out selectedEjendomMaterialeTypeID))
                    a.Oprindelsessted.AndenOprindJordType = _kodelisteBusiness.ReadAndenOprindJordType(selectedEjendomMaterialeTypeID);
                Guid selectedOffentligVejMaterialeTypeID;

                if (Guid.TryParse(opretModel.SelectedOffentligVejMaterialeTypeID, out selectedOffentligVejMaterialeTypeID))
                    a.Oprindelsessted.AndenOprindJordType = _kodelisteBusiness.ReadAndenOprindJordType(selectedOffentligVejMaterialeTypeID);

                Guid selectedAndenOprindJordTypeID;
                if (Guid.TryParse(opretModel.SelectedAndenOprindJordTypeID, out selectedAndenOprindJordTypeID))
                    a.Oprindelsessted.AndenOprindJordType = _kodelisteBusiness.ReadAndenOprindJordType(selectedAndenOprindJordTypeID);

                //Hvis ejendom og offentlig vej
                if (opretModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType.Id.ToString().ToUpper() != OprindelsesTypeAndenOprGuid)
                {
                    //Hvis ikke anden oprindelse
                    a.Oprindelsessted.Beskrivelse = "";
                    a.Kommune = _kodelisteBusiness.ReadKommuneByNavn(opretModel.Anmeldelse.Kommune.Navn);
                }
                else
                {
                    //Anden oprindelses
                    Guid g;
                    if (Guid.TryParse(opretModel.selectedAndenOprindelsesstedKommune, out g))
                        a.Kommune = _kodelisteBusiness.ReadKommuneById(g);

                    a.Oprindelsessted.Adresse = "Anden oprindelse";
                    a.Oprindelsessted.Beskrivelse = opretModel.Anmeldelse.Oprindelsessted.Beskrivelse;
                    a.Oprindelsessted.Postnummer = null;
                    a.Oprindelsessted.PostDistrikt = null;
                    a.Oprindelsessted.OffvejUrl = null;
                }
                if (!string.IsNullOrEmpty(opretModel.OprindelsesstedWkt))
                {
                    var g = DbGeometry.FromText(opretModel.OprindelsesstedWkt, 25832);
                    a.Oprindelsessted.Geom = g;
                }
                else
                {
                    a.Oprindelsessted.Geom = null;
                }

                //MATRIKEL
                if (Session["Matrikler_" + opretModel.Anmeldelse.Id] != null)
                {
                    var matrikels = (ICollection<Matrikel>)Session["Matrikler_" + opretModel.Anmeldelse.Id];
                    a.Oprindelsessted.Matrikel.AddRange(matrikels);

                    Session["Matrikler_" + opretModel.Anmeldelse.Id] = null;
                }

                //Jordforureningsopslag
                if (Session["Jordforureningsopslag_" + opretModel.Anmeldelse.Id] != null)
                {
                    var jordforureningsopslag = (Jordforureningsopslag)Session["Jordforureningsopslag_" + opretModel.Anmeldelse.Id];

                    if (a.Jordforureningsopslag == null)
                    {
                        a.Jordforureningsopslag = jordforureningsopslag;
                    }
                    else
                    {
                        //a.Jordforureningsopslag.JordKlassifikationType = jordforureningsopslag.JordKlassifikationType;
                        a.Jordforureningsopslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(jordforureningsopslag.JordKlassifikationTypeId);

                        a.Jordforureningsopslag.KommunensMiljoeDb = jordforureningsopslag.KommunensMiljoeDb;
                        a.Jordforureningsopslag.OmkAnalysepligt = jordforureningsopslag.OmkAnalysepligt;
                        a.Jordforureningsopslag.OmkLet = jordforureningsopslag.OmkLet;
                        a.Jordforureningsopslag.OmkRen = jordforureningsopslag.OmkRen;
                        a.Jordforureningsopslag.Tid = jordforureningsopslag.Tid;
                        a.Jordforureningsopslag.V1 = jordforureningsopslag.V1;
                        a.Jordforureningsopslag.V2 = jordforureningsopslag.V2;
                    }

                    Session["Jordforureningsopslag_" + opretModel.Anmeldelse.Id] = null;
                }

                //JORD
                a.Jord.JordflytningType = _kodelisteBusiness.ReadJordflytningType(opretModel.Anmeldelse.Jord.JordflytningType.Id);
                if (opretModel.Anmeldelse.Jord != null && opretModel.Anmeldelse.Jord.JordKlassifikationType != null &&
                    opretModel.Anmeldelse.Jord.JordKlassifikationType.Id != Guid.Empty)
                    a.Jord.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(opretModel.Anmeldelse.Jord.JordKlassifikationType.Id);

                if (opretModel.IndeholderAffald == false)
                {
                    a.Jord.AndenAffaldType = null;
                    a.Jord.AffaldType = null;
                }
                else
                {
                    Guid g;
                    if (Guid.TryParse(opretModel.selectedAffaldType, out g))
                    {
                        if (g != Guid.Empty)
                            a.Jord.AffaldType = _kodelisteBusiness.ReadAffaldType(g);
                    }
                    a.Jord.AndenAffaldType = opretModel.Anmeldelse.Jord.AndenAffaldType;
                }

                a.Jord.Jordproever = opretModel.Anmeldelse.Jord.Jordproever;
                if (opretModel.Anmeldelse.Jord.Jordproever.HasValue && opretModel.Anmeldelse.Jord.Jordproever.Value)
                {
                    a.Jord.MiljoeTekniskTilsyn = opretModel.Anmeldelse.Jord.MiljoeTekniskTilsyn;
                    a.Jord.JordproeverFoer = opretModel.Anmeldelse.Jord.JordproeverFoer;
                    a.Jord.AntalProever = opretModel.Anmeldelse.Jord.AntalProever;
                }
                else
                {
                    a.Jord.MiljoeTekniskTilsyn = null;
                    a.Jord.JordproeverFoer = null;
                    a.Jord.AntalProever = null;
                }

                a.Jord.IntaktJord = opretModel.Anmeldelse.Jord.IntaktJord;
                a.Jord.ForventetJordmaengdeTon = opretModel.Anmeldelse.Jord.ForventetJordmaengdeTon;
                a.Jord.KoerselStart = opretModel.Anmeldelse.Jord.KoerselStart;
                a.Jord.KoerselSlut = opretModel.Anmeldelse.Jord.KoerselSlut;
                a.Jord.Bemaerkning = opretModel.Anmeldelse.Jord.Bemaerkning;
                a.Jord.JordarbejdeBeskrivelse = opretModel.Anmeldelse.Jord.JordarbejdeBeskrivelse;
                a.Jord.TidligereErhvervsaktivitet = opretModel.Anmeldelse.Jord.TidligereErhvervsaktivitet;
                a.Jord.LinkTilGodkendtAnmeldelse = opretModel.Anmeldelse.Jord.LinkTilGodkendtAnmeldelse;

                if (a.Jord.JordflytningType.Id == _jordFlytTypeAlmtGuid) //Alm
                {
                    a.Jord.AkutBaggrund = null;
                    a.Jord.StraksGodkendJordhaandteringsplan = null;
                }
                else if (a.Jord.JordflytningType.Id == _jordFlytTypeAkutGuid) //Akut
                {
                    a.Jord.AkutBaggrund = opretModel.Anmeldelse.Jord.AkutBaggrund;
                    a.Jord.StraksGodkendJordhaandteringsplan = null;
                }
                else if (a.Jord.JordflytningType.Id == _jordFlytTypeStraksGuid) //Straks jordflytning
                {
                    a.Jord.AkutBaggrund = null;
                    a.Jord.StraksGodkendJordhaandteringsplan = opretModel.Anmeldelse.Jord.StraksGodkendJordhaandteringsplan;
                }

                // DOKUMENTATION
                if (Session["docList_" + opretModel.Anmeldelse.Id] != null)
                {
                    //Delete dokumentation
                    _anmeldelserBusiness.DeleteDokumentation(a);

                    var docList = (ICollection<Dokumentation>)Session["docList_" + opretModel.Anmeldelse.Id];

                    var newDocs = new List<Dokumentation>();
                    foreach (var dokumentation in docList)
                    {
                        var d = new Dokumentation();
                        d.Filnavn = dokumentation.Filnavn;
                        d.DokumentationType = _kodelisteBusiness.ReadDokumentationType(dokumentation.DokumentationType.Id);
                        d.OprindelsesDato = dokumentation.OprindelsesDato;
                        newDocs.Add(d);
                    }
                    a.Jord.Dokumentation = newDocs;
                }

                //TRANSPORTØR
                Guid gTrans;
                if (Guid.TryParse(opretModel.SelectedTransportoerId, out gTrans))
                    a.Transportoer = _transportoerBusiness.ReadTransportoer(gTrans);
                else
                    a.Transportoer = null;

                //MODTAGERANLAEG
                Guid gMod;
                if (Guid.TryParse(opretModel.SelectedModtagerAnlaegId, out gMod))
                    a.ModtagerAnlaeg = _modtagerAnlaegBusiness.ReadModtagerAnlaeg(gMod);
                else
                    a.ModtagerAnlaeg = null;

                //BETALER
                //Oplysninger om betaler må ikke rettes af sagsbehandler eller andre interne brugere. Derfor er der ikke noget med her.

                //ANMELDER
                if (a.Anmelder.Person == null)
                    a.Anmelder.Person = _personBusiness.Read(opretModel.Anmeldelse.Anmelder.Id);


                UdfyldAnmeldelseMedGebyrAendringer(ref a, opretModel);
            }
            return a;
        }

        private void UdfyldAnmeldelseMedGebyrAendringer(ref Anmeldelse a, OpretModel opretModel)
        {
            if (opretModel.Gebyr != null)
            {
                var gebyr = opretModel.Gebyr;
                var opgavetyper = _opgavetypeBusiness.ForKommune(a.KommuneId.Value);

                if (a.Gebyr == null)
                {
                    a.Gebyr = new Gebyr();
                    a.Gebyr.Id = Guid.NewGuid();
                    gebyr.Id = a.Gebyr.Id;
                }
                a.Gebyr.Gebyrpligtig = gebyr.Gebyrpligtig;
                a.Gebyr.BeslutningsDato = gebyr.BeslutningsDato;
                if (gebyr.SagsbehandlerId != Guid.Empty)
                    a.Gebyr.Sagsbehandler = _brugerBusiness.ReadSagsbehandler(gebyr.SagsbehandlerId);
                a.Gebyr.Bemaerkning = gebyr.Bemaerkning;
                a.Gebyr.FrigivetTilFakturering = gebyr.FrigivetTilFakturering;
                a.Gebyr.FrigivetTilFaktureringDato = gebyr.FrigivetTilFaktureringDato;
                if (gebyr.FrigivetAfSagsbehandlerId != Guid.Empty)
                    a.Gebyr.FrigivetAfSagsbehandler = _brugerBusiness.ReadSagsbehandler(gebyr.FrigivetAfSagsbehandlerId);
                a.Gebyr.FaktureringBemaerkning = gebyr.FaktureringBemaerkning;

                var entries = Session[$"GebyrTidsregistrering_{a.Id}"] as List<GebyrTidsregistreringViewModel>;
                if (entries != null) // Changes recorded
                {
                    var currentEntryIds = entries.Select(y => y.Id).ToArray();

                    foreach (var entry in entries)
                    {
                        var existing = a.Gebyr.GebyrTidsregistrering.FirstOrDefault(x => x.Id == entry.Id);
                        if (existing != null)
                        {
                            existing.Sagsbehandler = _brugerBusiness.ReadSagsbehandler(entry.Medarbejder.Id);
                            existing.Opgavetype = opgavetyper.First(x => x.Id == entry.Opgavetype.Id);
                            existing.Dato = entry.Dato;
                            existing.Minutter = (int)entry.ForbrugtTid.TotalMinutes;
                            existing.Beskrivelse = entry.Beskrivelse;
                        }
                        else
                        {
                            var tidsreg = new GebyrTidsregistrering
                            {
                                Id = entry.Id,
                                Sagsbehandler = _brugerBusiness.ReadSagsbehandler(entry.Medarbejder.Id),
                                Opgavetype = opgavetyper.First(x => x.Id == entry.Opgavetype.Id),
                                Dato = entry.Dato,
                                Minutter = (int)entry.ForbrugtTid.TotalMinutes,
                                Beskrivelse = entry.Beskrivelse,
                                Gebyr = a.Gebyr
                            };
                            a.Gebyr.GebyrTidsregistrering.Add(tidsreg);
                        }
                    }

                    var deletedEntries = a.Gebyr.GebyrTidsregistrering.Where(x => !currentEntryIds.Contains(x.Id)).ToArray();

                    for (var i = 0; i < deletedEntries.Count(); i++)
                    {
                        var entry = deletedEntries[i];
                        a.Gebyr.GebyrTidsregistrering.Remove(entry);
                        _gebyrTidsregistreringBusiness.Delete(entry);
                    }
                }
                Session[$"GebyrTidsregistrering_{a.Id}"] = null;
            }
        }

        private static List<ModtagerAnlaegModel> ConvertModtagerAnlaegToModel(IList<ModtagerAnlaeg> modtagerAnlaegs, DbGeometry oprindelsessted)
        {
            var res = new List<ModtagerAnlaegModel>();
            foreach (var modtagerAnlaeg in modtagerAnlaegs)
            {
                var label =
                  modtagerAnlaeg.Navn + "<br/>" +
                  modtagerAnlaeg.Adresse + "<br/>" +
                  modtagerAnlaeg.Postnummer + " " + modtagerAnlaeg.PostDistrikt;

                var labelJordM = "";
                if (modtagerAnlaeg.Jordmodtager != null)
                {
                    labelJordM = modtagerAnlaeg.Jordmodtager.Navn + "<br/>" +
                                 modtagerAnlaeg.Jordmodtager.Adresse + "<br/>" +
                                 modtagerAnlaeg.Jordmodtager.Postnummer + " " + modtagerAnlaeg.Jordmodtager.PostDistrikt;
                }

                var tlf = (modtagerAnlaeg.KontaktpersonTlf != null ? modtagerAnlaeg.KontaktpersonTlf : 0);
                if (tlf == 0 && modtagerAnlaeg.Jordmodtager != null)
                    tlf = (modtagerAnlaeg.Jordmodtager.Telefon != null ? modtagerAnlaeg.Jordmodtager.Telefon : 0);

                var anl = new ModtagerAnlaegModel
                {
                    Affald = modtagerAnlaeg.Affald,
                    Afstand = (DistanceBeregner(oprindelsessted, modtagerAnlaeg.Geom)),
                    Forureningskategori = (modtagerAnlaeg.JordKlassifikationType != null ? modtagerAnlaeg.JordKlassifikationType.Navn : ""),
                    Id = modtagerAnlaeg.Id,
                    By = modtagerAnlaeg.PostDistrikt,
                    Postnummer = modtagerAnlaeg.Postnummer.ToString(),
                    Ejerlav = modtagerAnlaeg.Ejerlav,
                    Ejerlavsnavn = modtagerAnlaeg.Ejerlavsnavn,
                    Matrikelnr = modtagerAnlaeg.Matrikelnr,
                    EsrEjendomsnummer = modtagerAnlaeg.EsrEjendomsnr,
                    JF = modtagerAnlaeg.AnvenderJF,
                    Label = label,
                    LabelJordModtager = labelJordM,
                    JordmodtagerNavn = (modtagerAnlaeg.Jordmodtager != null ? modtagerAnlaeg.Jordmodtager.Navn : ""),
                    Navn = modtagerAnlaeg.Navn,
                    www = (LinkHelper(modtagerAnlaeg.www)),
                    JordmodtagerId = modtagerAnlaeg.JordmodtagerId,
                    KortLink = LinkHelperModtagerAnlaegKort(modtagerAnlaeg.Id, modtagerAnlaeg.Navn),
                    DocLink = " ",
                    KontaktpersonNavn = (!String.IsNullOrEmpty(modtagerAnlaeg.KontaktpersonNavn) ? modtagerAnlaeg.KontaktpersonNavn : " - "),
                    KontaktpersonTlf = tlf,
                    KontaktpersonEmail = (!String.IsNullOrEmpty(modtagerAnlaeg.KontaktpersonEmail) ? modtagerAnlaeg.KontaktpersonEmail : " - ")
                };
                res.Add(anl);
            }
            return res;
        }

        #endregion *** Convert ***

        #region *** Validate ***

        private void ValiderStedBasic(OpretModel opretModel)
        {
            //Kontrollere at input geometrien er valid
            try
            {
                if (!string.IsNullOrEmpty(opretModel.OprindelsesstedWkt))
                {
                    var g = DbGeometry.FromText(opretModel.OprindelsesstedWkt, Epsg25832);
                    if (!g.IsValid)
                    {
                        ModelState.AddModelError("Adresse", @"Sted - Det tegnede område er ikke valid.");
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("Adresse", @"Sted - Det tegnede område er ikke valid.");
            }

            //Hvis ikke ejendom og offentlig vej
            if (opretModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType.Id.ToString().ToUpper() == "480738EC-4A9D-448E-9A18-5BA481A8A3C6")
            {
                if (string.IsNullOrEmpty(opretModel.SelectedAndenOprindJordTypeID) | opretModel.SelectedAndenOprindJordTypeID == Guid.Empty.ToString())
                {
                    ModelState.AddModelError("JordforureningOpslag", @"Sted - Vælg materiale.");
                }

                if (string.IsNullOrEmpty(opretModel.selectedAndenOprindelsesstedKommune) | opretModel.selectedAndenOprindelsesstedKommune == Guid.Empty.ToString())
                {
                    ModelState.AddModelError("JordforureningOpslag", @"Sted - Vælg kommune.");
                }
                return;
            }

            //Hvis ejendom og offentlig vej
            if (string.IsNullOrEmpty(opretModel.Anmeldelse.Oprindelsessted.Adresse))
                ModelState.AddModelError("Adresse", @"Sted - Der skal angives en adresse.");


        }

        private void ValiderJord(OpretModel opretModel)
        {
            if (opretModel.Anmeldelse.Jord.JordKlassifikationType == null)
                ModelState.AddModelError("JordKlassifikation", @"Jorden - Der skal angives en forureningskategori");



            if (opretModel.Anmeldelse.Jord.Jordproever.HasValue && opretModel.Anmeldelse.Jord.Jordproever.Value)
            {
                //Hvis brugeren har angivet at der er udtaget jordprøver skal nedenstående valideres.
                if (string.IsNullOrEmpty(opretModel.Anmeldelse.Jord.MiljoeTekniskTilsyn))
                    ModelState.AddModelError("MiljoetekniskTilsyn", @"Jorden - Jordprøver er udtaget af skal udfyldes");

                if (!opretModel.Anmeldelse.Jord.AntalProever.HasValue)
                    ModelState.AddModelError("AntalProever", @"Jorden - Antal prøver skal udfyldes");
            }

            if (!opretModel.Anmeldelse.Jord.KoerselStart.HasValue)
                ModelState.AddModelError("KoerselStart", @"Jorden - Kørsel start skal udfyldes");

            if (!opretModel.Anmeldelse.Jord.KoerselSlut.HasValue)
                ModelState.AddModelError("KoerselSlut", @"Jorden - Kørsel slut skal udfyldes");

            if (!opretModel.Anmeldelse.Jord.ForventetJordmaengdeTon.HasValue)
                ModelState.AddModelError("ForventetJordmaengdeTon", @"Jorden - Forventet jordmængde skal udfyldes");

            if (opretModel.IndeholderAffald && opretModel.selectedAffaldType == "ef9f54db-0a6d-4add-ab7b-d3e08b6764d8")
                ModelState.AddModelError("JordAndenAffaldType", @"Jorden - Hvis Andet affaldstype er valgt skal Anden affaldstype udfyldes");

            if (opretModel.IndeholderAffald && opretModel.selectedAffaldType == Guid.Empty.ToString())
                ModelState.AddModelError("JordAndenAffaldType", @"Jorden - Hvis jorden indeholder affald skal type angives");



            //Jorden - Akut jordflytning
            opretModel.JordflytningTypeListe = _kodelisteBusiness.ReadAktiveJordflytningTypes();
            if (opretModel.Anmeldelse.Jord.JordflytningType.Id == opretModel.JordflytningTypeListe[1].Id &&
                string.IsNullOrEmpty(opretModel.Anmeldelse.Jord.AkutBaggrund))
                ModelState.AddModelError("JordenAkutBaggrund", @"Jorden - Baggrund for Akut jordflytning skal angives");

            //Jord fra kommuner som ikke anvender FlytJord
            //Her skal der være vedhæftet eller linket til en godkendt anmeldelse
            if (!opretModel.Anmeldelse.Kommune.Aktiv)
            {
                var godkendtAnmeldelse = false;

                if (!string.IsNullOrEmpty(opretModel.Anmeldelse.Jord.LinkTilGodkendtAnmeldelse))
                    godkendtAnmeldelse = true;

                if (Session["docList_" + opretModel.Anmeldelse.Id.ToString()] != null)
                {
                    var dl = (ICollection<Dokumentation>)Session["docList_" + opretModel.Anmeldelse.Id.ToString()];
                    var res = dl.Where(d => d.DokumentationType.Kode == (short)EnumDokumentationType.AnmeldelseAndenKommune).FirstOrDefault();
                    if (res != null)
                        godkendtAnmeldelse = true;
                }


                //For oprindeelsested Ejendom og hvis jorden ikke er anmelderpligtig, så skal der ikke være krav om at der skal vedhæftes en godkendt anmeldelse.
                if (opretModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType.Id.ToString().ToUpper() == "BBB062F1-641A-45AD-BEDD-B0C126FCDFE6"
                    && !opretModel.ForureningOpslagKraeverAnmeldelse)
                {
                    godkendtAnmeldelse = true;
                }

                if (!godkendtAnmeldelse)
                {
                    ModelState.AddModelError("JordenGodkendtAnmeldelse", @"Jorden - Der skal vedhæftes eller laves et link til en godkendt anmeldelse, når jorden kommer fra en kommune, som ikke anvender FlytJord og jorden er anmelderpligtig.");
                }
            }

        }

        private void ValiderModtagerTransportoerVedAfsend(OpretModel opretModel)
        {
            Guid gMod;
            if (!Guid.TryParse(opretModel.SelectedModtagerAnlaegId, out gMod))
            {
                ModelState.AddModelError("ModtagerAnlaeg", @"Modtager og transportør - Der skal vælges et modtageanlæg");
            }
            else
            {
                var m = _modtagerAnlaegBusiness.Read(gMod);

                //Ejendom eller vej
                if (m != null &&
                  m.JordKlassifikationType != null &&
                    //opretModel.Anmeldelse.Jord.JordKlassifikationType != null &&
                    m.JordKlassifikationType.Id != opretModel.Anmeldelse.Jord.JordKlassifikationType.Id)
                {
                    ModelState.AddModelError("ModtagerAnlaeg", @"Modtager og transportør - Modtageanlægget modtager ikke jord af den valgte forureningskategori");
                }


                //Hvis jorden kommer fra en kommune som ikke anvender flytjord, så skal den valgt modtageranlæg anvende flytjord. (Systemet skal ikke må nemlig ikke sende sådanne godkendte anmeldelser ud)
                //Dette skal laves om når der kommer flere modtageranlæg på, som ikke er i Aarhus kommune.
                if (opretModel.Anmeldelse != null && opretModel.Anmeldelse.Kommune != null && opretModel.Anmeldelse.Kommune.Aktiv == false && m != null && m.AnvenderJF == false)
                {
                    ModelState.AddModelError("ModtagerAnlaegAffald", @"Modtager og transportør - Anvender kommunen, hvor jorden stammer fra, ikke FlytJord, skal modtageanlægget anvende FlytJord");
                }

            }

            Guid gTrans;
            if (!Guid.TryParse(opretModel.SelectedTransportoerId, out gTrans))
                ModelState.AddModelError("Transportoer", @"Modtager og transportør - Der skal vælges en transportør");


        }

        private void ValiderModtagerTransportoer(OpretModel opretModel, Anmeldelse anmeldelseFoerAendringer)
        {
            //Hvis anmeldelse under revision og betaler er transportør, så må transportør ikke ændres.
            if (anmeldelseFoerAendringer != null && anmeldelseFoerAendringer.RevisionAfAnmeldelse.HasValue
              && anmeldelseFoerAendringer.Betaler != null && anmeldelseFoerAendringer.Transportoer != null)
            {
                if (anmeldelseFoerAendringer.Betaler.Id == anmeldelseFoerAendringer.Transportoer.Id)
                {
                    //Transportør er betaler på en anmeldelse under revision
                    if (opretModel != null)
                    {
                        Guid gTrans;
                        if (Guid.TryParse(opretModel.SelectedTransportoerId, out gTrans)
                          && gTrans != anmeldelseFoerAendringer.Transportoer.Id)
                        {
                            ModelState.AddModelError("TransportørRevisionBetaler", @"Modtager og transportør - Det er ikke muligt at ændre transportøren ved revision af en anmeldelse, når transportøren er betaler. I sådanne tilfælde må der oprettes en ny anmeldelse.");
                        }
                    }
                }
            }
        }

        #endregion *** Validate ***

        #region *** Save ***

        [HttpPost]
        public JsonResult GemGebyrTidsregistrering(SagsbehandlingModel sagsbehandlingModel)
        {
            bool saved = false;
            string message = null;
            ModelState.Clear();
            try
            {
                if (sagsbehandlingModel.AnmeldelseModel.Anmeldelse.Id != Guid.Empty)
                {
                    var anmeldelse = _anmeldelserBusiness.Read(sagsbehandlingModel.AnmeldelseModel.Anmeldelse.Id);
                    var person = GetPerson();
                    if (ModelState.IsValid && anmeldelse != null)
                    {
                        UdfyldAnmeldelseMedGebyrAendringer(ref anmeldelse, sagsbehandlingModel.AnmeldelseModel);
                        var gammelanmeldelse = _anmeldelserBusiness.CloneForLogingPurpose(anmeldelse);
                        _anmeldelserBusiness.GemAnmeldelse(anmeldelse, person, gammelanmeldelse);
                        saved = true;
                        message = "Oplysningerne er gemt";
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                message = "Der skete en fejl!";
            }
            return Json(new { success = saved, message = message ?? "Dataene kunne ikke gemmes!" } );
        }

        [HttpPost]
        public ActionResult Gem(SagsbehandlingModel sagsbehandlingModel, string command)
        {
            try
            {
                ModelState.Clear();
                if (sagsbehandlingModel.AnmeldelseModel.Anmeldelse.Id != Guid.Empty)
                {
                    //slet matrikler database inden de indsættes igen fra session variabel
                    //_matrikelBusiness.DeleteMatrikler(_anmeldelserBusiness.Read(sagsbehandlingModel.AnmeldelseModel.Anmeldelse.Id));

                    //Anmeldelsen findes allerede  - Anmeldelsen hentes og opdateres med værdier fra brugerfladen
                    var anmeldelse = _anmeldelserBusiness.Read(sagsbehandlingModel.AnmeldelseModel.Anmeldelse.Id);
                    var oprindeligGodkendtStatus = anmeldelse.AnmeldelseEffektivStatus.Godkendt.HasValue;
                    var oprindeligGebyrFrigivetStatus = (anmeldelse.Gebyr != null && anmeldelse.Gebyr.FrigivetTilFakturering || false);

                    var person = GetPerson();

                    //Kommune
                    Guid komId;
                    if (sagsbehandlingModel.AnmeldelseModel.OprindelsesstedKlassifikationTypeSelectedIndex != 2 && sagsbehandlingModel.AnmeldelseModel.Anmeldelse != null && sagsbehandlingModel.AnmeldelseModel.Anmeldelse.Kommune != null && !string.IsNullOrEmpty(sagsbehandlingModel.AnmeldelseModel.Anmeldelse.Kommune.Navn))
                    {
                        var kom = _kodelisteBusiness.ReadKommuneByNavn(sagsbehandlingModel.AnmeldelseModel.Anmeldelse.Kommune.Navn);
                        sagsbehandlingModel.AnmeldelseModel.Anmeldelse.Kommune = kom;
                    }
                    else if (sagsbehandlingModel.AnmeldelseModel.OprindelsesstedKlassifikationTypeSelectedIndex == 2 && sagsbehandlingModel.AnmeldelseModel.Anmeldelse != null && sagsbehandlingModel.AnmeldelseModel.selectedAndenOprindelsesstedKommune != null && Guid.TryParse(sagsbehandlingModel.AnmeldelseModel.selectedAndenOprindelsesstedKommune, out komId))
                    {
                        var kom = _kodelisteBusiness.ReadKommuneById(komId);
                        sagsbehandlingModel.AnmeldelseModel.Anmeldelse.Kommune = kom;
                        sagsbehandlingModel.AnmeldelseModel.Anmeldelse.Oprindelsessted.Adresse = "Anden oprindelse";
                    }

                    if (anmeldelse != null)
                    {
                        anmeldelse = ConvertSagsbehandlingsoplysningerToAnmeldelse(anmeldelse, sagsbehandlingModel); //Oplysninger fra sagsbehandler fanen tilføjes anmeldelsen objektet.
                        var gammelanmeldelse = _anmeldelserBusiness.CloneForLogingPurpose(anmeldelse); //Anvendes til logging, hvor gammel anmeldelse sammenlignes med ny anmeldelse
                        anmeldelse = ConvertAnmeldelseToAnmeldelse(anmeldelse, sagsbehandlingModel.AnmeldelseModel);
                        /*
                        #if DEBUG // TESTING DEBUG ONLY!
                                    if (anmeldelse.RevisionAfAnmeldelse.HasValue && sagsbehandlingModel.AnmeldelseModel.ForureningOpslagJordklassifikationTypeId == Guid.Empty)
                                    {
                                        sagsbehandlingModel.AnmeldelseModel.ForureningOpslagJordklassifikationTypeId = Guid.Parse("1426C54A-6C06-4970-AC52-327D013751C7");
                                        sagsbehandlingModel.AnmeldelseModel.Anmeldelse.Jord.JordKlassifikationType = new JordKlassifikationType();
                                        sagsbehandlingModel.AnmeldelseModel.Anmeldelse.Jord.JordKlassifikationType.Id =
                                            sagsbehandlingModel.AnmeldelseModel.ForureningOpslagJordklassifikationTypeId;
                                        sagsbehandlingModel.AnmeldelseModel.SelectedJordklassifikationType = sagsbehandlingModel.AnmeldelseModel.ForureningOpslagJordklassifikationTypeId;
                                    }
                        #endif
                        */
                        //Validering
                        ValiderStedBasic(sagsbehandlingModel.AnmeldelseModel);
                        ValiderJord(sagsbehandlingModel.AnmeldelseModel);
                        ValiderModtagerTransportoer(sagsbehandlingModel.AnmeldelseModel, gammelanmeldelse);
                        ValiderModtagerTransportoerVedAfsend(sagsbehandlingModel.AnmeldelseModel);
                        if (ModelState.IsValid)
                        {
                            //Tjek om betaler er godkendt af jordmodtagers bogholder.
                            var anmodningOmGodkendelse = _statusBetalerBusiness.AnmodOmGodkendelseHosJordmodtager(anmeldelse.Betaler, anmeldelse.ModtagerAnlaeg.Jordmodtager);
                            if (anmodningOmGodkendelse == false)
                            {
                                sagsbehandlingModel.AnmeldelseModel.ValideringList.Add("Jordmodtager har afvist betaleren!<br/>");
                                sagsbehandlingModel = InitModel(sagsbehandlingModel);
                                sagsbehandlingModel = InitSagsbehandlingModel(sagsbehandlingModel);
                                ModelState.Clear();
                                return PartialView("_BackendValidering", sagsbehandlingModel);
                            }

                            //FlagSagsbehandler
                            //Hvis anmelderen laver ændringer på anmeldelsen før anmeldelsen er godkendt af sagsbehandler, skal der sættes et flag på anmeldelsen.
                            //Flaget bliver fjernet hvis anmeldelsen gemmes fra backend brugerfladen.
                            anmeldelse.FlagSagsbehandler = null;

                            _anmeldelserBusiness.GemAnmeldelse(anmeldelse, person, gammelanmeldelse);
                            ViewBag.Message = "Oplysningerne er gemt";

                            //Status
                            var lastStatusAnmeldelseType = _statusAnmeldelseBusiness.GetLastStatusTypeForKommune(anmeldelse.AnmeldelseEffektivStatus);
                            var lastJordmodtagerStatus = _statusAnmeldelseBusiness.GetLastStatusTypeForJordmodtager(anmeldelse.AnmeldelseEffektivStatus);
                            if (anmeldelse.Id != Guid.Empty && person != null)
                            {
                                //Kommunestatus
                                if ((sagsbehandlingModel.SelectedKommuneStatusKode > 0) && (lastStatusAnmeldelseType == EnumStatusAnmeldelse.Ukendt || (int)lastStatusAnmeldelseType != sagsbehandlingModel.SelectedKommuneStatusKode))
                                {
                                    _anmeldelserBusiness.SetAnmeldelseStatus(anmeldelse.Id, person, (EnumStatusAnmeldelse)sagsbehandlingModel.SelectedKommuneStatusKode);
                                    // Ny status, genindlæs
                                    _anmeldelserBusiness.Reload(anmeldelse);

                                    if ((EnumStatusAnmeldelse)sagsbehandlingModel.SelectedKommuneStatusKode == EnumStatusAnmeldelse.AfvistAfKommunen)
                                    {
                                        //Start PDF genering af blanket
                                        var startTime = DateTime.Now;
                                        _pdfBusiness.CreateAnmeldelseBlanket(anmeldelse.Id);
                                        var timeUsed = DateTime.Now - startTime;
                                        Logger.LogInfo("PDF service call time: " + timeUsed.TotalSeconds + " seconds.");

                                        //Advisering
                                        _adviseringBusiness.SendBeskedTilAnmelderAfvistAfKommunen(anmeldelse);
                                    }
                                    if ((EnumStatusAnmeldelse)sagsbehandlingModel.SelectedKommuneStatusKode == EnumStatusAnmeldelse.GodkendtAfKommunen)
                                    {
                                        //Betaler
                                        //Når kommunen har godkendt anmeldelsen skal Betaleren i visse tilfælde adviseres om denne ønsker at betale for jordflytningen.
                                        //Hvis betaleren altså ikke allerede har gjort det i forvejen.
                                        var statusBetaler = _statusAnmeldelseBusiness.GetLastStatusTypeForBetaler(anmeldelse.AnmeldelseEffektivStatus);
                                        if (statusBetaler == EnumStatusAnmeldelse.Ukendt)
                                        {
                                            //Tjekker om betaleren godkender automatisk eller om der skal sendes advis
                                            _anmeldelserBusiness.AutoBetalerAccepterBetaling(anmeldelse);
                                            // Ny status, genindlæs
                                            anmeldelse = _anmeldelserBusiness.Read(anmeldelse.Id);
                                        }
                                        /* TOK 22.3.2018: Der skal ikke sendes ved revision. Det gøres fra SendAktiveretRevideretAnmeldelse() */
                                        if (anmeldelse.RevisionAfAnmeldelse == null)
                                        {
                                            _adviseringBusiness.SendBeskedTilJordmodtageranlæggetsKontaktperson(anmeldelse);
                                        }
                                    }
                                }

                                //Jordmodtagerstatus
                                if ((sagsbehandlingModel.SelectedJordmodtagerStatusKode > 0) && (lastJordmodtagerStatus == EnumStatusAnmeldelse.Ukendt || (int)lastJordmodtagerStatus != sagsbehandlingModel.SelectedJordmodtagerStatusKode))
                                {
                                    _anmeldelserBusiness.SetAnmeldelseStatus(anmeldelse.Id, person, (EnumStatusAnmeldelse)sagsbehandlingModel.SelectedJordmodtagerStatusKode);
                                    // Ny status, genindlæs
                                    _anmeldelserBusiness.Reload(anmeldelse);

                                    if ((EnumStatusAnmeldelse)sagsbehandlingModel.SelectedJordmodtagerStatusKode == EnumStatusAnmeldelse.JordmodtagerAfviserJorden)
                                    {
                                        //Advisering
                                        var persons = new List<Person>();
                                        persons.Add(anmeldelse.Anmelder.Person);
                                        _adviseringBusiness.SendBeskedTilAnmelderAfvistAfJordmodtager(anmeldelse);
                                    }
                                }

                                //Automatik vedr. jordmodtagerfirmaet kan godkende anmeldelsen. Hvis der ikke er valgt en Jordmodtager status og kommunen godkender anmeldelsen.
                                if (sagsbehandlingModel.SelectedKommuneStatusKode == (short)EnumStatusAnmeldelse.GodkendtAfKommunen
                                    && sagsbehandlingModel.SelectedJordmodtagerStatusKode != (short)EnumStatusAnmeldelse.JordmodtagerAfviserJorden
                                    && sagsbehandlingModel.SelectedJordmodtagerStatusKode != (short)EnumStatusAnmeldelse.JordmodtagerAcceptererJorden
                                  )
                                {
                                    _anmeldelserBusiness.AutoJordmodtagerAccepterJord(anmeldelse);
                                    // Ny status, genindlæs
                                    anmeldelse = _anmeldelserBusiness.Read(anmeldelse.Id);
                                }

                                //Automatik - Anmeldelsen frigives, hvis alle forhold er i orden
                                if (!_statusAnmeldelseBusiness.IsAnmeldelseAktiv(anmeldelse) && _statusAnmeldelseBusiness.IsAnmeldelseReadyToBeAktiv(anmeldelse))
                                {
                                    _anmeldelserBusiness.AktiverAnmeldelse(anmeldelse.Id, null);
                                    // Ny status, genindlæs
                                    _anmeldelserBusiness.Reload(anmeldelse);
                                }
                            }

                            // Midlertidigt forsøg ifbm. at genindlæse til fordel for gebyr.
                            var nuvaerendeGodkendtStatus = anmeldelse.AnmeldelseEffektivStatus.Godkendt.HasValue;
                            var nuvaerendeGebyrFrigivetStatus = (anmeldelse.Gebyr != null && anmeldelse.Gebyr.FrigivetTilFakturering || false);
                            if (oprindeligGodkendtStatus != nuvaerendeGodkendtStatus || oprindeligGebyrFrigivetStatus != nuvaerendeGebyrFrigivetStatus)
                            {
                                ViewBag.Reload = true;
                            }
                        }
                        else
                        {
                            var mesValidering =
                              ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).OrderBy(e => e).ToList();
                            sagsbehandlingModel.AnmeldelseModel.ValideringList = mesValidering;
                            ViewBag.Message = "Dataene kunne ikke gemmes!";
                        }
                        sagsbehandlingModel.AnmeldelseModel.Anmeldelse = anmeldelse;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                ViewBag.Message = "Der skete en fejl!";
            }

            sagsbehandlingModel = InitModel(sagsbehandlingModel);
            ModelState.Clear();
            return PartialView("_BackendValidering", sagsbehandlingModel);
        }

        [HttpPost]
        public ActionResult TilfoejForureningskomponent(string anmeldelseId, string forureningsKomponentTypeAHeadSearch)
        {
            if (!string.IsNullOrEmpty(anmeldelseId) && !string.IsNullOrEmpty(forureningsKomponentTypeAHeadSearch))
            {
                var fkList = GetForureningskomponents();
                var forureningskomponents = fkList as Forureningskomponent[] ?? fkList.ToArray();
                var fkmList = ConvertForureningkomponentsToForureningskomponenterModel(forureningskomponents);

                var fkm = (from f in fkmList
                           where f.DisplayName == forureningsKomponentTypeAHeadSearch
                           select f).FirstOrDefault();

                if (Session["Forureningskomponenter_" + anmeldelseId] != null)
                {
                    var fksession = (List<Forureningskomponent>)Session["Forureningskomponenter_" + anmeldelseId];
                    if (fkm != null)
                    {
                        var f = (from ff in forureningskomponents
                                 where ff.Id == fkm.Id
                                 select ff).FirstOrDefault();
                        if (f != null)
                            fksession.Add(f);
                        Session["Forureningskomponenter_" + anmeldelseId] = fksession;
                    }
                }
                return Json("true");
            }
            return null;
        }

        [HttpPost]
        public ActionResult RemoveForureningskomponent([DataSourceRequest] DataSourceRequest request, ForureningskomponentModel forureningskomponentModel, string anmeldelseId)
        {
            if (Session["Forureningskomponenter_" + anmeldelseId] != null)
            {
                var fks = (List<Forureningskomponent>)Session["Forureningskomponenter_" + anmeldelseId];

                foreach (var f in fks)
                {
                    if (f.Id != forureningskomponentModel.Id)
                        continue;

                    fks.Remove(f);
                    Session["Forureningskomponenter_" + anmeldelseId] = fks;
                    var fkm = ConvertForureningkomponentsToForureningskomponenterModel(fks);
                    var result = fkm.ToDataSourceResult(request);
                    return Json(result);
                }
            }
            return Json(null);
        }

        [HttpPost]
        public ActionResult AccepterJord(string anmeldelsesId)
        {
            Guid g;
            if (Guid.TryParse(anmeldelsesId, out g))
            {
                //Miljømedarbejder kan acceptere jorden
                var a = _anmeldelserBusiness.Read(g);
                var b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);

                if (a != null && b != null && b.Person != null)
                {
                    _anmeldelserBusiness.SetAnmeldelseStatus(g, b.Person, EnumStatusAnmeldelse.JordmodtagerAcceptererJorden);
                    return Json(true);
                }
            }
            return Json(false);
        }

        [HttpPost]
        public ActionResult AfvisJord(string anmeldelsesId)
        {
            //Miljømedarbejder kan afvis jorden
            Guid g;
            if (Guid.TryParse(anmeldelsesId, out g))
            {
                var a = _anmeldelserBusiness.Read(g);
                var b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);

                if (a != null && b != null && b.Person != null)
                {
                    _anmeldelserBusiness.SetAnmeldelseStatus(g, b.Person, EnumStatusAnmeldelse.JordmodtagerAfviserJorden);
                    return Json(true);
                }
            }
            return Json(false);
        }

        [HttpPost]
        public ActionResult TildelTilMig(string anmeldelsesId)
        {
            Guid g;
            if (Guid.TryParse(anmeldelsesId, out g))
            {
                //Tildel sagen til sagsbehandleren som er logget ind og status sættes til underbehandling, hvis den tidligere status tillader det..
                var listMessage = new List<string>();
                var statusAnmeldelseKode = 0;
                var statusAnmeldelseTekst = "";

                var a = _anmeldelserBusiness.Read(g);
                var person = GetPerson();

                if (a != null && person != null)
                {
                    //Sagsbehandler skal findes i listen af sagsbehandlere.
                    var sagsbehandler = _brugerBusiness.ReadSagsbehandler(person.Id);
                    a.Sagsbehandler = sagsbehandler;
                    _anmeldelserBusiness.Create(a);
                    listMessage.Add("Sagsbehandler er tilknyttet.");

                    //Hvis statussen ikke allerede er en "kommune" status sættes status til underbehandling
                    var lastKommuneStatus = _statusAnmeldelseBusiness.GetLastStatusTypeForKommune(a.AnmeldelseEffektivStatus);
                    if (lastKommuneStatus == EnumStatusAnmeldelse.Ukendt)
                    {
                        _anmeldelserBusiness.SetAnmeldelseStatus(g, person, EnumStatusAnmeldelse.UnderbehandlingAfKommunen);
                        listMessage.Add("Status er sat til Underbehandling af kommunen");
                        statusAnmeldelseKode = (int)EnumStatusAnmeldelse.UnderbehandlingAfKommunen;
                        statusAnmeldelseTekst = "Underbehandling af kommunen";
                    }
                    var message = string.Join("<br>", listMessage);

                    //Værdierne i return json objektet skal bruges til at tilpasse brugerfladen.
                    return Json(new
                    {
                        SagsbehandlerId = person.Id,
                        StatusAnmeldelseKode = statusAnmeldelseKode,
                        StatusAnmeldelseTekst = statusAnmeldelseTekst,
                        Message = message
                    });
                }
            }
            return Json(false);
        }

        [HttpPost]
        public ActionResult AfslutAnmeldelse(string anmeldelsesId)
        {
            const string message = "Der skete en fejl!";
            Guid g;
            if (Guid.TryParse(anmeldelsesId, out g))
            {
                var currentPerson = GetPerson();

                var res = _anmeldelserBusiness.AfslutAnmeldelse(g, currentPerson);
                var mes = res ? "Anmeldelsen er afsluttet" : "Anmeldelsen kunne ikke afsluttes";
                ViewBag.Message = mes;
                return Json(new { Success = true, Message = mes });
            }

            return Json(new { Success = false, Message = message });
        }

        [HttpPost]
        public ActionResult PlanlaegStikproeve(string anmeldelsesId)
        {
            var message = "Der skete en fejl!";
            Guid g;
            if (Guid.TryParse(anmeldelsesId, out g))
            {
                if (!_planlagteStikproeverBusiness.Search(x => x.Anmeldelse.Id == g).Any())
                {
                    // Der er ikke planlagt nogen planlagte stikprøver for anmeldelsen
                    var currentPerson = GetPerson();

                    var planlagteStikproever = new PlanlagteStikproever();
                    planlagteStikproever.AnmeldelseId = g;
                    planlagteStikproever.Oprettet = DateTime.Now;
                    planlagteStikproever.PersonId = currentPerson.Id;

                    var sp = new Stikproeve();
                    sp.StatusStikproeve.Add(_statusStikproeveBusiness.CreateStatus(EnumStatusStikproeve.Planlagt, currentPerson));
                    sp.InternBemaerkning = "";

                    planlagteStikproever.Stikproeve = sp;
                    _planlagteStikproeverBusiness.Create(planlagteStikproever);
                    if (sp.Id != Guid.Empty && planlagteStikproever.Id != Guid.Empty)
                    {
                        //Værdierne i return json objektet skal bruges til at tilpasse brugerfladen.
                        message = "Der er planlagt en stikprøve";
                        return Json(new { StikproeveId = sp.Id, Success = true, Message = message });
                    }
                    return Json(new { Success = false, Message = message });
                }
                message = "Der er allerede planlagt en stikprøve for denne anmeldelse";
            }
            return Json(new { Success = false, Message = message });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult VognlaesUpdate([DataSourceRequest] DataSourceRequest request, VognlaesModel model, string anmeldelseId)
        {
            var v = new Vognlaes();
            v.Id = model.Id;
            v.MaengdeAksler = model.JordmaengdeAksler;
            v.MaengdeTon = model.JordmaengdeTon;
            //PT er det kun jordmængden som kan opdateres.
            //Er der valgt forkert transportør, må vognlæsset slettes.

            var res = _vognlaesBusiness.UpdateVognlaes(model.Id, v);

            return Json(res);
        }

        [HttpPost]
        public ActionResult VognlaesDestroy([DataSourceRequest] DataSourceRequest request, VognlaesModel model, string anmeldelseId)
        {
            Guid ga;
            if (Guid.TryParse(anmeldelseId, out ga) && model != null)
            {
                var res = _anmeldelserBusiness.RemoveVognlaes(ga, model.Id);
                if (res)
                {
                    var data = ConvertVognlaesToModel(_anmeldelserBusiness.Read(ga).Vognlaes.ToList()).ToList();
                    return Json(data.ToDataSourceResult(request));
                }
            }
            return Json(null);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult VognlaesCreate([DataSourceRequest] DataSourceRequest request, VognlaesModel model, Guid anmeldelseId)
        {


            if (ModelState.IsValid)
            {
                if (model != null && anmeldelseId != Guid.Empty)
                {
                    // check if transportør har et firma
                    var trans = _personBusiness.Read(model.TransportoerId);
                    if (trans != null && trans.Firmaoplysninger != null)
                    {
                        var vognlaes = new Vognlaes();
                        vognlaes.Dato = DateTime.Now;
                        vognlaes.LastbilId = model.LastbilId;
                        vognlaes.MaengdeTon = model.JordmaengdeTon;
                        vognlaes.MaengdeAksler = model.JordmaengdeAksler;
                        vognlaes.Afvist = false;
                        _vognlaesBusiness.CreateVognlaes(vognlaes, anmeldelseId, false, null, true);
                        ViewBag.Message = "Vognlæsset er gemt.";
                    }
                    else
                    {
                        ViewBag.Message = "Fejl: Transportør har ingen firma.";
                        return Json(true);
                    }
                }
            }
            return Json(ModelState.ToDataSourceResult());
        }

        [HttpPost]
        public ActionResult SaveFile(IEnumerable<HttpPostedFileBase> files, Guid dokumentationTypeId, DateTime metadataDato, string anmeldelseId)
        {
            var result = false;
            var person = GetPerson();
            if (person != null)
            {
                if (Session["docList_" + anmeldelseId] == null)
                {
                    //slet indhold af personid mappe, hvis der skulle ligge noget fra en tidligere session.
                    _dokumentationBusiness.RemoveTempDirectory(person.Id.ToString());
                    Session["docList_" + anmeldelseId] = new Collection<Dokumentation>();
                }
                // The Name of the Upload component is "attachments" 
                var dl = (ICollection<Dokumentation>)Session["docList_" + anmeldelseId];
                foreach (var file in files)
                {
                    var dok = _dokumentationBusiness.CreateTempDokument(person.Id.ToString(), anmeldelseId, file, dokumentationTypeId, metadataDato);
                    dl.Add(dok);
                }
                Session["docList_" + anmeldelseId] = dl;
                result = true;
            }
            return Json(new { res = result }, "text/plain");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemoveFile([DataSourceRequest] DataSourceRequest request, DokumentationModel dokumentation, string anmeldelseId)
        {
            if (dokumentation != null)
            {
                if (Session["docList_" + anmeldelseId] == null)
                    Session["docList_" + anmeldelseId] = new HashSet<Dokumentation>();

                var dl = (ICollection<Dokumentation>)Session["docList_" + anmeldelseId];
                foreach (var d in dl)
                {
                    if (d.Filnavn != dokumentation.FilNavn)
                        continue;

                    var person = GetPerson();
                    if (person == null)
                        continue;

                    if (!_dokumentationBusiness.RemoveDokumentation(person.Id, dokumentation.AnmeldelseId, dokumentation.FilNavn, dokumentation.Id))
                        continue;

                    dl.Remove(d);
                    Session["docList_" + anmeldelseId] = dl;
                    break;
                }
            }
            return Json(ModelState.ToDataSourceResult());
        }

        [HttpPost]
        public ActionResult SendAdviser(string besked, string anmeldelseId, bool anmelder, bool transportoer, bool betaler, bool interessenter, bool sagsbehandler)
        {
            Guid ga;
            if (Guid.TryParse(anmeldelseId, out ga))
            {
                var a = _anmeldelserBusiness.Read(ga);
                if (a != null)
                {
                    var b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
                    var personSomErLoggetInd = b.Person;
                    var res = _adviseringBusiness.SendBeskedTilInteresenter(besked, a, anmelder, transportoer, betaler,
                                                                            interessenter, sagsbehandler, personSomErLoggetInd);
                    if (res)
                        return Json(new { Success = true });
                }
            }
            return Json(new { Success = false });
        }

        [HttpPost]
        public ActionResult SendHoerAndenKommune(string emne, string besked, string modtagerEmail, string anmeldelseId)
        {
            System.Diagnostics.Trace.TraceInformation("AnmeldelsesController.SendHoerAndenKommune Start");

            Guid ga;
            if (Guid.TryParse(anmeldelseId, out ga))
            {
                var a = _anmeldelserBusiness.Read(ga);
                if (a != null)
                {
                    var b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
                    var personSomErLoggetInd = b.Person;
                    var brugerProfil = new BrugerProfil();

                    if (modtagerEmail != null && modtagerEmail != "")
                    {
                        Person p;
                        Guid result = Guid.Empty;
                        brugerProfil.Person = (Person)_personBusiness.SearchByEmail(modtagerEmail).FirstOrDefault();
                        //var modtagerPerson = _brugerBusiness.GetBrugerProfilList().FirstOrDefault(x => x.BrugerNavn.ToLower() == modtagerEmail.ToLower());

                        if (!brugerProfil.HarPerson)
                        {
                            brugerProfil.Person = new Person();
                            brugerProfil.Person.Navn = modtagerEmail;
                            brugerProfil.Person.Efternavn = modtagerEmail;
                            brugerProfil.Person.Email = modtagerEmail;
                            brugerProfil.BrugerNavn = modtagerEmail;
                            brugerProfil.Person.Aktiv = true;

                            brugerProfil.Person.Firmaoplysninger = new Firmaoplysninger();
                            brugerProfil.Person.Firmaoplysninger.Firmanavn = modtagerEmail;

                            _brugerBusiness.CreateBetaler(brugerProfil);
                        }
                    }
                    else
                    {
                        //error "Modtager email er ikke angivet"
                    }

                    System.Diagnostics.Trace.TraceInformation("AnmeldelsesController.SendHoerAndenKommune info:");
                    System.Diagnostics.Trace.TraceInformation(string.Format("Emne : {0} - Besked : {1} - Person : {2]", emne, besked, brugerProfil.Person != null ? brugerProfil.Person.Navn : "NULL"));
                    var res = _adviseringBusiness.SendHoerAndenKommune(emne, besked, brugerProfil.Person, personSomErLoggetInd, a);
                    if (res)
                        return Json(new { Success = true });
                }
            }
            return Json(new { Success = false });
        }


        #endregion *** Save ***

        #region *** Get ***

        public ActionResult SearchForureningskomponent(string wildcard)
        {
            var fk = GetForureningskomponents();
            var fks = ConvertForureningkomponentsToForureningskomponenterModel(fk);
            var res = (from v in fks
                       where v.DisplayName.ToLower().Contains(wildcard.ToLower())
                       orderby v.DisplayName
                       select v)
              .Take(20);

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetJordKlassifikationerForKommuneAndenOprindelse(SagsbehandlingModel sagsbehandlingModel)
        {
            if (sagsbehandlingModel.AnmeldelseModel != null)
            {
                Guid kGuid;
                if (!string.IsNullOrEmpty(sagsbehandlingModel.AnmeldelseModel.selectedAndenOprindelsesstedKommune) && Guid.TryParse(sagsbehandlingModel.AnmeldelseModel.selectedAndenOprindelsesstedKommune, out kGuid))
                {
                    var jks = _kodelisteBusiness.ReadAktiveJordKlassifikationTypesForKommune(kGuid);
                    sagsbehandlingModel.AnmeldelseModel.JordklassifikationTypeListe = jks;
                    ModelState.Clear();
                    return PartialView("_BackendJordjordklassifikation", sagsbehandlingModel);
                }
            }
            return null;
        }

        [HttpPost]
        public ActionResult GetJordKlassifikationerForKommune(SagsbehandlingModel sagsbehandlingModel)
        {
            if (sagsbehandlingModel != null)
            {
                if (sagsbehandlingModel.AnmeldelseModel.Anmeldelse != null && sagsbehandlingModel.AnmeldelseModel.Anmeldelse.Kommune != null && !string.IsNullOrEmpty(sagsbehandlingModel.AnmeldelseModel.Anmeldelse.Kommune.Navn))
                {
                    var kommuneNavn = sagsbehandlingModel.AnmeldelseModel.Anmeldelse.Kommune.Navn;
                    var jks = _kodelisteBusiness.ReadAktiveJordKlassifikationTypesForKommune(kommuneNavn);
                    sagsbehandlingModel.AnmeldelseModel.JordklassifikationTypeListe = jks;

                    ModelState.Clear();
                    return PartialView("_BackendJordjordklassifikation", sagsbehandlingModel);
                }

                sagsbehandlingModel.AnmeldelseModel.ForureningOpslagFejlbesked = "Sted - Der skal bestemmes en kommune ved angivelse af adresse eller vej.";
                return PartialView("_BackendStedJordforuningOpslag", sagsbehandlingModel);
            }
            return null;
        }

        [HttpPost]
        public ActionResult GetJordForurening(SagsbehandlingModel sagsbehandlingModel)
        {
            sagsbehandlingModel.AnmeldelseModel.ForureningOpslagResultatList = new List<ForureningsOpslagResult>();
            sagsbehandlingModel.AnmeldelseModel.ForureningOpslagFejlbesked = "";

            var opretModel = sagsbehandlingModel.AnmeldelseModel;
            var wkt = opretModel.OprindelsesstedWkt;
            if (opretModel.Anmeldelse == null)
                opretModel.Anmeldelse = new Anmeldelse();

            if (opretModel.Anmeldelse.Oprindelsessted == null)
                opretModel.Anmeldelse.Oprindelsessted = new Oprindelsessted();

            if (!string.IsNullOrEmpty(wkt))
            {
                try
                {
                    if (string.IsNullOrEmpty(opretModel.Anmeldelse.Kommune.Navn))
                    {
                        opretModel.ForureningOpslagFejlbesked = "Sted - Angiv først en kommune ved at slå en adresse eller et vejnavn op.";
                        return PartialView("_StedJordforuningOpslag", opretModel);
                    }

                    try
                    {
                        var g = DbGeometry.FromText(wkt, 25832);
                        if (!g.IsValid)
                        {
                            opretModel.ForureningOpslagFejlbesked = "Sted - Det tegnede område er ikke valid";
                            return PartialView("_StedJordforuningOpslag", opretModel);
                        }
                    }
                    catch (Exception)
                    {
                        opretModel.ForureningOpslagFejlbesked = "Sted - Det tegnede område er ikke valid";
                        return PartialView("_StedJordforuningOpslag", opretModel);
                    }

                    opretModel.Anmeldelse.Oprindelsessted.Geom = DbGeometry.FromText(wkt, 25832);
                    //Henter matrikler fra wfs service
                    if (opretModel.Anmeldelse.Oprindelsessted.Geom.Area < 10000)
                    {
                        var matrikler = _matrikelBusiness.ReadMatrikler(wkt);
                        Session["Matrikler_" + opretModel.Anmeldelse.Id.ToString()] = matrikler;
                        if (matrikler != null)
                        {
                            opretModel.Anmeldelse.Oprindelsessted.Matrikel.Clear();
                            opretModel.Anmeldelse.Oprindelsessted.Matrikel.AddRange(matrikler);
                        }
                    }
                    else
                    {
                        opretModel.ForureningOpslagFejlbesked = "Sted - Arealet er over 1 hektar. Kontakt oprindelseskommunen.";
                        return PartialView("_StedJordforuningOpslag", opretModel);
                    }
                }
                catch (Exception ex)
                {
                    opretModel.ForureningOpslagFejlbesked =
                      "Der er fejl! System administratoren er blevet underrettet - Prøv eventuelt igen lidt senere.";
                    Logger.LogException("Fejl i opslag til eksterne systemer:", ex);
                    ModelState.Clear();
                    return PartialView("_StedJordforuningOpslag", opretModel);
                }
            }

            var kommune = _kommuneBusiness.ReadKommuneByNavn(opretModel.Anmeldelse.Kommune.Navn);
            opretModel.OprindelsesKommuneAnvenderFlytJord = kommune.Aktiv;
            opretModel.Anmeldelse.Kommune = kommune;

            var forureningOpslag = _forureningOpslagBusiness.Read(opretModel.Anmeldelse.Oprindelsessted, opretModel.Anmeldelse.Kommune.Id);
            var forureningopslagResult = forureningOpslag.GetResultList();
            opretModel.ForureningOpslagResultatList.AddRange(forureningOpslag.GetResultList().OrderBy(f => f.HeaderSortering));

            //Special regler vedr. offentlig vej.
            opretModel.ForureningOpslagOffentligvej = false;
            short oprindelsesstedKlassificeringTypeKode = 1; //Det er ligegyldig om den er 1 eller 3. Det vigtige ligger i om den er 2 som er offentlig vej.
            if (opretModel.Anmeldelse != null &&
              opretModel.Anmeldelse.Oprindelsessted != null &&
              opretModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType != null &&
              opretModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType.Id.ToString().ToUpper() == "295407E9-2182-4876-AE6F-50AFD00A6194")
            {
                oprindelsesstedKlassificeringTypeKode = 2;
                opretModel.ForureningOpslagOffentligvej = true;
            }

            var ka = forureningOpslag.GetHigestKlassifikation(kommune.Kommunenr, oprindelsesstedKlassificeringTypeKode);
            //var ka = forureningOpslag.GetMiljoePortalKlassifikation(kommune.Kommunenr, oprindelsesstedKlassificeringTypeKode);

            opretModel.ForureningOpslagFejlbesked = "";
            if (ka != null)
            {
                opretModel.Anmeldelse.Jord.JordKlassifikationType.Id = ka.Id;
                opretModel.ForureningOpslagJordklassifikationTypeId = ka.Id;
                opretModel.SelectedJordklassifikationType = ka.Id;
            }
            opretModel.ForureningOpslagKraeverAnmeldelse = forureningOpslag.HasAnmeldePligt(kommune.Kommunenr, oprindelsesstedKlassificeringTypeKode);

            //Gemmer Jordforureningsopslaget
            var opslag = new Jordforureningsopslag();
            opslag.Id = opretModel.Anmeldelse.Id;

            if (ka != null)
                opslag.JordKlassifikationTypeId = ka.Id;

            opslag.Tid = DateTime.Now;

            //Tjekker om listen indeholder en bestemt type 
            var v2 = (from k in forureningopslagResult
                      where k.MiljoePortalKlassifikation != null
                            && k.MiljoePortalKlassifikation.Id == (int)MiljoePortalKlassifikationEnum.JordforureningV2 && k.ResultException == null
                      select k).FirstOrDefault();
            opslag.V2 = v2 != null;

            var v1 = (from k in forureningopslagResult
                      where k.MiljoePortalKlassifikation != null
                            && k.MiljoePortalKlassifikation.Id == (int)MiljoePortalKlassifikationEnum.JordforureningV1 && k.ResultException == null
                      select k).FirstOrDefault();
            opslag.V1 = v1 != null;

            //OMK analysepligt
            var omkAnalysepligt = (from k in forureningopslagResult
                                   where k.MiljoePortalKlassifikation != null
                                         && k.MiljoePortalKlassifikation.Id == (int)MiljoePortalKlassifikationEnum.OmraadeMedMravMmMnalyser && k.ResultException == null
                                   select k).FirstOrDefault();
            opslag.OmkAnalysepligt = omkAnalysepligt != null;

            //OMK Analysefri letforurenet jord
            var omkLet = (from k in forureningopslagResult
                          where k.MiljoePortalKlassifikation != null
                                && k.MiljoePortalKlassifikation.Id == (int)MiljoePortalKlassifikationEnum.AnalysefritOmraadeLetForurenetJord && k.ResultException == null
                          select k).FirstOrDefault();
            opslag.OmkLet = omkLet != null;

            //OMK ren
            var omkRen = (from k in forureningopslagResult
                          where k.MiljoePortalKlassifikation != null
                                && k.MiljoePortalKlassifikation.Id == (int)MiljoePortalKlassifikationEnum.AnalysefritOmraadeRenJord && k.ResultException == null
                          select k).FirstOrDefault();
            opslag.OmkRen = omkRen != null;

            //Kommunes miljødatabase (Århus Geoenviron)
            var komMiljoeDb = (from k in forureningopslagResult
                               where k.MiljoePortalKlassifikation == null && k.ResultException == null
                               select k.LongText).FirstOrDefault();
            if (!string.IsNullOrEmpty(komMiljoeDb))
                opslag.KommunensMiljoeDb = komMiljoeDb;

            //Tjek for exceptions fra opslag i eksterne datakilder
            var resultException = (from e in forureningopslagResult where e.ResultException != null select e.ResultException).ToList();
            if (resultException.Any())
            {
                opretModel.ForureningOpslagFejlbesked = "Det er ikke muligt at hente information fra eksterne service vedr. jordens forureningsstatus! Afsendes anmeldelsen skal den behandles manuelt af kommunen. Alternativ kan du vente til de eksterne services virker igen.";
                opslag.KommunensMiljoeDb = "Forureningsstatus kan ikke hentes fra eksterne datakilder.";//Når der er en besked i dette felt kan anmeldelsen ikke behandles automatisk
            }


            Session["Jordforureningsopslag_" + opretModel.Anmeldelse.Id] = opslag;

            //Gemmer anmeldelsen i en session til kvitteringssiden. 
            //Problemer med print på denne side pga. IE sikkerheds issue. 
            //Man må ikke oprettet et dokument og skrive indholdet fra en div i det efterfult af print... 
            //Derfor denne nødløsning.
            Session["anmeldelse_kvittering"] = opretModel;

            //Kodelister + Stamdata
            opretModel = InitAnmeldelseModel(opretModel);

            ModelState.Clear();
            return PartialView("_StedJordforuningOpslag", opretModel);
        }

        [HttpPost]
        public ActionResult GetHeaderForAnmeldelse(string anmeldelseId)
        {
            var model = new SagsbehandlingModel();
            Guid g;
            if (Guid.TryParse(anmeldelseId, out g))
                model.AnmeldelseModel.Anmeldelse = _anmeldelserBusiness.Read(g);
            model = InitHeader(model);
            ModelState.Clear();
            return View("_Header", model);
        }

        [HttpPost]
        public ActionResult GetStatusForAnmeldelse(string anmeldelseId)
        {
            Guid g;
            if (Guid.TryParse(anmeldelseId, out g))
            {
                var a = _anmeldelserBusiness.Read(g);
                var latestBetalerStatus = _statusAnmeldelseBusiness.GetLastStatusTypeForBetaler(a.AnmeldelseEffektivStatus);
                var latestJordmodtagerStatus = _statusAnmeldelseBusiness.GetLastStatusTypeForJordmodtager(a.AnmeldelseEffektivStatus);
                ModelState.Clear();
                return Json(new
                {
                    betalerStatusKode = (latestBetalerStatus != EnumStatusAnmeldelse.Ukendt ? (int)latestBetalerStatus : -1),
                    jordmodtagerStatusKode = (latestJordmodtagerStatus != EnumStatusAnmeldelse.Ukendt ? (int)latestJordmodtagerStatus : -1)
                });
            }
            ModelState.Clear();
            return Json(new { betalerStatus = 0, jordmodtagerStatus = 0 });
        }

        [HttpPost]
        public ActionResult ReadKommunikationForAnmeldelse([DataSourceRequest] DataSourceRequest request, Guid anmeldelseId)
        {
            //Read statusanmeldelse
            var a = _anmeldelserBusiness.Read(anmeldelseId);
            if (a != null)
            {
                var komListe = (from k in a.Kommunikation
                                orderby k.Besked.Tid descending
                                select new KommunikationModel
                                {
                                    Id = k.Id,
                                    Tid = k.Besked.Tid,
                                    Fra = k.Person.Navn + " " + k.Person.Efternavn,
                                    Til = k.Person1.Navn + " " + k.Person1.Efternavn,
                                    Advislink = k.Besked.Advis.FirstOrDefault() != null ? LinkHelperAdvis(k.Besked.Advis.First().Id) : null
                                });

                var result = komListe.ToDataSourceResult(request);
                return Json(result);
            }
            return Json(null);
        }

        [HttpPost]
        public ActionResult ReadHistorikForAnmeldelse([DataSourceRequest] DataSourceRequest request, Guid anmeldelseId)
        {
            //Read statusanmeldelse
            var a = _anmeldelserBusiness.Read(anmeldelseId);
            if (a != null)
            {
                var historikListe = (from sa in a.StatusAnmeldelse
                                     select new ViewModels.HistorikAnmeldelseModel
                                     {
                                         Id = sa.Id,
                                         Tid = sa.Tid,
                                         Handling = sa.StatusAnmeldelseType.Navn,
                                         Person = (sa.Person != null ? "Af: " + sa.Person.Navn + " " + sa.Person.Efternavn : "Af: System"),
                                         Link = LinkHelperStatusAnmeldelse(sa)
                                     }).ToList();

                var komListe = (from k in a.Kommunikation
                                orderby k.Besked.Tid descending
                                select new ViewModels.HistorikAnmeldelseModel
                                {
                                    Id = k.Id,
                                    Tid = k.Besked.Tid,
                                    Handling = k.Besked.Advis.FirstOrDefault().AdvisType.Navn == "Hør anden kommune" ? "Kommunikation - Hør anden kommune" : "Kommunikation" + " - " + k.Besked.Advis.FirstOrDefault().AdvisType.Navn,
                                    Person = string.Format("Fra: {0} {1} Til: {2} {3}", k.Person.Navn, k.Person.Efternavn, k.Person1.Navn, k.Person1.Efternavn),
                                    Link = k.Besked.Advis.FirstOrDefault() != null ? LinkHelperAdvis(k.Besked.Advis.First().Id) : null
                                });

                var advisListe = (from ad in a.Advis
                                  select new ViewModels.HistorikAnmeldelseModel
                                  {
                                      Id = ad.Id,
                                      Tid = ad.Besked.Tid,
                                      Handling = "Advis",
                                      Person = (ad.Person != null ? "Til: " + ad.Person.Navn + " " + ad.Person.Efternavn : "-"),
                                      Link = LinkHelperAdvis(ad.Id)
                                  });

                var samletliste = new List<ViewModels.HistorikAnmeldelseModel>();
                samletliste.AddRange(historikListe);
                samletliste.AddRange(komListe);
                samletliste.AddRange(advisListe);
                samletliste.Sort((p1, p2) => -p1.Tid.CompareTo(p2.Tid));
                var result = samletliste.ToDataSourceResult(request);
                return Json(result);
            }
            return Json(null);
        }

        [HttpPost]
        public ActionResult ReadDokumentation([DataSourceRequest] DataSourceRequest request, Guid anmeldelseId)
        {
            if (Session["docList_" + anmeldelseId] == null)
                Session["docList_" + anmeldelseId] = new Collection<Dokumentation>();

            var dl = (ICollection<Dokumentation>)Session["docList_" + anmeldelseId];

            var dd = from d in dl
                     select new DokumentationModel
                     {
                         Id = d.Id,
                         FilNavn = d.Filnavn,
                         FilNavnUrlEncode = HttpUtility.UrlEncode(d.Filnavn),
                         Type = d.DokumentationType.Navn,
                         OprindelseDato = d.OprindelsesDato,
                         AnmeldelseId =
                           ((d.Jord != null && d.Jord.Anmeldelse != null)
                              ? d.Jord.Anmeldelse.Id
                              : Guid.Empty)
                     };
            var result = dd.ToDataSourceResult(request);
            return Json(result);
        }

        [HttpPost]
        public ActionResult ReadJordforureningOpslag([DataSourceRequest] DataSourceRequest request, string anmeldelseId)
        {
            if (Session["Jordforureningsopslag_" + anmeldelseId] != null)
            {
                var jfo = (Jordforureningsopslag)Session["Jordforureningsopslag_" + anmeldelseId];
                var jfom = ConvertJordforureningOpsalgToModel(jfo);
                var result = jfom.ToDataSourceResult(request);
                return Json(result);
            }
            return Json(null);
        }

        [HttpPost]
        public ActionResult ReadForureningskomponenter([DataSourceRequest] DataSourceRequest request, string anmeldelseId)
        {
            if (Session["Forureningskomponenter_" + anmeldelseId] != null)
            {
                var fks = (List<Forureningskomponent>)Session["Forureningskomponenter_" + anmeldelseId];
                var fkm = ConvertForureningkomponentsToForureningskomponenterModel(fks);
                var result = fkm.ToDataSourceResult(request);
                return Json(result);
            }
            return Json(null);
        }

        [HttpPost]
        public ActionResult ReadAdvis(string advisId)
        {
            Guid g;
            if (Guid.TryParse(advisId, out g))
            {
                var ad = _adviseringBusiness.GetAdvis(g);
                if (ad != null)
                {
                    var beskedUdenHttpLink = _adviseringBusiness.AdvisUdenHttp(ad.Besked.Tekst);
                    return Json(beskedUdenHttpLink);
                }
            }
            return Json(null);
        }

        [HttpPost]
        public ActionResult ReadLog(string logId)
        {
            Guid g;
            if (Guid.TryParse(logId, out g))
            {
                var l = _logBusiness.ReadLog(g);
                if (l != null)
                {
                    var htmlDelta = "";

                    foreach (var d in l.Delta)
                    {
                        htmlDelta += string.Format("<p><b>{0}</b></p><p>Før: {1}</p><p>Efter: {2}</p><br/>", d.Navn, d.Foer, d.Efter);
                    }

                    return Json(htmlDelta);
                }
            }
            return Json(null);
        }

        [HttpPost]
        public ActionResult GensendeAdvis(string anmeldelseId, string advisId)
        {
            Guid g;
            if (Guid.TryParse(advisId, out g))
            {
                var advis = _adviseringBusiness.GetAdvis(g);
                if (advis != null)
                {
                }


                if (Guid.TryParse(anmeldelseId, out g))
                {
                    var a = _anmeldelserBusiness.Read(g);

                    if (a != null)
                    {
                        Boolean sendAdvisBool;
                        //SendBeskedTilBetalerAcceptereDuBetalingen
                        if (advis.Besked.Tekst.ToString().Contains("Hvis du kan acceptere betaling"))
                        {
                            sendAdvisBool = _adviseringBusiness.SendBeskedTilBetalerAcceptereDuBetalingen(a);
                        }
                        else if (advis.Besked.Tekst.ToString().Contains("Anmeldelsen er godkendt."))
                        {
                            sendAdvisBool = _adviseringBusiness.SendAktiveretAnmeldelse(a);
                        }
                        else
                            sendAdvisBool = false;

                        /*if (a.Betaler != null && a.Betaler.Person != null)
                            sendAdvisBool = _adviseringBusiness.SendBeskedTilBetalerAcceptereDuBetalingen(a);
                        else
                             sendAdvisBool = _adviseringBusiness.SendAktiveretAnmeldelse(a); */

                        if (sendAdvisBool)
                        {
                            return Json("<p>Advis gensendt med succes.</p>");
                        }

                        //var beskedUdenHttpLink = _adviseringBusiness.AdvisUdenHttp(a.Besked.Tekst);
                        //return Json(beskedUdenHttpLink);
                    }
                }
            }
            return Json("<p>Fejl under afsendelse.</p>");
        }

        [HttpPost]
        public string HentDokumentation(string anmeldelseId)
        {
            Guid aid;

            var personBaseDir = ConfigurationManager.AppSettings["TempBaseFileDir"];
            var dokumentationBaseFileDir = ConfigurationManager.AppSettings["DokumentationBaseFileDir"];
            var analyseDokumentBaseFileDir = ConfigurationManager.AppSettings["AnalyseDokumentBaseFileDir"];
            var blanketBaseFileDir = ConfigurationManager.AppSettings["BlanketBaseFileDir"];
            var zipArchivePath = ConfigurationManager.AppSettings["ZipArchivePath"];
            var zipDownloadPath = string.Empty;

            if (Guid.TryParse(anmeldelseId, out aid))
            {
                var a = _anmeldelserBusiness.Read(aid);
                if (a != null)
                {

                    //Start PDF genering af HistorikKommunikation
                    var startTime = DateTime.Now;
                    _pdfBusiness.CreateAnmeldelseBlanket(aid);
                    _pdfBusiness.CreateHistorikKommunikationPdf(aid);

                    var timeUsed = DateTime.Now - startTime;
                    Logger.LogInfo("PDF service call time: " + timeUsed.TotalSeconds + " seconds.");

                    var filePaths = new List<string>() { String.Format("{0}{1}_hk.pdf", blanketBaseFileDir, anmeldelseId), String.Format("{0}{1}.pdf", blanketBaseFileDir, anmeldelseId) };

                    // HENTER Historiske/Analyse dokumenter!
                    foreach (var matrikel in a.Oprindelsessted.Matrikel)
                    {
                        if (matrikel.Geom != null)
                        {
                            var historiskeDokumenter = _anmeldelserBusiness.HentHistoriskeDokumenter(matrikel.Geom, aid);
                            if (historiskeDokumenter != null)
                            {
                                foreach (var dok in historiskeDokumenter)
                                {
                                    filePaths.Add(Path.Combine(dokumentationBaseFileDir, anmeldelseId, dok.Filnavn));
                                }
                            }

                            var analysedokumenter = _stikproeveBusiness.HentAnalyseDokumenter(matrikel.Geom, aid);
                            if (analysedokumenter != null && analysedokumenter.Count != 0)
                            {
                                foreach (var dok in analysedokumenter)
                                {
                                    filePaths.Add(Path.Combine(analyseDokumentBaseFileDir, anmeldelseId, dok.Filnavn));
                                }
                            }
                        }
                    }

                    // HENTER Dokumenter!
                    if (Session["docList_" + anmeldelseId] != null)
                    {
                        var dl = (ICollection<Dokumentation>)Session["docList_" + anmeldelseId];
                        if (dl != null)
                        {
                            foreach (var dok in dl)
                            {
                                filePaths.Add(Path.Combine(dokumentationBaseFileDir, anmeldelseId, dok.Filnavn));
                            }
                        }
                    }

                    var zipFilePath = String.Format("{0}{1}.zip", zipArchivePath, a.Nummer);

                    if (System.IO.File.Exists(zipFilePath))
                        System.IO.File.Delete(zipFilePath);

                    using (ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
                    {
                        foreach (var fPath in filePaths)
                        {
                            try
                            {
                                if (System.IO.File.Exists(fPath))
                                {
                                    var filename = Path.GetFileName(fPath);
                                    if (filename.Contains(anmeldelseId))
                                        filename = filename.Replace(anmeldelseId, a.Nummer.ToString());

                                    archive.CreateEntryFromFile(fPath, filename);
                                }
                                else
                                {
                                    Logger.LogWarning("Kunne ikke finde fil : " + fPath);
                                }
                            }
                            catch (Exception e)
                            {
                                Logger.LogException(e);
                            }
                        }
                    }

                    zipDownloadPath = String.Format("https://{0}/HistorikArkiv/{1}", ConfigurationManager.AppSettings["FlytJordDomain"], a.Nummer);
                }
            }

            return zipDownloadPath;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult VognlaesRead([DataSourceRequest] DataSourceRequest request, Guid? anmeldelseId)
        {
            var data = new List<VognlaesModel>();
            if (anmeldelseId.GetValueOrDefault() != Guid.Empty)
            {
                data = ConvertVognlaesToModel(_anmeldelserBusiness.Read(anmeldelseId.GetValueOrDefault()).Vognlaes.ToList()).OrderByDescending(x => x.DatoBom).ToList();
            }
            return Json(data.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AnmeldelseStikproeverGridRead([DataSourceRequest] DataSourceRequest request, Guid anmeldelseId)
        {
            IList<StikproeveModel> models = new List<StikproeveModel>();
            // få tilføjet til den korrekte stikproeve.
            if (anmeldelseId != Guid.Empty)
            {
                var anmeldelse = _anmeldelserBusiness.Read(anmeldelseId);
                if (anmeldelse != null)
                {
                    foreach (var vognlaes in anmeldelse.Vognlaes)
                    {
                        models.AddRange(vognlaes.Stikproeve.Select(x => new StikproeveModel(x)).ToList());
                    }

                    //Planlagte stikprøver.
                    if (anmeldelse.PlanlagteStikproever != null)
                    {
                        foreach (var ps in anmeldelse.PlanlagteStikproever)
                        {
                            if (ps.Stikproeve == null || ps.Stikproeve.Vognlaes != null)
                                continue;

                            var spm = new StikproeveModel(ps.Stikproeve);
                            spm.Status = "Planlagt stikprøve";
                            models.Add(spm);
                        }
                    }
                    models = models.OrderByDescending(x => x.StatusDato).ToList();
                }
            }
            return Json(models.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public FileStreamResult DownloadModtagerAnlaegDokumenter(Guid modtageAnlaegId, string filnavn)
        {
            var fs = _dokumenterBusiness.ReadDokumentation(modtageAnlaegId, filnavn);
            if (fs != null && fs.Length > 0)
                return File(fs, "application/octet-stream", filnavn); //d.Filnavn

            return null;
        }

        [Authorize]
        public FileStreamResult DownloadDokumentation(Guid anmeldelseId, string filnavn)
        {
            var person = GetPerson();
            if (person != null)
            {
                var fs = _dokumentationBusiness.ReadDokumentation(person.Id, anmeldelseId, filnavn);

                if (fs != null && fs.Length > 0)
                    return File(fs, "application/octet-stream", filnavn);
            }
            return null;
        }

        [Authorize]
        public FileStreamResult DownloadHistoriskDokumenter(Guid anmeldelseId, Guid stikproeveId, string filnavn)
        {
            var person = GetPerson();
            if (person != null)
            {
                if (anmeldelseId != Guid.Empty)
                {
                    var fs = _dokumentationBusiness.ReadDokumentation(person.Id, anmeldelseId, filnavn);
                    if (fs != null && fs.Length > 0)
                        return File(fs, "application/octet-stream", filnavn); //d.Filnavn
                }

                if (stikproeveId != Guid.Empty)
                {
                    var fs = _analyseDokumentBusiness.ReadAnalyseDokument(stikproeveId, filnavn);
                    if (fs != null && fs.Length > 0)
                        return File(fs, "application/octet-stream", filnavn); //d.Filnavn
                }
            }
            return null;
        }

        private IEnumerable<Forureningskomponent> GetForureningskomponents()
        {
            IEnumerable<Forureningskomponent> fkm = null;
            if (HttpContext.Application["Forureningskomponent"] == null)
            {
                var fk = _forureningskomponentBusiness.ReadAktiveForureningskomponenter();
                HttpContext.Application["Forureningskomponent"] = fk;
            }
            else
            {
                fkm = (IEnumerable<Forureningskomponent>)HttpContext.Application["Forureningskomponent"];
            }
            return fkm;
        }

        private IEnumerable<SelectListItem> GetAktiveKommuner(string selectedValue)
        {
            var res = from k in _kodelisteBusiness.ReadAktiveKommune()
                      select new SelectListItem
                      {
                          Selected = (k.Id.ToString() == selectedValue),
                          Text = k.Navn,
                          Value = k.Id.ToString()
                      };
            var selectListItems = res.ToList();
            return selectListItems;
        }

        private IEnumerable<SelectListItem> GetAndenOprindJordtyper(string selectedValue)
        {
            var res = from a in _kodelisteBusiness.ReadAktiveAndenOprindJordTypes()
                      select new SelectListItem
                      {
                          Selected = (a.Id.ToString() == selectedValue),
                          Text = a.Navn,
                          Value = a.Id.ToString()
                      };
            return res;
        }

        private IEnumerable<SelectListItem> GetAktiveDokumentationTypeLister()
        {
            var res = from o in _kodelisteBusiness.ReadAktiveDokumentationType(ESortType.Alfabetisk)
                      select new SelectListItem
                      {
                          Selected = false,
                          Text = o.Navn,
                          Value = o.Id.ToString()
                      };
            return res;
        }

        private IEnumerable<JordKlassifikationType> GetJordKlassifikationType(Guid kommuneId)
        {
            var res = _kodelisteBusiness.ReadAktiveJordKlassifikationTypesForKommune(kommuneId);
            return res;
        }

        private IEnumerable<SelectListItem> GetAktiveAffaldTyper(string selectedValue)
        {
            var res = new List<SelectListItem>((from a in _kodelisteBusiness.ReadAktiveAffaldTypes()
                                                select new SelectListItem
                                                {
                                                    Selected = (a.Id.ToString() == selectedValue),
                                                    Text = a.Navn,
                                                    Value = a.Id.ToString()
                                                }));

            res.Add(new SelectListItem { Text = @" ", Value = Guid.Empty.ToString() });
            return res.OrderBy(f => f.Value);
        }

        public JsonResult GetLastbiler(string transportoere)
        {
            if (!string.IsNullOrEmpty(transportoere))
            {
                var lastbiler =
                  _lastbilRepo.Read()
                              .Where(x => x.TransportoerId.ToString() == transportoere)
                              .Select(x => new { x.Nummerplade, x.Id })
                              .ToList();
                return Json(lastbiler, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<TransportoerModel>(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTransportoere()
        {
            var transportoere = _personBusiness.Read()
                                               .Where(x => x.Transportoer != null && x.Transportoer.Aktiv)
                                               .Select(x => new
                                               {
                                                   x.Transportoer.Id,
                                                   Navn = x.Transportoer.Person.Firmaoplysninger.Firmanavn
                                               }).ToList();

            return Json(transportoere, JsonRequestBehavior.AllowGet);
        }

        #endregion *** Get ***

        private static int? DistanceBeregner(DbGeometry g1, DbGeometry g2)
        {
            int? res = null;
            if (g1 != null && g2 != null)
            {
                if (!g1.IsValid | !g2.IsValid)
                    return null;
                res = (int?)(g1.Distance(g2) / 1000);
            }
            return res;
        }

        private static string LinkHelper(string www)
        {
            var res = "";
            if (!string.IsNullOrEmpty(www))
            {
                res = "<a href='" + www + "' target='blank'>www</a>";
            }
            return res;
        }

        private static string LinkHelperModtagerAnlaegKort(Guid modtagerAnlaegId, string modtageranlaegnavn)
        {
            var res = "";
            if (modtagerAnlaegId != Guid.Empty)
            {
                res = "<div style=\"cursor: pointer;\" onclick=\"openModtagerAnlaegKort('" + modtagerAnlaegId + "','" + modtageranlaegnavn + "')\">Kort</div>";
            }
            return res;
        }

        private static string LinkHelperAdvis(Guid advisId)
        {
            var res = "";
            if (advisId != Guid.Empty)
            {
                res = "<div style=\"cursor: pointer;\" onclick=\"openAdvis('" + advisId + "')\">Vis</div>";
                res += "<div style=\"cursor: pointer;\" onclick=\"sendAdvis('" + advisId + "')\">Gensend advis</div>";

            }
            return res;
        }

        private static string LinkHelperStatusAnmeldelse(StatusAnmeldelse sa)
        {
            var res = "";
            if (sa != null)
            {
                if (sa.StatusAnmeldelseType != null && sa.StatusAnmeldelseType.Kode == (int)EnumStatusAnmeldelse.Gemt)
                {
                    var d1 = sa.Tid.AddSeconds(-10);
                    var d2 = sa.Tid.AddSeconds(10);

                    //Finder Log . Datamodel er ikke optimal, så jeg bliver nød til at anvende tiden...
                    var log = (from l in sa.Anmeldelse.Log where l.Dato > d1 && l.Dato < d2 select l).FirstOrDefault();
                    if (log != null)
                        res = "<div style=\"cursor: pointer;\" onclick=\"openLog('" + log.Id + "')\">Vis ændring</div>";
                }
            }
            return res;
        }

        /// <summary>
        /// Hent den påloggede brugers persondata
        /// </summary>
        private Person GetPerson()
        {
            var b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
            if (b != null)
            {
                var person = b.Person;
                return person;
            }
            return null;
        }

        private Sagsbehandler GetSagsbehandler(Guid userId)
        {
            var sagsbehandler = _brugerBusiness.ReadSagsbehandler(userId);
            return sagsbehandler;
        }

    }
}