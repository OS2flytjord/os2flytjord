using System.IO;
using System.Linq;
using System.Net.Mail;
using NUnit.Framework;
using Niras.Jordflytning.Core.BusinessLogic;

namespace Niras.Jordflytning.UnitTests
{
	[TestFixture]
	[Niras.Jordflytning.IntegrationTests.TestsSetup.RulTilbageEfterTest]
	public class MailBusinessTests
	{
		[Test]
		public void HtmlTilTekst_FjernerHeadOgTags_OgBevarerAfsnit()
		{
			var html = "<html><head><title>x</title><style>p{color:red}</style></head><body>" +
			           "<p>Du er blevet oprettet p&aring; FlytJord.dk.</p><p>Linje 1<br/>Linje 2</p></body></html>";

			var tekst = MailBusiness.HtmlTilTekst(html);

			Assert.AreEqual("Du er blevet oprettet på FlytJord.dk.\r\n\r\nLinje 1\r\nLinje 2", tekst);
		}

		[Test]
		public void HtmlTilTekst_LinkMedUrlSomTekst_SkrivesKunEnGang()
		{
			var url = "https://flytjord.dk/bruger/aktiver/abc123";
			var tekst = MailBusiness.HtmlTilTekst("<p><a href=\"" + url + "\">" + url + "</a></p>");

			Assert.AreEqual(url, tekst);
		}

		[Test]
		public void HtmlTilTekst_LinkMedAndenTekst_SkrivesSomTekstOgUrl()
		{
			var tekst = MailBusiness.HtmlTilTekst("<a href=\"https://flytjord.dk/x\">Godkend</a>");

			Assert.AreEqual("Godkend (https://flytjord.dk/x)", tekst);
		}

		[Test]
		public void HtmlTilTekst_TomInput_GiverTomStreng()
		{
			Assert.AreEqual(string.Empty, MailBusiness.HtmlTilTekst(null));
			Assert.AreEqual(string.Empty, MailBusiness.HtmlTilTekst(""));
		}

		[Test]
		public void HtmlTilTekst_RigtigSkabelon_IndeholderAktiveringslinkOgIngenTags()
		{
			var sti = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Niras.Jordflytning\App_Data\Skabeloner\System\TilNyBruger.st");
			if (!File.Exists(sti))
				Assert.Ignore("Skabelonen blev ikke fundet: " + sti);

			var html = File.ReadAllText(sti, System.Text.Encoding.GetEncoding(1252))
				.Replace("$style()$", "")
				.Replace("$aktiverUrl$", "https://flytjord.dk/bruger/aktiver/abc123");

			var tekst = MailBusiness.HtmlTilTekst(html);

			StringAssert.Contains("https://flytjord.dk/bruger/aktiver/abc123", tekst);
			StringAssert.Contains("Du er blevet oprettet som bruger på FlytJord.dk.", tekst);
			StringAssert.DoesNotContain("<", tekst);
		}

		[Test]
		public void OpretMail_HarTekstOgHtmlDel()
		{
			using (var mail = MailBusiness.OpretMail(new MailAddress("afsender@example.com", "FlytJord"),
				new MailAddress("modtager@example.com"), "Emne", "<p>Hej <b>dig</b></p>"))
			{
				Assert.IsFalse(mail.IsBodyHtml);
				Assert.AreEqual("Hej dig", mail.Body);
				Assert.AreEqual(1, mail.AlternateViews.Count);
				Assert.AreEqual("text/html", mail.AlternateViews.Single().ContentType.MediaType);
			}
		}
	}
}
