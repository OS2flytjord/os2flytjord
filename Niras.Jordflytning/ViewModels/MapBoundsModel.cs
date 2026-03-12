using System;
using System.Data.Spatial;
using System.Globalization;
using System.Linq;

namespace Niras.Jordflytning.ViewModels
{
    public class MapBoundsModel
    {

        private readonly CultureInfo _culture;
        private string _wkt;

        public string Wkt
        { 
            get { return _wkt; }
            set { Init(value); }
        }

        public string CenterWkt { get; private set; }
        public DbGeometry Geom { get; private set; }
        public DbGeometry CenterGeom { get; private set; }
        public string UpperX { get; private set; }
        public string UpperY { get; private set; }
        public string LowerX { get; private set; }
        public string LowerY { get; private set; }

        private enum MapBoundTarget
        {
            UpperX = 0,
            UpperY = 1,
            LowerX = 2,
            LowerY = 3
        }

        public MapBoundsModel()
        {
            _culture = new CultureInfo("en-US");
        }

        public MapBoundsModel(string wkt)
            : this()
        {
            Init(wkt);
        }

        public void Init(string wkt)
        {
            var wktParseResult = ParseWkt(wkt, 25832);
            Geom = wktParseResult.Geom;
            _wkt = wktParseResult.Wkt;
            if (Geom != null)
            {
                CenterWkt = GetCenterWkt(Geom);
                UpperX = GetMapBoundValue(Geom, MapBoundTarget.UpperX);
                UpperY = GetMapBoundValue(Geom, MapBoundTarget.UpperY);
                LowerX = GetMapBoundValue(Geom, MapBoundTarget.LowerX);
                LowerY = GetMapBoundValue(Geom, MapBoundTarget.LowerY);
            }
            if (!string.IsNullOrWhiteSpace(CenterWkt))
            {
                var centerParseResult = ParseWkt(CenterWkt, 25832);
                CenterGeom = centerParseResult.Geom;
                CenterWkt = centerParseResult.Wkt;
            }
        }

        private class ParseResult
        {
            public string Wkt { get; set; }
            public DbGeometry Geom { get; set; }
        }

        private ParseResult ParseWkt(string wkt, int srid)
        {
            var result = new ParseResult();
            if (!string.IsNullOrWhiteSpace(wkt))
            {
                result.Geom = DbGeometry.FromText(wkt, srid);
                result.Wkt = wkt;
            }
            return result;
        }

        private string GetMapBoundValue(DbGeometry geom, MapBoundTarget target)
        {
            double? value = (target == MapBoundTarget.UpperX || target == MapBoundTarget.UpperY) ? double.MinValue : double.MaxValue;
            try
            {
                if (geom.PointCount > 1)
                {
                    for (int i = 1; i <= geom.Boundary.PointCount; i++)
                    {
                        double? ordiant;
                        switch (target)
                        {
                            case MapBoundTarget.LowerX:
                            case MapBoundTarget.UpperX:
                                ordiant = geom.PointAt(i).XCoordinate;
                                break;
                            case MapBoundTarget.LowerY:
                            case MapBoundTarget.UpperY:
                                ordiant = geom.PointAt(i).YCoordinate;
                                break;
                            default:
                                ordiant = null;
                                break;
                        }
                        if (target == MapBoundTarget.UpperX || target == MapBoundTarget.UpperY)
                        {
                            if (ordiant > value)
                                value = ordiant;
                        }
                        else
                        {
                            if (ordiant < value)
                                value = ordiant;
                        }

                    }
                }
                else
                {
                    int margin = (target == MapBoundTarget.UpperX || target == MapBoundTarget.UpperY) ? 100 : -100;
                    switch (target)
                    {
                        case MapBoundTarget.LowerX:
                        case MapBoundTarget.UpperX:
                            value = geom.XCoordinate + margin;
                            break;
                        case MapBoundTarget.LowerY:
                        case MapBoundTarget.UpperY:
                            value = geom.YCoordinate + margin;
                            break;
                    }
                }
            }
            catch
            { }
            if (value == double.MinValue || value == double.MaxValue)
                return "";

            return Math.Floor(value.GetValueOrDefault()).ToString(_culture);
        }

        private string GetCenterWkt(DbGeometry geom)
        {
            double x = 0;
            double y = 0;
            try
            {
                if (geom.PointCount > 1)
                {
                    var xc = 0;
                    var yc = 0;
                    for (int i = 1; i <= geom.Boundary.PointCount; i++)
                    {
                        var px = geom.PointAt(i).XCoordinate;
                        var py = geom.PointAt(i).YCoordinate;
                        if (px.HasValue)
                        {
                            x += px.Value;
                            xc++;
                        }
                        if (py.HasValue)
                        {
                            y += py.Value;
                            yc++;
                        }
                    }
                    x = (x / xc);
                    y = (y / yc);
                }
                else if (geom.XCoordinate.HasValue && geom.YCoordinate.HasValue)
                {
                    x = geom.XCoordinate.Value;
                    y = geom.YCoordinate.Value;
                }
            }
            catch
            { }
            if (x == 0 || y == 0)
                return "";
            
            return string.Format("POINT({0} {1} 25832)",
                x.ToString(_culture),
                y.ToString(_culture)
            );
        }

    }
}