using System;

namespace Niras.Jordflytning.Areas.Backend.ViewModels
{
  public class KommunikationModel
  {
    public Guid Id { get; set; }
    public Guid AnmeldelseId { get; set; }
    public DateTime Tid { get; set; }
    public string Fra { get; set; }
    public string  Til { get; set; }
    public string Advislink { get; set; }
    }
}