using System;
using System.Collections.Generic;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic
{
    public class AnmelderBusiness : GenericBusiness<Anmelder>,  IAnmelderBusiness
    {
        private readonly IAnmelderRepository _anmelderRepo;

		public AnmelderBusiness(IAnmelderRepository anmelderRepo, IUnitOfWork uow) : base(anmelderRepo, uow)
		{
			_anmelderRepo = anmelderRepo;
        }

        public Anmelder ReadAnmelder(Guid personId)
        {
			    return (from k in _anmelderRepo.Search(x => x.Person.Id == personId) select k).FirstOrDefault();
        }

        public IList<Anmelder> ReadAnmeldereBypassEf()
        {
            return _anmelderRepo.ExecuteSqlQuery("EXEC [SP_SELECT_AnmelderList]").ToList();
        }
    }
}