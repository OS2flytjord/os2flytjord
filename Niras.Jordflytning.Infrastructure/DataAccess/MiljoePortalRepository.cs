using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.JordForurening;
using Niras.Jordflytning.Library.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Spatial;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
    public class MiljoePortalRepository : IMiljoePortalRepository
    {
        private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Infrastructure.DataAccess.MiljoePortalRepository");
        private const string HeaderText = "Danmarks Miljøportal";

        private enum ServiceType
        {
            V1 = 1,
            V2 = 2,
            Omk = 3
        }

        private class LayerHandler
        {
            public string Name { get; private set; }
            public string Description { get; private set; }

            public LayerHandler(string name, string description)
            {
                Name = name;
                Description = description;
            }
        }

        public KonfliktSoegningLeverandoerResultat KonfliktSoegningMidlertidigModtager(DbGeometry geom)
        {
            var result = new KonfliktSoegningLeverandoerResultat(HeaderText, new Uri("http://www.miljoeportal.dk/driftsstatus/Sider/default.aspx"));

            var layerHandlersDKJORD = new LayerHandler[]
            {
                    new LayerHandler("DKJord:View_V1Flader", "Jordforurening V1 (vidensniveau 1)"),
                    new LayerHandler("DKJord:View_V2Flader", "Jordforurening V2 (vidensniveau 2)"),
                    new LayerHandler("DKJord:View_NuanceringsFlader", "Nuancering af V2"),
            };

            var layerHandlers = new LayerHandler[]
            {
                new LayerHandler("OMR_KLASSIFICERING", "Områdeklassificeret (Analysefrit område kategori 1, Analysefrit område kategori 2 eller Område med krav om analyser)"),
                //new LayerHandler("BES_NAT_BUFFERZONER", "Beskyttede naturbufferzoner"),
                new LayerHandler("BES_NATURTYPER", "Beskyttede naturtyper"),
                new LayerHandler("BES_STEN_JORDDIGER", "Beskyttede sten- og jorddiger"),
                new LayerHandler("BES_VANDLOEB", "Beskyttede vandløb"),
                //new LayerHandler("DRIKKEVANDS_INTER", "Drikkevandsinteresser"),
                new LayerHandler("FREDEDE_OMR", "Fredede områder"),
                new LayerHandler("FREDEDE_OMR_FORSLAG", "Fredede områder, forslag"),
                //new LayerHandler("Indv_opland_u_osd", "Indvindingsoplande udenfor områder med særlige drikkevandsinteresser"),
                //new LayerHandler("NATURA2000_OPL", "Natura 2000 oplande"),
                new LayerHandler("SOE_BES_LINJER", "Søbeskyttelseslinjer"),
                new LayerHandler("AA_BES_LINJER", "Åbeskyttelseslinjer"),
                new LayerHandler("STATUS_BNBO", "Boringsnære beskyttelsesområder"),
            };

            try
            {
                if (geom == null)
                    throw new ArgumentException("SystemFejl: Der mangler en geometri, ved hentning af forureningsoplysninger! Kontakt den systemansvarlige.");

                // Da der her er tale om en indtegning, håndterer vi kun geometrien som den er. (Ingen matrikler)
                var inputGeometryGml = geom.AsGml();

                if (inputGeometryGml == null)
                    throw new ArgumentException("SystemFejl: Kunne ikke lave en geometri, ved hentning af forureningsoplysninger! Kontakt den systemansvarlige.");

                //Opslag i DAIdb
                // Fjern xml tag fra geometri
                inputGeometryGml = inputGeometryGml.Remove(0, 38);
                inputGeometryGml = inputGeometryGml.Replace("Polygon", "gml:Polygon").Replace("exterior", "gml:exterior").Replace("LinearRing", "gml:LinearRing").Replace("posList", "gml:posList");
                var appSettings = ConfigurationManager.AppSettings;
                var dmpDKJordWfsUrl = appSettings["dmpDKJordWfsUrl"];
                foreach (var layerHandler in layerHandlersDKJORD)
                {

                    var url = CreateInputParamForDKJordWfs(inputGeometryGml, layerHandler.Name, dmpDKJordWfsUrl);
                    var hit = false;
                    Exception error = null;
                    try
                    {
                        var serviceResult = CallWfsService(url, dmpDKJordWfsUrl);
                        hit = TjekReultatForHit(serviceResult, layerHandler.Name);
                    }
                    catch (Exception ex)
                    {
                        error = ex;
                        Logger.LogException(string.Format("MiljoePortalRepository.GetResultMidlertidigModtager -> Service kald fejlede for lag: '{0}' - Url: '{1}'", layerHandler, url), ex);
                    }
                    var lagResultat = new KonfliktSoegningServiceResultat(layerHandler.Name, layerHandler.Description, hit);
                    if (error != null)
                        lagResultat.MarkerSomFejlet(error);
                    result.ServiceResultater.Add(lagResultat);
                }


                //Opslag i DAIdb
                var dmpWfsUrl = appSettings["dmpWfsUrl"];
                foreach (var layerHandler in layerHandlers)
                {

                    var url = CreateInputParamForWfs(inputGeometryGml, layerHandler.Name, dmpWfsUrl);
                    var hit = false;
                    Exception error = null;
                    try
                    {
                        var serviceResult = CallWfsService(url, dmpWfsUrl);
                        hit = TjekReultatForHit(serviceResult, layerHandler.Name);
                    }
                    catch (Exception ex)
                    {
                        error = ex;
                        Logger.LogException(string.Format("MiljoePortalRepository.GetResultMidlertidigModtager -> Service kald fejlede for lag: '{0}' - Url: '{1}'", layerHandler, url), ex);
                    }
                    var lagResultat = new KonfliktSoegningServiceResultat(layerHandler.Name, layerHandler.Description, hit);
                    if (error != null)
                        lagResultat.MarkerSomFejlet(error);
                    result.ServiceResultater.Add(lagResultat);
                }
            }
            catch (Exception exception)
            {
                Logger.LogException("MiljoePortalRepository.GetResultMidlertidigModtager", exception);
                result.MarkerSomFejlet(exception);
            }
            return result;
        }

        private static bool TjekReultatForHit(XContainer resultXdocument, string serviceName)
        {
            var first = resultXdocument.Descendants().FirstOrDefault();
            if (first != null && first.Name.LocalName != "FeatureCollection")
                throw new Exception("Systemfejl: Miljøportal: " + first.Value);

            // Tjek for geometri, hvilket indikerer om der er et hit
            var localName = (serviceName.IndexOf(":") > -1 ? serviceName.Split(':')[1] : serviceName);
            return resultXdocument.Descendants().Where(x => x.Name.LocalName.ToString().Equals(localName, StringComparison.OrdinalIgnoreCase)).Any();
        }

        public List<ForureningsOpslagResult> GetResult(Oprindelsessted oprSted, IList<JordKlassifikationType> jordKlassifikationTypeList)
        {
            var resList = new List<ForureningsOpslagResult>();

            var inputJordV1 = "";
            var inputJordV2 = "";
            var inputOmrKl = "";
            string inputGeometryGml = null;
            try
            {
                if (oprSted == null || oprSted.Geom == null)
                    throw new ArgumentException("SystemFejl: Der mangler en geometri, ved hentning af forureningsoplysninger! Kontakt den systemansvarlige.");

                const string serviceNameDkjordV1 = "DKJord:View_V1Flader";
                const string serviceNameDkjordV2 = "DKJord:View_V2Flader";
                const string serviceNameOmrKlassificering = "omr_klassificering";

                //string inputGeometryGmlTilDAI = null;
                if (oprSted.Geom.SpatialTypeName == "Point")
                {
                    DbGeometry matrikelGeom = null;
                    var isFirst = true;
                    foreach (var matrikel in oprSted.Matrikel)
                    {
                        if (isFirst)
                        {
                            matrikelGeom = matrikel.Geom;
                            isFirst = false;
                        }
                        if (matrikel.Geom != null)
                            matrikelGeom.Union(matrikel.Geom);
                    }

                    if (matrikelGeom != null)
                    {
                        //inputGeometryGmlTilDAI = matrikelGeom.AsText();
                        inputGeometryGml = matrikelGeom.AsGml();
                    }
                    else
                    {
                        //inputGeometryGmlTilDAI = oprSted.Geom.AsText();
                        inputGeometryGml = oprSted.Geom.AsGml();
                    }
                }
                else
                {
                    //inputGeometryGmlTilDAI = oprSted.Geom.AsText();
                    inputGeometryGml = oprSted.Geom.AsGml();
                }

                if (inputGeometryGml == null || string.IsNullOrWhiteSpace(inputGeometryGml))
                    throw new ArgumentException("SystemFejl: Kunne ikke lave en geometri, ved hentning af forureningsoplysninger! Kontakt den systemansvarlige.");

                // have to remove the xml tag
                inputGeometryGml = inputGeometryGml.Remove(0, 38);
                inputGeometryGml = inputGeometryGml.Replace("Polygon", "gml:Polygon").Replace("exterior", "gml:exterior").Replace("LinearRing", "gml:LinearRing").Replace("posList", "gml:posList");

                var appSettings = ConfigurationManager.AppSettings;
                var dmpDkJordWfsUrl = appSettings["dmpDKJordWfsUrl"];
                // Get Jord_V1
                inputJordV1 = CreateInputParamForDKJordWfs(inputGeometryGml, serviceNameDkjordV1, dmpDkJordWfsUrl);
                var resultJordV1 = CallWfsService(inputJordV1, dmpDkJordWfsUrl);
                var resV1 = MapV1OrV2Result2(ServiceType.V1, resultJordV1, serviceNameDkjordV1, jordKlassifikationTypeList);

                // Get Jord_V2
                inputJordV2 = CreateInputParamForDKJordWfs(inputGeometryGml, serviceNameDkjordV2, dmpDkJordWfsUrl);
                var resultJordV2 = CallWfsService(inputJordV2, dmpDkJordWfsUrl);
                var resV2 = MapV1OrV2Result2(ServiceType.V2, resultJordV2, serviceNameDkjordV2, jordKlassifikationTypeList);

                // Get OmraadeKlassificering
                var dmpWfsUrl = appSettings["dmpWfsUrl"];
                inputOmrKl = CreateInputParamForWfs(inputGeometryGml, serviceNameOmrKlassificering, dmpWfsUrl);
                var resultOmrKl = CallWfsService(inputOmrKl, dmpWfsUrl);
                var resOmk = MapOmraadeKlassificeringResult(resultOmrKl, serviceNameOmrKlassificering, jordKlassifikationTypeList);

                if (resV1 != null)
                    resList.AddRange(resV1);

                if (resV2 != null)
                    resList.AddRange(resV2);

                if (resOmk != null)
                    resList.AddRange(resOmk);

            }
            catch (Exception exception)
            {
                var errorResult = new ForureningsOpslagResult(ExternalDataProviderName.MiljoePortal);
                errorResult.ResultException = exception;
                resList.Add(errorResult);
                Logger.LogException(String.Format(
                    "MiljoePortalRepository. Read: Failed. urlJordV1: {0}, urlJordV2: {1}, urlOmrKl: {2},", inputJordV1, inputJordV2, inputOmrKl), exception);

                Logger.LogInfo($"Fejl i {nameof(MiljoePortalRepository)}.{nameof(GetResult)}{Environment.NewLine}Løbenummer: {oprSted.Anmeldelse?.Nummer}{Environment.NewLine}Geometri:{Environment.NewLine}{inputGeometryGml ?? string.Empty}");
            }
            return resList;
        }

        private string CreateInputParamForWfs(string inputGeometryGml, string serviceName, string wmsUrl)
        {
            var filter =
                   @"<fes:Filter>" +
                   @"<fes:Intersects>" +
                   @"<fes:ValueReference>Shape</fes:ValueReference>" +
                   inputGeometryGml +
                   @"</fes:Intersects>" +
                   @"</fes:Filter>";
            //filter = filter.Replace(",", ".");

            return
                @"<?xml version='1.0' encoding='utf-8'?>" +
                @"<wfs:GetFeature " +
                @"	xmlns:wfs='http://www.opengis.net/wfs/2.0' " +
                @"	xmlns:fes='http://www.opengis.net/fes/2.0' " +
                @"	xmlns:gml='http://www.opengis.net/gml/3.2' " +
                @"	xmlns:dmp='" + wmsUrl + "' " +
                @"	xmlns:ogc='http://www.opengis.net/ogc' " +
                @"	xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' " +
                @"	service='WFS' " +
                @"	version='2.0.0' " +
                @"	outputFormat='GML2' " +
                @"	count='50' " +
                @"	handle='' >" +
                @"		<wfs:Query typeNames='" + serviceName + "' srsName='urn:ogc:def:crs:EPSG:25832' >" +
                @"			" + filter +
                @"		</wfs:Query>" +
                @"</wfs:GetFeature>";
        }

        private static string CreateInputParamForDKJordWfs(string inputGeometryGml, string serviceName, string wmsUrl)
        {
            var filter =
                @"<fes:Filter>" +
                @"<fes:Intersects>" +
                @"<fes:ValueReference>Fladegeometri</fes:ValueReference>" +
                inputGeometryGml +
                @"</fes:Intersects>" +
                @"</fes:Filter>";
            //filter = filter.Replace(",", ".");

            return 
                @"<?xml version='1.0' encoding='utf-8'?>" +
                @"<wfs:GetFeature " +
                @"	xmlns:wfs='http://www.opengis.net/wfs/2.0' " +
                @"	xmlns:fes='http://www.opengis.net/fes/2.0' " +
                @"	xmlns:gml='http://www.opengis.net/gml/3.2' " +
                @"	xmlns:dmp='" + wmsUrl + "' " +
                @"	xmlns:ogc='http://www.opengis.net/ogc' " +
                @"	xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' " +
                @"	service='WFS' " +
                @"	version='2.0.0' " +
                @"	outputFormat='GML2' " +
                @"	count='50' " +
                @"	handle='' >" +
                @"		<wfs:Query typeNames='" + serviceName + "' srsName='urn:ogc:def:crs:EPSG:25832' >" +
                @"			" + filter +
                @"		</wfs:Query>" +
                @"</wfs:GetFeature>";
        }

        private static XDocument CallWfsService(string input, string dmpWfsUrl)
        {
            XDocument resultXdocument;

            var httpWReq = (HttpWebRequest)WebRequest.Create(dmpWfsUrl);
            var data = Encoding.ASCII.GetBytes(input);

            httpWReq.Method = "POST";
            httpWReq.ContentType = "text/xml";
            httpWReq.ContentLength = data.Length;

            using (var stream = httpWReq.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            using (var stream = httpWReq.GetResponse().GetResponseStream())
            {
                resultXdocument = XDocument.Load(stream);
            }
            return resultXdocument;
        }

        private List<ForureningsOpslagResult> MapV1OrV2Result2(ServiceType serviceType, XContainer resultXdocument, string serviceName, IList<JordKlassifikationType> jordKlassifikationTypeList)
        {
            // Kolonner i output
            // DKJORD_V1: Statuskode,Status,Bemaerkning,Lokalitet_nr,Temanavn
            // DKJORD_V2: Statuskode,Status,Bemaerkning,Lokalitet_nr,Temanavn

            var resultList = new List<ForureningsOpslagResult>();
            var milPortalTypeList = GetMiljoePortalTypeList();

            try
            {
                var first = resultXdocument.Descendants().FirstOrDefault();
                if (first != null && first.Name.LocalName != "FeatureCollection")
                    throw new Exception("Systemfejl: Miljøportal: " + first.Value);

                // loop through returning geometries - There is geometries if there is V1 or V2.
                var localName = (serviceName.IndexOf(":") > -1 ? serviceName.Split(':')[1] : serviceName);
                var node = resultXdocument.Descendants().Where(x => x.Name.LocalName.ToString().Equals(localName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (node != null)
                {
                    foreach (var xElememt in node.Descendants())
                    {
                        MiljoePortalKlassifikation miljoePortalKlassifikation;
                        if (serviceType == ServiceType.V1)
                            miljoePortalKlassifikation = milPortalTypeList[1];
                        else
                            miljoePortalKlassifikation = milPortalTypeList[2];

                        var klass = GetKraftigForurenetKlasse(jordKlassifikationTypeList);
                        var res = new ForureningsOpslagResult(ExternalDataProviderName.MiljoePortal);
                        res.ShortText = miljoePortalKlassifikation.ShortText;
                        res.LongText = miljoePortalKlassifikation.LongText;
                        res.Header = HeaderText;
                        res.JordKlassifikation = klass;
                        res.HeaderSortering = 1;
                        res.MiljoePortalKlassifikation = miljoePortalKlassifikation;

                        var shouldAdd = true;
                        foreach (var forureningsOpslagResult in resultList)
                        {
                            if (forureningsOpslagResult.MiljoePortalKlassifikation != res.MiljoePortalKlassifikation)
                                continue;

                            shouldAdd = false;
                            break;
                        }
                        if (shouldAdd)
                        {
                            resultList.Add(res);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                var errorResult = new ForureningsOpslagResult(ExternalDataProviderName.MiljoePortal);
                errorResult.ResultException = exception;
                resultList.Add(errorResult);
                Logger.LogException("MiljoePortalRepository. MapV1OrV2Result2: Failed.", exception);
                throw; // Send beskeden op til kaldende metode.
            }
            return resultList;
        }

        private static JordKlassifikationType GetKraftigForurenetKlasse(IList<JordKlassifikationType> jordKlassifikationTypeList)
        {
            var kl = (from k in jordKlassifikationTypeList
                      where k.Kode == (short)ForureningsKlasse.JordKraftigForurenet
                      || k.Kode == (short)ForureningsKlasse.Klasse1
                      select k).FirstOrDefault();
            return kl;
        }

        private static JordKlassifikationType GetLetForurenetKlasse(IList<JordKlassifikationType> jordKlassifikationTypeList)
        {
            var kl = (from k in jordKlassifikationTypeList
                      where k.Kode == (short)ForureningsKlasse.JordLetForurenet
                      || k.Kode == (short)ForureningsKlasse.Klasse2
                      //|| k.Kode == (short)ForureningsKlasse.Klasse3
                      select k).FirstOrDefault();

            return kl;
        }

        private static JordKlassifikationType GetRenJordKlasse(IList<JordKlassifikationType> jordKlassifikationTypeList)
        {
            var kl = (from k in jordKlassifikationTypeList
                      where k.Kode == (short)ForureningsKlasse.JordRen
                      || k.Kode == (short)ForureningsKlasse.Klasse4
                      select k).FirstOrDefault();

            return kl;
        }

        private List<ForureningsOpslagResult> MapOmraadeKlassificeringResult(XContainer resultXdocument, string serviceName, IList<JordKlassifikationType> jordKlassifikationTypeList)
        {
            var resultList = new List<ForureningsOpslagResult>();

            // Kolonner i output
            // OMR_KLASSIFICERING: Ana_krkode, Ana_krnavn
            try
            {
                var milPortalTypeList = GetMiljoePortalTypeList();

                // loop through returning geometries
                var localName = (serviceName.IndexOf(":") > -1 ? serviceName.Split(':')[1] : serviceName);
                var nodes = resultXdocument.Descendants().Where(x => x.Name.LocalName.ToString().Equals(localName, StringComparison.OrdinalIgnoreCase)).ToArray();
                foreach (var node in nodes)
                {

                    var result = new ForureningsOpslagResult(ExternalDataProviderName.MiljoePortal);
                    var analysekode = "-";
                    var anakodeElement = node.Elements().Where(x => x.Name.LocalName.ToString().Equals("Ana_krkode", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (anakodeElement != null)
                        analysekode = anakodeElement.Value;

                    MiljoePortalKlassifikation miljoePortalKlassifikation;
                    JordKlassifikationType jordKlassifikationType;
                    short shrtStatusKode;
                    if (short.TryParse(analysekode, out shrtStatusKode))
                    {
                        switch (shrtStatusKode)
                        {
                            case 3:
                                {
                                    jordKlassifikationType = GetKraftigForurenetKlasse(jordKlassifikationTypeList);
                                    miljoePortalKlassifikation = milPortalTypeList[5];
                                }
                                break;
                            case 2:
                                {
                                    jordKlassifikationType = GetLetForurenetKlasse(jordKlassifikationTypeList);
                                    miljoePortalKlassifikation = milPortalTypeList[4];
                                }
                                break;
                            case 1:
                                {
                                    jordKlassifikationType = GetRenJordKlasse(jordKlassifikationTypeList);
                                    miljoePortalKlassifikation = milPortalTypeList[3];
                                }
                                break;
                            default:
                                throw new ArgumentException("SystemFejl: Kunne ikke finde en korrekt jordklassifikation i databasen. Statuskode: " + analysekode);
                        }
                    }
                    else
                    {
                        throw new ArgumentException("SystemFejl: Ukendt statuskode modtaget fra miljøportalen. Statuskode: " + analysekode);
                    }

                    result.ShortText = miljoePortalKlassifikation.ShortText;
                    result.LongText = miljoePortalKlassifikation.LongText;
                    result.Header = HeaderText;
                    result.HeaderSortering = 1;
                    result.JordKlassifikation = jordKlassifikationType;
                    result.MiljoePortalKlassifikation = miljoePortalKlassifikation;

                    //Fjerner dubletter
                    var shouldAdd = true;
                    foreach (var forureningsOpslagResult in resultList)
                    {
                        if (forureningsOpslagResult.MiljoePortalKlassifikation != result.MiljoePortalKlassifikation)
                            continue;

                        shouldAdd = false;
                        break;
                    }
                    if (shouldAdd)
                    {
                        resultList.Add(result);
                    }
                }
            }
            catch (Exception exception)
            {
                var errorResult = new ForureningsOpslagResult(ExternalDataProviderName.MiljoePortal);
                errorResult.ResultException = exception;
                resultList.Add(errorResult);
                Logger.LogException("MiljoePortalRepository. MapOmraadeKlassificeringResult2: Failed.", exception);
            }
            return resultList;
        }

        private static Dictionary<short, MiljoePortalKlassifikation> GetMiljoePortalTypeList()
        {
            var list = new Dictionary<short, MiljoePortalKlassifikation>(5);

            list.Add(1, new MiljoePortalKlassifikation
            {
                Id = 1,
                ShortText = "Jordforurening, kortlagt på V1",
                ForureningsType = ForureningsKlasse.JordKraftigForurenet,
                LongText =
                "Arealet er kortlagt på vidensniveau 1 (V1) efter jordforureningsloven, da der har været aktiviteter eller anlæg, som kan have medført jordforurening på ejendommen. Der gælder særlige regler for jordflytning fra kortlagte arealer, jf. jordflytningsbekendtgørelsen. Kontakt evt. kommunen. Som udgangspunkt skal jorden undersøges for jordforurening i forbindelse med jordflytningen. Årsagen til kortlægningen skal indgå i planlægningen af prøvetagningen. Prøvetagning kan ske på lokaliteten, hvor den graves op, eller på et modtageanlæg, der er godkendt til kartering af jord."
            });

            list.Add(2, new MiljoePortalKlassifikation
            {
                Id = 2,
                ShortText = "Jordforurening, kortlagt på V2",
                ForureningsType = ForureningsKlasse.JordKraftigForurenet,
                LongText =
                "Arealet er kortlagt på vidensniveau 2 (V2) efter jordforureningsloven, da der er påvist jordforurening på ejendommen. Der gælder særlige regler for jordflytning fra kortlagte arealer, jf. jordflytningsbekendtgørelsen. Kontakt evt. kommunen. Som udgangspunkt skal jorden undersøges for jordforurening i forbindelse med jordflytningen. Årsagen til kortlægningen skal indgå i planlægningen af prøvetagningen. Prøvetagning kan ske på lokaliteten, hvor den graves op, eller på et modtageanlæg, der er godkendt til kartering af jord."
            });

            list.Add(3, new MiljoePortalKlassifikation
            {
                Id = 3,
                ShortText = "Områdeklassificeret, analysefrit område, ren jord (Kategori 1)",
                ForureningsType = ForureningsKlasse.JordRen,
                LongText =
                "Jorden kan, uden undersøgelse, bortskaffes til anlæg, som kan modtage REN JORD, med mindre der er tegn på forurening i jorden."
            });

            list.Add(4, new MiljoePortalKlassifikation
            {
                Id = 4,
                ShortText = "Områdeklassificeret, analysefrit område, let forurenet jord (Kategori 2)",
                ForureningsType = ForureningsKlasse.JordLetForurenet,
                LongText = "Jorden kan, uden undersøgelse, bortskaffes til anlæg, som kan modtage LET FORURENET JORD, med mindre der er tegn på kraftigere forurening i jorden."
            });

            list.Add(5, new MiljoePortalKlassifikation
            {
                Id = 5,
                ShortText = "Områdeklassificeret areal med analysepligt af forureningsgraden",
                ForureningsType = ForureningsKlasse.JordKraftigForurenet,
                LongText = "Jorden kan være lettere forurenet. Som udgangspunkt skal jorden undersøges for jordforurening i forbindelse med jordflytningen. Prøvetagning kan ske på lokaliteten, hvor den graves op, eller på et modtageanlæg, der er godkendt til kartering af jord."
            });

            return list;
        }


    }
}