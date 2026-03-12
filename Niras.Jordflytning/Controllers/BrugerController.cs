using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Niras.Jordflytning.Areas.Backend.ViewModels;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
//using Niras.Jordflytning.Filters;
using Niras.Jordflytning.ViewModels.Anmeldelse;
using Niras.Jordflytning.ViewModels.Bruger;
using Niras.Jordflytning.ViewModels.GridBindingModels;
using WebMatrix.WebData;

namespace Niras.Jordflytning.Controllers
{
  //[InitializeSimpleMembership]
  public class BrugerController : Controller
  {

    #region Variables

    private readonly IBrugereBusiness _brugerBusiness;
    private readonly ITransportoerBusiness _transportoerBusiness;
    private readonly IModtagerAnlaegBusiness _modtagerAnlaegBusiness;
    private readonly ISecurityProvider _securityProvider;
    private readonly IGenericBusiness<Lastbil> _lastbilBusiness;
    private readonly IStatusBetalerRepository _statusBetalerRepository;
    private readonly IKodelisteBusiness _kodelisteBusiness;
    private readonly IPersonBusiness _personBusiness;
    private readonly IBemyndigedeAnmelderBusiness _bemyndigedeAnmelderBusiness;

    #endregion

    public BrugerController(
      IBrugereBusiness brugerBusiness,
      IModtagerAnlaegBusiness modtagerAnlaegBusiness,
      ISecurityProvider securityProvider,
      IGenericBusiness<Lastbil> lastbilBusiness,
      IStatusBetalerRepository statusBetalerRepository,
      IKodelisteBusiness kodelisteBusiness,
      IBemyndigedeAnmelderBusiness bemyndigedeAnmelderBusiness,
      IPersonBusiness personBusiness,
      ITransportoerBusiness transportoerBusiness
      )
    {
      _brugerBusiness = brugerBusiness;
      _modtagerAnlaegBusiness = modtagerAnlaegBusiness;
      _securityProvider = securityProvider;
      _lastbilBusiness = lastbilBusiness;
      _statusBetalerRepository = statusBetalerRepository;
      _kodelisteBusiness = kodelisteBusiness;
      _bemyndigedeAnmelderBusiness = bemyndigedeAnmelderBusiness;
      _personBusiness = personBusiness;
      _transportoerBusiness = transportoerBusiness;
    }

    #region Public methods

    /// <summary>
    /// Get the model
    /// </summary>
    public ActionResult Profil()
    {
      var brugerProfilModel = new BrugerProfilModel();

      var aktiveModtageAnlaeg = _modtagerAnlaegBusiness.ReadAktiveModtagerAnlaeg().Where(x => x.AnvenderJF).ToList();
      brugerProfilModel.FiltreredeModtagerAnlaeg = AnmeldelserController.ConvertModtagerAnlaegToModel(aktiveModtageAnlaeg, null).ToList();
      brugerProfilModel.MiljoeklasseTyper = _kodelisteBusiness.ReadAktiveMiljoeklasseTyper();
      brugerProfilModel.BackendUser = false;
      Session["BemyndigedeAnmeldere"] = null;

      if (_securityProvider.CurrentUser.Identity.IsAuthenticated)
      {
        var brugerProfil = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
        var person = brugerProfil.Person;

        brugerProfilModel.ErLoggetInd = true;
        brugerProfilModel.BackendUser = brugerProfil.Roles.Any();
        brugerProfilModel.BrugerModel.Person = person;

        GetTransportoer(brugerProfilModel, person);
        GetMineModtagerAnlaeg(brugerProfilModel);
        GetAdvisOgAlarm(person, brugerProfilModel);
        GetBemyndigedeAnmeldere(person);

        //Transportør fanen synlig eller ej
        if (brugerProfil.Person != null && brugerProfil.Person.Transportoer != null)
          brugerProfilModel.ShowTransportoerFane = brugerProfil.Person.Transportoer.Aktiv;

        brugerProfilModel.ShowBrugerEditFane = true;
        brugerProfilModel.ShowAdviseringFane = true;
        brugerProfilModel.ShowBemyndigedeFane = true;
        brugerProfilModel.ShowModtageAnlaegFane = true;

        if (brugerProfil.Person != null && brugerProfil.Person.Firmaoplysninger != null)
        {
            var anlaegliste = _modtagerAnlaegBusiness.Search(a => a.Jordmodtager.CVR == brugerProfil.Person.Firmaoplysninger.CVR && a.Jordmodtager.ModtagerAnlaeg.FirstOrDefault(p => p.KontaktpersonEmail == brugerProfil.Person.Email) != null);
            var anlaeg = anlaegliste.FirstOrDefault();
            brugerProfilModel.
                ErJordmodtager = anlaeg != null;
        }

          

      }
      else
      {
        //Ved ny brugere som ikke er Authenticated endnu
        
        brugerProfilModel.OenskerAlarmer = true;
        brugerProfilModel.AlarmJordmaengdeIProcent = 75; //Default værdi

        brugerProfilModel.ShowBrugerEditFane = true;
        brugerProfilModel.ShowAdviseringFane = false;
        brugerProfilModel.ShowBemyndigedeFane = false;
        brugerProfilModel.ShowModtageAnlaegFane = false;
        
      }
      ModelState.Clear();
      return View(brugerProfilModel);
    }

