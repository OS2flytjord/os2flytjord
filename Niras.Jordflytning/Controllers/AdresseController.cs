using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models.Adresse;
using Niras.Jordflytning.Core.Models.MatrikelOpslag;
using Niras.Jordflytning.Infrastructure.DataAccess;
using Niras.Jordflytning.Library.Logging;
using Niras.Jordflytning.ViewModels.Anmeldelse;

namespace Niras.Jordflytning.Controllers
{
    public class AdresseController : Controller
    {
        private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Controllers.AdresseController");

        private readonly IKodelisteBusiness _kodelisteBusiness;
        private readonly IAnmeldelserBusiness _anmeldelserBusiness;
        private readonly IDawaOpslagBusiness _dawaOpslagBusiness;
        private readonly IMatrikelBusiness _matrikelBusiness;

        public AdresseController(IKodelisteBusiness kodelisteBusiness, IAnmeldelserBusiness anmeldelserBusiness, IDawaOpslagBusiness dawaOpslagBusiness, IMatrikelBusiness matrikelBusiness)
        {
            _kodelisteBusiness = kodelisteBusiness;
            _anmeldelserBusiness = anmeldelserBusiness;
            _matrikelBusiness = matrikelBusiness;
            _dawaOpslagBusiness = dawaOpslagBusiness;
        }

        public ActionResult Opslag()
        {
            return View();
        }

        public ActionResult DawaAutoComplete(string q, string type, string caretpos, string adgangsadresserOnly, string startfra)
        {
            string url;

            if (startfra != null)
            {
                url = String.Format("https://dawa.aws.dk/autocomplete?q={0}&type={1}&caretpos={2}&adgangsadresserOnly={3}&startfra={4}", q, type, caretpos, adgangsadresserOnly, startfra);
            }
            else
            {
                url = String.Format("https://dawa.aws.dk/autocomplete?q={0}&type={1}&caretpos={2}&adgangsadresserOnly={3}", q, type, caretpos, adgangsadresserOnly);
            }

            var request = WebRequest.Create(String.Format(url));
            // set request method 
            request.Method = "GET";
            // set content type 
            request.ContentType = "application/json";
            // get response for GET request 

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response != null)
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        var serverResponse = reader.ReadToEnd();
                        return Content(serverResponse, "application/json");
                    }
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReLoad()
        {
            HttpContext.Application["AdresseOpslagVeje"] = null;
            var loadInfo = "";
            var start = DateTime.Now;

            GetVeje();

            var slut = DateTime.Now;
            var dif = (slut - start);
            loadInfo += "GetVeje: " + dif.TotalSeconds + " total sec.";
            ViewBag.LoadInfo = loadInfo;
            return View("Load");
        }

        public ActionResult Load()
        {
            var loadInfo = "";
            var start = DateTime.Now;

            GetVeje();

            var slut = DateTime.Now;
            var dif = (slut - start);
            loadInfo += "GetVeje: " + dif.TotalSeconds + " total sec.";
            ViewBag.LoadInfo = loadInfo;
            return View("Load");
        }

        private List<VejModel> GetVeje()
        {
            try
            {
                List<VejModel> vejM = new List<VejModel>();
                if (HttpContext.Application["AdresseOpslagVeje"] == null)
                {

                    var appSettings = ConfigurationManager.AppSettings;
                    var alleKommuner = appSettings["AdresseAlleKommuner"];

                    if (alleKommuner == "0")
                    {
                        //Kun aktive kommuner 
                        //I produktion er det alle veje i DK som skal indlæses
                        foreach (var k in _kodelisteBusiness.ReadAktiveKommune())
                        {
                            vejM.AddRange(ConvertVejToModel(GetVejeInKommune(k.Kommunenr), k.Aktiv));
                        }

                    }
                    else
                    {
                        //Alle kommuner       
                        //Under udvikling er det ikke alle veje i DK som skal indlæsses
                        foreach (var k in _kodelisteBusiness.ReadAllKommuner())
                        {
                            //veje.AddRange(GetVejeInKommune(k.Kommunenr));
                            vejM.AddRange(ConvertVejToModel(GetVejeInKommune(k.Kommunenr),
                                //postnre, 
                              k.Aktiv));
                        }
                    }

                    //Test formål, så vi kan test på en kommune som ikke anvender FlytJord
                    //vejM.AddRange(ConvertVejToModel(GetVejeInKommune(846), postnre,false));
                    //vejM.AddRange(ConvertVejToModel(GetVejeInKommune(706), postnre,false)); //SydDjurs
                    //vejM.AddRange(ConvertVejToModel(GetVejeInKommune(400), postnre, false)); //Bornholm
                    //vejM.AddRange(ConvertVejToModel(GetVejeInKommune(661), postnre, false)); //Holstebro
                    //vejM.AddRange(ConvertVejToModel(GetVejeInKommune(657), postnre, false)); //Herning
                    //veje.AddRange(GetVejeInKommune(201));//MariagerFjord 846, Allerød 201
                    //vejM.AddRange(ConvertVejToModel(GetVejeInKommune(746), postnre, false)); //Skanderborg
                    //vejM.AddRange(ConvertVejToModel(GetVejeInKommune(730), postnre, false)); //Randers



                    HttpContext.Application["AdresseOpslagVeje"] = vejM;
                }
                else
                {
                    vejM = (List<VejModel>)HttpContext.Application["AdresseOpslagVeje"];
                }
                return vejM;
            }

            catch (Exception e)
            {
                Logger.LogException("Adresseopslag - Fejl ved hent veje", e);
                throw;
            }
        }


