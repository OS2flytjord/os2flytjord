using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niras.Jordflytning.Core.Models
{

    [MetadataType(typeof(JordmodtagerMetaData))]
    public partial class Jordmodtager
    {
    }

    public class JordmodtagerMetaData
    {
      [DisplayFormat(DataFormatString = "{0:#}")]
      public Nullable<decimal> Telefon { get; set; }

    }
}
