using System.Collections.Generic;
using Niras.Jordflytning.Core.Models;
using System;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
  public interface IBrugereBusiness : IGenericBusiness<BrugerProfil>
  {
    bool ProfileExists(string username);
    Boolean Create(BrugerProfil userProfile, String[] roles, String organisationsId = null);
    Boolean CreateSilent(BrugerProfil userProfile, String[] roles, String organisationsId = null);
    BrugerProfil Read(String userName);
    Boolean CreateBetaler(BrugerProfil brugerProfil);
    Boolean Update(BrugerProfil userProfile, IList<String> roles);
    Boolean AddUserToRole(String userName, String roleName);
    Boolean RemoveUserFromRole(String userName, String roleName);
    IList<PersonKommune> ReadPersonKommune();
    IList<PersonJordmodtager> ReadPersonJordmodtager();
    IList<PersonKommune> ReadKommunePersoner(Guid kommuneId);
    IList<PersonJordmodtager> ReadJordmodtagerPersoner(Guid jordmodtagerId);
    IList<Person> ReadAktiveSagsbehandlere(Guid kommuneId);
    IList<Person> ReadAktivePladsmaend(Guid jordmodtagerId);
    IList<Person> ReadAktiveMiljoemedarbejdere(Guid jordmodtagerId);
    IList<Person> ReadAktiveProevetagere(Guid? jordmodtagerId);
    IList<Person> ReadAktiveLaboratoriepersoner(Guid jordmodtagerId);
    Sagsbehandler ReadSagsbehandler(Guid sagsbehandlerId);
    void DeletePersonKommune(PersonKommune personKommune);
    void DeletePersonJordmodtager(PersonJordmodtager personJordmodtager);
    PersonJordmodtager ReadJordmodtagerPerson(Guid jordmodtagerId, Guid personId);
    PersonKommune ReadKommunePerson(Guid kommuneId, Guid personId);
    IList<Jordmodtager> ReadJordmodtagerForPerson(Guid personId);
    Person ReadPerson(Guid personId);
    //void SendAktivationEmail(BrugerProfil userProfile, string activationToken);

	  IList<BrugerProfil> GetBrugerProfilList(); 

		List<Kommune> GetUsersKommuneOrganisationer(Guid id);
    List<Jordmodtager> GetUsersJordModtOrganisationer(Guid id);
    IList<String> GetUserRole(int brugerId);
	  IList<String> GetUserRoleList(Guid userId);

    bool IsUserRelatedToSevelralKommunerOrJordmodtagere(string email,Guid organisationId);

		Person GetCurrentLoggedOnPerson();
  }
}
