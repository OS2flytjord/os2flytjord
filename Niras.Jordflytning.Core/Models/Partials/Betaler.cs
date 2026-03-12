using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niras.Jordflytning.Core.Models
{
  [MetadataType(typeof(BetalerMetaData))]
  public partial class Betaler
  {
    public Models.Betaler ShallowCopy()
    {
      //Shallow copy til brug ved log ændringerne på anmeldelsen.
      //return (Betaleringsoplysning)this.MemberwiseClone();
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
  public class BetalerMetaData
  {
  }
}
