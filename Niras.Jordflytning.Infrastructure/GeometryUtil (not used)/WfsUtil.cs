//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using UGIS.Mapping.Cartographer;
//using UGIS.Mapping.Cartographer.Features;
//using UGIS.Mapping.Cartographer.FilterExtensions;
//using UGIS.Mapping.Gml;
//using UGIS.Mapping.Ogc.Wkb;
//using UGIS.Mapping.Ogc.Wkt;
//using UGIS.Mapping.Wfs2.v100;
//using UGIS.Mapping.Wfs2.v100.Capabilities;

//namespace Niras.Jordflytning.Infrastructure.GeometryUtil
//{
//    public static class WfsUtil
//    {
//        public static IWkbGeometry ConvertWktToWkb(string wktGeometry)
//        {
//            var wkbGeometry = WktReader.Read(wktGeometry);

//            if (wkbGeometry is WkbGeometryCollection)
//            {
//                var wkbCollection = wkbGeometry as WkbGeometryCollection;
//                return wkbCollection[0];
//            }

//            return wkbGeometry;
//        }

//        private static CgLayer_Wfs GetWfsLayerFromUrl(string wfsurl, string layername)
//        {
//            WfsCapDocument caps = WfsCapDocument.Request(wfsurl, WfsCompabilitySettings.Default);
//            WfsCapFeatureType type = caps.FeatureTypes.FirstOrDefault(a => a.Name == layername);
//            WfsCapFeatureTypeDescription typeDesc = WfsCapFeatureTypeDescription.Request(caps, type, WfsCompabilitySettings.Default);
//            //typeDesc.FindGmlColumn();

//            CgEngine cgEngine = new CgEngine(); // Genbrug denne i stedet for at new hver gang, da den cacher forskellige ting…

//            return cgEngine.CreateWfsLayer(caps, type, typeDesc, WfsCompabilitySettings.Default);
//        }

//        public static List<IWkbGeometry> GetWkbFromWfs(IWkbGeometry geometry, string wfsServicePostUrl, string wfsIdColumn, string wfsGeometryColumn, string wfsLayer)
//        {

//            var result = new List<IWkbGeometry>();
//            if (geometry.GeometryType == WkbGeometryType.GeometryCollection)
//            {
//                WkbGeometryCollection geoCol = (WkbGeometryCollection)geometry;

//                foreach (IWkbGeometry wkbGeometry in geoCol)
//                {
//                    if (wkbGeometry.GeometryType == WkbGeometryType.GeometryCollection)
//                        result.AddRange(GetWkbFromWfs(wkbGeometry, wfsServicePostUrl, wfsIdColumn, wfsGeometryColumn, wfsLayer));
//                    else if (wkbGeometry.GeometryType == WkbGeometryType.Polygon)
//                        result.AddRange(GetWkbFromWfs((WkbPolygon)wkbGeometry, wfsServicePostUrl, wfsIdColumn, wfsGeometryColumn, wfsLayer));
//                    else if (wkbGeometry.GeometryType == WkbGeometryType.LineString)
//                        result.AddRange(GetWkbFromWfs((WkbLineString)wkbGeometry, wfsServicePostUrl, wfsIdColumn, wfsGeometryColumn, wfsLayer));
//                    else
//                        throw new Exception("Geometry type unknow 2 (" + geometry.GeometryType + ")");
//                }
//            }
//            else if (geometry.GeometryType == WkbGeometryType.Polygon)
//                result.AddRange(GetWkbFromWfs((WkbPolygon)geometry, wfsServicePostUrl, wfsIdColumn, wfsGeometryColumn, wfsLayer));
//            else if (geometry.GeometryType == WkbGeometryType.LineString)
//                result.AddRange(GetWkbFromWfs((WkbLineString)geometry, wfsServicePostUrl, wfsIdColumn, wfsGeometryColumn, wfsLayer));
//            else
//                throw new Exception("Geometry type unknow (" + geometry.GeometryType + ")");

//            return result;
//        }

//        public static List<IWkbGeometry> GetWkbFromWfs(WkbPolygon polygon, string wfsServicePostUrl, string wfsIdColumn, string wfsGeometryColumn, string wfsLayer)
//        {
//            if (polygon.Bounds != null)
//                return GetWkbFromWfs(GeometryTools.GetWktFromGml((GmlTools.ToGml(polygon.Bounds.Value, "EPSG:25832", true))),
//                                     wfsServicePostUrl, wfsIdColumn, wfsGeometryColumn, wfsLayer);

//            return new List<IWkbGeometry>();
//        }

//        public static List<IWkbGeometry> GetWkbFromWfs(WkbLineString line, string wfsServicePostUrl, string wfsIdColumn, string wfsGeometryColumn, string wfsLayer)
//        {
//            if (line.Bounds != null)
//                return GetWkbFromWfs(GeometryTools.GetWktFromGml((GmlTools.ToGml(line.Bounds.Value, "EPSG:25832", true))),
//                                     wfsServicePostUrl, wfsIdColumn, wfsGeometryColumn, wfsLayer);

//            return new List<IWkbGeometry>();
//        }

