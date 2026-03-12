using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Niras.Jordflytning.Core;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.JordForurening;
using Niras.Jordflytning.Library.Logging;
using Niras.Jordflytning.ViewModels.Anmeldelse;
using Niras.Jordflytning.ViewModels.Backend;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.Spatial;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using InteresantModel = Niras.Jordflytning.ViewModels.Anmeldelse.InteresantModel;

namespace Niras.Jordflytning.Controllers
{
    [Authorize]
    public class AnmeldelserController : Controller
    {
        private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Controllers.AnmeldelserController");

        #region Variables

        private const int Epsg25832 = 25832;

        private readonly IAnmeldelserBusiness _anmeldelserBusiness;
        private readonly IKodelisteBusiness _kodelisteBusiness;
        private readonly IModtagerAnlaegBusiness _modtagerAnlaegBusiness;
        private readonly ITransportoerBusiness _transportoerBusiness;
        private readonly IPersonBusiness _personBusiness;
        private readonly ISecurityProvider _securityProvider;
        private readonly IBrugereBusiness _brugerBusiness;
        private readonly IForureningsOpslagBusiness _forureningOpslagBusiness;
        private readonly IDokumentationBusiness _dokumentationBusiness;
        private readonly IBetalerBusiness _betalerBusiness;
        private readonly IMatrikelBusiness _matrikelBusiness;
        private readonly IKonfigBusiness _konfigBusiness;
        private readonly IAnmelderBusiness _anmelderBusiness;
        private readonly IStatusBetalerBusiness _statusBetalerBusiness;
        private readonly IAdviseringBusiness _adviseringBusiness;
        private readonly IStatusAnmeldelseBusiness _statusAnmeldelseBusiness;
        private readonly IStikproeveBusiness _stikproeveBusiness;
        private readonly IAnalyseDokumentBusiness _analyseDokumentBusiness;
        private readonly IKommuneBusiness _kommuneBusiness;
        private readonly IGebyrTidsregistreringBusiness _gebyrTidsregistreringBusiness;
        private readonly IOpgavetypeBusiness _opgavetypeBusiness;
        private readonly ILogBusiness _logBusiness;

        #endregion

        #region Constructors

        public AnmeldelserController(
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
          IKonfigBusiness konfigBusiness,
          IAnmelderBusiness anmelderBusiness,
          IStatusBetalerBusiness statusBetalerBusiness,
          IAdviseringBusiness adviseringBusiness,
          IStatusAnmeldelseBusiness statusAnmeldelseBusiness,
          IStikproeveBusiness stikproeveBusiness,
          IAnalyseDokumentBusiness analyseDokumentBusiness,
          IKommuneBusiness kommuneBusiness,
          IGebyrTidsregistreringBusiness gebyrTidsregistreringBusiness,
          IOpgavetypeBusiness opgavetypeBusiness,
          ILogBusiness logBusiness
          )
        {
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
            _konfigBusiness = konfigBusiness;
            _anmelderBusiness = anmelderBusiness;
            _statusBetalerBusiness = statusBetalerBusiness;
            _adviseringBusiness = adviseringBusiness;
            _statusAnmeldelseBusiness = statusAnmeldelseBusiness;
            _stikproeveBusiness = stikproeveBusiness;
            _analyseDokumentBusiness = analyseDokumentBusiness;
            _kommuneBusiness = kommuneBusiness;
            _gebyrTidsregistreringBusiness = gebyrTidsregistreringBusiness;
            _opgavetypeBusiness = opgavetypeBusiness;
            _logBusiness = logBusiness;
        }

        #endregion

        #region "Ajax functions"

        //http: //docs.kendoui.com/getting-started/using-kendo-with/aspnet-mvc/helpers/grid/ajax-binding

        private List<GebyrTidsregistreringViewModel> LoadGebyrTidsregistreringerToSession(Guid anmeldelsesId)
        {

            var registreringer = Session[$"GebyrTidsregistrering_{anmeldelsesId}"] as List<GebyrTidsregistreringViewModel>;
            if (registreringer == null)
            {
                var gebyr = _anmeldelserBusiness.GetAnmeldelse(anmeldelsesId).Gebyr;
                if (gebyr != null && gebyr.GebyrTidsregistrering != null)
                    registreringer = gebyr.GebyrTidsregistrering
                        .Select(GebyrTidsregistreringViewModel.Create)
                        .ToList();
                else
                    registreringer = new List<GebyrTidsregistreringViewModel>();
            }

            SaveGebyrTidsregistreringerToSession(anmeldelsesId, registreringer);
            return registreringer;
        }
        private void SaveGebyrTidsregistreringerToSession(Guid anmeldelsesId, List<GebyrTidsregistreringViewModel> registreringer)
        {
            Session[$"GebyrTidsregistrering_{anmeldelsesId}"] = registreringer ?? new List<GebyrTidsregistreringViewModel>();
        }

        public ActionResult ReadGebyrTidsregistrering([DataSourceRequest] DataSourceRequest request, Guid id)
        {
            var registreringer = LoadGebyrTidsregistreringerToSession(id);

            var result = registreringer.ToDataSourceResult(request);
            return Json(result);
        }

        public ActionResult EditGebyrTidsregistrering([DataSourceRequest] DataSourceRequest request, GebyrTidsregistreringViewModel model)
        {
            ModelState.Clear();

            if (model.Medarbejder == null || model.Medarbejder.Id == Guid.Empty)
                ModelState.AddModelError("Medarbeder", "Vælg medarbejder");

            if (model.Opgavetype == null || model.Opgavetype.Id == Guid.Empty)
                ModelState.AddModelError("Opgavetype", "Vælg opgavetype");

            // Opstartsgebyr har ikke krav om tidsforbrug. (Kode 0)
            if (model.ForbrugtTid == TimeSpan.Zero && model.Opgavetype.KraeverTidsregistrering)
            {
                var tidTimer = System.Web.HttpContext.Current.Request["ForbrugtTid.Hours"];
                var tidMinutter = System.Web.HttpContext.Current.Request["ForbrugtTid.Minutes"];
                if (string.IsNullOrWhiteSpace(tidTimer) || string.IsNullOrWhiteSpace(tidMinutter))
                    ModelState.AddModelError("ForbrugtTid", "Angiv forbrugt tid");
                else
                {
                    var hours = int.Parse(tidTimer);
                    var minutes = int.Parse(tidMinutter);
                    model.ForbrugtTid = new TimeSpan(hours, minutes, 0);

                    if (model.ForbrugtTid == TimeSpan.Zero)
                        ModelState.AddModelError("ForbrugtTid", "Angiv forbrugt tid");
                }
            }

            if (ModelState.IsValid)
            {
                var anmeldelsesId = Guid.Parse(Request.Form["AnmeldelseId"]);
                var registreringer = LoadGebyrTidsregistreringerToSession(anmeldelsesId);
                if (model.Id == Guid.Empty)
                {
                    model.Id = Guid.NewGuid();
                    registreringer.Add(model);
                }
                else {
                    for (var i = 0; i < registreringer.Count; i++)
                    {
                        if (registreringer[i].Id == model.Id)
                        {
                            registreringer[i] = model;
                            break;
                        }
                    }                    
                }
                SaveGebyrTidsregistreringerToSession(anmeldelsesId, registreringer);
            }

            var output = new List<GebyrTidsregistreringViewModel>( new[] { model });
            return Json(output.ToDataSourceResult(request, ModelState));
        }

        public ActionResult RemoveGebyrTidsregistrering([DataSourceRequest] DataSourceRequest request, GebyrTidsregistreringViewModel model)
        {
            var anmeldelsesId = Guid.Parse(Request.Form["AnmeldelseId"]);
            var registreringer = LoadGebyrTidsregistreringerToSession(anmeldelsesId);
                        
            var item = registreringer.First(x => x.Id == model.Id);
            registreringer.Remove(item);
            SaveGebyrTidsregistreringerToSession(anmeldelsesId, registreringer);

            var output = new List<GebyrTidsregistreringViewModel>(new[] { model });
            return Json(output.ToDataSourceResult(request));
        }

