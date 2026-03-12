using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic
{
	public class KonfigBusiness : GenericBusiness<Konfig>, IKonfigBusiness
  {
    private IKonfigRepository _konfigRepo;
		private IUnitOfWork unitOfWork;

    public KonfigBusiness(IKonfigRepository konfigRepo, IUnitOfWork unitOfWork) : base(konfigRepo, unitOfWork)
    {
      this._konfigRepo = konfigRepo;
	    this.unitOfWork = unitOfWork;
    }


    public string ReadKommuneKonfig(string key, Guid kommuneId)
	  {
      //return ( from k in _konfigRepo.Read().Where(k => k.Key.ToLower() == key.ToLower() & k.KommuneId == kommuneId) select k.Value).FirstOrDefault();
      return (from k in _konfigRepo.Search(k => k.Key.ToLower() == key.ToLower() & k.KommuneId == kommuneId) select k.Value).FirstOrDefault();
    }
  }
}
