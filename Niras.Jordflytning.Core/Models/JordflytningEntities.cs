using System.Configuration;
using System.Data.EntityClient;
using System.Data.SqlClient;

namespace Niras.Jordflytning.Core.Models
{
    public partial class JordflytningEntities
    {
        private const string PROVIDER = "System.Data.SqlClient";
        private const string METADATA = "res://*/Models.JFModel.csdl|res://*/Models.JFModel.ssdl|res://*/Models.JFModel.msl";

        public JordflytningEntities(string connectionStringName) : base(GenerateConnectionString(connectionStringName))
        {
        }

        private static string GenerateConnectionString(string connectionStringName)
        {
            var efcs = new EntityConnectionStringBuilder()
            {
                Provider = PROVIDER,
                Metadata = METADATA,
                ProviderConnectionString = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString).ConnectionString
            };
            return efcs.ConnectionString;
        }
    }
}
