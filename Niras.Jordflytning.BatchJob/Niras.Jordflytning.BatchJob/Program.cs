using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using Niras.Jordflytning.Infrastructure.DataAccess;
using Niras.Jordflytning.Core.Models.JordForurening;
using Niras.Jordflytning.Core.Models;
using System.IO;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.BatchJob
{
    class Program
    {
        private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.BatchJob");

        static void Main(string[] args)
        {
            ConfigHelper.Initialize();
            var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            SqlServerTypes.Utilities.LoadNativeAssemblies(dir.FullName);

            var jordklassifikationer = DataAccess.GetKommuneJordklassifikation();
            var resultat = new Dictionary<Guid, Guid>();

            /*
             * Item1 = Guid         - Oprindelsessted Id/AnmeldelseId
             * Item2 = string       - Ejerlav
             * Item3 = string       - Ejerlavsnavn
             * Item4 = string       - Matrikelnr
             * Item5 = string       - Navn
             * Item6 = Guid         - KommuneId
             * Item7 = int          - KommuneNr
             * REST 
             * Item1 = DbGeometry   - Geom - Oprindelsessted
             * Item2 = DbGeometry   - Geom
             * Item3 = Guid         - JordKlassifikationTypeId ORGI
             * Item4 = Guid         - OprindelsesstedKlassifikationTypeId
             */
            var reviews = DataAccess.GetReviews(Logger);

            var groups = reviews.GroupBy(r => r.Item1).ToList();

            if (groups != null && groups.Count() != 0)
            {
                foreach (var group in groups)
                {
                    var firstReview = group.First();
                    var kommuneKlassifikationer = jordklassifikationer.Where(k => k.LandsdelTypeId == firstReview.Item6).ToList();

                    var oprindelsessted = new Oprindelsessted();
                    oprindelsessted.Id = firstReview.Item1;
                    oprindelsessted.Geom = firstReview.Rest.Item1;
                    short oprindelsesstedKlassificeringTypeKode = 1;

                    if (firstReview.Rest.Item4 != Guid.Parse("295407E9-2182-4876-AE6F-50AFD00A6194")) //Off. vej
                    {
                        foreach (var review in group)
                        {
                            oprindelsessted.Matrikel.Add(new Matrikel() { Matrikelnr = review.Item4, Ejerlav = review.Item2, Ejerlavsnavn = review.Item3, Geom = review.Rest.Item2 });
                        }

                        var opslag = new ForureningsOpslag2(kommuneKlassifikationer);
                        try
                        {
                            var miljoePortalResult = CallMiljoePortal(oprindelsessted, kommuneKlassifikationer);
                            opslag.AddResultList(miljoePortalResult);
                        }
                        catch (Exception ex)
                        {
                            Log(ex);
                            continue;
                        }

                        if (firstReview.Item6 == Guid.Parse("A15D5888-BC70-4206-871E-A47A0C05DECC")) // AARHUS KOMMUNE ID!
                        {
                            try
                            {
                                var geoEnvironResult = CallGeoEnviron(oprindelsessted, kommuneKlassifikationer);
                                opslag.AddResultList(geoEnvironResult);
                            }
                            catch (Exception ex)
                            {
                                Log(ex);
                                continue;
                            }
                        }

                        var ka = opslag.GetHigestKlassifikation(firstReview.Item7, oprindelsesstedKlassificeringTypeKode);
                        if (ka != null)
                        {
                            if (firstReview.Rest.Item3 != ka.Id)
                                resultat.Add(oprindelsessted.Id, ka.Id);
                        }
                    }
                }
            }

            // UPDATE Reviews Flag "FlagJordForurening".
            if (resultat.Count != 0)
            {
                DataAccess.UpdateReviews(resultat);
            }

            //Console.WriteLine("Press <enter> to quit!");
            //Console.ReadLine();
        }

        private static List<ForureningsOpslagResult> CallMiljoePortal(Oprindelsessted oprindelsessted, List<JordKlassifikationType> jordKlassifikationTypeList)
        {
            var miljoe = new MiljoePortalRepository();
            var jordForureningsOpslag = miljoe.GetResult(oprindelsessted, jordKlassifikationTypeList);
            return jordForureningsOpslag;
        }

        private static List<ForureningsOpslagResult> CallGeoEnviron(Oprindelsessted oprindelsessted, IList<JordKlassifikationType> jordKlassifikationTypeList)
        {
            var geoEnvironRepository = new GeoEnvironRepository();

            var resultList = new List<ForureningsOpslagResult>();
            if (oprindelsessted.Matrikel != null)
            {
                foreach (var matrikel in oprindelsessted.Matrikel)
                {
                    var tmpList = geoEnvironRepository.GetResult(matrikel.Ejerlav, matrikel.Ejerlavsnavn, matrikel.Matrikelnr, jordKlassifikationTypeList);
                    if (tmpList != null && tmpList.Count > 0)
                        resultList.AddRange(tmpList);
                }
            }

            return resultList;
        }

        private static void Log(Exception ex)
        {
            Logger.LogException(ex);
            Console.WriteLine(ex.ToString());
        }
    }
}
