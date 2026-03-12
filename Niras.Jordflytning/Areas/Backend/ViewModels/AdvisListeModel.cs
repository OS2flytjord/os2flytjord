using System;

namespace Niras.Jordflytning.Areas.Backend.ViewModels
{
  public class AdvisListeModel
  {
    public Guid Id { get; set; }
    public DateTime Tid { get; set; }
    public string Handling { get; set; }
    public string Person { get; set; }
    public string Link { get; set; }
  }
}