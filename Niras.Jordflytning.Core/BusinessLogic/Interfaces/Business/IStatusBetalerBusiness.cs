using System;
using System.Collections.Generic;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
	public interface IStatusBetalerBusiness : IGenericBusiness<StatusBetaler>
	{
    bool AnmodOmGodkendelseHosJordmodtager(Betaler betaler, Jordmodtager jordmodtager);
    IList<StatusBetaler> GetStatusBetalerForBetaler(Betaler betaler, Guid jordmodtagerId);
	  StatusBetaler GetStatusBetaler(Guid betalerId, Guid jordmodtagerId);

		bool IsBetalerGodkendtForJordmodtager(Guid jordmodtagerId, Guid betalerId);
		bool IsBetalerSpaerretForJordmodtager(Guid jordmodtagerId, Guid betalerId);
		bool? GetGodkendtStatusForBogholder(Anmeldelse anmeldelse);
	}
}
