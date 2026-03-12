using System;
using System.ComponentModel.DataAnnotations;

namespace Niras.Jordflytning.Core.Models
{
	[MetadataType(typeof(FirmaoplysningerMetaData))]
	public partial class Firmaoplysninger
	{
	}

	public class FirmaoplysningerMetaData
	{

    [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = @"CVR nummeret skal være 8 cifre")]
    [DisplayFormat(DataFormatString = "{0:#}")]
    public Decimal CVR { get; set; }


    [StringLength(40,ErrorMessage = "Firmanavnet er for langt.")]
    public String Firmanavn { get; set; }

	 [StringLength(40,ErrorMessage = "PNummeret er for langt.")]
		public string PNummer { get; set; }

		[StringLength(100,ErrorMessage = "Addressen er for lang.")]
		public String Adresse { get; set; }

    [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = @"Postnummeret er ikke gyldigt.")]
    [Range(typeof(Decimal),"0","9999",ErrorMessage = "Postnummeret er ikke gyldigt.")]
    [DisplayFormat(DataFormatString = "{0:#}")]
		public Nullable<Decimal> Postnummer{ get; set; }

    [StringLength(40,ErrorMessage = "Postdistriktet er for langt.")]
    public String Postdistikt { get; set; }

    [Range(typeof(Decimal),"0","99999999",ErrorMessage = "Telefonnummeret er ikke gyldigt.")]
    [DisplayFormat(DataFormatString = "{0:#}")]
    public Nullable<Decimal> Telefon{ get; set; }

    [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = @"EAN skal være 13 cifre")]
    [Display(Name = @"EAN")]
    [Range(typeof(Decimal), "999999999999", "9999999999999", ErrorMessage = @"EAN skal være 13 cifre.")]
    [DisplayFormat(DataFormatString = "{0:#}")]
    public Nullable<decimal> EAN { get; set; }


	}

}
