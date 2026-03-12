using Microsoft.SqlServer.Types;
using Microsoft.Web.Services3.Referral;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models.MatrikelOpslag;
using Niras.Jordflytning.Library.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Spatial;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
    public class MatrikelOpslagRepository : IMatrikelOpslagRepository
    {
        private static readonly ILogger logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Infrastructure.DataAccess.MatrikelOpslagRepository");
        private const int SpatialReference = 25832;

        public string GetEsrEjendomsnummer(string ejerlavkode, string matrikelnr)
        {
            string esrEjendomsnummer = null;
            if (!string.IsNullOrWhiteSpace(ejerlavkode) && !string.IsNullOrWhiteSpace(matrikelnr))
            {
                string requestUrl = string.Format("https://dawa.aws.dk/jordstykker/{0}/{1}", ejerlavkode, matrikelnr);
                string serverResponse = null;
                try
                {
                    var request = WebRequest.Create(requestUrl);
                    request.Method = "GET";
                    request.ContentType = "application/json";
                    using (var response = request.GetResponse() as HttpWebResponse)
                    {
                        if (response != null && response.StatusCode != HttpStatusCode.OK)
                            throw new Exception("MatrikelOpslag - Matriklerne kunne ikke hentes.");
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            serverResponse = reader.ReadToEnd();
                            dynamic result = JObject.Parse(serverResponse);
                            esrEjendomsnummer = result.esrejendomsnr;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogException("MatrikelOpslagRepository. GetEsrEjendomsnummer: Failed. Input: " + requestUrl + " Server response: " + serverResponse, ex);
                }
            }
            return esrEjendomsnummer;
        }

        class DawaJordstykke
        {
            public double visueltcenter_x;
            public double visueltcenter_y;
            public string ejerlavnavn;
        }

        Niras.Jordflytning.Core.Models.MatrikelOpslag.Properties GetProperties(string ejerlavKode, string matrikelNummer)
        {
            using (var wc = new System.Net.WebClient())
            {
                var result = new MatrikelOpslagResultat
                {
                    features = new List<Niras.Jordflytning.Core.Models.MatrikelOpslag.Feature>()
                };

                var baseUrl = "https://api.dataforsyningen.dk/";
                var path = $"jordstykker/{ejerlavKode}/{matrikelNummer}?struktur=flad&srid=25832";

                wc.BaseAddress = baseUrl;
                wc.Encoding = Encoding.UTF8;
                var s = wc.DownloadString(path);
                var jordstykke = JsonConvert.DeserializeObject<DawaJordstykke>(s);

                return new Niras.Jordflytning.Core.Models.MatrikelOpslag.Properties
                {
                    centroid_x = jordstykke.visueltcenter_x.ToString(CultureInfo.InvariantCulture),
                    centroid_y = jordstykke.visueltcenter_y.ToString(CultureInfo.InvariantCulture),
                    ejerlav_navn = jordstykke.ejerlavnavn,
                    ejerlav_kode = ejerlavKode,
                    matnr = matrikelNummer
                };
            }
        }

        public MatrikelOpslagResultat Get(string ejerlavkode, string matrikelnr)
        {
            var url = ConfigurationManager.AppSettings["matrikelWfsRequestUrl"];
            try
            {
                if (!string.IsNullOrWhiteSpace(ejerlavkode) && !string.IsNullOrWhiteSpace(matrikelnr))
                {
                    var query = CreateWfsInputQuery(matrikelnr, ejerlavkode);
                    var doc = GetDocumentFromWfsService(url, query);
                    var result = ParseDoc(doc);
                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.LogException($"MatrikelOpslagRepository. Get: Failed. Input: {url}", ex);
            }
            return null;
        }

        public MatrikelOpslagResultat Get(string wkt)
        {
            var url = ConfigurationManager.AppSettings["matrikelWfsRequestUrl"];
            try
            {
                var inputGeometry = DbGeometry.FromText(wkt, SpatialReference);
                if (inputGeometry != null)
                {
                    DbGeometry inputPolygon = null;
                    if (inputGeometry.SpatialTypeName == "Polygon")
                        inputPolygon = inputGeometry;
                    if (inputGeometry.SpatialTypeName == "LineString")
                        inputPolygon = inputGeometry.Buffer(1);
                    if (inputGeometry.SpatialTypeName == "Point")
                        inputPolygon = inputGeometry.Buffer(1).Envelope;

                    if (inputPolygon != null && inputPolygon.SpatialTypeName == "Polygon" && inputPolygon.PointCount.HasValue && inputPolygon.PointCount.Value > 0)
                    {
                        var query = CreateWfsInputQuery(inputPolygon);
                        var doc = GetDocumentFromWfsService(url, query);
                        var result = ParseDoc(doc);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogException($"MatrikelOpslagRepository. Get: Failed. Input: {url}", ex);
            }
            return null;
        }

        private MatrikelOpslagResultat ParseDoc(XDocument doc)
        {
            var result = new MatrikelOpslagResultat
            {
                features = new List<Feature>()
            };

            XNamespace nsMat = "http://data.gov.dk/schemas/matrikel/1";
            XNamespace nsGml = "http://www.opengis.net/gml/3.2";
            XNamespace nsWfs = "http://www.opengis.net/wfs/2.0";

            var root = doc.Root;
            var count = int.Parse(root.Attribute("numberReturned").Value);

            var memberElements = root.Descendants(nsWfs + "member");
            foreach (var memberElement in memberElements)
            {
                var geomElement = memberElement.Descendants(nsMat + "geometri").FirstOrDefault();
                if (geomElement == null)
                    continue;

                var matnr = memberElement.Descendants(nsMat + "matrikelnummer").Select(x => x.Value).First();
                var ejerlav_kode = memberElement.Descendants(nsMat + "ejerlavskode").Select(x => x.Value).First();

                var coordinates = new List<List<List<string>>>();
                var posList = memberElement.Descendants(nsGml + "posList").FirstOrDefault();
                if (posList != null && !string.IsNullOrWhiteSpace(posList.Value))
                {
                    var parts = posList.Value
                       .Split(' ')
                       .ToArray();

                    if (parts.Length > 1 && parts.Length % 2 == 0)
                    {
                        var sets = new List<List<string>>();
                        var setCounter = -1;
                        for (var i = 0; i < parts.Length; i++)
                        {
                            if (i % 2 == 0)
                            {
                                setCounter++;
                                sets.Add(new List<string>());
                            }
                            sets[setCounter].Add(parts[i]);
                        }
                        coordinates.Add(sets);
                    }
                }
                result.features.Add(
                   new Feature
                   {
                       properties = GetProperties(ejerlav_kode, matnr),
                       geometry = new Polygon
                       {
                           coordinates = coordinates,
                           type = "Polygon"
                       }
                   }
                );
            }
            return result;
        }

        private XDocument GetDocumentFromWfsService(string url, string query)
        {
            XDocument result;
            var httpWReq = (HttpWebRequest)WebRequest.Create(url);
            var data = Encoding.UTF8.GetBytes(query);
            httpWReq.Method = "POST";
            httpWReq.ContentType = "text/xml";
            httpWReq.ContentLength = data.Length;
            using (var stream = httpWReq.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            using (var stream = httpWReq.GetResponse().GetResponseStream())
            {
                result = XDocument.Load(stream);
            }
            return result;
        }

        private string CreateWfsInputQuery(DbGeometry dbGeom)
        {
            var pointCulture = new CultureInfo("en-US");
            var sqlGeom = SqlGeometry.STGeomFromWKB(new SqlBytes(dbGeom.AsBinary()), dbGeom.CoordinateSystemId);
            sqlGeom.MakeValid();

            var points = new StringBuilder();
            for (var i = 1; i <= sqlGeom.STNumPoints(); i++)
            {
                var entry = sqlGeom.STPointN(i);
                if (i > 1)
                    points.Append(" ");
                points.AppendFormat("{0} {1}", entry.STX.Value.ToString(pointCulture), entry.STY.Value.ToString(pointCulture));
            }

            var q = new StringBuilder();
            q.AppendLine("<GetFeature version=\"2.0.0\"");
            q.AppendLine("            service=\"WFS\"");
            q.AppendLine("            xmlns=\"http://www.opengis.net/wfs/2.0\"");
            q.AppendLine("            xmlns:mat=\"http://data.gov.dk/schemas/matrikel/1\">");
            q.AppendLine("	<Query typeNames=\"mat:Jordstykke_Gaeldende\">");
            q.AppendLine("		<Filter xmlns=\"http://www.opengis.net/fes/2.0\">");
            q.AppendLine("		<Intersects xmlns=\"http://www.opengis.net/fes/2.0\">");
            q.AppendLine("		<ValueReference>mat:geometri</ValueReference>");
            q.AppendLine("		<Polygon srsName=\"EPSG:25832\" xmlns=\"http://www.opengis.net/gml/3.2\">");
            q.AppendLine("		<exterior>");
            q.AppendLine("			<LinearRing>");
            q.AppendLine($"				<posList>{points}</posList>");
            q.AppendLine("			</LinearRing>");
            q.AppendLine("		</exterior>");
            q.AppendLine("	</Polygon>");
            q.AppendLine("</Intersects>");
            q.AppendLine("</Filter>");
            q.AppendLine("</Query>");
            q.AppendLine("</GetFeature>");
            return q.ToString();
        }

        private string CreateWfsInputQuery(string matrikelnummer, string ejelavskode)
        {
            var q = new StringBuilder();
            q.AppendLine("<GetFeature version=\"2.0.0\"");
            q.AppendLine("            service=\"WFS\"");
            q.AppendLine("            xmlns=\"http://www.opengis.net/wfs/2.0\"");
            q.AppendLine("            xmlns:mat=\"http://data.gov.dk/schemas/matrikel/1\">");
            q.AppendLine("	<Query typeNames=\"mat:Jordstykke_Gaeldende\">");
            q.AppendLine("		<Filter xmlns=\"http://www.opengis.net/fes/2.0\">");
            q.AppendLine("		<And>");

            q.AppendLine("		   <PropertyIsEqualTo>");
            q.AppendLine("		      <ValueReference>mat:matrikelnummer</ValueReference>");
            q.AppendLine($"		      <Literal>{matrikelnummer}</Literal>");
            q.AppendLine("		   </PropertyIsEqualTo>");
            q.AppendLine("		   <PropertyIsEqualTo>");
            q.AppendLine("		      <ValueReference>mat:ejerlavskode</ValueReference>");
            q.AppendLine($"		      <Literal>{ejelavskode}</Literal>");
            q.AppendLine("		   </PropertyIsEqualTo>");
            q.AppendLine("		</And>");
            q.AppendLine("</Filter>");
            q.AppendLine("</Query>");
            q.AppendLine("</GetFeature>");
            return q.ToString();
        }

        public IList<Core.Models.Matrikel> GetListFromRestService(string wkt)
        {
            var startTime = DateTime.Now;
            var matrikler = Get(wkt);
            var res = MapMatrikler(matrikler);
            var timeUsed = DateTime.Now - startTime;
            logger.LogInfo("MatrikelRest call time: " + timeUsed.TotalSeconds + " seconds.");
            return res;
        }

        private List<Niras.Jordflytning.Core.Models.Matrikel> MapMatrikler(MatrikelOpslagResultat matrikelOpslagResultat)
        {
            string wkt = "";
            var res = new List<Core.Models.Matrikel>();
            if (matrikelOpslagResultat != null && matrikelOpslagResultat.features != null && matrikelOpslagResultat.features.Any())
            {

                foreach (var feature in matrikelOpslagResultat.features)
                {

                    var o = new Core.Models.Matrikel();
                    if (feature.properties != null)
                    {
                        o.Ejerlav = feature.properties.ejerlav_kode;
                        o.Ejerlavsnavn = feature.properties.ejerlav_navn;
                        o.Matrikelnr = feature.properties.matnr;
                    }

                    string wktCoordiantes = "";
                    if (feature.geometry != null)
                    {
                        if (feature.geometry.type.ToLower() == "polygon")
                        {
                            if (feature.geometry.coordinates != null && feature.geometry.coordinates.Any())
                            {
                                var polygon = feature.geometry.coordinates.First();
                                //foreach (var polygon in feature.geometry.coordinates) //Der bør kun være en geometri i matrikelen
                                //{
                                wktCoordiantes += " POLYGON(("; //Start polygon (Der kan være flere)

                                foreach (var c in polygon)
                                {
                                    if (c != null && c.Any() && c.Count() == 2)
                                    {
                                        wktCoordiantes += c[0] + " " + c[1] + ",";
                                    }

                                }
                                if (!string.IsNullOrEmpty(wktCoordiantes))
                                {
                                    //Fjerner det sidste komma som adskilder punkterne i polygonet.
                                    wktCoordiantes = wktCoordiantes.Substring(0, wktCoordiantes.Length - 1);
                                }

                                wktCoordiantes += ")),";//Start polygon (Der kan være flere)
                                //}
                                //Fjerner det sidste komma som adskilder polygonerne.
                                wktCoordiantes = wktCoordiantes.Substring(0, wktCoordiantes.Length - 1);

                                //Laver Wkt til geometry
                                try
                                {
                                    //wkt = "GEOMETRYCOLLECTION(" + wktCoordiantes + ")";
                                    wkt = wktCoordiantes;
                                    var geom = DbGeometry.FromText(wkt, SpatialReference);
                                    if (geom != null)
                                    {
                                        o.Geom = geom;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logger.LogException("MatrikelOpslagRepository: Fejl i konverteringen af wkt til geometri. WKT:" + wkt, ex);

                                }
                            }

                        }
                    }
                    res.Add(o);
                }
            }
            return res;
        }
    }
}
