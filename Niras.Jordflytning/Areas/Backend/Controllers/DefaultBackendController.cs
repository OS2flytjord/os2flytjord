using System;
using System.Collections.Generic;
using Niras.Jordflytning.Core;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using Niras.Jordflytning.ViewModels;
using System.Web.Mvc;
using Menu = Niras.Jordflytning.Entities.Menu;
using MenuItem = Niras.Jordflytning.Entities.MenuItem;

namespace Niras.Jordflytning.Areas.Backend.Controllers
{
    [Authorize]
    public class DefaultBackendController : Controller
    {
        #region Variables

        private readonly ISecurityProvider _securityProvider;


        private const string SearchViewPath = @"/Backend/Search/AdminSearch?soegetype=0";
        private const string ForsideText = @"Forside";
        private const string SoegText = @"Søg";
        private readonly IStikproeveBusiness _stikproeveBusiness;

        private bool _soegHasBeenAdded;

        #endregion

        #region Constructor

        public DefaultBackendController(ISecurityProvider securityProvider, IStikproeveBusiness stikproeveBusiness)
        {
            _securityProvider = securityProvider;
            _stikproeveBusiness = stikproeveBusiness;
        }

        #endregion

        public ActionResult Index(string rolle, string fane = "", string id = "")
        {
            var model = new IndexModel();
            BuildMenues(model);

            //Deep link
            Guid gs;
            if (!string.IsNullOrEmpty(fane) && fane == "stikproeve" && !string.IsNullOrEmpty(id) && Guid.TryParse(id, out gs))
            {
                var s = _stikproeveBusiness.Read(gs);
                if (s != null)
                {
                    model.StikproeveDeepLink = "/Backend/Stikproeve/stikproeve?stikproeveId=" + id;
                    model.StikproeveFaneTekst = "Stikprøve - " + s.Nummer;
                }
            }
            return View(model);
        }

        /// <summary>
        /// Logs out current user.
        /// </summary>
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            _securityProvider.Logout();
            return RedirectToAction("FrontPage", "Default", new { area = "" });
        }

        #region private methods

        private void BuildMenues(IndexModel model)
        {
            if (_securityProvider.CurrentUser.IsInRole(ApplicationConstants.MiljoemedarbejderRolle))
            {
                AddRoleToModel(model, ApplicationConstants.MiljoemedarbejderRolle);
                if (model.Menu == null)
                    model.Menu = BuildMiljoemedarbejderMenu();
                else
                    model.Menu.MergeMenues(BuildMiljoemedarbejderMenu());
            }
            if (_securityProvider.CurrentUser.IsInRole(ApplicationConstants.SagsbehandlerRolle))
            {
                AddRoleToModel(model, ApplicationConstants.SagsbehandlerRolle);
                if (model.Menu == null)
                    model.Menu = BuildKommuneSagsbehandlerMenu();
                else
                    model.Menu.MergeMenues(BuildKommuneSagsbehandlerMenu());
            }
            if (_securityProvider.CurrentUser.IsInRole(ApplicationConstants.LaboratorieRolle))
            {
                AddRoleToModel(model, ApplicationConstants.LaboratorieRolle);
                if (model.Menu == null)
                    model.Menu = BuildLaboratorieMenu();
                else
                    model.Menu.MergeMenues(BuildLaboratorieMenu());
            }
            if (_securityProvider.CurrentUser.IsInRole(ApplicationConstants.ProevetagerRolle))
            {
                AddRoleToModel(model, ApplicationConstants.ProevetagerRolle);
                if (model.Menu == null)
                    model.Menu = BuildStikprøvetagerMenu();
                else
                    model.Menu.MergeMenues(BuildStikprøvetagerMenu());
            }
            if (_securityProvider.CurrentUser.IsInRole(ApplicationConstants.PladsmandRolle))
            {
                AddRoleToModel(model, ApplicationConstants.PladsmandRolle);
                if (model.Menu == null)
                    model.Menu = BuildPladsmandMenu();
                else
                    model.Menu.MergeMenues(BuildPladsmandMenu());
            }
            if (_securityProvider.CurrentUser.IsInRole(ApplicationConstants.BogholderRolle))
            {
                AddRoleToModel(model, ApplicationConstants.BogholderRolle);
                if (model.Menu == null)
                    model.Menu = BuildJordModtagerBogholderMenu();
                else
                    model.Menu.MergeMenues(BuildJordModtagerBogholderMenu());
            }
            if (_securityProvider.CurrentUser.IsInRole(ApplicationConstants.JordmodtagerAdminRolle))
            {
                AddRoleToModel(model, ApplicationConstants.JordmodtagerAdminRolle);
                if (model.Menu == null)
                    model.Menu = BuildJordModtagerAdminMenu();
                else
                    model.Menu.MergeMenues(BuildJordModtagerAdminMenu());
            }
            if (_securityProvider.CurrentUser.IsInRole(ApplicationConstants.KommuneAdminRolle))
            {
                AddRoleToModel(model, ApplicationConstants.KommuneAdminRolle);
                if (model.Menu == null)
                    model.Menu = BuildKommuneAdministratorMenu();
                else
                    model.Menu.MergeMenues(BuildKommuneAdministratorMenu());
            }
        }

