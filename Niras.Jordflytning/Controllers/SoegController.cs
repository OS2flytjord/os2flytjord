using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.SoegeResultat;
using Niras.Jordflytning.ViewModels.Anmeldelse;

namespace Niras.Jordflytning.Controllers
{
    [Authorize]
    public class SoegController : Controller
    {
        #region *** Properties ***

        private readonly IModtagerAnlaegBusiness _modtagerAnlaegBusiness;
        private readonly ITransportoerBusiness _transportoerBusiness;
        private readonly ISecurityProvider _securityProvider;
        private readonly IBrugereBusiness _brugerBusiness;
        private readonly ISoegBusiness _soegBusiness;
        private readonly IAnmelderBusiness _anmelderBusiness;

        #endregion *** Properties ***

        #region *** Constructors ***

        public SoegController(
            IModtagerAnlaegBusiness modtagerAnlaegBusiness,
            ITransportoerBusiness transportoerBusiness,
            ISecurityProvider securityProvider,
            IBrugereBusiness brugerBusiness,
            ISoegBusiness soegBusiness,
            IAnmelderBusiness anmelderBusiness)
        {
            _transportoerBusiness = transportoerBusiness;
            _modtagerAnlaegBusiness = modtagerAnlaegBusiness;
            _securityProvider = securityProvider;
            _brugerBusiness = brugerBusiness;
            _soegBusiness = soegBusiness;
            _anmelderBusiness = anmelderBusiness;
        }

        #endregion *** Constructors ***

        #region *** Public methods ***

        public ActionResult Soeg(int soegetype)
        {
            SoegModel model = ExecuteSoeg((SoegeTypeDefault) soegetype);
            BrugerProfil b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
            Person person = b.Person;
            if (person != null)
            {
                AddSoegeComboData(person, model);
            }
            ModelState.Clear();
            return View(model);
        }

        [HttpPost]
        public ActionResult Soeg(SoegModel soegModel, string command)
        {
            string s = command;
            bool isClearSearch = (s == "Ny søgning");
            bool isParamSearch = (s == "Søg");

            var model = new SoegModel();

            if (isClearSearch)
            {
                model = ExecuteSoeg(model);
            }
            else if (isParamSearch)
            {
                soegModel.SelectedSoegeType = "0";
                model = ExecuteSoeg(soegModel);
            }
            else
            {
                if (!String.IsNullOrEmpty(soegModel.SelectedSoegeType) || soegModel.SelectedSoegeType != "0")
                {
                    string soegeTypeStr = soegModel.SelectedSoegeType;

                    int soegeType;
                    if (Int32.TryParse(soegeTypeStr, out soegeType))
                        model = ExecuteSoeg((SoegeTypeDefault) soegeType);

                    List<SelectListItem> soeget = model.SoegeTyper.ToList();
                    if (model.SelectedSoegeType != null)
                    {
                        SelectListItem dd = soeget.First(x => x.Value == soegModel.SelectedSoegeType);
                        SelectListItem selected = soeget[soeget.IndexOf(dd)];
                        selected.Selected = true;
                    }
                    model.SoegeTyper = soeget;
                }
            }
            BrugerProfil b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
            Person person = b.Person;
            if (person != null)
                AddSoegeComboData(person, model);
            ModelState.Clear();
            return View(model);
        }

        #endregion *** Public methods ***

        #region *** Private methods ***

        private SoegModel ExecuteSoeg(SoegeTypeDefault soegTypeEnum)
        {
            BrugerProfil b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
            Guid brugerId = b.Person.Id;
            var model = new SoegModel();
            // set selected item in combo
            model.SoegeTyper
                .Where(x => x.Value == ((Int32) soegTypeEnum).ToString(CultureInfo.InvariantCulture))
                .ToList()
                .ForEach(x => { x.Selected = true; });
            model.SelectedSoegeType = ((Int32) soegTypeEnum).ToString(CultureInfo.InvariantCulture);
            model.SoegeResultatList = new List<SoegeResultat>();
            switch (soegTypeEnum)
            {
                case SoegeTypeDefault.Ingen:
                    break;
                case SoegeTypeDefault.Fremsendte:
                    model.SoegeResultatList = _soegBusiness.GetFremSendteAnmeldelser(brugerId, false);
                    break;
                case SoegeTypeDefault.IkkeFremsendte:
                    model.SoegeResultatList = _soegBusiness.GetIkkeFremSendteAnmeldelser(brugerId, false);
                    break;
                case SoegeTypeDefault.Aktive:
                    model.SoegeResultatList = _soegBusiness.GetAktiveAnmeldelser(brugerId, false);
                    break;
                case SoegeTypeDefault.Afsluttede:
                    model.SoegeResultatList = _soegBusiness.GetAfsluttedeAnmeldelser(brugerId);
                    break;
            }
            return model;
        }

        private SoegModel ExecuteSoeg(SoegModel model)
        {
            BrugerProfil b = _brugerBusiness.Read(_securityProvider.CurrentUser.Identity.Name);
            Guid brugerId = b.Person.Id;

            if (model == null)
                model = new SoegModel();

            // set selected item in combo
            model.SoegeTyper
                .Where(x => x.Value == (0).ToString(CultureInfo.InvariantCulture))
                .ToList()
                .ForEach(x => { x.Selected = true; });

            model.SoegeResultatList = new List<SoegeResultat>();

            Guid transportoerGuid = Guid.Empty;
            Guid anmelderrGuid = Guid.Empty;
            Guid modtagerGuid = Guid.Empty;

            if (!String.IsNullOrEmpty(model.TransportoerId) && model.TransportoerId != "0")
                transportoerGuid = new Guid(model.TransportoerId);
            if (!String.IsNullOrEmpty(model.AnmelderId) && model.AnmelderId != "0")
                anmelderrGuid = new Guid(model.AnmelderId);
            if (!String.IsNullOrEmpty(model.ModtagerId) && model.ModtagerId != "0")
                modtagerGuid = new Guid(model.ModtagerId);

            model.SoegeResultatList = _soegBusiness.GetAnmeldelserBy(
                model.LoebeNr, model.Adresse,
                model.FoerDato, model.EfterDato,
                transportoerGuid, anmelderrGuid,
                modtagerGuid, brugerId,
                model.IsKunMine, model.InclAfsluttede);

            return model;
        }

        private void AddSoegeComboData(Person person, SoegModel model)
        {
            IList<ModtagerAnlaeg> annvendteModtagerList =
                _modtagerAnlaegBusiness.ReadTidligereAktiveAnvendteModtagerAnlaeg(person.Id);
            model.AddModtagere(annvendteModtagerList);

            IList<Transportoer> anvendteTransportoereList =
                _transportoerBusiness.ReadTidligereAktiveAnvendteTransportoerer(person.Id);
            model.AddTransportoere(anvendteTransportoereList);

            Anmelder anmelder = _anmelderBusiness.ReadAnmelder(person.Id);
            var existerendeAnmeldereList = new List<Anmelder>();
            if (anmelder != null)
                existerendeAnmeldereList.Add(anmelder);
            model.AddAnmeldere(existerendeAnmeldereList);
        }

        #endregion *** Private methods ***
    }
}
