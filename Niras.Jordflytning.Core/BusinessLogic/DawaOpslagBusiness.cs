using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.Models.MatrikelOpslag;

namespace Niras.Jordflytning.Core.BusinessLogic
{
    public class DawaOpslagBusiness : IDawaOpslagBusiness
    {

        public Models.MatrikelOpslag.DawaClasses.EjerlavInfo[] EjerlavSoegning(string q)
        {
            if (string.IsNullOrEmpty(q))
                return new DawaClasses.EjerlavInfo[] { };

            var urlBuilder = new StringBuilder();

            urlBuilder.Append("http://dawa.aws.dk/ejerlav/autocomplete?q=");
            // Hvis der er mere end et ord, søges der uden sidste (Kan være matrikel)
            var qParts = q.Trim().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (qParts.Length > 1)
                urlBuilder.Append(string.Join(" ", qParts.Take(qParts.Length - 1)));
            else
                urlBuilder.Append(qParts[0]);

            var request = WebRequest.Create(urlBuilder.ToString());
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";
            string responseContent = "";
            using (var response = request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            responseContent = reader.ReadToEnd();
                        }
                    }
                }
            }
            return JsonConvert.DeserializeObject<DawaClasses.EjerlavInfo[]>(responseContent);
        }

        public Models.MatrikelOpslag.DawaClasses.Jordstykke[] JordstykkeOpslag(string q, Models.MatrikelOpslag.DawaClasses.EjerlavInfo[] ejerlavListe = null)
        {
            if (string.IsNullOrEmpty(q))
                return new DawaClasses.Jordstykke[] { };

            if (ejerlavListe == null)
                ejerlavListe = EjerlavSoegning(q);

            q = q.Trim();

            // Findes ejerlav allerede i søgestrengen?
            // Hvis det gør, fjernes dette navn fra den videre søgning.
            DawaClasses.EjerlavInfo ejerlav = null;
            foreach (var entry in ejerlavListe)
            {
                if (q.StartsWith(entry.ejerlav.navn, StringComparison.InvariantCultureIgnoreCase))
                {
                    q = new string(q.Skip(entry.ejerlav.navn.Length).ToArray());
                    ejerlav = entry;
                    break;
                }
            }

            var qParts = q.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            var urlBuilder = new StringBuilder();
            urlBuilder.Append("https://dawa.aws.dk/jordstykker?");

            if (ejerlav != null)
            {
                urlBuilder.AppendFormat("ejerlavkode={0}", ejerlav.ejerlav.kode);
                if (qParts.Length > 0)
                    urlBuilder.AppendFormat("&matrikelnr={0}", qParts.Last());
            }
            else if (ejerlavListe.Length > 0)
            {
                // Der er ikke valgt et bestemt ejerlav, søg blandt dem i listen
                urlBuilder.AppendFormat("ejerlavkode={0}", ejerlavListe[0].ejerlav.kode);
                for (var i = 1; i < ejerlavListe.Length; i++)
                    urlBuilder.AppendFormat("|{0}", ejerlavListe[i].ejerlav.kode);
                // Der findes ejerlav, skip til sidste parameter, hvis aktuelt
                if (qParts.Length > 1)
                    urlBuilder.AppendFormat("&matrikelnr={0}", qParts.Last());
            }
            else
                urlBuilder.AppendFormat("matrikelnr={0}", q); // Direkte input

            var request = WebRequest.Create(urlBuilder.ToString());
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";
            string responseContent = "";
            using (var response = request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            responseContent = reader.ReadToEnd();
                        }
                    }
                }
            }
            var result = JsonConvert.DeserializeObject<DawaClasses.Jordstykke[]>(responseContent);
            // De returnerer desværre ikke ejerlav navne, så dette er manuelt 
            foreach (var item in result)
            {
                var el = ejerlavListe.FirstOrDefault(e => e.ejerlav.kode == item.ejerlav.kode);
                if (el != null)
                    item.ejerlav.navn = el.ejerlav.navn;
            }
            return result;
        }


        public DawaClasses.AdresseMini ReverseGeokodning(string x, string y)
        {
            if (string.IsNullOrWhiteSpace(x) || string.IsNullOrWhiteSpace(y))
                return new DawaClasses.AdresseMini();

            var url = string.Format("https://dawa.aws.dk/adgangsadresser/reverse?x={0}&y={1}&struktur=mini&srid=25832", x, y);
            var request = WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";
            string responseContent = "";
            using (var response = request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            responseContent = reader.ReadToEnd();
                        }
                    }
                }
            }
            return JsonConvert.DeserializeObject<DawaClasses.AdresseMini>(responseContent);
        }
    }
}