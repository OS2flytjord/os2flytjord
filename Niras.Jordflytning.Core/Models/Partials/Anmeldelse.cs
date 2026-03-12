using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Niras.Jordflytning.Core.Models
{
    [MetadataType(typeof (AnmedelseMetaData))]
	public partial class Anmeldelse
	{

		public string KoerselStartSlut
		{
			get
			{
				if (Jord == null || Jord.KoerselStart == null || Jord.KoerselSlut == null)
					return "Ikke valgt"; 
				return FormatDatetime(Jord.KoerselStart.Value) + " - " + FormatDatetime(Jord.KoerselSlut.Value);
			}
		}

		public string JordmaengdeKoert
		{
			get { return GetJordMaengdeFormatted(this); }
			}

    public Anmeldelse ShallowCopy()
    {
      //Shallow copy til brug ved log ændringerne på anmeldelsen.
      //return (Anmeldelse) this.MemberwiseClone();
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


		private static string FormatDatetime(DateTime? d)
		{
			var res = "";
			if (d.HasValue)
			{
				res += d.Value.ToString("dd-MM-yyyy");
			}
			return res;
		}

		private static string GetJordMaengdeFormatted(Anmeldelse anmeldelse)
		{
			Decimal? jordMaengde = 0;
			Decimal? jordMaengdeForvent = 0;

			if (anmeldelse != null)
			{
			    if (anmeldelse.TotalMaengdeJordIkkeFJ == null)
			    {

			        foreach (var vognlaese in anmeldelse.Vognlaes)
			        {
			            if (vognlaese.MaengdeTon != null)
			                jordMaengde = jordMaengde + vognlaese.MaengdeTon;
			        }
			    }
			    else
			    {
			        jordMaengde = anmeldelse.TotalMaengdeJordIkkeFJ.GetValueOrDefault();
			    }

			    if (anmeldelse.Jord != null && anmeldelse.Jord.ForventetJordmaengdeTon != null)
				{
					jordMaengdeForvent = anmeldelse.Jord.ForventetJordmaengdeTon;
				}
			}

			var note = "";
			if (jordMaengde > jordMaengdeForvent)
			{
				note = " !!!";
			}

			var retVal = String.Format("{0:0.##} / {1:0.##}{2}", jordMaengde, jordMaengdeForvent, note);
			retVal = retVal.Replace(".", ",");

			return retVal;
		}

	}

	public class AnmedelseMetaData
	{
		[Display(Name = @"Løbenummer")]
    [DisplayFormat(DataFormatString = "{0:#}")]
		public decimal? Nummer { get; set; }
	}

  

}
