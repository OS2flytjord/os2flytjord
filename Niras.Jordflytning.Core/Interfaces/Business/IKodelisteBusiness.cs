using Niras.Jordflytning.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Niras.Jordflytning.Core.Models.JordForurening;

namespace Niras.Jordflytning.Core.Interfaces.Business
{
  public enum EnumAffaldstype
  {
    TeglMurbrokker = 1,
    BetonCementrørFliser = 2,
    Asfalt = 3,
    Slagge = 4,
    TræGreneRødder = 5,
    Andet = 6
  }

  public interface IKodelisteBusiness
  {
		/// <summary>
		/// Henter de forskellige miljøklassetyper fra databasen.
		/// </summary>
		/// <returns></returns>
	  IList<MiljoeklasseType> ReadAktiveMiljoeklasseTyper();

		/// <summary>
    /// Henter en documentationType med et bestemt id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    DokumentationType ReadDokumentationType(Guid id);
    
    /// <summary>
    /// Henter alle aktive documentationType'er
    /// Sortering efter default sorteringsmetode
    /// </summary>
    /// <returns></returns>
    IList<DokumentationType> ReadAktiveDokumentationType();

    /// <summary>
    /// Henter alle aktive documentationType'er
    /// Sortere efter sorteringsmetode
    /// </summary>
    /// <returns></returns>
    IList<DokumentationType> ReadAktiveDokumentationType(ESortType sorteringsmetode);

    /// <summary>
    /// Henter alle aktive kommuner.
    /// Sortere altid alfabetisk
    /// </summary>
    /// <returns></returns>
    IList<Kommune> ReadAktiveKommune();

    Kommune ReadKommuneById(Guid id);
    Kommune ReadKommuneByNavn(string navn);

    OprindelsesstedKlassifikationType ReadOprindelsesstedKlassifikationType(Guid id);
    IList<OprindelsesstedKlassifikationType> ReadAktiveOprindelsesstedKlassifikationTypes();

    AndenOprindJordType ReadAndenOprindJordType(Guid id);
    IList<AndenOprindJordType> ReadAktiveAndenOprindJordTypes();

    AffaldType ReadAffaldType(Guid id);
    IList<AffaldType> ReadAktiveAffaldTypes();

    JordKlassifikationType ReadJordKlassifikationType(Guid id);
    IList<JordKlassifikationType> ReadAktiveJordKlassifikationTypesForKommune(System.Guid kommuneId);

    JordflytningType ReadJordflytningType(Guid id);

    IList<JordflytningType> ReadAktiveJordflytningTypes();

		IList<JordanlaegType> ReadAktiveJordanlaegTypes();

		IList<Enhed> ReadAktiveEnheder();

  }
}
