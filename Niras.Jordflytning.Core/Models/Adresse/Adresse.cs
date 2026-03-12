namespace Niras.Jordflytning.Core.Models.Adresse
{
  public class Adresse
  {
    public Vej Vej { get; set; }
    //public PostDistrikt PostDistrikt { get; set; }
    public string Husnr { get; set; }
    public Kommune Kommune { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
  }
}