        private static Menu BuildKommuneAdministratorMenu()
        {
            var menu = new Menu();
            var menuItemAdministration = new MenuItem("Administration");
            menuItemAdministration.AddSubItem(new MenuItem("Brugere", "Kommune brugere", "/Backend/Administration/AdminBrugere"));
            menuItemAdministration.AddSubItem(new MenuItem("Jordmodtagerfirmaer", "Jordmodtagere", "/Backend/Administration/AdminJordmodtager"));
            menuItemAdministration.AddSubItem(new MenuItem("Modtageanlæg", "Modtageanlæg", "/Backend/Administration/AdminModtagerAnlaeg"));
            menuItemAdministration.AddSubItem(new MenuItem("Fakturering", "Fakturering", "/Backend/Administration/AdminFakturering"));
            menu.AddMenuItem(menuItemAdministration);
            return menu;
        }

        private static Menu BuildJordModtagerAdminMenu()
        {
            var menu = new Menu();
            var menuItemAdministration = new MenuItem("Administration");
            menuItemAdministration.AddSubItem(new MenuItem("Brugere", "Jord brugere", "/Backend/Administration/AdminBrugere"));
            menuItemAdministration.AddSubItem(new MenuItem("Jordmodtageranlæg", "Jordmodtageranlæg", "/Backend/Administration/AdminModtageAnlaeg"));
            menu.AddMenuItem(menuItemAdministration);
            return menu;
        }

        private Menu BuildJordModtagerBogholderMenu()
        {
            var menu = new Menu();
            var menuItemStartside = new MenuItem("Menu");
            AddSoegMenuItem(menuItemStartside);

            const string text = ForsideText + " (" + ApplicationConstants.BogholderRolle + ")";
            menuItemStartside.AddSubItem(new MenuItem(text, text, "/Backend/FrontPages/StartBogholder"));
            menu.AddMenuItem(menuItemStartside);
            return menu;
        }


        private Menu BuildKommuneSagsbehandlerMenu()
        {
            var menu = new Menu();
            var menuItemStartside = new MenuItem("Menu");
            AddSoegMenuItem(menuItemStartside);
            const string text = ForsideText + " (" + ApplicationConstants.SagsbehandlerRolle + ")";
            menuItemStartside.AddSubItem(new MenuItem(text, text, "/Backend/FrontPages/StartSagsbehandler"));
            menu.AddMenuItem(menuItemStartside);
            return menu;
        }

        private Menu BuildMiljoemedarbejderMenu()
        {
            var menu = new Menu();
            var menuItemStartside = new MenuItem("Menu");
            AddSoegMenuItem(menuItemStartside);
            const string text = ForsideText + " (" + ApplicationConstants.MiljoemedarbejderRolle + ")";
            menuItemStartside.AddSubItem(new MenuItem(text, text, "/Backend/FrontPages/StartMiljoemedarbejder"));
            menu.AddMenuItem(menuItemStartside);
            return menu;
        }

        private Menu BuildPladsmandMenu()
        {
            var menu = new Menu();
            var menuItemStartside = new MenuItem("Menu");
            AddSoegMenuItem(menuItemStartside);
            const string text = ForsideText + " (" + ApplicationConstants.PladsmandRolle + ")";
            menuItemStartside.AddSubItem(new MenuItem(text, text, "/Backend/FrontPages/StartPladsmand"));
            menu.AddMenuItem(menuItemStartside);
            return menu;
        }

        private Menu BuildStikprøvetagerMenu()
        {
            var menu = new Menu();
            var menuItemStartside = new MenuItem("Menu");
            AddSoegMenuItem(menuItemStartside);
            const string text = ForsideText + " (" + ApplicationConstants.ProevetagerRolle + ")";
            menuItemStartside.AddSubItem(new MenuItem(text, text, "/Backend/FrontPages/StartProevetager"));
            menu.AddMenuItem(menuItemStartside);
            return menu;
        }

        private Menu BuildLaboratorieMenu()
        {
            var menu = new Menu();
            var menuItemStartside = new MenuItem("Menu");
            AddSoegMenuItem(menuItemStartside);
            const string text = ForsideText + " (" + ApplicationConstants.LaboratorieRolle + ")";
            menuItemStartside.AddSubItem(new MenuItem(text, text, "/Backend/FrontPages/StartLab"));
            menu.AddMenuItem(menuItemStartside);
            return menu;
        }

        private void AddSoegMenuItem(MenuItem menuItemStartside)
        {
            if (!_soegHasBeenAdded)
            {
                menuItemStartside.AddSubItem(new MenuItem(SoegText, SoegText, SearchViewPath));
                _soegHasBeenAdded = true;
            }
        }

        private static void AddRoleToModel(IndexModel model, string rolle)
        {
            if (model == null || String.IsNullOrEmpty(rolle))
                return;

            if (model.RoleList == null)
                model.RoleList = new List<String>();
            model.RoleList.Add(rolle);

            if (String.IsNullOrEmpty(model.CurrentRoles))
            {
                model.CurrentRoles = rolle;
            }
            else
            {
                model.CurrentRoles = String.Format("{0};{1}", model.CurrentRoles, rolle);
            }
        }

        #endregion
    }
}

