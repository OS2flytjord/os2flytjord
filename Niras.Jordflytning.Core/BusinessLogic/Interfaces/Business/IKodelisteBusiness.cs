using Niras.Jordflytning.Core.Models;
using System;
using System.Collections.Generic;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
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

  public enum EnumJordflytningType
  {
    Alm = 1,
    Akut = 2,
    Straks = 3
  }

  public enum EnumAdvisType
  {
    Email=1,
    SMS=2,
    HørAndenKommune=3,
    SvarfraAndenKommune=4
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

    /// <summary>
    /// Henter alle kommuner
    /// Sorteres alfabetisk
    /// </summary>
    /// <returns></returns>
    IList<Kommune> ReadAllKommuner();

    Kommune ReadKommuneById(Guid id);
    Kommune ReadKommuneByNavn(string navn);
    Kommune ReadKommuneByKode(int kode);

    OprindelsesstedKlassifikationType ReadOprindelsesstedKlassifikationType(Guid id);
		OprindelsesstedKlassifikationType ReadOprindelsesstedKlassifikationType(short id);
    IList<OprindelsesstedKlassifikationType> ReadAktiveOprindelsesstedKlassifikationTypes();

    AndenOprindJordType ReadAndenOprindJordType(Guid id);
    IList<AndenOprindJordType> ReadAktiveAndenOprindJordTypes();

    AffaldType ReadAffaldType(Guid id);
    IList<AffaldType> ReadAktiveAffaldTypes();

    JordKlassifikationType ReadJordKlassifikationType(Guid id);
    IList<JordKlassifikationType> ReadAktiveJordKlassifikationTypesForLandsdel(Guid landsdel);
    
    IList<JordKlassifikationType> ReadAktiveJordKlassifikationTypesForKommune(string navn);
    IList<JordKlassifikationType> ReadAktiveJordKlassifikationTypesForKommune(Guid kommuneId);
    IList<JordKlassifikationType> ReadAktiveJordKlassifikationTypesForKommune(short kommuneNr);
    


    JordflytningType ReadJordflytningType(Guid id);

    IList<JordflytningType> ReadAktiveJordflytningTypes();

    IList<JordanlaegType> ReadAktiveJordanlaegTypes();

    IList<Enhed> ReadAktiveEnheder();

    IList<AdvisType> ReadAktiveAdvisTypes();
    AdvisType ReadAdvisType(Guid id);

	  List<StatusStikproeveType> ReadAktiveStikproeveStatusTyper();
    StatusStikproeveType ReadStikproeveStatusType(Guid statusGuid);

    IList<LandsdelType> ReadAktiveLandsdelTypes();
    LandsdelType ReadLandsdelType(Guid id);

  }
}
