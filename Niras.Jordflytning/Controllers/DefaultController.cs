using DocumentFormat.OpenXml.Presentation;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using Niras.Jordflytning.Entities;
using Niras.Jordflytning.ViewModels;
using Niras.Jordflytning.ViewModels.Anmeldelse;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace Niras.Jordflytning.Controllers
{
	[Authorize]
	//[InitializeSimpleMembership]
	public class DefaultController : Controller
	{
		#region Variables

		private readonly ISecurityProvider _securityProvider;
		private readonly IKodelisteBusiness _kodelisteBusiness;
		private readonly IKonfigBusiness _konfigBusiness;
		private readonly IStatusAnmeldelseBusiness _statusAnmeldelseBusiness;
		private readonly IAnmeldelserBusiness _anmeldelserBusiness;
		private readonly ISoegBusiness _soegBusiness;
		private readonly IBrugereBusiness _brugereBusiness;
		private readonly IAdviseringBusiness _adviseringBusiness;
		private readonly IKommunikationBusiness _kommunikationBusiness;
		private readonly IModtagerAnlaegBusiness _ModtagerAnlaegBusiness;

		#endregion

		#region Constructors

		public DefaultController(ISecurityProvider securityProvider, IKodelisteBusiness kodelisteBusiness, IKonfigBusiness konfigBusiness,
								 IStatusAnmeldelseBusiness statusAnmeldelseBusiness, IAnmeldelserBusiness anmeldelserBusiness, ISoegBusiness soegBusiness,
								 IBrugereBusiness brugereBusiness, IAdviseringBusiness adviseringBusiness, IKommunikationBusiness kommunikationBusiness, IModtagerAnlaegBusiness modtagerAnlaegBusiness)
		{
			_securityProvider = securityProvider;
			_kodelisteBusiness = kodelisteBusiness;
			_konfigBusiness = konfigBusiness;
			_statusAnmeldelseBusiness = statusAnmeldelseBusiness;
			_anmeldelserBusiness = anmeldelserBusiness;
			_soegBusiness = soegBusiness;
			_brugereBusiness = brugereBusiness;
			_adviseringBusiness = adviseringBusiness;
			_ModtagerAnlaegBusiness = modtagerAnlaegBusiness;
			_kommunikationBusiness = kommunikationBusiness;
		}

		#endregion

		[AllowAnonymous]
		public ActionResult FrontPage(string returnUrl)
		{
			_securityProvider.Logout();
			var m = new LoginModel();
			m = InitModel(m);

			ViewBag.DetailedMessage = TempData["detailedMessage"];
			ViewBag.ReturnUrl = returnUrl;
			return View(m);
		}

		[AllowAnonymous]
		public ActionResult HistorikArkiv(int id)
		{
			try
			{
                var filnavn = $"{id}.zip";
                var mappe = ConfigurationManager.AppSettings["ZipArchivePath"];
                var target = Path.Combine(mappe, filnavn);
                var contentType = MimeMapping.GetMimeMapping(filnavn);
                if (System.IO.File.Exists(target))
                    return File(target, contentType, filnavn);
            }
			catch
			{
            }
            return HttpNotFound();
        }

        [AllowAnonymous]
        public ActionResult Blanketter(Guid id)
        {
            try
            {
				var filnavn = $"{id}.pdf";
				var mappe = ConfigurationManager.AppSettings["BlanketBaseFileDir"];
                var target = Path.Combine(mappe, filnavn);
                var contentType = MimeMapping.GetMimeMapping(filnavn);
                if (System.IO.File.Exists(target))
                    return File(target, contentType);
            }
            catch
            {
            }
            return HttpNotFound();
        }

        [AllowAnonymous]
        public JsonResult MarkdownNyheder()
		{
			var pages = new List<dynamic>();
			string error = null;
			try
			{
                var rootPath = ConfigurationManager.AppSettings["NyhederFileDir"] ?? "";
				if (rootPath.Contains("|DataDirectory|"))
					rootPath = rootPath.Replace("|DataDirectory|", (string)AppDomain.CurrentDomain.GetData("DataDirectory"));

                var parser = new MarkdownWeb.PageParser(rootPath, $"https://{ConfigurationManager.AppSettings["FlytjordDomain"]}");
                var dir = new DirectoryInfo(rootPath);
                var files = dir.GetFiles("*.md").OrderByDescending(x => x.CreationTime);
				foreach (var file in files)
				{
					if (file.Name.StartsWith("_"))
						continue;

					var pageBuilder = new StringBuilder();
					var lines = System.IO.File.ReadAllLines(file.FullName);
					var hasText = false;
					foreach (var line in lines)
					{
						// Strip empty top lines
						if (!hasText && line.Trim() == string.Empty)
							continue;
						pageBuilder.AppendLine(line);
					}
                    var page = parser.ParseString(pageBuilder.ToString());
					
					// Strip the title from the body (The first line)
                    var body = string.Join("\n", page.Body.Split(new char[] { '\n' }).Skip(1).ToArray());
					var title = page.Title;
					var time = file.CreationTime;

					// TODO: Check for "_1" etc.
					var baseName = file.Name.Substring(0, file.Name.Length - file.Extension.Length);
					var important = false;
					if (baseName.Contains('!'))
					{
						important = true;
						baseName = baseName.Replace("!", "");
					}

					var fileIndex = 0;
					if (baseName.Contains('_'))
					{
						var nameParts = baseName.Split('_');
						baseName = nameParts[0];
						if (nameParts.Length == 2)
						{
							if (!int.TryParse(nameParts[1], out fileIndex))
							{
								// Whatever, use the numeric value of the chars
								foreach (var c in nameParts[1].Trim())
									fileIndex += c;
							}
						}
					}

					if (DateTime.TryParse(baseName, out var fileDate))
						time = fileDate;

					// Spring over fremtidige nyheder
					if (time > DateTime.Now)
						continue;

					pages.Add(new { time, title, body, important, fileIndex });
                }
				pages.Sort((a, b) => { 
					var result = a.time.CompareTo(b.time);
					if (result == 0)
						return a.fileIndex.CompareTo(b.fileIndex);
					return result;
				});
            }
            catch (Exception ex) 
			{
				error = ex.ToString();
			}
			return Json(new { pages = pages.Select(x => new { x.time, x.title, x.body, x.important }), error }, JsonRequestBehavior.AllowGet);
		}

        [AllowAnonymous]
        public ActionResult HjaelpVejledninger()
        {
            return View();
        }

		#region Mobile

		[HttpPost]
		public ActionResult ReadAnmeldelse(Guid id)
		{
			return Json(_anmeldelserBusiness.Read(id), JsonRequestBehavior.AllowGet);
		}


		[HttpGet]
		[AllowAnonymous]
		public ActionResult MobileLoginPage()
		{
			var model = new LoginModel();
			return View(model);
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult MobileLoginPage(string brugernavn, string password, string returnUrl = null)
		{
            if (!WebSecurity.IsConfirmed(brugernavn)) // Er brugeren aktiveret
                if (!Membership.ValidateUser(brugernavn, password))
                    ModelState.AddModelError("", @"Log på fejlede. Tjek venligst dit brugenavn og password og prøv igen.");
                else
                    ModelState.AddModelError("", @"Brugeren er ikke aktiveret. Aktivér med den mail der er sendt til brugeren.");
            else
            {
                if (!Membership.ValidateUser(brugernavn, password))
                    ModelState.AddModelError("", @"Log på fejlede. Tjek venligst dit brugenavn og password og prøv igen.");
                else
                {
                    if (!String.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("MobileMinSide", "Default");
                }
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", @"Log på fejlede.Prøv venligst igen.");
            return View();
		}

		public ActionResult MobileMinSide()
		{
			//var brugerId = _brugereBusiness.Read(_securityProvider.CurrentUser.Identity.Name).Person.Id;
			//brugerId = new Guid("EC388BED-7884-452C-84DD-B7CCC29B69CA");
			//var model = new MobileMinSideModel();
			//model.AfsluttedeAnmeldelser =
			//	_soegBusiness.GetAfsluttedeAnmeldelser(brugerId, false)
			//							 .Select(x => new MobileAnmeldelseListModel {Id = x.AnmeldelseId, Status = x.SenesteStatus, Visningstekst = x.OprindelsesSted})
			//							 .ToList();
			//model.AktiveAnmeldelser =
			//	_soegBusiness.GetAktiveAnmeldelser(brugerId, false)
			//							 .Select(x => new MobileAnmeldelseListModel {Id = x.AnmeldelseId, Status = x.SenesteStatus, Visningstekst = x.OprindelsesSted})
			//							 .ToList();
			//model.FremsendteAnmeldelser =
			//	_soegBusiness.GetFremSendteAnmeldelser(brugerId, false)
			//							 .Select(x => new MobileAnmeldelseListModel {Id = x.AnmeldelseId, Status = x.SenesteStatus, Visningstekst = x.OprindelsesSted})
			//							 .ToList();
			//model.IkkeFremsendteAnmeldelser =
			//	_soegBusiness.GetIkkeFremSendteAnmeldelser(brugerId, false)
			//							 .Select(x => new MobileAnmeldelseListModel {Id = x.AnmeldelseId, Status = x.SenesteStatus, Visningstekst = x.OprindelsesSted})
			//							 .ToList();

			//return View(model);
			return View();
		}

		#endregion Mobile

		[HttpPost]
		[AllowAnonymous]
#if RELEASE
        [ValidateAntiForgeryToken]
#endif
		public ActionResult FrontPage(LoginModel model, string returnUrl = null)
		{
			model = InitModel(model);

            if (!WebSecurity.IsConfirmed(model.UserName)) // Er brugeren aktiveret
                if (!Membership.ValidateUser(model.UserName, model.Password))
                    ModelState.AddModelError("", @"Log på fejlede. Tjek venligst dit brugenavn og password og prøv igen.");
                 else
				    ModelState.AddModelError("", @"Brugeren er ikke aktiveret. Aktivér med den mail der er sendt til brugeren.");
			else
			{
                if (!Membership.ValidateUser(model.UserName, model.Password))
                    ModelState.AddModelError("", @"Log på fejlede. Tjek venligst dit brugenavn og password og prøv igen.");
                else
                {
				    if (ModelState.IsValid && _securityProvider.Login(model.UserName, model.Password, model.RememberMe))
				    {
					    var userRoles = _securityProvider.GetUserRoles(model.UserName);
					    return RedirectToLocal(returnUrl, userRoles);
				    }
				    // If we got this far, something failed, redisplay form
                    ModelState.AddModelError("", @"Log på fejlede.Prøv venligst igen."); 
                }
			}
			return View(model);
		}

		public ActionResult ContentPage()
		{
			var model = new LoginModel();
			if (_securityProvider.CurrentUser.IsInRole("Leverandoer"))
			{
				var anmMenu = new MenuItem("Anmeldelser");
				var anmSubItem1 = new MenuItem();
				anmSubItem1.Text = "Opret anmeldelse";
				anmSubItem1.Url = "/Anmeldelser/Opret";
				anmMenu.AddSubItem(anmSubItem1);
				model.Menu.AddMenuItem(anmMenu);
				var soegMenu = new MenuItem("Søg");
				model.Menu.AddMenuItem(soegMenu);
			}
			return View(model);
		}

		public ActionResult Startside()
		{
		  var soegModel = new SoegModel();
      var b = _brugereBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
      var brugerId = b.Person.Id;
		   

		  var alarmerendeAnmeldelser = _soegBusiness.GetAnmelderAlarmAnmeldelser(brugerId);
		  soegModel.SoegeResultatList = alarmerendeAnmeldelser;

			return View(soegModel);
		}

		[HttpPost]
		[AllowAnonymous]
		public ActionResult Resetpassword(LoginModel model)
		{
            if (String.IsNullOrEmpty(model.ResetEmailAddress))
            {
                InitModel(model);
                model.MessageToUser = "Der skal angives en e-mail adresse";
            }
            else
            {

                //Tjek at det er en gyldig email som findes i systemet inden vi prøver at nulstille passwordet.
                var validUser = _securityProvider.GetUserId(model.ResetEmailAddress) > 0;

                model = InitModel(model);
                if (validUser)
                {
                    var generatedPassword = _securityProvider.GenerateNewPassword(model.ResetEmailAddress);
                    if (generatedPassword != "")
                    {
                        _adviseringBusiness.SendGlemtPassword(model.ResetEmailAddress, generatedPassword);
                        model.MessageToUser = "Der er sendt en mail med dit nye password";
                        return View("FrontPage", model);
                        //return RedirectToAction("FrontPage", model);
                    }
                    else
                    {
                        model.MessageToUser = "Der opstod en fejl.";
                    }
                }
                else
                {
                    model.MessageToUser = "Emailadressen findes ikke i systemet.";
                }
            }
			return View("FrontPage",model);
		}

		/// <summary>
		/// Logs out current user.
		/// </summary>
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			_securityProvider.Logout();
			return RedirectToAction("FrontPage", "Default", new {area = ""});
		}

		[AllowAnonymous]
		public ActionResult TjekEjendom()
		{
			var model = new OpretModel();
			var appSettings = System.Configuration.ConfigurationManager.AppSettings;
			model.KortApiUrl = appSettings["KortApiUrl"];
			model.KortPageSted = appSettings["KortPageSted"];
			model.KortSite = appSettings["KortSite"];
			return View(model);
		}

		[AllowAnonymous]
		public ActionResult Kvittering()
		{
			var model = new OpretModel();
			if (Session["anmeldelse_kvittering"] != null)
			{
				model = (OpretModel)  Session["anmeldelse_kvittering"];
			}
			ModelState.Clear();
			return View(model);
		}

#region Gateway

		[AllowAnonymous]
		public ActionResult Gateway(string g1, string handling, string g2)
		{
			var model = new GateWayModel();
			Guid ga; //anmeldelse
			Guid gb; //betaler
			if (Guid.TryParse(g1, out ga) && Guid.TryParse(g2, out gb))
			{
				if (handling == EnumStatusAnmeldelse.BetalerAccepteretBetalingen.ToString())
				{
					var a = _anmeldelserBusiness.Read(ga);
					if (a != null && a.Betaler != null && a.Betaler.Id == gb)
					{

						_anmeldelserBusiness.SetAnmeldelseStatus(a.Id, a.Betaler.Person, EnumStatusAnmeldelse.BetalerAccepteretBetalingen);

						//Frigiver anmeldelse hvis det er muligt.
						if (!_statusAnmeldelseBusiness.IsAnmeldelseAktiv(a) && _statusAnmeldelseBusiness.IsAnmeldelseReadyToBeAktiv(a))
						{
							_anmeldelserBusiness.AktiverAnmeldelse(a.Id, null);
						}

						model.GateWayHeader = "Betaling for jordflytning";
						model.GateWayMessage = "Du har accepteret at betale for jordflytningen";
						return View(model);
					}

				}
				else if (handling == EnumStatusAnmeldelse.BetalerAfviserBetalingen.ToString())
				{
					var a = _anmeldelserBusiness.Read(ga);
					if (a != null && a.Betaler != null && a.Betaler.Id == gb)
					{
						_anmeldelserBusiness.SetAnmeldelseStatus(a.Id, a.Betaler.Person, EnumStatusAnmeldelse.BetalerAfviserBetalingen);

						model.GateWayHeader = "Betaling for jordflytning";
						model.GateWayMessage = "Du har afvist at betale for jordflytningen";
						return View(model);
					}

				}
			}

			model.GateWayHeader = "Fejl";
			model.GateWayMessage = "Handlingen er ikke understøttet af denne gateway";

			return View(model);
		}

        [AllowAnonymous, HttpGet]
        public ActionResult AndenKommuneSvar(Guid id, string afsenderEmail, string modtagerEmail)
        {
            ViewBag.Info = "<b>Godkend eller afvis modtagelse af jord</b><br /><br />I nedenstående bedes I angive om jordlytningen kan accepteres eller afvises. Endvidere har du mulighed for at indsætte eventuelle bemærkninger i tekstfeltet.<br />Såfremt der vælges \"Afvis\" skal der skrives en bemærkning.";
            //ViewBag.Info = "Kan det midlertidige modtageanlæg på adressen Knæhøj 4, 4000 Roskilde godkendes vælges Indsend. Hvis ikke vælges \"Afvis midlertidigt modtageanlæg\" og der skrives begrundelse for afvisningen i besked feltet, hvor efter der vælges Indsend";
            ViewBag.Godkendt = true;
            ViewBag.AnmeldelseId = id;
            ViewBag.AfsenderEmail = afsenderEmail;
            ViewBag.ModtagerEmail = modtagerEmail;
            //ViewBag.AfsenderId = afsenderId;
            //ViewBag.ModtagerId = modtagerId;
            //ViewBag.txtBesked = besked;

            return View();
        }

        [AllowAnonymous, HttpPost]
        public ActionResult AndenKommuneSvar(FormCollection form)
        {
            Boolean bGodkendt = form["Godkend"] == "Godkend midlertidigt modtageanlæg";
            string sBesked = form["txtBesked"];
            Guid anmeldelseId = Guid.Parse(form["AnmeldelseId"]);
            string sAfsenderEmail = form["AfsenderEmail"];
            string sModtagerEmail = form["ModtagerEmail"];
            //Guid afsenderId = Guid.Parse(form["AfsenderId"]);
            //Guid modtagerId = Guid.Parse(form["ModtagerId"]);

            ViewBag.Info = "<b>Godkend eller afvis modtagelse af jord</b><br /><br />I nedenstående bedes I angive om jordlytningen kan accepteres eller afvises. Endvidere har du mulighed for at indsætte eventuelle bemærkninger i tekstfeltet.<br />Såfremt der vælges \"Afvis\" skal der skrives en bemærkning.";
            //ViewBag.Info = "Kan det midlertidige modtageanlæg på adressen Knæhøj 4, 4000 Roskilde godkendes vælges Indsend. Hvis ikke vælges \"Afvis midlertidigt modtageanlæg\" og der skrives begrundelse for afvisningen i besked feltet, hvor efter der vælges Indsend";
            ViewBag.AnmeldelseId = anmeldelseId;
            ViewBag.AfsenderEmail = sAfsenderEmail;
            ViewBag.ModtagerEmail = sModtagerEmail;
            ViewBag.Afvist = !bGodkendt;


            if (!bGodkendt && string.IsNullOrWhiteSpace(sBesked))
            {
                ModelState.AddModelError("txtBesked", "Bemærkning skal indtastes.");
                return View(form);
            }

            var a = _anmeldelserBusiness.Read(anmeldelseId);
            _ModtagerAnlaegBusiness.UpdateModtageAnlaegAktiv(a.ModtagerAnlaegId.Value, true);

            var res = _adviseringBusiness.AndenKommuneSvar(bGodkendt, sBesked, sModtagerEmail, sAfsenderEmail, anmeldelseId);
            
            ViewBag.Result = res;
            
            return View(form);
        }

      [AllowAnonymous]
	  public ActionResult Error()
	  {

	    return View();
	  }

#endregion

#region Helpers

		private ActionResult RedirectToLocal(string returnUrl, IList<string> userRoles)
		{
			if (!String.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			if (userRoles.Any())
			{
				return RedirectToAction("Index", "DefaultBackend", new {area = "Backend"});
			}

			return RedirectToAction("Startside", "Default");
		}

		private LoginModel InitModel(LoginModel m)
		{
			var kontaktInfoListe = new List<SelectListItem>();
			var k = _kodelisteBusiness.ReadAktiveKommune();
			foreach (var kommune in k)
			{
				var kontaktInfo = _konfigBusiness.ReadKommuneKonfig("KontaktInfo", kommune.Id);
				kontaktInfoListe.Add(new SelectListItem {Selected = false, Text = kommune.Navn, Value = kontaktInfo});
			}
			m.KommuneKontaktListe = kontaktInfoListe;
			return m;

		}

#endregion

#region Sandbox

		// GET: /Default/Sandbox
		[AllowAnonymous]
		public ActionResult Sandbox()
		{
			return View();
		}

		// POST: /Default/Sandbox
		[AllowAnonymous]
		[HttpPost]
		public ActionResult Sandbox(SandboxViewModel model)
		{
			return View(model);
		}

		// POST: /Default/SandboxPartialUpdate
		[AllowAnonymous]
		[HttpPost]
		public ActionResult SandboxPartialUpdate(SandboxViewModel model)
		{
			ModelState.Clear();
			model.Value = DateTime.Now.ToString(CultureInfo.InvariantCulture);
			return PartialView("_SandboxPartial", model);
		}

#endregion

	}
}
