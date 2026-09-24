using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Tools.Datafordeler;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Niras.Jordflytning.Core.BusinessLogic
{
    public class MatrikelBusiness : GenericBusiness<Matrikel>, IMatrikelBusiness
    {
        private readonly IMatrikelRepository _matrikelRepository;

        public MatrikelBusiness(IMatrikelRepository matrikelRepository, IUnitOfWork unitOfWork)
            : base(matrikelRepository, unitOfWork)
        {
            _matrikelRepository = matrikelRepository;
        }

        /// <summary>
        /// Hent de matrikler der intersecter med den
        /// givne wkt geometri (enten et punkt eller 
        /// en polygom)
        /// </summary>
        public IList<Matrikel> ReadMatrikler(string wkt)
        {
            var datafordelerApiKey = ConfigurationManager.AppSettings["datafordelerApiKey"];
            DatafordelerMatrikelClient matrikkelClient = new DatafordelerMatrikelClient(datafordelerApiKey);
            return matrikkelClient.HentMatrikler(wkt);
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
            //return _matrikelOpslagRepository.GetEsrEjendomsnummer(ejerlavkode, matrikelnr);

            var datafordelerApiKey = ConfigurationManager.AppSettings["datafordelerApiKey"];
            var client = new DatafordelerGraphQLClient("MAT", "v2", datafordelerApiKey);
            var query = new DatafordelerGraphQLQuery("Jordstykke", "MAT_Ejerlav");
            query.AddNode("id_lokalId");
            query.AddNode("ejerlavskode");
            query.AddNode("ejerlavsnavn");
            query.AddGeometriNode();
            query.AddArgument("ejerlavskode", ejerlavkode);
            var ejerlavListe = client.Request<Models.datafordeler.Ejerlav>(query);

            if (ejerlavListe == null || !ejerlavListe.Any())
                return null;

            var searchejerlav = ejerlavListe.FirstOrDefault();
            client = new DatafordelerGraphQLClient("MAT", "v2", datafordelerApiKey);
            query = new DatafordelerGraphQLQuery("Jordstykke", "MAT_Jordstykke");
            query.AddNode("matrikelnummer");
            query.AddNode("id_lokalId");
            query.AddNode("ejerlavLokalId");
            query.AddNode("samletFastEjendomLokalId");
            query.AddArgument("ejerlavLokalId", searchejerlav.id_lokalId);
            query.AddArgument("matrikelnummer", matrikelnr);
            var jordstykke = client.RequestFirst<Models.datafordeler.Jordstykke>(query);

            if (jordstykke == null)
                return null;

            return jordstykke.samletFastEjendomLokalId;
        }





    }
}
