using System;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
	public interface IBemyndigedeAnmelderBusiness
	{
		void AddBemyndigedeAnmelder(Guid personIdAnmelder, Guid personIdBetaler);
		void RemoveBemyndigedeAnmelder(Guid personIdAnmelder, Guid personIdBetaler);	
	}
}
