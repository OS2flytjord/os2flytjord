using System;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Web.UI.WebControls.Expressions;

namespace Niras.Jordflytning.BatchJob
{
    static class ConfigHelper
    {
        public static void Initialize()
        {
#if !DEBUG
            // Kopier Azure Environment variabler til den aktuelle konfiguration.
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
            {
                var name = de.Key as string;
                var value = de.Value;
                if (name != null && value != null)
                {
                    try
                    {
                        if (name.StartsWith("APPSETTING_", StringComparison.OrdinalIgnoreCase))
                        {
                            var key = name.Replace("APPSETTING_", "");
                            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
                                configuration.AppSettings.Settings[key].Value = value.ToString();
                            else
                                configuration.AppSettings.Settings.Add(key, value.ToString());
                        }
                        else if (name.StartsWith("SQLCONNSTR_", StringComparison.OrdinalIgnoreCase))
                        {
                            var key = name.Replace("SQLCONNSTR_", "");
                            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
                                configuration.ConnectionStrings.ConnectionStrings[key].ConnectionString = value.ToString();
                            else
                                configuration.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(key, value.ToString(), "System.Data.SqlClient"));
                        }
                    }
                    catch {
                        
                    }
                }
            }
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.RefreshSection("connectionStrings");
#endif
        }
    }
}
