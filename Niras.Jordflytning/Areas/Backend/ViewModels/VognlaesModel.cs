using System;

namespace Niras.Jordflytning.Areas.Backend.ViewModels
{
  public class VognlaesModel
  {
    public Guid Id { get; set; }
    public DateTime DatoBom { get; set; }
    public string Transportoer { get; set; }
    public decimal? JordmaengdeTon { get; set; }
    public decimal? JordmaengdeAksler { get; set; }
	public Guid TransportoerId { get; set; }
	public Guid LastbilId { get; set; }

	  public bool Afvist { get; set; }
	  public string AfvistNote { get; set; }
  }
}