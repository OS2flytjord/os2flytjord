//using System;
//using Microsoft.SqlServer.Types;
//using UGIS.Mapping.Ogc.Wkb;

//namespace Niras.Jordflytning.Infrastructure.GeometryUtil
//{
//    public static class SqlWkbGeometry
//    {

//        public static SqlGeometry ConvertWkbGeometryToSqlGeometry(IWkbGeometry geometry)
//        {
//            SqlGeometry result = null;
//            try
//            {
//                switch (geometry.GeometryType)
//                {
//                    case WkbGeometryType.Polygon:
//                        result = ConvertWkbPolygonToSqlGeometry((WkbPolygon)geometry);
//                        break;
//                    case WkbGeometryType.GeometryCollection:
//                        {
//                            WkbGeometryCollection collection = (WkbGeometryCollection)geometry;
//                            foreach (IWkbGeometry localgeometry in collection)
//                            {
//                                SqlGeometry tmpResult = ConvertWkbGeometryToSqlGeometry(localgeometry);

//                                //SqlGeometry tmpResult = null;
//                                //if (localgeometry.GeometryType != WkbGeometryType.Polygon)
//                                //    continue;
//                                //if ((localgeometry.GeometryType == WkbGeometryType.))

//                                //if (localgeometry.GeometryType == WkbGeometryType.Polygon)
//                                //    tmpResult = ConvertWkbPolygonToSqlGeometry((WkbPolygon)localgeometry);
                                
//                                if (tmpResult == null)
//                                    continue;

//                                result = result == null ? tmpResult.MakeValid() : result.STUnion(tmpResult).MakeValid();
//                            }
//                        }
//                        break;
//                    case WkbGeometryType.LineString:
//                        {
//                            result = ConvertWkbLineStringToSqlGeometry((WkbLineString)geometry);
//                        }
//                        break;
//                    case WkbGeometryType.Point:
//                        {
//                            result = ConvertWkbPointToSqlGeometry((WkbPoint)geometry);
//                        }
//                        break;

//                }

//            }
//            catch (Exception)
//            {
//                return null;
//            }
//            return result;
//        }

//        private static SqlGeometry ConvertWkbLineStringToSqlGeometry(WkbLineString line)
//        {
//            SqlGeometryBuilder gbuilder = new SqlGeometryBuilder();

//            gbuilder.SetSrid(25832);

//            gbuilder.BeginGeometry(OpenGisGeometryType.LineString);

//            gbuilder.BeginFigure(line[0].x, line[0].y);
//            for (int cnt = 1; cnt <= line.Count - 1; cnt++)
//                gbuilder.AddLine(line[cnt].x, line[cnt].y);
//            gbuilder.EndFigure();

//            gbuilder.EndGeometry();
            
//            return gbuilder.ConstructedGeometry;

//        }

//        private static SqlGeometry ConvertWkbPointToSqlGeometry(WkbPoint point)
//        {
//            SqlGeometryBuilder gbuilder = new SqlGeometryBuilder();

//            gbuilder.SetSrid(25832);

//            gbuilder.BeginGeometry(OpenGisGeometryType.Point);

//            gbuilder.BeginFigure(point.x, point.y);
//            gbuilder.EndFigure();

//            gbuilder.EndGeometry();

//            return gbuilder.ConstructedGeometry;

//        }



//        private static SqlGeometry ConvertWkbPolygonToSqlGeometry(WkbPolygon polygon)
//        {
//            SqlGeometryBuilder gbuilder = new SqlGeometryBuilder();

//            gbuilder.SetSrid(25832);

//            gbuilder.BeginGeometry(OpenGisGeometryType.Polygon);

//            if (polygon.Shell.Count > 1)
//            {
//                gbuilder.BeginFigure(polygon.Shell[0].x, polygon.Shell[0].y);
//                for (int cnt = 1; cnt <= polygon.Shell.Count - 1; cnt++)
//                    gbuilder.AddLine(polygon.Shell[cnt].x, polygon.Shell[cnt].y);
//                gbuilder.AddLine(polygon.Shell[0].x, polygon.Shell[0].y);
//                gbuilder.EndFigure();
//            }

//            foreach (WkbLinearRing hole in polygon.Holes)
//            {
//                gbuilder.BeginFigure(hole[0].x, hole[0].y);
//                for (int cnt = 1; cnt <= hole.Count - 1; cnt++)
//                    gbuilder.AddLine(hole[cnt].x, hole[cnt].y);
//                gbuilder.AddLine(hole[0].x, hole[0].y);
//                gbuilder.EndFigure();
//            }

//            gbuilder.EndGeometry();

//            return gbuilder.ConstructedGeometry;
//        }


//    }
//}
