using System;
using System.Configuration;
using System.Data.Spatial;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using Microsoft.SqlServer.Types;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models.JordForurening;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{

    /*
     * http://geoservice.plansystem.dk/wfs/
     */

    public class PlansystemRepository : IPlansystemRepository
    {
        private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Infrastructure.DataAccess.PlansystemRepository");
        private const string HeaderText = "Planlov og planlægning";
        private readonly string _wfsUrl;

        public PlansystemRepository()
        {
            _wfsUrl = ConfigurationManager.AppSettings["plansystemWfsUrl"];
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
            var result = new KonfliktSoegningLeverandoerResultat(HeaderText, null);

            var layerHandlers = new LayerHandler[]
            {
                new LayerHandler("theme_pdk_bevaringsvaerdigelandskaber_vedtaget_v", "Bevaringsværdigt landskab"),
            };

            try
            {
                if (geom == null)
                    throw new ArgumentException("SystemFejl: Der mangler en geometri, ved hentning af oplysninger! Kontakt den systemansvarlige.");

                foreach (var layerHandler in layerHandlers)
                {
                    var url = CreateInputParamPost(geom, layerHandler.Name);
                    var hit = false;
                    Exception error = null;
                    try
                    {
                        var serviceResult = CallWfsService(url);
                        hit = TjekReultatForHit(serviceResult, layerHandler.Name);
                    }
                    catch (Exception ex)
                    {
                        error = ex;
                        Logger.LogException(string.Format("PlansystemRepository.GetResultMidlertidigModtager -> Service kald fejlede for lag: '{0}' - Url: '{1}'", layerHandler, url), ex);
                    }
                    var lagResultat = new KonfliktSoegningServiceResultat(layerHandler.Name, layerHandler.Description, hit);
                    if (error != null)
                        lagResultat.MarkerSomFejlet(error);
                    result.ServiceResultater.Add(lagResultat);
                }
            }
            catch (Exception exception)
            {
                Logger.LogException("PlansystemRepository.GetResultMidlertidigModtager", exception);
                result.MarkerSomFejlet(exception);
            }
            return result;
        }

        private bool TjekReultatForHit(XDocument xDoc, string serviceName)
        {
            // Get namespace
            var appSettings = ConfigurationManager.AppSettings;
            var first = xDoc.Descendants().FirstOrDefault();
            if (first != null && first.Name.LocalName != "FeatureCollection")
                throw new Exception("Systemfejl: Plansystem: " + first.Value);

            /*
            <gml:featureMember>
                <public:fundogfortidsminder_areal_beskyttelse fid="fundogfortidsminder_areal_beskyttelse.fid-2353df31_15a5a44a5c0_2a38">
                    <...></...> 
                </public:fundogfortidsminder_areal_beskyttelse>
            </gml:featureMember>
             */

            // Tjek for om servicens navn er med i output, hvilket indikerer om der er et hit
            return xDoc.Descendants().Any(n => n.Name.LocalName == serviceName);
        }

        private string CreateInputParamPost(DbGeometry dbGeom, string serviceName)
        {
            // Bemærk at DbGeometry.AsGml() ikke accepteres som inputformat til deres service.
            // Derfor bukker vi en hat ud af deres eget outputformat, og genbruger dette til at fodre med.

            // Lav om til DbGeometry om til SqlGeometry, da den håndterer at man trækker points ud:
            var pointCulture = new CultureInfo("en-US");
            var sqlGeom = SqlGeometry.STGeomFromWKB(new SqlBytes(dbGeom.AsBinary()), dbGeom.CoordinateSystemId);
            sqlGeom.MakeValid();

            var points = new StringBuilder();
            for (var i = 1; i <= sqlGeom.STNumPoints(); i++)
            {
                var entry = sqlGeom.STPointN(i);
                if (i > 1)
                    points.Append(" ");
                points.AppendFormat("{0},{1}", entry.STX.Value.ToString(pointCulture), entry.STY.Value.ToString(pointCulture));
            }
            
            var filter =
                @"<ogc:Filter xmlns:ogc='http://www.opengis.net/ogc'>" +
                @"<ogc:Intersects>" +
                @"<ogc:PropertyName></ogc:PropertyName>" +

                @"<gml:MultiPolygon srsName='http://www.opengis.net/gml/srs/epsg.xml#25832'>" +
                @"    <gml:polygonMember>" +
                @"        <gml:Polygon>" +
                @"            <gml:outerBoundaryIs>" +
                @"                <gml:LinearRing>" +
                @"                    <gml:coordinates xmlns:gml='http://www.opengis.net/gml' decimal='.' cs=',' ts=' '>" +
                points +
                @"                    </gml:coordinates>" +
                @"                </gml:LinearRing>" +
                @"            </gml:outerBoundaryIs>" +
                @"        </gml:Polygon>" +
                @"    </gml:polygonMember>" +
                @"</gml:MultiPolygon>" +

                @"</ogc:Intersects>" +
                @"</ogc:Filter>";
            
            var requestStr =
                @"<?xml version='1.0' encoding='utf-8'?>" +
                @"<GetFeature " +
                @"	xmlns='http://www.opengis.net/wfs' " +
                @"	xmlns:dmp='" + _wfsUrl + "' " +
                @"	xmlns:ogc='http://www.opengis.net/ogc' " +
                @"	xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' " +
                @"	xmlns:gml='http://www.opengis.net/gml' " +
                @"	service='WFS' " +
                @"	version='1.0.0' " +
                @"	outputFormat='GML2' " +
                @"	maxFeatures='50' " +
                @"	handle='' >" +
                @"		<Query typeName='pdk:" + serviceName + "' srsName='EPSG:25832' >" +
                @"			" + filter +
                @"		</Query>" +
                @"</GetFeature>";

            return requestStr;
        }

        private XDocument CallWfsService(string inputUrl)
        {
            XDocument resultXdocument;

            var httpWReq = (HttpWebRequest)WebRequest.Create(_wfsUrl);
            var data = Encoding.ASCII.GetBytes(inputUrl);

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

    }
}
