using System;
using System.Reflection;
using NLog;
using NLog.Config;

namespace Niras.Jordflytning.Library.Logging
{
	internal sealed class NLogLogger : ILogger
	{
		private Logger logger;

		public NLogLogger()
		{
			logger = NLog.LogManager.GetCurrentClassLogger();
		}

		public NLogLogger(String name)
		{
			logger = NLog.LogManager.GetLogger(name);
		}

		public void LogException(Exception exc)
		{
			logger.Error(String.Empty, exc);
		}

		public void LogException(String message, Exception exc)
		{
			logger.Error(message, exc);
		}

		public void LogTrace(String message)
		{
			logger.Trace(message);
		}		
		
		public void LogInfo(String message)
		{
			logger.Info(message);
		}

		public void LogWarning(String message)
		{
			logger.Warn(message);
		}

	    internal static void Reconfigure()
	    {
            Type definitionType;
            var layoutRenderers = ConfigurationItemFactory.Default.LayoutRenderers;

            if (layoutRenderers.TryGetDefinition("exceptiondetails", out definitionType))
            {
                return;
            }

            layoutRenderers.RegisterDefinition("exceptiondetails", typeof(ExceptionDetailsRenderer));
            NLog.LogManager.ReconfigExistingLoggers();
	    }

	}
}
