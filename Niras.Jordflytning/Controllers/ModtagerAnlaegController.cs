using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Linq;
using System.Net.Mime;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Enums;
using Niras.Jordflytning.Infrastructure.Common;
using Niras.Jordflytning.Library.Logging;
using Niras.Jordflytning.ViewModels;
using Niras.Jordflytning.ViewModels.Mobile;
using Niras.Jordflytning.ViewModels.ModtagerAnlaeg;
using Niras.Jordflytning.ViewModels.Placering;

namespace Niras.Jordflytning.Controllers
{
    public class ModtagerAnlaegController : Controller
    {
        private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.ModtagerAnlaegController");

        #region Variables

        private readonly IModtagerAnlaegBusiness _modtagerAnlaegBusiness;
        private readonly IDokumenterBusiness _dokumenterBusiness;
        private readonly IKodelisteBusiness _kodelisteBusiness;
        private readonly IJordmodtagerBusiness _jordmodtagerBusiness;
        private readonly IForureningsOpslagBusiness _forureningsOpslagBusiness;

        #endregion

        #region Constructors

        public ModtagerAnlaegController(IModtagerAnlaegBusiness modtagerAnlaegBusiness, IDokumenterBusiness dokumenterBusiness, IKodelisteBusiness kodelisteBusiness, IJordmodtagerBusiness jordmodtagerBusiness, IForureningsOpslagBusiness forureningsOpslagBusiness)
        {
            _modtagerAnlaegBusiness = modtagerAnlaegBusiness;
            _dokumenterBusiness = dokumenterBusiness;
            _kodelisteBusiness = kodelisteBusiness;
            _jordmodtagerBusiness = jordmodtagerBusiness;
            _forureningsOpslagBusiness = forureningsOpslagBusiness;
        }

        #endregion

        private static ModtagerAnlaegKortModel InitModel(ModtagerAnlaegKortModel m)
        {
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;
            m.KortApiUrl = appSettings["KortApiUrl"];
            m.KortPageModtagere = appSettings["KortPageModtagere"];
            m.KortSite = appSettings["KortSite"];
            return m;
        }

        public ActionResult KortAlle()
        {
            var model = new ModtagerAnlaegKortModel();
            model = InitModel(model);
            return View("Kort", model);
        }

        public ActionResult Kort(string modtagerAnlaegId)
        {
            var model = new ModtagerAnlaegKortModel();
            model = InitModel(model);

            //Hent ModtagerAnlaeg
            Guid g;
            if (Guid.TryParse(modtagerAnlaegId, out g))
            {
                var ma = _modtagerAnlaegBusiness.Read(g);

                //Set kort værider
                if (ma.Geom != null)
                {
                    model.wkt = ma.Geom.WellKnownValue.WellKnownText;
                    model.MapModtagerAnlaegLx = model.GetMapBoundLowerX(ma.Geom.WellKnownValue.WellKnownText);
                    model.MapModtagerAnlaegLy = model.GetMapBoundLowerY(ma.Geom.WellKnownValue.WellKnownText);
                    model.MapModtagerAnlaegUx = model.GetMapBoundUpperX(ma.Geom.WellKnownValue.WellKnownText);
                    model.MapModtagerAnlaegUy = model.GetMapBoundUpperY(ma.Geom.WellKnownValue.WellKnownText);
                }
            }
            model.ShowDigitaliserButton = false;
            return View(model);
        }

        public ActionResult RedigerPlacering(string modtagerAnlaegId)
        {
            var model = new ModtagerAnlaegKortModel();
            model = InitModel(model);

            //Hent ModtagerAnlaeg
            Guid g;
            if (Guid.TryParse(modtagerAnlaegId, out g))
            {
                var ma = _modtagerAnlaegBusiness.Read(g);

                //Set kort værider
                if (ma != null && ma.Geom != null)
                {
                    model.MapModtagerAnlaegLx = model.GetMapBoundLowerX(ma.Geom.WellKnownValue.WellKnownText);
                    model.MapModtagerAnlaegLy = model.GetMapBoundLowerY(ma.Geom.WellKnownValue.WellKnownText);
                    model.MapModtagerAnlaegUx = model.GetMapBoundUpperX(ma.Geom.WellKnownValue.WellKnownText);
                    model.MapModtagerAnlaegUy = model.GetMapBoundUpperY(ma.Geom.WellKnownValue.WellKnownText);
                }
            }
            model.ShowDigitaliserButton = true;
            return View(model);
        }

        public ActionResult Dokumenter(string modtagerAnlaegId)
        {
            var model = new ModtagerAnlaegDokumentModel();
            //Hent ModtagerAnlaeg
            Guid g;
            if (Guid.TryParse(modtagerAnlaegId, out g))
            {
                model.ModtagerAnlaegId = g;
            }
            return View(model);

        }

