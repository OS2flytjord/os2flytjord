using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Spatial;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using Niras.Jordflytning.Areas.Backend.ViewModels;
using Niras.Jordflytning.Core;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Library.Logging;
using Niras.Jordflytning.ViewModels;
using Niras.Jordflytning.ViewModels.Backend;
using Niras.Jordflytning.ViewModels.GridBindingModels;
using WebGrease.Css.Extensions;

namespace Niras.Jordflytning.Areas.Backend.Controllers
{
    [Authorize]
    public class AdministrationController : Controller
    {
        private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Areas.Backend.Controllers.AdministrationController");

        #region *** Variables ***

        private readonly IJordmodtagerBusiness _jordmodtagerBusiness;
        private readonly IModtagerAnlaegBusiness _modtagerAnlaegBusiness;
        private readonly ISecurityProvider _securityProvider;
        private readonly IBrugereBusiness _brugereBusiness;
        private readonly IDokumenterBusiness _dokumenterBusiness;
        private readonly IForureningskomponentBusiness _forureningskomponentBusiness;
        private readonly IKodelisteBusiness _kodelisteBusiness;
        private readonly IKommuneBusiness _kommuneBusiness;
        private readonly IBetalerBusiness _betalerBusiness;
        private readonly IStatusBetalerBusiness _statusbetalerBusiness;
        private readonly IBrugereBusiness _brugerBusiness;
        private readonly IBogholderOpslagstavleBusiness _bogholderOpslagstavleBusiness;
        private readonly IAdviseringBusiness _adviseringBusiness;
        private readonly IAnmeldelserBusiness _anmeldelseBusiness;

        private const string DokumenterBaseFileDir = "DokumenterBaseFileDir";

        #endregion *** Variables ***

        #region *** Constructor ***

        public AdministrationController(
          ISecurityProvider securityProvider,
          IModtagerAnlaegBusiness modtagerAnlaegBusiness,
          IJordmodtagerBusiness jordmodtagerBusiness,
          IBrugereBusiness brugereBusiness,
          IDokumenterBusiness dokumenterBusiness,
           IForureningskomponentBusiness forureningskomponentBusiness,
          IKodelisteBusiness kodelisteBusiness,
          IKommuneBusiness kommuneBusiness,
          IBetalerBusiness betalerBusiness,
          IStatusBetalerBusiness statusBetalerBusiness,
          IBrugereBusiness brugerBusiness,
          IBogholderOpslagstavleBusiness bogholderOpslagstavleBusiness,
          IAdviseringBusiness adviseringBusiness,
          IAnmeldelserBusiness anmeldelserBusiness
          )
        {
            _jordmodtagerBusiness = jordmodtagerBusiness;
            _modtagerAnlaegBusiness = modtagerAnlaegBusiness;
            _securityProvider = securityProvider;
            _brugereBusiness = brugereBusiness;
            _dokumenterBusiness = dokumenterBusiness;
            _forureningskomponentBusiness = forureningskomponentBusiness;
            _kodelisteBusiness = kodelisteBusiness;
            _kommuneBusiness = kommuneBusiness;
            _betalerBusiness = betalerBusiness;
            _statusbetalerBusiness = statusBetalerBusiness;
            _brugerBusiness = brugerBusiness;
            _bogholderOpslagstavleBusiness = bogholderOpslagstavleBusiness;
            _adviseringBusiness = adviseringBusiness;
            _anmeldelseBusiness = anmeldelserBusiness;
        }

        #endregion *** Constructor ***

        #region *** Betaler ***



        private static string LinkHelperAdvis(Guid advisId)
        {

            var res = "";
            if (advisId != Guid.Empty)
            {
                res = "<div style=\"cursor: pointer;\" onclick=\"openAdvis('" + advisId.ToString() +
                      "')\">Vis advis</div>";
            }
            return res;

        }

