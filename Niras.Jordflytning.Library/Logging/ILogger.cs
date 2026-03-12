using System;

namespace Niras.Jordflytning.Library.Logging
{
	public interface ILogger
	{
		void LogException(Exception exc);
		void LogException(String message, Exception exc);
		void LogTrace(String message);
		void LogWarning(String message);
		void LogInfo(String message);
	}
}