    /// <summary>
    /// Save the model
    /// </summary>
    [HttpPost]
    public ActionResult Profil(BrugerProfilModel brugerProfilModel)
    {
        string detailedMsg = "";
        var brugerProfil = new BrugerProfil();
        
        if (brugerProfilModel.BrugerModel.Transportoer && !brugerProfilModel.BrugerModel.Company)
        {
            ModelState.AddModelError("BrugerModel.Company", @"Firmaoplysninger skal udfyldes for transportør.");
            detailedMsg = "Transportør skal udfylde firmaoplysninger";
        }

        if (_brugerBusiness.ProfileExists(brugerProfilModel.BrugerModel.Person.Email) &&
            !_securityProvider.CurrentUser.Identity.IsAuthenticated)
        {
            ModelState.AddModelError("BrugerModel.Person.Email", @"E-mail findes allerede i systemet.");
        }


        brugerProfilModel.MiljoeklasseTyper = _kodelisteBusiness.ReadAktiveMiljoeklasseTyper();

        if (!brugerProfilModel.BrugerModel.Company)
        {
            ModelState.Remove("BrugerModel.CVR");
        }


        //Jira 456; kun en bruger per transportør
        if (brugerProfilModel.BrugerModel.Transportoer && !_securityProvider.CurrentUser.Identity.IsAuthenticated && _transportoerBusiness.ReadTransportoer(brugerProfilModel.BrugerModel.CVR) != null)
        {
            ModelState.AddModelError("BrugerModel.CVR", @"Der findes allerede en transportør med dette CVR nummer.");
            detailedMsg = "En transportør med dette CVR nummer findes allerede i systemet.";
        }


        if (ModelState.IsValid)
        {
            if (_securityProvider.CurrentUser.Identity.IsAuthenticated)
            {
                brugerProfilModel.ErLoggetInd = true;
                brugerProfil = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);

                //Transportør fanen synlig eller ej
                brugerProfilModel.ShowTransportoerFane = brugerProfilModel.BrugerModel.Transportoer;


                brugerProfilModel.BackendUser = brugerProfil.Roles.Any();
                if (brugerProfilModel.BrugerModel.Password != null)
                {
                    brugerProfil.PasswordClearText = brugerProfilModel.BrugerModel.Password;
                    _securityProvider.ChangePassword(brugerProfil.BrugerNavn,
                        brugerProfilModel.BrugerModel.CurrentPassword, brugerProfil.PasswordClearText);
                }
                brugerProfil.BrugerNavn = brugerProfilModel.BrugerModel.Person.Email;

                if (!brugerProfilModel.BrugerModel.Transportoer)
                {
                    if (brugerProfil.Person.Transportoer != null)
                    {
                        foreach (Lastbil lastbil in brugerProfil.Person.Transportoer.Lastbil)
                        {
                            lastbil.Aktiv = false;
                        }
                        brugerProfil.Person.Transportoer.Aktiv = false;
                    }
                }
                else
                {
                    if (brugerProfil.Person.Transportoer == null || brugerProfil.Person.Transportoer.Id == Guid.Empty)
                    {
                        brugerProfil.Person.Transportoer = new Transportoer();
                        brugerProfil.Person.Transportoer.Aktiv = true;
                    }
                    else
                        brugerProfil.Person.Transportoer.Aktiv = true;
                }
                if (brugerProfilModel.BrugerModel.Company)
                {
                    if (brugerProfil.Person.Firmaoplysninger != null)
                    {
                        brugerProfil.Person.Firmaoplysninger.Adresse =
                            brugerProfilModel.BrugerModel.Firmaoplysninger.Adresse;
                        brugerProfil.Person.Firmaoplysninger.CVR = brugerProfilModel.BrugerModel.Firmaoplysninger.CVR;
                        brugerProfil.Person.Firmaoplysninger.Firmanavn =
                            brugerProfilModel.BrugerModel.Firmaoplysninger.Firmanavn;
                        brugerProfil.Person.Firmaoplysninger.PNummer =
                            brugerProfilModel.BrugerModel.Firmaoplysninger.PNummer;
                        brugerProfil.Person.Firmaoplysninger.EAN = brugerProfilModel.BrugerModel.Firmaoplysninger.EAN;
                        brugerProfil.Person.Firmaoplysninger.Postdistikt =
                            brugerProfilModel.BrugerModel.Firmaoplysninger.Postdistikt;
                        brugerProfil.Person.Firmaoplysninger.Postnummer =
                            brugerProfilModel.BrugerModel.Firmaoplysninger.Postnummer;
                    }
                    brugerProfil.Person.Firmaoplysninger = brugerProfilModel.BrugerModel.Firmaoplysninger;
                }
                else
                {
                    brugerProfil.Person.Firmaoplysninger = null;
                }
                brugerProfil.Person.Adresse = brugerProfilModel.BrugerModel.Person.Adresse;
                brugerProfil.Person.Efternavn = brugerProfilModel.BrugerModel.Person.Efternavn;
                brugerProfil.Person.Mobiltelefon = brugerProfilModel.BrugerModel.Person.Mobiltelefon;
                brugerProfil.Person.Navn = brugerProfilModel.BrugerModel.Person.Navn;
                brugerProfil.Person.Telefon = brugerProfilModel.BrugerModel.Person.Telefon;
                brugerProfil.Person.Email = brugerProfilModel.BrugerModel.Person.Email;
                brugerProfil.Person.Postnummer = brugerProfilModel.BrugerModel.Person.Postnummer;
                brugerProfil.Person.Postdistrikt = brugerProfilModel.BrugerModel.Person.Postdistrikt;
                brugerProfil.Person.By = brugerProfilModel.BrugerModel.Person.By;

                ////Advis og alarm

                _brugerBusiness.SaveChanges();
                brugerProfilModel.BrugerModel.Person = brugerProfil.Person;
                ViewBag.Message = "Brugeren er opdateret.";
            }
            else
            {
                //Brugeren er ikke authenticated - argo er det en ny bruger som er ved at oprette sig.

                var roller = new List<String>();

                Person person = brugerProfilModel.BrugerModel.Person;
                person.Aktiv = true;

                if (brugerProfilModel.BrugerModel.Transportoer)
                {
                    person.Transportoer = new Transportoer();
                    person.Transportoer.Aktiv = true;
                }
                if (brugerProfilModel.BrugerModel.Company)
                    person.Firmaoplysninger = brugerProfilModel.BrugerModel.Firmaoplysninger;

                // Alle bliver nød til at være oprettet som betaler som det er pt.
                person.Betaler = new Betaler();

                brugerProfil.BrugerNavn = brugerProfilModel.BrugerModel.Person.Email;
                brugerProfil.PasswordClearText = brugerProfilModel.BrugerModel.Password;
                brugerProfil.Person = person;

                //Advis og alarm
                brugerProfil.Person.KoertJordAlarm = 75; //Default ved opret.


                // Skal oprettes en ny bruger i systemet, derfor create metoden.
                _brugerBusiness.Create(brugerProfil, roller.ToArray());
                brugerProfil.BrugerId = _securityProvider.GetUserId(brugerProfil.Person.Email);
                if (brugerProfil.Person.Id != Guid.Empty && brugerProfil.BrugerId > 0)
                {
                    ViewBag.Message = "Brugeren er oprettet.";
                    ViewBag.DetailedMessage = "Brugeren er oprettet og du vil modtage en aktiveringsmail inden længe";
                    ViewBag.OnloadScript = "true";
                    return View(brugerProfilModel);
                }
                else
                {
                    if (brugerProfil.BrugerId > 0)
                    {
                        // Slet brugerprofilen hvis Person objektet ikke blev gemt korrekt i databasen.
                        _brugerBusiness.Delete(_brugerBusiness.Read(brugerProfil.BrugerNavn));
                    }
                    ViewBag.Message = "Der opstod en fejl.";
                    ViewBag.DetailedMessage =
                        "Der opstod en fejl. <br/>Brugeren er ikke oprettet.<br/>Tjek dine oplysninger og prøv igen.";

                   
                }
            }
        }
        else
        {
            if (detailedMsg == "")
                detailedMsg = "Et eller flere felter er ikke udfyldt korrekt.";
            ViewBag.DetailedMessage = detailedMsg;
            return View(brugerProfilModel);
        }
        ModelState.Clear();
        //return View(brugerProfilModel);

