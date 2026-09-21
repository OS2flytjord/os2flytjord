using Niras.Jordflytning.Core.Tools.Datafordeler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Niras.Jordflytning.Infrastructure.Common.Datafordeler
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

        protected void addNodeRecursive(List<string> nodeNames, List<DatafordelerGraphQLQueryNode> nodes)
        {
            var childNode = nodes.FirstOrDefault(n => n.Name == nodeNames[0]);
            if (childNode == null)
            {
                childNode = new DatafordelerGraphQLQueryNode(nodeNames[0]);
                nodes.Add(childNode);
            }

            if (nodeNames.Count > 1)
            {
                nodeNames.RemoveAt(0);
                this.addNodeRecursive(nodeNames, childNode.Children);
            }
        }

        public void AddNode(string nodePath)
        {
            var nodeNames = nodePath.Split('.').ToList();
            this.addNodeRecursive(nodeNames, this.Nodes);
        }

        public void AddGeometriNode()
        {
            this.AddNode("geometri.type");
            this.AddNode("geometri.wkt");
            this.AddNode("geometri.dimension");
            this.AddNode("geometri.crs");
        }

        public void AddArgument<T>(string name, string op, T value)
        {
            string arg = "";
            if (typeof(T) == typeof(string))
            {
                arg = $"\"{value}\"";
            }
            else if (typeof(T) == typeof(Guid))
            {
                arg = $"\"{value.ToString()}\"";
            }
            else if (value.ToString() != null)
            {
                arg = value.ToString();
            }

            this.Arguments.Add(new Tuple<string, string, string>(name, op, arg));
        }

        public void AddArgument<T>(string name, T value)
        {
            this.AddArgument(name, "eq", value);
        }

        public void AddArgument<T>(string name, List<T> values)
        {
            List<string> args = new List<string>();
            if (typeof(T) == typeof(string))
            {
                args = values.Select(v => $"\"{v}\"").ToList();
            }
            else if (typeof(T) == typeof(Guid))
            {
                args = values.Select(v => $"\"{v.ToString()}\"").ToList();
            }
            else
            {
                args = values.Select(v => v.ToString()).ToList();
            }

            this.Arguments.Add(new Tuple<string, string, string>(name, "in", $"[{string.Join(",", args)}]"));
        }

        public StringContent Body
        {
            get
            {
                var query = new StringBuilder();
                query.AppendLine($"query {this.QueryName} {{");

                query.Append($"{this.QueryType}(virkningstid: \"{DateTime.UtcNow.ToString("o")}\", registreringstid: \"{DateTime.UtcNow.ToString("o")}\"");

                if (this.Arguments.Count > 0)
                {
                    query.AppendLine(", where: {");

                    foreach (var arg in this.Arguments)
                    {
                        query.AppendLine($"{arg.Item1}: {{ {arg.Item2}: {arg.Item3} }}");
                    }

                    query.AppendLine("}");
                }


                query.AppendLine(") {");

                query.AppendLine("nodes {");

                foreach (var node in this.Nodes)
                {
                    query.AppendLine(node.ToQueryString());
                }


                query.AppendLine("}");

                query.AppendLine("}");

                query.AppendLine("}");

                System.Diagnostics.Debug.WriteLine(query);

                var json = System.Text.Json.JsonSerializer.Serialize(new
                {
                    query = query.ToString()
                });
                return new StringContent(json, Encoding.UTF8,  "application/json");
            }
        }
    }
}
