using System.Collections.Generic;

namespace Niras.Jordflytning.Library.Email
{
	/// <summary>
	/// Data class for e-mail notification.
	/// </summary>
	public class EmailNotification
	{
		public string Subject { get; private set; }
		public string Body { get; private set; }

		public IEnumerable<string> Recipients { get; private set; }

		public EmailNotification(string subject, string body, IEnumerable<string> recipients)
		{
			Subject = subject;
			Body = body;
			Recipients = recipients;
		}
	}
}