using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niras.Jordflytning.Core.Models
{
	[MetadataType(typeof(ModtagerAnlaegMetaData))]
	public partial class ModtagerAnlaeg
	{
	}

  public class ModtagerAnlaegMetaData
  {
    [DisplayFormat(DataFormatString = "{0:#}")]
    public Nullable<decimal> Postnummer { get; set; }

    [DisplayFormat(DataFormatString = "{0:#}")]
    public Nullable<decimal> KontaktpersonTlf { get; set; }

      [DisplayFormat(DataFormatString = "{0:d}")]
    public Nullable<DateTime> AktivFra { get; set; }

      [DisplayFormat(DataFormatString = "{0:d}")]
      public Nullable<DateTime> AktivTil { get; set; }

  }


}
