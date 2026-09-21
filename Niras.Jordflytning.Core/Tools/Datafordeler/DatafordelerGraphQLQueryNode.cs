using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niras.Jordflytning.Core.Tools.Datafordeler
{
    public class DatafordelerGraphQLQueryNode
    {
        public string Name { get; set; }
        public List<DatafordelerGraphQLQueryNode> Children { get; set; }

        public DatafordelerGraphQLQueryNode(string Name)
        {
            this.Name = Name;
            this.Children = new List<DatafordelerGraphQLQueryNode>();
        }

        public string ToQueryString()
        {
            var query = new StringBuilder();
            if (this.Children.Count > 0)
            {
                query.AppendLine($"{this.Name} {{");
                foreach (var c in this.Children)
                {
                    query.AppendLine(c.ToQueryString());
                }
                query.Append("}");
            }
            else
            {
                query.Append(this.Name);
            }

            return query.ToString();
        }
    }

}
