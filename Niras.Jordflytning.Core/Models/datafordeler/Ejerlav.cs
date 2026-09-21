namespace Niras.Jordflytning.Core.Models.datafordeler
{
    public class Ejerlav
    {
        public string id_lokalId { get; set; }
        public int ejerlavskode { get; set; }
        public string ejerlavsnavn { get; set; }
        public DatafordelerGeometry geometri { get; set; }
    }
}