        return RedirectToAction("Profil");
    }

    public ActionResult UdskrivLastbil(string id)
    {
      return View(_lastbilBusiness.Read(new Guid(id)));
    }

    public ActionResult SaveAdvis(BrugerProfilModel model)
    {
      var person = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name).Person;
      person.FrivilligeAdvis = model.OenskerFrivilligAdvis;
      if (model.OenskerAlarmer)
        person.KoertJordAlarm = model.AlarmJordmaengdeIProcent;
      else
        person.KoertJordAlarm = null;

       person.JordmodtagerAdvis = model.BrugerModel.Person.JordmodtagerAdvis;

      person.FrivilligeAdvis = model.OenskerFrivilligAdvis;
      _personBusiness.SaveChanges();
      ViewBag.Message = "Brugeren er opdateret.";
      ModelState.Clear();
      return RedirectToAction("Profil");
    }

    public ActionResult VisPerson(Guid personId)
    {
      var m = new PersonModel();
      if (personId != Guid.Empty)
      {
        var p = _personBusiness.Read(personId);
        m.Person = p;
      }

      return View(m);
    }

    [AllowAnonymous]
    public ActionResult Aktiver(string id)
    {
      if (WebSecurity.ConfirmAccount(id))
      {
        ViewBag.DetailedMessage = "Bruger aktiveret! <br/> Du vil blive sendt til forsiden hvor du nu kan logge ind.";
        return View();
      }
      ViewBag.DetailedMessage = "Brugeren kunne IKKE aktiveres.<br/>";
      return View();
    }

    #region JordmodtagerGrid

    [HttpPost]
    public ActionResult TilfoejJordmodtager(Object obj)
    {
      var currentUser = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name).Person;
      if (currentUser.Betaler == null)
        currentUser.Betaler = new Betaler();
      var jordmodtagerId = Guid.Parse((string)obj);
      var statusBetaler = new StatusBetaler();
      statusBetaler.JordmodtagerId = jordmodtagerId;

      // "jordmodtagerId" er et modtageranlægId. 	
      // Skal lige have sat JordmodtagerId på Modtageranlægmodel.
      statusBetaler.BetalerId = currentUser.Betaler.Id;
      statusBetaler.Redigeret = DateTime.Now;
      currentUser.Betaler.StatusBetaler.Add(statusBetaler);
      _brugerBusiness.SaveChanges();

      return Json(obj);
    }

    public ActionResult GridFiltredeJordmodtagereRead([DataSourceRequest] DataSourceRequest request)
    {
      if (!_securityProvider.CurrentUser.Identity.IsAuthenticated)
      {
        //brugeren er ikke logget ind/oprettet return tom liste? <-- giver i hvert fald ikke en fejl
        return Json(new List<ModtagerAnlaegModel>());
      }
      var modtageranlaeg = AnmeldelserController.ConvertModtagerAnlaegToModel(_modtagerAnlaegBusiness.ReadAktiveModtagerAnlaeg().Where(x => x.AnvenderJF).ToList(), null).ToList();
      var result = modtageranlaeg.ToDataSourceResult(request);
      return Json(result);
    }

    [ValidateInput(false)]
    public ActionResult MineJordmodtagereDestroy([DataSourceRequest] DataSourceRequest request, ModtagerAnlaegModel modtagerAnlaegModel)
    {
      if (modtagerAnlaegModel != null)
      {
        //hent alle modtageranlæg som har samme jordmodtagerId som den slettede række.			
        var brugerProfil = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
        //slet de fundne
        _statusBetalerRepository.Delete(brugerProfil.Person.Betaler.StatusBetaler.FirstOrDefault(x => x.JordmodtagerId == modtagerAnlaegModel.JordmodtagerId));
        _brugerBusiness.SaveChanges();
      }
      return Json("delete");
    }

    public ActionResult MineJordmodtagereRead([DataSourceRequest] DataSourceRequest request)
    {
      if (!_securityProvider.CurrentUser.Identity.IsAuthenticated)
      {
        //brugeren er ikke logget ind/oprettet return tom liste? <-- giver i hvert fald ikke en fejl
        return Json(new List<ModtagerAnlaegModel>());
      }

      var modtageranlaeg = GetMineModtageranlaeg();
      var result = modtageranlaeg.Where(x=>x.JF).ToDataSourceResult(request);
      return Json(result);
    }

    #endregion

    #region TransportoerGrid

    public ActionResult CustomCommandRead([DataSourceRequest] DataSourceRequest request)
    {
      if (!_securityProvider.CurrentUser.Identity.IsAuthenticated)
      {
        //brugeren er ikke logget ind/oprettet return tom liste? <-- giver i hvert fald ikke en fejl
        return Json(new List<Lastbil>());
      }

      var brugerProfil = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
      if (brugerProfil != null && brugerProfil.Person != null && brugerProfil.Person.Transportoer != null)
      {
        return Json(brugerProfil.Person.Transportoer.Lastbil.OrderByDescending(x => x.Aktiv).ThenBy(x => x.Nummerplade).Select(x => new LastbilModel(x)).ToDataSourceResult(request));
      }
      return Json(new List<Lastbil>());
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult CustomCommandCreate([DataSourceRequest] DataSourceRequest request, LastbilModel lastbil)
    {
      // Sæt transportoerID
      var gemtLastbil = lastbil.ToLastbil();
      var brugerProfil = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
      gemtLastbil.TransportoerId = brugerProfil.Person.Transportoer.Id;

      gemtLastbil.Redigeret = DateTime.Now;
      if (ModelState.IsValid)
        _lastbilBusiness.Create(gemtLastbil);
      lastbil.TransportoerId = gemtLastbil.TransportoerId;
      lastbil.Id = gemtLastbil.Id;
      return Json(new[] { lastbil }.ToDataSourceResult(request, ModelState));
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult CustomCommandUpdate([DataSourceRequest] DataSourceRequest request, LastbilModel lastbilModel)
    {
      if (lastbilModel != null && ModelState.IsValid && lastbilModel.Id != Guid.Empty)
      {
        var lastbil = _lastbilBusiness.Read(lastbilModel.Id);
        if (lastbil != null)
        {
          lastbil.Redigeret = DateTime.Now;
          lastbil.Bemærkning = lastbilModel.Bemærkning;
          lastbil.Aktiv = lastbilModel.Aktiv;
          lastbil.Nummerplade = lastbilModel.Nummerplade;
          lastbil.Fabrikat = lastbilModel.Fabrikat;
          lastbil.MiljoeklasseTypeId = lastbilModel.MiljoeklasseTypeId;
          _lastbilBusiness.SaveChanges();
        }
      }
      return Content(ModelState.ToString());
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult CustomCommandDestroy([DataSourceRequest] DataSourceRequest request, LastbilModel lastbilModel)
    {
      if (lastbilModel != null)
        _lastbilBusiness.Delete(_lastbilBusiness.Read(lastbilModel.Id));

      return Json(ModelState.ToDataSourceResult());
    }

    #endregion

    #region Bemyndigede anmeldere

    [HttpPost]
    public ActionResult TilfoejBemyndigedeAnmeldere([DataSourceRequest] DataSourceRequest request, string anmelderId)
    {
      Guid g;
      if (Guid.TryParse(anmelderId, out g))
      {
        var p = _personBusiness.Read(g);

        //if (Session["BemyndigedeAnmeldere"] == null)
        //  Session["BemyndigedeAnmeldere"] = new List<Person>();

        if (Session["BemyndigedeAnmeldere"] != null)
        {
          //var pSession = (List<Person>)Session["BemyndigedeAnmeldere"];

          //Henter bemyndigedeanmeldere for personen som er logget ind.
          var brugerProfil = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);


          if (brugerProfil != null && brugerProfil.Person != null && brugerProfil.Person.Betaler != null && brugerProfil.Person.Betaler.BemyndigedeAnmeldere != null)
          {
            var bemyndigedeAnmeldere = (from pp in brugerProfil.Person.Betaler.BemyndigedeAnmeldere
                                        let anmelder = pp.Anmelder
                                        where anmelder != null
                                        select anmelder.Person).ToList();

            if (p != null && !bemyndigedeAnmeldere.Contains(p))
            {
              var currentPersonGuid = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name).Person.Id;
              //pSession.Add(p);
              //Session["BemyndigedeAnmeldere"] = pSession;

              _bemyndigedeAnmelderBusiness.AddBemyndigedeAnmelder(p.Id, currentPersonGuid);

              return Json("true");
            }
          }
        }
      }
      return null;
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult RemoveBemyndigedeAnmeldere([DataSourceRequest] DataSourceRequest request, BemyndigetAnmelderModel bemyndigetAnmelderModel)
    {
      var b = new List<Person>();

      var p = _personBusiness.Read(bemyndigetAnmelderModel.Id);
      //if (Session["BemyndigedeAnmeldere"] == null)
      //  Session["BemyndigedeAnmeldere"] = new List<Person>();

      //if (Session["BemyndigedeAnmeldere"] != null)
      //{
      //var pSession = (List<Person>) Session["BemyndigedeAnmeldere"];

      if (p != null)
      {
        b.Remove(p);
        //Session["BemyndigedeAnmeldere"] = pSession;
        var currentPersonGuid = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name).Person.Id;
        _bemyndigedeAnmelderBusiness.RemoveBemyndigedeAnmelder(p.Id, currentPersonGuid);
        var pModel = ConvertPersonListToModelList(b);
        var result = pModel.ToDataSourceResult(request);
        return Json(result);
        //}
      }
      return Json(null);
    }

    [HttpPost]
    public ActionResult ReadBemyndigedeAnmeldere([DataSourceRequest] DataSourceRequest request)
    {
	    if (_securityProvider.CurrentUser.Identity.IsAuthenticated)
	    {
		    var brugerProfil = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
		    var person = brugerProfil.Person;

		    if (person != null && person.Betaler != null && person.Betaler.BemyndigedeAnmeldere != null)
		    {
			    var persons = (from p in person.Betaler.BemyndigedeAnmeldere
			                   let anmelder = p.Anmelder
			                   where anmelder != null
			                   select anmelder.Person).ToList();

			    var pModel = ConvertPersonListToModelList(persons);
			    var result = pModel.ToDataSourceResult(request);
			    return Json(result);
		    }
	    }

	    return Json(null);

    }

    #endregion

    #endregion

    #region Private methods

    private static void GetAdvisOgAlarm(Person person, BrugerProfilModel brugerProfilModel)
    {
      //Advisering og alarm
      if (person.KoertJordAlarm.HasValue)
      {
        brugerProfilModel.OenskerAlarmer = true;
        brugerProfilModel.AlarmJordmaengdeIProcent = person.KoertJordAlarm.Value;
      }
      else
      {
        brugerProfilModel.OenskerAlarmer = false; //Som default skal brugeren have personlig besked, hvis tilkørt jord overskrider 75%. Flere brugere forstår ikke de ikke får besked herom - men det skyldes at de ikke har krydset dette flueben af. Og som ny bruger forstår man ikke advis reglerne...
        brugerProfilModel.AlarmJordmaengdeIProcent = null; //Default værdi
      }
      brugerProfilModel.OenskerFrivilligAdvis = person.FrivilligeAdvis;
    }

    private void GetBemyndigedeAnmeldere(Person person)
    {
      //Bemyndigede anmeldere
      if (person != null && person.Betaler != null && person.Betaler.BemyndigedeAnmeldere != null)
      {
        var persons = (from p in person.Betaler.BemyndigedeAnmeldere
                       let anmelder = p.Anmelder
                       where anmelder != null
                       select anmelder.Person).ToList();
        Session["BemyndigedeAnmeldere"] = persons;
      }
      else
        Session["BemyndigedeAnmeldere"] = new List<Person>();
    }

    private static void GetMineModtagerAnlaeg(BrugerProfilModel brugerProfilModel)
    {
      if (brugerProfilModel.BrugerModel.Person.Betaler != null)
      {
        brugerProfilModel.MineModtagerAnlaeg = (
          brugerProfilModel.FiltreredeModtagerAnlaeg
          .Where(x => brugerProfilModel.BrugerModel.Person.Betaler.StatusBetaler.FirstOrDefault(
            m => m.JordmodtagerId == x.JordmodtagerId) != null))
          .ToList();
      }
      else
        brugerProfilModel.MineModtagerAnlaeg = new List<ModtagerAnlaegModel>();
    }

    private static void GetTransportoer(BrugerProfilModel brugerProfilModel, Person person)
    {
      if (brugerProfilModel.BrugerModel.Person.Transportoer != null && brugerProfilModel.BrugerModel.Person.Transportoer.Aktiv)
      {
        brugerProfilModel.TransportoerModel.MineLastbiler =
          brugerProfilModel.BrugerModel.Person.Transportoer.Lastbil.OrderByDescending(x => x.Aktiv).ThenBy(x => x.Nummerplade).Select(x => new LastbilModel(x)).ToList();
      }

      brugerProfilModel.BrugerModel.Transportoer =
        (person.Transportoer != null && person.Transportoer.Id != Guid.Empty && person.Transportoer.Aktiv);

      if (person.Firmaoplysninger != null)
        brugerProfilModel.BrugerModel.Firmaoplysninger = person.Firmaoplysninger;

      brugerProfilModel.BrugerModel.Company = (person.Firmaoplysninger != null && person.Firmaoplysninger.Id != Guid.Empty);
    }

    private static IList<BetalerModel> ConvertPersonListToModelList(IList<Person> persons)
    {
      var pp = persons.ToList();
      var res = new List<BetalerModel>();
      foreach (var p in pp)
      {
        var pm = ConvertPersonToModel(p);
        res.Add(pm);
      }

      return res;
    }

    private static BetalerModel ConvertPersonToModel(Person p)
    {
      var betModel = new BetalerModel
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
      return betModel;
    }

    private IList<ModtagerAnlaegModel> GetMineModtageranlaeg()
    {
      IList<ModtagerAnlaegModel> mineModtageranlaeg = new List<ModtagerAnlaegModel>();
      var person = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name).Person;
      if (person.Betaler != null)
      {
        mineModtageranlaeg =
          AnmeldelserController.ConvertModtagerAnlaegToModel(
            _modtagerAnlaegBusiness.ReadAktiveModtagerAnlaeg()
                                   .Where(x => person.Betaler.StatusBetaler.FirstOrDefault(m => m.JordmodtagerId == x.JordmodtagerId) != null)
                                   .ToList(), null).ToList();
      }
      return mineModtageranlaeg;
    }

    #endregion

  }
}
