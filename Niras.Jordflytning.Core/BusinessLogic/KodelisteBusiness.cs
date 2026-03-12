using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Niras.Jordflytning.Core.BusinessLogic
{
  public class KodelisteBusiness : IKodelisteBusiness
  {
    private readonly IDokumentationTypeRepository _dokumentationTypeRepo;
    private readonly IKommuneRepository _kommuneRepo;
    private readonly IOprindelsesstedKlassifikationTypeRepository _oprindelsesstedKlassifikationTypeRepo;
    private readonly IAndenOprindJordTypeRepository _andenOprindJordTypeRepo;
    private readonly IAffaldTypeRepository _affaldTypeRepo;
    private readonly IJordKlassifikationTypeRepository _jordKlassifikationTypeRepo;
    private readonly IJordflytningTypeRepository _jordflytningTypeRepo;
    private readonly IJordanlaegTypeRepository _jordanlaegTypeRepo;
    private readonly IRepository<MiljoeklasseType> _miljoeklasseTypeRepo;
    private readonly IEnhedTypeRepository _enhedTypeRepository;
    private readonly IKommuneJordklassifikationRepository _kommuneJordklassifikationRepository;
    private readonly IAdvisTypeRepository _advisTypeRepository;
    private readonly IStatusStikproeveTypeRepository _stikproeveTypeRepository;
    private readonly ILandsdelTypeRepository _landsdelTypeRepository;
    private readonly IKommuneBusiness _kommuneBusiness;



    public KodelisteBusiness(
      IDokumentationTypeRepository dokumentationTypeRepo,
      IKommuneRepository kommuneRepo,
      IOprindelsesstedKlassifikationTypeRepository oprindelsesstedKlassifikationTypeRepo,
      IAndenOprindJordTypeRepository andenOprindJordTypeRepo,
      IAffaldTypeRepository affaldTypeRepo,
      IEnhedTypeRepository enhedTypeRepository,
      IJordKlassifikationTypeRepository jordKlassifikationTypeRepo,
      IJordflytningTypeRepository jordflytningTypeRepo,
      IRepository<MiljoeklasseType> miljoeklasseTypeRepo,
      IJordanlaegTypeRepository jordanlaegTypeRepo,
      IKommuneJordklassifikationRepository kommuneJordklassifikationRepository,
      IAdvisTypeRepository advisTypeRepository,
      IStatusStikproeveTypeRepository stikproeveTypeRepository,
      ILandsdelTypeRepository landsdelTypeRepository,
      IKommuneBusiness kommuneBusiness
      )
    {
      _dokumentationTypeRepo = dokumentationTypeRepo;
      _kommuneRepo = kommuneRepo;
      _oprindelsesstedKlassifikationTypeRepo = oprindelsesstedKlassifikationTypeRepo;
      _andenOprindJordTypeRepo = andenOprindJordTypeRepo;
      _affaldTypeRepo = affaldTypeRepo;
      _enhedTypeRepository = enhedTypeRepository;
      _jordKlassifikationTypeRepo = jordKlassifikationTypeRepo;
      _jordflytningTypeRepo = jordflytningTypeRepo;
      _miljoeklasseTypeRepo = miljoeklasseTypeRepo;
      _jordanlaegTypeRepo = jordanlaegTypeRepo;
      _kommuneJordklassifikationRepository = kommuneJordklassifikationRepository;
      _advisTypeRepository = advisTypeRepository;
      _stikproeveTypeRepository = stikproeveTypeRepository;
      _landsdelTypeRepository = landsdelTypeRepository;
      _kommuneBusiness = kommuneBusiness;
    }


    #region MiljøklasseType

    public IList<MiljoeklasseType> ReadAktiveMiljoeklasseTyper()
    {
      return _miljoeklasseTypeRepo.Search(x => x.Aktiv).OrderBy(x => x.Navn).ToList();
    }

    #endregion

    #region DokumentationType

    public DokumentationType ReadDokumentationType(Guid id)
    {
      return _dokumentationTypeRepo.Read(id);
    }

    public IList<DokumentationType> ReadAktiveDokumentationType()
    {
      //return (from d in _dokumentationTypeRepo.Read() where d.Aktiv orderby d.Navn select d).ToList();
      return (from d in _dokumentationTypeRepo.Search(x=>x.Aktiv) orderby d.Navn select d).ToList();
    }

    public IList<DokumentationType> ReadAktiveDokumentationType(ESortType sorteringsmetode)
    {
      switch (sorteringsmetode)
      {
        case ESortType.Alfabetisk:
          return ReadAktiveDokumentationType();
        case ESortType.SorteringVaerdi:
          throw new Exception("Mangler sortering attribut på dokumentationType");
        //return (from d in dokumentationTypeRepo.Read() where (d.Aktiv.HasValue && d.Aktiv == true) orderby d.sortering select d).ToList;
        default:
          return ReadAktiveDokumentationType();
      }
    }

    #endregion

    public IList<Kommune> ReadAktiveKommune()
    {
      //return (from k in _kommuneRepo.Read().Where(kommune => kommune.Aktiv) orderby k.Navn select k).ToList();
      return (from k in _kommuneRepo.Search(kommune => kommune.Aktiv) orderby k.Navn select k).ToList();
    }

    public IList<Kommune> ReadAllKommuner()
    {
      return (from k in _kommuneRepo.Read() orderby k.Navn select k).ToList();
    }

    public Kommune ReadKommuneById(Guid id)
    {
      //return (from k in _kommuneRepo.Read().Where(x => x.Id == id) select k).FirstOrDefault();
      return (from k in _kommuneRepo.Search(x => x.Id == id) select k).FirstOrDefault();
    }

    public Kommune ReadKommuneByNavn(string kommunenavn)
    {
      //Vi anvender metoden i KommuneBusiness, 
      // gamle funkition return (from k in _kommuneRepo.Read().Where(x => x.Navn.ToLower()==kommunenavn.ToLower()) select k).FirstOrDefault();
      return _kommuneBusiness.ReadKommuneByNavn(kommunenavn);

    }

    public Kommune ReadKommuneByKode(int kommunekode)
    {
        return _kommuneRepo.Read().FirstOrDefault(k => k.Kommunenr == kommunekode && k.Kommunenr > 0);
    }

    public OprindelsesstedKlassifikationType ReadOprindelsesstedKlassifikationType(Guid id)
    {
      //return (from o in _oprindelsesstedKlassifikationTypeRepo.Read().Where(i => i.Id == id) select o).FirstOrDefault();
      return (from o in _oprindelsesstedKlassifikationTypeRepo.Search(i => i.Id == id) select o).FirstOrDefault();
    }

    public OprindelsesstedKlassifikationType ReadOprindelsesstedKlassifikationType(short kode)
    {
      //return (from o in _oprindelsesstedKlassifikationTypeRepo.Read().Where(i => i.Kode == kode) select o).FirstOrDefault();
      return (from o in _oprindelsesstedKlassifikationTypeRepo.Search(i => i.Kode == kode) select o).FirstOrDefault();
    }

    public IList<OprindelsesstedKlassifikationType> ReadAktiveOprindelsesstedKlassifikationTypes()
    {
      return (from o in _oprindelsesstedKlassifikationTypeRepo.Search(op => op.Aktiv) orderby o.Navn select o).ToList();
    }

    public AndenOprindJordType ReadAndenOprindJordType(Guid id)
    {
      //return (from o in _andenOprindJordTypeRepo.Read().Where(t => t.Id == id) select o).FirstOrDefault();
      return (from o in _andenOprindJordTypeRepo.Search(t => t.Id == id) select o).FirstOrDefault();
    }

    public IList<AndenOprindJordType> ReadAktiveAndenOprindJordTypes()
    {
      return (from o in _andenOprindJordTypeRepo.Search(op => op.Aktiv) orderby o.Navn select o).ToList();
    }

    public AffaldType ReadAffaldType(Guid id)
    {
      //return (from a in _affaldTypeRepo.Read().Where(c => c.Id == id) select a).FirstOrDefault();
      return (from a in _affaldTypeRepo.Search(c => c.Id == id) select a).FirstOrDefault();
    }

    public IList<AffaldType> ReadAktiveAffaldTypes()
    {
      return (from a in _affaldTypeRepo.Search(af => af.Aktiv) orderby a.Sortering select a).ToList();
    }

    public JordKlassifikationType ReadJordKlassifikationType(Guid id)
    {
      //return (from jk in _jordKlassifikationTypeRepo.Read().Where(o => o.Id == id) select jk).FirstOrDefault();
      return (from jk in _jordKlassifikationTypeRepo.Search(o => o.Id == id) select jk).FirstOrDefault();
    }

    public IList<JordKlassifikationType> ReadAktiveJordKlassifikationTypesForLandsdel(Guid landsdel)
    {

      var list = (from jk in _jordKlassifikationTypeRepo.Search(k => k.Aktiv && k.LandsdelType != null && k.LandsdelType.Id == landsdel)
                  select jk).OrderBy(o => o.Sortering).ToList();

      return list;

    }

    public IList<JordKlassifikationType> ReadAktiveJordKlassifikationTypesForKommune(Guid kommuneId)
    {

      var list = (from kj in _kommuneJordklassifikationRepository.Search(k => k.Aktiv && k.Kommune.Id == kommuneId)
                  select kj.JordKlassifikationType).OrderBy(o => o.Sortering).ToList();
      return list;

    }

    public IList<JordKlassifikationType> ReadAktiveJordKlassifikationTypesForKommune(string navn)
    {
	    var kom = ReadKommuneByNavn(navn);

      var list = (from kj in _kommuneJordklassifikationRepository.Search(k => k.Aktiv && k.Kommune.Id==kom.Id)
                  select kj.JordKlassifikationType).OrderBy(o => o.Sortering).ToList();
      return list;

    }

    public IList<JordKlassifikationType> ReadAktiveJordKlassifikationTypesForKommune(short kommuneNr)
    {

        var list = (from kj in _kommuneJordklassifikationRepository.Search(k => k.Aktiv && k.Kommune.Kommunenr == kommuneNr)
                    select kj.JordKlassifikationType).OrderBy(o => o.Sortering).ToList();
        return list;
    }

    public JordflytningType ReadJordflytningType(Guid id)
    {
      //return (from j in _jordflytningTypeRepo.Read().Where(i => i.Id == id) select j).FirstOrDefault();
      return (from j in _jordflytningTypeRepo.Search(i => i.Id == id) select j).FirstOrDefault();
    }

    public IList<JordflytningType> ReadAktiveJordflytningTypes()
    {
      return (from j in _jordflytningTypeRepo.Search(jt => jt.Aktiv) orderby j.Sortering select j).ToList();
    }

    public IList<JordanlaegType> ReadAktiveJordanlaegTypes()
    {
      var list = (from j in _jordanlaegTypeRepo.Search(jt => jt.Aktiv) orderby j.Sortering select j).ToList();
      return list;
    }

    public IList<Enhed> ReadAktiveEnheder()
    {
      var list = (from j in _enhedTypeRepository.Search(jt => jt.Aktiv) orderby j.Sortering select j).ToList();
      return list;
    }

    public IList<AdvisType> ReadAktiveAdvisTypes()
    {
      var list = (from a in _advisTypeRepository.Search(at => at.Aktiv) orderby a.Sortering select a).ToList();
      return list;
    }

    public AdvisType ReadAdvisType(Guid id)
    {
      return _advisTypeRepository.Read(id);
    }

    public List<StatusStikproeveType> ReadAktiveStikproeveStatusTyper()
    {
      var list = _stikproeveTypeRepository.Read();
      return list.ToList();
    }

    public StatusStikproeveType ReadStikproeveStatusType(Guid statusGuid)
    {
      var statType = _stikproeveTypeRepository.Read(statusGuid);
      return statType;

    }

    public IList<LandsdelType> ReadAktiveLandsdelTypes()
    {

      var landsdeltypes = _landsdelTypeRepository.Search(lt => lt.Aktiv).OrderBy(s => s.Sortering).ToList();
      return landsdeltypes;
    }

    public LandsdelType ReadLandsdelType(Guid id)
    {
      return _landsdelTypeRepository.Read(id);
    }
  }
}

