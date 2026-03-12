using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic
{
  public class LastbilBusiness : GenericBusiness<Lastbil>, ILastbilBusiness
  {
    private readonly ILastbilRepository _lastbilRepository;

		public LastbilBusiness(ILastbilRepository lastbilRepository, IUnitOfWork unitOfWork) 
			: base(lastbilRepository, unitOfWork)
    {
			_lastbilRepository = lastbilRepository;
    }

    public List<Lastbil> GetAktiveLastbilList()
    {
      //var list = (from last in _lastbilRepository.Read() where last.Aktiv select last).ToList();
      var list = (from last in _lastbilRepository.Search(x=>x.Aktiv) select last).ToList();

	    return list;
    }

  }
}
