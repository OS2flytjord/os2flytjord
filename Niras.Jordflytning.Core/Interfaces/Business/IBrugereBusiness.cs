using System.Collections.Generic;
using Niras.Jordflytning.Core.Models;
using System;

namespace Niras.Jordflytning.Core.Interfaces.Business
{
	public interface IBrugereBusiness : IGenericBusiness<BrugerProfil>
	{
		Boolean Create(BrugerProfil userProfile, String[] roles,String organisationsId = null);
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
	  Sagsbehandler ReadSagsbehandler(Guid sagsbehandlerId);
		void DeletePersonKommune(PersonKommune personKommune);
		void DeletePersonJordmodtager(PersonJordmodtager personJordmodtager);
	  PersonJordmodtager ReadJordmodtagerPerson(Guid jordmodtagerId, Guid personId);
	  PersonKommune ReadKommunePerson(Guid kommuneId, Guid personId);

	}
}
