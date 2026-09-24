
using DocumentFormat.OpenXml.Bibliography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Niras.Jordflytning.Core.Tools.Datafordeler
{
    public class DatafordelerGraphQLQuery
    {
        public string QueryName { get; set; }
        public string QueryType { get; set; }
        protected List<Tuple<string, string, string>> Arguments;
        protected List<DatafordelerGraphQLQueryNode> Nodes { get; set; }


        public DatafordelerGraphQLQuery(string queryName, string queryType)
        {
            this.QueryName = queryName;
            this.QueryType = queryType;
            this.Arguments = new List<Tuple<string, string, string>>();
            this.Nodes = new List<DatafordelerGraphQLQueryNode>();
        }

        protected void addNodeRecursive(List<string> names, List<DatafordelerGraphQLQueryNode> nodes)
        {
            var node = nodes.FirstOrDefault(n => n.Name == names[0]);
            if (node == null)
            {
                node = new DatafordelerGraphQLQueryNode(names[0]);
                nodes.Add(node);
            }
            if (names.Count > 1)
                addNodeRecursive(names.Skip(1).ToList(), node.Children);
        }

        public void AddNode(string nodePath)
        {
            if (string.IsNullOrWhiteSpace(nodePath))
                throw new ArgumentException("Node path is required.", nameof(nodePath));
            var names = nodePath.Split('.').ToList();
            foreach (var name in names) ValidateName(name);
            addNodeRecursive(names, Nodes);
        }

        public void AddGeometriNode()
        {
            AddNode("geometri.type");
            AddNode("geometri.wkt");
            AddNode("geometri.dimension");
            AddNode("geometri.crs");
        }

        public void AddArgument<T>(string name, string op, T value)
        {
            ValidateName(name);
            ValidateName(op);
            var literal = ToGraphQL(value == null ? JValue.CreateNull() : JToken.FromObject(value));
            // Merge different operators for the same field when rendering.
            Arguments.RemoveAll(a => a.Item1 == name && a.Item2 == op);
            Arguments.Add(Tuple.Create(name, op, literal));
        }

        public void AddArgument<T>(string name, T value) { AddArgument(name, "eq", value); }
        public void AddArgument<T>(string name, List<T> values) { AddArgument(name, "in", values); }

        public void AddIntersectsArgument(string wkt, int crs)
        {
            AddArgument("geometri", "intersects", new { crs, wkt });
        }

        // JSON strings/lists/scalars are valid GraphQL literals, but input-object
        // field names must be unquoted. Never interpolate user strings directly.
        private static string ToGraphQL(JToken token)
        {
            var obj = token as JObject;
            if (obj != null)
                return "{ " + string.Join(", ", obj.Properties().Select(p =>
                {
                    ValidateName(p.Name);
                    return p.Name + ": " + ToGraphQL(p.Value);
                })) + " }";
            var array = token as JArray;
            if (array != null) return "[" + string.Join(", ", array.Select(ToGraphQL)) + "]";
            return token.ToString(Formatting.None);
        }

        private static void ValidateName(string name)
        {
            if (name == null || !Regex.IsMatch(name, @"\A[_A-Za-z][_0-9A-Za-z]*\z"))
                throw new ArgumentException("Invalid GraphQL name.", nameof(name));
        }

        public string ToQueryString() { return BuildQuery(); }

        // The client supplies its own cursor without mutating the caller's query.
        internal string BuildQuery()
        {
            ValidateName(QueryName);
            ValidateName(QueryType);            
            var query = new StringBuilder();
            query.Append("query ").Append(QueryName).Append(" { ").Append(QueryType);
            query.Append($"(virkningstid: \"{DateTime.UtcNow.ToString("o")}\", registreringstid: \"{DateTime.UtcNow.ToString("o")}\"");
            if (Arguments.Count > 0)
            {
                query.Append(", where: { ");
                foreach (var group in Arguments.GroupBy(a => a.Item1))
                {
                    query.Append(group.Key).Append(": { ");
                    foreach (var arg in group)
                        query.Append(arg.Item2).Append(": ").Append(arg.Item3).Append(' ');
                    query.Append("} ");
                }
                query.Append('}');
            }
            query.AppendLine(") { nodes {");
            foreach (var node in Nodes) query.AppendLine(node.ToQueryString());
            query.AppendLine("} } }");

            System.Diagnostics.Debug.WriteLine(query);

            return query.ToString();
        }

        public StringContent Body { get { return CreateBody(); } }
        internal StringContent CreateBody()
        {
            return new StringContent(JsonConvert.SerializeObject(new { query = BuildQuery() }),
                Encoding.UTF8, "application/json");
        }
    }
}
