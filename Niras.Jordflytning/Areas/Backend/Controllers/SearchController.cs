using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using Niras.Jordflytning.Areas.Backend.ViewModels;
using Niras.Jordflytning.Core;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.SoegeResultat;
using Niras.Jordflytning.Library.Helpers;
using Niras.Jordflytning.Library.Logging;


namespace Niras.Jordflytning.Areas.Backend.Controllers
{
    public class SearchController : Controller
    {
        #region *** Variables ***

        private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Areas.Backend.Controllers.SearchController");
        private readonly IKodelisteBusiness _kodelisteBusiness;
        private readonly ISecurityProvider _securityProvider;
        private readonly IBrugereBusiness _brugereBusiness;
        private readonly ISoegBusiness _soegBusiness;
        private readonly ISoegAdminBusiness _soegAdminBusiness;
        private readonly IKommuneBusiness _kommuneBusiness;
        private readonly IVognlaesBusiness _vognlaesBusiness;
        private readonly IModtagerAnlaegBusiness _modtagerAnlaegBusiness;
        private readonly ITransportoerBusiness _transportoerBusiness;
        private readonly IAnmelderBusiness _anmelderBusiness;
        private readonly IAndenOprindJordTypeRepository _andenOprindJordTypeRepository;
        private readonly IJordKlassifikationTypeRepository _jordKlassifikationTypeRepository;

        #endregion *** Variables ***

        #region *** Constructor ***

        public SearchController(
          ISecurityProvider securityProvider,
          IKodelisteBusiness kodelisteBusiness,
          IBrugereBusiness brugereBusiness,
          ISoegBusiness soegBusiness,
          ISoegAdminBusiness soegAdminBusiness,
          IKommuneBusiness kommuneBusiness,
          IVognlaesBusiness vognlaesBusiness,
            IModtagerAnlaegBusiness modtagerAnlaegBusiness,
            ITransportoerBusiness transportoerBusiness,
            IAnmelderBusiness anmelderBusiness,
            IAndenOprindJordTypeRepository andenOprindJordTypeRepository,
            IJordKlassifikationTypeRepository jordKlassifikationTypeRepository
          )
        {
            _kodelisteBusiness = kodelisteBusiness;
            _securityProvider = securityProvider;
            _brugereBusiness = brugereBusiness;
            _soegBusiness = soegBusiness;
            _soegAdminBusiness = soegAdminBusiness;
            _kommuneBusiness = kommuneBusiness;
            _vognlaesBusiness = vognlaesBusiness;
            _modtagerAnlaegBusiness = modtagerAnlaegBusiness;
            _transportoerBusiness = transportoerBusiness;
            _anmelderBusiness = anmelderBusiness;
            _andenOprindJordTypeRepository = andenOprindJordTypeRepository;
            _jordKlassifikationTypeRepository = jordKlassifikationTypeRepository;
        }

        #endregion *** Constructor ***

        #region *** Search ***

