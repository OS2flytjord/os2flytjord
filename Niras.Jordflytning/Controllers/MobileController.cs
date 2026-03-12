using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.WebPages;
using Kendo.Mvc.Extensions;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.JordForurening;
using Niras.Jordflytning.Library.Logging;
using Niras.Jordflytning.ViewModels.Anmeldelse;
using Niras.Jordflytning.ViewModels.Mobile;

namespace Niras.Jordflytning.Controllers
{
  [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
  public class MobileController : Controller
  {
    private static readonly ILogger logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Controllers.MobileController");
    private readonly IBrugereBusiness _brugereBusiness;
    private readonly ISecurityProvider _securityProvider;
    private readonly IAnmeldelserBusiness _anmeldelserBusiness;
    private readonly ITransportoerBusiness _transportoerBusiness;
    private readonly IModtagerAnlaegBusiness _modtagerAnlaegBusiness;
    private readonly IKodelisteBusiness _kodelisteBusiness;
    private readonly IPersonBusiness _personBusiness;
    private readonly IMatrikelBusiness _matrikelBusiness;
    private readonly IForureningsOpslagBusiness _forureningsOpslagBusiness;
    private readonly IStatusBetalerBusiness _statusBetalerBusiness;
    private readonly IStatusAnmeldelseBusiness _statusAnmeldelseBusiness;
    private readonly IJordKlassifikationTypeRepository _jordKlassifikationTypeRepository;
    private readonly IAnmelderBusiness _anmelderBusiness;
    private readonly IAdviseringBusiness _adviseringBusiness;
    private readonly IKommuneBusiness _kommuneBusiness;
    private readonly ISoegBusiness _soegBusiness;

    public MobileController(IBrugereBusiness brugereBusiness, ISecurityProvider securityProvider, IAnmeldelserBusiness anmeldelserBusiness, ITransportoerBusiness transportoerBusiness, IModtagerAnlaegBusiness modtagerAnlaegBusiness, IKodelisteBusiness kodelisteBusiness, IPersonBusiness personBusiness, IMatrikelBusiness matrikelBusiness, IForureningsOpslagBusiness forureningsOpslagBusiness, IStatusBetalerBusiness statusBetalerBusiness, IStatusAnmeldelseBusiness statusAnmeldelseBusiness, IJordKlassifikationTypeRepository jordKlassifikationTypeRepository, IAnmelderBusiness anmelderBusiness, IAdviseringBusiness adviseringBusiness, IKommuneBusiness kommuneBusiness, ISoegBusiness soegBusiness)
    {
      _brugereBusiness = brugereBusiness;
      _securityProvider = securityProvider;
      _anmeldelserBusiness = anmeldelserBusiness;
      _modtagerAnlaegBusiness = modtagerAnlaegBusiness;
      _transportoerBusiness = transportoerBusiness;
      _kodelisteBusiness = kodelisteBusiness;
      _personBusiness = personBusiness;
      _matrikelBusiness = matrikelBusiness;
      _forureningsOpslagBusiness = forureningsOpslagBusiness;
      _statusBetalerBusiness = statusBetalerBusiness;
      _statusAnmeldelseBusiness = statusAnmeldelseBusiness;
      _jordKlassifikationTypeRepository = jordKlassifikationTypeRepository;
      _anmelderBusiness = anmelderBusiness;
      _adviseringBusiness = adviseringBusiness;
      _kommuneBusiness = kommuneBusiness;
      _soegBusiness = soegBusiness;
    }

    public ActionResult MobileAdgang(Guid anmeldelseId)
    {
      var anmeldelse = _anmeldelserBusiness.Read(anmeldelseId);
      MobileAdgangModel model = new MobileAdgangModel();
      model.Adresse = anmeldelse.Oprindelsessted.Adresse;
      model.ModtagerAnlaegNavn = anmeldelse.ModtagerAnlaeg.Navn;
      model.Loebenummer = "1" + anmeldelse.Nummer.ToString();

      return View("_MobileAdgang", model);
    }

    public ActionResult Kvittering(Guid anmeldelseId)
    {
      var anmeldelse = _anmeldelserBusiness.Read(anmeldelseId);
      KvitteringModel model = new KvitteringModel();
      model.Kommune = anmeldelse.Kommune.Navn;
      model.Email = anmeldelse.Anmelder.Person.Email;

      return View("_Kvittering", model);
    }


    public ActionResult PreviewAnmeldelse(Guid anmeldelseId)
    {
      var anmeldelse = _anmeldelserBusiness.Read(anmeldelseId);
      MobilePreviewAnmeldelseModel model = new MobilePreviewAnmeldelseModel();
      model.Anmelder = anmeldelse.Anmelder.Person.Navn + " " + anmeldelse.Anmelder.Person.Efternavn + ", " + anmeldelse.Anmelder.Person.Adresse + " " + anmeldelse.Anmelder.Person.Postnummer + " " + anmeldelse.Kommune.Navn + ", Tlfnr. " + anmeldelse.Anmelder.Person.Telefon + ", Email " + anmeldelse.Anmelder.Person.Email;

      var currentBruger = _brugereBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
      model.LockAnmeldelse = anmeldelse.ModtagerAnlaeg != null && anmeldelse.ModtagerAnlaeg.Jordmodtager != null && currentBruger.Person.Firmaoplysninger != null && (anmeldelse.Anmelder.Person.Firmaoplysninger == null || anmeldelse.Anmelder.Person.Firmaoplysninger.CVR != currentBruger.Person.Firmaoplysninger.CVR);


      if (anmeldelse.Betaler != null && anmeldelse.Betaler.Person != null)
        model.Betaler = anmeldelse.Betaler.Person.Navn + " " + anmeldelse.Betaler.Person.Efternavn + ", " + anmeldelse.Betaler.Person.Adresse + " " + anmeldelse.Betaler.Person.Postnummer + " " + anmeldelse.Kommune.Navn + ", Tlfnr. " + anmeldelse.Betaler.Person.Telefon + ", Email " + anmeldelse.Betaler.Person.Email;

      if (anmeldelse.Jord.JordKlassifikationType != null && anmeldelse.Jord.JordKlassifikationType.Navn != null)
      {
        model.Jorden = anmeldelse.Jord.JordKlassifikationType.Navn;
      }
      if (anmeldelse.ModtagerAnlaeg != null && anmeldelse.ModtagerAnlaeg.Navn != null)
      {
        model.Modtager = anmeldelse.ModtagerAnlaeg.Navn;
      }
      if (anmeldelse.Oprindelsessted != null && anmeldelse.Oprindelsessted.Adresse != null)
      {
        model.Sted = anmeldelse.Oprindelsessted.Adresse;
      }
      if (anmeldelse.Transportoer != null && anmeldelse.Transportoer.Person != null && anmeldelse.Transportoer.Person.Firmaoplysninger != null && anmeldelse.Transportoer.Person.Firmaoplysninger.Firmanavn != null)
      {
        model.Transportoer = anmeldelse.Transportoer.Person.Firmaoplysninger.Firmanavn;
      }

      return View("_MobilePreviewAnmeldelse", model);
    }

    public ActionResult _MobileAnmeldelse(Guid? anmeldelseId, string kommunenavn)
    {
      MobileAnmeldelseModel model = new MobileAnmeldelseModel();
      

      model.Transportoere = _transportoerBusiness.Search(x => x.Aktiv && x.Person.Firmaoplysninger != null && x.Person.Firmaoplysninger.Firmanavn != null && x.Person.Firmaoplysninger.Firmanavn != "").Select(y => new TransportoerModel { Firma = y.Person.Firmaoplysninger.Firmanavn, Id = y.Id }).OrderBy(x => x.Firma).ToList();

      //		model.ModtagerAnlaeg = _modtagerAnlaegBusiness.ReadAktiveModtagerAnlaeg().OrderBy(x => x.Navn).Select(x => new ModtagerAnlaegModel { Navn = x.Navn, Id = x.Id }).ToList();
      if (anmeldelseId == null || anmeldelseId == Guid.Empty)
      {


          


        var jks = _kodelisteBusiness.ReadAktiveJordKlassifikationTypesForKommune(kommunenavn);
        model.JordklassifikationTypeListe = from o in jks select new SelectListItem { Selected = false, Text = o.Navn, Value = o.Id.ToString() };

        model.Anmeldelse = new Anmeldelse();
        model.kanIndsende = true;
        model.Anmeldelse.Jord = new Jord();



        if ((model.Anmeldelse.Jordforureningsopslag == null || model.Anmeldelse.Jordforureningsopslag.Id == Guid.Empty) && Session["Jordforureningsopslag"] != null)
        {
          var opslag = (Jordforureningsopslag)Session["Jordforureningsopslag"];
          model.Anmeldelse.Jordforureningsopslag = opslag;
          model.Anmeldelse.Jordforureningsopslag.JordKlassifikationType = _jordKlassifikationTypeRepository.Read(opslag.JordKlassifikationType.Id);
        }
        return View(model);
      }
      var anmeldelse = _anmeldelserBusiness.Read(anmeldelseId.Value);
      model.SelectedModtagerAnlaegId = anmeldelse.ModtagerAnlaegId.ToString();
      model.SelectedTransportoerId = anmeldelse.TransportoerId.ToString();
      if (anmeldelseId != null && anmeldelse.ModtagerAnlaeg != null)
      {
        model.AnvenderFJ = anmeldelse.ModtagerAnlaeg.AnvenderJF;
      }
      var currentBruger = _brugereBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
      model.LockAnmeldelse = anmeldelse.ModtagerAnlaeg != null && anmeldelse.ModtagerAnlaeg.Jordmodtager != null && currentBruger.Person.Firmaoplysninger != null && (anmeldelse.Anmelder.Person.Firmaoplysninger == null || anmeldelse.Anmelder.Person.Firmaoplysninger.CVR != currentBruger.Person.Firmaoplysninger.CVR);

      var kommuneNavn = anmeldelse.Kommune.Navn;
      var jordks = _kodelisteBusiness.ReadAktiveJordKlassifikationTypesForKommune(kommuneNavn);
      model.JordklassifikationTypeListe = from o in jordks select new SelectListItem { Selected = false, Text = o.Navn, Value = o.Id.ToString() };

      if (anmeldelse.Jord.KoerselStart.HasValue)
      {
        model.DatoFra = anmeldelse.Jord.KoerselStart.Value.ToString("dd-MM-yyyy");
      }
      if (anmeldelse.Jord.KoerselSlut.HasValue)
      {
        model.DatoTil = anmeldelse.Jord.KoerselSlut.Value.ToString("dd-MM-yyyy");
      }

      //if (anmeldelse.StatusAnmeldelse.FirstOrDefault(x => x.StatusAnmeldelseType.Kode == (int) EnumStatusAnmeldelse.Afsendt) != null)
      if (_statusAnmeldelseBusiness.IsAnmeldelseIndsendtMenIkkeAktiv(anmeldelse.AnmeldelseEffektivStatus))
      {
        model.kanIndsende = false;
      }
      else
      {
        model.kanIndsende = true;
      }
      model.Anmeldelse = anmeldelse;
      model.AlmJordflytning = anmeldelse.Jord.JordflytningType.Kode == (int)EnumJordflytningType.Alm;

      model.IndeholderJordenAffald = (!string.IsNullOrEmpty(anmeldelse.Jord.AndenAffaldType) | anmeldelse.Jord.AffaldType != null);

      if (anmeldelse.Jord != null && anmeldelse.Jord.AffaldType != null)
      {
        model.Affaldstype = anmeldelse.Jord.AffaldType.Kode.ToString();
      }
      if (anmeldelse.Jord != null && anmeldelse.Jord.AffaldType != null)
      {
        model.AndetAffald = anmeldelse.Jord.AndenAffaldType;
      }
      if (anmeldelse.Jord != null && anmeldelse.Jord.JordKlassifikationType != null)
      {
        model.Forueningskategori = anmeldelse.Jord.JordKlassifikationType.Id.ToString();
      }
      model.HvemBetaler = SetBetaler(anmeldelse);
      if (model.HvemBetaler == "3")
      {
        if (anmeldelse.Betaler != null && anmeldelse.Betaler.Person != null && anmeldelse.Betaler.Person.Firmaoplysninger != null && anmeldelse.Betaler.Person.Firmaoplysninger.Firmanavn != null)
        {
          model.AndenBetaler = anmeldelse.Betaler.Person.Email;
        }
        else
        {
          model.AndenBetaler = "";
        }
      }

      return View(model);
    }

    public RedirectResult SwitchView(bool mobile, string returnUrl)
    {
      // If the mobile user has requested to view the mobile view
      // remove any overridden user agent for the current request
      if (Request.Browser.IsMobileDevice == mobile)
        HttpContext.ClearOverriddenBrowser();
      else
        // Otherwise override the browser setting to desktop mode
        HttpContext.SetOverriddenBrowser(mobile ? BrowserOverride.Mobile : BrowserOverride.Desktop);

      return Redirect(returnUrl);
    }

    public ActionResult Login(string brugernavn, string password)
    {

      bool res = _securityProvider.Login(brugernavn, password, true);

      return Json(new { Success = res });
    }


    public ActionResult SoegAndenBetaler(string term)
    {
      var temp = _personBusiness.Search(x => x.Email.Contains(term) & x.Aktiv & x.Betaler != null).Select(y => y.Email).ToList();
      return Json(temp, JsonRequestBehavior.AllowGet);
    }

    [AllowAnonymous]
    public ActionResult GetJordforurening(string wkt, string kommunenavn, string returnType = "view")
    {
      var model = new TjekJordSvarModel();
      IList<Matrikel> matrikler = new List<Matrikel>();
      DbGeometry geom = null;
      if (!string.IsNullOrEmpty(wkt))
      {
        try
        {
          geom = DbGeometry.FromText(wkt, 25832);
          if (geom.Area < 10000)
          {
            //Henter matrikler fra wfs service
            matrikler = _matrikelBusiness.ReadMatrikler(wkt);
            Session["Matrikler"] = matrikler;
            Session["OprindelsesGeom"] = geom;
            if (matrikler == null)
            {
              var f = new ForureningsOpslagResult("Arealet er over 1 hektar. Kontakt myndigheden.");

            }
          }

        }
        catch (Exception)
        {
          model.FejlBesked = "Der er fejl i de eksterne systemer!<br/>System administratoren er blevet underrettet.<br/>Prøv eventuelt igen lidt senere.";
          ModelState.Clear();

        }

        Kommune kommune = _kodelisteBusiness.ReadKommuneByNavn(kommunenavn);


                //Udkommenteret 09-01-2019 af KFK for at sørge for at vi altid slår op og tjekker for forurening.
                //Da når brugeren er logget ind så kan der kun søges på adresser der findes hos aktive kommuner er det udkommenterede kode ikke længere nødvendigt
        //Hvis ikke logget ind skal vi altid returnerer svaret, tror dog kun der kan søges på aktiv kommune adresser når man er logget ind.
        //if (kommune == null || !kommune.Aktiv)
        //{
        //  model.AnmelderpligtigJord = false;
        //  model.FejlBesked = "Denne kommune er ikke tilsluttet dette system.";
        // // model.AnmelderpligtigJordTekst = "Denne kommune er ikke tilsluttet dette system."; //ved ikke om dette vil være en god ide.
        //  ModelState.Clear();
        //  return View("_TjekJordSvar", model);
        //}

        var oprindelsessted = new Oprindelsessted();
        oprindelsessted.Geom = geom;
        oprindelsessted.Matrikel = matrikler;

        var forureningOpslag = _forureningsOpslagBusiness.Read(oprindelsessted, kommune.Id);
        var forureningopslagResult = forureningOpslag.GetResultList();

        model.ForureningOpslagResultatList = new List<ForureningsOpslagResult>();
        model.ForureningOpslagResultatList.AddRange(forureningOpslag.GetResultList().OrderBy(f => f.HeaderSortering));

        var ka = forureningOpslag.GetHigestKlassifikation(kommune.Kommunenr, 1);//altid jord fra ejendom
        //var ka = forureningOpslag.GetMiljoePortalKlassifikation(kommune.Kommunenr, 1);//altid jord fra ejendom

        model.AnmelderpligtigJord = forureningOpslag.HasAnmeldePligt(kommune.Kommunenr, 1);

        //Gemmer Jordforureningsopslaget
        var opslag = new Jordforureningsopslag();
        //opslag.Id = opretModel.Anmeldelse.Id;
        opslag.JordKlassifikationType = ka;
        opslag.Tid = DateTime.Now;

        //Tjekker om listen indeholder en bestemt type 
        var v2 = (from k in forureningopslagResult where k.MiljoePortalKlassifikation != null && k.MiljoePortalKlassifikation.Id == (int)MiljoePortalKlassifikationEnum.JordforureningV2 && k.ResultException == null select k).FirstOrDefault();
        opslag.V2 = v2 != null;

        var v1 = (from k in forureningopslagResult where k.MiljoePortalKlassifikation != null && k.MiljoePortalKlassifikation.Id == (int)MiljoePortalKlassifikationEnum.JordforureningV1 && k.ResultException == null select k).FirstOrDefault();
        opslag.V1 = v1 != null;

        //OMK analysepligt
        var omkAnalysepligt = (from k in forureningopslagResult where k.MiljoePortalKlassifikation != null && k.MiljoePortalKlassifikation.Id == (int)MiljoePortalKlassifikationEnum.OmraadeMedMravMmMnalyser && k.ResultException == null select k).FirstOrDefault();
        opslag.OmkAnalysepligt = omkAnalysepligt != null;

        //OMK Analysefri letforurenet jord
        var omkLet = (from k in forureningopslagResult where k.MiljoePortalKlassifikation != null && k.MiljoePortalKlassifikation.Id == (int)MiljoePortalKlassifikationEnum.AnalysefritOmraadeLetForurenetJord && k.ResultException == null select k).FirstOrDefault();
        opslag.OmkLet = omkLet != null;

        //OMK ren
        var omkRen = (from k in forureningopslagResult where k.MiljoePortalKlassifikation != null && k.MiljoePortalKlassifikation.Id == (int)MiljoePortalKlassifikationEnum.AnalysefritOmraadeRenJord && k.ResultException == null select k).FirstOrDefault();
        opslag.OmkRen = omkRen != null;

        //Kommunes miljødatabase (Århus Geoenviron)
        var komMiljoeDb = (from k in forureningopslagResult where k.MiljoePortalKlassifikation == null && k.ResultException == null select k.LongText).FirstOrDefault();
        if (!string.IsNullOrEmpty(komMiljoeDb))
          opslag.KommunensMiljoeDb = komMiljoeDb;
        model.ForureningsStatus = opslag.JordKlassifikationType.Navn;
        model.Forureningskode = opslag.JordKlassifikationType.Id.ToString();

        //Tjek for exceptions fra opslag i eksterne datakilder
        var resultException = (from e in forureningopslagResult where e.ResultException != null select e.ResultException).ToList();
        if (resultException.Any())
        {
          model.FejlBesked = "Det er ikke muligt at hente oplysning om ejendommens forureningsstatus fra Danmarks Miljøportal eller kommunens database. Har du kendskab til den aktuelle forureningsstatus, kan du udfylde anmeldelsen og sende den til kommunen. Alternativt kan du vente til det igen er muligt at hente disse oplysninger.";
          opslag.KommunensMiljoeDb = "Forureningsstatus kan ikke hentes fra eksterne datakilder.";//Når der er en besked i dette felt kan anmeldelsen ikke behandles automatisk.
        }

        Session["Jordforureningsopslag"] = opslag;


        ////Gemmer anmeldelsen i en session til kvitteringssiden. Problemer  med print på denne side pga. IE sikkerheds issue. Man må ikke oprettet et dokument og skrive indholdet fra en div i det efterfult af print... Derfor denne nødløsning.
        //		Session["anmeldelse_kvittering"] = opretModel;

        //Kodelister + Stamdata
        //opretModel = initOpretMode(opretModel);

        ModelState.Clear();
        //return PartialView("_StedJordforuningOpslag", opretModel);

      }
      if (returnType == "view")
      {
        return View("_TjekJordSvar", model);
      }
      else
      {
        return Json(new { Forureningskode = model.Forureningskode }, JsonRequestBehavior.AllowGet);
      }
    }

    public ActionResult GetModtageranlaeg(string anvenderFJ, string jordklassifikation)
    {
      dynamic modtagerAnlaeg = null;
      Guid _jordklassifikation = new Guid(jordklassifikation);
      //Guid _jordklassifikation = new Guid("1426C54A-6C06-4970-AC52-327D013751C7");
      if (anvenderFJ != null && anvenderFJ == "on")
      {
        modtagerAnlaeg = _modtagerAnlaegBusiness.ReadAktiveModtagerAnlaeg().Where(x => x.AnvenderJF && x.JordKlassifikationTypeId == _jordklassifikation).Select(x => new { text = x.Navn, value = x.Id }).OrderBy(x => x.text).ToList();
      }
      else
      {
        modtagerAnlaeg = _modtagerAnlaegBusiness.ReadAktiveModtagerAnlaeg().Where(x => !x.AnvenderJF && x.JordKlassifikationTypeId == _jordklassifikation).Select(x => new { text = x.Navn, value = x.Id }).OrderBy(x => x.text).ToList();
      }

      return Json(modtagerAnlaeg, JsonRequestBehavior.AllowGet);
    }

    /// <summary>
    /// Henter anmeldelser for den nuværende bruger baseret på teksten sendt med i variablen type.
    /// </summary>
    /// <param name="type">Definerer den type af anmeldelser der skal hentes</param>
    /// <returns>Json liste med MobileAnmeldelseListModel for de matchende anmeldelser</returns>
    public ActionResult GetBrugerAnmeldelser(string type)
    {
      var brugerId = _brugereBusiness.Read(_securityProvider.CurrentUser.Identity.Name).Person.Id;
      IList<MobileAnmeldelseListModel> result = new List<MobileAnmeldelseListModel>();

      if (type == "afsluttede")
      {
        result =
          _soegBusiness.GetAfsluttedeAnmeldelser(brugerId)
                       .Select(
                         x =>
                         new MobileAnmeldelseListModel
                           {
                             Id = x.AnmeldelseId,
                             Status = x.SenesteStatus,
                             Visningstekst = x.OprindelsesSted
                           }).ToList();
      }
      else if (type == "aktive")
      {
        result =
          _soegBusiness.GetAktiveAnmeldelser(brugerId, false)
                       .Select(
                         x =>
                         new MobileAnmeldelseListModel
                           {
                             Id = x.AnmeldelseId,
                             Status = x.SenesteStatus,
                             Visningstekst = x.OprindelsesSted
                           }).ToList();
      }
      else if (type == "fremsendte")
      {
        result =
          _soegBusiness.GetFremSendteAnmeldelser(brugerId, false)
                       .Select(
                         x =>
                         new MobileAnmeldelseListModel
                           {
                             Id = x.AnmeldelseId,
                             Status = x.SenesteStatus,
                             Visningstekst = x.OprindelsesSted
                           }).ToList();
      }
      else if (type == "kladder")
      {
        result =
          _soegBusiness.GetIkkeFremSendteAnmeldelser(brugerId, false)
                       .Select(
                         x =>
                         new MobileAnmeldelseListModel
                           {
                             Id = x.AnmeldelseId,
                             Status = x.SenesteStatus,
                             Visningstekst = x.OprindelsesSted
                           }).ToList();
      }
      return Json(result, JsonRequestBehavior.AllowGet);
    }

    public ActionResult GetLoebenummer(string anmeldelsesId)
    {

      var anmeldelse = _anmeldelserBusiness.Read(new Guid(anmeldelsesId));

      if (anmeldelse != null)
      {
        return Json(anmeldelse.Nummer, JsonRequestBehavior.AllowGet);
      }
      else
      {
        return Json(-1, JsonRequestBehavior.AllowGet);
      }


    }



    [AllowAnonymous]
    public ActionResult ResetPassword(string email)
    {
      bool res = false;
      String generatedPassword = _securityProvider.GenerateNewPassword(email);
      if (!string.IsNullOrEmpty(generatedPassword))
      {
        _adviseringBusiness.SendGlemtPassword(email, generatedPassword);
        res = true;
      }
      return Json(new { Success = res }, JsonRequestBehavior.AllowGet);
    }

    #region gem / indsend anmeldelse

    //[HttpPost]
    public ActionResult IndsendAnmeldelse(MobileAnmeldelseModel model)
    {
      Anmeldelse aDb = _anmeldelserBusiness.Read(model.Anmeldelse.Id);



      var gammelanmeldelse = _anmeldelserBusiness.CloneForLogingPurpose(aDb); //ShalowClon af anmeldelse til loging formål

      Anmeldelse anmeldelse = OpdaterAnmeldelse(aDb, model);
      bool res = true;
      string valideringsErrors = "";
      //Validation

      anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType = _kodelisteBusiness.ReadOprindelsesstedKlassifikationType(1);

      if (anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType != null)
      {
        anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationTypeId = anmeldelse.Oprindelsessted.OprindelsesstedKlassifikationType.Id; // Nok ikke nødvendigt, men just in case.
      }


      //Tjekker status på anmeldelse.
      //Er anmeldelsen Aktiv eller afsluttes, må den ikke ændres. 
      //Jeg har ikke godt kendskab til mobil sitet, så jeg lavet tjekket her og sender en besked retur. 
      //KVE - 2014 01 15
      var isAnmeldelseAktiv = _statusAnmeldelseBusiness.IsAnmeldelseAktiv(aDb);
      if (isAnmeldelseAktiv)
      {
        res = false;
        ModelState.AddModelError("Anmeldelse", "Anmeldelsen er aktiv");
        valideringsErrors += "<p>Anmeldelsen er aktiv og kan ikke indsendes.</p>";
      }
      var isAnmeldelseAfsluttet = _statusAnmeldelseBusiness.IsAnmeldelseAfsluttet(aDb);
      if (isAnmeldelseAfsluttet)
      {
        res = false;
        ModelState.AddModelError("Anmeldelse", "Anmeldelsen er afsluttet");
        valideringsErrors += "<p>Anmeldelsen er afsluttet og kan ikke indsendes.</p>";
      }




      //ValiderJord(opretModel);
      if (anmeldelse.Jord.Jordproever.HasValue && anmeldelse.Jord.Jordproever.Value)
      {
        //Hvis brugeren har angivet at der er udtaget jordprøver skal nedenstående valideres.

        if (string.IsNullOrEmpty(anmeldelse.Jord.MiljoeTekniskTilsyn))
        {
          res = false;
          ModelState.AddModelError("MiljoetekniskTilsyn", "Jorden - Jordprøver er udtaget af skal udfyldes");
          valideringsErrors += "<p>Jordprøver er udtaget af skal udfyldes</p>";
        }
        if (!anmeldelse.Jord.AntalProever.HasValue)
        {
          res = false;
          ModelState.AddModelError("AntalProever", "Jorden - Antal prøver skal udfyldes");
          valideringsErrors += "<p>Antal prøver skal udfyldes</p>";
        }

        if (!anmeldelse.Jord.KoerselStart.HasValue)
        {
          res = false;
          ModelState.AddModelError("KoerselStart", "Jorden - Kørsel start skal udfyldes");
          valideringsErrors += "<p>Kørsel start skal udfyldes</p>";
        }
        if (!anmeldelse.Jord.KoerselSlut.HasValue)
        {
          res = false;
          ModelState.AddModelError("KoerselSlut", "Jorden - Kørsel slut skal udfyldes");
          valideringsErrors += "<p>Kørsel slut skal udfyldes</p>";
        }
        if (!anmeldelse.Jord.ForventetJordmaengdeTon.HasValue)
        {
          res = false;
          ModelState.AddModelError("ForventetJordmaengdeTon", "Jorden - Forventet jordmængde skal udfyldes");
          valideringsErrors += "<p>Forventet jordmængde skal udfyldes</p>";
        }
        if (model.IndeholderJordenAffald && anmeldelse.Jord.AffaldType.Kode == 6 && model.AndetAffald == "") // Ved ikke om dette er helt korrekt
        {
          res = false;
          ModelState.AddModelError("JordAndenAffaldType", "Jorden - Hvis anden affaldstype er valgt skal anden affaldstype udfyldes");
          valideringsErrors += "<p>Hvis anden affaldstype er valgt skal anden affaldstype udfyldes</p>";
        }

        if (model.IndeholderJordenAffald && model.Affaldstype == "")
        {
          res = false;
          ModelState.AddModelError("JordAndenAffaldType",
                                   "Jorden - Hvis jorden indeholder affald skal type angives");
          valideringsErrors += "<p>Hvis jorden indeholder affald skal type angives</p>";
        }

        if (anmeldelse.Jord.IntaktJord == false)
        {

          //Ved ikke om dette er aktuelt for mobil løsningen da der ikke kan uploades dokumenter.

          //Hvis brugeren ændre Jordforureningskategori fra det som er fundet i miljøportalen skal brugeren have vedhæftet filer.
          //  if (opretModel.ForureningOpslagJordklassifikationTypeId != opretModel.Anmeldelse.Jord.JordKlassifikationType.Id)
          //      {
          //      bool docVal = false;

          //    if (Session["docList"] != null)
          //     docVal = true;

          //   if (opretModel.Anmeldelse.Jord.Dokumentation != null && opretModel.Anmeldelse.Jord.Dokumentation.Count > 0)
          //    docVal = true;

          //   if (!docVal)
          //  {
          //   ModelState.AddModelError("JordenDokumentationKraevet", "Jorden - Der skal vedhæftes dokumentation for den valgte forureningskategori.");
          //  }
        }
      }

      //Jorden - Akut jordflytning
      //opretModel.JordflytningTypeListe = _kodelisteBusiness.ReadAktiveJordflytningTypes();
      //if (opretModel.Anmeldelse.Jord.JordflytningType.Id == opretModel.JordflytningTypeListe[1].Id && string.IsNullOrEmpty(opretModel.Anmeldelse.Jord.AkutBaggrund))
      //	ModelState.AddModelError("JordenAkutBaggrund", "Jorden - Baggrund for Akut jordflytning skal angives");

      Guid gMod;
      if (!Guid.TryParse(model.SelectedModtagerAnlaegId, out gMod))
      {
        res = false;
        ModelState.AddModelError("ModtagerAnlaeg", "Modtager og transportør - Der skal vælges et modtageanlæg");
        valideringsErrors += "<p>Der skal vælges et modtageanlæg</p>";
      }

      Guid gTrans;
      if (!Guid.TryParse(model.SelectedTransportoerId, out gTrans))
      {
        res = false;
        ModelState.AddModelError("Transportoer", "Modtager og transportør - Der skal vælges en transportør");
        valideringsErrors += "<p>Der skal vælges en transportør</p>";
      }

      if (ModelState.ContainsKey("Anmeldelse.Jord.JordKlassifikationType") && anmeldelse.Jord != null && anmeldelse.Jord.JordKlassifikationType != null)
        ModelState["Anmeldelse.Jord.JordKlassifikationType"].Errors.Clear();

      if (ModelState.IsValid)
      {
        try
        {
          _anmeldelserBusiness.GemAnmeldelse(anmeldelse, _brugereBusiness.Read(_securityProvider.CurrentUser.Identity.Name).Person, gammelanmeldelse);
          //opretModel = GemAnmeldelse(opretModel);
          ViewBag.Message = "Anmeldelsen er gemt";

          var b = _brugereBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
          var personSomErLoggetInd = b.Person;

          //Tjek om betaler er godkendt af jordmodtagers bogholder.
          var anmodningOmGodkendelse = _statusBetalerBusiness.AnmodOmGodkendelseHosJordmodtager(anmeldelse.Betaler, anmeldelse.ModtagerAnlaeg.Jordmodtager);
          if (anmodningOmGodkendelse == false)
          {
            valideringsErrors += "<p>Jordmodtager har afvist betaleren!</p>";
            ModelState.Clear();
            res = false;
            return Json(new { Success = res, Validering = valideringsErrors }, JsonRequestBehavior.AllowGet);
          }

          //Automatiske procedure som sætter diverse statusser.
          _anmeldelserBusiness.IndsendAnmeldelseAutomatiskeProcedurer(anmeldelse, personSomErLoggetInd);

        }
        catch (Exception ex)
        {
          res = false;
          //Loging
          logger.LogException(ex);
          //Errormessage
          valideringsErrors += "<p>Der er sket en fejl!<p/>";
          ModelState.Clear();
          //		return PartialView("_Validering", opretModel);
          return Json(new { Success = res, Validering = valideringsErrors }, JsonRequestBehavior.AllowGet);
        }
      }
      return Json(new { Success = res, Validering = valideringsErrors }, JsonRequestBehavior.AllowGet);
    }

    // model.Forueningskategori er null ved opret
    public ActionResult SaveAnmeldelse(MobileAnmeldelseModel model)
    {
      Anmeldelse aDb = null;
      Anmeldelse gammelanmeldelse = null;
      BrugerProfil currentUser = _brugereBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
      if (model.Anmeldelse.Id == Guid.Empty)
      {
        var forureningsId = new Guid(model.Forueningskategori);
        aDb = model.Anmeldelse;
        if (forureningsId != aDb.Jord.JordKlassifikationTypeId)
        {
          aDb.Jord.JordKlassifikationType = _jordKlassifikationTypeRepository.Read(forureningsId);
        }
        //aDb.Jord.JordKlassifikationType =	_jordKlassifikationTypeRepository.Search(x => x.Id == forureningsId).First();

        aDb.Kommune = _kommuneBusiness.ReadKommuneByNavn(model.Anmeldelse.Kommune.Navn);

        var anmelder = _anmelderBusiness.Search(x => x.Id == currentUser.Person.Id).FirstOrDefault();

        if (anmelder != null && anmelder.Person != null)
        {
          aDb.Anmelder = _anmelderBusiness.Read(currentUser.Person.Id);
        }
        else
        {
          aDb.Anmelder = new Anmelder { Person = currentUser.Person, Id = currentUser.Person.Id };
        }
        if ((aDb.Jordforureningsopslag == null || model.Anmeldelse.Jordforureningsopslag.Id == Guid.Empty) && Session["Jordforureningsopslag"] != null)
        {
          //aDb.Jordforureningsopslag = (Jordforureningsopslag)Session["Jordforureningsopslag"];
          //aDb.Jordforureningsopslag.JordKlassifikationType = _jordKlassifikationTypeRepository.Read(aDb.Jordforureningsopslag.Id);
          var opslag = (Jordforureningsopslag)Session["Jordforureningsopslag"];
          aDb.Jordforureningsopslag = opslag;
          aDb.Jordforureningsopslag.JordKlassifikationType = _jordKlassifikationTypeRepository.Read(opslag.JordKlassifikationType.Id);
        }
        var matrikler = Session["Matrikler"];
        var oprindelseGeom = Session["OprindelsesGeom"];

        if (matrikler != null && aDb.Oprindelsessted != null)
        {
          aDb.Oprindelsessted.Matrikel = (IList<Matrikel>)matrikler;
          aDb.Oprindelsessted.Geom = (DbGeometry)oprindelseGeom;
        }


      }
      else
      {
        aDb = _anmeldelserBusiness.Read(model.Anmeldelse.Id);

        var isAnmeldelseAktiv = _statusAnmeldelseBusiness.IsAnmeldelseAktiv(aDb);
        if (isAnmeldelseAktiv)
          return Json(new { Success = false, anmeldelseId = aDb.Id, Message = "Anmeldelsen er aktiv og kan ikke redigeres." }, JsonRequestBehavior.AllowGet);

        var isAnmeldelseAfsluttet = _statusAnmeldelseBusiness.IsAnmeldelseAfsluttet(aDb);
        if (isAnmeldelseAfsluttet)
          return Json(new { Success = false, anmeldelseId = aDb.Id, Message = "Anmeldelsen er afsluttet og kan ikke redigeres." }, JsonRequestBehavior.AllowGet);



        gammelanmeldelse = _anmeldelserBusiness.CloneForLogingPurpose(aDb); //ShalowClon af anmeldelse til loging formål
      }

      if (aDb.Jord.IntaktJord == false)
      {
        if (_anmeldelserBusiness.DokumentationPaakraevet(aDb.Jordforureningsopslag,
                                                           aDb.Jordforureningsopslag.JordKlassifikationType.Id,
                                                           Guid.Parse(model.Forueningskategori),
                                                           model.Anmeldelse.Jord.ForventetJordmaengdeTon,
                                                           aDb.Kommune.Kommunenr))
        {
          return Json(new { Success = false, anmeldelseId = aDb.Id, Message = "Anmeldelser, som kræver vedlagt dokumentation, kan ikke indsendes via mobil udgaven af FlytJord." }, JsonRequestBehavior.AllowGet);
        }
      }

      Anmeldelse anmeldelse = OpdaterAnmeldelse(aDb, model);

      bool res = true;

      //Vil tro at dette er den korrekte metode.
      _anmeldelserBusiness.GemAnmeldelse(anmeldelse, currentUser.Person, gammelanmeldelse);

      return Json(new { Success = res, anmeldelseId = anmeldelse.Id }, JsonRequestBehavior.AllowGet);
    }

    #endregion


    #region private methods

    private Anmeldelse OpdaterAnmeldelse(Anmeldelse anmeldelse, MobileAnmeldelseModel model)
    {
      anmeldelse.Oprindelsessted.Adresse = model.Anmeldelse.Oprindelsessted.Adresse;
      anmeldelse.Oprindelsessted.Postnummer = model.Anmeldelse.Oprindelsessted.Postnummer;
      anmeldelse.Oprindelsessted.PostDistrikt = model.Anmeldelse.Oprindelsessted.PostDistrikt;
      //	anmeldelse.Kommune.Navn = model.Anmeldelse.Kommune.Navn;
      anmeldelse.Jord.Jordproever = model.Anmeldelse.Jord.Jordproever;
      if (model.IndeholderJordenAffald)
      {
        //m.Affaldstype;
        anmeldelse.Jord.AffaldType = _kodelisteBusiness.ReadAktiveAffaldTypes().FirstOrDefault(x => x.Kode.ToString() == model.Affaldstype);
        anmeldelse.Jord.AffaldTypeId = anmeldelse.Jord.AffaldType.Id;

        if (anmeldelse.Jord.AffaldType.Kode == (int)EnumAffaldstype.Andet)
        {
          anmeldelse.Jord.AndenAffaldType = model.AndetAffald;
        }
        else
        {
          anmeldelse.Jord.AndenAffaldType = null;
        }
      }
      else
      {
        anmeldelse.Jord.AndenAffaldType = null;
        anmeldelse.Jord.AffaldType = null;
      }
      anmeldelse.Jord.IntaktJord = model.Anmeldelse.Jord.IntaktJord;
      anmeldelse.Jord.JordKlassifikationType = _jordKlassifikationTypeRepository.Read(Guid.Parse(model.Forueningskategori));

      anmeldelse.Jord.JordflytningType = _kodelisteBusiness.ReadAktiveJordflytningTypes().FirstOrDefault(x => x.Kode == (int)EnumJordflytningType.Alm);

      anmeldelse.Jord.ForventetJordmaengdeTon = model.Anmeldelse.Jord.ForventetJordmaengdeTon;
      if (!string.IsNullOrEmpty(model.DatoFra))
      {
        anmeldelse.Jord.KoerselStart = Convert.ToDateTime(model.DatoFra, CultureInfo.GetCultureInfo("da-DK"));
      }
      if (!string.IsNullOrEmpty(model.DatoTil))
      {
        anmeldelse.Jord.KoerselSlut = Convert.ToDateTime(model.DatoTil, CultureInfo.GetCultureInfo("da-DK"));
      }
      anmeldelse.Jord.TidligereErhvervsaktivitet = model.Anmeldelse.Jord.TidligereErhvervsaktivitet;
      anmeldelse.Jord.JordarbejdeBeskrivelse = model.Anmeldelse.Jord.JordarbejdeBeskrivelse;
      anmeldelse.Jord.Bemaerkning = model.Anmeldelse.Jord.Bemaerkning;
      // m.HvemBetaler;

      if (model.SelectedTransportoerId != null)
      {
        anmeldelse.Transportoer = _transportoerBusiness.Read(new Guid(model.SelectedTransportoerId));
        anmeldelse.TransportoerId = anmeldelse.Transportoer.Id; // Ved ikke om dette er nødvendigt, men better safe than sorry.
      }
      else
      {
        anmeldelse.Transportoer = null;
        anmeldelse.TransportoerId = null;
      }

      if (model.HvemBetaler == "1")
      {
        if (anmeldelse.Anmelder != null && anmeldelse.Anmelder.Person != null) //  && anmeldelse.Anmelder.Person.Betaler != null ??
        {
          anmeldelse.Betaler = anmeldelse.Anmelder.Person.Betaler;
          if (anmeldelse.Betaler != null)
          {
            anmeldelse.BetalerId = anmeldelse.Betaler.Id;
          }
        }
        else
        {
          anmeldelse.Betaler = _brugereBusiness.Read(_securityProvider.CurrentUser.Identity.Name).Person.Betaler;
          if (anmeldelse.Betaler != null)
          {
            anmeldelse.BetalerId = anmeldelse.Betaler.Id;
          }
        }
      }
      else if (model.HvemBetaler == "2")
      {
        anmeldelse.Betaler = anmeldelse.Transportoer.Person.Betaler;

        if (anmeldelse.Transportoer != null && anmeldelse.Transportoer.Person != null)
        {
          anmeldelse.BetalerId = anmeldelse.Transportoer.Person.Betaler.Id;
        }
        else
        {
          anmeldelse.Betaler = _transportoerBusiness.Read(new Guid(model.SelectedTransportoerId)).Person.Betaler;
          //anmeldelse.Betaler = _brugereBusiness.Read(model.SelectedTransportoerId).Person.Betaler;
          if (anmeldelse.Betaler != null)
          {
            anmeldelse.BetalerId = anmeldelse.Betaler.Id;

          }
        }

      }
      else if (model.HvemBetaler == "3" && model.AndenBetaler.Length > 0)
      {
        var person = _personBusiness.Search(x => x.Email == model.AndenBetaler).FirstOrDefault();

        if (person != null && person.Betaler != null)
        {
          anmeldelse.Betaler = person.Betaler;
          anmeldelse.BetalerId = person.Betaler.Id;
        }
      }

      if (model.SelectedModtagerAnlaegId != null)
      {
        anmeldelse.ModtagerAnlaeg = _modtagerAnlaegBusiness.Read(new Guid(model.SelectedModtagerAnlaegId));
        anmeldelse.ModtagerAnlaegId = anmeldelse.ModtagerAnlaegId;
        // Ved ikke om dette er nødvendigt, men better safe than sorry.
      }
      else
      {
        anmeldelse.ModtagerAnlaeg = null;
        anmeldelse.ModtagerAnlaegId = null;
      }
      return anmeldelse;
    }

    private string SetBetaler(Anmeldelse anmeldelse)
    {
      string res = "3";

      if (anmeldelse.Betaler != null && anmeldelse.Betaler.Person != null && anmeldelse.Anmelder != null && anmeldelse.Betaler.Person.Id == anmeldelse.Anmelder.Person.Id)
      {
        res = "1";
      }
      else if (anmeldelse.Transportoer != null && anmeldelse.Transportoer.Person != null && anmeldelse.Betaler != null && anmeldelse.Betaler.Person != null && anmeldelse.Betaler.Person.Id == anmeldelse.Transportoer.Person.Id)
      {
        res = "2";
      }
      else if (anmeldelse.Betaler == null)
      {
        res = "";
      }

      return res;
    }

    #endregion
  }
}