//        public static List<IWkbGeometry> GetWkbFromWfs(string wkt, string wfsServicePostUrl, string wfsIdColumn, string wfsGeometryColumn, string wfsLayername)
//        {
//            try
//            {
//                var wkbList = new List<IWkbGeometry>();

//                IWkbGeometry wkb = ConvertWktToWkb(wkt);
//                if (wkb.Bounds == null)
//                    return wkbList;

//                CgLayer_Wfs wfsLayer = GetWfsLayerFromUrl(wfsServicePostUrl, wfsLayername);
//                if (wfsLayer == null)
//                    return wkbList;

//// ReSharper disable PossibleInvalidOperationException
//                wkbList.AddRange(wfsLayer.Features.Where(ftr => ftr.Geometry.FilterOgcBBox(wkb.Bounds.Value)).Select(cgFeature => cgFeature.Geometry));
//// ReSharper restore PossibleInvalidOperationException

//                return wkbList;
//            }
//            catch (Exception ex)
//            {
//                Debug.WriteLine("WfsUtil.GetWkbFromWfs - Layer = " + wfsLayername + " - Exception : " + ex.Message);
//                throw new Exception("Fejl i WFS forespørgsel - WfsUtil.GetWkbFromWFS - Layer = " + wfsLayername + " (" + ex.Message + ")");
//            }

//        }

//        //private static OgcFilter GetFilter(string wkt, string wfsGeometryColumn)
//        //{
//        //    OgcFilter filter = new OgcFilter();

//        //    // Setup request filter (BBOX as GML)
//        //    try
//        //    {
//        //        //Converts Wkt to Wkb geometri
//        //        IWkbGeometry wkb = ConvertWktToWkb(wkt);

//        //        //Reads GML from wkb
//        //        if (wkb.Bounds != null)
//        //        {
//        //            string sBoxGml = GmlTools.ToGml(wkb.Bounds.Value, "EPSG:25832", false);
//        //            //sBoxGml = WfsTools.AddNamespaceTagsToGml(sBoxGml);

//        //            //Creates Spatial filter
//        //            filter.AddSpatialFilterItem(wfsGeometryColumn, sBoxGml, OgcSpatialOperator.BBOX);
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        throw new Exception("Fejl i generering af GML BBOX (" + ex.Message + ")");
//        //    }

//        //    return filter;
//        //}

//        public static IList<String> GetValuesFromColumn(string url, string idColumnName, string geometryColumnName, string layerName, string valueFiledName, string wkt)
//        {
//            IList<String> result = new List<string>();

//            IList<string> wktList = GeometryTools.GetWktListFromWkt(wkt);

//            foreach (string w in wktList)
//            {
//                try
//                {
//                    IWkbGeometry wkb = ConvertWktToWkb(w);
//                    if (wkb.Bounds == null)
//                        continue;

//                    CgLayer_Wfs wfsLayer = GetWfsLayerFromUrl(url, layerName);
//                    if (wfsLayer == null)
//                        continue;

//// ReSharper disable PossibleInvalidOperationException
//                    foreach (ICgFeature cgFeature in wfsLayer.Features.Where(ftr => ftr.Geometry.FilterOgcBBox(wkb.Bounds.Value)))
//// ReSharper restore PossibleInvalidOperationException
//                    {
//                        foreach (var cgColumn in wfsLayer.Columns)
//                        {
//                            if (cgColumn.Name.ToLower() == valueFiledName.ToLower())
//                                if (cgFeature[cgColumn.Name] != null)
//                                    result.Add(cgFeature[cgColumn.Name].ToString());
//                        }
//                    }

//                    return result;

//                }
//                catch (Exception ex)
//                {
//                    throw new Exception("Fejl i WfsUtil.GetValuesFromColumn (" + ex.Message + ")");
//                }

//            }

//            return result;
//        }

//        public static string FillSkabelonFromWfs(string url, string layerName, string wkt, string skabelon)
//        {
//            string result = "";

//            IWkbGeometry wkb = ConvertWktToWkb(wkt);
//            if (wkb.Bounds == null)
//                return "";

//            CgLayer_Wfs wfsLayer = GetWfsLayerFromUrl(url, layerName);
//            if (wfsLayer == null)
//                return "";

//// ReSharper disable PossibleInvalidOperationException
//            foreach (ICgFeature feature in wfsLayer.Features.Where(ftr => ftr.Geometry.FilterOgcBBox(wkb.Bounds.Value)))
//// ReSharper restore PossibleInvalidOperationException
//            {
//                if (!string.IsNullOrEmpty(result))
//                    result += "</br>";

//                string tmpStr = skabelon;
//                //result += wfsLayer.Columns.Aggregate(skabelon, (current, cgColumn) => current.Replace("[" + cgColumn.Name + "]", feature[cgColumn.Name].ToString()));

//                foreach (var cgColumn in wfsLayer.Columns)
//                {
//                    if (feature[cgColumn.Name] != null)
//                        tmpStr = tmpStr.Replace("[" + cgColumn.Name + "]", feature[cgColumn.Name].ToString());                    
//                }

//                result += tmpStr;
//            }


//            return result;
//        }

//    }
//}
