using System;
using System.Collections.Generic;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic
{
  public class BogholderOpslagstavleBusiness :  GenericBusiness<BogholderOpslagstavle>, IBogholderOpslagstavleBusiness   
  {
		//private IUnitOfWork uow;
    private IBogholderOpslagstavleRepository _bogholderOpslagstavleRepository;


    public BogholderOpslagstavleBusiness(IBogholderOpslagstavleRepository bogholderOpslagstavleRepository, IUnitOfWork uow)
      : base(bogholderOpslagstavleRepository, uow)
    {
      this._bogholderOpslagstavleRepository = bogholderOpslagstavleRepository;
      
			//this.uow = uow;
    }

   
  }
}
