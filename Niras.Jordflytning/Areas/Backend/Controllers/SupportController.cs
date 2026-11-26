using System;
using System.Web.Mvc;
using Niras.Jordflytning.Areas.Backend.ViewModels;
using Niras.Jordflytning.Core;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;

namespace Niras.Jordflytning.Areas.Backend.Controllers
{
    /// <summary>
    /// Supportfunktioner der kun er tilgængelige for MasterAdmin: skift email og aktivér/deaktivér bruger.
    /// </summary>
    [Authorize(Roles = ApplicationConstants.MasterAdminRolle)]
    public class SupportController : Controller
    {
        private readonly IBrugerSupportBusiness _brugerSupportBusiness;
        private readonly ISecurityProvider _securityProvider;

        public SupportController(IBrugerSupportBusiness brugerSupportBusiness, ISecurityProvider securityProvider)
        {
            _brugerSupportBusiness = brugerSupportBusiness;
            _securityProvider = securityProvider;
        }

        [HttpGet]
        public ActionResult SkiftEmail(string email)
        {
            var model = new SkiftEmailModel { SoegEmail = email };

            if (!string.IsNullOrWhiteSpace(email))
            {
                model.ErSoegt = true;
                model.Bruger = _brugerSupportBusiness.HentBruger(email);
                if (model.Bruger != null)
                {
                    model.FraEmail = model.Bruger.BrugerNavn;
                    model.OpdaterKontaktpersonEmail = model.Bruger.ModtageanlaegMedKontaktpersonEmail.Count > 0;
                }
            }

            if (TempData["Besked"] != null)
            {
                model.Besked = (string)TempData["Besked"];
                model.BeskedErFejl = TempData["BeskedErFejl"] as bool? ?? false;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("SkiftEmail")]
        public ActionResult SkiftEmailPost(SkiftEmailModel model)
        {
            var udfoertAf = _securityProvider.CurrentUser.Identity.Name;

            if (string.Equals((model.FraEmail ?? "").Trim(), udfoertAf, StringComparison.OrdinalIgnoreCase))
                return Tilbage(model.FraEmail, "Du kan ikke skifte din egen email her.", true);

            if (!ModelState.IsValid)
                return Tilbage(model.FraEmail, "Ny email er ikke gyldig (skal være en emailadresse på højst 40 tegn).", true);

            var resultat = _brugerSupportBusiness.SkiftEmail(model.FraEmail, model.TilEmail, model.OpdaterKontaktpersonEmail, udfoertAf);

            return Tilbage(resultat.Succes ? model.TilEmail : model.FraEmail, resultat.Besked, !resultat.Succes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaetAktiveret(string email, bool aktiveret)
        {
            var udfoertAf = _securityProvider.CurrentUser.Identity.Name;

            if (string.Equals((email ?? "").Trim(), udfoertAf, StringComparison.OrdinalIgnoreCase))
                return Tilbage(email, "Du kan ikke ændre aktivering på dig selv her.", true);

            var resultat = _brugerSupportBusiness.SaetAktiveret(email, aktiveret, udfoertAf);
            return Tilbage(email, resultat.Besked, !resultat.Succes);
        }

        private ActionResult Tilbage(string email, string besked, bool erFejl)
        {
            TempData["Besked"] = besked;
            TempData["BeskedErFejl"] = erFejl;
            return RedirectToAction("SkiftEmail", new { email = email == null ? null : email.Trim() });
        }
    }
}