        [AllowAnonymous]
        public FileStreamResult DownloadDokumentation(Guid modtageAnlaegId, string filnavn)
        {
            var fs = _dokumenterBusiness.ReadDokumentation(modtageAnlaegId, filnavn);
            if (fs != null && fs.Length > 0)
                return File(fs, "application/octet-stream", filnavn); //d.Filnavn

            return null;
        }

        private void InitModel(IndtegnetPlaceringModel m)
        {
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;
            m.KortApiUrl = appSettings["KortApiUrl"];
            m.KortPageModtagere = appSettings["KortPageModtagere"];
            m.KortPageSted = appSettings["KortPageSted"];
            m.KortSite = appSettings["KortSite"];
        }

        private void InitModel(RedigerMidlertidigtBrugerAnlaegModel m)
        {
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;
            m.KortApiUrl = appSettings["KortApiUrl"];
            m.KortPageModtagere = appSettings["KortPageModtagere"];
            m.KortPageSted = appSettings["KortPageMidlertidigeModtagere"];
            m.KortSite = appSettings["KortSite"];
            var kommuneliste = _kodelisteBusiness.ReadAllKommuner().ToArray();
            m.Kommuner = kommuneliste.Select(k => new SelectListItem() { Selected = false, Value = k.Kommunenr.ToString("0000"), Text = k.Navn }).ToSelectList();
            m.InitFejl = new List<string>();
            m.SaveState = SaveState.None;
        }

