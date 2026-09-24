using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.Models.datafordeler;
using Niras.Jordflytning.Core.Models.MatrikelOpslag;
using Niras.Jordflytning.Core.Tools.Datafordeler;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using static Niras.Jordflytning.Core.Models.MatrikelOpslag.DawaClasses;

namespace Niras.Jordflytning.Core.BusinessLogic
{
    public class DawaOpslagBusiness : IDawaOpslagBusiness
    {

        public DawaClasses.EjerlavInfo[] EjerlavSoegning(string q)
        {
            if (string.IsNullOrEmpty(q))
                return new DawaClasses.EjerlavInfo[] { };

            if (!int.TryParse(q, out int ejerlavskode))
                return new DawaClasses.EjerlavInfo[] { };

            var datafordelerApiKey = ConfigurationManager.AppSettings["datafordelerApiKey"];
            var client = new DatafordelerGraphQLClient("MAT", "v2", datafordelerApiKey);
            var query = new DatafordelerGraphQLQuery("Jordstykke", "MAT_Ejerlav");
            query.AddNode("id_lokalId");
            query.AddNode("ejerlavskode");
            query.AddNode("ejerlavsnavn");
            query.AddGeometriNode();
            query.AddArgument("ejerlavskode", ejerlavskode);
            var ejerlavListe = client.Request<Models.datafordeler.Ejerlav>(query);

            var infoList = new List<EjerlavInfo>();
            foreach (var e in ejerlavListe)
            {
                infoList.Add(new EjerlavInfo
                {
                    tekst = e.ejerlavsnavn,
                    ejerlav = new DawaClasses.Ejerlav
                    {
                        lokalid = e.id_lokalId,
                        navn = e.ejerlavsnavn,
                        kode = e.ejerlavskode
                    }
                });
            }

            return infoList.ToArray();
        }

        public NavngivenVej SearchAllVeje(string lokalId)
        {
            if (string.IsNullOrEmpty(lokalId.ToString()))
                return null;


            var datafordelerApiKey = ConfigurationManager.AppSettings["datafordelerApiKey"];
            var client = new DatafordelerGraphQLClient("DAR", "v3", datafordelerApiKey);
            var query = new DatafordelerGraphQLQuery("NavngivenVej", "DAR_NavngivenVejPostnummer");
            query.AddNode("navngivenVej");
            query.AddArgument("id_lokalId", lokalId);

            var navngivenVejPostnummerList = client.Request<Models.datafordeler.NavngivenVejPosternummer>(query);

            if (navngivenVejPostnummerList.Count() == 1)
            {
                var query2 = new DatafordelerGraphQLQuery("NavngivenVej", "DAR_NavngivenVej");
                query2.AddArgument("id_lokalId", navngivenVejPostnummerList[0].navngivenVej);
                query2.AddNode("administreresAfKommune");
                query2.AddNode("vejnavnebeliggenhed_vejnavnelinje.wkt");

                var navngivenVejList = client.Request<Models.datafordeler.NavngivenVej>(query2);
                if (navngivenVejList.Count() == 1)
                {
                    return navngivenVejList[0];
                }
            }

            return null;
        }

