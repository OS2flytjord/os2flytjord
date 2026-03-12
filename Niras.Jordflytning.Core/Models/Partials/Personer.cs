using System.Reflection;
using Foolproof;
using Niras.Jordflytning.Core.Models.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Niras.Jordflytning.Core.Models
{
	[MetadataType(typeof(PersonMetaData))]
	public partial class Person
	{
	}


	public class PersonMetaData
	{
		[Display(Name = "Email", ResourceType = typeof(Personer))]
		[StringLength(40, ErrorMessage = "E-mail er for lang.")]
		[Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Personer))]
		[EmailAddress]
		[DataType(DataType.EmailAddress, ErrorMessageResourceName="InvalidEmail", ErrorMessageResourceType=typeof(Personer))]
		public string Email { get; set; }

		[Required(ErrorMessage = "Skal udfyldes")]
		[StringLength(40, ErrorMessage = "Fornavn er for langt.")]
		[Display(Name = "Fornavn")]
		public string Navn { get; set; }

		[StringLength(40,ErrorMessage = "Efternavn er for langt.")]
        [Required(ErrorMessage = "Skal udfyldes")]
		[Display(Name = "Efternavn")]
		public string Efternavn { get; set; }

        [Required(ErrorMessage = "Skal udfyldes")]
		[StringLength(40, ErrorMessage = "Adressen er for lang.")]
		[Display(Name = "Adresse")]
		public string Adresse { get; set; }

		[Required(ErrorMessage = "Skal udfyldes")] 
		[Display(Name = "Postdistrikt")]
    public string Postdistrikt { get; set; }

    [DisplayFormat(DataFormatString = "{0:#}")]
    public Nullable<decimal> Postnummer { get; set; }


		[Display(Name = "By")]
		public string By { get; set; }

		[Required(ErrorMessage = "Skal udfyldes")]
		[Range(typeof(Decimal),"0", "99999999", ErrorMessage = "Nummeret er ikke gyldigt.")]
		[Display(Name = "Telefon")]
    [DisplayFormat(DataFormatString = "{0:#}")]
		public Nullable<decimal> Telefon { get; set; }

		[Required(ErrorMessage = "Skal udfyldes")]
		[Range(0,99999999,ErrorMessage = "Nummeret er ikke gyldigt." )]
		[Display(Name = "Mobiltelefon")]
    [DisplayFormat(DataFormatString = "{0:#}")]
    public Nullable<decimal> Mobiltelefon { get; set; }

	}
}
