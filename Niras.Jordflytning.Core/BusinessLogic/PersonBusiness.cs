using System;
using System.Collections.Generic;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Core.BusinessLogic
{
  public class PersonBusiness : GenericBusiness<Person>, IPersonBusiness
  {
		private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.PersonBusiness");

    private readonly IPersonRepository _personRepo;
    private readonly IFirmaoplysningerRepository _firmaoplysningerRepository;

    public PersonBusiness(IPersonRepository personRepo, IFirmaoplysningerRepository firmaoplysningerRepository, IUnitOfWork unitOfWork)
      : base(personRepo, unitOfWork)
    {
      _personRepo = personRepo;
      _firmaoplysningerRepository = firmaoplysningerRepository;
    }

    public Person ReadByBrugerId(Int32 brugerId)
    {
      Person pers = null;

      try
      {
        //var persList = _personRepo.Read();
        //var persList2 = persList.Where(x => x.BrugerId == brugerId).ToList();
        //pers = persList2.FirstOrDefault();
        
        pers = _personRepo.Search(x => x.BrugerId == brugerId).FirstOrDefault();

      }
      catch (Exception ex)
      {
        //Logger.LogWarning("Error in method: ReadByBrugerId. (Dette er en asyncfejl. Ødelægger ikke noget, men...)");
        
        Logger.LogException("Error in method: ReadByBrugerId. ", ex);

      }

      return pers;
    }

    /// <summary>
    /// Find Person udfra del af email.
    /// </summary>
    public IList<Person> SearchByEmail(string email)
    {
      var res = (
                  from p in _personRepo
                    .Search(x => (x.Email.Contains(email) || x.Navn.Contains(email) || x.Efternavn.Contains(email) || (x.Navn + x.Efternavn).Contains(email)) 
                        & x.Aktiv)
                  select p).ToList();
      return res;
    }

    /// <summary>
    /// Hent andre personer i samme firma
    /// </summary>
    public List<Person> GetAndrePersonerIfirma(Guid brugerId)
    {
      var list = new List<Person>();
      var bruger = _personRepo.Read(brugerId);

      if (bruger.Firmaoplysninger != null)
      {
        var cvr = bruger.Firmaoplysninger.CVR;
        var emailStr = bruger.Email;

        var emailDomaine = GetDomaineFromEmail(emailStr);
        if (!String.IsNullOrEmpty(emailDomaine))
        {

          // find firmaer udfra cvr
          //var firmaList = (from v in _firmaoplysningerRepository.Read()
          //                 where v.CVR == cvr
          //                 select v.Id).ToList();
         
          var firmaList = (from v in _firmaoplysningerRepository.Search(x=> x.CVR == cvr) select v.Id).ToList();

          // find brugere med samme firma
          //var brugerListe = from b in _personRepo.Read()
          //                  where b.FirmaoplysningerId != null
          //                        && firmaList.Contains((Guid)b.FirmaoplysningerId)
          //                  select b;
          
          var brugerListe = from b in _personRepo.Search(x=> x.FirmaoplysningerId != null
                                  && firmaList.Contains((Guid)x.FirmaoplysningerId)) select b;

          // find brugere med samme domaine i email adressen
	        foreach (var person in brugerListe)
	        {
		        if (String.IsNullOrEmpty(person.Email))
			        continue;

		        var otherBrugerDomaine = GetDomaineFromEmail(person.Email);

		        if (String.IsNullOrEmpty(otherBrugerDomaine))
			        continue;

		        if (emailDomaine.ToUpperInvariant() == otherBrugerDomaine.ToUpperInvariant())
			        list.Add(person);
	        }
        }
      }
      else
      {
        //Vi tilføjer brugeren som kommer med som input til denne metode
        list.Add(bruger);
      }
      return list;
    }

    private static string GetDomaineFromEmail(string emailStr)
    {
      var domaineStr = "";
      if (!String.IsNullOrEmpty(emailStr))
      {
        var strList = emailStr.Split('@');
        if (strList.Length > 1)
        {
          domaineStr = strList[1];
        }
      }
      return domaineStr;
    }
  }
}
