using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic
{
    public class TransportoerBusiness :  GenericBusiness<Transportoer>,  ITransportoerBusiness
    {
        private readonly ITransportoerRepository _transRepo;
        private readonly IAnmeldelserRepository _anmeldelserRepo;

	    public TransportoerBusiness(ITransportoerRepository transRepo, IAnmeldelserRepository anmeldelserRepository, IUnitOfWork uow) : base(transRepo, uow)
        {
            _transRepo = transRepo;
            _anmeldelserRepo = anmeldelserRepository;
        }

        public enum EnumFiltreringsMetode
        {
            //I prioteret rækkefølge
            Tidligere = 1,
            Alle = 2
        }

        public IList<Transportoer> ReadAktiveTransportoerer()
        {
            return _transRepo.Search(t => t.Person.Aktiv && t.Aktiv).ToList();
        }

        public IList<Transportoer> ReadTidligereAktiveAnvendteTransportoerer(Guid personid)
        {   
            return (
            from a in _anmeldelserRepo.Search(a =>
                a.Anmelder != null &&
                a.Anmelder.Id == personid &&
                a.Transportoer != null &&
                a.Transportoer.Person != null &&
                a.Transportoer.Person.Firmaoplysninger != null)
            select a.Transportoer)
            .Distinct()
            .OrderBy(t => t.Person.Firmaoplysninger.Firmanavn)
            .ToList();     
        }

        public Transportoer ReadTransportoer(Guid personId)
        {
            return (
				    from k in _transRepo
					    .Search(x => x.Id == personId) 
				    select k).FirstOrDefault();
        }

        public Transportoer ReadTransportoer(decimal? cvr)
        {
            return (
                        from k in _transRepo
                            .Search(x => x.Person.Firmaoplysninger.CVR == cvr)
                        select k).FirstOrDefault();
        }

        public IList<Transportoer> ReadTransportoererBypassEf()
        {
            return _transRepo.ExecuteSqlQuery("EXEC dbo.[SP_SELECT_TransportoerList]").ToList();
        }
    }
}