        [HttpPost]
        public ActionResult ReadOpslagstavle([DataSourceRequest] DataSourceRequest request, string betalerId, string jordmodtagerId)
        {
            Guid gj;
            Guid gb;
            if (Guid.TryParse(betalerId, out gb) && Guid.TryParse(jordmodtagerId, out gj))
            {
                var betaler = _betalerBusiness.ReadBetaler(gb);
                if (betaler.BogholderOpslagstavle != null)
                {
                    var bos = (from bb in betaler.BogholderOpslagstavle where bb.Jordmodtager.Id == gj select bb).OrderBy(f => f.Tid).ToList();
                    var bom = ConvertBogholderOpslagstavleToModel(bos);
                    DataSourceResult result = bom.ToDataSourceResult(request);
                    return Json(result);
                }
            }
            return Json(null);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteOpslagstavleItem([DataSourceRequest] DataSourceRequest request, BogholderOpslagstavleModel bogholderOpslagstavleModel, string betalerId, string jordmodtagerId)
        {
            Guid gj;
            Guid gb;
            if (Guid.TryParse(betalerId, out gb) && Guid.TryParse(jordmodtagerId, out gj) && bogholderOpslagstavleModel.Id.HasValue)
            {

                var itemToBeDeleted = _bogholderOpslagstavleBusiness.Read(bogholderOpslagstavleModel.Id.Value);
                _bogholderOpslagstavleBusiness.Delete(itemToBeDeleted);

                var betaler = _betalerBusiness.ReadBetaler(gb);
                if (betaler.BogholderOpslagstavle != null)
                {
                    var bos = (from bb in betaler.BogholderOpslagstavle where bb.Jordmodtager.Id == gj select bb).OrderBy(f => f.Tid).ToList();

                    var bom = ConvertBogholderOpslagstavleToModel(bos);
                    DataSourceResult result = bom.ToDataSourceResult(request);
                    return Json(result);
                }
            }
            return Json(null);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateOpslagstavleItem([DataSourceRequest] DataSourceRequest request, BogholderOpslagstavleModel bogholderOpslagstavleModel, string betalerId, string jordmodtagerId)
        {
            Guid gj;
            Guid gb;
            if (Guid.TryParse(betalerId, out gb) && Guid.TryParse(jordmodtagerId, out gj) && bogholderOpslagstavleModel.Id.HasValue)
            {

                var itemToBeUpdated = _bogholderOpslagstavleBusiness.Read(bogholderOpslagstavleModel.Id.Value);
                var b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);

                itemToBeUpdated.Person = b.Person;
                itemToBeUpdated.Tekst = bogholderOpslagstavleModel.Tekst;
                itemToBeUpdated.Tid = DateTime.Now;
                _bogholderOpslagstavleBusiness.Create(itemToBeUpdated);

                var betaler = _betalerBusiness.ReadBetaler(gb);
                if (betaler.BogholderOpslagstavle != null)
                {
                    var bos = (from bb in betaler.BogholderOpslagstavle where bb.Jordmodtager.Id == gj select bb).OrderBy(f => f.Tid).ToList();

                    var bom = ConvertBogholderOpslagstavleToModel(bos);
                    DataSourceResult result = bom.ToDataSourceResult(request);
                    return Json(result);
                }
            }
            return Json(null);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateOpslagstavleItem([DataSourceRequest] DataSourceRequest request, BogholderOpslagstavleModel bogholderOpslagstavleModel, string betalerId, string jordmodtagerId)
        {
            Guid gj;
            Guid gb;
            if (Guid.TryParse(betalerId, out gb) && Guid.TryParse(jordmodtagerId, out gj))
            {
                var b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);

                var bot = new BogholderOpslagstavle();
                bot.Betaler = _betalerBusiness.ReadBetaler(gb);
                bot.Jordmodtager = _jordmodtagerBusiness.Read(gj);
                bot.Person = b.Person;
                bot.Tekst = bogholderOpslagstavleModel.Tekst;
                bot.Tid = DateTime.Now;
                _bogholderOpslagstavleBusiness.Create(bot);

                return Json(true);
            }
            return Json(null);
        }

        public ActionResult AdminBetaler(string betalerId)
        {
            var model = new AdminBetalerModel();

            var b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
            Jordmodtager jordmodtager = null;
            var jordmodtagers = _brugerBusiness.ReadJordmodtagerForPerson(b.Person.Id);
            if (jordmodtagers.Count == 0)
            {
                ViewBag.Message = "Du er ikke logget ind som en person knyttet til et jordmodtagerfirma!";
                ModelState.Clear();
                return View("AdminBetaler", model);
            }
            else if (jordmodtagers.Count > 1)
            {
                ViewBag.Message = "Denne side understøtter ikke at du tilknyttet flere jordmodtagerfirmaer!";
                ModelState.Clear();
                return View("AdminBetaler", model);
            }
            else if (jordmodtagers.Count == 1)
            {
                jordmodtager = jordmodtagers[0];
            }

            Guid gb;
            if (Guid.TryParse(betalerId, out gb) && jordmodtager != null)
            {
                var betaler = _betalerBusiness.ReadBetaler(gb);
                model = initModel(model, betaler);
                model.BetalerId = gb.ToString();
                model.JordmodtagerId = jordmodtager.Id.ToString();

                var betalerStatus = _statusbetalerBusiness.GetStatusBetalerForBetaler(betaler, jordmodtager.Id);
                model.StatusBetalerListe = ConvertStatusBetalerToModel(betalerStatus);


                //StatusBetaler oplsyninger
                var statusbetaler = _statusbetalerBusiness.GetStatusBetaler(betaler.Id, jordmodtager.Id);
                if (statusbetaler != null)
                {
                    model.StatusBetalerId = statusbetaler.Id;
                    model.StatusBetalerGodkendt = statusbetaler.Godkendt;
                    model.Kernekunde = statusbetaler.KerneKunde.HasValue && statusbetaler.KerneKunde.Value;
                    model.StatusRedigeret = statusbetaler.Redigeret;
                    model.InternBemaerkning = statusbetaler.Bemaerkning;
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult GemBetaler(AdminBetalerModel model)
        {
            string message = "";
            try
            {


                Guid gb;
                Guid gj;
                if (Guid.TryParse(model.BetalerId, out gb) && Guid.TryParse(model.JordmodtagerId, out gj))
                {
                    var betaler = _betalerBusiness.ReadBetaler(gb);
                    var isBetalerStatusChanged = false;
                    var statusbetaler = (from sb in betaler.StatusBetaler where sb.Jordmodtager.Id == gj select sb).FirstOrDefault();
                    if (statusbetaler == null)
                    {
                        var jordmodtager = _jordmodtagerBusiness.Read(gj);
                        var sb = new StatusBetaler();
                        sb.Betaler = betaler;
                        sb.Jordmodtager = jordmodtager;
                        sb.KerneKunde = model.Kernekunde;
                        sb.Redigeret = DateTime.Now;
                        sb.Godkendt = model.StatusBetalerGodkendt;
                        sb.Bemaerkning = model.InternBemaerkning;
                        _statusbetalerBusiness.Create(sb);

                    }
                    else
                    {
                        statusbetaler.KerneKunde = model.Kernekunde;
                        statusbetaler.Redigeret = DateTime.Now;


                        isBetalerStatusChanged = (statusbetaler.Godkendt != model.StatusBetalerGodkendt);

                        statusbetaler.Godkendt = model.StatusBetalerGodkendt;

                        statusbetaler.Bemaerkning = model.InternBemaerkning;
                        _statusbetalerBusiness.Create(statusbetaler);

                        //Tjekker om der er nogle anmeldelser, som er er klar til at blive aktiveret. 
                        //Dette er tilfældet, hvis betaler har accepteret betaling, kommune har godkendt 
                        //anmeldelsen og jordmodtagerfirmaers miljømedarbejder har accepteret jorden og det eneste der mangler er bogholderens accept af betaleren.
                        if (isBetalerStatusChanged)
                        {
                            if (statusbetaler.Godkendt == true)
                            {
                                var antalAktivederetAnmeldeleser = _anmeldelseBusiness.AktiverAnmeldelserIfmBogholderAcceptererBetaler(gj, gb);
                                if (antalAktivederetAnmeldeleser > 0)
                                    message += "Betaler har fået aktiveret " + antalAktivederetAnmeldeleser + " anmeldelser.\n";
                            }

                            //Sender advis til relevante personer om at betaleren er blevet afvist.
                            if (statusbetaler.Godkendt == false)
                            {
                                var betalersAnmeldelser = _anmeldelseBusiness.GetBetalersAktiveAnmeldelserHosJordmodtager(betaler.Id, gj);
                                var advisResult = _adviseringBusiness.SendBeskedVedrBetalerAfvistAfBogholder(betalersAnmeldelser);
                                if (advisResult)
                                {
                                    message += "Betaler har fået låst følgende anmeldelser - hermed er der ingen adgang på modtageanlægget:\n";
                                    //Afslutter anmeldelser, hvor betaler er blevet afvist.
                                    foreach (var a in betalersAnmeldelser)
                                    {
                                        message += a.Oprindelsessted.Adresse + "\n";
                                    }
                                    message += "\nAdvis er blevet sendt til anmeldere, transportør og betaler.";
                                }

                            }
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                message = "Der skette en fejl!";
            }


            message += "\n\nOplysningerne er gemt!";

            //ModelState.Clear();
            //return View("AdminBetaler", model);
            return Json(new { Message = message });
        }

        private AdminBetalerModel initModel(AdminBetalerModel model, Betaler betaler)
        {
            if (betaler.Person != null)
            {
                if (betaler.Person.Firmaoplysninger != null)
                {
                    //Hvis firma
                    model.Firmanavn = betaler.Person.Firmaoplysninger.Firmanavn;
                    model.Adresse = betaler.Person.Firmaoplysninger.Adresse;
                    model.Postnr = betaler.Person.Firmaoplysninger.Postnummer;
                    model.PostDistrikt = betaler.Person.Firmaoplysninger.Postdistikt;
                    //model.Telefon = betaler.Person.Firmaoplysninger.Telefon; //Firmatelefon bliver aldrig brugt i brugerfladen
                    model.Telefon = betaler.Person.Telefon;
                    model.Cvr = betaler.Person.Firmaoplysninger.CVR;
                    model.Pnummer = betaler.Person.Firmaoplysninger.PNummer;
                    model.EAN = betaler.Person.Firmaoplysninger.EAN.ToString();


                    model.PersonNavn = betaler.Person.Navn + " " + betaler.Person.Efternavn;
                    model.Email = betaler.Person.Email;
                }
                else
                {
                    //Privat person

                    model.Firmanavn = "-";
                    model.Adresse = betaler.Person.Adresse;
                    model.Postnr = betaler.Person.Postnummer;
                    model.PostDistrikt = betaler.Person.Postdistrikt;
                    model.Telefon = betaler.Person.Telefon;
                    model.Cvr = null;
                    model.Pnummer = "";
                    model.EAN = "";


                    model.PersonNavn = betaler.Person.Navn + " " + betaler.Person.Efternavn;
                    model.Email = betaler.Person.Email;
                }
            }

            //Rights
            model.RightIsBogholder = _securityProvider.CurrentUser.IsInRole("Bogholder");

            return model;
        }

        private IEnumerable<StatusBetalerModel> ConvertStatusBetalerToModel(IEnumerable<StatusBetaler> sbs)
        {
            var res = (from sb in sbs
                       select
                         new StatusBetalerModel
                           {
                               Id = sb.Id,
                               Godkend = sb.Godkendt,
                               Dato = sb.Redigeret,
                               Jordmodtager = sb.Jordmodtager.Navn
                           }).ToList();
            return res;
        }

        private IEnumerable<BogholderOpslagstavleModel> ConvertBogholderOpslagstavleToModel(
          IEnumerable<BogholderOpslagstavle> bos)
        {
            var res = (from bo in bos
                       select
                         new BogholderOpslagstavleModel
                           {
                               Id = bo.Id,
                               Tekst = bo.Tekst,
                               UdfoertAf = (bo.Person.Navn + " " + bo.Person.Efternavn),
                               Tid = bo.Tid
                           }).ToList();
            return res;
        }

        [HttpPost]
        public ActionResult AdviserBetaler(string besked, Guid betalerId, Guid jordmodtagerId)
        {

            //Adviser
            var res = _adviseringBusiness.SendBeskedTilBetalerFraBogholder(besked, betalerId, jordmodtagerId);

            return Json(res);
        }

        #endregion *** Betaler ***

        #region *** BrugerAdministration ***

        [HttpPost]
        public ActionResult Tilfoejbrugertilorganisation(string email, string organisationsid)
        {
            var result = false;
            var currentUser = _brugereBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
            var userToAdd = _brugereBusiness.Read(email);
            if (currentUser != null && userToAdd != null && userToAdd.Person != null)
            {
                var personKommune = currentUser.Person.PersonKommune.FirstOrDefault(x => x.Kommune.Id == new Guid(organisationsid));

                if (personKommune != null)
                {
                    var newPersonKommune = new PersonKommune();
                    newPersonKommune.KommuneId = new Guid(organisationsid);
                    newPersonKommune.Person = userToAdd.Person;
                    personKommune.Kommune.PersonKommune.Add(newPersonKommune);
                    _brugereBusiness.SaveChanges();
                    if (newPersonKommune.Id != Guid.Empty)
                        result = true;
                }
                else
                {
                    var personJordmodtager = currentUser.Person.PersonJordmodtager.FirstOrDefault(x => x.Jordmodtager.Id == new Guid(organisationsid));
                    if (personJordmodtager != null)
                    {
                        var newPersonJordmodtager = new PersonJordmodtager();
                        newPersonJordmodtager.JordmodtagerId = new Guid(organisationsid);
                        newPersonJordmodtager.Person = userToAdd.Person;
                        personJordmodtager.Jordmodtager.PersonJordmodtager.Add(newPersonJordmodtager);
                        _brugereBusiness.SaveChanges();
                        if (newPersonJordmodtager.Id != Guid.Empty)
                        {
                            result = true;
                        }
                    }
                }
            }
            var returnVal = Json(result ? new { Success = true } : new { Success = false });
            return returnVal;
        }

        public ActionResult AdminBrugere()
        {

            IList<SelectListItem> orgs = new List<SelectListItem>();
            var bruger = _brugereBusiness.Read(_securityProvider.CurrentUser.Identity.Name);

            var model = new BrugerAdministrationModel();

            model.VisKommuneRoller = _securityProvider.CurrentUser.IsInRole(ApplicationConstants.KommuneAdminRolle);
            model.VisJordmodtagerRoller = _securityProvider.CurrentUser.IsInRole(ApplicationConstants.JordmodtagerAdminRolle);

            if (model.VisKommuneRoller)
            {
                var orgKommuneList = _brugerBusiness.GetUsersKommuneOrganisationer(bruger.Person.Id);
                foreach (var kommune in orgKommuneList)
                {
                    var item = new SelectListItem { Value = kommune.Id.ToString(), Text = kommune.Navn, Selected = false };
                    orgs.Add(item);
                }
            }

            model.VisUnderskrift = orgs.Count >= 1;
            if (model.VisJordmodtagerRoller)
            {
                var orgJordModtList = _brugerBusiness.GetUsersJordModtOrganisationer(bruger.Person.Id);
                foreach (var jordMdtg in orgJordModtList)
                {
                    var item = new SelectListItem { Value = jordMdtg.Id.ToString(), Text = jordMdtg.Navn, Selected = false };
                    orgs.Add(item);
                }
            }

            if (orgs.Any())
                orgs.First(x => x.Selected = true);

            ViewData["organisationer"] = orgs;
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveJohnHancock(IEnumerable<HttpPostedFileBase> attachments)
        {
            var result = false;
            var b = _brugereBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
            var person = b.Person;

            if (person != null)
            {
                // The Name of the Upload component is "attachments" 
                foreach (var file in attachments)
                {
                    var d = new Dokumentation();
                    d.Id = Guid.NewGuid();
                    d.Filnavn = Path.GetFileName(file.FileName);
                }
                result = true;
            }
            return Json(new { res = result }, "text/plain");
        }

        #region BrugeradministrationsGrid

        public ActionResult BrugeradministrationsGridRead([DataSourceRequest] DataSourceRequest request, Guid? organisationsId = null)
        {
            ModelState.Clear();
            IList<BrugerProfilGridModel> brugere = new List<BrugerProfilGridModel>();
            IList<string> usernameOfUsersToGet = new List<string>();
            if (_securityProvider.CurrentUser.Identity.IsAuthenticated)
            {
                var brugerProfil = _brugereBusiness.Read(_securityProvider.CurrentUser.Identity.Name);

                if (brugerProfil.Roles != null && brugerProfil.Roles.Count > 0)
                {

                    if (organisationsId != null && organisationsId != Guid.Empty)
                    {
                        usernameOfUsersToGet = _brugereBusiness.ReadKommunePersoner(organisationsId.Value).Select(x => x.Person.Email).ToList();
                        usernameOfUsersToGet.AddRange(_brugereBusiness.ReadJordmodtagerPersoner(organisationsId.Value).Select(x => x.Person.Email).ToList());
                    }
                    else
                    {
                        //Brugeren er kommuneadministrator
                        if (brugerProfil.Roles.Contains(ApplicationConstants.KommuneAdminRolle))
                        {
                            foreach (var kommune in brugerProfil.Person.PersonKommune)
                            {
                                usernameOfUsersToGet.AddRange(_brugereBusiness.ReadKommunePersoner(kommune.KommuneId).Select(x => x.Person.Email));
                            }
                        }
                        //Brugeren er jordmodtageradministrator
                        if (brugerProfil.Roles.Contains(ApplicationConstants.JordmodtagerAdminRolle))
                        {
                            foreach (var item in brugerProfil.Person.PersonJordmodtager)
                            {
                                usernameOfUsersToGet.AddRange(_brugereBusiness.ReadJordmodtagerPersoner(item.JordmodtagerId).Select(x => x.Person.Email));
                            }
                        }
                    }
                    foreach (var item in usernameOfUsersToGet)
                    {
                        var bProfil = _brugereBusiness.Read(item);
                        if (bProfil == null)
                            continue;

                        var bModel = new BrugerProfilGridModel(bProfil);
                        brugere.Add(bModel);
                    }
                }
            }
            return Json(brugere.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BrugeradministrationsGridUpdate([DataSourceRequest] DataSourceRequest request, BrugerProfilGridModel brugerProfil)
        {
            ModelState.Clear();
            if (brugerProfil != null && ModelState.IsValid && brugerProfil.BrugerId > 0)
            {
                var brugerprofilToUpdate = _brugereBusiness.Read(brugerProfil.BrugerNavn);

                if (brugerprofilToUpdate != null)
                {
                    brugerprofilToUpdate.Person.Adresse = brugerProfil.Adresse;
                    brugerprofilToUpdate.Person.Aktiv = brugerProfil.Aktiv;
                    brugerprofilToUpdate.Person.By = brugerProfil.By;
                    brugerprofilToUpdate.Person.Efternavn = brugerProfil.Efternavn;
                    brugerprofilToUpdate.Person.Email = brugerProfil.Email;
                    brugerprofilToUpdate.Person.Mobiltelefon = brugerProfil.Mobiltelefon;
                    brugerprofilToUpdate.Person.Navn = brugerProfil.Navn;
                    brugerprofilToUpdate.Person.Postdistrikt = brugerProfil.Postdistrikt;
                    brugerprofilToUpdate.Person.Postnummer = brugerProfil.Postnummer;
                    brugerprofilToUpdate.Person.Telefon = brugerProfil.Telefon;
                    _brugereBusiness.Update(brugerprofilToUpdate, brugerProfil.GetRoles());
                }
            }
            return Content(ModelState.ToString());
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BrugeradministrationsGridCreate([DataSourceRequest] DataSourceRequest request, BrugerProfilGridModel brugerProfil)
        {
            if (brugerProfil != null && ModelState.IsValid)
            {
                brugerProfil.BrugerNavn = brugerProfil.Email;
                var bruger = brugerProfil.ToBrugerProfil();


                _brugereBusiness.Create(bruger, brugerProfil.GetRoles().ToArray(), brugerProfil.ValgtOrganisation);
                brugerProfil.Id = bruger.Person.Id;
                brugerProfil.BrugerId = bruger.BrugerId;
            }
            return Json(new[] { brugerProfil }.ToDataSourceResult(request, ModelState));
            //return Content(ModelState.ToString());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BrugeradministrationsGridDestroy([DataSourceRequest] DataSourceRequest request, BrugerProfilGridModel brugerProfil)
        {
            if (brugerProfil != null && brugerProfil.ValgtOrganisation != null)
            {
                var userToRemove = _brugereBusiness.Read(brugerProfil.BrugerNavn);
                var personKommune = userToRemove.Person.PersonKommune.FirstOrDefault(x => x.Kommune.Id == new Guid(brugerProfil.ValgtOrganisation));

                if (personKommune != null)
                {
                    _brugereBusiness.DeletePersonKommune(userToRemove.Person.PersonKommune.First(x => x.KommuneId == new Guid(brugerProfil.ValgtOrganisation)));
                    _brugereBusiness.SaveChanges();
                }
                else
                {
                    _brugereBusiness.DeletePersonJordmodtager(userToRemove.Person.PersonJordmodtager.First(x => x.Jordmodtager.Id == new Guid(brugerProfil.ValgtOrganisation)));
                    _brugereBusiness.SaveChanges();
                }
            }
            else
            {
                ModelState.AddModelError("BrugerProfilGridModel.ValgtOrganisation", @"Vælg organisation");
            }
            return Json(ModelState.ToDataSourceResult());
        }

        #endregion BrugeradministrationsGrid

        #endregion *** BrugerAdministration ***

        #region *** Jordmodtager ***

        #region *** Jordmodtager List ***

        public ActionResult AdminJordmodtager()
        {
            var model = new JordmodtagerListModel();
            var list = GetJordModtagerList(null);
            model.JordModtagerList = list;
            return View(model);
        }

        public ActionResult AdminModtageAnlaeg()
        {
            var model = new JordmodtagerListModel();
            var pers = GetPerson();
            var list = GetJordModtagerList(pers.Id);
            model.JordModtagerList = list;
            return View("AdminJordmodtager", model);
        }

        public ActionResult AdminModtagerAnlaeg()
        {
            var list = _jordmodtagerBusiness.ReadJordmodtagere();
            var newList = new List<ModtagerAnlaegViewModel>();

            foreach (var item in list)
            {
                var modtagerList = GetAnlaegList(item).ToList();
                newList.AddRange(modtagerList);
            }

            newList = newList.OrderBy(q => q.Navn).ToList();
            return View("AdminModtagerAnlaeg", newList);
        }

        public ActionResult GetModtageAnlaeg(string text)
        {
            var x = Request.Form;

            var list = _jordmodtagerBusiness.ReadJordmodtagere()
                //.Where(q => q.Navn == "Aarhus Oliehavn - ren jord")
                .OrderBy(q => q.Navn)
                .Select(q => q.Navn)
                .ToList();

            //var newList = new List<ModtagerAnlaegViewModel>();

            //foreach (var item in list)
            //{
            //    var modtagerList = GetAnlaegList(item).ToList();
            //    newList.AddRange(modtagerList);
            //}
            //var strList = newList
            //    .Where(q => newList
            //        .Any(r => r.JordmodtagerNavn.ToUpperInvariant()
            //            .Contains(filter.ToUpperInvariant())))
            //    .Select(q => q.JordmodtagerNavn)
            //    .ToList();

            var filteredList = list.FindAll(q => q.ToUpperInvariant().Contains(text.ToUpperInvariant())
                || q.ToUpperInvariant().StartsWith(text.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase));

            return Json(filteredList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReadJordklassifikation(Guid landsdelTypeId)
        {
            var jk = _kodelisteBusiness.ReadAktiveJordKlassifikationTypesForLandsdel(landsdelTypeId);
            var list = jk.Select(jkt => new { Value = jkt.Id.ToString(), Text = jkt.Navn }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReadJordmodtagere([DataSourceRequest] DataSourceRequest request)
        {
            Guid? userId = null;
            if (_securityProvider.CurrentUser.IsInRole(ApplicationConstants.JordmodtagerAdminRolle))
            {
                var person = GetPerson();
                userId = person.Id;
            }

            var t = GetJordModtagerList(userId);
            var result = t.ToDataSourceResult(request);
            return Json(result);
        }

        private IList<JordmodtagerViewModel> GetJordModtagerList(Guid? userId)
        {
            IList<Jordmodtager> modtagerList;
            if (userId == null || userId == Guid.Empty)
                modtagerList = _jordmodtagerBusiness.ReadJordmodtagere();
            else
                modtagerList = _jordmodtagerBusiness.GetJordModtagerListForUserAll((Guid)userId);

            var list = new List<JordmodtagerViewModel>();
            foreach (var jordmodtager in modtagerList)
            {
                // Hvis der udelukkende er brugeroprettede midlertidige anlæg, skal denne modtager ikke vises.
                // Bemærk: Der kan være tidligere data fejl, derfor bruges her ikke en .Any()
                if (!jordmodtager.ModtagerAnlaeg.Any() || !jordmodtager.ModtagerAnlaeg.All(m => m.AnmelderOprettetMidlertidigtAnlaeg)) 
                {
                    var viewModel = MapToJordmodtagerViewModel(jordmodtager);
                    list.Add(viewModel);
                }
            }

            list = list.OrderByDescending(j => j.Aktiv).ThenBy(x => x.Navn).ToList();

            return list;
        }

        public ActionResult SelectJordmodtager(string id)
        {
            var model = new ModtagerAnlaegAdminModel();

            var guid = Guid.Empty;
            if (!String.IsNullOrEmpty(id) && !id.StartsWith("0000"))
                guid = new Guid(id);

            var jordModtager = _jordmodtagerBusiness.Read(guid);
            model.ModtagerAnlaegList = GetAnlaegList(jordModtager);
            model.SelectedJordmodtager = MapToJordmodtagerViewModel(jordModtager);

            ModelState.Clear();
            return View("AdminModtageAnlaeg", model);
        }

        #endregion *** Jordmodtager List ***

        #region *** Jordmodtager Edit ***

        [HttpPost]
        public ActionResult CreateJordmodtager([DataSourceRequest] DataSourceRequest request, JordmodtagerViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var jordModtager = new Jordmodtager();
                MapToJordmodtager(jordModtager, model);
                if (ValidateJordmodtager(jordModtager))
                    _jordmodtagerBusiness.Create(jordModtager);
                else
                    ModelState.AddModelError("gemfejl", @"Fejl i input");
                model.Id = jordModtager.Id;
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }


        [HttpPost]
        public ActionResult EditJordmodtager(ModtagerAnlaegAdminModel model, string command)
        {
            if (ModelState.IsValid && model.SelectedJordmodtager != null)
            {
                ModelState.Clear();
                var jordModtager = _jordmodtagerBusiness.Read(model.SelectedJordmodtager.Id);
                MapToJordmodtager(jordModtager, model.SelectedJordmodtager);

                if (jordModtager.ModtagerAnlaeg != null && jordModtager.ModtagerAnlaeg.Count() != 0)
                {
                    foreach (var modtageranlaeg in jordModtager.ModtagerAnlaeg)
                    {
                        modtageranlaeg.AnvenderJF = model.SelectedJordmodtager.JordmodtagerMedFlytjordAbonnement;
                    }
                    _modtagerAnlaegBusiness.SaveChanges();
                }

                if (ValidateJordmodtager(jordModtager))
                    _jordmodtagerBusiness.SaveChanges();
            }
            return View("AdminModtageAnlaeg", model);
        }

        private static bool ValidateJordmodtager(Jordmodtager jordmodtager)
        {
            if (jordmodtager == null) return false;
            if (String.IsNullOrEmpty(jordmodtager.Navn)) return false;
            if (String.IsNullOrEmpty(jordmodtager.Adresse)) return false;
            if (String.IsNullOrEmpty(jordmodtager.PostDistrikt)) return false;
            if (jordmodtager.CVR == null || jordmodtager.CVR > 999999999999999) return false;
            if (jordmodtager.Postnummer == null || jordmodtager.Postnummer > 9999) return false;
            return true;
        }

        private static JordmodtagerViewModel MapToJordmodtagerViewModel(Jordmodtager jordmodtager)
        {
            var viewModel = new JordmodtagerViewModel();
            viewModel.Adresse = jordmodtager.Adresse;
            viewModel.Id = jordmodtager.Id;
            viewModel.By = jordmodtager.PostDistrikt;
            viewModel.Postnummer = jordmodtager.Postnummer == null ? "" : jordmodtager.Postnummer.ToString();
            viewModel.Cvr = jordmodtager.CVR == null ? "" : jordmodtager.CVR.ToString();
            viewModel.Telefon = jordmodtager.Telefon == null ? "" : jordmodtager.Telefon.ToString();
            viewModel.Navn = jordmodtager.Navn;
            viewModel.Aktiv = jordmodtager.Aktiv;
            viewModel.JordmodtagerMedFlytjordAbonnement = jordmodtager.AnvenderJF.GetValueOrDefault();
            return viewModel;
        }

        private static void MapToJordmodtager(Jordmodtager jordmodtager, JordmodtagerViewModel viewModel)
        {
            jordmodtager.Navn = viewModel.Navn;
            jordmodtager.Adresse = viewModel.Adresse;
            jordmodtager.PostDistrikt = viewModel.By;
            jordmodtager.Postnummer = String.IsNullOrEmpty(viewModel.Postnummer) ? null : (decimal?)Decimal.Parse(viewModel.Postnummer);
            jordmodtager.CVR = String.IsNullOrEmpty(viewModel.Cvr) ? null : (decimal?)Decimal.Parse(viewModel.Cvr);
            jordmodtager.Telefon = String.IsNullOrEmpty(viewModel.Telefon) ? null : (decimal?)Decimal.Parse(viewModel.Telefon);
            jordmodtager.Aktiv = viewModel.Aktiv;
            jordmodtager.AnvenderJF = viewModel.JordmodtagerMedFlytjordAbonnement;
        }

        #endregion *** Jordmodtager Edit ***

        #region *** ModtageAnlæg ***

        public ActionResult SelectModtagerAnlaeg(string id)
        {
            var guid = new Guid(id);
            var anlaeg = _modtagerAnlaegBusiness.Read(guid);
            var jordGuidNullable = anlaeg.JordmodtagerId;
            var model = new ModtagerAnlaegAdminModel();

            if (jordGuidNullable != Guid.Empty && jordGuidNullable != null)
            {
                var jordGuid = (Guid)jordGuidNullable;
                var jordModtager = _jordmodtagerBusiness.Read(jordGuid);
                var anlaegTypeList = _kodelisteBusiness.ReadAktiveJordanlaegTypes();
                model.AddJordanlaegType(anlaegTypeList);

                //Henter Landsdele til listen
                var l = _kodelisteBusiness.ReadAktiveLandsdelTypes();
                model.AddLandsdelType(l);
                model.ModtagerAnlaegList = GetAnlaegList(jordModtager);
                model.SelectedJordmodtager = MapToJordmodtagerViewModel(jordModtager);
                model.SelectedModtagerAnlaeg = MapToModtagerAnlaegViewModel(anlaeg);
                model.SelectedModtagerAnlaeg.DokumentList = MapToDokumenterViewModel(anlaeg);
                model.SelectedModtagerAnlaeg.GraenseVaerdierList = MapToGraenseVaerdiList(anlaeg);

                if (anlaeg.JordKlassifikationType != null && anlaeg.JordKlassifikationType.LandsdelType != null)
                {
                    var klassiTypeList = _kodelisteBusiness.ReadAktiveJordKlassifikationTypesForLandsdel(anlaeg.JordKlassifikationType.LandsdelType.Id);
                    model.AddKlassifikationType(klassiTypeList);
                }

                if (anlaeg.JordanlaegType != null)
                    model.SelectedModtagerAnlaeg.JordanlaegTypeId = anlaeg.JordanlaegType.Id;
            }
            ModelState.Clear();
            return View("AdminModtageAnlaeg", model);
        }

        public ActionResult CreateModtagerAnlaeg(Guid jordGuid)
        {
            var model = new ModtagerAnlaegAdminModel();
            var jordModtager = _jordmodtagerBusiness.Read(jordGuid);

            var anlaegTypeList = _kodelisteBusiness.ReadAktiveJordanlaegTypes();
            model.AddJordanlaegType(anlaegTypeList);

            //Henter Landsdele til listen
            var l = _kodelisteBusiness.ReadAktiveLandsdelTypes();
            model.AddLandsdelType(l);

            model.ModtagerAnlaegList = GetAnlaegList(jordModtager);
            model.SelectedJordmodtager = MapToJordmodtagerViewModel(jordModtager);
            model.SelectedModtagerAnlaeg = new ModtagerAnlaegViewModel();
            model.SelectedModtagerAnlaeg.JordmodtagerId = jordModtager.Id;
            model.SelectedModtagerAnlaeg.DokumentList = new List<DokumenterViewModel>();
            model.SelectedModtagerAnlaeg.GraenseVaerdierList = new List<GraenseVaerdierViewModel>();

            var jordanlaegType = anlaegTypeList.FirstOrDefault();
            if (jordanlaegType != null)
                model.SelectedModtagerAnlaeg.JordanlaegTypeId = jordanlaegType.Id;

            if (jordModtager.AnvenderJF == true)
            {
                model.SelectedModtagerAnlaeg.AnvenderJf = true;
                model.SelectedModtagerAnlaeg.Advis = true;
            }

            //default landsdel Vest hvis ingen landsdel er valgt.
            //var defaultLandsdelTypeId = new Guid("1FDC87BB-41E7-462C-8051-274C7DB14787");
            var defaultLandsdelType = _kodelisteBusiness.ReadAktiveLandsdelTypes().FirstOrDefault();
            if (defaultLandsdelType != null)
            {
                var defaultLandsdelTypeId = defaultLandsdelType.Id;
                var klassiTypeList = _kodelisteBusiness.ReadAktiveJordKlassifikationTypesForLandsdel(defaultLandsdelTypeId);
                model.AddKlassifikationType(klassiTypeList);
                model.SelectedModtagerAnlaeg.LandsdelTypeId = defaultLandsdelTypeId;
            }
            ModelState.Clear();
            return View("AdminModtageAnlaeg", model);
        }

        [HttpPost]
        public ActionResult EditModtagerAnlaeg(ModtagerAnlaegAdminModel model, string command)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedModtagerAnlaeg != null)
                {
                    var anlaeg = new ModtagerAnlaeg();

                    if (model.SelectedModtagerAnlaeg.Id != Guid.Empty)
                        anlaeg = _modtagerAnlaegBusiness.Read(model.SelectedModtagerAnlaeg.Id);
                    else
                        command = "Opret";

                    //// Se om der er indtegnede matrikler med i post data:
                    //var matrikelInfoJson = Request["hfIndtegnedeMatriklerJson"];
                    //if (!string.IsNullOrWhiteSpace(matrikelInfoJson))
                    //{
                    //    var matrikelInfoModels = JsonConvert.DeserializeObject<IList<MatrikelInfoModel>>(matrikelInfoJson);
                    //    if (matrikelInfoModels != null)
                    //    {
                    //        var matrikler = new List<ModtagerAnlaegMatrikler>();
                    //        foreach (var matrikelInfoModel in matrikelInfoModels)
                    //        {
                    //            matrikler.Add(new ModtagerAnlaegMatrikler()
                    //            {
                    //                Id = Guid.NewGuid(),
                    //                Ejerlav = matrikelInfoModel.Ejerlav,
                    //                Ejerlavsnavn = matrikelInfoModel.Ejerlavsnavn,
                    //                Matrikelnr = matrikelInfoModel.Matrikelnummer,
                    //                ModtagerAnlaeg = anlaeg
                    //            });
                    //        }
                    //        // Gem et eller andet sted...
                    //    }
                    //}

                    //hfEsrEjendomsnummer

                    switch (command)
                    {
                        case "Slet":
                            _modtagerAnlaegBusiness.Delete(anlaeg);
                            break;
                        case "Gem":
                            MapToModtagerAnlaeg(anlaeg, model);
                            if (ValidateModtageAnlaeg(anlaeg))
                                _modtagerAnlaegBusiness.SaveChanges();
                            else
                                ModelState.AddModelError("GemFejl",
                                    @"Fejl: Felterne er ikke udfyldt korrekt! Prøv igen.");
                            break;
                        case "Opret":
                            MapToModtagerAnlaeg(anlaeg, model);
                            if (ValidateModtageAnlaeg(anlaeg))
                            {
                                anlaeg.Id = new Guid();
                                _modtagerAnlaegBusiness.Create(anlaeg);
                                ModelState.Clear();
                                var jordId = model.SelectedModtagerAnlaeg.JordmodtagerId.ToString();
                                /*
                                 * KTW 22-12-2016: Rettet til JSON output, for klient side redirect, da RedirectToAction ikke virkede. 
                                 * Håndteret i JavaScript funktionen "showOkAlert" i viewet.
                                 */
                                return Json(new { redirectTo = Url.Action("SelectJordmodtager", "Administration", new { id = jordId }) });
                            }
                            ModelState.AddModelError("GemFejl", @"Fejl: Felterne er ikke udfyldt korrekt! Prøv igen.");
                            break;
                    }
                }
            }
            else
            {
                ModelState.AddModelError("GemFejl", @"Fejl: Felterne er ikke udfyldt korrekt! Prøv igen.");
            }
            return View("AdminModtageAnlaeg", model);
        }

        private static bool ValidateModtageAnlaeg(ModtagerAnlaeg anlaeg)
        {
            if (anlaeg.AnvenderJF && anlaeg.KommuneKode == null)
                return false;

            return anlaeg != null;
        }

        private static void MapToModtagerAnlaeg(ModtagerAnlaeg modtagerAnlaeg, ModtagerAnlaegAdminModel viewModel)
        {
            modtagerAnlaeg.Id = viewModel.SelectedModtagerAnlaeg.Id;
            modtagerAnlaeg.JordmodtagerId = viewModel.SelectedModtagerAnlaeg.JordmodtagerId;
            modtagerAnlaeg.CentraltOprettetMidlertidigtAnlaeg = viewModel.SelectedModtagerAnlaeg.CentraltOprettetMidlertidigtAnlaeg;
            modtagerAnlaeg.AnmelderOprettetMidlertidigtAnlaeg = viewModel.SelectedModtagerAnlaeg.AnmelderOprettetMidlertidigtAnlaeg;
            modtagerAnlaeg.Navn = viewModel.SelectedModtagerAnlaeg.Navn;
            modtagerAnlaeg.Adresse = viewModel.SelectedModtagerAnlaeg.Adresse;
            modtagerAnlaeg.PostDistrikt = viewModel.SelectedModtagerAnlaeg.By;
            modtagerAnlaeg.Postnummer = viewModel.SelectedModtagerAnlaeg.Postnummer;
            modtagerAnlaeg.Affald = viewModel.SelectedModtagerAnlaeg.TakesAffald;
            modtagerAnlaeg.AnvenderJF = viewModel.SelectedModtagerAnlaeg.AnvenderJf; //System administrator bestemmer om et modtageranlæg anvender FJ digitalt.
            modtagerAnlaeg.AutoGodkend = viewModel.SelectedModtagerAnlaeg.AutoGodkend;
            modtagerAnlaeg.OphaevAutoGodkendAnmKom = viewModel.SelectedModtagerAnlaeg.OphaevAutoGodkendAnmKom;
            modtagerAnlaeg.Aktiv = viewModel.SelectedModtagerAnlaeg.Aktiv;
            modtagerAnlaeg.JordanlaegTypeId = viewModel.SelectedModtagerAnlaeg.JordanlaegTypeId;
            modtagerAnlaeg.JordKlassifikationTypeId = viewModel.SelectedModtagerAnlaeg.JordKlassifikationTypeId;
            modtagerAnlaeg.www = viewModel.SelectedModtagerAnlaeg.Www;
            modtagerAnlaeg.KontaktpersonNavn = viewModel.SelectedModtagerAnlaeg.KontaktpersonNavn;
            modtagerAnlaeg.KontaktpersonEmail = viewModel.SelectedModtagerAnlaeg.KontaktpersonEmail;
            modtagerAnlaeg.KontaktpersonTlf = viewModel.SelectedModtagerAnlaeg.KontaktpersonTlf;
            modtagerAnlaeg.KommuneKode = viewModel.SelectedModtagerAnlaeg.KommuneKode;
            modtagerAnlaeg.Bemaerkning = viewModel.SelectedModtagerAnlaeg.Bemaerkning;
            modtagerAnlaeg.AktivFra = viewModel.SelectedModtagerAnlaeg.AktivFra;
            modtagerAnlaeg.AktivTil = viewModel.SelectedModtagerAnlaeg.AktivTil;
            modtagerAnlaeg.Advis = viewModel.SelectedModtagerAnlaeg.Advis;
            modtagerAnlaeg.Matrikelnr = viewModel.SelectedModtagerAnlaeg.Matrikelnr;
            modtagerAnlaeg.Ejerlav = viewModel.SelectedModtagerAnlaeg.Ejerlav;
            modtagerAnlaeg.Ejerlavsnavn = viewModel.SelectedModtagerAnlaeg.Ejerlavsnavn;
            modtagerAnlaeg.EsrEjendomsnr = viewModel.SelectedModtagerAnlaeg.EsrEjensomsnummer;

            if (!string.IsNullOrEmpty(viewModel.SelectedModtagerAnlaeg.Wkt))
            {
                var g = DbGeometry.FromText(viewModel.SelectedModtagerAnlaeg.Wkt, 25832);
                modtagerAnlaeg.Geom = g;
            }
        }

        private static List<ModtagerAnlaegViewModel> GetAnlaegList(Jordmodtager jordModtager)
        {
            var list = new List<ModtagerAnlaegViewModel>();
            if (jordModtager != null)
            {
                var anlaeg = jordModtager.ModtagerAnlaeg;
                foreach (var modtagerAnlaeg in anlaeg)
                {
                    var viewModel = MapToModtagerAnlaegViewModel(modtagerAnlaeg);
                    if (viewModel.AnmelderOprettetMidlertidigtAnlaeg == false)
                        list.Add(viewModel);
                }
                list = list.OrderByDescending(j => j.Aktiv).ThenBy(x => x.Navn).ToList();
            }
            return list;
        }

        private static ModtagerAnlaegViewModel MapToModtagerAnlaegViewModel(ModtagerAnlaeg modtagerAnlaeg)
        {
            var viewModel = new ModtagerAnlaegViewModel();
            viewModel.JordmodtagerId = modtagerAnlaeg.JordmodtagerId;
            viewModel.CentraltOprettetMidlertidigtAnlaeg = modtagerAnlaeg.CentraltOprettetMidlertidigtAnlaeg;
            viewModel.AnmelderOprettetMidlertidigtAnlaeg = modtagerAnlaeg.AnmelderOprettetMidlertidigtAnlaeg;
            viewModel.Aktiv = modtagerAnlaeg.Aktiv;
            viewModel.Id = modtagerAnlaeg.Id;
            viewModel.Navn = modtagerAnlaeg.Navn;
            viewModel.Adresse = modtagerAnlaeg.Adresse;
            viewModel.Postnummer = modtagerAnlaeg.Postnummer;
            viewModel.By = modtagerAnlaeg.PostDistrikt;
            viewModel.TakesAffald = modtagerAnlaeg.Affald;
            viewModel.Aktiv = modtagerAnlaeg.Aktiv;
            viewModel.JordanlaegTypeId = modtagerAnlaeg.JordanlaegTypeId;
            viewModel.AnvenderJf = modtagerAnlaeg.AnvenderJF;
            viewModel.JordKlassifikationTypeId = modtagerAnlaeg.JordKlassifikationTypeId;
            viewModel.JordKlassifikationNavn = modtagerAnlaeg.JordKlassifikationType != null ? modtagerAnlaeg.JordKlassifikationType.Navn : "";
            viewModel.JordmodtagerNavn = modtagerAnlaeg.Jordmodtager.Navn;
            viewModel.AutoGodkend = modtagerAnlaeg.AutoGodkend;
            viewModel.OphaevAutoGodkendAnmKom = modtagerAnlaeg.OphaevAutoGodkendAnmKom;
            viewModel.Ejerlav = modtagerAnlaeg.Ejerlav;
            viewModel.Matrikelnr = modtagerAnlaeg.Matrikelnr;
            viewModel.Ejerlavsnavn = modtagerAnlaeg.Ejerlavsnavn;
            viewModel.EsrEjensomsnummer = modtagerAnlaeg.EsrEjendomsnr;

            if (modtagerAnlaeg.Geom != null)
                viewModel.Wkt = modtagerAnlaeg.Geom.AsText();
            viewModel.Www = modtagerAnlaeg.www;
            viewModel.KontaktpersonNavn = modtagerAnlaeg.KontaktpersonNavn;
            viewModel.KontaktpersonEmail = modtagerAnlaeg.KontaktpersonEmail;
            viewModel.KontaktpersonTlf = modtagerAnlaeg.KontaktpersonTlf;
            viewModel.KommuneKode = modtagerAnlaeg.KommuneKode;
            viewModel.Bemaerkning = modtagerAnlaeg.Bemaerkning;
            viewModel.AktivFra = modtagerAnlaeg.AktivFra;
            viewModel.AktivTil = modtagerAnlaeg.AktivTil;
            viewModel.Advis = modtagerAnlaeg.Advis;

            if (modtagerAnlaeg.JordKlassifikationType != null && modtagerAnlaeg.JordKlassifikationType.LandsdelType != null)
            {
                viewModel.LandsdelTypeId = modtagerAnlaeg.JordKlassifikationType.LandsdelType.Id;
            }
            else
            {
                //default landsdel Vest hvis ingen landsdel er valgt.
                viewModel.LandsdelTypeId = ApplicationConstants.LandsdelTypeVestGuid;
            }

            return viewModel;
        }

        #endregion *** ModtageAnlæg ***

        #region *** ModtageAnlæg Dokumenter ***

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadDocument(IEnumerable<HttpPostedFileBase> files, Guid modtageAnlaegGuid)
        {

            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    _dokumenterBusiness.SaveFile(modtageAnlaegGuid, file);
                    break; // kun een fil ad gangen
                }
            }
            // Return an empty string to signify success
            ModelState.Clear();
            const string message = "";
            return Content(message);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemoveTheDocument([DataSourceRequest] DataSourceRequest request, DokumenterViewModel dokumenterViewModel, Guid anlaegGuid)
        {
            var docId = dokumenterViewModel.Id;
            var dok = _dokumenterBusiness.Read(docId);
            if (dok != null)
            {
                var physicalPath = Path.Combine(dok.Sti, dok.Filnavn);
                if (System.IO.File.Exists(physicalPath))
                    System.IO.File.Delete(physicalPath);
                _dokumenterBusiness.Delete(dok);
            }
            // Return an empty string to signify success
            return Content("");
        }

        [AllowAnonymous]
        public FileStreamResult DownloadDokumentation(Guid modtageAnlaegId, string filnavn)
        {
            var person = GetPerson();
            if (person != null)
            {
                var fs = _dokumenterBusiness.ReadDokumentation(modtageAnlaegId, filnavn);
                if (fs != null && fs.Length > 0)
                {
                    var fileStreamResult = File(fs, "application/octet-stream", filnavn); //d.Filnavn
                    return fileStreamResult;
                }
            }
            return null;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ReadDokumentation([DataSourceRequest] DataSourceRequest request, Guid anlaegGuid)
        {
            var list = _dokumenterBusiness.GetList(anlaegGuid);
            var dd = list.Select(d => new DokumenterViewModel
              {
                  Id = d.Id,
                  FilNavn = d.Filnavn,
                  ModtageAnlaegId = (Guid)(d.ModtagerAnlaegId != null ? d.ModtagerAnlaegId : Guid.Empty),
                  FilSti = d.Sti
              });
            var result = dd.ToDataSourceResult(request);
            return Json(result);
        }

        private static void CreateFolderIfMissing(string path)
        {
            var folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
        }

        private static List<DokumenterViewModel> MapToDokumenterViewModel(ModtagerAnlaeg anlaeg)
        {
            var list = new List<DokumenterViewModel>();

            if (anlaeg != null)
            {
                foreach (var dokumenter in anlaeg.Dokumenter)
                {
                    var docViewModel = new DokumenterViewModel();
                    docViewModel.Id = dokumenter.Id;
                    docViewModel.FilNavn = dokumenter.Filnavn;
                    docViewModel.FilSti = dokumenter.Sti;
                    docViewModel.ModtageAnlaegId = anlaeg.Id;
                    list.Add(docViewModel);
                }
            }
            return list;
        }

        #endregion *** ModtageAnlæg Dokumenter ***

        #region *** GraenseVaerdier ***

        public ActionResult SearchForureningskomponent(string wildcard)
        {
            var fks = GetForureningskomponentModelList();
            var res = (from v in fks where v.DisplayName.ToLower().Contains(wildcard.ToLower()) orderby v.DisplayName select v).Take(20);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemoveGraenseVaerdi(ForureningskomponentModel forureningskomponentModel, Guid modtageAnlaegGuid)
        {
            if (ModelState.IsValid)
            {
                if (forureningskomponentModel != null)
                {
                    _modtagerAnlaegBusiness.RemoveGraenseVaerdi(modtageAnlaegGuid, forureningskomponentModel.Id);
                }
            }
            // Return an empty string to signify success
            ModelState.Clear();
            return Content("");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateGraenseVaerdi(Guid modtageAnlaegGuid, ForureningskomponentModel forureningskomponentModel)
        {
            if (ModelState.IsValid)
            {
                if (forureningskomponentModel != null)
                {
                    _modtagerAnlaegBusiness.UpdateGraenseVaerdi(modtageAnlaegGuid, forureningskomponentModel.Id, forureningskomponentModel.Max);
                }
            }
            // Return an empty string to signify success
            ModelState.Clear();
            return Content("");
        }

        [HttpPost]
        public ActionResult AddGraenseVaerdi(string DisplayName, decimal max, Guid modtageAnlaegGuid)
        {
            var fks = GetForureningskomponentModelList();
            var fk = (from f in fks where f.DisplayName == DisplayName select f).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (fk != null && modtageAnlaegGuid != Guid.Empty)
                {
                    var graensevaerdi = MapToGraenseVaerdi(fk, max, modtageAnlaegGuid);
                    _modtagerAnlaegBusiness.AddGraenseVaerdi(graensevaerdi);
                    fk.Max = max;
                    fk.Enhed = graensevaerdi.Enhed.Navn;
                }
            }
            return Json(fk);
        }
        private IEnumerable<ForureningskomponentModel> GetForureningskomponentModelList()
        {
            IEnumerable<ForureningskomponentModel> fkm;
            if (HttpContext.Application["Forureningskomponent"] == null)
            {
                var fk = _forureningskomponentBusiness.ReadAktiveForureningskomponenter();
                fkm = MapToForureningskomponentModel(fk);
                HttpContext.Application["Forureningskomponent"] = fkm;
            }
            else
            {
                fkm = (IEnumerable<ForureningskomponentModel>)HttpContext.Application["Forureningskomponent"];
            }
            return fkm;
        }

        public static List<GraenseVaerdierViewModel> MapToGraenseVaerdiList(ModtagerAnlaeg ma)
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

        public Graensevaerdier MapToGraenseVaerdi(ForureningskomponentModel viewModel, decimal max, Guid modtageAnlaegGuid)
        {
            var grVaerdi = new Graensevaerdier();
            var enhedList = _kodelisteBusiness.ReadAktiveEnheder();
            var enhed = enhedList.FirstOrDefault();
            grVaerdi.Enhed = enhed;

            grVaerdi.ForureningskomponenterId = viewModel.Id;
            grVaerdi.JordanlaegId = modtageAnlaegGuid;
            grVaerdi.Max = max;

            return grVaerdi;
        }

        public static IEnumerable<ForureningskomponentModel> MapToForureningskomponentModel(IList<Forureningskomponent> fk)
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

        #endregion *** GraenseVaerdier ***

        #endregion *** Jordmodtager ***

        #region *** Fakturering ***

        public ActionResult AdminFakturering()
        {
            var model = new FaktureringViewModel();
            var person = GetPerson();
            var kommuneId = GetKommuneId(person.Id);
            model.Anmeldelser = new List<Anmeldelse>(_anmeldelseBusiness.GetKommunesFakturerbareAnmeldelser(kommuneId));
            return View("AdminFakturering", model);
        }

        //public ActionResult AdminFakturering()
        //{
        //    var model = new FaktureringViewModel();
        //    var person = GetPerson();
        //    var kommuneId = GetKommuneId(person.Id);
        //    model.Anmeldelser = new List<Anmeldelse>(_anmeldelseBusiness.GetKommunesFakturerbareAnmeldelser(kommuneId));
        //    return View("AdminFakturering", model);
        //}

        #endregion

        #region *** Private methods ***

        /// <summary>
        /// Hent den påloggede brugers persondata
        /// </summary>
        private Person GetPerson()
        {
            var b = _brugereBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
            var person = b.Person;
            return person;
        }

        /// <summary>
        /// Hent den påloggede brugers kommuneid
        /// </summary>
        private Guid GetKommuneId(Guid userId)
        {
            var id = _kommuneBusiness.GetKommuneIdByUserId(userId);
            return id;
        }

        #endregion *** Private methods ***

    }
}

