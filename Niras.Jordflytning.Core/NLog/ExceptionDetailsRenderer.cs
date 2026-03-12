using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;
using NLog;
using NLog.Config;
using NLog.LayoutRenderers;

namespace Niras.Jordflytning.Core.NLog
{
    [ThreadAgnostic]
    [LayoutRenderer("exceptiondetails")]
    public class ExceptionDetailsRenderer : LayoutRenderer
    {
        public ExceptionDetailsRenderer():base()
        {
             
        }
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            try
            {

                if ((logEvent.Level == LogLevel.Error || logEvent.Level == LogLevel.Fatal))
                {
                    builder.AppendLine("");
                    builder.AppendLineFormat("Identity: {0}", HttpContext.Current.User.Identity.Name);
                    WriteNameValueCollection(builder, "QueryString", HttpContext.Current.Request.QueryString);
                    WriteNameValueCollection(builder, "Headers", HttpContext.Current.Request.Headers);
                    WriteNameValueCollection(builder, "Form", HttpContext.Current.Request.Form);
                    WriteNameValueCollection(builder, "ServerVars", HttpContext.Current.Request.ServerVariables);
                    WriteBody(builder, HttpContext.Current.Request);
                }
                
            }
            catch
            {
                builder.AppendLineFormat("\nFailed to print HttpRequest. Exception: {0}", logEvent.Exception);
            }
        }

        #region Private methods

        private static void WriteNameValueCollection(StringBuilder sb, string name, NameValueCollection collection)
        {
            sb.AppendLineFormat("{0}:", name);

            foreach (string key in collection.AllKeys)
                sb.AppendLineFormat("{0}={1}", key, collection[key]);
        }

        private static void WriteBody(StringBuilder sb, HttpRequest request)
        {
            Stream input = request.InputStream;
            long oldPosition = input.Position;

            var data = new byte[request.ContentLength];
            input.Position = 0;
            int length = input.Read(data, 0, data.Length);
            input.Position = oldPosition;

            sb.AppendLineFormat("ContentLength={0}", data.Length);
            sb.AppendLineFormat("BodyLength={0}", length);
            sb.AppendLine("Body=");
            sb.AppendLine(BitConverter.ToString(data, 0, length));

        }

        #endregion
    }
    public static class NativeExtensions
    {
        public static void AppendLineFormat(this StringBuilder sb, string format, params object[] args)
        {
            sb.AppendLine(String.Format(format, args));
        }
    }
}

