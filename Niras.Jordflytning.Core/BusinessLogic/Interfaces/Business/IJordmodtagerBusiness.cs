using System;
using System.Collections.Generic;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
	public interface IJordmodtagerBusiness : IGenericBusiness<Jordmodtager>
	{
	  IList<Jordmodtager> ReadAktiveJormodtagere();
		IList<Jordmodtager> GetJordModtagerListForUser(Guid userId);

		IList<Jordmodtager> ReadJordmodtagere();
        //IList<Jordmodtager> GetModtagerAnlaegForAll(string searchparam);
		IList<Jordmodtager> GetJordModtagerListForUserAll(Guid userId);

		void AddAnlaegToPerson(Guid jordmodtagerId, Guid personId);
		void RemoveAnlaegFromPerson(Guid jordmodtagerId, Guid personId);
	}
}
