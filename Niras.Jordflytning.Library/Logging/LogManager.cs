using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog.Config;

namespace Niras.Jordflytning.Library.Logging
{
	public sealed class LogManager
	{
		private static readonly LogManager instance = new LogManager();

		private LogManager()
		{

		}

		public static LogManager Instance
		{
			get { return instance; }
		}

		public ILogger GetLogger()
		{
			return new NLogLogger();
		}

		public ILogger GetLogger(String name)
		{
			return new NLogLogger(name);
		}

        public static void ReconfigExistingLoggers()
	    {
            NLogLogger.Reconfigure();
	    }

	}
}
