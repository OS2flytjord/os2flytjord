using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Net;
using System.Text;
using System.Xml.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
  public class MatrikelRepository : GenericRepository<Matrikel>, IMatrikelRepository
  {
    private static readonly ILogger logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Infrastructure.DataAccess.MatrikelRepository");
    private const int SpatialReference = 25832;

    public MatrikelRepository(IConfigService configService)
      : base(configService)
    {
    }
    /// <summary>
    /// Get a list of matrikler
    /// </summary>
    /// <param name="wkt">a point wkt or a polygon wkt</param>
    public IList<Matrikel> GetListFromWfsService(string wkt)
    {
      var startTime = DateTime.Now;

      var inputUrl = "";
      List<Matrikel> matrikelList = null;
      try
      {
        // Create Input
        var inputGeometry = DbGeometry.FromText(wkt, SpatialReference);

        inputUrl = CreateInputUrl(inputGeometry);

        // Call Service
        var xdocResult = CallWfsService(inputUrl);

        // Map Result
        matrikelList = MapResult(xdocResult);

        // Handling that we get to many matrikler as result, because of query BBOX
        if (inputGeometry.SpatialTypeName == "Polygon")
        {
          // have to shrink the polygon to account for decimal errors
          var shrinkedPolygon = inputGeometry.Buffer(-1);
          var matrikelFinalList = new List<Matrikel>();
          foreach (var matrikel in matrikelList)
          {
            if (shrinkedPolygon.Intersects(matrikel.Geom))
            {
              matrikelFinalList.Add(matrikel);
            }
          }
          matrikelList = matrikelFinalList;
        }
      }
      catch (Exception exception)
      {
        logger.LogException("MatrikelRepository. GetList: Failed. Input: " + inputUrl, exception);
      }

      var timeUsed = DateTime.Now - startTime;
      logger.LogInfo("GetList call time: " + timeUsed.TotalSeconds + " seconds.");

      return matrikelList;
    }

    /// <summary>
    /// Create the input parameters
    /// </summary>
    private static string CreateInputUrl(DbGeometry inputGeometry)
    {
      var appSettings = System.Configuration.ConfigurationManager.AppSettings;
      var matrikelWfsUrl = appSettings["MatrikelWfsUrl"];
      const string typeName = "ugis:T510";
      const string requestStr = "?Site=Ejendomsdata&Page=Jordstykke&request=GetFeature&TypeName=" + typeName + "&MaxFeatures=100&service=WFS";

      var geom = inputGeometry;

      var filter = "";
      switch (geom.SpatialTypeName)
      {
        case "Point":
          {
            // *********************************
            // if it is a point; use intersects
            // *********************************
            var inputGeometryGml = geom.AsGml();
            // have to remove the xml tag
            inputGeometryGml = inputGeometryGml.Remove(0, 38);
            // Sådan ser den ud: "<Point xmlns:gml=""http://www.opengis.net/gml""><pos>568124.29904483 6219190.04749136</pos></Point>";
            // Replace mellemrum med komma (for at det virker)
            var str1 = inputGeometryGml.Substring(0, 20);
            var str2 = inputGeometryGml.Substring(20);
            str2 = str2.Replace(" ", ",");
            inputGeometryGml = String.Format("{0}{1}", str1, str2);

            filter =
                @"&Filter=" +
                @"<ogc:Filter xmlns:ogc=""http://www.opengis.net/ogc"">" +
                @"<ogc:Intersects>" +
                @"<ogc:PropertyName>ugis:CG_GEOMETRY</ogc:PropertyName>" +
                inputGeometryGml +
                @"</ogc:Intersects>" +
                @"</ogc:Filter>";
          }
          break;

        case "LineString":
          {
            // *********************************
            // if it is a LineString; use intersects
            // *********************************
            var inputGeometryGml = geom.AsGml();
            // have to remove the xml tag
            inputGeometryGml = inputGeometryGml.Remove(0, 38);
            // Sådan ser den ud: 
            //<LineString xmlns="http://www.opengis.net/gml"><posList>
            //		575099.56051933451 6226417.7570828721 575122.36051933456 6226411.7570828721</posList></LineString>";


            var strStartXml = inputGeometryGml.Substring(0, 56);

            var strPoslist = inputGeometryGml.Substring(56, inputGeometryGml.Length - (23 + 56));

            var strEndXml = inputGeometryGml.Substring(inputGeometryGml.Length - 23);

            var coordList = strPoslist.Split(' ');
            var strCoordXml = new StringBuilder();
            var isX = true;
            var i = 0;
            foreach (var s in coordList)
            {
              if (i == 0)
              {
                // første
                strCoordXml.Append(s);
                isX = false;
              }
              else
              {
                if (isX)
                {
                  strCoordXml.Append(" ");
                  strCoordXml.Append(s);
                  isX = false;
                }
                else
                {
                  strCoordXml.Append(",");
                  strCoordXml.Append(s);
                  isX = true;
                }
              }
              i++;
            }

            inputGeometryGml = String.Format("{0}{1}{2}", strStartXml, strCoordXml, strEndXml);

            filter =
                @"&Filter=" +
                @"<ogc:Filter xmlns:ogc=""http://www.opengis.net/ogc"">" +
                @"<ogc:Intersects>" +
                @"<ogc:PropertyName>ugis:CG_GEOMETRY</ogc:PropertyName>" +
                inputGeometryGml +
                @"</ogc:Intersects>" +
                @"</ogc:Filter>";
          }
          break;

        case "Polygon":
          {
            // *********************************
            // if it is a polygon; use BBOX
            // *********************************

            var lowerLeftCorner = geom.Envelope.PointAt(2);
            var upperRigthCorner = geom.Envelope.PointAt(4);
            var lowCornX = lowerLeftCorner.XCoordinate;
            var lowCornY = lowerLeftCorner.YCoordinate;
            var uppCornX = upperRigthCorner.XCoordinate;
            var uppCornY = upperRigthCorner.YCoordinate;

            var strLowCornX = lowCornX.ToString().Replace(",", ".");
            var strLowCornY = lowCornY.ToString().Replace(",", "."); 
            var strUppCornX = uppCornX.ToString().Replace(",", "."); 
            var strUppCornY = uppCornY.ToString().Replace(",", "."); 

            var coordinates = String.Format("{0},{1} {2},{3}",
                strLowCornX, strLowCornY, strUppCornX, strUppCornY);
            filter =
                @"&Filter=" +
                @"<ogc:Filter xmlns:ogc=""http://www.opengis.net/ogc"">" +
                @"<ogc:BBOX>" +
                @"<ogc:PropertyName>CG_GEOMETRY</ogc:PropertyName>" +
                @"<gml:Box xmlns:gml=""http://www.opengis.net/gml"">" +
                @"<gml:coordinates decimal=""."" ts="" "" cs="","">" + coordinates + "</gml:coordinates>" +
                @"</gml:Box>" +
                @"</ogc:BBOX>" +
                @"</ogc:Filter>";
          }
          break;
      }

      var inputUrl = matrikelWfsUrl + requestStr + filter;
      return inputUrl;
    }

    /// <summary>
    /// Call service
    /// </summary>
    private static XDocument CallWfsService(string inputUrl)
    {
      XDocument resultXdocument;
      var wrGeturl = WebRequest.Create(inputUrl);
      using (var stream = wrGeturl.GetResponse().GetResponseStream())
      {
        resultXdocument = XDocument.Load(stream);
      }
      return resultXdocument;
    }

    /// <summary>
    /// Map the result
    /// </summary>
    private static List<Matrikel> MapResult(XContainer resultXdocument)
    {
      var matrikelList = new List<Matrikel>();
      try
      {
        // Create namespaces
        XNamespace uGisNamespace = @"http://www.ugis.dk/wfs";
        XNamespace gmlNamespace = @"http://www.opengis.net/gml";

        // loop through returning geometries
        foreach (var xElememt in resultXdocument.Descendants(uGisNamespace + "T510"))
        {
          var matrikel = new Matrikel();

          var outElement = xElememt.Element(uGisNamespace + "landsejerlavskode");
          if (outElement != null) matrikel.Ejerlav = outElement.Value;


          outElement = xElememt.Element(uGisNamespace + "ejerlavsnavn");
          if (outElement != null) matrikel.Ejerlavsnavn = outElement.Value;

          outElement = xElememt.Element(uGisNamespace + "matrikelnummer");
          if (outElement != null) matrikel.Matrikelnr = outElement.Value;

          outElement = xElememt.Element(uGisNamespace + "sognenavn");
          if (outElement != null) matrikel.Sogn = outElement.Value;

          var geomElement = xElememt.Element(uGisNamespace + "CG_GEOMETRY");
          if (geomElement != null)
            outElement = geomElement.Element(gmlNamespace + "Polygon");

          if (outElement != null)
          {
            var gml = outElement.ToString();

            // Problem dbGeometry only supports gml 3
            // But we get gml 2
            // So, for this gml to work:
            //1) Remove srsName attribute to avoid the error 24130
            //2) Replace 'outerBoundaryIs' by 'exterior'
            //3) Replace 'coordinates' by 'posList'
            //4) Replace commas in coordinates by white blanks

            gml = gml.Replace(@"srsName=""EPSG:25832""", "");
            gml = gml.Replace(@"decimal="".""", "");
            gml = gml.Replace(@"ts="" """, "");
            gml = gml.Replace(@"cs="",""", "");

            gml = gml.Replace("outerBoundaryIs", "exterior");
            gml = gml.Replace("innerBoundaryIs", "interior");
            gml = gml.Replace("coordinates", "posList");
            gml = gml.Replace(",", " ");

            var geom = DbGeometry.FromGml(gml, SpatialReference);
            matrikel.Geom = geom;
          }
          matrikelList.Add(matrikel);
        }
      }
      catch (Exception exception)
      {
        logger.LogException("MatrikelRepository. Map: Failed", exception);
        throw;
      }
      return matrikelList;
    }

    #region *** output eksempel ***

    //<ugis:CG_ID>227881</ugis:CG_ID> 
    //<ugis:CG_STYLE>Pen (1,2,3158064) Brush (2,16777168,16777215)</ugis:CG_STYLE> 
    //<ugis:featureID>374533</ugis:featureID> 
    //<ugis:featureType>Skelpolygon matrikelnummer</ugis:featureType> 
    //<ugis:featureCode>9034</ugis:featureCode> 
    //<ugis:datasetIdentifier/> 
    //<ugis:dataQualitySpecification/> 
    //<ugis:dataQualityStatement/> 
    //<ugis:dataQualityDescription/> 
    //<ugis:dataQualityProcessor/> 
    //<ugis:dataQualityResponsibleParty/> 
    //<ugis:timeOfCreation>1980-03-13T00:00:00</ugis:timeOfCreation> 
    //<ugis:timeOfPublication>1980-03-13T00:00:00</ugis:timeOfPublication> 
    //<ugis:timeOfRevision>2012-08-09T09:24:25.5</ugis:timeOfRevision> 
    //<ugis:sfe_SagsID>7814079</ugis:sfe_SagsID> 
    //<ugis:sfe_Journalnummer>U1979/15545</ugis:sfe_Journalnummer> 
    //<ugis:sfe_Registreringsdato>1980-03-13T00:00:00</ugis:sfe_Registreringsdato> 
    //<ugis:sfe_Ejendomsnummer>4237962</ugis:sfe_Ejendomsnummer> 
    //<ugis:esr_Ejendomsnummer>7510759134</ugis:esr_Ejendomsnummer> 
    //<ugis:sfe_Registrering/> <ugis:l_NoteringsType/> 
    //<ugis:kms_SagsID>7814079</ugis:kms_SagsID> 
    //<ugis:kms_Journalnummer>U1979/15545</ugis:kms_Journalnummer> 
    //<ugis:registreringsdato>1980-03-13T00:00:00</ugis:registreringsdato> 
    //<ugis:ejerlavsnavn>Kolt By, Kolt</ugis:ejerlavsnavn> 
    //<ugis:landsejerlavskode>1030456</ugis:landsejerlavskode> 
    //<ugis:matrikelnummer>5hl</ugis:matrikelnummer> 
    //<ugis:faelleslod>nej</ugis:faelleslod> 
    //<ugis:moderjordstykke>0</ugis:moderjordstykke> 
    //<ugis:supmSagsID>0</ugis:supmSagsID> 
    //<ugis:skelforretningsSagsID>0</ugis:skelforretningsSagsID> 
    //<ugis:registreretAreal>333</ugis:registreretAreal> 
    //<ugis:arealBeregn>o</ugis:arealBeregn> 
    //<ugis:vejAreal>0</ugis:vejAreal> 
    //<ugis:vejArealBeregn>b</ugis:vejArealBeregn> 
    //<ugis:vandArealBeregn>ukendt</ugis:vandArealBeregn> 
    //<ugis:arealType/> 
    //<ugis:regionskode>1082</ugis:regionskode> 
    //<ugis:regionsnavn>Region Midtjylland</ugis:regionsnavn> 
    //<ugis:kommunekode>751</ugis:kommunekode> 
    //<ugis:kommunenavn>Århus Kommune</ugis:kommunenavn> 
    //<ugis:sognekode>8108</ugis:sognekode> 
    //<ugis:sognenavn>Kolt</ugis:sognenavn> 
    //<ugis:retskredskode>1165</ugis:retskredskode> 
    //<ugis:retskredsnavn>Århus retskreds</ugis:retskredsnavn>
    //<ugis:CG_GEOMETRY>
    //  <gml:Polygon srsName="EPSG:25832">
    //      <gml:outerBoundaryIs>
    //          <gml:LinearRing>
    //              <gml:coordinates decimal="." cs="," ts=" ">568392.176,6218892.646 568391.308,6218894.286 568390.22,6218895.79 568388.934,6218897.131 568387.475,6218898.279 568384.878,6218898.621 568359.91,6218901.918 568361.288,6218891.033 568393.022,6218886.669 568392.176,6218892.646</gml:coordinates>
    //          </gml:LinearRing>
    //      </gml:outerBoundaryIs>
    //  </gml:Polygon>
    //</ugis:CG_GEOMETRY>


    #endregion *** output eksempel ***

  }
}