        [HttpGet]
        public ActionResult RedigerMidlertidigtBrugerAnlaeg(Guid? id, Guid? jordKlassifikationTypeId)
        {
            var viewModel = new RedigerMidlertidigtBrugerAnlaegModel();
            InitModel(viewModel);

            //Hent fra db og map her
            if (id.HasValue && id.Value != Guid.Empty)
            {
                var anlaeg = _modtagerAnlaegBusiness.Read(id.Value);
                var modtager = _jordmodtagerBusiness.Read(anlaeg.JordmodtagerId.Value);

                // Hvis vi kommer ind med alt andet en tomt id, eller noget der ikke er anmelder oprettet midlertidigt anlæg, bruges dette ikke.
                // (Tvinges til nyoprettelse)
                if (anlaeg.AnmelderOprettetMidlertidigtAnlaeg)
                {
                    MapToViewModel(viewModel, modtager);
                    MapToViewModel(viewModel, anlaeg);
                }
            }
            viewModel.JordKlassifikationTypeId = jordKlassifikationTypeId ?? Guid.Empty;
            if (jordKlassifikationTypeId == null || jordKlassifikationTypeId == Guid.Empty)
                viewModel.InitFejl.Add("Der er ikke valgt en gyldig jordklassifikationstype, redigering kan ikke fortsætte!");

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult RedigerMidlertidigtBrugerAnlaeg(RedigerMidlertidigtBrugerAnlaegModel viewModel)
        {
            // Guids bliver tvunget til Guid.Empty i viewmodel.
            // ModelState brokker sig over ikke nullable Guid.Empty værdier)
            InitModel(viewModel);
            if (ModelState.IsValid)
            {
                using (
                    var scope = new TransactionScope(TransactionScopeOption.Required,
                        new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    try
                    {
                        var isNew = (viewModel.AnlaegId.Value == Guid.Empty);
                        Jordmodtager modtager = null;
                        ModtagerAnlaeg anlaeg = null;
                        
                        if (isNew)
                        {
                            modtager = new Jordmodtager();
                            anlaeg = new ModtagerAnlaeg();
                            // Modtager:
                            MapToModel(modtager, viewModel);
                            modtager.Aktiv = true;
                            _jordmodtagerBusiness.Create(modtager);
                            // Anlæg:
                            viewModel.ModtagerId = modtager.Id;
                            MapToModel(anlaeg, viewModel);
                            _modtagerAnlaegBusiness.Create(anlaeg);
                            viewModel.AnlaegId = anlaeg.Id;
                        }
                        else
                        {
                            anlaeg = _modtagerAnlaegBusiness.Read(viewModel.AnlaegId.Value);
                            modtager = _jordmodtagerBusiness.Read(viewModel.ModtagerId.Value);
                            MapToModel(modtager, viewModel);
                            MapToModel(anlaeg, viewModel);
                            _jordmodtagerBusiness.SaveChanges();
                            _modtagerAnlaegBusiness.SaveChanges();
                        }
                        // Gem filer:
                        for (var i = 0; i < Request.Files.Count; i++)
                        {
                            var file = Request.Files[i];
                            _dokumenterBusiness.SaveFile(anlaeg.Id, file);
                        }
                        scope.Complete();
                        viewModel.SaveState = SaveState.Saved;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Fejl", ex);
                        viewModel.SaveState = SaveState.Failed;
                    }
                }
            }
            else
                viewModel.SaveState = SaveState.Failed;
            viewModel.PostAttempts++;
            return View(viewModel);
        }

        private void MapToModel(Jordmodtager model, RedigerMidlertidigtBrugerAnlaegModel viewModel)
        {
            model.Navn = viewModel.Grundejer;
            model.Adresse = viewModel.Adresse;
            model.Postnummer = viewModel.Postnummer;
            model.PostDistrikt = viewModel.Postdistrikt;
            model.Telefon = viewModel.Telefon;
            model.CVR = viewModel.CVR;
        }

        private void MapToModel(ModtagerAnlaeg model, RedigerMidlertidigtBrugerAnlaegModel viewModel)
        {
            model.JordmodtagerId = viewModel.ModtagerId;
            model.Navn = viewModel.Navn;
            model.Adresse = viewModel.Adresse;
            model.Postnummer = viewModel.Postnummer;
            model.PostDistrikt = viewModel.Postdistrikt;
            model.KommuneKode = int.Parse(viewModel.Kommunekode);
            if (viewModel.Ejerlav.HasValue)
                model.Ejerlav = viewModel.Ejerlav.Value.ToString();
            model.Ejerlavsnavn = viewModel.Ejerlavsnavn;
            model.Matrikelnr = viewModel.Matrikelnummer;
            model.EsrEjendomsnr = viewModel.EsrEjensomsnummer;
            model.Matrikelnr = viewModel.Matrikelnummer;
            model.Bemaerkning = viewModel.Bemaerkninger;
            model.KontaktpersonNavn = viewModel.Kontaktperson;
            model.KontaktpersonEmail = viewModel.Email;
            model.KontaktpersonTlf = viewModel.Telefon;
            model.JordKlassifikationTypeId = viewModel.JordKlassifikationTypeId;
            model.Affald = viewModel.Affald;
            model.AnmelderOprettetMidlertidigtAnlaeg = true;
            model.Geom = viewModel.MapBounds.Geom;
        }

        private void MapToViewModel(RedigerMidlertidigtBrugerAnlaegModel viewModel, Jordmodtager model)
        {
            viewModel.ModtagerId = model.Id;
            viewModel.Grundejer = model.Navn;
            viewModel.Adresse = model.Adresse;
            viewModel.Postnummer = (int?)model.Postnummer;
            viewModel.Postdistrikt = model.PostDistrikt;
            viewModel.Telefon = (int?)model.Telefon;
            viewModel.CVR = (int?)model.CVR;
        }

        private void MapToViewModel(RedigerMidlertidigtBrugerAnlaegModel viewModel, ModtagerAnlaeg model)
        {
            // Nogle af disse vil ovskrive eventuelle værdier, der måtte stamme fra ovenstående jordmodtager mamping.
            viewModel.AnlaegId = model.Id;
            if (!viewModel.ModtagerId.HasValue)
                viewModel.ModtagerId = model.JordmodtagerId;
            viewModel.Navn = model.Navn;
            viewModel.Adresse = model.Adresse;
            viewModel.Postnummer = (int?) model.Postnummer;
            viewModel.Postdistrikt = model.PostDistrikt;
            viewModel.Kommunekode = model.KommuneKode.ToString();
            int ejerlav = 0;
            if (!string.IsNullOrWhiteSpace(model.Ejerlav) && int.TryParse(model.Ejerlav, out ejerlav))
                viewModel.Ejerlav = ejerlav;
            viewModel.Ejerlavsnavn = model.Ejerlavsnavn;
            viewModel.Matrikelnummer = model.Matrikelnr;
            viewModel.EsrEjensomsnummer = model.EsrEjendomsnr;
            viewModel.Bemaerkninger = model.Bemaerkning;
            viewModel.Kontaktperson = model.KontaktpersonNavn;
            viewModel.Email = model.KontaktpersonEmail;
            viewModel.Telefon = (int?)model.KontaktpersonTlf;
            viewModel.JordKlassifikationTypeId = model.JordKlassifikationTypeId ?? Guid.Empty;
            viewModel.Affald = model.Affald;
            if (model.Geom != null)
                viewModel.MapBounds.Init(model.Geom.AsText());
        }

        [AllowAnonymous]
        public PartialViewResult KonfliktSoegningMidlertidigtAnlaeg(string wkt)
        {
            KonfliktSoegningModel model = null;
            if (!string.IsNullOrEmpty(wkt))
            {
                try
                {
                    var geom = DbGeometry.FromText(wkt, 25832);
                    var resultat = _forureningsOpslagBusiness.KonfliktSoegningMidlertidigModtager(geom);
                    model = KonfliktSoegningModel.Create(resultat);
                }
                catch (Exception)
                {
                    model = new KonfliktSoegningModel();
                    model.GenerelFejl = "Der er fejl i de eksterne systemer!<br/>System administratoren er blevet underrettet.<br/>Prøv eventuelt igen lidt senere.";
                }
            }
            else
                model = new KonfliktSoegningModel() { GenerelFejl = "Ugyldig forespørgsel!"};
            return PartialView(model);
        }

    }
}
