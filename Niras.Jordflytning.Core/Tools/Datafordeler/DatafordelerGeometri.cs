using Niras.Jordflytning.Core.Models.datafordeler;
using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niras.Jordflytning.Core.Tools.Datafordeler
{
    public static class DatafordelerGeometri
    {
        public static DatafordelerGeometry ExtendGeometriData(DatafordelerGeometry geometri)
        {
            var g = DbGeometry.FromText(geometri.wkt, 25832);

            if (g.SpatialTypeName == "Point")
            {                
                geometri.X = g.Centroid.XCoordinate;
                geometri.Y = g.Centroid.YCoordinate;
            } 
            if (g.SpatialTypeName == "Polygon")
            {
                geometri.X = g.Centroid.XCoordinate;
                geometri.Y = g.Centroid.YCoordinate;
            }
            if (g.SpatialTypeName == "MultiPolygon")
            {
                geometri.X = g.Centroid.XCoordinate;
                geometri.Y = g.Centroid.YCoordinate;
            }

            return geometri;
        }


    }
}