        public ActionResult SearchVejeMobile(string wildcard, string term = null)
        {
            try
            {

                //Mobil løsningen skal kun have veje fra kommuner som anvender FlytJord

                //quickfix til mobile
                if (term != null)
                    wildcard = term;

                var vejM = GetVeje();
                var res = (
                            from v in vejM
                            where v.vejnavn.ToLower().StartsWith(wildcard.ToLower())
                            & v.aktivKommune
                            select v).OrderBy(x => x.displayname).ToList();

                //quickfix til mobile
                foreach (var vejModel in res)
                {
                    vejModel.label = vejModel.displayname;
                }
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SearchVeje(string wildcard, string term = null)
        {
            try
            {

                //quickfix til mobile
                if (term != null)
                    wildcard = term;

                var vejM = GetVeje();
                var res = (
                            from v in vejM
                            where v.vejnavn.ToLower().StartsWith(wildcard.ToLower())
                            //orderby v.vejnavn 
                            select v).OrderBy(x => x.displayname).ToList();

                //quickfix til mobile
                foreach (var vejModel in res)
                {
                    vejModel.label = vejModel.displayname;
                }
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(null, JsonRequestBehavior.AllowGet);

            }
        }

        //private static IEnumerable<VejModel> ConvertVejToModel(List<Vej> veje, List<Postnummer> postnummers)
        private static List<VejModel> ConvertVejToModel(List<Vej> veje,
            //List<Postnummer> postnummers, 
          bool _aktivKommune)
        {

            var res = (from v in veje
                       select new VejModel
                       {
                           displayname = v.navn + ", " + v.postnummer.navn
                                       ,
                           postnr = v.postnummer.nr,
                           vejkode = v.kode,
                           vejnavn = v.navn,
                           aktivKommune = _aktivKommune

                       }).ToList();
            return res;
        }



        private static List<Vej> GetVejeInKommune(int kommuneNr)
        {
            //http://stackoverflow.com/questions/11126242/using-jsonconvert-deserializeobject-to-deserialize-json-to-a-c-sharp-poco-class
            //http://www.codeproject.com/Questions/427387/ASP-NET-MVC3-HttpWebRequest-Help-Needed

            var veje = new List<Vej>();
            //var requestUrl = @"http://geo.oiorest.dk/vejnavne.json?kommunekode=" + kommuneNr;
            //var requestUrl = @"http://webapi.aws.dk/vejnavne.json?kommunekode=0" + kommuneNr;
            var requestUrl = @"https://dawa.aws.dk/vejstykker?kommunekode=" + kommuneNr;


            // Create Request
            var request = WebRequest.Create(requestUrl);
            // set request method 
            request.Method = "GET";
            // set content type 
            request.ContentType = "application/json";
            // get response for GET request 
            request.Proxy = null;
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response != null && response.StatusCode != HttpStatusCode.OK)
                    throw new Exception("Adresseopslag - Vejnavne kunne ikke hentes.");

                if (response != null)
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        var serverResponse = reader.ReadToEnd();

                        var arrVeje = JsonConvert.DeserializeObject<dynamic>(serverResponse);


                        foreach (var vej in arrVeje)
                        {
                            try
                            {
                                var kommune = vej.kommune;


                                foreach (var postnumer in vej.postnumre)
                                {


                                    veje.Add(new Vej
                                        {
                                            etrs89koordinat = new etrs89koordinat { nord = null, øst = null },
                                            href = vej.href,
                                            kode = vej.kode,
                                            kommune = new Kommune { href = kommune.href, kode = kommune.kode },
                                            navn = vej.navn,
                                            postnummer =
                                                new Postnummer { href = postnumer.href, navn = postnumer.navn, nr = postnumer.nr },
                                            wgs84Koordinat = new wgs84koordinat { bredde = null, længde = null }
                                        });
                                }
                            }
                            catch
                            {

                            }
                        }




                        //if (arrVeje != null && arrVeje.Length > 0)
                        //  veje = arrVeje.ToList();
                    }
            }
            return veje;
        }

        public ActionResult MatrikelAutoComplete(string q)
        {
            if (q != null)
            {
                try
                {
                    var ejerlavListe = _dawaOpslagBusiness.EjerlavSoegning(q);
                    var jordstykkeListe = _dawaOpslagBusiness.JordstykkeOpslag(q, ejerlavListe);
                    return Json(new
                    {
                        items = jordstykkeListe.Select(x => new
                        {
                            ejerlav = x.ejerlav.kode, 
                            ejerlavnavn = x.ejerlav.navn,
                            matrikelnr = x.matrikelnr,
                            id = string.Format("{0}@{1}", x.ejerlav.kode, x.matrikelnr),
                            tekst = string.Join(" ", new string[] { x.ejerlav.navn, x.matrikelnr })
                        })
                    }, JsonRequestBehavior.AllowGet);
                }
                catch {}
            }
            return Json(new object[] {}, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult KoordinaterForMatrikel(int ejerlav, string matrikelnr)
        {
            string x = null;
            string y = null;
            try
            {
                var result = _matrikelBusiness.GetMatrikel(ejerlav.ToString(), matrikelnr);
                var props = result.features[0].properties;
                x = props.centroid_x;
                y = props.centroid_y;
            }
            catch { }
            return Json(new { x = x, y = y }, JsonRequestBehavior.AllowGet);
        }
        
    }
}