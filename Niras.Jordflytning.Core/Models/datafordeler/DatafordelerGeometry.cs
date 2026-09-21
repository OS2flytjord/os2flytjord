namespace Niras.Jordflytning.Core.Models.datafordeler
{
    public class DatafordelerGeometry
    {
        public string wkt { get; set; }

        public string type { get; set; }
        
        public int crs { get; set; }

        public string dimension { get; set; }

        public double? X { get; set; }

        public double? Y { get; set; }


    }
}
