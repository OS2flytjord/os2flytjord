using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foolproof;

namespace Niras.Jordflytning.Core.Models
{
	[MetadataType(typeof(JordMetaData))]
	public partial class Jord
	{
    public Jord ShallowCopy()
    {
      //Shallow copy til brug ved log ændringerne på anmeldelsen.
      //return (Jord)this.MemberwiseClone();
      return ShallowCopyEntity(this);

    }

    /// <summary>
    /// Makes a shallow copy of an entity object. This works much like a MemberwiseClone
    /// but directly instantiates a new object and copies only properties that work with
    /// EF and don't have the NotMappedAttribute.
    /// http://stackoverflow.com/questions/12315233/entityframework-entity-proxy-error
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="source">The source entity.</param>
    public static TEntity ShallowCopyEntity<TEntity>(TEntity source) where TEntity : class, new()
    {

      // Get properties from EF that are read/write and not marked witht he NotMappedAttribute
      var sourceProperties = typeof(TEntity).GetProperties().Where(p => p.CanRead && p.CanWrite);
      var newObj = new TEntity();

      foreach (var property in sourceProperties)
      {
        // Copy value
        property.SetValue(newObj, property.GetValue(source, null), null);
      }

      return newObj;

    }

	}

	public class JordMetaData
	{
    [Display(Name = "Er der udtaget jordprøver")]
    public Nullable<bool> Jordproever { get; set; }

    //&#34;
    [Display(Name = "Intakt jord")]
    public bool IntaktJord { get; set; }

    
    //[RequiredIfTrue("IntaktJord", ErrorMessage = "Jorden - Antal prøver skal udfyldes")]//"An unexpected exception was thrown during validation of 'Antal prøver' when invoking Foolproof.RequiredIfTrueAttribute.IsValid. See the inner exception for details."
    [Display(Name = "Antal prøver *")]
    public Nullable<decimal> AntalProever { get; set; }

    //[Required(ErrorMessage = "Jorden - Jordprøver er udtaget af skal udfyldes")]
	  [Display(Name = "Jordprøver er udtaget af *")]
    public string MiljoeTekniskTilsyn { get; set; }

		[Display(Name = "Jordprøverne er udtaget")]
		public bool JordproeverFoer { get; set; }

    //[Required(ErrorMessage = "Jorden - Kørsel start skal udfyldes")]
    [DisplayFormat(DataFormatString = "{0:d}")]
		public DateTime? KoerselStart { get; set; }

    //[Required(ErrorMessage = "Jorden - Kørsel slut skal udfyldes")]
     [DisplayFormat(DataFormatString = "{0:d}")]
    public DateTime? KoerselSlut { get; set; }

    [Display(Name = "Affaldstype")]
    public virtual AffaldType AffaldType { get; set; }
    

		[Display(Name = "Anden affaldstype")]
    //[RequiredIf("AffaldType",Operator.EqualTo,"",ErrorMessage = "Jorden - Anden affaldstype skal udfyldes")]
		public string AndenAffaldType { get; set; }

		[Display(Name = "Bemærkning til projektet")]
		public string Bemaerkning { get; set; }

    //[RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]", ErrorMessage = "Jorden - Forventet jordmængde skal udfyldes")]
    //[Required(ErrorMessage = "Jorden - Forventet jordmængde skal udfyldes")]
		[Display(Name = "Forventet jordmængde (t) *")]
    [DisplayFormat(DataFormatString = "{0:#,##0#}")]
    public decimal ForventetJordmaengdeTon { get; set; }

    [Display(Name = "Beskrivelse af jordarbejdet")]
    public string JordarbejdeBeskrivelse { get; set; }

    [Display(Name = "Tidligere erhvervsaktivitet")]
    public string TidligereErhvervsaktivitet { get; set; }

    //[Required(ErrorMessage = "Jorden - Der skal angives en forureningskategori")]
    public virtual JordKlassifikationType JordKlassifikationType { get; set; }

	}



}
