using System;
using System.Collections.Generic;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.MatrikelOpslag;

namespace Niras.Jordflytning.Core.BusinessLogic
{
    public class MatrikelBusiness : GenericBusiness<Matrikel>, IMatrikelBusiness
    {
        private readonly IMatrikelRepository _matrikelRepository;
        private readonly IMatrikelOpslagRepository _matrikelOpslagRepository;

        public MatrikelBusiness(IMatrikelRepository matrikelRepository, IMatrikelOpslagRepository matrikelOpslagRepository, IUnitOfWork unitOfWork)
            : base(matrikelRepository, unitOfWork)
        {
            _matrikelRepository = matrikelRepository;
            _matrikelOpslagRepository = matrikelOpslagRepository;
        }

        public MatrikelOpslagResultat GetMatrikel(string ejerlavkode, string matrikelnummer)
        {
            return _matrikelOpslagRepository.Get(ejerlavkode, matrikelnummer);
        }

        /// <summary>
        /// Hent de matrikler der intersecter med den
        /// givne wkt geometri (enten et punkt eller 
        /// en polygom)
        /// </summary>
        public IList<Matrikel> ReadMatrikler(string wkt)
        {
            var listRest = _matrikelOpslagRepository.GetListFromRestService(wkt);
            //var list = _matrikelRepository.GetListFromWfsService(wkt);
            return listRest;
        }

        public void DeleteMatrikler(Anmeldelse anmeldelse)
        {
            if (anmeldelse != null &&
              anmeldelse.Oprindelsessted != null &&
              anmeldelse.Oprindelsessted.Matrikel != null &&
              anmeldelse.Oprindelsessted.Matrikel.Count > 0)
            {
                var guids = (from m in anmeldelse.Oprindelsessted.Matrikel select m.Id).ToList();

                foreach (var g in guids)
                {
                    var mm = _matrikelRepository.Read(g);
                    if (mm != null)
                    {
                        _matrikelRepository.Delete(mm);
                    }

                }
                //SaveChanges();
            }
        }

        public string GetEsrEjendomsnummer(string ejerlavkode, string matrikelnr)
        {
            return _matrikelOpslagRepository.GetEsrEjendomsnummer(ejerlavkode, matrikelnr);
        }

    }
}
