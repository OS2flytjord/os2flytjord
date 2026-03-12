using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Niras.Jordflytning.Library.Email
{
	/// <summary>
	/// E-mail gateway that sends given messages through SMTP server.
	/// </summary>
	public class EmailGateway
	{
		private readonly IEnumerable<EmailNotification> _notifications;
		private readonly int _smtpPort = 25;
		private readonly string _senderAddress = "noreply@jordflytning.dk";
		private readonly string _senderDisplayName = "Jordflytning";
		private readonly string _smtpHost = "mxi.niras.net";
		private readonly string _smtUsername = "";
        private readonly string _smtPassword = "";
        private readonly bool _smtUseSsl = false;

        private EmailGateway(IEnumerable<EmailNotification> notifications, string smtpHost, int smtpPort, string senderAddress, string senderDisplayName, string username, string password, bool usessl)
		{
			_notifications = notifications;
			_smtpHost = smtpHost;
			_smtpPort = smtpPort;
			_senderAddress = senderAddress;
			_senderDisplayName = senderDisplayName;
            _smtUsername = username;
			_smtPassword = password;
			_smtUseSsl = usessl;
        }

		public static EmailGateway Create(IEnumerable<EmailNotification> notifications, string smtpHost = null, int? smtpPort = null, 
			string senderAddress = null,string senderDisplayName = null)
		{
			var aSmtpHost = smtpHost ?? ConfigurationManager.AppSettings["smtp.host"];
			var aSmtpPort = smtpPort ?? int.Parse(ConfigurationManager.AppSettings["smtp.port"]);
			var aSenderAddress = senderAddress ?? ConfigurationManager.AppSettings["sender.address"];
			var aSenderDisplayName = senderDisplayName ?? ConfigurationManager.AppSettings["sender.displayname"];
            var smtpUserName = ConfigurationManager.AppSettings["smtp.username"];
            var smtpPassword = ConfigurationManager.AppSettings["smtp.password"];
            var smtpUseSsl = ConfigurationManager.AppSettings["smtp.usessl"] ?? "true";
            var smtpUseSslbool = (smtpUseSsl.Trim().ToLower() == "true" || smtpUseSsl.Trim() == "1");

            return new EmailGateway(notifications, aSmtpHost, aSmtpPort, aSenderAddress, aSenderDisplayName, smtpUserName, smtpPassword, smtpUseSslbool);
		}

		public void SendNotifications(bool asHtml, Action allFinished = null)
		{
			var from = new MailAddress(_senderAddress, _senderDisplayName, Encoding.UTF8);
			var emails = PrepareEmails(from, asHtml).ToList();
			var emailsToSend = emails.Count();

			foreach (var email in emails)
			{
				var currentEmail = email;
				try
				{
					var client = new SmtpClient(_smtpHost)
					{
						Port = _smtpPort,
						Credentials = new NetworkCredential(_smtUsername, _smtPassword),
						EnableSsl = _smtUseSsl
					};
					client.Send(currentEmail);
				}
				catch (Exception exc)
				{
					var ex = exc;
				}
				finally
				{
					emailsToSend--;
					if (emailsToSend == 0 && allFinished != null)
						allFinished();
				}
			}
		}

		private IEnumerable<MailMessage> PrepareEmails(MailAddress sender, bool asHtml)
		{
			var emailList = new List<MailMessage>();

			foreach (var emailNotification in _notifications)
			{
				foreach (var modtager in emailNotification.Recipients)
				{
					var email = new MailMessage(sender, new MailAddress(modtager))
						{
							SubjectEncoding = Encoding.UTF8,
							Subject = emailNotification.Subject,
							BodyEncoding = Encoding.UTF8,
							Body = emailNotification.Body,
							IsBodyHtml = asHtml
						};
					try
					{
						var altView = AlternateView.CreateAlternateViewFromString(emailNotification.Body, null, "text/html");
						email.AlternateViews.Add(altView);
					}
					catch (Exception exception)
					{
						var ex = exception;
					}
					
					emailList.Add(email);
				}
			}

			return emailList;
		}
	}
}