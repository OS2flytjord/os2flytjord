using System;

namespace Niras.Jordflytning.Areas.Backend.ViewModels
{
  public class StatusBetalerModel
  {
    public Guid Id { get; set; }
    public string Jordmodtager { get; set; }
    public bool? Godkend { get; set; }
    public DateTime Dato { get; set; }
  }
}