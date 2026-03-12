using System;
using System.Collections.Generic;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic
{
  public class JordforureningsopslagBusiness :  GenericBusiness<Jordforureningsopslag>, IJordforureningsopslagBusiness
  {
		//private IUnitOfWork uow;

    private IJordforureningsopslagRepository _jordforureningsopslagRepository;
    
    public JordforureningsopslagBusiness(IJordforureningsopslagRepository jordforureningsopslagRepository, IUnitOfWork uow)
      : base(jordforureningsopslagRepository, uow)
    {
      _jordforureningsopslagRepository = jordforureningsopslagRepository;
      
    }

    
  }
}
