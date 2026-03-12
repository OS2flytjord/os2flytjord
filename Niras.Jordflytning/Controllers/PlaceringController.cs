using System;
using System.Data.Spatial;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Infrastructure.DataAccess;
using Niras.Jordflytning.Library.Logging;
using Niras.Jordflytning.ViewModels;
using Niras.Jordflytning.ViewModels.Placering;

namespace Niras.Jordflytning.Controllers
{
    public class PlaceringController : Controller
    {

        private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.PlaceringController");

        private readonly IMatrikelBusiness _matrikelBusiness;
        private readonly IDawaOpslagBusiness _dawaOpslagBusiness;
        private readonly IMatrikelRepository _matrikelRepository;

        public PlaceringController(IMatrikelBusiness matrikelBusiness, IDawaOpslagBusiness dawaOpslagBusiness)
        {
            _matrikelBusiness = matrikelBusiness;
            _dawaOpslagBusiness = dawaOpslagBusiness;
        }

        private void InitModel(IndtegnetPlaceringModel model, string wkt)
        {
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;
            model.KortApiUrl = appSettings["KortApiUrl"];
            model.KortPageModtagere = appSettings["KortPageModtagere"];
            model.KortPageSted = appSettings["KortPageMidlertidigeModtagere"];  // KortPageSted
            model.KortSite = appSettings["KortSite"];
            // Hvis wkt allerede er angivet, findes de matrikler tegningen dækker over:
            if (!string.IsNullOrWhiteSpace(wkt))
            {
                try
                {
                    // Initialiser map bounds
                    model.MapBounds.Init(wkt);
                    model.Data.Wkt = wkt; // Retur værdi

                    var matrikler = _matrikelBusiness.ReadMatrikler(model.MapBounds.Wkt);
                    foreach (var matrikel in matrikler)
                    {
                        model.Data.Matrikler.Add(
                            new MatrikelInfoModel(
                                matrikel.Ejerlav, 
                                matrikel.Ejerlavsnavn,
                                matrikel.Matrikelnr, 
                                matrikel.Geom.AsText(),
                                _matrikelBusiness.GetEsrEjendomsnummer(matrikel.Ejerlav, matrikel.Matrikelnr)
                            )
                        );
                    }

                    // Find center wkt til at udlede hovedmatrikel:
                    if (model.MapBounds.CenterGeom != null)
                    {
                        foreach (var matrikel in matrikler)
                        {
                            if (matrikel.Geom.Contains(model.MapBounds.CenterGeom))
                            {
                                model.Data.Ejerlav = matrikel.Ejerlav;
                                model.Data.Ejerlavsnavn = matrikel.Ejerlavsnavn;
                                model.Data.Matrikelnummer = matrikel.Matrikelnr;
                                // Siden data ikke indeholder Esr ejendomsnummer fra leverandøren, sættes dette manuelt.
                                model.Data.EsrEjendomsnummer =
                                    model.Data.Matrikler.First(
                                            m => m.Ejerlav == matrikel.Ejerlav && m.Matrikelnummer == matrikel.Matrikelnr)
                                        .EsrEjendomsnummer;
                                break;
                            }
                        }
                    }
                }
                catch
                {
                    model.InitFejl.Add("Fejl ved opslag på koordinater!");
                }
            }
        }

        [HttpGet]
        public ActionResult RedigerIndtegnetPlacering()
        {
            var model = new IndtegnetPlaceringModel();
            // Håndteres ikke endnu
            //model.Matrikelnr = Request["matrikelnr"];
            //model.Ejerlav = Request["ejerlav"];
            //model.Ejerlavsnavn = Request["ejerlavsnavn"];
            //model.Adresse = Request["adresse"];
            string wkt = Request["wkt"];

            InitModel(model, wkt);
            return View(model);
        }

        [HttpPost]
        public ActionResult RedigerIndtegnetPlacering(IndtegnetPlaceringModel model)
        {
            InitModel(model, model.MapBounds.Wkt);
            model.Opdateret = true;
            return View(model);
        }


        public ActionResult KoordinaterForMatrikel(string ejerlavkode, string matrikelnr)
        {
            string x = null;
            string y = null;
            try
            {
                var result = _matrikelBusiness.GetMatrikel(ejerlavkode, matrikelnr);
                var props = result.features[0].properties;
                x = props.centroid_x;
                y = props.centroid_y;
            }
            catch { }
            return Json(new { x = x, y = y }, JsonRequestBehavior.AllowGet);
        }

    }
}
