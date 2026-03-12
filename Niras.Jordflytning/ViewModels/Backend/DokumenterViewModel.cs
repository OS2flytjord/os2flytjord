using System;

namespace Niras.Jordflytning.ViewModels.Backend
{
  public class DokumenterViewModel
  {
    public Guid Id { get; set; }
    public string FilNavn { get; set; }
    public string FilSti  { get; set; }
    public Guid ModtageAnlaegId { get; set; }
  }
}