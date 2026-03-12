//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.SqlServer.Types;
//using UGIS.Geometry;
//using UGIS.Mapping.Gml;
//using UGIS.Mapping.Ogc.Wkb;
//using UGIS.Mapping.Ogc.Wkt;

//namespace Niras.Jordflytning.Infrastructure.GeometryUtil
//{
//  public static class GeometryTools
//  {

//    public static IList<string> GetWktListFromWkt(string wkt)
//    {
//      IWkbGeometry geometry = WktReader.Read(wkt);

//      var result = new List<string>();
//      if (geometry.GeometryType == WkbGeometryType.GeometryCollection)
//      {
//        WkbGeometryCollection geoCol = (WkbGeometryCollection)geometry;

//        foreach (IWkbGeometry wkbGeometry in geoCol)
//        {
//          switch (wkbGeometry.GeometryType)
//          {
//            case WkbGeometryType.Point:
//              result.Add(GetWktFromWkb(wkbGeometry));
//              break;
//            case WkbGeometryType.Polygon:
//              result.Add(GetWktFromWkb(wkbGeometry));
//              break;
//            case WkbGeometryType.GeometryCollection:
//              result.AddRange(GetWktListFromWkt(GetWktFromWkb(wkbGeometry)));
//              break;
//            default:
//              throw new Exception("Geometry type unknow 2 (" + geometry.GeometryType + " ** " + wkt + " )");
//          }
//        }
//      }
//      else if (geometry.GeometryType == WkbGeometryType.Point)
//        result.Add(GetWktFromWkb(geometry));
//      else if (geometry.GeometryType == WkbGeometryType.Polygon)
//        result.Add(GetWktFromWkb(geometry));
//      else
//        throw new Exception("Geometry type unknow (" + geometry.GeometryType + " ** " + wkt + ")");

//      return result;
//    }


//    public static string GetPolygonWktFromLineStringWkt(string lineStringWkt)
//    {
//      IWkbGeometry geometry = GetWkbFromWkt(lineStringWkt);

//      if (geometry.GeometryType == WkbGeometryType.LineString)
//      {
//        WkbLineString lineGeo = (WkbLineString)geometry;

//        Ring2 ring = new Ring2();
//        foreach (IVector2 vector2 in lineGeo)
//          ring.Add(vector2);

//        return GetWktFromWkb(new WkbPolygon(ring));
//      }

//      return lineStringWkt;
//    }

//    public static string GetWktFromWktList(IList<string> wktList)
//    {
//      var geoCol = new WkbGeometryCollection();
//      geoCol.AddRange(wktList.Select(s => WktReader.Read(s)));

//      return GetWktFromWkb(geoCol);
//    }


//    public static string GetWktFromWkb(IWkbGeometry geometry)
//    {
//      return WkbTools.ToWkt(geometry);
//    }

//    public static IWkbGeometry GetWkbFromWkt(string wkt)
//    {
//      return WktReader.Read(wkt);
//    }

//    public static bool ParseWktFromGml(string gml, out string wkt)
//    {
//      try
//      {
//        IWkbGeometry wkb = GmlTools.ParseGml(gml);

//        wkt = WkbTools.ToWkt(wkb);

//        return true;
//      }
//      catch (Exception ex)
//      {
//        wkt = "";
//      }
//      return false;
//    }

//    public static string GetWktFromGml(string gml)
//    {
//      IWkbGeometry wkb = GmlTools.ParseGml(gml);

//      return WkbTools.ToWkt(wkb);
//    }

//    public static bool GmlGeometryOverlaps(string wkt, string gml)
//    {
//      IWkbGeometry wkb1 = GmlTools.ParseGml(gml);
//      IWkbGeometry wkb2 = WktReader.Read(wkt);

//      return GeometryOverlaps(wkb1, wkb2);
//    }

//    public static bool WktGeometryOverlaps(string wkt1, string wkt2)
//    {
//      IWkbGeometry wkb1 = WktReader.Read(wkt1);
//      IWkbGeometry wkb2 = WktReader.Read(wkt2);

//      return GeometryOverlaps(wkb1, wkb2);
//    }

//    public static bool GeometryOverlaps(IWkbGeometry wkb1, IWkbGeometry wkb2)
//    {
//      SqlGeometry sql1 = SqlWkbGeometry.ConvertWkbGeometryToSqlGeometry(wkb1);
//      SqlGeometry sql2 = SqlWkbGeometry.ConvertWkbGeometryToSqlGeometry(wkb2);

//      return SqlGeometryOverlaps(sql1, sql2);
//    }

//    public static bool GeometryOverlapsWkt(IList<IWkbGeometry> wkbList, string wkt)
//    {
//      SqlGeometry gmlSql = SqlWkbGeometry.ConvertWkbGeometryToSqlGeometry(WktReader.Read(wkt));
//      foreach (IWkbGeometry wkbGeometry in wkbList)
//      {
//        SqlGeometry sqlGeometry = SqlWkbGeometry.ConvertWkbGeometryToSqlGeometry(wkbGeometry);

//        if (sqlGeometry == null)
//          continue;

//        if (SqlGeometryOverlaps(gmlSql, sqlGeometry))
//          return true;
//      }

//      return false;
//    }

//    public static bool GeometryOverlapsGml(IList<IWkbGeometry> wkbList, string gml)
//    {
//      var gmlSql = SqlWkbGeometry.ConvertWkbGeometryToSqlGeometry(GmlTools.ParseGml(gml));

//      return gmlSql != null && wkbList.Any(geometry => SqlGeometryOverlaps(gmlSql, SqlWkbGeometry.ConvertWkbGeometryToSqlGeometry(geometry)));
//    }

//    private static bool SqlGeometryOverlaps(SqlGeometry sql1, SqlGeometry sql2)
//    {
//      if ((sql1 == null) || (sql2 == null))
//        return false;

//      var sqlTemp1 = sql1.MakeValid();
//      var sqlTemp2 = sql2.MakeValid();

//      if (sqlTemp1.STIntersects(sqlTemp2).Value)
//        return true;

//      return (sqlTemp1.STContains(sqlTemp2).Value) || (sqlTemp2.STContains(sqlTemp1).Value);
//    }

//  }
//}
