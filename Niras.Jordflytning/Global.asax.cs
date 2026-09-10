using Niras.Jordflytning.App_Start;
using Niras.Jordflytning.Library.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using WebMatrix.WebData;


namespace Niras.Jordflytning
{
    public class MvcApplication : System.Web.HttpApplication
	{
	    public static string FlytjordDomain
	    {
            get { return ConfigurationManager.AppSettings["FlytJordDomain"]; }
	    }

		protected void Application_Start()
		{
            LogManager.ReconfigExistingLoggers();
            try
            {
                if (!WebSecurity.Initialized)
                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "BrugerProfil", "BrugerId", "BrugerNavn", autoCreateTables: true);
            }
            catch (Exception ex) {
                var logger = LogManager.Instance.GetLogger();
                logger.LogException("WebSecurity initialization failed: " + ex.Message, ex);
                throw;
            }

            AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
            Niras.Jordflytning.SqlServerTypes.Utilities.LoadNativeAssemblies(Path.GetDirectoryName((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).LocalPath));
            //ViewEngineConfig.Configure();

            //var defaultDisplayMode = new DefaultDisplayMode("");		
            //defaultDisplayMode.ContextCondition = (context => IsIpad(context.GetOverriddenUserAgent()));		
            //DisplayModeProvider.Instance.Modes.Insert(0, defaultDisplayMode);

            //find the default JsonVAlueProviderFactory
            JsonValueProviderFactory jsonValueProviderFactory = null;

            foreach (var factory in ValueProviderFactories.Factories)
            {
                if (factory is JsonValueProviderFactory)
                {
                    jsonValueProviderFactory = factory as JsonValueProviderFactory;
                }
            }

            //remove the default JsonVAlueProviderFactory
            if (jsonValueProviderFactory != null) ValueProviderFactories.Factories.Remove(jsonValueProviderFactory);

            //add the custom one
            ValueProviderFactories.Factories.Add(new  CustomJsonValueProviderFactory());
		}

		private static bool IsIpad(string useragentString)
		{
			var isIpad = false;
			if (!String.IsNullOrEmpty(useragentString))
				isIpad = useragentString.IndexOf("iPad", StringComparison.OrdinalIgnoreCase) >= 0;
			return isIpad;	
		}

        /* private void Application_Error(object sender, EventArgs e)
        {
        
#if !DEBUG
            Exception ex = Server.GetLastError().GetBaseException();
            Server.ClearError();

           
                var logger = LogManager.Instance.GetLogger();
                logger.LogException("caught in global error handler:", ex);
#endif
            
    }  
        */

    }
    public sealed class CustomJsonValueProviderFactory : ValueProviderFactory
    {

        private static void AddToBackingStore(Dictionary<string, object> backingStore, string prefix, object value)
        {
            IDictionary<string, object> d = value as IDictionary<string, object>;
            if (d != null)
            {
                foreach (KeyValuePair<string, object> entry in d)
                {
                    AddToBackingStore(backingStore, MakePropertyKey(prefix, entry.Key), entry.Value);
                }
                return;
            }

            IList l = value as IList;
            if (l != null)
            {
                for (int i = 0; i < l.Count; i++)
                {
                    AddToBackingStore(backingStore, MakeArrayKey(prefix, i), l[i]);
                }
                return;
            }

            // primitive
            backingStore[prefix] = value;
        }

        private static object GetDeserializedObject(ControllerContext controllerContext)
        {
            if (!controllerContext.HttpContext.Request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
            {
                // not JSON request
                return null;
            }

            StreamReader reader = new StreamReader(controllerContext.HttpContext.Request.InputStream);
            string bodyText = reader.ReadToEnd();
            if (String.IsNullOrEmpty(bodyText))
            {
                // no JSON data
                return null;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue; //increase MaxJsonLength.  This could be read in from the web.config if you prefer
            object jsonData = serializer.DeserializeObject(bodyText);
            return jsonData;
        }

        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }

            object jsonData = GetDeserializedObject(controllerContext);
            if (jsonData == null)
            {
                return null;
            }

            Dictionary<string, object> backingStore = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            AddToBackingStore(backingStore, String.Empty, jsonData);
            return new DictionaryValueProvider<object>(backingStore, CultureInfo.CurrentCulture);
        }

        private static string MakeArrayKey(string prefix, int index)
        {
            return prefix + "[" + index.ToString(CultureInfo.InvariantCulture) + "]";
        }

        private static string MakePropertyKey(string prefix, string propertyName)
        {
            return (String.IsNullOrEmpty(prefix)) ? propertyName : prefix + "." + propertyName;
        }
    }
}