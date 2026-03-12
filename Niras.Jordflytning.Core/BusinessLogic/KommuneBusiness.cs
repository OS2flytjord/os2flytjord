using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic
{
  public class KommuneBusiness : GenericBusiness<Kommune>, IKommuneBusiness
  {
    private readonly IPersonKommuneRepository _personkommuneRepo;
    private readonly IKommuneRepository _kommuneRepo;

    public KommuneBusiness(IKommuneRepository kommuneRepo, IPersonKommuneRepository personkommuneRepo, IUnitOfWork unitOfWork)
      : base(kommuneRepo, unitOfWork)
    {
      _kommuneRepo = kommuneRepo;
      _personkommuneRepo = personkommuneRepo;
    }

    public IList<Kommune> ReadAktiveKommuner()
    {
      return (from k in _kommuneRepo.Search(x => x.Aktiv) orderby k.Navn select k).ToList();
    }

    public Kommune ReadKommune(Guid kommuneId)
    {
      return (from k in _kommuneRepo.Search(x => x.Id == kommuneId) select k).FirstOrDefault();
    }

    public Kommune ReadKommuneByNavn(string kommunenavn)
    {

      //Alt efter hvilken adresse servicen vi bruger kommer kommunenavn med eller uden Kommune efter bynavnet. Fx Faverskov og Faverskov Kommune.
      //Århus har skriftet navn til Aarhus. Det er dog ikke opdateret i AWS.
      //I databasen står kommunenavnet uden "Kommune"

      var komList = ( from k in _kommuneRepo.Read() where kommunenavn.ToLower().Contains(k.Navn.ToLower()) && k.Kommunenr > 0 select k);
      //var komList = (from k in _kommuneRepo.Search(x=> kommunenavn.ToLower().Contains(x.Navn.ToLower()) && x.Kommunenr > 0)  select k);
      
      var kom = komList.FirstOrDefault();

      if (kom == null && kommunenavn.StartsWith("Å"))
      {
        //Fix for Århus vs Aarhus.
        komList = (
                  from k in _kommuneRepo.Read()
                  where kommunenavn.ToLower().Replace("å", "aa").Contains(k.Navn.ToLower())
                        && k.Kommunenr > 0
                  select k);
        kom = komList.FirstOrDefault();
      }
      
      return kom;
    }

    public Guid GetKommuneIdByUserId(Guid userId)
    {
      var persKommune = _personkommuneRepo.Search(x => x.PersonId == userId).FirstOrDefault();
      var komId = Guid.Empty;
      if (persKommune != null)
        komId = persKommune.KommuneId;

      return komId;
    }


  }
}
