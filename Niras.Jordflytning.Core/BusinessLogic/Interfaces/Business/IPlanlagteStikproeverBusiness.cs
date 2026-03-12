using System;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
	public interface IPlanlagteStikproeverBusiness : IGenericBusiness<PlanlagteStikproever>
	{
		PlanlagteStikproever GetPlanlagtUdenStikproeve(Guid anmeldelseId);
		void UpdateStikproeve(Guid anmeldelseId, Vognlaes vognlaes, bool forceCreateStikproeve, int? stikproeveBaas);
	}
}
