using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.dk.niras.informatik.emailservice;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Core.BusinessLogic
{

    public class MailBusiness : IMailBusiness
    {
        private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.MailBusiness");

        public void SendEmail(string emailSubject, string emailContent, string emailTo)
        {
            try
            {
                var smtpHost = ConfigurationManager.AppSettings["smtp.host"];
                var smtpPort = ConfigurationManager.AppSettings["smtp.port"] ?? "25";
                var smtpUserName = ConfigurationManager.AppSettings["smtp.username"];
                var smtpPassword = ConfigurationManager.AppSettings["smtp.password"];
                var smtpUseSsl = ConfigurationManager.AppSettings["smtp.usessl"] ?? "false";
                var smtpUseSslbool = (smtpUseSsl.Trim().ToLower() == "true" || smtpUseSsl.Trim() == "1");
                var senderAddress = ConfigurationManager.AppSettings["sender.address"];
                var senderDisplayName = ConfigurationManager.AppSettings["sender.displayname"];
                var mail = new MailMessage(new MailAddress(senderAddress, senderDisplayName), new MailAddress(emailTo))
                {
                    Subject = emailSubject, 
                    Body =  emailContent,
                    IsBodyHtml = true
                };
                var server = new SmtpClient(smtpHost)
                {
                    Port = int.Parse(smtpPort),
                    Credentials = new NetworkCredential(smtpUserName, smtpPassword),                    
                    EnableSsl = smtpUseSslbool                   
                };
                
                server.Send(mail);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            //try
            //{
            //  var mailService = new EmailService();
            //  var appSettings = ConfigurationManager.AppSettings;
            //  var afsender = appSettings["sender.address"];


            //  mailService.SendEmailWithBccByNirasSMTP(afsender, emailSubject, emailContent, emailTo, null);
            //}
            //catch (Exception ex)
            //{
            //  Logger.LogException(ex);
            //}

        }
    }
}
