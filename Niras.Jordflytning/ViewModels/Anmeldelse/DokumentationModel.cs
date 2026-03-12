using System;
using System.ComponentModel.DataAnnotations;

namespace Niras.Jordflytning.ViewModels.Anmeldelse
{
  public class DokumentationModel
  {
    public Guid Id { get; set; }
    [Display(Name = "Navn")]
    public string FilNavn { get; set; }

    public string DisplayFilename
    {
      get
      {
	      if (!string.IsNullOrEmpty(FilNavn) && FilNavn.Length > 10)
		      return FilNavn.Substring(0, 10) + "...";

	      return FilNavn;
      }
    }

    public string FilNavnUrlEncode { get; set; }
    public string FilUrl { get; set; }
    public string Type { get; set; }
    public DateTime? OprindelseDato { get; set; }
    public Guid AnmeldelseId { get; set; }

  }
}