        public ActionResult AdminSearch(int soegetype)
        {
            var model = new AdminSoegModel();

            if (_securityProvider.CurrentUser.IsInRole(ApplicationConstants.MiljoemedarbejderRolle))
            {
                model.ShowAnmeldFane = true;
                model.ShowStikproeveFane = true;
                model.ShowVognlaesFane = true;
            }
            if (_securityProvider.CurrentUser.IsInRole(ApplicationConstants.SagsbehandlerRolle))
            {
                model.ShowAnmeldFane = true;
            }
            if (_securityProvider.CurrentUser.IsInRole(ApplicationConstants.LaboratorieRolle))
            {
                model.ShowStikproeveFane = true;
            }
            if (_securityProvider.CurrentUser.IsInRole(ApplicationConstants.ProevetagerRolle))
            {
                model.ShowStikproeveFane = true;
                model.ShowVognlaesFane = true;
            }
            if (_securityProvider.CurrentUser.IsInRole(ApplicationConstants.PladsmandRolle))
            {
                model.ShowAnmeldFane = true;
                model.ShowStikproeveFane = true;
                model.ShowVognlaesFane = true;
            }
            if (_securityProvider.CurrentUser.IsInRole(ApplicationConstants.BogholderRolle))
            {
                model.ShowAnmeldFane = true;
                model.ShowBetalerFane = true;
                model.ShowVognlaesFane = true;
            }
            if (_securityProvider.CurrentUser.IsInRole(ApplicationConstants.MasterAdminRolle))
            {
                model.ShowBetalerFane = true;
                model.ShowStikproeveFane = true;
                model.ShowVognlaesFane = true;
                model.ShowAnmeldFane = true;
            }
            if (_securityProvider.CurrentUser.IsInRole(ApplicationConstants.JordmodtagerAdminRolle))
            {
                // dont show
            }
            if (_securityProvider.CurrentUser.IsInRole(ApplicationConstants.KommuneAdminRolle))
            {
                // dont show
            }

            if (model.ShowStikproeveFane)
            {
                // Stikprøve statustyper
                var statusList = _kodelisteBusiness.ReadAktiveStikproeveStatusTyper();
                model.AddStikProeveStatuser(statusList);
            }
            model.VognlaesReturnType = EnumVognlaesReturnType.Normal;
            model.PopulateSoegeRapporter();

            model.ModtagerAnlaegList = _modtagerAnlaegBusiness.ReadModtagerAnlaegBypassEf().Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Navn }).OrderBy(m => m.Text.Trim());
            model.TransportoerList = _transportoerBusiness.ReadTransportoererBypassEf().Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Person.Firmaoplysninger.Firmanavn }).OrderBy(t => t.Text.Trim());
            model.AnmelderList = _anmelderBusiness.ReadAnmeldereBypassEf().Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Person.Navn + " " + a.Person.Efternavn }).OrderBy(a => a.Text.Trim());
            model.MaterialeList = _andenOprindJordTypeRepository.Read().Select(a => new SelectListItem {Value = a.Id.ToString(), Text = a.Navn}).OrderBy(a => a.Text.Trim());
            model.ForureningskategoriList = _jordKlassifikationTypeRepository.Read().Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Navn }).OrderBy(a => a.Text.Trim());

            ModelState.Clear();
            return View(model);
        }

        public ActionResult AdminResultatAnmeld(int soegType = 0, string foerDate = "", string efterDate = "", int loebeNr = 0, string adresse = "", bool inclafsluttede = false, string modtagerId = "", string transportoerId = "", string anmelderId = "", string andenOprindJordTypeId = "", string andenOprindBeskrivelse = "", string forureningsKategori = "")
        {
            var model = new AdminSoegModel();
            model.ShowAnmeldFane = true;
            model.PopulateSoegeRapporter();

            DateTime? dtFoerDate = null;
            if (!String.IsNullOrEmpty(foerDate))
                dtFoerDate = DateTime.ParseExact(foerDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            DateTime? dtEfterDate = null;
            if (!String.IsNullOrEmpty(efterDate))
                dtEfterDate = DateTime.ParseExact(efterDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            model = new AdminSoegModel
            {
                SelectedSoegeType = soegType.ToString(),
                FoerDato = dtFoerDate,
                EfterDato = dtEfterDate,
                Adresse = adresse,
                LoebeNr = loebeNr,
                InclAfsluttede = inclafsluttede,
                ModtagerId = modtagerId,
                TransportoerId = transportoerId,
                AnmelderId = anmelderId,
                AndenOprindJordTypeId = andenOprindJordTypeId,
                AndenOprindBeskrivelse = andenOprindBeskrivelse,
                Forureningskategori = forureningsKategori
            };

            //var person = GetPerson();
            //var id = person.Id;
            //var isKommune = IsUserKommune(person);
            //if (isKommune)
            //  id = GetKommuneId(person.Id);

            //if (soegType != 0)
            //{
            //switch (soegType)
            //{
            //  case (int)SoegeTypeDefaultAdmin.AnmeldelserIgangvaerendeSager:
            //    model.SoegeResultatList = _soegBusiness.GetIkkeAfsluttedeAnmeldelser(id, isKommune);
            //    model.SetSelectedRapport(SoegeTypeDefaultAdmin.AnmeldelserIgangvaerendeSager);

            //    break;
            //  case (int)SoegeTypeDefaultAdmin.AnmeldelserSagerIkkeMeldtAfsluttet:
            //    model.SoegeResultatList = _soegBusiness.GetIkkeMeldtAfsluttetAnmeldelser(id, isKommune);
            //    model.SetSelectedRapport(SoegeTypeDefaultAdmin.AnmeldelserSagerIkkeMeldtAfsluttet);
            //    break;
            //}
            //}
            //else
            //{
            //DateTime? dtFoerDate = null;
            //if (!String.IsNullOrEmpty(foerDate))
            //  dtFoerDate = DateTime.ParseExact(foerDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            //DateTime? dtEfterDate = null;
            //if (!String.IsNullOrEmpty(efterDate))
            //  dtEfterDate = DateTime.ParseExact(efterDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            //model = new AdminSoegModel
            //  {
            //    FoerDato = dtFoerDate,
            //    EfterDato = dtEfterDate,
            //    Adresse = adresse,
            //    LoebeNr = loebeNr,
            //    InclAfsluttede = inclafsluttede
            //  };


            //  var isMiljoeMedarbejder = false;
            //  var isSagsbehandler = false;
            //  if (_securityProvider.CurrentUser.IsInRole(ApplicationConstants.MiljoemedarbejderRolle))
            //    isMiljoeMedarbejder = true;
            //  if (_securityProvider.CurrentUser.IsInRole(ApplicationConstants.SagsbehandlerRolle))
            //    isSagsbehandler = true;

            //  if (isMiljoeMedarbejder && isSagsbehandler)
            //  {
            //    var sagbehandlList = _soegBusiness.GetAnmeldelserBy(model.LoebeNr, model.Adresse, model.FoerDato, model.EfterDato, id, isKommune, inclafsluttede);
            //    var miljoeMedarbList = _soegBusiness.GetAnmeldelserBy(model.LoebeNr, model.Adresse, model.FoerDato, model.EfterDato, person.Id, false, inclafsluttede);

            //    foreach (var soegeResultat in miljoeMedarbList)
            //    {
            //      var resultat = soegeResultat;
            //      var sagsbehandlerRes = (from res in sagbehandlList
            //                              where res.LoebeNr == resultat.LoebeNr
            //                              select res).FirstOrDefault();
            //      if (sagsbehandlerRes == null)
            //        sagbehandlList.Add(resultat);
            //    }
            //    model.SoegeResultatList = sagbehandlList.ToList();
            //  }
            //  else
            //  {
            //    model.SoegeResultatList = _soegBusiness.GetAnmeldelserBy(model.LoebeNr, model.Adresse, model.FoerDato, model.EfterDato, id, isKommune, inclafsluttede);
            //  }
            //}
            return View(model);
        }


        [HttpPost]
        public ActionResult AdminResultatAnmeldAjax([DataSourceRequest] DataSourceRequest request, int soegType = 0, DateTime? foerDate = null, DateTime? efterDate = null, int loebeNr = 0, string adresse = "", bool inclafsluttede = false, string modtagerId = "", string transportoerId = "", string anmelderId = "", string andenOprindJordTypeId = "", string andenOprindBeskrivelse = "", string forureningsKategori = "")
        {
            try
            {

                var model = new AdminSoegModel();
                model.ShowAnmeldFane = true;
                model.PopulateSoegeRapporter();

                var person = GetPerson();
                var id = person.Id;
                var isKommune = IsUserKommune(person);
                if (isKommune)
                    id = GetKommuneId(person.Id);

                if (soegType != 0)
                {
                    switch (soegType)
                    {
                        case (int)SoegeTypeDefaultAdmin.AnmeldelserIgangvaerendeSager:
                            model.SoegeResultatList = _soegBusiness.GetIkkeAfsluttedeAnmeldelser(id, isKommune);
                            model.SetSelectedRapport(SoegeTypeDefaultAdmin.AnmeldelserIgangvaerendeSager);

                            break;
                        case (int)SoegeTypeDefaultAdmin.AnmeldelserSagerIkkeMeldtAfsluttet:
                            model.SoegeResultatList = _soegBusiness.GetIkkeMeldtAfsluttetAnmeldelser(id, isKommune);
                            model.SetSelectedRapport(SoegeTypeDefaultAdmin.AnmeldelserSagerIkkeMeldtAfsluttet);
                            break;
                    }
                }
                else
                {

                    var emptyGuid = Guid.Empty;

                    model = new AdminSoegModel
                    {
                        FoerDato = foerDate,
                        EfterDato = efterDate,
                        Adresse = adresse,
                        LoebeNr = loebeNr,
                        InclAfsluttede = inclafsluttede,
                        ModtagerId = modtagerId,
                        TransportoerId = transportoerId,
                        AnmelderId = anmelderId,
                        AndenOprindJordTypeId = andenOprindJordTypeId,
                        AndenOprindBeskrivelse = andenOprindBeskrivelse,
                        Forureningskategori = forureningsKategori
                    };


                    var isMiljoeMedarbejder = false;
                    var isSagsbehandler = false;
                    if (_securityProvider.CurrentUser.IsInRole(ApplicationConstants.MiljoemedarbejderRolle))
                        isMiljoeMedarbejder = true;
                    if (_securityProvider.CurrentUser.IsInRole(ApplicationConstants.SagsbehandlerRolle))
                        isSagsbehandler = true;

                    var guidModtagerId = Guid.Empty;
                    Guid.TryParse(model.ModtagerId, out guidModtagerId);

                    var guidTransportoerId = Guid.Empty;
                    Guid.TryParse(model.TransportoerId, out guidTransportoerId);

                    var guidAnmelderId = Guid.Empty;
                    Guid.TryParse(model.AnmelderId, out guidAnmelderId);

                    var guidAndenOprindJordTypeId = Guid.Empty;
                    Guid.TryParse(model.AndenOprindJordTypeId, out guidAndenOprindJordTypeId);

                    if (isMiljoeMedarbejder && isSagsbehandler)
                    {
                        var countStart1 = Environment.TickCount;
                        var sagbehandlList = _soegBusiness.GetAnmeldelserBy(model.LoebeNr, model.Adresse, model.FoerDato, model.EfterDato, id, isKommune, inclafsluttede, guidModtagerId, guidTransportoerId, guidAnmelderId, guidAndenOprindJordTypeId, model.AndenOprindBeskrivelse, model.Forureningskategori);
                        var endCount1 = Environment.TickCount - countStart1;

                        var countStart2 = Environment.TickCount;
                        var miljoeMedarbList = _soegBusiness.GetAnmeldelserBy(model.LoebeNr, model.Adresse, model.FoerDato, model.EfterDato, person.Id, false, inclafsluttede, guidModtagerId, guidTransportoerId, guidAnmelderId, guidAndenOprindJordTypeId, model.AndenOprindBeskrivelse, model.Forureningskategori);
                        var endCount2 = Environment.TickCount - countStart2;

                        foreach (var soegeResultat in miljoeMedarbList)
                        {
                            var resultat = soegeResultat;
                            var sagsbehandlerRes = (from res in sagbehandlList
                                                    where res.LoebeNr == resultat.LoebeNr
                                                    select res).FirstOrDefault();
                            if (sagsbehandlerRes == null)
                                sagbehandlList.Add(resultat);
                        }
                        model.SoegeResultatList = sagbehandlList.ToList();
                    }
                    else
                    {
                        model.SoegeResultatList = _soegBusiness.GetAnmeldelserBy(model.LoebeNr, model.Adresse, model.FoerDato, model.EfterDato, id, isKommune, inclafsluttede, guidModtagerId, guidTransportoerId, guidAnmelderId, guidAndenOprindJordTypeId, model.AndenOprindBeskrivelse, model.Forureningskategori);
                    }
                }

                var serializer = new JavaScriptSerializer();
                var result = new ContentResult();
                serializer.MaxJsonLength = Int32.MaxValue; // Whatever max length you want here
                result.Content = serializer.Serialize(model.SoegeResultatList.ToDataSourceResult(request));
                result.ContentType = "application/json";
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return Json(null);
        }

        public ActionResult AdminResultatVognlaes1(string foerDate = "", string efterDate = "", bool udtraekTilOkonimiSystem = false)
        {
            DateTime? dtFoerDate = null;
            if (!String.IsNullOrEmpty(foerDate))
                dtFoerDate = DateTime.ParseExact(foerDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            DateTime? dtEfterDate = null;
            if (!String.IsNullOrEmpty(efterDate))
                dtEfterDate = DateTime.ParseExact(efterDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var returType = EnumVognlaesReturnType.Normal;

            if (udtraekTilOkonimiSystem)
                returType = EnumVognlaesReturnType.UdtraekTilOkonimiSystem;

            var model = new AdminSoegModel
              {
                  VognlaesFoerDato = dtFoerDate,
                  VognlaesEfterDato = dtEfterDate,
                  VognlaesReturnType = returType
              };

            //var person = GetPerson();

            //model.SoegeResultatVognlaesList = new List<SoegeResultatVognlaes1>();
            //model.SoegeResultatVognlaesList = _soegAdminBusiness.GetVognlaesBy(model.VognlaesFoerDato, model.VognlaesEfterDato, returType, person.Id);

            return View(model);
        }




        [HttpPost]
        public ActionResult AdminResultatVognlaes1Ajax([DataSourceRequest] DataSourceRequest request, DateTime? foerDate = null, DateTime? efterDate = null, string udtraekTilOkonimiSystem = "")
        {
            //DateTime? dtFoerDate = null;
            //if (!String.IsNullOrEmpty(foerDate))
            //  dtFoerDate = DateTime.ParseExact(foerDate, "dd-MM-yyyy hh:mi:ss", CultureInfo.InvariantCulture);

            //DateTime? dtEfterDate = null;
            //if (!String.IsNullOrEmpty(efterDate))
            //  dtEfterDate = DateTime.ParseExact(efterDate, "dd-MM-yyyy hh:mi:ss", CultureInfo.InvariantCulture);



            var returType = EnumVognlaesReturnType.Normal;

            if (!string.IsNullOrEmpty(udtraekTilOkonimiSystem) && udtraekTilOkonimiSystem == "UdtraekTilOkonimiSystem")
                returType = EnumVognlaesReturnType.UdtraekTilOkonimiSystem;

            try
            {
                var person = GetPerson();

                var vognlaesList = new List<SoegeResultatVognlaes1>();
                vognlaesList = _soegAdminBusiness.GetVognlaesBy(foerDate, efterDate, returType, person.Id);

                var serializer = new JavaScriptSerializer();
                var result = new ContentResult();
                serializer.MaxJsonLength = Int32.MaxValue; // Whatever max length you want here
                result.Content = serializer.Serialize(vognlaesList.ToDataSourceResult(request));
                result.ContentType = "application/json";
                return result;

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return Json(null);
        }

        public ActionResult AdminResultatVognlaes2(string foerDate = "", string efterDate = "")
        {
            DateTime? dtFoerDate = null;
            if (!String.IsNullOrEmpty(foerDate))
                dtFoerDate = DateTime.ParseExact(foerDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            DateTime? dtEfterDate = null;
            if (!String.IsNullOrEmpty(efterDate))
                dtEfterDate = DateTime.ParseExact(efterDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var model = new AdminSoegModel
              {
                  VognlaesFoerDato = dtFoerDate,
                  VognlaesEfterDato = dtEfterDate,
                  VognlaesReturnType = EnumVognlaesReturnType.UdtraekTilMaegnder
              };
            //var person = GetPerson();

            //model.SoegeResultatVognlaes2List = new List<SoegeResultatVognlaes2>();
            //model.SoegeResultatVognlaes2List = _soegAdminBusiness.GetVognlaesBy(model.VognlaesFoerDato, model.VognlaesEfterDato, person.Id);

            return View(model);
        }

        [HttpPost]
        public ActionResult AdminResultatVognlaes2Ajax([DataSourceRequest] DataSourceRequest request, DateTime? foerDate = null, DateTime? efterDate = null)
        {
            try
            {
                var person = GetPerson();

                var vognlaesList = new List<SoegeResultatVognlaes2>();
                vognlaesList = _soegAdminBusiness.GetVognlaesBy(foerDate, efterDate, person.Id);

                var serializer = new JavaScriptSerializer();
                var result = new ContentResult();
                serializer.MaxJsonLength = Int32.MaxValue; // Whatever max length you want here
                result.Content = serializer.Serialize(vognlaesList.ToDataSourceResult(request));
                result.ContentType = "application/json";
                return result;

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return Json(null);
        }

        public ActionResult AdminResultatStikproeve(int soegType = 0, string foerDate = "", string efterDate = "", string status = "", int loebenummer = 0)
        {
            var model = new AdminSoegModel();
            model.ShowStikproeveFane = true;
            model.PopulateSoegeRapporter();
            var person = GetPerson();
            if (soegType != 0)
            {
                switch (soegType)
                {
                    case (int)SoegeTypeDefaultAdmin.StikproeverDerErAktive:
                        model.SoegeResultatStikProeveList = _soegAdminBusiness.GetStikproeverDerErAktive(person.Id);
                        model.SetSelectedRapport(SoegeTypeDefaultAdmin.StikproeverDerErAktive);
                        break;
                }
            }
            else
            {
                DateTime? dtFoerDate = null;
                if (!String.IsNullOrEmpty(foerDate))
                    dtFoerDate = DateTime.ParseExact(foerDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                DateTime? dtEfterDate = null;
                if (!String.IsNullOrEmpty(efterDate))
                    dtEfterDate = DateTime.ParseExact(efterDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                var statusGuid = Guid.Empty;
                if (!String.IsNullOrEmpty(status) && status != "0")
                    statusGuid = new Guid(status);

                var statusType = _kodelisteBusiness.ReadStikproeveStatusType(statusGuid);
                var statusText = "";

                if (statusType != null)
                    statusText = statusType.Navn;

                model = new AdminSoegModel
                  {
                      StikproeveFoerDato = dtFoerDate,
                      StikproeveEfterDato = dtEfterDate,
                      StikproeveId = loebenummer,
                      StikproeveStatus = statusGuid,
                      StikproeveStatusText = statusText
                  };


                model.SoegeResultatStikProeveList = new List<SoegeResultatStikproeve>();
                model.SoegeResultatStikProeveList = _soegAdminBusiness.GetStikproeverBy(model.StikproeveFoerDato, model.StikproeveEfterDato, model.StikproeveId,
                  model.StikproeveStatus, person.Id);
            }
            return View(model);
        }

        public ActionResult AdminResultatBetaler(int soegType = 0, string firma = "", string navn = "", int status = 0, int kernekunde = 0)
        {
            var model = new AdminSoegModel();
            var person = GetPerson();
            if (soegType != 0)
            {
            }
            else
            {
                model = new AdminSoegModel
                  {
                      BetalerFirma = firma,
                      BetalerNavn = navn,
                      BetalerStatus = status,
                      BetalerKerneKunde = kernekunde
                  };

                model.SoegeResultatBetalerList = new List<SoegeResultatBetaler>();
                model.SoegeResultatBetalerList = _soegAdminBusiness.GetBetalereBy(model.BetalerFirma, model.BetalerNavn, model.BetalerStatus, model.BetalerKerneKunde,
                                                                                  person.Id);
            }
            return View(model);
        }

        #endregion *** Search ***

        #region *** Afvis ***



        //public ActionResult EditingCustom_Update([DataSourceRequest] DataSourceRequest request,
        //		[Bind(Prefix = "models")]IEnumerable<ProductViewModel> products)
        //{
        //	if (products != null && ModelState.IsValid)
        //	{
        //		foreach (var product in products)
        //		{
        //			productService.Update(product);
        //		}
        //	}

        //	return Json(products.ToDataSourceResult(request, ModelState));
        //}
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AfvisVognlaes([DataSourceRequest] DataSourceRequest request, SoegeResultatVognlaes1 model)
        {
            var res = _vognlaesBusiness.AfvisVognlaes(model.VognlaesId, model.AfvistNote);
            return Json(res);
        }

        #endregion *** Afvis ***

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

        private bool IsUserKommune(Person person)
        {
            var isKommune = false;
            var roleList = _brugereBusiness.GetUserRole(person.BrugerId);
            foreach (var role in roleList)
            {
                if (role == ApplicationConstants.SagsbehandlerRolle)
                {
                    isKommune = true;
                    break;
                }
            }
            return isKommune;
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

        #region *** Export to Csv ***

        public JsonResult ExportToCsv(string data, string title)
        {
            List<SoegeResultatVognlaes1> vognlaesList = null;
            JsonResult jsonResult;
            try
            {
                var dataObject = JsonConvert.DeserializeObject<List<SoegeResultatVognlaes1>>(data);
                vognlaesList = dataObject;
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
            }

            var vognlaesGuidList = new List<Guid>();
            if (vognlaesList != null)
            {
                foreach (var vl in vognlaesList)
                {
                    if (vl.Afvist != true)
                    {
                        vognlaesGuidList.Add(vl.VognlaesId);
                    }
                }
            }

            byte[] file;

            var list = new List<SoegeResultatFakturaExport>();
            if (vognlaesList != null)
            {
                list = _soegAdminBusiness.GetVognlaesFaktura(vognlaesGuidList);
            }
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
                {
                    var tempSoe = new SoegeResultatFakturaExport();
                    tempSoe.SeralizeColnamesAsCsv(writer);
                    foreach (var soegeResultatFakturaExport in list)
                    {
                        soegeResultatFakturaExport.SerializeAsCsv(writer);
                    }
                    writer.Flush();
                    file = memoryStream.ToArray();
                }
            }
            Session[title] = file;
            if (vognlaesList != null)
            {
                jsonResult = Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                jsonResult = Json(new { success = false }, JsonRequestBehavior.DenyGet);
            }
            return jsonResult;
        }

        public FileResult GetCsvFile(string title)
        {
            // Is there a Csv stored in session?
            if (Session[title] != null)
            {
                // Get the csv from seession.
                var file = Session[title] as byte[];
                var filename = string.Format("{0}.txt", title);

                // Remove the csv from session.
                Session.Remove(title);

                // Return the csv.
                //Response.Buffer = true;
                //Response.StatusCode = (int)HttpStatusCode.OK;
                //Response.AddHeader("Content-Disposition", string.Format("attachment; filename=${0}", filename));

                return File(file, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
                //return File(file, "text/plain", filename);
            }
            throw new Exception(string.Format("{0} not found", title));
        }

        #endregion *** Export to Csv ***

        #region *** Export to Excel ***

        public JsonResult ExportToExcel(string model, string data, string title)
        {

            var file = ExcelHelper.CreateExcelFile(model, data, title);
            Session[title] = file;
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }


        public FileResult GetExcelFile(string title)
        {
            // Is there a spreadsheet stored in session?
            if (Session[title] != null)
            {
                // Get the spreadsheet from seession.
                var file = Session[title] as byte[];
                var filename = string.Format("{0}.xlsx", title);

                // Remove the spreadsheet from session.
                Session.Remove(title);

                // Return the spreadsheet.
                //Response.Buffer = true;
                //Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", filename));
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
            }
            throw new Exception(string.Format("{0} not found", title));
        }

        #endregion *** Export to Excel ***


    }
}
