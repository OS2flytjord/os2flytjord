using System;
using System.ComponentModel.DataAnnotations;

namespace Niras.Jordflytning.Core.Models
{
	[MetadataType(typeof(LastbilMetaData))]
	public partial class Lastbil
	{
		
	}

	public class LastbilMetaData
	{
		public Guid Id { get; set; }
		public Guid? TransportoerId { get; set; }
		public Guid? MiljoeklasseTypeId { get; set; } 
	}



}
