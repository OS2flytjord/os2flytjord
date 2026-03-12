using Niras.Jordflytning.Core.Models.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Niras.Jordflytning.Core.Models
{
	[MetadataType(typeof(TransportoerMetaData))]
	public partial class Transportoer
	{
		
		}


		public class TransportoerMetaData
		{

      //[Required(ErrorMessage = "Modtager og transportør - Der skal vælges en transportør")]
      public System.Guid Id { get; set; }
		}
	}
