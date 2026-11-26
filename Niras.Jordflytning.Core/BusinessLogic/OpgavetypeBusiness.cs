using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Niras.Jordflytning.Core.BusinessLogic
{
    public class OpgavetypeBusiness : GenericBusiness<Opgavetype>, IOpgavetypeBusiness
    {
        private IOpgavetypeRepository _repository;

        public OpgavetypeBusiness(IOpgavetypeRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
            _repository = repository;
        }

        public IEnumerable<Opgavetype> ForKommune(Guid kommuneId)
        {
            return _repository.Search(x => x.Kommune.Id == kommuneId).OrderBy(x => x.Sortering);
        }

        public bool ErGyldigPaaDato(Opgavetype opgavetype, DateTime dato)
        {
            if (opgavetype == null)
                return false;

            var dag = dato.Date;
            if (opgavetype.FraDato.HasValue && opgavetype.FraDato.Value.Date > dag)
                return false;
            if (opgavetype.TilDato.HasValue && opgavetype.TilDato.Value.Date < dag)
                return false;
            return true;
        }

    }
}