        public DawaClasses.Jordstykke[] JordstykkeOpslag(string q, Models.MatrikelOpslag.DawaClasses.EjerlavInfo[] ejerlavListe = null)
        {
            if (string.IsNullOrEmpty(q))
                return new DawaClasses.Jordstykke[] { };

            if (ejerlavListe == null)
                ejerlavListe = EjerlavSoegning(q);

            q = q.Trim();

            var datafordelerApiKey = ConfigurationManager.AppSettings["datafordelerApiKey"];
            var client = new DatafordelerGraphQLClient("MAT", "v2", datafordelerApiKey);
            var query = new DatafordelerGraphQLQuery("Jordstykke", "MAT_Jordstykke");
            query.AddNode("matrikelnummer");
            query.AddNode("id_lokalId");
            query.AddNode("ejerlavLokalId");


            if (ejerlavListe.Any() && ejerlavListe.FirstOrDefault() != null)
            {//If we got a ejerlav, we can use the GraphQL API to get the jordstykke list instead of the REST API. This is because the REST API does not return the ejerlav name, which is needed for the UI.
                var searchejerlav = ejerlavListe.FirstOrDefault().ejerlav;

                query.AddArgument("ejerlavLokalId", searchejerlav.lokalid);
                var jordstykkelist = client.Request<Models.datafordeler.Jordstykke>(query);
                var jordstykkeList = new List<DawaClasses.Jordstykke>();
                foreach (var j in jordstykkelist)
                {
                    jordstykkeList.Add(new DawaClasses.Jordstykke
                    {
                        matrikelnr = j.matrikelnummer,
                        ejerlav = searchejerlav
                    });
                }

                return jordstykkeList.ToArray();
            } 
            else
            {//The q value is not a ejerlav now we try a matrikkelsearch                

                query.AddArgument("matrikelnummer", q);                
                var jordstykkelist = client.Request<Models.datafordeler.Jordstykke>(query);

                var ejerlavIds = jordstykkelist
                                .Select(x => x.ejerlavLokalId)
                                .Where(x => !string.IsNullOrWhiteSpace(x))
                                .Distinct()
                                .ToList();
                query = new DatafordelerGraphQLQuery("Jordstykke", "MAT_Ejerlav");
                query.AddNode("id_lokalId");
                query.AddNode("ejerlavskode");
                query.AddNode("ejerlavsnavn");
                query.AddGeometriNode(); 
                query.AddArgument("id_lokalId", ejerlavIds);
                var searchejerlavListe = client.Request<Models.datafordeler.Ejerlav>(query);

                var jordstykkeList = new List<DawaClasses.Jordstykke>();
                foreach (var j in jordstykkelist)
                {
                    var el = searchejerlavListe.FirstOrDefault(e => e.id_lokalId == j.ejerlavLokalId);

                    jordstykkeList.Add(new DawaClasses.Jordstykke
                    {
                        matrikelnr = j.matrikelnummer,
                        ejerlav = new DawaClasses.Ejerlav
                        {
                            lokalid = el.id_lokalId,
                            kode = el.ejerlavskode,
                            navn = el.ejerlavsnavn
                        }
                    });
                }

                return jordstykkeList.ToArray();
            }
        }


        public MatrikelGeometri GetMatrikel(string ejerlavkode, string matrikelnummer)
        {
            if (!int.TryParse(ejerlavkode, out int ejerlavskode))
                return new MatrikelGeometri(); 

            var datafordelerApiKey = ConfigurationManager.AppSettings["datafordelerApiKey"];
            var client = new DatafordelerGraphQLClient("MAT", "v2", datafordelerApiKey);
            var query = new DatafordelerGraphQLQuery("Jordstykke", "MAT_Ejerlav");
            query.AddNode("id_lokalId");
            query.AddNode("ejerlavskode");
            query.AddNode("ejerlavsnavn");
            query.AddGeometriNode();
            query.AddArgument("ejerlavskode", ejerlavskode);
            var ejerlavListe = client.Request<Models.datafordeler.Ejerlav>(query);

            if (ejerlavListe == null || !ejerlavListe.Any())
                return new MatrikelGeometri();

            var searchejerlav = ejerlavListe.FirstOrDefault();
            client = new DatafordelerGraphQLClient("MAT", "v2", datafordelerApiKey);
            query = new DatafordelerGraphQLQuery("Jordstykke", "MAT_Jordstykke");
            query.AddNode("matrikelnummer");
            query.AddNode("id_lokalId");
            query.AddNode("ejerlavLokalId");
            query.AddArgument("ejerlavLokalId", searchejerlav.id_lokalId);
            query.AddArgument("matrikelnummer", matrikelnummer);
            var jordstykkelist = client.Request<Models.datafordeler.Jordstykke>(query);

            if (jordstykkelist == null || !jordstykkelist.Any())
                return new MatrikelGeometri();

            var jordstykke = jordstykkelist.FirstOrDefault();
            client = new DatafordelerGraphQLClient("MAT", "v2", datafordelerApiKey);
            query = new DatafordelerGraphQLQuery("MatrikkelGeometri", "MAT_Lodflade");
            query.AddNode("id_lokalId");
            query.AddNode("jordstykkeLokalId");
            query.AddGeometriNode();
            query.AddArgument("jordstykkeLokalId", jordstykke.id_lokalId);
            var matrikkelGeometrilist = client.Request<Models.datafordeler.MatrikelGeometri>(query);

            if (matrikkelGeometrilist == null || !matrikkelGeometrilist.Any())
                return new MatrikelGeometri();

            var res = matrikkelGeometrilist.FirstOrDefault();
            res.geometri = DatafordelerGeometri.ExtendGeometriData(res.geometri);
            return res;
        }
    }
}