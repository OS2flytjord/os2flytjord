using System;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.ViewModels.Backend
{
  public class ForureningskomponentModel
  {

		public ForureningskomponentModel()
		{
		}

    public Guid Id { get; set; }
    public string Navn { get; set; }
    public Nullable<decimal> Max { get; set; }
    public string Kode { get; set; }
    public string Enhed { get; set; }
		public Decimal Vaerdi { get; set; }
		public string StringId { get; set; }
    public string DisplayMax
    {
      get
      {
        if(Max.HasValue)
          return Max + " " + Enhed;
        return "-";
      }
    }

		public string DisplayVaerdi
		{
			get
			{
			  return Vaerdi + " " + Enhed;
			}
		}
    
    public string DisplayName
    {
      get { return Navn + " - " + Kode; }
    }

	  public string Status
	  {
		  get
		  {
		    if (!Max.HasValue)
		    {
          return "Modtageanlægget har ikke registreret nogen grænseværdi";
		    }
		    if (Vaerdi > Max)
				  return "Overskredet";
			  else
				  return "OK";
		  }
	  }
  }
}