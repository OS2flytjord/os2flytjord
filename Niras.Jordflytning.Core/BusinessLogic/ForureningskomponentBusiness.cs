using System;
using System.Collections.Generic;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic
{
	public class ForureningskomponentBusiness : GenericBusiness<Forureningskomponent>, IForureningskomponentBusiness
  {
    private readonly IForureningskomponentRepository _forureningskomponentRepository;

		public ForureningskomponentBusiness(IForureningskomponentRepository forureningskomponentRepository, IUnitOfWork unitOfWork): base(forureningskomponentRepository, unitOfWork)
    {
      _forureningskomponentRepository = forureningskomponentRepository;
    }

    public IList<Forureningskomponent> ReadAktiveForureningskomponenter()
    {
      var fk = (from f in _forureningskomponentRepository.Search(x => x.Udloebsdato ==null || x.Udloebsdato > DateTime.Now) select f).ToList();
      return fk;
    }


  }
}
