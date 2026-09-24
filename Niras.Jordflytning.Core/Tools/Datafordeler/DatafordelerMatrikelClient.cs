using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.MatrikelOpslag;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Spatial;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Niras.Jordflytning.Core.Tools.Datafordeler
{
    /// <summary>
    /// Henter gældende matrikler fra Datafordelerens MAT GraphQL-tjeneste.
    /// Inputgeometrien skal være et gyldigt WKT-punkt eller en WKT-polygon i EPSG:25832.
    /// </summary>
    public sealed class DatafordelerMatrikelClient
    {
        private const int Srid = 25832;
        private const int IdBatchSize = 100; // Maksimum i MAT-skemaets "in"-filter.

        private readonly string _apiKey;

        public DatafordelerMatrikelClient(string apiKey, HttpClient httpClient = null)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("En Datafordeler API-key er påkrævet.", nameof(apiKey));

            _apiKey = apiKey;
        }

        public List<Matrikel> HentMatrikler(
            string wkt,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return HentMatriklerInterntAsync(wkt, cancellationToken)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Henter én gældende matrikel ud fra matrikelnummer og ejerlavskode.
        /// Returnerer null, hvis matriklen ikke findes.
        /// </summary>
        public Matrikel HentMatrikel(
            string matrikelnummer,
            long ejerlavskode,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return HentMatrikelInterntAsync(
                    matrikelnummer, ejerlavskode, cancellationToken)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        private async Task<Matrikel> HentMatrikelInterntAsync(
            string matrikelnummer,
            long ejerlavskode,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(matrikelnummer))
                throw new ArgumentException("Matrikelnummer må ikke være tomt.", nameof(matrikelnummer));
            if (ejerlavskode <= 0)
                throw new ArgumentOutOfRangeException(nameof(ejerlavskode));

            var tidspunkt = DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture);
            var ejerlav = await HentEjerlavVedKodeAsync(
                    ejerlavskode, cancellationToken)
                .ConfigureAwait(false);

            if (ejerlav == null)
                return null;

            var jordstykke = await HentJordstykkeVedMatrikelnummerAsync(
                    matrikelnummer.Trim(), ejerlav.id_lokalId, cancellationToken)
                .ConfigureAwait(false);

            if (jordstykke == null)
                return null;

            var lodfladerTask = HentLodfladerForJordstykkeAsync(
                jordstykke.id_lokalId, cancellationToken);
            var sogneTask = string.IsNullOrWhiteSpace(jordstykke.sognLokalId)
                ? Task.FromResult(new Dictionary<string, Models.datafordeler.Sogn>(StringComparer.Ordinal))
                : HentSogneAsync(
                    new[] { jordstykke.sognLokalId }, cancellationToken);

            await Task.WhenAll(lodfladerTask, sogneTask).ConfigureAwait(false);

            Models.datafordeler.Sogn sogn;
            sogneTask.Result.TryGetValue(jordstykke.sognLokalId ?? string.Empty, out sogn);
            DbGeometry geometri;
            SamlGeometrier(lodfladerTask.Result).TryGetValue(jordstykke.id_lokalId, out geometri);

            return new Matrikel
            {
                Id = DeterministiskGuid(jordstykke.id_lokalId),
                OprindelsesstedId = null,
                Matrikelnr = jordstykke.matrikelnummer,
                Ejerlav = ejerlav.ejerlavskode.ToString(CultureInfo.InvariantCulture),
                Sogn = sogn == null ? jordstykke.sognLokalId : sogn.sognenavn,
                Herred = null,
                //Dato = jordstykke.VirkningFra,
                Geom = geometri,
                Ejerlavsnavn = ejerlav.ejerlavsnavn
            };
        }

        private async Task<List<Matrikel>> HentMatriklerInterntAsync(
            string wkt,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(wkt))
                throw new ArgumentException("WKT må ikke være tom.", nameof(wkt));

            var upperWkt = wkt.TrimStart().ToUpperInvariant();
            if (!upperWkt.StartsWith("POINT", StringComparison.Ordinal) &&
                !upperWkt.StartsWith("POLYGON", StringComparison.Ordinal) &&
                !upperWkt.StartsWith("MULTIPOLYGON", StringComparison.Ordinal))
            {
                throw new ArgumentException(
                    "WKT skal være et POINT, en POLYGON eller en MULTIPOLYGON i EPSG:25832.",
                    nameof(wkt));
            }
                        
            var lodflader = await HentLodfladerAsync(wkt, cancellationToken)
                .ConfigureAwait(false);

            var jordstykkeIds = lodflader
                .Select(x => x.jordstykkeLokalId)
                .Distinct(StringComparer.Ordinal)
                .ToArray();

            if (jordstykkeIds.Length == 0)
                return new List<Matrikel>();

            var jordstykker = await HentJordstykkerAsync(
                    jordstykkeIds, cancellationToken)
                .ConfigureAwait(false);

            var ejerlavIds = jordstykker
                .Select(x => x.ejerlavLokalId)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.Ordinal)
                .ToArray();

            var sognIds = jordstykker
                .Select(x => x.sognLokalId)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.Ordinal)
                .ToArray();

            var ejerlavTask = HentEjerlavAsync(ejerlavIds, cancellationToken);
            var sogneTask = HentSogneAsync(sognIds, cancellationToken);
            await Task.WhenAll(ejerlavTask, sogneTask).ConfigureAwait(false);

            var ejerlavById = ejerlavTask.Result;
            var sogneById = sogneTask.Result;
            var geometriByJordstykke = SamlGeometrier(lodflader);

            var resultat = new List<Matrikel>(jordstykker.Count);
            foreach (var jordstykke in jordstykker)
            {
                Models.datafordeler.Ejerlav ejerlav;
                Models.datafordeler.Sogn sogn;
                DbGeometry geometri;

                ejerlavById.TryGetValue(jordstykke.ejerlavLokalId ?? string.Empty, out ejerlav);
                sogneById.TryGetValue(jordstykke.sognLokalId ?? string.Empty, out sogn);
                geometriByJordstykke.TryGetValue(jordstykke.id_lokalId, out geometri);

                resultat.Add(new Matrikel
                {
                    Id = DeterministiskGuid(jordstykke.id_lokalId),
                    OprindelsesstedId = null,
                    Matrikelnr = jordstykke.matrikelnummer,
                    Ejerlav = ejerlav == null
                        ? jordstykke.ejerlavLokalId
                        : ejerlav.ejerlavskode.ToString(CultureInfo.InvariantCulture),
                    Sogn = sogn == null ? jordstykke.sognLokalId : sogn.sognenavn,
                    Herred = null, // Herred udstilles ikke i MAT GraphQL.
                    //Dato = jordstykke.VirkningFra,
                    Geom = geometri,
                    Ejerlavsnavn = ejerlav == null ? null : ejerlav.ejerlavsnavn
                });
            }

            return resultat;
        }

        private async Task<Models.datafordeler.Ejerlav> HentEjerlavVedKodeAsync(
            long ejerlavskode,
            CancellationToken cancellationToken)
        {
            var client = new DatafordelerGraphQLClient("MAT", "v2", _apiKey);
            var query = new DatafordelerGraphQLQuery("Jordstykke", "MAT_Ejerlav");
            query.AddNode("id_lokalId");
            query.AddNode("ejerlavskode");
            query.AddNode("ejerlavsnavn");
            query.AddGeometriNode();
            query.AddArgument("ejerlavskode", ejerlavskode);
            var ejerlav = client.RequestFirst<Models.datafordeler.Ejerlav>(query);

            if (ejerlav == null)
                return null;

            return ejerlav;
        }

        private async Task<Models.datafordeler.Jordstykke> HentJordstykkeVedMatrikelnummerAsync(
            string matrikelnummer,
            string ejerlavId,
            CancellationToken cancellationToken)
        {
            var client = new DatafordelerGraphQLClient("MAT", "v2", _apiKey);
            var query = new DatafordelerGraphQLQuery("Jordstykke", "MAT_Jordstykke");
            query.AddNode("id_lokalId");
            query.AddNode("matrikelnummer");            
            query.AddNode("ejerlavLokalId");
            query.AddNode("sognLokalId");
            query.AddArgument("ejerlavLokalId", ejerlavId);
            query.AddArgument("matrikelnummer", matrikelnummer);
            var jordstykke = client.RequestFirst<Models.datafordeler.Jordstykke>(query);

            if (jordstykke == null)
                return null;
            return jordstykke;
        }

        private async Task<List<Models.datafordeler.LoadFlade>> HentLodfladerForJordstykkeAsync(
            string jordstykkeId,
            CancellationToken cancellationToken)
        {
            var client = new DatafordelerGraphQLClient("MAT", "v2", _apiKey);
            var query = new DatafordelerGraphQLQuery("Jordstykke", "MAT_Lodflade");
            query.AddNode("jordstykkeLokalId");
            query.AddGeometriNode();            
            query.AddArgument("jordstykkeLokalId", jordstykkeId);
            var flader = client.Request<Models.datafordeler.LoadFlade>(query);

            if (flader == null)
                return null;
            return flader;
 
        }

        private async Task<List<Models.datafordeler.LoadFlade>> HentLodfladerAsync(
            string wkt,
            CancellationToken cancellationToken)
        {
            var client = new DatafordelerGraphQLClient("MAT", "v2", _apiKey);
            var query = new DatafordelerGraphQLQuery("Jordstykke", "MAT_Lodflade");
            query.AddNode("jordstykkeLokalId");
            query.AddGeometriNode();
            query.AddIntersectsArgument(wkt, 25832);
            var flader = client.Request<Models.datafordeler.LoadFlade>(query);

            if (flader == null)
                return null;
            return flader;
        }

        private async Task<List<Models.datafordeler.Jordstykke>> HentJordstykkerAsync(
            IReadOnlyCollection<string> ids,
            CancellationToken cancellationToken)
        {
            var client = new DatafordelerGraphQLClient("MAT", "v2", _apiKey);
            var query = new DatafordelerGraphQLQuery("Jordstykke", "MAT_Jordstykke");
            query.AddNode("id_lokalId");
            query.AddNode("matrikelnummer");
            query.AddNode("ejerlavLokalId");
            query.AddNode("sognLokalId");            
            query.AddArgument("id_lokalId", "in", ids);
            var jordstykke = client.Request<Models.datafordeler.Jordstykke>(query);

            if ((jordstykke == null) || !jordstykke.Any())
                return null;
            return jordstykke;
                       
        }

        private async Task<Dictionary<string, Models.datafordeler.Ejerlav>> HentEjerlavAsync(
            IReadOnlyCollection<string> ids,
            CancellationToken cancellationToken)
        {
            var client = new DatafordelerGraphQLClient("MAT", "v2", _apiKey);
            var query = new DatafordelerGraphQLQuery("Jordstykke", "MAT_Ejerlav");
            query.AddNode("id_lokalId");
            query.AddNode("ejerlavskode");
            query.AddNode("ejerlavsnavn");
            query.AddGeometriNode();
            query.AddArgument("id_lokalId", "in", ids);
            var ejerlavs = client.Request<Models.datafordeler.Ejerlav>(query);

            if ((ejerlavs == null) || !ejerlavs.Any())
                return null;

            var resultat = new Dictionary<string, Models.datafordeler.Ejerlav>(StringComparer.Ordinal); 
            foreach (var e in ejerlavs)
                resultat[e.id_lokalId] = e;

            return resultat;
        }

        private async Task<Dictionary<string, Models.datafordeler.Sogn>> HentSogneAsync(
            IReadOnlyCollection<string> ids,
            CancellationToken cancellationToken)
        {
            var client = new DatafordelerGraphQLClient("MAT", "v2", _apiKey);
            var query = new DatafordelerGraphQLQuery("Jordstykke", "MAT_MatrikelSogn");
            query.AddNode("id_lokalId");
            query.AddNode("sognekode");
            query.AddNode("sognenavn");
            query.AddGeometriNode();
            query.AddArgument("sognekode", "in", ids);
            var sogne = client.Request<Models.datafordeler.Sogn>(query);

            if ((sogne == null) || !sogne.Any())
                return null;

            var resultat = new Dictionary<string, Models.datafordeler.Sogn>(StringComparer.Ordinal);
            foreach (var s in sogne)
                resultat[s.id_lokalId] = s;
            return resultat;
        }
        
        private static Dictionary<string, DbGeometry> SamlGeometrier(
            IEnumerable<Models.datafordeler.LoadFlade> lodflader)
        {
            var resultat = new Dictionary<string, DbGeometry>(StringComparer.Ordinal);
            foreach (var lodflade in lodflader)
            {
                if (string.IsNullOrWhiteSpace(lodflade.jordstykkeLokalId) ||
                    string.IsNullOrWhiteSpace(lodflade.geometri?.wkt))
                    continue;

                var geometri = DbGeometry.FromText(lodflade.geometri.wkt, lodflade.geometri.crs);
                DbGeometry eksisterende;
                resultat[lodflade.jordstykkeLokalId] =
                    resultat.TryGetValue(lodflade.jordstykkeLokalId, out eksisterende)
                        ? eksisterende.Union(geometri)
                        : geometri;
            }

            return resultat;
        }
               

        private static Guid DeterministiskGuid(string value)
        {
            Guid guid;
            if (Guid.TryParse(value, out guid))
                return guid;

            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(value ?? string.Empty));
                var bytes = new byte[16];
                Buffer.BlockCopy(hash, 0, bytes, 0, bytes.Length);
                return new Guid(bytes);
            }
        }
               
    }

    public sealed class DatafordelerGraphQlException : Exception
    {
        public DatafordelerGraphQlException(string message) : base(message)
        {
        }
    }
}

