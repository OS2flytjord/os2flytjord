using System;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Core.BusinessLogic
{
	public class PlanlagteStikproeveBusiness : GenericBusiness<PlanlagteStikproever>, IPlanlagteStikproeverBusiness
	{
		private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.PlanlagteStikproeveBusiness");

		private readonly IPlanlagteStikproeverRepository _planlagteStikproeverRepo;
		private readonly IStikproeveBusiness _stikproeveBusiness;

		public PlanlagteStikproeveBusiness(IPlanlagteStikproeverRepository planlagteStikproeverRepository,
			IStikproeveBusiness stikproeveBusiness, IUnitOfWork uow): base(planlagteStikproeverRepository, uow)
		{
			_planlagteStikproeverRepo = planlagteStikproeverRepository;
			_stikproeveBusiness = stikproeveBusiness;
		}

		public PlanlagteStikproever GetPlanlagtUdenStikproeve(Guid anmeldelseId)
		{
      //En planlagtstikprøve har en stikprøve, men ingen vognlæs.
      //var plStikProeve = (from st in _planlagteStikproeverRepo.Read()
      //                    where st.AnmeldelseId == anmeldelseId 
      //                    && st.Stikproeve != null 
      //                    && st.Stikproeve.Vognlaes == null 
      //                    select st)
      //                    .FirstOrDefault();

      var plStikProeve = (from st in _planlagteStikproeverRepo.Search(x=> x.AnmeldelseId == anmeldelseId
                          && x.Stikproeve != null
                          && x.Stikproeve.Vognlaes == null)
                          select st)
                          .FirstOrDefault();

			return plStikProeve;
		}

		public void UpdateStikproeve(Guid anmeldelseId, Vognlaes vognlaes, bool forceCreateStikproeve, int? stikproeveBaas)
		{
			try
			{
				//Der søges på planlagtestikprøver uden nogen vognlæs.
				var planlagtstikproeve = GetPlanlagtUdenStikproeve(anmeldelseId);
				if (planlagtstikproeve != null)
        {
          var planlagtstikproeveId = planlagtstikproeve.Id;
					_stikproeveBusiness.KnytVognlæsTilPlanlagtStikprøve(vognlaes,stikproeveBaas,planlagtstikproeve);

					// Når der er knyttet et vognlæs til stikprøven er der en reference til anmeldelsen 
					// og hermed kan planlagtstikprøve slettes.
          Delete(Read(planlagtstikproeveId));
				}
				else
				{
					if (forceCreateStikproeve)
					{
						_stikproeveBusiness.CreateStikProeve(vognlaes, stikproeveBaas);
					}
				}			
			}
			catch (Exception e)
			{
				Logger.LogException("UpdateStikproeve: Fejl! ", e);
				throw;
			}
		}

	}
}
