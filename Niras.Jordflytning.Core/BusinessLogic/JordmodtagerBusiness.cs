using System;
using System.Collections.Generic;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic
{
    public class JordmodtagerBusiness : GenericBusiness<Jordmodtager>, IJordmodtagerBusiness
    {
        private readonly IJordmodtagerRepository _jordmodtagerRepository;

        public JordmodtagerBusiness(IJordmodtagerRepository jordmodtagerRepository, IUnitOfWork uow)
            : base(jordmodtagerRepository, uow)
        {
            _jordmodtagerRepository = jordmodtagerRepository;
        }

        public IList<Jordmodtager> ReadAktiveJormodtagere()
        {
            return (from j in _jordmodtagerRepository.Search(x => x.Aktiv) select j).ToList();
        }

        public IList<Jordmodtager> ReadJordmodtagere()
        {
            var list = (from j in _jordmodtagerRepository.Read()
                        orderby j.Aktiv
                        select j).ToList();
            return list;
        }

        //public IList<Jordmodtager> GetModtagerAnlaegForAll(string findfirma)
        //{
        //    var list = (from j in _jordmodtagerRepository.Read()
        //                   orderby j.Aktiv
        //                   select j).ToList();

        //    //var modtagerlist = list.SelectMany(q => q.ModtagerAnlaeg).Distinct().ToList();

        //    return list;
        //}

        public IList<Jordmodtager> GetJordModtagerListForUserAll(Guid userId)
        {
            var list =
                (from j in _jordmodtagerRepository.Read()
                 orderby j.Aktiv
                 select j);

            var list2 = list
                .Where(x => x.PersonJordmodtager.Any(u => u.PersonId == userId)).ToList();
            return list2;
        }

        public IList<Jordmodtager> GetJordModtagerListForUser(Guid userId)
        {
            var list =
                (from j in _jordmodtagerRepository.Search(x => x.Aktiv)
                 select j);

            var list2 = list
                .Where(x => x.PersonJordmodtager.Any(u => u.PersonId == userId)).ToList();
            return list2;
        }

        public void AddAnlaegToPerson(Guid jordmodtagerId, Guid personId)
        {
            var jordmodtager = _jordmodtagerRepository.Read(jordmodtagerId);
            var persJormodt = new PersonJordmodtager
                {
                    Id = new Guid(),
                    JordmodtagerId = jordmodtager.Id,
                    PersonId = personId
                };
            jordmodtager.PersonJordmodtager.Add(persJormodt);
            SaveChanges();
        }

        public void RemoveAnlaegFromPerson(Guid jordmodtagerId, Guid personId)
        {
            var jordmodtager = _jordmodtagerRepository.Read(jordmodtagerId);
            var persJormodt = jordmodtager.PersonJordmodtager.FirstOrDefault(x => x.JordmodtagerId == jordmodtagerId);
            jordmodtager.PersonJordmodtager.Remove(persJormodt);
            SaveChanges();

        }
    }
}
