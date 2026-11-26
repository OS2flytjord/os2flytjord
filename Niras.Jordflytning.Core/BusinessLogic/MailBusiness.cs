using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Core.BusinessLogic
{

    public class MailBusiness : IMailBusiness
    {
        private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.MailBusiness");

        public void SendEmail(string emailSubject, string emailContent, string emailTo)
        {
            var smtpHost = ConfigurationManager.AppSettings["smtp.host"];
            var senderAddress = ConfigurationManager.AppSettings["sender.address"];
            try
            {
                var smtpPort = ConfigurationManager.AppSettings["smtp.port"] ?? "25";
                var smtpUserName = ConfigurationManager.AppSettings["smtp.username"];
                var smtpPassword = ConfigurationManager.AppSettings["smtp.password"];
                var smtpUseSsl = ConfigurationManager.AppSettings["smtp.usessl"] ?? "false";
                var smtpUseSslbool = (smtpUseSsl.Trim().ToLower() == "true" || smtpUseSsl.Trim() == "1");
                var senderDisplayName = ConfigurationManager.AppSettings["sender.displayname"];

                using (var mail = OpretMail(new MailAddress(senderAddress, senderDisplayName), new MailAddress(emailTo), emailSubject, emailContent))
                using (var server = new SmtpClient(smtpHost)
                {
                    Port = int.Parse(smtpPort),
                    Credentials = new NetworkCredential(smtpUserName, smtpPassword),
                    EnableSsl = smtpUseSslbool
                })
                {
                    server.Send(mail);
                }

                Logger.LogInfo(string.Format("Email sendt til {0}: {1}", emailTo, emailSubject));
                GemEmailLog(emailTo, senderAddress, emailSubject, true, null, smtpHost);
            }
            catch (Exception ex)
            {
                Logger.LogException(string.Format("Email til {0} kunne IKKE sendes: {1}", emailTo, emailSubject), ex);
                GemEmailLog(emailTo, senderAddress, emailSubject, false, ex.ToString(), smtpHost);
            }
        }

        /// <summary>
        /// Registrerer afsendelsen i tabellen EmailLog (Database\Version\1.2.0\003_EmailLog.sql).
        /// Skrives med en selvstændig forbindelse, så den hverken påvirker eller rulles tilbage
        /// sammen med Entity Framework-konteksten. Mailens indhold gemmes ikke.
        /// En fejl her må aldrig stoppe afsendelsen af mails - den logges kun.
        /// </summary>
        private static void GemEmailLog(string modtager, string afsender, string emne, bool sendt, string fejl, string smtpServer)
        {
            try
            {
                var cn = ConfigurationManager.ConnectionStrings["DefaultConnection"];
                if (cn == null || string.IsNullOrEmpty(cn.ConnectionString))
                    return;

                using (var conn = new SqlConnection(cn.ConnectionString))
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO dbo.EmailLog (Modtager, Afsender, Emne, Sendt, Fejl, SmtpServer) " +
                                      "VALUES (@Modtager, @Afsender, @Emne, @Sendt, @Fejl, @SmtpServer)";
                    cmd.Parameters.Add("@Modtager", SqlDbType.NVarChar, 320).Value = (object)Afkort(modtager ?? string.Empty, 320);
                    cmd.Parameters.Add("@Afsender", SqlDbType.NVarChar, 320).Value = (object)Afkort(afsender, 320) ?? DBNull.Value;
                    cmd.Parameters.Add("@Emne", SqlDbType.NVarChar, 500).Value = (object)Afkort(emne, 500) ?? DBNull.Value;
                    cmd.Parameters.Add("@Sendt", SqlDbType.Bit).Value = sendt;
                    cmd.Parameters.Add("@Fejl", SqlDbType.NVarChar, -1).Value = (object)fejl ?? DBNull.Value;
                    cmd.Parameters.Add("@SmtpServer", SqlDbType.NVarChar, 255).Value = (object)Afkort(smtpServer, 255) ?? DBNull.Value;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("Kunne ikke skrive til EmailLog (er Database\\Version\\1.2.0\\003_EmailLog.sql kørt?)", ex);
            }
        }

        private static string Afkort(string vaerdi, int maks)
        {
            if (vaerdi == null || vaerdi.Length <= maks)
                return vaerdi;
            return vaerdi.Substring(0, maks);
        }

        /// <summary>
        /// Opretter mailen som multipart/alternative med både en ren tekst-del og en HTML-del.
        /// Mails der kun har HTML giver højere spam-score hos mange mailfiltre.
        /// </summary>
        public static MailMessage OpretMail(MailAddress fra, MailAddress til, string emne, string html)
        {
            var mail = new MailMessage(fra, til)
            {
                Subject = emne,
                SubjectEncoding = Encoding.UTF8,
                Body = HtmlTilTekst(html),
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = false
            };
            mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html ?? string.Empty, Encoding.UTF8, "text/html"));
            return mail;
        }

        /// <summary>
        /// Laver en simpel ren tekst-udgave af en HTML-mail: links skrives som "tekst (url)",
        /// afsnit og linjeskift bevares, og alle øvrige tags fjernes.
        /// </summary>
        public static string HtmlTilTekst(string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            var tekst = html;
            const RegexOptions opt = RegexOptions.IgnoreCase | RegexOptions.Singleline;

            // Fjern alt der ikke er synligt indhold.
            tekst = Regex.Replace(tekst, @"<(head|style|script|title)\b.*?</\1\s*>", string.Empty, opt);

            // Links: "tekst (url)" - eller kun url hvis teksten er selve url'en.
            tekst = Regex.Replace(tekst, @"<a\b[^>]*?href\s*=\s*[""']([^""']*)[""'][^>]*>(.*?)</a\s*>", m =>
            {
                var url = m.Groups[1].Value.Trim();
                var linkTekst = Regex.Replace(m.Groups[2].Value, "<[^>]+>", string.Empty).Trim();
                if (linkTekst.Length == 0 || string.Equals(WebUtility.HtmlDecode(linkTekst), WebUtility.HtmlDecode(url), StringComparison.OrdinalIgnoreCase))
                    return url;
                return linkTekst + " (" + url + ")";
            }, opt);

            // Linjeskift og afsnit.
            tekst = Regex.Replace(tekst, @"<br\s*/?>", "\n", opt);
            tekst = Regex.Replace(tekst, @"</(p|div|tr|h[1-6]|li|table)\s*>", "\n\n", opt);
            tekst = Regex.Replace(tekst, @"<li\b[^>]*>", "- ", opt);

            // Resten af tags væk, og entiteter (&nbsp; &aring; ...) afkodes.
            tekst = Regex.Replace(tekst, "<[^>]+>", string.Empty, opt);
            tekst = WebUtility.HtmlDecode(tekst).Replace(' ', ' ');

            // Ryd op i whitespace: trim hver linje og højst én tom linje i træk.
            tekst = tekst.Replace("\r\n", "\n").Replace('\r', '\n');
            tekst = Regex.Replace(tekst, @"[ \t]+", " ");
            tekst = Regex.Replace(tekst, @" *\n *", "\n");
            tekst = Regex.Replace(tekst, @"\n{3,}", "\n\n");

            return tekst.Trim().Replace("\n", "\r\n");
        }
    }
}
