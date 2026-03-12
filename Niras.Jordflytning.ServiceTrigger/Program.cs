using Niras.Jordflytning.Library.Logging;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;

namespace Niras.Jordflytning.ServiceTrigger
{
    public class Program
	{
		private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.ServiceTrigger");

		private static void Main(string[] args)
		{
			if (args == null | !args.Any())
				Console.WriteLine("Programmet skal kaldes med et argument. Fx med argumentet adresser eller afslut_gamle");

			if (args != null)
				foreach (var a in args)
				{
					Logger.LogInfo(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " - Start: " + a);
					switch (a.ToLower())
					{
						case "adresser":
							{
								//Vejene til adresseopslaget gemmes i en application variable
								//Denne metode nulstiller variablen og henter alle vejene på ny.
								try
								{
									var appSettings = ConfigurationManager.AppSettings;
									var requestUrl = appSettings["adresseUrl"];
									var timeout = int.Parse(appSettings["serviceTimeout"]);

									SendRequest(requestUrl, timeout);
								}
								catch (Exception ex)
								{
									Logger.LogException("Fejl i ServiceTrigger - Adresser",ex);
								}
								break;
							}

						case "afslut_gamle":
							{
								//Denne metode afslutter gamle anmeldelser
								try
								{
									var appSettings = ConfigurationManager.AppSettings;
									var requestUrl = appSettings["afslutGamleUrl"];
									var afslutGamleAntalUger = Int32.Parse(appSettings["afslutGamleAntalUger"]);
									var timeout = int.Parse(appSettings["serviceTimeout"]);

									requestUrl = requestUrl + "?antalUger=" + afslutGamleAntalUger;

									SendRequest(requestUrl, timeout);
									
								}
								catch (Exception ex)
								{
									Logger.LogException("Fejl i ServiceTrigger - Afslut_gamle anmeldelser.", ex);
								}
								break;
							}

						default:
							{
								Console.WriteLine();
								break;
							}
					}
					Logger.LogInfo(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " - Slut: " + a);
				}
		}

		private static void SendRequest(string requestUrl, int timeout)
		{
            // Accept all certificates:
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;

            // Create Request
            var request = WebRequest.Create(requestUrl);
			// set request method 
			request.Method = "GET";

			// set content type 
			request.ContentType = "application/x-www-form-urlencoded";
		    request.Timeout =  timeout; // i millisec.

			// get response for GET request 
			using (var response = request.GetResponse() as HttpWebResponse)
			{
				if (response != null && response.StatusCode != HttpStatusCode.OK)
					throw new Exception("Kunne ikke kalde med: " + requestUrl);

				if (response == null) 
					return;

				var resStream = response.GetResponseStream();
				if (resStream != null)
				{
					using (var reader = new StreamReader(resStream))
					{
						reader.ReadToEnd();
					}
					resStream.Close();
				}
				response.Close();
			}
		}
	}
}
