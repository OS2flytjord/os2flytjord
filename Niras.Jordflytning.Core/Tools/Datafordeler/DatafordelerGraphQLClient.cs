using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Niras.Jordflytning.Infrastructure.Common.Datafordeler
{
    public class DatafordelerGraphQLClient
    {
        public string Register { get; set; }
        public string Version { get; set; }
        public string ApiKey { get; set; } = "IyC42RmK6HyJNw2vPbgN94aPbSFsn3pipiRBBbB2Sattc5lBxp7nezAapJS2wEr47rYiTa7C2DidziUKbzvuhxKJaqXGzb7kU";

        public DatafordelerGraphQLClient(string Register, string Version, string ApiKey)
        {
            this.Register = Register;
            this.Version = Version;
            this.ApiKey = ApiKey;
        }

        public JsonNode Request(DatafordelerGraphQLQuery query)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/graphql-response+json"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var url = $"https://graphql.datafordeler.dk/{this.Register}/{this.Version}?apikey={this.ApiKey}";

            var result = Task.Run(async () =>
            {
                var res = await client.PostAsync(url, query.Body);
                res.EnsureSuccessStatusCode();
                return await res.Content.ReadAsStringAsync();
            }).Result;

            return System.Text.Json.JsonSerializer.Deserialize<JsonNode>(result);
        }

        public List<T> Request<T>(DatafordelerGraphQLQuery query)
        {
            var response = this.Request(query);

            if (response == null)
            {
                return null;
            }

            return System.Text.Json.JsonSerializer.Deserialize<List<T>>(response["data"]?[query.QueryType]?["nodes"]?.AsArray());
        }

        public T RequestFirst<T>(DatafordelerGraphQLQuery query)
        {
            var response = this.Request<T>(query);

            if (response == null || response.Count == 0)
            {
                return default(T);
            }

            return response[0];
        }
    }
}