        public ActionResult ReadSagsbehandlere([DataSourceRequest] DataSourceRequest request)
        {
            var anmeldelsesId = Guid.Parse(Request.Form["AnmeldelseId"]);
            var kommuneId = _anmeldelserBusiness.GetAnmeldelse(anmeldelsesId).KommuneId.Value;
            var sagsbehandlere = _brugerBusiness
                .ReadAktiveSagsbehandlere(kommuneId)
                .Select(SagsbehandlerViewModel.Create);
            return Json(sagsbehandlere.ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult ReadModtagerAnlaeg([DataSourceRequest] DataSourceRequest request, int filtreringMetode, string wkt, bool anvenderJf,
                                               string jordKlassifikationTypeId, bool affald)
        {
            //http://docs.kendoui.com/getting-started/using-kendo-with/aspnet-mvc/helpers/grid/ajax-binding
            DbGeometry g = null;
            try
            {
                if (!String.IsNullOrEmpty(wkt))
                {
                    g = DbGeometry.FromText(wkt, Epsg25832);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            Guid gJordKlassifikationType;
            Guid.TryParse(jordKlassifikationTypeId, out gJordKlassifikationType);

            IList<ModtagerAnlaeg> ma = new List<ModtagerAnlaeg>();
            switch (filtreringMetode)
            {
                case (int)ModtagerAnlaegBusiness.EnumFiltreringmetode.Alle:
                    {
                        ma = _modtagerAnlaegBusiness.ReadAktiveModtagerAnlaeg(anvenderJf, gJordKlassifikationType, affald);
                        break;
                    }
                case (int)ModtagerAnlaegBusiness.EnumFiltreringmetode.Afstand:
                    {
                        if (g != null)
                            ma = _modtagerAnlaegBusiness.ReadAktiveNaermesteModtagerAnlaeg(g, anvenderJf, gJordKlassifikationType, affald);
                        break;
                    }
                case (int)ModtagerAnlaegBusiness.EnumFiltreringmetode.Oprindelseskommune:
                    {
                        break;
                    }
                case (int)ModtagerAnlaegBusiness.EnumFiltreringmetode.Tidligere:
                    {
                        var b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
                        var person = b.Person;
                        if (person != null)
                            ma = _modtagerAnlaegBusiness.ReadTidligereAktiveAnvendteModtagerAnlaeg(person.Id, anvenderJf, gJordKlassifikationType, affald);
                        break;
                    }
            }
            var modtageranlaeg = ConvertModtagerAnlaegToModel(ma, g);
            var result = modtageranlaeg.ToDataSourceResult(request);
            return Json(result);
        }

        [HttpPost]
        public ActionResult ReadTransportoerer([DataSourceRequest] DataSourceRequest request, int filtreringMetode)
        {
            //http: //docs.kendoui.com/getting-started/using-kendo-with/aspnet-mvc/helpers/grid/ajax-binding
            IList<Transportoer> transportoers = new List<Transportoer>();
            switch (filtreringMetode)
            {
                case (int)TransportoerBusiness.EnumFiltreringsMetode.Tidligere:
                    {
                        var b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
                        var person = b.Person;
                        if (person != null)
                            transportoers = _transportoerBusiness.ReadTidligereAktiveAnvendteTransportoerer(person.Id);
                        break;
                    }
                case (int)TransportoerBusiness.EnumFiltreringsMetode.Alle:
                    {
                        transportoers = _transportoerBusiness.ReadAktiveTransportoerer();
                        break;
                    }
            }
            var t = ConvertTransportoerToModel(transportoers);

            var result = t.ToDataSourceResult(request);
            return Json(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ReadDokumentation([DataSourceRequest] DataSourceRequest request)
        {
            if (Session["docList"] == null)
            {
                Session["docList"] = new Collection<Dokumentation>();
            }
            var dl = (ICollection<Dokumentation>)Session["docList"];
            var dd =
              from d in dl
              select new DokumentationModel
                {
                    Id = d.Id,
                    FilNavn = d.Filnavn,
                    FilNavnUrlEncode = HttpUtility.UrlEncode(d.Filnavn),
                    Type = d.DokumentationType.Navn,
                    OprindelseDato = d.OprindelsesDato,
                    AnmeldelseId = ((d.Jord != null && d.Jord.Anmeldelse != null) ? d.Jord.Anmeldelse.Id : Guid.Empty)
                };
            var result = dd.ToDataSourceResult(request);
            return Json(result);
        }

        [HttpPost]
        public ActionResult ReadDokumentationForRevision([DataSourceRequest] DataSourceRequest request)
        {
            if (Session["docListRev"] == null)
            {
                Session["docListRev"] = new Collection<Dokumentation>();
            }
            var dl = (ICollection<Dokumentation>)Session["docListRev"];
            var dd =
              from d in dl
              select new DokumentationModel
              {
                  Id = d.Id,
                  FilNavn = d.Filnavn,
                  FilNavnUrlEncode = HttpUtility.UrlEncode(d.Filnavn),
                  Type = d.DokumentationType.Navn,
                  OprindelseDato = d.OprindelsesDato,
                  AnmeldelseId = ((d.Jord != null && d.Jord.Anmeldelse != null) ? d.Jord.Anmeldelse.Id : Guid.Empty)
              };
            var result = dd.ToDataSourceResult(request);
            return Json(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ReadHistoriskDokumentation([DataSourceRequest] DataSourceRequest request, string matrikelWkt, Guid excludeAnmeldelseId)
        {
            //matrikelWkt kan være en geometryCollection, hvis der er tegnet et polygon eller en linie som går over flere matrikler.

            //Historiske dokumenter
            if (!string.IsNullOrEmpty(matrikelWkt))
            {
                try
                {
                    var g = DbGeometry.FromText(matrikelWkt, Epsg25832);
                    var dd = ConvertDokumentationToModel(_anmeldelserBusiness.HentHistoriskeDokumenter(g, excludeAnmeldelseId));

                    var analyseDokumneter = _stikproeveBusiness.HentAnalyseDokumenter(g, excludeAnmeldelseId);
                    var ad = ConvertAnalyseDokumentToModel(analyseDokumneter);

                    var docList = new List<HistoriskDokumentationModel>();
                    docList.AddRange(dd);
                    docList.AddRange(ad);

                    docList.Sort((p1, p2) => -p1.DatoData.CompareTo(p2.DatoData));

                    var result = docList.ToDataSourceResult(request);
                    return Json(result);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
            return Json(null);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ReadAnalyseDokumentation([DataSourceRequest] DataSourceRequest request)
        {
            if (Session["analyseDocList"] == null)
            {
                Session["analyseDocList"] = new Collection<Dokumentation>();
            }
            var dl = (ICollection<Dokumentation>)Session["analyseDocList"];
            var dd =
              from d in dl
              select new DokumentationModel
              {
                  Id = d.Id,
                  FilNavn = d.Filnavn,
                  FilNavnUrlEncode = HttpUtility.UrlEncode(d.Filnavn),
                  Type = d.DokumentationType.Navn,
                  OprindelseDato = d.OprindelsesDato,
                  AnmeldelseId = ((d.Jord != null && d.Jord.Anmeldelse != null) ? d.Jord.Anmeldelse.Id : Guid.Empty)
              };
            var result = dd.ToDataSourceResult(request);
            return Json(result);
        }


        [HttpPost]
        public ActionResult ReadInteresanter([DataSourceRequest] DataSourceRequest request)
        {
            if (Session["Interesanter"] != null)
            {
                var interesants = (HashSet<Interesant>)Session["Interesanter"];
                var interesantModel = ConvertInteresantToModel(interesants);
                var result = interesantModel.ToDataSourceResult(request);
                return Json(result);
            }
            return Json(null);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateInteresant([DataSourceRequest] DataSourceRequest request, InteresantModel interesant)
        {
            if (interesant != null && ModelState.IsValid)
            {
                if (Session["Interesanter"] == null)
                    Session["Interesanter"] = new HashSet<Interesant>();

                var interesants = (HashSet<Interesant>)Session["Interesanter"];

                //var interesants = new HashSet<Interesant>();

                ////Henter interesanter fra databasen
                //var a = _anmeldelserBusiness.Read(anmeldelseId);
                //if (a != null && a.Interesant != null && a.Interesant.Any())
                //  interesants.AddRange(a.Interesant);

                var i = new Interesant { Id = interesant.Id, Navn = interesant.Navn, Email = interesant.Email };
                interesants.Add(i);
                Session["Interesanter"] = interesants;
                //return Json(new { res = result }, "text/plain");
                interesant.Id = Guid.NewGuid();
            }

            return Json(new[] { interesant }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemoveInteresant([DataSourceRequest] DataSourceRequest request, InteresantModel interesant)
        {
            if (interesant != null)
            {
                if (Session["Interesanter"] != null)
                {
                    var interesants = (HashSet<Interesant>)Session["Interesanter"];
                    foreach (var i in interesants)
                    {
                        if (i.Email == interesant.Email)
                            interesants.Remove(i);
                    }
                    Session["Interesanter"] = interesants;
                }
            }
            return Json(new[] { interesant }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult ReadBetalere([DataSourceRequest] DataSourceRequest request, string searchstring)
        {
            var t = ConvertPersonToModel(_personBusiness.SearchByEmail(searchstring));
            var result = t.ToDataSourceResult(request);
            return Json(result);
        }

        [HttpPost]
        public ActionResult ReadValgtBetaler(string personId)
        {
            Guid g;
            if (Guid.TryParse(personId, out g))
            {
                var p = _personBusiness.Read(g);
                var pp = new List<Person> { p };

                var pm = ConvertPersonToModel(pp);
                return Json(pm);
            }
            return null;
        }

        #endregion

        [AllowAnonymous]
        public ActionResult VisAnmeldelse(string id)
        {
            var opretModel = new OpretModel();
            Guid anmeldelseId = Guid.Empty;
            Session["docList"] = null;
            Session["analyseDocList"] = null;

            if (Guid.TryParse(id, out anmeldelseId))
            {
                opretModel.Anmeldelse = _anmeldelserBusiness.Read(anmeldelseId);
                if (opretModel.Anmeldelse == null)
                    return View();
                /* 09-04-2025 TOK: Det bør være muligt at se afsluttede anmeldelser */
                /* var status = _statusAnmeldelseBusiness.GetFormattedStatusListForGrids(opretModel.Anmeldelse);
                if (status == "Afsluttet")
                    return View();
                */

                if (opretModel.Anmeldelse != null && opretModel.Anmeldelse.Oprindelsessted != null &&
                opretModel.Anmeldelse.Oprindelsessted.Geom != null)
                {
                    opretModel.OprindelsesstedWkt = opretModel.Anmeldelse.Oprindelsessted.Geom.WellKnownValue.WellKnownText;
                    opretModel.MapStedLx = opretModel.GetMapBoundLowerX(opretModel.OprindelsesstedWkt);
                    opretModel.MapStedLy = opretModel.GetMapBoundLowerY(opretModel.OprindelsesstedWkt);
                    opretModel.MapStedUx = opretModel.GetMapBoundUpperX(opretModel.OprindelsesstedWkt);
                    opretModel.MapStedUy = opretModel.GetMapBoundUpperY(opretModel.OprindelsesstedWkt);

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
                    }
                }

                if (opretModel.Anmeldelse.ModtagerAnlaeg != null)
                    opretModel.SelectedModtagerAnlaeg =
                        ConvertModtagerAnlaegToModel(new List<ModtagerAnlaeg>() {opretModel.Anmeldelse.ModtagerAnlaeg},
                            opretModel.Anmeldelse.ModtagerAnlaeg.Geom);



                if (opretModel.Anmeldelse != null && opretModel.Anmeldelse.Jord != null && opretModel.Anmeldelse.Jord.Dokumentation.Count > 0)
                { 
                    Session["docList"] = opretModel.Anmeldelse.Jord.Dokumentation.ToArray();

                    var dl = (ICollection<Dokumentation>)Session["docList"];
                    var nyDocList = new Collection<Dokumentation>();
                    foreach (var dokumentation in dl)
                    {
                        var d = new Dokumentation();
                        d = _anmeldelserBusiness.AttributeCopyDokumenation(dokumentation, d);
                        nyDocList.Add(d);
                    }
                    Session["docList"] = nyDocList;
                }


                return View(opretModel);
            }

            return View();
        }


        [HttpPost]
        public Guid OpretBetaler(OpretModel model, string brugernavn)
        {
            brugernavn = brugernavn == null ? model.betalerNy : brugernavn;

            Guid result = Guid.Empty;
            var betaler = _brugerBusiness.GetBrugerProfilList().FirstOrDefault(x => x.BrugerNavn.ToLower() == brugernavn.ToLower());
            if (betaler == null)
            {
                var brugerProfil = new BrugerProfil();
                brugerProfil.Person = new Person();
                brugerProfil.Person.Navn = model.betalerFornavn;
                brugerProfil.Person.Efternavn = model.betalerEternavn;
                brugerProfil.Person.Adresse = model.betalerAdresse;
                brugerProfil.Person.Postnummer = model.betalerPostnummer;
                brugerProfil.Person.Postdistrikt = model.betalerBy;
                brugerProfil.Person.Telefon = model.betalerTelefon;
                brugerProfil.Person.Mobiltelefon = model.betalerMobiltelefon;
                brugerProfil.Person.Email = brugernavn;
                brugerProfil.BrugerNavn = brugernavn;
                brugerProfil.Person.Aktiv = true;

                if (model.betalerCompany)
                {
                    brugerProfil.Person.Firmaoplysninger = new Firmaoplysninger();
                    brugerProfil.Person.Firmaoplysninger.Firmanavn = model.firmaNavn;
                    brugerProfil.Person.Firmaoplysninger.Adresse = model.firmaAdresse;
                    brugerProfil.Person.Firmaoplysninger.Postnummer = model.firmaPostnummer;
                    brugerProfil.Person.Firmaoplysninger.Postdistikt = model.firmaBy;
                    brugerProfil.Person.Firmaoplysninger.CVR = model.firmaCVR.Value;
                    brugerProfil.Person.Firmaoplysninger.EAN = model.firmaEAN;
                    brugerProfil.Person.Firmaoplysninger.PNummer = model.firmaPNummer;
                }

                _brugerBusiness.CreateBetaler(brugerProfil);

                result = brugerProfil.Person.Id;
            }
            else
            {
                result = betaler.Person.Id;
            }
            return result;
        }

        

        //KFK eksperiment
        [HttpPost]
        public Guid OpretBruger(string brugernavn,string fornavn, string efternavn, string adresse, Int32 postnummer,string by,string telefon, string mobiltelefon,bool firma,string firmaNavn, string firmaAdresse, string firmaBy, string firmaPostnummer,string cvr, string ean,string pNummer, string besked)
        {
            //brugernavn = brugernavn == null ? model.NyBrugerModel.betalerNy : brugernavn;

            Guid result = Guid.Empty;
            //var betaler = _brugerBusiness.GetBrugerProfilList().FirstOrDefault(x => x.BrugerNavn.ToLower() == brugernavn.ToLower());
          
            var brugerProfil = new BrugerProfil();
            brugerProfil.Person = new Person();
            brugerProfil.Person.Navn = fornavn;
            brugerProfil.Person.Efternavn = efternavn;
            brugerProfil.Person.Adresse = adresse;
            brugerProfil.Person.Postnummer = postnummer;
            brugerProfil.Person.Postdistrikt = by;
            brugerProfil.Person.Telefon = Int32.Parse(telefon);
            brugerProfil.Person.Mobiltelefon = Int32.Parse(mobiltelefon);
            brugerProfil.Person.Email = brugernavn;
            brugerProfil.BrugerNavn = brugernavn;
            brugerProfil.PasswordClearText = Membership.GeneratePassword(6, 0);
            brugerProfil.Person.Aktiv = true;

            if (firma)
            {
                brugerProfil.Person.Firmaoplysninger = new Firmaoplysninger();
                brugerProfil.Person.Firmaoplysninger.Firmanavn = firmaNavn;
                brugerProfil.Person.Firmaoplysninger.Adresse = firmaAdresse;
                brugerProfil.Person.Firmaoplysninger.Postnummer = Int32.Parse(firmaPostnummer);
                brugerProfil.Person.Firmaoplysninger.Postdistikt = firmaBy;
                brugerProfil.Person.Firmaoplysninger.CVR = Int32.Parse(cvr);
                if (ean.Length != 13)
                {
                    brugerProfil.Person.Firmaoplysninger.EAN = null;
                }
                else
                {
                    brugerProfil.Person.Firmaoplysninger.EAN = Decimal.Parse(ean);  
                }                   
                brugerProfil.Person.Firmaoplysninger.PNummer = pNummer;
            }

            //_brugerBusiness.CreateBetaler(brugerProfil);
            var created =  _brugerBusiness.CreateSilent(brugerProfil, new string[0]);
                

        //SEND BESKED HER!

            result = brugerProfil.Person.Id;          
         
            return result;
        }
        //slut
        [HttpGet]
        public ActionResult Opret()
        {
            var model = new OpretModel();
            model.Anmeldelse = new Anmeldelse();
            model.Anmeldelse.Jord = new Jord();

            Session["Matrikler"] = null;
            Session["docList"] = null;
            Session["docListRev"] = null;
            Session["analyseDocList"] = null;
            Session["Interesanter"] = null;
            Session["Jordforureningsopslag"] = null;
            Session["anmeldelse_kvittering"] = null;

            var b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
            var person = b.Person;

            if (person != null)
            {
                //Ryd op i dokumenter
                //slet indhold af personid mappe, hvis der skulle liggen noget fra en tidligere session.
                _dokumentationBusiness.RemoveTempDirectory(person.Id.ToString());

                //Anmelder
                if (model.Anmeldelse.Anmelder == null)
                    model.Anmeldelse.Anmelder = new Anmelder();

                model.Anmeldelse.Anmelder.Id = person.Id;
                model.Anmeldelse.Anmelder.Person = person;

                //Betaler - Som default er Anmelder også betaler.
                if (model.Anmeldelse.Betaler == null)
                    model.Anmeldelse.Betaler = new Betaler();

                model.Anmeldelse.Betaler.Id = person.Id;
            }
            else
            {
                throw new Exception("Niras.Jordflytning.Controllers.Opret() - Personen (" + _securityProvider.CurrentUser.Identity.Name +
                                    ") ikke fundet i databasen, hvormed anmelder ikke bliver sat.");
            }

            //Kodelister + Stamdata
            model = InitOpretModel(model);

            ////Lås brugerfladen
            model.LockButtonReviderAnmeldelse = true;
            model.LockButtonVisAnmeldelse = true;
            model.LockButtonAfslut = true;

            model.betalerCompany = true;

            ModelState.Clear();
            return View(model);
        }

        [HttpPost]
        public bool SendAnmeldelseTilRaadgiver(string anmeldelseId, string raadgiverEmail, string raadgiverBesked)
        {
            Guid ga;
            if (Guid.TryParse(anmeldelseId, out ga))
            {
                var anmeldelse = _anmeldelserBusiness.Read(ga);
                var b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
                var status = _adviseringBusiness.SendBeskedTilRaadgiver(anmeldelse, raadgiverEmail, raadgiverBesked, b.Person.Navn + " " + b.Person.Efternavn, b.Person.Telefon.ToString());
                return status;
            }

            return false;
        }
        [HttpPost]
        public JsonResult SendJordmodtgaerAngivJordmaengdeIFJ(string anmeldelseId, int? jordmaengdeTons)
        {
            Guid ga;
            bool success = false;
            string message = "";
            if (jordmaengdeTons.HasValue && jordmaengdeTons.Value >= 0 && Guid.TryParse(anmeldelseId, out ga))
            {

                var anmeldelse = _anmeldelserBusiness.Read(ga);
                var anmeldelseFoerAendriger = _anmeldelserBusiness.CloneForLogingPurpose(anmeldelse);
                anmeldelse.TotalMaengdeJordIkkeFJ = jordmaengdeTons;
                var b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
                _anmeldelserBusiness.GemAnmeldelse(anmeldelse, b.Person, anmeldelseFoerAendriger);
                success = true;
            }
            else
            {
                message = "Indtast venligst et gyldigt heltal større end eller lig med 0";
            }
            return Json(new { Success=success, Message=message });
        }

        [HttpPost]
        public ActionResult GetJordKlassifikationerForKommuneAndenOprindelse(OpretModel opretModel)
        {
            if (opretModel != null)
            {
                Guid kGuid;
                if (!string.IsNullOrEmpty(opretModel.selectedAndenOprindelsesstedKommune) && Guid.TryParse(opretModel.selectedAndenOprindelsesstedKommune, out kGuid))
                {
                    opretModel.Anmeldelse.Kommune.Id = kGuid;

                    //var jks = _kodelisteBusiness.ReadAktiveJordKlassifikationTypesForKommune(kGuid);
                    //opretModel.JordklassifikationTypeListe = jks;
                    //ModelState.Clear();

                    //Konfig
                    opretModel.KonfigInfoKommuneWwwVedrJordflyt = _konfigBusiness.ReadKommuneKonfig(EnumKonfigKey.InfoKommuneWwwVedrJordflyt.ToString(), kGuid);
                    //link til kommunes hjemmeside vedr. flytning af jord


                    var jks = _kodelisteBusiness.ReadAktiveJordKlassifikationTypesForKommune(kGuid);
                    opretModel.JordklassifikationTypeListe = jks;
                    ModelState.Clear();

                    return PartialView("_Jordjordklassifikation", opretModel);
                }
            }
            return null;
        }

        [HttpPost]
        public ActionResult GetJordKlassifikationerForKommune(OpretModel opretModel)
        {
            if (opretModel != null)
            {
                if (opretModel.Anmeldelse != null && opretModel.Anmeldelse.Kommune != null && !string.IsNullOrEmpty(opretModel.Anmeldelse.Kommune.Navn))
                {
                    var kommuneNavn = opretModel.Anmeldelse.Kommune.Navn;
                    var kommunenr = opretModel.Anmeldelse.Kommune.Kommunenr;

                    var jks = new List<JordKlassifikationType>();

                    if (kommunenr > 0)
                    {
                        jks = _kodelisteBusiness.ReadAktiveJordKlassifikationTypesForKommune(kommunenr).ToList();
                    }
                    else
                    {
                        jks = _kodelisteBusiness.ReadAktiveJordKlassifikationTypesForKommune(kommuneNavn).ToList();    
                    }
                    
                    opretModel.JordklassifikationTypeListe = jks;
                    ModelState.Clear();
                    return PartialView("_Jordjordklassifikation", opretModel);
                }
                opretModel.ForureningOpslagFejlbesked = "Sted - Der skal bestemmes en kommune ved angivelse af adresse eller vej.";
                return PartialView("_StedJordforuningOpslag", opretModel);
            }
            return null;
        }

        public JsonResult GetJordKlassifikationerForKommuneTilMidlertidigtAnlaeg(int kommunekode)
        {
            var result = new List<JordKlassifikationType>();
            if (kommunekode != 0)
                result.AddRange(_kodelisteBusiness.ReadAktiveJordKlassifikationTypesForKommune((short)kommunekode).OrderBy(i => i.Sortering).ToList());
            return Json(result.Select(i => new { value=i.Id.ToString(), text=i.Navn, tooltip=i.Tooltip }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetJordForurening(OpretModel opretModel)
        {
            try
            {
                opretModel.ForureningOpslagResultatList = new List<ForureningsOpslagResult>();
                opretModel.ForureningOpslagFejlbesked = "";

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
                            var g = DbGeometry.FromText(wkt, Epsg25832);
                            if (!g.IsValid)
                            {
                                opretModel.ForureningOpslagFejlbesked = "Sted - Det tegnede område er ikke valid";
                                return PartialView("_StedJordforuningOpslag", opretModel);
                            }
                        }
                        catch (Exception ex)
                        {
                            opretModel.ForureningOpslagFejlbesked = "Sted - Der er sket en fejl!<br/>" + ex.Message;
                            return PartialView("_StedJordforuningOpslag", opretModel);

                        }


                        opretModel.Anmeldelse.Oprindelsessted.Geom = DbGeometry.FromText(wkt, Epsg25832);
                        if (opretModel.Anmeldelse.Oprindelsessted.Geom.Area < 10000)
                        {
                            //Henter matrikler fra wfs service
                            var matrikler = _matrikelBusiness.ReadMatrikler(wkt);
                            Session["Matrikler"] = matrikler;
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
                          "Der er sket en fejl! System administratoren er blevet underrettet - Prøv eventuelt igen lidt senere.";
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

                opretModel.ForureningOpslagFejlbesked = "";
                if (ka != null)
                {
                    opretModel.ForureningOpslagJordklassifikationTypeId = ka.Id;
                    opretModel.SelectedJordklassifikationType = ka.Id;
                }
                opretModel.ForureningOpslagKraeverAnmeldelse = forureningOpslag.HasAnmeldePligt(kommune.Kommunenr, oprindelsesstedKlassificeringTypeKode);

                //Gemmer Jordforureningsopslaget
                var opslag = new Jordforureningsopslag();
                opslag.Id = opretModel.Anmeldelse.Id;
                //opslag.JordKlassifikationType = ka;
                opslag.JordKlassifikationTypeId = ka.Id;
                opslag.Tid = DateTime.Now;

                //Tjekker om listen indeholder en bestemt type 
                var v2 = (from k in forureningopslagResult
                          where k.MiljoePortalKlassifikation != null
                                && k.MiljoePortalKlassifikation.Id == (int)MiljoePortalKlassifikationEnum.JordforureningV2
                          select k).FirstOrDefault();
                opslag.V2 = v2 != null;

                var v1 = (from k in forureningopslagResult
                          where k.MiljoePortalKlassifikation != null
                                && k.MiljoePortalKlassifikation.Id == (int)MiljoePortalKlassifikationEnum.JordforureningV1
                          select k).FirstOrDefault();
                opslag.V1 = v1 != null;

                //OMK analysepligt
                var omkAnalysepligt = (from k in forureningopslagResult
                                       where k.MiljoePortalKlassifikation != null
                                             && k.MiljoePortalKlassifikation.Id == (int)MiljoePortalKlassifikationEnum.OmraadeMedMravMmMnalyser
                                       select k).FirstOrDefault();
                opslag.OmkAnalysepligt = omkAnalysepligt != null;

                //OMK Analysefri letforurenet jord
                var omkLet = (from k in forureningopslagResult
                              where k.MiljoePortalKlassifikation != null
                                    && k.MiljoePortalKlassifikation.Id == (int)MiljoePortalKlassifikationEnum.AnalysefritOmraadeLetForurenetJord
                              select k).FirstOrDefault();
                opslag.OmkLet = omkLet != null;

                //OMK ren
                var omkRen = (from k in forureningopslagResult
                              where k.MiljoePortalKlassifikation != null
                                    && k.MiljoePortalKlassifikation.Id == (int)MiljoePortalKlassifikationEnum.AnalysefritOmraadeRenJord
                              select k).FirstOrDefault();
                opslag.OmkRen = omkRen != null;

                //Kommunes miljødatabase (Århus Geoenviron)
                var komMiljoeDb = (from k in forureningopslagResult where k.MiljoePortalKlassifikation == null && k.ResultException == null select k.LongText).FirstOrDefault();
                if (!string.IsNullOrEmpty(komMiljoeDb))
                    opslag.KommunensMiljoeDb = komMiljoeDb;

                //Tjek for exceptions fra opslag i eksterne datakilder
                var resultException = (from e in forureningopslagResult where e.ResultException != null select e.ResultException).ToList();
                if (resultException.Any())
                {
                    opretModel.ForureningOpslagFejlbesked = "Det er ikke muligt at hente oplysning om ejendommens forureningsstatus fra Danmarks Miljøportal eller kommunens database. Har du kendskab til den aktuelle forureningsstatus, kan du udfylde anmeldelsen og sende den til kommunen. Alternativt kan du vente til det igen er muligt at hente disse oplysninger.";
                    opslag.KommunensMiljoeDb = "Forureningsstatus kan ikke hentes fra eksterne datakilder.";//Når der er en besked i dette felt kan anmeldelsen ikke behandles automatisk
                }

                Session["Jordforureningsopslag"] = opslag;
                //Gemmer anmeldelsen i en session til kvitteringssiden. Problemer  med print på denne side pga. IE sikkerheds issue. 
                //Man må ikke oprettet et dokument og skrive indholdet fra en div i det efterfult af print... Derfor denne nødløsning.
                Session["anmeldelse_kvittering"] = opretModel;

                //Konfig
                opretModel.KonfigInfoKommuneWwwVedrJordflyt = _konfigBusiness.ReadKommuneKonfig(EnumKonfigKey.InfoKommuneWwwVedrJordflyt.ToString(), kommune.Id);
                //link til kommunes hjemmeside vedr. flytning af jord

                //Kodelister + Stamdata
                opretModel = InitOpretModel(opretModel);

                ModelState.Clear();
                return PartialView("_StedJordforuningOpslag", opretModel);

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

        public ActionResult Anmeldelse()
        {
            return View();
        }

        private OpretModel CreateOpretModel(Guid anmeldelseId)
        {
            Session["Matrikler"] = null;
            Session["docList"] = null;
            Session["docListRev"] = null;
            Session["analyseDocList"] = null;
            Session["Interesanter"] = null;
            Session["Jordforureningsopslag"] = null;
            Session["anmeldelse_kvittering"] = null;

            var opretModel = new OpretModel();
            opretModel.AdresseOpslagViewMode = true; //Adressekontrollen skal ikke vises fra start af.
            var currentBruger = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
            opretModel.Anmeldelse = _anmeldelserBusiness.Read(anmeldelseId);

            //Anmelder
            //Få gange mangler der en anmelder på en anmeldelse, hvilket medfører fejl. Ved dette tjek sikre jeg mig at anmelderen kommer med.
            if (opretModel.Anmeldelse != null)
            {

                if (opretModel.Anmeldelse.Anmelder == null)
                {
                    var person = currentBruger.Person;
                    opretModel.Anmeldelse.Anmelder = new Anmelder();
                    if (person != null)
                    {
                        opretModel.Anmeldelse.Anmelder.Id = person.Id;
                        opretModel.Anmeldelse.Anmelder.Person = person;
                    }

                }
                if (opretModel.Anmeldelse.Anmelder != null && opretModel.Anmeldelse.Anmelder.Id == Guid.Empty)
                {
                    var person = currentBruger.Person;
                    if (person != null)
                    {
                        opretModel.Anmeldelse.Anmelder.Id = person.Id;
                        opretModel.Anmeldelse.Anmelder.Person = person;
                    }

                }
            }

            //STED 
            //Kodelister
            if (opretModel.Anmeldelse.Oprindelsessted != null &&
                opretModel.Anmeldelse.Oprindelsessted.AndenOprindJordType != null &&
                opretModel.Anmeldelse.Oprindelsessted.AndenOprindJordType.Id != Guid.Empty)
            {
                opretModel.SelectedAndenOprindJordTypeID = opretModel.Anmeldelse.Oprindelsessted.AndenOprindJordType.Id.ToString();
                opretModel.SelectedEjendomMaterialeTypeID = opretModel.Anmeldelse.Oprindelsessted.AndenOprindJordType.Id.ToString();
                opretModel.SelectedOffentligVejMaterialeTypeID =
                    opretModel.Anmeldelse.Oprindelsessted.AndenOprindJordType.Id.ToString();
            }
                    

            if (opretModel.Anmeldelse.Kommune != null)
                opretModel.selectedAndenOprindelsesstedKommune = opretModel.Anmeldelse.Kommune.Id.ToString();

            //JORDEN
            if (opretModel.Anmeldelse.Jord != null && opretModel.Anmeldelse.Jord.AffaldType != null && opretModel.Anmeldelse.Jord.AffaldType.Id != Guid.Empty)
                opretModel.selectedAffaldType = opretModel.Anmeldelse.Jord.AffaldType.Id.ToString();

            var status = _statusAnmeldelseBusiness.GetFormattedStatusListForGrids(opretModel.Anmeldelse);

            //Jordkategori
            if (opretModel.Anmeldelse.Jord != null && opretModel.Anmeldelse.Jord.JordKlassifikationType != null && status == "Aktiv")
                opretModel.SelectedJordklassifikationTypeNavn =
                    opretModel.Anmeldelse.Jord.JordKlassifikationType.Navn;

            if (opretModel.Anmeldelse != null && opretModel.Anmeldelse.Jord != null && opretModel.Anmeldelse.Jord.Dokumentation.Count > 0)
                Session["docList"] = opretModel.Anmeldelse.Jord.Dokumentation;
                
            if (opretModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType != null && opretModel.Anmeldelse.Jordforureningsopslag != null) //NMR MULIGVIS FORKERT MAPNING
            {
                opretModel.ForureningOpslagJordklassifikationTypeId =
                    opretModel.Anmeldelse.Jordforureningsopslag.JordKlassifikationTypeId; //TILRETTET VÆRDI NMR
                    //opretModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType.Id;
            }

            //JF-472: Automatisk opslag i miljødatabaser på kladder
            if (status == "Gemt" && opretModel.Anmeldelse != null && opretModel.Anmeldelse.Oprindelsessted != null &&
            opretModel.Anmeldelse.Oprindelsessted.Geom != null)
            {
                opretModel.OpdaterJordforureningsKategori = true;
                opretModel.JordklassifikationTypeListe = GetJordKlassifikationType(opretModel.Anmeldelse.KommuneId.GetValueOrDefault());
            }

            //TRANSPORTØR MODTAGERANLAEG
            //TransportørId og ModtagerAnlægId anvendes ikke på model.anmeldelse.transportoer og model.anmeldelse.modtageranlaeg, 
            //da disse vil kræve at modtageranlæg og transportør skal være valgt før man kan gemme.
            //Istedet anvendes selectedTransportørId og selectedModtagerAnlaegId
            if (opretModel.Anmeldelse.ModtagerAnlaeg != null && opretModel.Anmeldelse.ModtagerAnlaeg.Id != Guid.Empty)
                opretModel.SelectedModtagerAnlaegId = opretModel.Anmeldelse.ModtagerAnlaeg.Id.ToString();

            if (opretModel.Anmeldelse.Transportoer != null && opretModel.Anmeldelse.Transportoer.Id != Guid.Empty)
                opretModel.SelectedTransportoerId = opretModel.Anmeldelse.Transportoer.Id.ToString();

            //KOMMUNIKATION
            if (opretModel.Anmeldelse != null && opretModel.Anmeldelse.Interesant != null)
                Session["Interesanter"] = opretModel.Anmeldelse.Interesant;

            if (opretModel.Anmeldelse.Sagsbehandler != null && opretModel.Anmeldelse.Sagsbehandler.Person != null)
                opretModel.Sagsbehandler = opretModel.Anmeldelse.Sagsbehandler.Person.Navn + " " + opretModel.Anmeldelse.Sagsbehandler.Person.Efternavn;

            //LÅS BRUGERFLADEN
            //Lås på baggrund af data
            opretModel.LockAnmeldelse = opretModel.Anmeldelse.ModtagerAnlaeg != null && opretModel.Anmeldelse.ModtagerAnlaeg.Jordmodtager != null && currentBruger.Person.Firmaoplysninger != null && (opretModel.Anmeldelse.Anmelder.Person.Firmaoplysninger == null || opretModel.Anmeldelse.Anmelder.Person.Firmaoplysninger.CVR != currentBruger.Person.Firmaoplysninger.CVR);

            Boolean bErJordmodtager = opretModel.Anmeldelse.ModtagerAnlaeg != null && opretModel.Anmeldelse.ModtagerAnlaeg.Jordmodtager != null && opretModel.Anmeldelse.ModtagerAnlaeg.Jordmodtager.CVR != null && currentBruger.Person.Firmaoplysninger != null && opretModel.Anmeldelse.ModtagerAnlaeg.Jordmodtager.CVR ==
                                                    currentBruger.Person.Firmaoplysninger.CVR;
            opretModel.AnmelderErJordmodtager = status == "Aktiv" && bErJordmodtager;
                
            /* TOK 04.12.2019: Der skal være mulighed for at jordmodtagere på afsluttede anmeldelser kan registere tilkørt jordmængde */
            if (status == "Afsluttet" && bErJordmodtager  && opretModel.Anmeldelse.Kommune != null)
                opretModel.AnmelderErJordmodtager = true;

            opretModel.LockButtonSendTilRaadgiver = opretModel.AnmelderErJordmodtager;

            opretModel.IndeholderVognlaes = (opretModel.Anmeldelse.Vognlaes != null && opretModel.Anmeldelse.Vognlaes.Count > 0) ||
                                        opretModel.Anmeldelse.TotalMaengdeJordIkkeFJ.HasValue;

            if (opretModel.LockAnmeldelse)
            {
                opretModel.LockButtonGem = true;
                opretModel.LockButtonIndsend = true;
                opretModel.LockSted = true;
                opretModel.LockJorden = true;
                opretModel.LockModtagerTransportoer = true;
                opretModel.LockBetaler = true;
                opretModel.LockKommunikation = true;
                opretModel.LockButtonGem = true;
                opretModel.LockButtonIndsend = true;
                opretModel.LockButtonVisAnmeldelse = false;
                opretModel.LockButtonReviderAnmeldelse = true;
                opretModel.Revision = false;
                opretModel.LockButtonAfslut = true;
                opretModel.LockButtonGem = true;
                opretModel.LockButtonIndsend = true;
            }
            else
            {

                if ((opretModel.Anmeldelse != null && opretModel.Anmeldelse.Jord != null &&
                        opretModel.Anmeldelse.Jord.JordKlassifikationType == null))
                {
                    //Hvis der ikke er jordklassifikationtype skal man ikke kunne gemme anmeldelsen.
                    opretModel.LockButtonGem = true;
                    opretModel.LockButtonIndsend = true;
                }

                //Hvis anmeldelsen er godkendt af kommune
                if (_statusAnmeldelseBusiness.HasKommuneGodkendt(opretModel.Anmeldelse))
                {
                    opretModel.LockSted = true;
                    opretModel.LockJorden = true;
                    opretModel.LockModtagerTransportoer = true;
                    opretModel.LockBetaler = true;
                    opretModel.LockKommunikation = false;
                    opretModel.LockButtonGem = true;
                    opretModel.LockButtonIndsend = true;
                    opretModel.LockButtonVisAnmeldelse = false;
                }
                else
                {
                    opretModel.LockSted = false;
                    opretModel.LockJorden = false;
                    opretModel.LockModtagerTransportoer = false;
                    opretModel.LockBetaler = false;
                    opretModel.LockKommunikation = false;
                    opretModel.LockButtonGem = false;
                    opretModel.LockButtonIndsend = false;
                    // 1.3.2018: Rettelse vedrørende at blanketten (med vandmærke "Anvist ikke godkendt" skal kunne åbnes, når anmeldelsen er indsendt) 
                    if (_statusAnmeldelseBusiness.IsAnmeldelseIndsendtMenIkkeAktiv(opretModel.Anmeldelse.AnmeldelseEffektivStatus))
                        opretModel.LockButtonVisAnmeldelse = false; 
                    else
                        opretModel.LockButtonVisAnmeldelse = true;

                    // LÅS Indsend på "fremsendte anmeldelser" så det ikke er muligt at indsende en anmeldelse som er indsendt!
                    //if (_statusAnmeldelseBusiness.IsAnmeldelseIndsendtMenIkkeAktiv(opretModel.Anmeldelse.StatusAnmeldelse))
                    //{
                    //    opretModel.LockButtonIndsend = true;
                    //}
                }

                //Hvis anmeldelsen er aktiv eller afsluttet skal anmeldelsen kunne revideres.
                opretModel.LockButtonReviderAnmeldelse = true;
                if (_statusAnmeldelseBusiness.MaaAnmeldelseRevideres(opretModel.Anmeldelse))
                    opretModel.LockButtonReviderAnmeldelse = false;

                //Hvis anmeldelsen er under revision
                opretModel.RevisionAnmeldelse =
                    _anmeldelserBusiness.IsThereARevideretAnmeldelseBaseOnThisAnmeldelse(opretModel.Anmeldelse.Id);
                if (opretModel.RevisionAnmeldelse != null && !opretModel.RevisionAnmeldelse.StatusAnmeldelse.Where(s => s.StatusAnmeldelseTypeId == Guid.Parse("5CF8F32A-A0C3-4442-87C9-36F770D2180B")).ToList().Any())
                {
                    opretModel.Revision = true;
                    opretModel.LockSted = true;
                    opretModel.LockJorden = true;
                    opretModel.LockModtagerTransportoer = true;
                    opretModel.LockBetaler = true;
                    opretModel.LockKommunikation = true;

                    opretModel.LockButtonGem = true;
                    opretModel.LockButtonIndsend = true;
                    opretModel.LockButtonReviderAnmeldelse = true;

                    //opretModel.LockButtonVisAnmeldelse = true; 
                    //Jeg har udkommenteret ovenstående linie, da jeg ikke mener, at "vis anmeldelse" knappen på den oprindelige anmeldelse skal skjules, 
                    //bare fordi der findes uafsluttede reviderede anmeldelser 
                    //KVE 20140730.

                    // Tilføj eventuelle dokumenter på revisionen:
                    if (opretModel.RevisionAnmeldelse.Jord != null && opretModel.RevisionAnmeldelse.Jord.Dokumentation != null && opretModel.RevisionAnmeldelse.Jord.Dokumentation.Any())
                        Session["docListRev"] = opretModel.RevisionAnmeldelse.Jord.Dokumentation.ToList();
                }

                //Hvis anmeldelsen er afsluttet skal den ikke kunne afsluttes igen.
                //Den skal heller ikke kunne gemmes eller indsendesigen, så oplysningerne ændre sig på anmeldelsen.
                if (_statusAnmeldelseBusiness.IsAnmeldelseAfsluttet(opretModel.Anmeldelse))
                {
                    opretModel.LockButtonAfslut = true;
                    opretModel.LockButtonGem = true;
                    opretModel.LockButtonIndsend = true;
                }
                else
                {
                    opretModel.LockButtonAfslut = false;
                }

                if (opretModel.Revision)
                {
                    opretModel.LockButtonAfslut = false;
                    opretModel.LockButtonGem = false;
                    opretModel.LockButtonIndsend = false;
                    opretModel.OpdaterJordforureningsKategori = true;
                }

                // OPDATERE IKKE jord forureningskategori, når det er offentlig vej!
                if (opretModel.OprindelsesstedKlassifikationTypeSelectedIndex == 1)
                    opretModel.OpdaterJordforureningsKategori = false;

                //Hvis jordmodtageranlægget ikke er aktivt skal der gives fejlbesked
                if (opretModel.Anmeldelse.ModtagerAnlaeg != null)
                {
                    if (!opretModel.Anmeldelse.ModtagerAnlaeg.Aktiv ||
                        !opretModel.Anmeldelse.ModtagerAnlaeg.Jordmodtager.Aktiv)
                    {
                        opretModel.JordmodtagerAnlaegErInaktiv = true;

                    }
                }
            }
            return InitOpretModel(opretModel);
        }

        public ActionResult Rediger(string anmeldelseId)
        {
            if (Guid.TryParse(anmeldelseId, out var id))
            {
                var model = CreateOpretModel(id);
                return View("Opret", model);
            }
        
            //Returner til Forsiden
            return RedirectToAction("FrontPage", "Default");
        }


        [HttpPost]
        public ActionResult AfslutAnmeldelse(string anmeldelsesId)
        {
            const string message = "Der skete en fejl!";
            try
            {
                Guid g;
                if (Guid.TryParse(anmeldelsesId, out g))
                {
                    var currentPerson = GetPerson();
                    var newDocs = new List<Dokumentation>();
                    if (Session["analyseDocList"] != null)
                    {
                        var dl = (ICollection<Dokumentation>)Session["analyseDocList"];                        
                        foreach (var dd in dl)
                        {
                            var d = new Dokumentation();
                            d.Filnavn = dd.Filnavn;
                            d.DokumentationType = _kodelisteBusiness.ReadDokumentationType(dd.DokumentationType.Id);
                            d.DokumentationTypeId = dd.DokumentationTypeId;
                            d.OprindelsesDato = dd.OprindelsesDato;
                            d.JordId = dd.JordId;
                            d.Id = dd.Id;
                            newDocs.Add(d);
                        }
                    }
                    var res = _anmeldelserBusiness.AfslutAnmeldelse(g, currentPerson, newDocs);
                    var mes = res ? "Anmeldelsen er afsluttet" : "Anmeldelsen kunne ikke afsluttes";

                    if (res == true)
                        _dokumentationBusiness.MoveTempDokumentationToAnmeldelseFolder(currentPerson.Id, g);

                    ViewBag.Message = mes;

                    ViewBag.Message = mes;
                    return Json(new { Success = true, Message = mes });
                }
                return Json(new { Success = false, Message = message });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { Success = false, Message = message });
            }
        }

        private enum OpretHandling
        {
            Ignorer = 0,
            Gem = 1,
            Revider = 2,
            Gennemse = 3,
            Indsend = 4,
        }

        [HttpPost]
        public ActionResult Opret(OpretModel opretModel, string command)
        {
            var b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
            var personSomErLoggetInd = b.Person;
            Guid? conditionalRefreshAnmeldelseId = null;

            var handling = OpretHandling.Ignorer;
            if (command == null)
                return Opret();

            switch (command)
            {
                case "Gem":
                    handling = OpretHandling.Gem;
                    break;
                case "Revider anmeldelse":
                    handling = OpretHandling.Revider;
                    break;
                case "Gennemse":
                    handling = OpretHandling.Gennemse;
                    break;
                case "Indsend":
                    handling = OpretHandling.Indsend;
                    break;
            }

            if (handling == OpretHandling.Ignorer)
                return Opret();

            // JF-430
            Anmeldelse revision = null;
            if (opretModel.Revision && opretModel.RevisionAnmeldelse != null)
            {
                // Dette kode giver nogle problemer, derfor er det nødvendigt at lave en fuld genindlæsning af modellen bagefter.
                conditionalRefreshAnmeldelseId = opretModel.Anmeldelse.Id;
                revision = opretModel.RevisionAnmeldelse;
                opretModel.Anmeldelse = _anmeldelserBusiness.IsThereARevideretAnmeldelseBaseOnThisAnmeldelse(opretModel.Anmeldelse.Id);
            }

            //Kommune
            Guid komId;
            if (opretModel.OprindelsesstedKlassifikationTypeSelectedIndex != 2 && opretModel.Anmeldelse != null && opretModel.Anmeldelse.Kommune != null &&
                !string.IsNullOrEmpty(opretModel.Anmeldelse.Kommune.Navn))
            {
                var kom = _kodelisteBusiness.ReadKommuneByNavn(opretModel.Anmeldelse.Kommune.Navn);
                opretModel.Anmeldelse.Kommune = kom;
            }
            else if (opretModel.OprindelsesstedKlassifikationTypeSelectedIndex == 2 && opretModel.Anmeldelse != null &&
                     opretModel.selectedAndenOprindelsesstedKommune != null && Guid.TryParse(opretModel.selectedAndenOprindelsesstedKommune, out komId))
            {
                var kom = _kodelisteBusiness.ReadKommuneById(komId);
                opretModel.Anmeldelse.Kommune = kom;
                opretModel.Anmeldelse.Oprindelsessted.Adresse = "Anden oprindelse";
            }

            //Anmelder
            //Få gange mangler der en anmelder på en anmeldelse, hvilket medfører fejl. Ved dette tjek sikre jeg mig at anmelderen kommer med.
            if (opretModel.Anmeldelse != null)
            {
                if (opretModel.Anmeldelse.Anmelder == null)
                {
                    var person = b.Person;
                    opretModel.Anmeldelse.Anmelder = new Anmelder();
                    if (person != null)
                    {
                        opretModel.Anmeldelse.Anmelder.Id = person.Id;
                        opretModel.Anmeldelse.Anmelder.Person = person;
                    }
                    else
                    {
                        ModelState.AddModelError("Anmelder", @"Anmelder - Log venligst ind igen");
                        Logger.LogWarning("OBS - Det er ikke muligt at finde brugeren, som er logget ind. _securityProvider.CurrentUser.Identity.Name: " + _securityProvider.CurrentUser.Identity.Name);
                    }


                }
                if (opretModel.Anmeldelse.Anmelder != null && opretModel.Anmeldelse.Anmelder.Id == Guid.Empty)
                {
                    var person = b.Person;
                    if (person != null)
                    {
                        opretModel.Anmeldelse.Anmelder.Id = person.Id;
                        opretModel.Anmeldelse.Anmelder.Person = person;
                    }

                }
                // Case hvor personen manglede
                if (opretModel.Anmeldelse.Anmelder.Person == null && b.Person != null)
                    opretModel.Anmeldelse.Anmelder.Person = b.Person;
            }

            Anmeldelse anmeldelseFoerAendringer = null;
            if (opretModel.Anmeldelse != null)
                anmeldelseFoerAendringer = _anmeldelserBusiness.Read(opretModel.Anmeldelse.Id);

            //Validation
            /* KTW 2020-01-17:
             * Validering fejlede ved manglende oprindelsessted på anmeldelse, hvis det var tale om en revision.
             * Oprindelsessted valideres nu ikke for disse, da dette alligevel ikke kan ændres.
             */
            if (!opretModel.Revision)
                ValiderStedBasic(opretModel);

            ValiderModtagerTransportoer(opretModel, anmeldelseFoerAendringer);
            if (handling == OpretHandling.Gennemse || handling == OpretHandling.Indsend) //Hvis brugeren forsøger at afsende anmeldelsen skal den være tip top valideret
            {
                if (opretModel.Revision)
                {
                    ValiderJordRevision(opretModel);
                }
                else // Normal validering
                {
                    ValiderStedVedAfsend(opretModel);
                    ValiderJord(opretModel);
                    ValiderModtagerTransportoerVedAfsend(opretModel);
                    ValiderBetaler(opretModel);
                }
            }

            // Hvis valideringen er god, kan vi fortsætte
            if (ModelState.IsValid)
            {
                try
                {
                    // Revider tjek:
                    if (handling == OpretHandling.Revider && opretModel.Anmeldelse != null)
                    {
                        var oprindeligeAnmeldelseId = opretModel.Anmeldelse.Id;
                        OpretRevAnmeldelse(opretModel);

                        //Reload siden. Man kan ikke nøjes med partial update.
                        return JavaScript("window.location = '../Anmeldelser/Rediger?anmeldelseId=" + oprindeligeAnmeldelseId + "'");
                    }

                    // Tid til at gemme
                    opretModel = GemAnmeldelse(opretModel, revision);
                    ViewBag.Message = "Anmeldelsen er gemt";

                    // JavaScript gemmer denne først
                    if (handling == OpretHandling.Gennemse)
                    {
                        // Frisk model fra rediger kaldet
                        if (conditionalRefreshAnmeldelseId.HasValue)
                            opretModel = CreateOpretModel(conditionalRefreshAnmeldelseId.Value);
                        else
                            opretModel = CreateOpretModel(opretModel.Anmeldelse.Id);

                        // Manglende opslag til gennemsyn
                        if (opretModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType.Kode != 2 && opretModel.Anmeldelse.Jord != null && opretModel.Anmeldelse.Kommune != null)
                        {
                            opretModel.JordklassifikationTypeListe = GetJordKlassifikationType(opretModel.Anmeldelse.Kommune.Id);

                            var forureningsOpslag = _forureningOpslagBusiness
                                .Read(opretModel.Anmeldelse.Oprindelsessted, opretModel.Anmeldelse.Kommune.Id)
                                .GetResultList();
                            opretModel.ForureningsKategoriTekster.AddRange(forureningsOpslag.Select(x => x.ShortText).Distinct());
                        }

                        return PartialView("_Gennemse", opretModel);
                    }

                    // Indsend
                    if (handling == OpretHandling.Indsend)
                    {
                        //Tjek om betaler er godkendt af jordmodtagers bogholder.
                        var anmodningOmGodkendelse = _statusBetalerBusiness.AnmodOmGodkendelseHosJordmodtager(opretModel.Anmeldelse.Betaler, opretModel.Anmeldelse.ModtagerAnlaeg.Jordmodtager);
                        if (anmodningOmGodkendelse == false)
                        {
                            if (opretModel.ValideringList == null)
                                opretModel.ValideringList = new List<string>();

                            opretModel.ValideringList.Add("Jordmodtager har afvist betaleren!<br/>");

                            opretModel = InitOpretModel(opretModel);
                            ModelState.Clear();

                            return PartialView("_Validering", opretModel);
                        }

                        // Opret Alarmer - Der oprettes alarmer mht. % kørt jord
                        _anmeldelserBusiness.CreateAlarmMaengdeKoertJord(opretModel.Anmeldelse);

                        // Automatiske procedure som sætter diverse statusser.
                        _anmeldelserBusiness.IndsendAnmeldelseAutomatiskeProcedurer(opretModel.Anmeldelse, personSomErLoggetInd);

                        // Når anmeldelse afsendes, skal FlagSagsbehandler være NULL! 
                        // MEN hvis anmeldelse er indsendt en gang, så skal vi ikke fjerne flag.
                        if (!_statusAnmeldelseBusiness.IsAnmeldelseIndsendtMenIkkeAktiv(opretModel.Anmeldelse.AnmeldelseEffektivStatus))
                            _anmeldelserBusiness.GemAnmeldelseFjernFlagSagsbehandler(opretModel.Anmeldelse);

                        // Returner til Forsiden
                        return JavaScript("window.location = '../Default/Startside'");

                    }

                    // Kodelister + Stamdata - Eller genindlæs om nødvendigt.
                    // Den anvendte udskiftning af anmelseler på modellen fra JF-430, gav en del problemer.
                    // Man kunne f.eks. ikke gemme 2 gange, uden at få en exception ved indsend bagefter.
                    if (conditionalRefreshAnmeldelseId.HasValue)
                        opretModel = CreateOpretModel(conditionalRefreshAnmeldelseId.Value);
                    else
                        opretModel = InitOpretModel(opretModel);

                    ModelState.Clear();
                    return PartialView("_Validering", opretModel);
                }
                catch (Exception ex)
                {
                    //Loging
                    Logger.LogException(ex);
                    //Errormessage
                    opretModel.ValideringList = new List<string>();
                    opretModel.ValideringList.Add("Der er sket en fejl!<br/>" + ex.Message);
                    ModelState.Clear();
                    opretModel = InitOpretModel(opretModel);
                    ModelState.Clear();
                    return PartialView("_Validering", opretModel);
                }
            }
            //Validering
            var mesValidering = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).OrderBy(e => e).ToList();
            opretModel.ValideringList = mesValidering;
            ModelState.Clear();

            return PartialView("_Validering", opretModel);
        }

        [HttpPost]
        public ActionResult SaveFile(IEnumerable<HttpPostedFileBase> files, Guid dokumentationTypeId, DateTime metadataDato, string anmeldelseId)
        {
            var result = false;
            var person = GetPerson();
            if (person != null)
            {
                if (Session["docList"] == null)
                {
                    //slet indhold af personid mappe, hvis der skulle ligge noget fra en tidligere session.
                    _dokumentationBusiness.RemoveTempDirectory(person.Id.ToString());
                    Session["docList"] = new Collection<Dokumentation>();
                }
                // The Name of the Upload component is "attachments" 
                var dl = (ICollection<Dokumentation>)Session["docList"];
                foreach (var file in files)
                {
                    var dok = _dokumentationBusiness.CreateTempDokument(person.Id.ToString(), anmeldelseId, file, dokumentationTypeId, metadataDato);
                    dl.Add(dok);
                }
                Session["docList"] = dl;
                result = true;
            }
            return Json(new { res = result }, "text/plain");
        }

        [HttpPost]
        public ActionResult SaveFileForRevision(IEnumerable<HttpPostedFileBase> files, Guid dokumentationTypeId, DateTime metadataDato, string anmeldelseId)
        {
            var result = false;
            var person = GetPerson();
            if (person != null)
            {
                if (Session["docListRev"] == null)
                {
                    //slet indhold af personid mappe, hvis der skulle ligge noget fra en tidligere session.
                    _dokumentationBusiness.RemoveTempDirectory(person.Id.ToString());
                    Session["docListRev"] = new Collection<Dokumentation>();
                }
                // The Name of the Upload component is "attachments" 
                var dl = (ICollection<Dokumentation>)Session["docListRev"];
                foreach (var file in files)
                {
                    var dok = _dokumentationBusiness.CreateTempDokument(person.Id.ToString(), anmeldelseId, file, dokumentationTypeId, metadataDato);
                    dl.Add(dok);
                }
                Session["docListRev"] = dl;
                result = true;
            }
            return Json(new { res = result }, "text/plain");
        }


        [HttpPost]
        public ActionResult SaveFileAnalysedocs(IEnumerable<HttpPostedFileBase> files, Guid dokumentationTypeId, DateTime metadataDato, string anmeldelseId)
        {
            var result = false;
            var person = GetPerson();
            if (person != null)
            {
                if (Session["analyseDocList"] == null)
                {
                    //slet indhold af personid mappe, hvis der skulle ligge noget fra en tidligere session.
                    _dokumentationBusiness.RemoveTempDirectory(person.Id.ToString());
                    Session["analyseDocList"] = new Collection<Dokumentation>();
                }
                // The Name of the Upload component is "attachments" 
                var dl = (ICollection<Dokumentation>)Session["analyseDocList"];
                foreach (var file in files)
                {
                    var dok = _dokumentationBusiness.CreateTempDokument(person.Id.ToString(), anmeldelseId, file, dokumentationTypeId, metadataDato);
                    dl.Add(dok);
                }
                Session["analyseDocList"] = dl;
                result = true;
            }
            return Json(new { res = result }, "text/plain");
        }


        //[Authorize]
        [AllowAnonymous]
        public FileStreamResult DownloadDokumentation(Guid anmeldelseId, string filnavn)
        {
            var b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
            Guid personId = Guid.Empty;
            if (b != null && b.Person != null)
                personId = b.Person.Id;

            var fs = _dokumentationBusiness.ReadDokumentation(personId, anmeldelseId, filnavn);
            if (fs != null && fs.Length > 0)
                return File(fs, "application/octet-stream", filnavn); //d.Filnavn
            return null;
        }

        //[Authorize]
        [AllowAnonymous]
        public FileStreamResult DownloadHistoriskDokumenter(Guid anmeldelseId, Guid stikproeveId, string filnavn)
        {
            var b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
            Guid personId = Guid.Empty;
            if (b != null && b.Person != null)
                personId = b.Person.Id;

            if (anmeldelseId != Guid.Empty)
            {
                var fs = _dokumentationBusiness.ReadDokumentation(personId, anmeldelseId, filnavn);
                if (fs != null && fs.Length > 0)
                    return File(fs, "application/octet-stream", filnavn); //d.Filnavn
            }

            if (stikproeveId != Guid.Empty)
            {
                var fs = _analyseDokumentBusiness.ReadAnalyseDokument(stikproeveId, filnavn);
                if (fs != null && fs.Length > 0)
                    return File(fs, "application/octet-stream", filnavn); //d.Filnavn
            }
            
            return null;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemoveFile([DataSourceRequest] DataSourceRequest request, DokumentationModel dokumentation)
        {
            if (dokumentation != null)
            {
                if (Session["docList"] == null)
                    Session["docList"] = new Collection<Dokumentation>();
                var dl = (ICollection<Dokumentation>)Session["docList"];

                foreach (var d in dl)
                {
                    if (d.Filnavn != dokumentation.FilNavn)
                        continue;

                    var b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
                    var person = b.Person;
                    if (person == null)
                        continue;

                    if (!_dokumentationBusiness.RemoveDokumentation(person.Id, dokumentation.AnmeldelseId, dokumentation.FilNavn, dokumentation.Id))
                        continue;

                    dl.Remove(d);
                    Session["docList"] = dl;
                    break;
                }
            }
            return Json(ModelState.ToDataSourceResult());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemoveFileFromRevision([DataSourceRequest] DataSourceRequest request, DokumentationModel dokumentation)
        {
            if (dokumentation != null)
            {
                if (Session["docListRev"] == null)
                    Session["docListRev"] = new Collection<Dokumentation>();
                var dl = (ICollection<Dokumentation>)Session["docListRev"];

                foreach (var d in dl)
                {
                    if (d.Filnavn != dokumentation.FilNavn)
                        continue;

                    var b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
                    var person = b.Person;
                    if (person == null)
                        continue;

                    if (!_dokumentationBusiness.RemoveDokumentation(person.Id, dokumentation.AnmeldelseId, dokumentation.FilNavn, dokumentation.Id))
                        continue;

                    dl.Remove(d);
                    Session["docListRev"] = dl;
                    break;
                }
            }
            return Json(ModelState.ToDataSourceResult());
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemoveFileAnalysedocs([DataSourceRequest] DataSourceRequest request, DokumentationModel dokumentation)
        {
            if (dokumentation != null)
            {
                if (Session["analyseDocList"] == null)
                    Session["analyseDocList"] = new Collection<Dokumentation>();
                var dl = (ICollection<Dokumentation>)Session["analyseDocList"];

                foreach (var d in dl)
                {
                    if (d.Filnavn != dokumentation.FilNavn)
                        continue;

                    var b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
                    var person = b.Person;
                    if (person == null)
                        continue;

                    if (!_dokumentationBusiness.RemoveDokumentation(person.Id, dokumentation.AnmeldelseId, dokumentation.FilNavn, dokumentation.Id))
                        continue;

                    dl.Remove(d);
                    Session["analyseDocList"] = dl;
                    break;
                }
            }
            return Json(ModelState.ToDataSourceResult());
        }


        [AllowAnonymous]
        public ActionResult Blanket(string anmeldelseId, string printStyle = "0")
        {
            //Temp under udvikling
            //var anmeldelseId = "86AC3DB2-C70E-4BD8-9FE3-0EC0B1924606";
            //anmeldelseId = "b945d59e-4799-40f6-a4d4-fb504010d7de";//med matrikler

            var model = new SagsbehandlingModel();
            model.AnmeldelseModel = new OpretModel();

            Guid g;
            if (Guid.TryParse(anmeldelseId, out g))
            {
                model.AnmeldelseModel.Anmeldelse = _anmeldelserBusiness.Read(g);
            }
            //PrintStyle -
            //PDf service kan ikke lide width på body. Derfor fjernes dette med dette triks.
            if (printStyle == "1")
                ViewBag.BlanketPrintStyle = true;

            //HEADER
            model.BlanketLogo = _konfigBusiness.ReadKommuneKonfig("BlanketLogo", model.AnmeldelseModel.Anmeldelse.Kommune.Id);
            model.BlanketFooter = _konfigBusiness.ReadKommuneKonfig("BlanketFooter", model.AnmeldelseModel.Anmeldelse.Kommune.Id);

            //var aktiv = _statusAnmeldelseBusiness.IsAnmeldelseAktiv(model.AnmeldelseModel.Anmeldelse);
            model.AnmeldelseModel.AnmeldelseErAktiv = _statusAnmeldelseBusiness.IsAnmeldelseAktiv(model.AnmeldelseModel.Anmeldelse);
            
            //BLANKET
            var indsendDato = _statusAnmeldelseBusiness.GetTimeForIndsend(model.AnmeldelseModel.Anmeldelse.AnmeldelseEffektivStatus);
            model.BlanketAnmeldelseAfsendt = indsendDato.HasValue ? indsendDato.Value.ToShortDateString() + " " + indsendDato.Value.ToShortTimeString() : "";

            _statusAnmeldelseBusiness.GetLastStatus(model.AnmeldelseModel.Anmeldelse.AnmeldelseEffektivStatus, out var lastStatus, out var lastStatusTid);
            model.BlanketAnmeldelseÆndret = lastStatus != EnumStatusAnmeldelse.Ukendt ? lastStatusTid.ToShortDateString() + " " + lastStatusTid.ToShortTimeString() : "";
            model.BlanketAnmeldelseGodkendt = _statusAnmeldelseBusiness.GetAktivDato(model.AnmeldelseModel.Anmeldelse.AnmeldelseEffektivStatus);

            model.BlanketAnmeldelseErIndsendtMenIkkeAktiv = _statusAnmeldelseBusiness.IsAnmeldelseIndsendtMenIkkeAktiv(model.AnmeldelseModel.Anmeldelse.AnmeldelseEffektivStatus);


            model.BlanketAnmeldelseAfvist = _statusAnmeldelseBusiness.GetAfvistDato(model.AnmeldelseModel.Anmeldelse.AnmeldelseEffektivStatus);

            if (model.AnmeldelseModel.Anmeldelse.AnmeldelseEffektivStatus.Afsluttet.HasValue)
                model.BlanketAnmeldelseAfsluttet = model.AnmeldelseModel.Anmeldelse.AnmeldelseEffektivStatus.Afsluttet.Value.ToShortDateString();
            else
                model.BlanketAnmeldelseAfsluttet = "";


            if (model.AnmeldelseModel.Anmeldelse != null
                && model.AnmeldelseModel.Anmeldelse.Sagsbehandler != null
                && model.AnmeldelseModel.Anmeldelse.Sagsbehandler.Person != null)
            {
                model.BlanketSagsbehandler = model.AnmeldelseModel.Anmeldelse.Sagsbehandler.Person.Navn + " " +
                                             model.AnmeldelseModel.Anmeldelse.Sagsbehandler.Person.Efternavn;
            }
            else
            {
                if (model.AnmeldelseModel.Anmeldelse.Kommune != null)
                    model.BlanketSagsbehandler = model.AnmeldelseModel.Anmeldelse.Kommune.Navn;
                else
                    model.BlanketSagsbehandler = "";
            }



            //Anmelder
            if (model.AnmeldelseModel.Anmeldelse != null &&
                model.AnmeldelseModel.Anmeldelse.Anmelder != null &&
                model.AnmeldelseModel.Anmeldelse.Anmelder.Person != null)
            {
                if (model.AnmeldelseModel.Anmeldelse.Anmelder.Person.Firmaoplysninger != null)
                {
                    //Firma
                    model.BlanketAnmelderNavn = model.AnmeldelseModel.Anmeldelse.Anmelder.Person.Firmaoplysninger.Firmanavn;
                    model.BlanketAnmelderAdresse = model.AnmeldelseModel.Anmeldelse.Anmelder.Person.Firmaoplysninger.Adresse;
                    model.BlanketAnmelderPostnr = model.AnmeldelseModel.Anmeldelse.Anmelder.Person.Firmaoplysninger.Postnummer + " " +
                                                  model.AnmeldelseModel.Anmeldelse.Anmelder.Person.Firmaoplysninger.Postdistikt;
                    model.BlanketAnmelderKontaktperson = model.AnmeldelseModel.Anmeldelse.Anmelder.Person.Navn + " " +
                                                         model.AnmeldelseModel.Anmeldelse.Anmelder.Person.Efternavn;
                    model.BlanketAnmelderTelefon = model.AnmeldelseModel.Anmeldelse.Anmelder.Person.Telefon.ToString();
                }
                else
                {
                    //Privat person
                    model.BlanketAnmelderNavn = model.AnmeldelseModel.Anmeldelse.Anmelder.Person.Navn + " " + model.AnmeldelseModel.Anmeldelse.Anmelder.Person.Efternavn;
                    model.BlanketAnmelderAdresse = model.AnmeldelseModel.Anmeldelse.Anmelder.Person.Adresse;
                    model.BlanketAnmelderPostnr = model.AnmeldelseModel.Anmeldelse.Anmelder.Person.Postnummer + " " +
                                                  model.AnmeldelseModel.Anmeldelse.Anmelder.Person.Postdistrikt;
                    model.BlanketAnmelderKontaktperson = "";
                    model.BlanketAnmelderTelefon = model.AnmeldelseModel.Anmeldelse.Anmelder.Person.Telefon.ToString();
                }

            }
            //Oprindelsessted
            if (model.AnmeldelseModel.Anmeldelse != null &&
                model.AnmeldelseModel.Anmeldelse.Oprindelsessted != null)
            {
                if (model.AnmeldelseModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType != null && model.AnmeldelseModel.Anmeldelse.Kommune != null)
                {
                    switch (model.AnmeldelseModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType.Kode)
                    {
                        case 1:
                            {
                                //Ejendom
                                model.BlanketStedAdresse = model.AnmeldelseModel.Anmeldelse.Oprindelsessted.Adresse + ", " +
                                                           model.AnmeldelseModel.Anmeldelse.Oprindelsessted.Postnummer + " " +
                                                           model.AnmeldelseModel.Anmeldelse.Oprindelsessted.PostDistrikt;
                                break;
                            }
                        case 2:
                            {
                                //Offvej
                                model.BlanketStedAdresse = model.AnmeldelseModel.Anmeldelse.Oprindelsessted.Adresse + ", " +
                                                           model.AnmeldelseModel.Anmeldelse.Oprindelsessted.Postnummer + " " +
                                                           model.AnmeldelseModel.Anmeldelse.Oprindelsessted.PostDistrikt;
                                break;
                            }
                        case 3:
                            {
                                //Anden oprind:
                                model.BlanketStedAdresse = model.AnmeldelseModel.Anmeldelse.Oprindelsessted.Beskrivelse;
                                break;
                            }
                    }
                }

                if (model.AnmeldelseModel.Anmeldelse.Oprindelsessted.Matrikel != null && model.AnmeldelseModel.Anmeldelse.Oprindelsessted.Matrikel.Count > 0)
                {
                    foreach (var m in model.AnmeldelseModel.Anmeldelse.Oprindelsessted.Matrikel)
                    {
                        model.BlanketStedMatrikler += m.Matrikelnr + ", " + m.Ejerlavsnavn + "; ";
                    }
                    if (model.AnmeldelseModel.Anmeldelse.Oprindelsessted.Matrikel.Count > 0)
                        model.BlanketStedMatrikler = model.BlanketStedMatrikler.Substring(0, model.BlanketStedMatrikler.Length - 2);
                }
            }

            //OPLYSNINGER OM PROJEKTET. TOK 23.3.2022: Der skal ikke foretages forureningsopslag for Offentlig vej
            if (model.AnmeldelseModel.Anmeldelse != null && model.AnmeldelseModel.Anmeldelse.Jord != null && model.AnmeldelseModel.Anmeldelse.Kommune != null && model.AnmeldelseModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationTypeId.ToString() != "295407e9-2182-4876-ae6f-50afd00a6194")
            {
                model.AnmeldelseModel.JordklassifikationTypeListe = GetJordKlassifikationType(model.AnmeldelseModel.Anmeldelse.Kommune.Id);

                var forureningsOpslag = _forureningOpslagBusiness
                    .Read(model.AnmeldelseModel.Anmeldelse.Oprindelsessted, model.AnmeldelseModel.Anmeldelse.Kommune.Id)
                    .GetResultList();
                model.AnmeldelseModel.ForureningsKategoriTekster.AddRange(forureningsOpslag.Select(x => x.ShortText).Distinct());
            }

            ModelState.Clear();
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult HistorikKommunikation(Guid anmeldelseId)
        {
            var hkm = new HistorikKommunikationModel();

            var a = _anmeldelserBusiness.Read(anmeldelseId);
            if (a != null)
            {
                hkm.LoebeNr = a.Nummer;
                hkm.internBemaerkning = a.BemarkningInternKommune;
                hkm.aarsagTilAfvisning = a.AarsagAfvisning;

                hkm.Rows = new List<Rows>();

                hkm.Rows.AddRange((from sa in a.StatusAnmeldelse
                    select new Rows
                    {
                        Tid = sa.Tid,
                        Handling = sa.StatusAnmeldelseType.Navn,
                        Person = (sa.Person != null ? sa.Person.Navn + " " + sa.Person.Efternavn : "System"),
                        Log = ReturnLog(sa)
                    }));

                

                hkm.Rows.AddRange((from k in a.Kommunikation
                                orderby k.Besked.Tid descending
                                   select new Rows
                                {
                                    Tid = k.Besked.Tid,
                                    Handling = "Kommunikation",
                                    Person = string.Format("{0} {1} Til: {2} {3}", k.Person.Navn, k.Person.Efternavn, k.Person1.Navn, k.Person1.Efternavn),
                                    Log = Regex.Replace(Regex.Replace(k.Besked.Tekst, @"<style.*?>[\s\S]+?<\/style>", ""), @"<(?!\s*\/?\s*p\b)[^>]*>", "")
                                }));

                hkm.Rows.AddRange((from ad in a.Advis
                                   select new Rows
                                  {
                                      Tid = ad.Besked.Tid,
                                      Handling = "Advis",
                                      Person = (ad.Person != null ? ad.Person.Navn + " " + ad.Person.Efternavn : "-"),
                                      Log = Regex.Replace(Regex.Replace(ad.Besked.Tekst, @"<style.*?>[\s\S]+?<\/style>", ""), @"<(?!\s*\/?\s*p\b)[^>]*>", "")
                                  }));

                hkm.Rows = hkm.Rows.OrderByDescending(r => r.Tid).ToList();
            }

            return View(hkm);
        }

        public ActionResult Vognlaes(Guid anmeldelseId)
        {
            var m = new EksternVognlaesListeModel();
            m.VognlaesListe = new List<EksternVognlaesModel>();
            m.AnmeldelseId = anmeldelseId;


            if (anmeldelseId != Guid.Empty)
            {
                var a = _anmeldelserBusiness.Read(anmeldelseId);

                if (a != null && a.Vognlaes != null && a.Vognlaes.Any())
                {
                    m.VognlaesListe = ConvertVognlaesToModel(a.Vognlaes).OrderByDescending(v => v.DatoBom).ToList();
                }

            }
            return View(m);
        }

        #region private methods

        private string ReturnLog(StatusAnmeldelse sa)
        {
            var result = string.Empty;

            if (sa.StatusAnmeldelseType != null && sa.StatusAnmeldelseType.Kode == (int) EnumStatusAnmeldelse.Gemt)
            {
                var d1 = sa.Tid.AddSeconds(-10);
                var d2 = sa.Tid.AddSeconds(10);

                //Finder Log . Datamodel er ikke optimal, så jeg bliver nød til at anvende tiden...
                var log = (from l in sa.Anmeldelse.Log where l.Dato > d1 && l.Dato < d2 select l).FirstOrDefault();
                if (log != null)
                {
                    var l = _logBusiness.ReadLog(log.Id);
                    foreach (var d in l.Delta)
                    {
                        result += string.Format("<p><b>{0}</b></p><p>Før: {1}</p><p>Efter: {2}</p><br/>", d.Navn, d.Foer, d.Efter);
                    }
                }
            }

            return result;
        }

        private void OpretRevAnmeldelse(OpretModel opretModel)
        {
            try
            {
                var a = new Anmeldelse();
                a.Oprindelsessted = new Oprindelsessted();
                a.Jord = new Jord();

                var b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
                var personSomErLoggetInd = b.Person;

                var anmeldelseFoerAendriger = _anmeldelserBusiness.CloneForLogingPurpose(a);

                //Data i sessions skal tømmes for nøgler til den oprindelige anmeldelse, ellers får man EF fejl.
                //Dokumenter
                if (Session["docList"] != null)
                {
                    var dl = (ICollection<Dokumentation>)Session["docList"];
                    var nyDocList = new Collection<Dokumentation>();
                    foreach (var dokumentation in dl)
                    {
                        var d = new Dokumentation();
                        d = _anmeldelserBusiness.AttributeCopyDokumenation(dokumentation, d);
                        nyDocList.Add(d);
                    }
                    Session["docList"] = nyDocList;
                }
                if (Session["docListRev"] != null)
                {
                    var dl = (ICollection<Dokumentation>)Session["docListRev"];
                    var nyDocList = new Collection<Dokumentation>();
                    foreach (var dokumentation in dl)
                    {
                        var d = new Dokumentation();
                        d = _anmeldelserBusiness.AttributeCopyDokumenation(dokumentation, d);
                        nyDocList.Add(d);
                    }
                    Session["docListRev"] = nyDocList;
                }

                if (Session["Matrikler"] != null)
                {
                    var matrikels = (IList<Matrikel>)Session["Matrikler"];
                    var ms = new List<Matrikel>();
                    foreach (var mo in matrikels)
                    {
                        var m = new Matrikel();
                        m = _anmeldelserBusiness.AttributeCopyMatrikel(mo, m);

                        ms.Add(m);
                    }
                    Session["Matrikler"] = ms;
                }

                if (Session["Interesanter"] != null)
                {
                    var interesants = (HashSet<Interesant>)Session["Interesanter"];
                    var ints = new HashSet<Interesant>();
                    foreach (var io in interesants)
                    {
                        var i = new Interesant();
                        i.Email = io.Email;
                        i.Navn = io.Navn;
                        ints.Add(i);
                    }
                    Session["Interesanter"] = ints;
                }

                if (Session["Jordforureningsopslag"] != null)
                {
                    var jo = (Jordforureningsopslag)Session["Jordforureningsopslag"];
                    var j = new Jordforureningsopslag();
                    j.JordKlassifikationTypeId = jo.JordKlassifikationTypeId;
                    j.JordKlassifikationType = jo.JordKlassifikationType;
                    j.KommunensMiljoeDb = jo.KommunensMiljoeDb;
                    j.OmkAnalysepligt = jo.OmkAnalysepligt;
                    j.OmkLet = jo.OmkLet;
                    j.OmkRen = jo.OmkRen;
                    j.Tid = jo.Tid;
                    j.V1 = jo.V1;
                    j.V2 = jo.V2;

                    Session["Jordforureningsopslag"] = j;
                }
                //else
                //{
                //    var item = _kodelisteBusiness.ReadJordKlassifikationType(opretModel.SelectedJordklassifikationType);
                    

                //} NMR

                a = BrugerfladeToAnmeldelse(a, opretModel, null);

                a.Id = opretModel.Anmeldelse.Id;

                a = _anmeldelserBusiness.CreateRevisionAfAnmeldelse(a, personSomErLoggetInd);

                //FlagSagsbehandler
                //Hvis anmelderen laver ændringer på anmeldelsen før anmeldelsen er godkendt af sagsbehandler, skal der sættes et flag på anmeldelsen.
                //Flaget bliver fjernet hvis anmeldelsen gemmes fra backend brugerfladen.
                if (a != null) a.FlagSagsbehandler = DateTime.Now;

                if (personSomErLoggetInd != null)
                {
                    _anmeldelserBusiness.GemAnmeldelse(a, personSomErLoggetInd, anmeldelseFoerAendriger);
                    opretModel.Anmeldelse = a;

                    ViewBag.Message = "Anmeldelsen er gemt.";
                    //return opretModel;
                }
                ViewBag.Message = "Anmeldelsen blev ikke gemt.";
                //return opretModel;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }

        private OpretModel GemAnmeldelse(OpretModel opretModel, Anmeldelse revisionAfAnmeldelse)
        {
            try
            {
                Anmeldelse a;
                var anmeldelseFoerAendriger = new Anmeldelse();

                if (opretModel.Anmeldelse.Id != Guid.Empty)
                {
                    //slet matrikler database inden de indsættes igen fra session variabel
                    if (Session["Matrikler"] != null)
                        //Dvs at der er lavet et nyt matrikelopslag. Vi sletter alle for ikke at søge hvilke matrikler som er ændret / hvilke er nye / osv.
                        _matrikelBusiness.DeleteMatrikler(_anmeldelserBusiness.Read(opretModel.Anmeldelse.Id));

                    //Anmeldelsen findes allerede  - Anmeldelsen hentes og opdateres med værdier fra brugerfladen
                    a = _anmeldelserBusiness.Read(opretModel.Anmeldelse.Id);

                    //FlagSagsbehandler
                    //Hvis anmelderen laver ændringer på anmeldelsen før anmeldelsen er godkendt af sagsbehandler, skal der sættes et flag på anmeldelsen.
                    //Flaget bliver fjernet hvis anmeldelsen gemmes fra backend brugerfladen.
                    /*if (Request.Form["command"] != "Indsend")
                    {
                        a.FlagSagsbehandler = DateTime.Now;
                    }*/
                    a.FlagSagsbehandler = DateTime.Now;
                }
                else
                {
                    //Anmeldelsen er ikke oprettet endnu.
                    a = opretModel.Anmeldelse;
                }

                if (a != null)
                {
                    anmeldelseFoerAendriger = _anmeldelserBusiness.CloneForLogingPurpose(a);
                    if (revisionAfAnmeldelse == null) {
                        a = BrugerfladeToAnmeldelse(a, opretModel, null);
                    } else {
                        a = BrugerfladeToAnmeldelse(a, opretModel, revisionAfAnmeldelse);
                    }
                }


                var b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
                var personSomErLoggetInd = b.Person;

                if (personSomErLoggetInd != null)
                {
                    //Når anmeldelse afsendes, skal FlagSagsbehandler være NULL! 
                    //MEN hvis anmeldelse er indsendt en gang, så skal vi ikke fjerne flag.
                    /*if (!_statusAnmeldelseBusiness.IsAnmeldelseIndsendtMenIkkeAktiv(opretModel.Anmeldelse.StatusAnmeldelse))
                        a.FlagSagsbehandler = null;*/

                    _anmeldelserBusiness.GemAnmeldelse(a, personSomErLoggetInd, anmeldelseFoerAendriger);

                    if (revisionAfAnmeldelse == null) {
                        opretModel.Anmeldelse = a;
                    } else {
                        opretModel.RevisionAnmeldelse = a;
                    }

                    ViewBag.Message = "Anmeldelsen er gemt.";
                    return opretModel;
                }
                ViewBag.Message = "Anmeldelsen blev ikke gemt.";
                return opretModel;
            }

            catch (Exception ex)
            {
                Logger.LogException(ex);
                throw;
            }
        }
        
        private Anmeldelse BrugerfladeToAnmeldelse(Anmeldelse a, OpretModel opretModel, Anmeldelse revisionAnmeldelse)
        {
            if (revisionAnmeldelse != null)
            {
                a.Jord.ForventetJordmaengdeTon = revisionAnmeldelse.Jord.ForventetJordmaengdeTon;
                a.Jord.KoerselStart = revisionAnmeldelse.Jord.KoerselStart;
                a.Jord.KoerselSlut = revisionAnmeldelse.Jord.KoerselSlut;
                a.Jord.LinkTilGodkendtAnmeldelse = revisionAnmeldelse.Jord.LinkTilGodkendtAnmeldelse;
                a.Jord.TidligereErhvervsaktivitet = revisionAnmeldelse.Jord.TidligereErhvervsaktivitet;
                a.Jord.JordarbejdeBeskrivelse = revisionAnmeldelse.Jord.JordarbejdeBeskrivelse;
                a.Jord.Bemaerkning = revisionAnmeldelse.Jord.Bemaerkning;

                a.BemaerkningTilKommune = revisionAnmeldelse.BemaerkningTilKommune;
                a.BemaerkningTilJordmodtager = revisionAnmeldelse.BemaerkningTilJordmodtager;
                a.AnmelderSagsnummer = revisionAnmeldelse.AnmelderSagsnummer;
                a.Jord.JordKlassifikationTypeId = opretModel.SelectedJordklassifikationType;
            }

            //OPRINDELSESTED
            a.Oprindelsessted.OprindelsesstedKlassifikationType = _kodelisteBusiness.ReadOprindelsesstedKlassifikationType(opretModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType.Id);
            a.Oprindelsessted.Adresse = opretModel.Anmeldelse.Oprindelsessted.Adresse;
            a.Oprindelsessted.Postnummer = opretModel.Anmeldelse.Oprindelsessted.Postnummer;
            a.Oprindelsessted.PostDistrikt = opretModel.Anmeldelse.Oprindelsessted.PostDistrikt;
            a.Oprindelsessted.TidligereErhvervsAktivitet = opretModel.Anmeldelse.Oprindelsessted.TidligereErhvervsAktivitet;
            a.Oprindelsessted.Kortlagt = opretModel.Anmeldelse.Oprindelsessted.Kortlagt; //Bruges ikke i brugerfladen
            a.Oprindelsessted.OffvejUrl = opretModel.Anmeldelse.Oprindelsessted.OffvejUrl;

            //Hvis ejendom og offentlig vej
            if (opretModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType.Id.ToString().ToUpper() != "480738EC-4A9D-448E-9A18-5BA481A8A3C6")
            //Hvis ikke anden oprindelse
            {
                a.Oprindelsessted.AndenOprindJordType = null;
                a.Oprindelsessted.Beskrivelse = "";
                a.Kommune = _kodelisteBusiness.ReadKommuneByNavn(opretModel.Anmeldelse.Kommune.Navn);

                Guid selectedEjendomMaterialeTypeID;
                if (Guid.TryParse(opretModel.SelectedEjendomMaterialeTypeID, out selectedEjendomMaterialeTypeID))
                    a.Oprindelsessted.AndenOprindJordType = _kodelisteBusiness.ReadAndenOprindJordType(selectedEjendomMaterialeTypeID);

                Guid selectedOffentligVejMaterialeTypeID;
                if (Guid.TryParse(opretModel.SelectedOffentligVejMaterialeTypeID, out selectedOffentligVejMaterialeTypeID))
                    a.Oprindelsessted.AndenOprindJordType = _kodelisteBusiness.ReadAndenOprindJordType(selectedOffentligVejMaterialeTypeID);
            }
            else
            {
                //Anden oprindelses
                Guid g;
                if (Guid.TryParse(opretModel.selectedAndenOprindelsesstedKommune, out g))
                    a.Kommune = _kodelisteBusiness.ReadKommuneById(g);

                Guid gg;
                if (Guid.TryParse(opretModel.SelectedAndenOprindJordTypeID, out gg))
                    a.Oprindelsessted.AndenOprindJordType = _kodelisteBusiness.ReadAndenOprindJordType(gg);

                a.Oprindelsessted.Adresse = "Anden oprindelse";
                a.Oprindelsessted.Beskrivelse = opretModel.Anmeldelse.Oprindelsessted.Beskrivelse;

                a.Oprindelsessted.Postnummer = null;
                a.Oprindelsessted.PostDistrikt = null;
                a.Oprindelsessted.OffvejUrl = null;
            }

            if (!string.IsNullOrEmpty(opretModel.OprindelsesstedWkt))
            {
                var g = DbGeometry.FromText(opretModel.OprindelsesstedWkt, Epsg25832);
                a.Oprindelsessted.Geom = g;
            }
            else
            {
                a.Oprindelsessted.Geom = null;
            }

            //MATRIKEL
            //Matriklerne slettes inden anmeldelsen indlæses og beriges med input fra brugerfladen.
            if (Session["Matrikler"] != null)
            {
                var matrikels = (IList<Matrikel>)Session["Matrikler"];

                a.Oprindelsessted.Matrikel.AddRange(matrikels);
                Session["Matrikler"] = null;
            }


            //Jordforureningsopslag
            if (opretModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType.Id.ToString().ToUpper() ==
                "480738EC-4A9D-448E-9A18-5BA481A8A3C6")
            {
                var jk =
                    _kodelisteBusiness.ReadJordKlassifikationType(opretModel
                        .SelectedJordklassifikationTypeVedAndenOprindelse);
                if (jk != null)
                {

                    //Anden oprindelse
                    if (a.Jordforureningsopslag == null)
                        a.Jordforureningsopslag = new Jordforureningsopslag();

                    a.Jordforureningsopslag.Tid = DateTime.Now;
                    a.Jordforureningsopslag.JordKlassifikationType = jk;
                    a.Jordforureningsopslag.KommunensMiljoeDb = null;
                    a.Jordforureningsopslag.OmkAnalysepligt = false;
                    a.Jordforureningsopslag.OmkLet = false;
                    a.Jordforureningsopslag.OmkRen = false;
                    a.Jordforureningsopslag.V1 = false;
                    a.Jordforureningsopslag.V2 = false;
                }
            }
            else
            {
                //Ejendom eller vej

                if (Session["Jordforureningsopslag"] != null)
                {
                    var jordforureningsopslag = (Jordforureningsopslag) Session["Jordforureningsopslag"];
                    if (a.Jordforureningsopslag == null)
                    {
                        a.Jordforureningsopslag = jordforureningsopslag;
                    }
                    else
                    {
                        var jk =
                            _kodelisteBusiness.ReadJordKlassifikationType(jordforureningsopslag
                                .JordKlassifikationTypeId);
                        if (jk != null)
                        {
                            a.Jordforureningsopslag.JordKlassifikationType = jk;
                            a.Jordforureningsopslag.KommunensMiljoeDb = jordforureningsopslag.KommunensMiljoeDb;
                            a.Jordforureningsopslag.OmkAnalysepligt = jordforureningsopslag.OmkAnalysepligt;
                            a.Jordforureningsopslag.OmkLet = jordforureningsopslag.OmkLet;
                            a.Jordforureningsopslag.OmkRen = jordforureningsopslag.OmkRen;
                            a.Jordforureningsopslag.Tid = jordforureningsopslag.Tid;
                            a.Jordforureningsopslag.V1 = jordforureningsopslag.V1;
                            a.Jordforureningsopslag.V2 = jordforureningsopslag.V2;
                        }
                    }
                    Session["Jordforureningsopslag"] = null;
                }
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
                    a.Jord.AffaldType = _kodelisteBusiness.ReadAffaldType(g);

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

            if (a.Jord.JordflytningType.Id == new Guid("1DB6CAC8-8335-49B1-A5AF-65DB18D7F9CB")) //Alm
            {
                a.Jord.AkutBaggrund = null;
                a.Jord.StraksGodkendJordhaandteringsplan = null;
            }
            else if (a.Jord.JordflytningType.Id == new Guid("952A8432-9CE0-493B-AA98-29239502F11C")) //Akut
            {
                a.Jord.AkutBaggrund = opretModel.Anmeldelse.Jord.AkutBaggrund;
                a.Jord.StraksGodkendJordhaandteringsplan = null;
            }
            else if (a.Jord.JordflytningType.Id == new Guid("9C85A889-7AA5-47E4-B076-B70CCCCBD45F")) //Straks jordflytning
            {
                a.Jord.AkutBaggrund = null;
                a.Jord.StraksGodkendJordhaandteringsplan = opretModel.Anmeldelse.Jord.StraksGodkendJordhaandteringsplan;
            }

            if (Session["docList"] != null)
            {
                //Delete dokumentation
                _anmeldelserBusiness.DeleteDokumentation(a);

                var dl = (ICollection<Dokumentation>)Session["docList"];


                var newDocs = new List<Dokumentation>();
                foreach (var dd in dl)
                {
                    if (newDocs.All(x => x.Id != dd.Id))
                    {
                        var d = new Dokumentation();
                        d.Filnavn = dd.Filnavn;
                        d.DokumentationType = _kodelisteBusiness.ReadDokumentationType(dd.DokumentationType.Id);
                        d.DokumentationTypeId = dd.DokumentationTypeId;
                        d.OprindelsesDato = dd.OprindelsesDato;
                        d.JordId = dd.JordId;
                        d.Id = dd.Id;
                        newDocs.Add(d);
                    }
                }

                a.Jord.Dokumentation = newDocs;
            }
            if (Session["docListRev"] != null) // Dette ser bare ud til at nulstille sidste operation?
            {
                //Delete dokumentation
                _anmeldelserBusiness.DeleteDokumentation(a);

                var dl = (ICollection<Dokumentation>)Session["docListRev"];


                var newDocs = new List<Dokumentation>();
                foreach (var dd in dl)
                {
                    if (newDocs.All(x => x.Id != dd.Id))
                    {
                        var d = new Dokumentation();
                        d.Filnavn = dd.Filnavn;
                        d.DokumentationType = _kodelisteBusiness.ReadDokumentationType(dd.DokumentationType.Id);
                        d.DokumentationTypeId = dd.DokumentationTypeId;
                        d.OprindelsesDato = dd.OprindelsesDato;
                        d.JordId = dd.JordId;
                        d.Id = dd.Id;
                        newDocs.Add(d);
                    }
                }

                a.Jord.Dokumentation = newDocs;
            }
            a.Jord.EgetOrdrenummer = opretModel.Anmeldelse.Jord.EgetOrdrenummer;

            //TRANSPORTØR
            Guid gTrans;
            if (Guid.TryParse(opretModel.SelectedTransportoerId, out gTrans))
            {
                a.Transportoer = _transportoerBusiness.ReadTransportoer(gTrans);
            }
            else
            {
                a.Transportoer = null;
            }

            //MODTAGERANLAEG
            Guid gMod;
            if (Guid.TryParse(opretModel.SelectedModtagerAnlaegId, out gMod))
            {
                a.ModtagerAnlaeg = _modtagerAnlaegBusiness.ReadModtagerAnlaeg(gMod);
            }
            else
            {
                a.ModtagerAnlaeg = null;
            }

            //BETALER
            switch (opretModel.SelectedBetalerIndex)
            {
                case 0:
                    {
                        var betaler = _betalerBusiness.ReadBetaler(opretModel.Anmeldelse.Anmelder.Id);
                        a.Betaler = betaler;
                    }
                    break;
                case 1:
                    {
                        Guid ggTrans;
                        if (Guid.TryParse(opretModel.SelectedTransportoerId, out ggTrans))
                        {
                            var betaler = _betalerBusiness.ReadBetaler(ggTrans);
                            a.Betaler = betaler;
                        }
                    }
                    break;
                default:
                    if (opretModel.SelectedBetalerIndex == 2 && opretModel.BetalerId != Guid.Empty)
                    {
                        var betaler = _betalerBusiness.ReadBetaler(opretModel.BetalerId);
                        a.Betaler = betaler;
                    }
                    break;
            }

            //KVE: Jeg vil gerne lave nedenstående, så der ikke blev registreret en betaler på anmeldelsen, hvis modtageranlæget ikke anvendte FlytJord. Men så får jeg en dum "Id is required" validerings på modellen.
            if (a != null && a.ModtagerAnlaeg != null && a.ModtagerAnlaeg.AnvenderJF == false && a.Betaler != null)
            {
                if (a.Betaler.Anmeldelse.Contains(a))
                    a.Betaler.Anmeldelse.Remove(a);
                a.Betaler = null;
            }
            //KOMMUNIKATION
            if (Session["Interesanter"] != null)
            {
                //if (!gennemsyn)
                _anmeldelserBusiness.DeleteInteressanter(a);
                var interesants = (HashSet<Interesant>)Session["Interesanter"];

                var newInteressanter = (from i in interesants select new Interesant() { Email = i.Email, Navn = i.Navn });
                a.Interesant.AddRange(newInteressanter);

            }

            a.BemaerkningTilKommune = opretModel.Anmeldelse.BemaerkningTilKommune;
            a.BemaerkningTilJordmodtager = opretModel.Anmeldelse.BemaerkningTilJordmodtager;
            a.KoertJordAlarm = opretModel.Anmeldelse.KoertJordAlarm;
            a.AnmelderSagsnummer = opretModel.Anmeldelse.AnmelderSagsnummer;
            //ANMELDER
            if (a.Anmelder != null)
            { //Hvis der er en anmelder på anmeldelsen
                var anmelder = _anmelderBusiness.ReadAnmelder(opretModel.Anmeldelse.Anmelder.Id);
                if (anmelder == null)
                {
                    //Ved oprettelse af første anmeldelse
                    var p = _personBusiness.Read(opretModel.Anmeldelse.Anmelder.Id);

                    if (p != null && p.Anmelder == null)
                    {
                        a.Anmelder = new Anmelder();
                        a.Anmelder.Person = p;
                        a.Anmelder.Id = p.Id;
                    }
                }
                else
                {
                    a.Anmelder = anmelder;
                }

            }
            else
            {
                var p = _personBusiness.Read(opretModel.Anmeldelse.Anmelder.Id);
                if (p != null && p.Anmelder != null)
                {
                    a.Anmelder = p.Anmelder;
                }
                else
                {
                    a.Anmelder = new Anmelder();
                    a.Anmelder.Person = p;
                    a.Anmelder.Id = p.Id;
                }
            }
            return a;
        }

        private OpretModel InitOpretModel(OpretModel opretModel)
        {
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;

            opretModel.KortApiUrl = appSettings["KortApiUrl"];
            opretModel.KortPageModtagere = appSettings["KortPageModtagere"];
            opretModel.KortPageSted = appSettings["KortPageSted"];
            opretModel.KortSite = appSettings["KortSite"];
            //Sted
            opretModel.StedKommuneListe = GetAktiveKommuner(opretModel.selectedAndenOprindelsesstedKommune);
            opretModel.AndenOprindJordTypeListe = GetAndenOprindJordtyper(opretModel.SelectedAndenOprindJordTypeID);

            // Sæt den valgte "ukendt transportør" bruger
            var ukendtTransportoer = _transportoerBusiness.ReadTransportoer(Guid.Parse(ConfigurationManager.AppSettings["UkendtTransportoerId"]));
            opretModel.UkendtTransportoer = ConvertTransportoerToModel(ukendtTransportoer);

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
            }
            else
            {
                opretModel.MatrikelWkt = "";
            }

            //JORD
            opretModel.DokumentationTypeListe = GetAktiveDokumentationTypeLister();
            //var kommune = (from k in _kodelisteBusiness.ReadAktiveKommune() where k.Navn == "Aarhus" select k).FirstOrDefault();
            if (opretModel.Anmeldelse != null && opretModel.Anmeldelse.Kommune != null)
            {
                if (opretModel.Anmeldelse != null && opretModel.Anmeldelse.Oprindelsessted != null &&
                    opretModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType != null &&
                    opretModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType.Id.ToString().ToUpper() == "480738EC-4A9D-448E-9A18-5BA481A8A3C6"//Anden oprindelse
                    )
                {
                    //Hvis oprindelsesstedet er AndenOprindelse så skal jordklassifikationerne vises fra start af.
                    opretModel.JordklassifikationTypeListe = GetJordKlassifikationType(opretModel.Anmeldelse.Kommune.Id);
                }

                //Hvis der lige er blevet foretaget et jordforureningsopslag. 
                //Info: Brugeren skal tjekke de eksterne systmer, når en anmeldelse genindlæses. Det kan være der kommet nye oplysninger vedr. jorden.
                if (opretModel.SelectedJordklassifikationType != null && opretModel.SelectedJordklassifikationType != Guid.Empty)
                    opretModel.JordklassifikationTypeListe = GetJordKlassifikationType(opretModel.Anmeldelse.Kommune.Id);

                var renjordKlassifikationTypeId =
                  (from f in _kodelisteBusiness.ReadAktiveJordKlassifikationTypesForLandsdel(opretModel.Anmeldelse.Kommune.Id) select f).OrderByDescending(x => x.Priotering).FirstOrDefault();
                if (renjordKlassifikationTypeId != null)
                    opretModel.JordRenJordKlassifikationTypeId = renjordKlassifikationTypeId.Id.ToString();
                /*
                if(opretModel.Revision)
                    opretModel.JordklassifikationTypeListe = GetJordKlassifikationType(opretModel.Anmeldelse.Kommune.Id);
                 */
            }

            // KTW: Tjek at der ikke allerede er sat kryds i "indeholder affald", og respekter dette hvis der er.
            if (!opretModel.IndeholderAffald && opretModel.Anmeldelse != null && opretModel.Anmeldelse.Jord != null)
                opretModel.IndeholderAffald = (!string.IsNullOrEmpty(opretModel.Anmeldelse.Jord.AndenAffaldType) | opretModel.Anmeldelse.Jord.AffaldType != null);

            opretModel.AffaldTypeListe = GetAktiveAffaldTyper(opretModel.selectedAffaldType);
            opretModel.JordflytningTypeListe = _kodelisteBusiness.ReadAktiveJordflytningTypes();

            if (opretModel.Anmeldelse != null &&
              opretModel.Anmeldelse.Jord != null &&
              opretModel.Anmeldelse.Jord.JordKlassifikationType != null)
            {
                //Anvendes til at holde valgte jordklassifikation. Da jordklassifiaktionen kan sættes fra radiobuttons og jordforureningsopslaget.
                opretModel.SelectedJordklassifikationType = opretModel.Anmeldelse.Jord.JordKlassifikationType.Id;
                opretModel.SelectedJordklassifikationTypeVedAndenOprindelse = opretModel.Anmeldelse.Jord.JordKlassifikationType.Id;
            }

            /*REVISION*/
            if (opretModel.RevisionAnmeldelse != null && opretModel.Revision)
            {
                if (opretModel.RevisionAnmeldelse.Jordforureningsopslag != null)
                {
                    if (opretModel.RevisionAnmeldelse.Jordforureningsopslag.JordKlassifikationTypeId != Guid.Empty)
                    {
                        opretModel.ForureningOpslagJordklassifikationTypeId =
                            opretModel.RevisionAnmeldelse.Jordforureningsopslag.JordKlassifikationTypeId;
                    }
                    else
                    {
                        opretModel.ForureningOpslagJordklassifikationTypeId = Guid.Empty;
                    }
                }

                if (opretModel.RevisionAnmeldelse.Jord != null &&
                    opretModel.RevisionAnmeldelse.Jord.JordKlassifikationType != null)
                {

                    opretModel.SelectedJordklassifikationType =
                        opretModel.RevisionAnmeldelse.Jord.JordKlassifikationType.Id;
                    opretModel.SelectedRevisionJordklassifikationTypeNavn =
                        opretModel.RevisionAnmeldelse.Jord.JordKlassifikationType.Navn;

                    opretModel.JordklassifikationTypeListe = null;
                    //opretModel.SelectedJordklassifikationTypeVedAndenOprindelse = opretModel.RevisionAnmeldelse.Jord.JordKlassifikationType.Id;
                }
                else //NMR
                {
                    // opretModel.SelectedJordklassifikationType =  Guid.Empty;

                    if (opretModel.SelectedJordklassifikationType != Guid.Empty)
                    {

                        var test =
                            _kodelisteBusiness.ReadJordKlassifikationType(opretModel.SelectedJordklassifikationType);

                        opretModel.SelectedRevisionJordklassifikationTypeNavn = test.Navn;
                    }
                }//NMR
            }
            //Andenoprindelse
            //////////////////////////////////

            //if (kommune != null)
            //{
            //  opretModel.JordklassifikationTypeListe = GetJordKlassifikationType(kommune.Id);

            //}

            //if (opretModel.Anmeldelse != null && opretModel.Anmeldelse.Jord != null &&
            //    opretModel.Anmeldelse.Jord.JordKlassifikationType != null)
            //{
            //  opretModel.ForureningOpslagJordklassifikationTypeId = opretModel.Anmeldelse.Jord.JordKlassifikationType.Id;
            //  opretModel.SelectedJordklassifikationType = opretModel.Anmeldelse.Jord.JordKlassifikationType.Id;
            //}

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

            if (opretModel.BetalerSelectedIndex == 0 && opretModel.Anmeldelse != null && opretModel.Anmeldelse.Anmelder != null)
            {
                var betaler = _betalerBusiness.ReadBetaler(opretModel.Anmeldelse.Anmelder.Id);
                opretModel.BetalerId = opretModel.Anmeldelse.Anmelder.Id;

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
                    opretModel.BetalerId = ggTrans;
                    var pp = new List<Person> { betaler.Person };

                    var pm = ConvertPersonToModel(pp);
                    opretModel.SelectedBetaler = pm;
                }
            }
            else if (opretModel.BetalerSelectedIndex == 2 && opretModel.Anmeldelse != null &&
                     opretModel.Anmeldelse.Betaler != null && opretModel.Anmeldelse.Betaler.Id != Guid.Empty)
            {
                var betaler = _betalerBusiness.ReadBetaler(opretModel.Anmeldelse.Betaler.Id);
                opretModel.BetalerId = opretModel.Anmeldelse.Betaler.Id;
                var pp = new List<Person> { betaler.Person };
                var pm = ConvertPersonToModel(pp);
                opretModel.SelectedBetaler = pm;
            }
            else
            {
                if (opretModel.Anmeldelse != null && opretModel.Anmeldelse.Anmelder != null)
                {
                    var p = _personBusiness.Read(opretModel.Anmeldelse.Anmelder.Id);
                    opretModel.BetalerId = opretModel.Anmeldelse.Anmelder.Id;
                    var pp = new List<Person> { p };
                    var pm = ConvertPersonToModel(pp);
                    opretModel.SelectedBetaler = pm;
                }
                opretModel.SelectedBetalerIndex = 0;
            }

            return opretModel;
        }

        private void ValiderStedBasic(OpretModel opretModel)
        {
            //Hvis ejendom og offentlig vej
            if (opretModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType != null &&
              opretModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType.Id.ToString().ToUpper() == "480738EC-4A9D-448E-9A18-5BA481A8A3C6")
                return;

            if (string.IsNullOrEmpty(opretModel.Anmeldelse.Oprindelsessted.Adresse))
                ModelState.AddModelError("Adresse", @"Sted - Der skal angives en adresse.");

            //Kontrollere at input geometrien er valid
            //if (opretModel.OprindelsesstedKlassifikationTypeSelectedIndex != 1) { 
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
            //}
        }

        private void ValiderStedVedAfsend(OpretModel opretModel)
        {
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
            if (opretModel.ForureningOpslagJordklassifikationTypeId == Guid.Empty && opretModel.OprindelsesstedKlassifikationTypeSelectedIndex != 1)
                ModelState.AddModelError("JordforureningOpslag", @"Sted - Anmeldelsen kan ikke afsendes før jorden er kontrolleret i de eksterne systemer.");
            //JF-429 : Bedre besked her.
        }

        private void ValiderJord(OpretModel opretModel)
        {

            //Nye regler vedr revision
            //Hvis revision - Tjek for tilfældet at de i den oprindelige anmeldelse har op eller nedklassificeret jorden
            //Den valgte jordklassifikation findes ved at se på det modtageranlæg som er valgt i den oprindelige anmeldelse - på den måde undgår jeg at slå op i databasen.
            //Guid gMod;
            //if (opretModel.Anmeldelse.RevisionAfAnmeldelse.HasValue && Guid.TryParse(opretModel.SelectedModtagerAnlaegId, out gMod))
            //{
            //  var m = _modtagerAnlaegBusiness.Read(gMod);
            //  if (m != null &&
            //    m.JordKlassifikationType != null &&
            //    //opretModel.Anmeldelse.Jord.JordKlassifikationType != null &&
            //      m.JordKlassifikationType.Id != opretModel.SelectedJordklassifikationType)
            //  {
            //    ModelState.AddModelError("JordKlassifikationRevision", @"Jorden - Jordforureningsopslaget giver en anden klassifikation end angivet i den oprindelige anmeldelse. Du kan ikke anvende revision af en anmeldelse, såfremt jorden skal op- eller nedklassificeres, her skal du oprette en ny anmeldelse.");
            //  }
            //}


            //if (opretModel.Anmeldelse.Jord.JordKlassifikationType == null)
            if (opretModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType.Id.ToString().ToUpper() == "480738EC-4A9D-448E-9A18-5BA481A8A3C6")
            {
                //Anden oprindelse
                if (string.IsNullOrEmpty(opretModel.SelectedJordklassifikationTypeVedAndenOprindelse.ToString()) || opretModel.SelectedJordklassifikationTypeVedAndenOprindelse.ToString() == Guid.Empty.ToString())
                    ModelState.AddModelError("JordKlassifikation", @"Jorden - Der skal angives en forureningskategori");
            }
            else
            {
                //Ejendom eller vej
                if (string.IsNullOrEmpty(opretModel.SelectedJordklassifikationType.ToString()) || opretModel.SelectedJordklassifikationType.ToString() == Guid.Empty.ToString())
                    ModelState.AddModelError("JordKlassifikation", @"Jorden - Der skal angives en forureningskategori");
            }


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

            if (opretModel.Anmeldelse.Jord.ForventetJordmaengdeTon.HasValue && opretModel.Anmeldelse.Jord.ForventetJordmaengdeTon.Value <= 0)
                ModelState.AddModelError("ForventetJordmaengdeTon", @"Jorden - Forventet jordmængde skal være større end 0");

            if (opretModel.IndeholderAffald && opretModel.selectedAffaldType == "ef9f54db-0a6d-4add-ab7b-d3e08b6764d8")
                ModelState.AddModelError("JordAndenAffaldType", @"Jorden - Hvis Andet affaldstype er valgt skal Anden affaldstype udfyldes");

            if (opretModel.IndeholderAffald && opretModel.selectedAffaldType == Guid.Empty.ToString())
                ModelState.AddModelError("JordAndenAffaldType", @"Jorden - Hvis jorden indeholder affald skal type angives");
            
            if (string.IsNullOrWhiteSpace(opretModel.Anmeldelse.Jord.JordarbejdeBeskrivelse))
                ModelState.AddModelError("JordarbejdeBeskrivelse", @"Jorden - Beskrivelse af jordabejde skal udfyldes");

            if (opretModel.Anmeldelse.Jord.IntaktJord == false)
            {
                if (opretModel.Anmeldelse.Oprindelsessted != null &&
                    opretModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType != null &&
                    opretModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType.Id != new Guid("480738EC-4A9D-448E-9A18-5BA481A8A3C6"))
                //Hardcoded id for anden oprindelse :0(
                {

                    if (opretModel.Anmeldelse.Jord.JordKlassifikationType != null)
                    {
                        //Hvis brugeren ændre Jordforureningskategori fra det som er fundet i miljøportalen skal brugeren have vedhæftet filer.
                        //if (opretModel.ForureningOpslagJordklassifikationTypeId != opretModel.Anmeldelse.Jord.JordKlassifikationType.Id)

                        //jordforureningsopslaget gemmes i session, men sesion "null stilles" når brugeren gemmer.
                        //Har brugeren gemt før indsend, hentes jorddforureningsopslaget fra databasen.
                        Jordforureningsopslag jordforureningsopslag = null;
                        if (Session["Jordforureningsopslag"] == null)
                        {
                            if (opretModel.Anmeldelse != null && opretModel.Anmeldelse.Id != Guid.Empty)
                            {
                                var a = _anmeldelserBusiness.Read(opretModel.Anmeldelse.Id);
                                if (a != null && a.Jordforureningsopslag != null)
                                    jordforureningsopslag = a.Jordforureningsopslag;
                            }
                        }
                        else
                        {
                            jordforureningsopslag = (Jordforureningsopslag)Session["Jordforureningsopslag"];
                        }

                        if (jordforureningsopslag != null &&
                          opretModel.Anmeldelse.Jord != null &&
                           opretModel.Anmeldelse.Kommune != null &&
                          _anmeldelserBusiness.DokumentationPaakraevet(
                          jordforureningsopslag,
                          opretModel.ForureningOpslagJordklassifikationTypeId,
                          opretModel.Anmeldelse.Jord.JordKlassifikationType.Id,
                          opretModel.Anmeldelse.Jord.ForventetJordmaengdeTon,
                          opretModel.Anmeldelse.Kommune.Kommunenr))
                        {
                            var docVal = false;
                            if (Session["docList"] != null)
                            {
                                var dl = (ICollection<Dokumentation>)Session["docList"];
                                docVal = dl.Any();
                            }

                            if (opretModel.Anmeldelse.Jord.Dokumentation != null && opretModel.Anmeldelse.Jord.Dokumentation.Count > 0)
                                docVal = true;

                            if (!docVal)
                                ModelState.AddModelError("JordenDokumentationKraevet", @"Jorden - Der skal vedhæftes dokumentation for den valgte forureningskategori.");
                        }
                    }
                }
            }

            //Jorden - Akut jordflytning
            opretModel.JordflytningTypeListe = _kodelisteBusiness.ReadAktiveJordflytningTypes();

            if (opretModel.Anmeldelse.Jord.JordflytningType.Id == opretModel.JordflytningTypeListe[1].Id && string.IsNullOrEmpty(opretModel.Anmeldelse.Jord.AkutBaggrund))
                ModelState.AddModelError("JordenAkutBaggrund", @"Jorden - Baggrund for Akut jordflytning skal angives");

            //Jord fra kommuner som ikke anvender FlytJord
            //Her skal der være vedhæftet eller linket til en godkendt anmeldelse
            if (!opretModel.Anmeldelse.Kommune.Aktiv)
            {
                var godkendtAnmeldelse = false;

                if (!string.IsNullOrEmpty(opretModel.Anmeldelse.Jord.LinkTilGodkendtAnmeldelse))
                    godkendtAnmeldelse = true;

                if (Session["docList"] != null)
                {
                    var dl = (ICollection<Dokumentation>)Session["docList"];
                    var res = dl.Where(d => d.DokumentationType.Kode == (short)EnumDokumentationType.AnmeldelseAndenKommune).FirstOrDefault();
                    if (res != null)
                        godkendtAnmeldelse = true;
                }

                //Hvis det er Ejendom og hvis jorden ikke er anmelderpligtig, så skal der ikke være krav om at der skal vedhæftes en godkendt anmeldelse.
                if (opretModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType.Id == new Guid("BBB062F1-641A-45AD-BEDD-B0C126FCDFE6") && !opretModel.ForureningOpslagKraeverAnmeldelse)
                {
                    godkendtAnmeldelse = true;
                }

                if (!godkendtAnmeldelse)
                {
                    ModelState.AddModelError("JordenGodkendtAnmeldelse",
                                             @"Jorden - Der skal vedhæftes eller laves et link til en godkendt anmeldelse, når jorden kommer fra en kommune, som ikke anvender FlytJord og jorden er anmelderpligtig.");
                }
            }

        }

        private void ValiderJordRevision(OpretModel opretModel)
        {
            if (!opretModel.RevisionAnmeldelse.Jord.KoerselStart.HasValue)
                ModelState.AddModelError("KoerselStart", @"Jorden - Kørsel start skal udfyldes");

            if (!opretModel.RevisionAnmeldelse.Jord.KoerselSlut.HasValue)
                ModelState.AddModelError("KoerselSlut", @"Jorden - Kørsel slut skal udfyldes");

            if (!opretModel.RevisionAnmeldelse.Jord.ForventetJordmaengdeTon.HasValue)
                ModelState.AddModelError("ForventetJordmaengdeTon", @"Jorden - Forventet jordmængde skal udfyldes");

            if (opretModel.RevisionAnmeldelse.Jord.ForventetJordmaengdeTon.HasValue && opretModel.RevisionAnmeldelse.Jord.ForventetJordmaengdeTon.Value <= 0)
                ModelState.AddModelError("ForventetJordmaengdeTon", @"Jorden - Forventet jordmængde skal være større end 0");
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
                if (opretModel.Anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType.Id.ToString().ToUpper() == "480738EC-4A9D-448E-9A18-5BA481A8A3C6")
                {
                    //Anden oprindelse
                    if (m != null &&
                      m.JordKlassifikationType != null &&
                        //opretModel.Anmeldelse.Jord.JordKlassifikationType != null &&
                        m.JordKlassifikationType.Id != opretModel.SelectedJordklassifikationTypeVedAndenOprindelse)
                    {
                        ModelState.AddModelError("ModtagerAnlaeg", @"Modtager og transportør - Modtageanlægget modtager ikke jord af den valgte forureningskategori");
                    }

                }
                else
                {
                    //Ejendom eller vej
                    if (m != null &&
                      m.JordKlassifikationType != null &&
                        //opretModel.Anmeldelse.Jord.JordKlassifikationType != null &&
                        m.JordKlassifikationType.Id != opretModel.SelectedJordklassifikationType)
                    {
                        ModelState.AddModelError("ModtagerAnlaeg", @"Modtager og transportør - Modtageanlægget modtager ikke jord af den valgte forureningskategori");
                    }

                }


                //kontrol at jordmodtager må modtage jord med affald.
                if (
                  m != null && m.Affald == false &&
                  opretModel.IndeholderAffald
                  )
                {
                    ModelState.AddModelError("ModtagerAnlaegAffald", @"Modtager og transportør - Modtageanlægget modtager ikke jord med affald");
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

            //JF-433: Tjek på om modtageranlæg er blevet inaktiv eller transportør er gået på pension.

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

        private void ValiderBetaler(OpretModel opretModel)
        {
        }

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
                const string httpTxt = "http://";
                if (!www.StartsWith(httpTxt))
                    www = httpTxt + www;

                res = "<a href='" + www + "' target='blank'>www</a>";
            }
            return res;
        }

        [HttpPost]
        private IEnumerable<SelectListItem> GetAktiveKommuner(string selectedValue)
        {
            var res = from k in _kodelisteBusiness.ReadAktiveKommune()
                      select new SelectListItem
                      {
                          Selected = (k.Id.ToString() == selectedValue),
                          Text = k.Navn,
                          Value = k.Id.ToString()
                      };
            var selectListItems = res as IList<SelectListItem> ?? res.ToList();
            return selectListItems;
        }

        [HttpPost]
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

        [HttpPost]
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

        [HttpPost]
        private IEnumerable<SelectListItem> GetAktiveAffaldTyper(string selectedValue)
        {
            var res = new List<SelectListItem>((
                                                 from a in _kodelisteBusiness.ReadAktiveAffaldTypes()
                                                 select new SelectListItem
                                                 {
                                                     Selected = (a.Id.ToString() == selectedValue),
                                                     Text = a.Navn,
                                                     Value = a.Id.ToString()
                                                 }));

            res.Add(new SelectListItem { Text = @" ", Value = Guid.Empty.ToString() });
            return res.OrderBy(f => f.Value);
        }

        [HttpPost]
        public static IEnumerable<ModtagerAnlaegModel> ConvertModtagerAnlaegToModel(IList<ModtagerAnlaeg> modtagerAnlaegs, DbGeometry oprindelsessted)
        {
            var res = new List<ModtagerAnlaegModel>();
            foreach (var modtagerAnlaeg in modtagerAnlaegs)
            {
                // Tjek om modtageranlæg er aktiv via fra/til dato! JF-477
                var modtagerAnlaegAktivDage = Int32.Parse(ConfigurationManager.AppSettings["ModtagerAnlaegAktivDage"]);
                if (modtagerAnlaeg.AktivFra.HasValue && modtagerAnlaeg.AktivTil.HasValue
                    && modtagerAnlaeg.AktivFra.Value > DateTime.Now && modtagerAnlaeg.AktivTil.Value <= DateTime.Now.AddDays(modtagerAnlaegAktivDage))
                {
                    continue;
                }

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
                    Adresse = modtagerAnlaeg.Adresse,
                    Ejerlav = modtagerAnlaeg.Ejerlav,
                    Matrikelnr = modtagerAnlaeg.Matrikelnr,
                    JF = modtagerAnlaeg.AnvenderJF,
                    CentraltOprettetMidlertidigtAnlaeg = modtagerAnlaeg.CentraltOprettetMidlertidigtAnlaeg,
                    AnmelderOprettetMidlertidigtAnlaeg = modtagerAnlaeg.AnmelderOprettetMidlertidigtAnlaeg,
                    //MidlertidigtAnlaeg = modtagerAnlaeg.MidlertidigtAnlaeg,
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

        private static string LinkHelperModtagerAnlaegKort(Guid modtagerAnlaegId, string modtageranlaegnavn)
        {
            var res = "";
            if (modtagerAnlaegId != Guid.Empty)
            {
                res = "<div style=\"cursor: pointer;\" onclick=\"openModtagerAnlaegKort('" + modtagerAnlaegId + "','" + modtageranlaegnavn + "')\">Kort</div>";
            }
            return res;

        }

        private static TransportoerModel ConvertTransportoerToModel(Transportoer transportoer)
        {
            var transModel = new TransportoerModel();
            transModel.Email = transportoer.Person.Email;
            transModel.Telefon = transportoer.Person.Telefon.ToString();  //Telefon på firma anvendes ikke
            transModel.Navn = transportoer.Person.Navn;
            transModel.Kontakt = transportoer.Person.Firmaoplysninger != null ? transportoer.Person.Navn + " " + transportoer.Person.Efternavn : "";
            transModel.Adresse = transportoer.Person.Firmaoplysninger != null ? transportoer.Person.Firmaoplysninger.Adresse : "(Fejl i data: Intet firma!)";
            transModel.Firma = transportoer.Person.Firmaoplysninger != null ? transportoer.Person.Firmaoplysninger.Firmanavn : "(Fejl i data: Intet firma!)";
            transModel.Mobiltelefon = transportoer.Person.Mobiltelefon.HasValue ? transportoer.Person.Mobiltelefon.Value.ToString() : "";
            transModel.Id = transportoer.Id;
            //transModel.Telefon = t.Person.Firmaoplysninger != null ? t.Person.Firmaoplysninger.Telefon.ToString() : "(Fejl i data: Intet firma!)";


            transModel.LastbilMiljoeKlasserNavn = String.Join(", ", transportoer.Lastbil.ToList().Select(x => x.MiljoeklasseType.Navn).Distinct().OrderBy(n => n).ToList());
            if (String.IsNullOrEmpty(transModel.LastbilMiljoeKlasserNavn))
                transModel.LastbilMiljoeKlasserNavn = "Ingen lastbiler er tilknyttet!";
            transModel.PostDistrikt = transportoer.Person.Firmaoplysninger != null ? transportoer.Person.Firmaoplysninger.Postdistikt : "(Fejl i data: Intet firma!)";
            transModel.PostNr = transportoer.Person.Firmaoplysninger != null ? transportoer.Person.Firmaoplysninger.Postnummer.ToString() : "(Fejl i data: Intet firma!)";
            
            return transModel;
        }

        [HttpPost]
        private static List<TransportoerModel> ConvertTransportoerToModel(IList<Transportoer> transportoers)
        {
            return transportoers.Select(ConvertTransportoerToModel).ToList();
        }

        private static IEnumerable<InteresantModel> ConvertInteresantToModel(IEnumerable<Interesant> interesants)
        {
            var res = from i in interesants
                      select new InteresantModel
                      {
                          Id = i.Id,
                          Navn = i.Navn,
                          Email = i.Email
                      };
            return res;
        }

        [HttpPost]
        private static IEnumerable<BetalerModel> ConvertPersonToModel(IList<Person> persons)
        {
            var res = from p in persons
                      select new BetalerModel
                      {
                          Id = p.Id,
                          Adresse = p.Adresse,
                          CVR = p.Firmaoplysninger != null ? Convert.ToInt32(p.Firmaoplysninger.CVR).ToString(CultureInfo.InvariantCulture) : "",
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
                           FilNavnUrlEncoded = HttpUtility.UrlEncode(d.Filnavn),
                           FilUrl = "Skal ikke bruges",
                           FilNavn = d.Filnavn,
                           Type = d.DokumentationType.Navn,
                           StikproeveId = Guid.Empty,
                           AnmeldelseId = d.Jord.Anmeldelse.Id,
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
                           Type = "Analysedokument",
                           StikproeveId = d.Stikproeve.Id,
                           AnmeldelseId = Guid.Empty,
                           DatoData = d.Dato,
                       }).ToList();

            return res;
        }

        private static IEnumerable<EksternVognlaesModel> ConvertVognlaesToModel(ICollection<Vognlaes> vognlaeses)
        {
            var res = (from v in vognlaeses
                       select new EksternVognlaesModel
                       {
                           Id = v.Id,
                           DatoBom = v.Dato,
                           Afvist = v.Afvist,
                           AfvistNote = v.AfvistNote,
                           JordmaengdeAksler = v.MaengdeAksler.HasValue ? Convert.ToDecimal(v.MaengdeAksler.Value) : 0,
                           JordmaengdeTon = v.MaengdeTon.HasValue ? Convert.ToDecimal(v.MaengdeTon.Value) : 0,
                           Transportoer = v.Lastbil.Transportoer.Person.Firmaoplysninger != null
                              ? v.Lastbil.Transportoer.Person.Firmaoplysninger.Firmanavn + Environment.NewLine + "(" + v.Lastbil.Nummerplade + ")"
                              : "(Fejl i data: Intet firma!)"
                       }
                      );
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

        #endregion Private methods


        public JsonResult HentMidlertidigtAnlaeg(Guid anlaegId, Guid? anmeldelseId)
        {
            var modtagerAnlaeg = _modtagerAnlaegBusiness.ReadModtagerAnlaeg(anlaegId);
            int? afstand = null;
            if (anmeldelseId.HasValue && anmeldelseId != Guid.Empty)
            {
                var anmeldelse = _anmeldelserBusiness.GetAnmeldelse(anmeldelseId.Value);
                afstand = DistanceBeregner(anmeldelse.Oprindelsessted.Geom, modtagerAnlaeg.Geom);
            }
            var anl = new ModtagerAnlaegModel
            {
                Affald = modtagerAnlaeg.Affald,
                Afstand = afstand,
                Forureningskategori = (modtagerAnlaeg.JordKlassifikationType != null ? modtagerAnlaeg.JordKlassifikationType.Navn : ""),
                Id = modtagerAnlaeg.Id,
                Adresse = modtagerAnlaeg.Adresse,
                By = modtagerAnlaeg.PostDistrikt,
                Postnummer = modtagerAnlaeg.Postnummer.ToString(),
                Ejerlav = modtagerAnlaeg.Ejerlav,
                Ejerlavsnavn = modtagerAnlaeg.Ejerlavsnavn,
                Matrikelnr = modtagerAnlaeg.Matrikelnr,
                EsrEjendomsnummer = modtagerAnlaeg.EsrEjendomsnr,
                Bemaerkning = modtagerAnlaeg.Bemaerkning,
                Kommune = (modtagerAnlaeg.KommuneKode.HasValue ? _kodelisteBusiness.ReadKommuneByKode(modtagerAnlaeg.KommuneKode.Value).Navn : string.Empty),
                JF = modtagerAnlaeg.AnvenderJF,
                /* Det vides ikke om de to label properties vises et sted, men de får lige et klistermærke alligevel */
                Label = "Midlertidig modtager",
                LabelJordModtager = "Midlertidig modtager",
                JordmodtagerNavn = (modtagerAnlaeg.Jordmodtager != null ? modtagerAnlaeg.Jordmodtager.Navn : ""),
                Navn = modtagerAnlaeg.Navn,
                www = (LinkHelper(modtagerAnlaeg.www)),
                JordmodtagerId = modtagerAnlaeg.JordmodtagerId,
                KortLink = LinkHelperModtagerAnlaegKort(modtagerAnlaeg.Id, modtagerAnlaeg.Navn),
                DocLink = " ",
                KontaktpersonNavn = (!String.IsNullOrEmpty(modtagerAnlaeg.KontaktpersonNavn) ? modtagerAnlaeg.KontaktpersonNavn : " - "),
                KontaktpersonTlf = modtagerAnlaeg.KontaktpersonTlf,
                KontaktpersonEmail = (!String.IsNullOrEmpty(modtagerAnlaeg.KontaktpersonEmail) ? modtagerAnlaeg.KontaktpersonEmail : " - ")
            };
            return Json(anl, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ReadHistorikForAnmeldelse([DataSourceRequest] DataSourceRequest request, Guid anmeldelseId)
        {
            // Ignorerede StatusAnmeldelsesType koder:
            var ignoredStatusAnmeldelseTypeKoder = new [] {
                2, // Gemt
            };

            //Read statusanmeldelse
            var a = _anmeldelserBusiness.Read(anmeldelseId);
            if (a != null)
            {
                var historikListe = (from sa in a.StatusAnmeldelse where !ignoredStatusAnmeldelseTypeKoder.Contains(sa.StatusAnmeldelseType.Kode)
                                     select new HistorikAnmeldelseModel
                                     {
                                         Id = sa.Id,
                                         Tid = sa.Tid,
                                         Handling = sa.StatusAnmeldelseType.Navn,
                                         Person = (sa.Person != null ? "Af: " + sa.Person.Navn + " " + sa.Person.Efternavn : "Af: System"),
                                         Link = LinkHelperStatusAnmeldelse(sa)
                                     }).ToList();

                var komListe = (from k in a.Kommunikation
                                orderby k.Besked.Tid descending
                                select new HistorikAnmeldelseModel
                                {
                                    Id = k.Id,
                                    Tid = k.Besked.Tid,
                                    Handling = k.Besked.Advis.FirstOrDefault().AdvisType.Navn == "Hør anden kommune" ? "Kommunikation - Hør anden kommune" : "Kommunikation" + " - " + k.Besked.Advis.FirstOrDefault().AdvisType.Navn,
                                    Person = string.Format("Fra: {0} {1} Til: {2} {3}", k.Person.Navn, k.Person.Efternavn, k.Person1.Navn, k.Person1.Efternavn),
                                    Link = k.Besked.Advis.FirstOrDefault() != null ? LinkHelperAdvis(k.Besked.Advis.First().Id) : null
                                });

                var advisListe = (from ad in a.Advis
                                  select new HistorikAnmeldelseModel
                                  {
                                      Id = ad.Id,
                                      Tid = ad.Besked.Tid,
                                      Handling = "Advis",
                                      Person = (ad.Person != null ? "Til: " + ad.Person.Navn + " " + ad.Person.Efternavn : "-"),
                                      Link = LinkHelperAdvis(ad.Id)
                                  });

                var samletliste = new List<HistorikAnmeldelseModel>();
                samletliste.AddRange(historikListe);
                samletliste.AddRange(komListe);
                samletliste.AddRange(advisListe);
                samletliste.Sort((p1, p2) => -p1.Tid.CompareTo(p2.Tid));
                var result = samletliste.ToDataSourceResult(request);
                return Json(result);
            }
            return Json(null);
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

        private static string LinkHelperAdvis(Guid advisId)
        {
            if (advisId != Guid.Empty)
                return "<div style=\"cursor: pointer;\" onclick=\"openAdvis('" + advisId + "')\">Vis</div>";
            return string.Empty;
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
    }
}
