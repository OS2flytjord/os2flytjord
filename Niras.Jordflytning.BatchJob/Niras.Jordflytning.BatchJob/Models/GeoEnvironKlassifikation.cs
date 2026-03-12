using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niras.Jordflytning.BatchJob.Models
{
    public class GeoEnvironKlassifikation
    {

        public GeoEnvironKlassifikation(string textId, string navn, bool hasAnmeldePligt, bool besvaresVedEjdForsprgsl, bool useDescription, string description)
        {
            TextId = textId;
            Description = description;
            Navn = navn;
            HasAnmeldePligt = hasAnmeldePligt;
            BesvaresVedEjdForsprgsl = besvaresVedEjdForsprgsl;
            UseShortDescription = useDescription;
        }

        /// <summary>
        /// Kategori id (text)
        /// </summary>
        public string TextId { get; private set; }

        /// <summary>
        /// Navn på kategorien
        /// </summary>
        public string Navn { get; private set; }

        /// <summary>
        /// Kort beskrivelse af kategorien
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Markering om denne kategori kræver
        /// anmeldelse
        /// </summary>
        public bool HasAnmeldePligt { get; private set; }

        /// <summary>
        /// Markering om man skal bruge den korte 
        /// Description (ellers skal man bruge den der 
        /// kommer fra selve matriklen)
        /// </summary>
        public bool UseShortDescription { get; private set; }

        /// <summary>
        /// Besvares ved ejendomsforespørgsel
        /// </summary>
        public bool BesvaresVedEjdForsprgsl { get; private set; }
    }
}
