using System;

namespace Niras.Jordflytning.Areas.Backend.ViewModels
{
  public class StikproeveListeModel
  {

    public Guid Id { get; set; }
    public int Proevenr { get; set; }
    public string ProeveUdtagetAf { get; set; }
    public string AnalyseretAf { get; set; }
    public string Transportoer { get; set; }
    public string StatusStikproeve { get; set; }
    public DateTime StatusDato { get; set; }
    public Guid AnmeldelsesId { get; set; }

  }
}