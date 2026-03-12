using System;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
	public interface IBetalerBusiness : IGenericBusiness<Betaler>
	{
	  Betaler ReadBetaler(Guid personId);
	  bool IsAnmelderBemyndigetByBetaler(Anmelder anmelder, Betaler betaler);
    bool CheckAutoBetalerAccepterBetaling(Anmeldelse anmeldelse);
	}
}
