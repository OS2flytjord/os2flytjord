using Newtonsoft.Json;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models.datafordeler;
using Niras.Jordflytning.Core.Models.MatrikelOpslag;
using Niras.Jordflytning.Core.Tools.Datafordeler;
using Niras.Jordflytning.Infrastructure.Common.Datafordeler;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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


            //var urlBuilder = new StringBuilder();

            //urlBuilder.Append("http://dawa.aws.dk/ejerlav/autocomplete?q=");
            //// Hvis der er mere end et ord, søges der uden sidste (Kan være matrikel)
            //var qParts = q.Trim().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            //if (qParts.Length > 1)
            //    urlBuilder.Append(string.Join(" ", qParts.Take(qParts.Length - 1)));
            //else
            //    urlBuilder.Append(qParts[0]);

            //var request = WebRequest.Create(urlBuilder.ToString());
            //request.Method = "GET";
            //request.ContentType = "application/json; charset=utf-8";
            //string responseContent = "";
            //using (var response = request.GetResponse())
            //{
            //    using (var stream = response.GetResponseStream())
            //    {
            //        if (stream != null)
            //        {
            //            using (var reader = new StreamReader(stream))
            //            {
            //                responseContent = reader.ReadToEnd();
            //            }
            //        }
            //    }
            //}
            //return JsonConvert.DeserializeObject<DawaClasses.EjerlavInfo[]>(responseContent);
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






            //// Findes ejerlav allerede i søgestrengen?
            //// Hvis det gør, fjernes dette navn fra den videre søgning.
            //DawaClasses.EjerlavInfo ejerlav = null;
            //foreach (var entry in ejerlavListe)
            //{
            //    if (q.StartsWith(entry.ejerlav.navn, StringComparison.InvariantCultureIgnoreCase))
            //    {
            //        q = new string(q.Skip(entry.ejerlav.navn.Length).ToArray());
            //        ejerlav = entry;
            //        break;
            //    }
            //}

            //var qParts = q.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            //var urlBuilder = new StringBuilder();
            //urlBuilder.Append("https://dawa.aws.dk/jordstykker?");

            //if (ejerlav != null)
            //{
            //    urlBuilder.AppendFormat("ejerlavkode={0}", ejerlav.ejerlav.kode);
            //    if (qParts.Length > 0)
            //        urlBuilder.AppendFormat("&matrikelnr={0}", qParts.Last());
            //}
            //else if (ejerlavListe.Length > 0)
            //{
            //    // Der er ikke valgt et bestemt ejerlav, søg blandt dem i listen
            //    urlBuilder.AppendFormat("ejerlavkode={0}", ejerlavListe[0].ejerlav.kode);
            //    for (var i = 1; i < ejerlavListe.Length; i++)
            //        urlBuilder.AppendFormat("|{0}", ejerlavListe[i].ejerlav.kode);
            //    // Der findes ejerlav, skip til sidste parameter, hvis aktuelt
            //    if (qParts.Length > 1)
            //        urlBuilder.AppendFormat("&matrikelnr={0}", qParts.Last());
            //}
            //else
            //    urlBuilder.AppendFormat("matrikelnr={0}", q); // Direkte input

            //var request = WebRequest.Create(urlBuilder.ToString());
            //request.Method = "GET";
            //request.ContentType = "application/json; charset=utf-8";
            //string responseContent = "";
            //using (var response = request.GetResponse())
            //{
            //    using (var stream = response.GetResponseStream())
            //    {
            //        if (stream != null)
            //        {
            //            using (var reader = new StreamReader(stream))
            //            {
            //                responseContent = reader.ReadToEnd();
            //            }
            //        }
            //    }
            //}
            //var result = JsonConvert.DeserializeObject<DawaClasses.Jordstykke[]>(responseContent);
            //// De returnerer desværre ikke ejerlav navne, så dette er manuelt 
            //foreach (var item in result)
            //{
            //    var el = ejerlavListe.FirstOrDefault(e => e.ejerlav.kode == item.ejerlav.kode);
            //    if (el != null)
            //        item.ejerlav.navn = el.ejerlav.navn;
            //}
            //return result;
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


        //public DawaClasses.AdresseMini ReverseGeokodning(string x, string y)
        //{
        //    if (string.IsNullOrWhiteSpace(x) || string.IsNullOrWhiteSpace(y))
        //        return new DawaClasses.AdresseMini();

        //    var url = string.Format("https://dawa.aws.dk/adgangsadresser/reverse?x={0}&y={1}&struktur=mini&srid=25832", x, y);
        //    var request = WebRequest.Create(url);
        //    request.Method = "GET";
        //    request.ContentType = "application/json; charset=utf-8";
        //    string responseContent = dawa.
        //    using (var response = request.GetResponse())
        //    {
        //        using (var stream = response.GetResponseStream())
        //        {
        //            if (stream != null)
        //            {
        //                using (var reader = new StreamReader(stream))
        //                {
        //                    responseContent = reader.ReadToEnd();
        //                }
        //            }
        //        }
        //    }
        //    return JsonConvert.DeserializeObject<DawaClasses.AdresseMini>(responseContent);
        //}
    }
}