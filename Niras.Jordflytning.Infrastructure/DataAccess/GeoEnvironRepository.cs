using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web.Services.Description;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.JordForurening;
using Niras.Jordflytning.Infrastructure.GeoEnvironServiceReference;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
	public class GeoEnvironRepository : IGeoEnvironRepository
	{
		private static readonly ILogger logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Infrastructure.DataAccess.GeoEnvironRepository");

		private const string EmptyChar1 = " - ";
		private const string EmptyChar2 = "Ingen";
		private const string HeaderText = "Kommunens miljødatabase";

		#region *** new stuff ***

		public List<ForureningsOpslagResult> GetResult(string ejerlav, string ejerlavsnavn, string matrikelnr, IList<JordKlassifikationType> jordKlassifikationTypeList)
		{
			var resList = new List<ForureningsOpslagResult>();
			try
			{
				if (String.IsNullOrEmpty(ejerlav))
					throw new ArgumentException("Systemfejl: Der mangler ejerlavsnummer i input. Kontakt den systemansvarlige.");
				if (String.IsNullOrEmpty(matrikelnr))
					throw new ArgumentException("Systemfejl: Der mangler matrikelnummer i input. Kontakt den systemansvarlige.");

				var url = ConfigurationManager.AppSettings["geoEnvironUrl"];
				var username = ConfigurationManager.AppSettings["geoEnvironUsername"];
				var password = ConfigurationManager.AppSettings["geoEnvironPassword"];

				// Mens vi venter på Geokon, kan man slå authentication fra ved at fjerne credentials

				var hasCredentials = (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password));

                System.ServiceModel.Channels.Binding binding = null;
				if (hasCredentials)
				{
					var wsHttpBinding = new WSHttpBinding();
                    wsHttpBinding.Security.Mode = SecurityMode.Transport;
                    wsHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
					binding = wsHttpBinding;
				}
				else
					binding = new BasicHttpBinding();

				var ea = new EndpointAddress(url);
                var client = new Service_GeoEnvironClient(binding, ea);
				
				if (hasCredentials)
				{
					client.ClientCredentials.UserName.UserName = username;
					client.ClientCredentials.UserName.Password = password;
				}

                var response = client.HentOplysningerForMatrikel(ejerlav, matrikelnr);

				// Map Retur
				var punkter = response.Punkter;

				// only use punkt 11 (according to Århus Kommune - Mads Andersen)
				if (punkter == null || punkter.Punkt11 == null)
					throw new Exception("Systemfejl: Data ikke fundet på den pågældende matrikel. Kontakt den systemansvarlige.");

				var punkt11 = punkter.Punkt11;
				if (punkt11.P11)
				{
					var pStruktur = punkt11.P11Struktur;
					foreach (var punkt11Struktur in pStruktur)
					{
						var res = MapResult(punkt11Struktur, ejerlav, ejerlavsnavn, matrikelnr, jordKlassifikationTypeList);
						if (res!=null)						
							resList.Add(res);
					}
				}
			}
			catch (Exception exception)
			{
				var errorResult = new ForureningsOpslagResult(ExternalDataProviderName.GeoEnviron);
				errorResult.ResultException = exception;
				resList.Add(errorResult);
				logger.LogException("GeoEnvironRepository. Get: Failed.", exception);
			}
			return resList;
		}

		private static ForureningsOpslagResult MapResult(Punkt11Struktur punkt11Struktur, string ejerlav, string ejerlavsnavn, string matrikelnr, IList<JordKlassifikationType> jordKlassifikationTypeList)
		{
			var hasKortlagtForurening = punkt11Struktur.P11A;
			var vidensNiveau = String.IsNullOrEmpty(punkt11Struktur.P11B) ? EmptyChar1 : punkt11Struktur.P11B;
			var dato = String.IsNullOrEmpty(punkt11Struktur.P11C) ? EmptyChar1 : punkt11Struktur.P11C;
			var nuancering = String.IsNullOrEmpty(punkt11Struktur.P11D.ToString()) ? EmptyChar1 : punkt11Struktur.P11D.ToString();
			var lokalitetsId = String.IsNullOrEmpty(punkt11Struktur.SiteId) ? EmptyChar1 : punkt11Struktur.SiteId;
			var bemaekning = String.IsNullOrEmpty(punkt11Struktur.Comment) ? EmptyChar1 : punkt11Struktur.Comment;

			//\r\n\r\n

			bemaekning = bemaekning.Replace("\r\n", "<br/>");

			var hasData = lokalitetsId != EmptyChar1;
			if (!hasData) return null;

			GeoEnvironKlassifikation klassifikation = null;
			foreach (var geoEnvironKlassifikation in GetGeoEnvironKlassifikationList())
			{
				if (vidensNiveau.ToUpper() != geoEnvironKlassifikation.TextId)
					continue;

				klassifikation = geoEnvironKlassifikation;
				break;
			}

			if (klassifikation == null)
			{
				return null;
			}

			var result = new ForureningsOpslagResult(ExternalDataProviderName.GeoEnviron);

			//var longText = string.Format(
			//	"Vidensniveau: {1}, Nuancering: {3}, Kortlagt forurening: {0}, Dato: {2}, Lokalitets id: {4}, {5}Bemærkninger: {6}",
			//	hasKortlagtForurening, vidensNiveau, dato, nuancering, lokalitetsId, Environment.NewLine, bemaekning);			

			var longText = string.Format(
				"Matrikelnr.:&nbsp;&nbsp;&nbsp;{0} <br/>" +
				"Lokalitet:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{1} <br/>" +
				"Bemærkninger:&nbsp;&nbsp;{2}", matrikelnr + "&nbsp;" + ejerlavsnavn, lokalitetsId, bemaekning);

			result.Header = HeaderText;
			//result.ShortText = String.Format("Matrikelnr.: {0}, Ejerlavnr.: {1}, Lokalitetsid: {2} - {3}", matrikelnr, ejerlav, lokalitetsId, klassifikation.Description);
			result.ShortText = klassifikation.Description;
			result.LongText = longText;
			result.JordKlassifikation = jordKlassifikationTypeList.FirstOrDefault(i => i.Kode == (short) ForureningsKlasse.JordKraftigForurenet);
			result.HeaderSortering = 2;
			result.GeoEnvironKlassification = klassifikation;

			return result;
		}

		/// <summary>
		/// Hardcoded klassifikations liste fra GeoEnviron
		/// </summary>
		private static IList<GeoEnvironKlassifikation> GetGeoEnvironKlassifikationList()
		{
			var list = new List<GeoEnvironKlassifikation>();

			list.Add(new GeoEnvironKlassifikation("OPHÆ.J", "Ophævelse af kortlægning, jordhåndtering", true, true, false, "Den tidligere kortlægning på ejendommen er ophævet. Imidlertid er der begrundet mistanke om at jorden indeholder lettere forurenende stoffer og/eller affald og kan således ikke håndteres frit"));
			list.Add(new GeoEnvironKlassifikation("UDT.JO", "Vurderet, ikke kortlægningsgrundlag, jordhåndtering", true, true, false, "Ejendommen er vurderet og der var ikke grundlag for kortlægning. Imidlertid er der begrundet mistanke om at jorden indeholder lettere forurenende stoffer og/eller affald og kan således ikke håndteres frit"));
			list.Add(new GeoEnvironKlassifikation("UAFKL.", "Uafklaret", true, true, true, "Der er en verserende sag på ejendommen."));
			list.Add(new GeoEnvironKlassifikation("V1", "V1 kortlagt ejendom", true, true, true, "V1 kortlagt ejendom."));
			list.Add(new GeoEnvironKlassifikation("V2", "V2 kortlagt ejendom", true, true, true, "V2 kortlagt ejendom."));

			list.Add(new GeoEnvironKlassifikation("OPHÆV.", "Ophævelse af kortlægning", false, true, true, "Den tidligere kortlægning på ejendommen er ophævet."));
			list.Add(new GeoEnvironKlassifikation("OPRYD.", "Opryddet inden kortlægning", false, true, true, "Der er tidligere påvist jordforurening på ejendommen. Forureningen er bortgravet og det er dokumenteret, at der ikke er efterladt jordforurening på ejendommen."));
			list.Add(new GeoEnvironKlassifikation("V1,V2", "V1 og V2 kortlagt", true, true, true, "V1 og V2 kortlagt ejendom."));		
			list.Add(new GeoEnvironKlassifikation("V2,F0", "V2 kortlagt, nuanceret på F0", true, true, true, "V2, F0 kortlagt ejendom."));
			list.Add(new GeoEnvironKlassifikation("V2,F1", "V2 kortlagt, nuanceret på F1", true, true, true, "V2, F1 kortlagt ejendom."));
			list.Add(new GeoEnvironKlassifikation("V2,F2", "V2 kortlagt, nuanceret på F2", true, true, true, "V2, F2 kortlagt ejendom."));		
			//list.Add(new GeoEnvironKlassifikation("UDT.RM", "Vurderet ikke kortlægningsgrundlag", false, false, true, "Ejendommen er vurderet og der var ikke grundlag for kortlægning."));
			//list.Add(new GeoEnvironKlassifikation("UDT.KO", "Vurderet ikke kortlægningsgrundlag", false, false, true, "Ejendommen er undersøgt og der er ikke påvist jordforurenet."));
			
			return list;
		}

		#endregion *** new stuff ***




		#region *** old stuff ***
		
		public List<string> Get(string ejerlav, string matrikelNummer, IList<JordKlassifikationType> jordKlassifikationTypeList, out string exceptionList)
		{

			var strList = new List<string>();
			exceptionList = null;

			try
			{
				if (String.IsNullOrEmpty(ejerlav))
				{
					throw new ArgumentException("Systemfejl: Der mangler ejerlavsnummer i input. Kontakt den systemansvarlige");
				}
				if (String.IsNullOrEmpty(matrikelNummer))
				{
					throw new ArgumentException("Systemfejl: Der mangler matrikelnummer i input. Kontakt den systemansvarlige");
				}
				// Kald service
				var client = new Service_GeoEnvironClient();
				var response = client.HentOplysningerForMatrikel(ejerlav, matrikelNummer);

				// Map Retur
				var punkter = response.Punkter;

				// only use punkt 11 (according to Århus Kommune - Mads Andersen)
				if (punkter == null || punkter.Punkt11 == null)
				{
					throw new Exception("Systemfejl: Data ikke fundet på den pågældende matrikel. Kontakt den systemansvarlige");
				}

				var punkt11 = punkter.Punkt11;
				if (punkt11.P11)
				{
					var pStruktur = punkt11.P11Struktur;

					foreach (var punkt11Struktur in pStruktur)
					{
						var hasKortlagtForurening = punkt11Struktur.P11A;
						if (hasKortlagtForurening == Marker.Ja)
						{
							var vidensNiveau = String.IsNullOrEmpty(punkt11Struktur.P11B) ? " - " : punkt11Struktur.P11B;
							var dato = String.IsNullOrEmpty(punkt11Struktur.P11C) ? " - " : punkt11Struktur.P11C;
							var hasNuancering = String.IsNullOrEmpty(punkt11Struktur.P11D.ToString()) ? " - " : punkt11Struktur.P11D.ToString();
							var lokalitetsId = String.IsNullOrEmpty(punkt11Struktur.SiteId) ? " - " : punkt11Struktur.SiteId;
							var bemaekning = String.IsNullOrEmpty(punkt11Struktur.Comment) ? " - " : punkt11Struktur.Comment;

							var niceString = string.Format(
								"Kortlagt forurening: {0}, Vidensniveau: {1}, Dato: {2}, Nuancering: {3}, Lokalitets id: {4}, {5}Bemærkninger: {6}",
								hasKortlagtForurening, vidensNiveau, dato, hasNuancering, lokalitetsId, Environment.NewLine, bemaekning);
							strList.Add(niceString);
						}
					}
				}
			}
			catch (Exception exception)
			{
				strList = null;
				logger.LogException("GeoEnvironRepository. Get: Failed.", exception);
				exceptionList = exception.Message;
			}

			if (strList != null && strList.Count < 1)
				strList = null;

			return strList;
		}

		public List<GeoEnvironRow> GetRows(string ejerlav, string matrikelNummer, IList<GeoEnvironKlassifikation> geoEnvironKlassifikationList,out string exceptionList)
		{
			var strList = new List<GeoEnvironRow>();
			exceptionList = null;

			try
			{
				if (String.IsNullOrEmpty(ejerlav))
				{
					throw new ArgumentException("Systemfejl: Der mangler ejerlavsnummer i input. Kontakt den systemansvarlige");
				}
				if (String.IsNullOrEmpty(matrikelNummer))
				{
					throw new ArgumentException("Systemfejl: Der mangler matrikelnummer i input. Kontakt den systemansvarlige");
				}
				// Kald service
				var client = new Service_GeoEnvironClient();
				var response = client.HentOplysningerForMatrikel(ejerlav, matrikelNummer);

				// Map Retur
				var punkter = response.Punkter;

				// only use punkt 11 (according to Århus Kommune - Mads Andersen)
				if (punkter == null || punkter.Punkt11 == null)
				{
					throw new Exception("Systemfejl: Data ikke fundet på den pågældende matrikel. Kontakt den systemansvarlige");
				}

				var punkt11 = punkter.Punkt11;
				if (punkt11.P11)
				{
					var pStruktur = punkt11.P11Struktur;

					foreach (var punkt11Struktur in pStruktur)
					{

						var hasKortlagtForurening = punkt11Struktur.P11A;
						var vidensNiveau = String.IsNullOrEmpty(punkt11Struktur.P11B) ? EmptyChar1 : punkt11Struktur.P11B;
						var dato = String.IsNullOrEmpty(punkt11Struktur.P11C) ? EmptyChar1 : punkt11Struktur.P11C;
						var hasNuancering = String.IsNullOrEmpty(punkt11Struktur.P11D.ToString()) ? EmptyChar1 : punkt11Struktur.P11D.ToString();
						var lokalitetsId = String.IsNullOrEmpty(punkt11Struktur.SiteId) ? EmptyChar1 : punkt11Struktur.SiteId;
						var bemaekning = String.IsNullOrEmpty(punkt11Struktur.Comment) ? EmptyChar1 : punkt11Struktur.Comment;

						var hasData = HasAnyData(vidensNiveau, dato, hasNuancering, lokalitetsId, bemaekning);
						if (hasData)
						{

							var niceString = string.Format(
								"Kortlagt forurening: {0}, Vidensniveau: {1}, Dato: {2}, Nuancering: {3}, Lokalitets id: {4}, {5}Bemærkninger: {6}",
								hasKortlagtForurening, vidensNiveau, dato, hasNuancering, lokalitetsId, Environment.NewLine, bemaekning);

							GeoEnvironKlassifikation klassifikation = null;
							foreach (var geoEnvironKlassifikation in geoEnvironKlassifikationList)
							{
								if (vidensNiveau.ToUpper() == geoEnvironKlassifikation.TextId)
								{
									klassifikation = geoEnvironKlassifikation;
									break;
								}
							}
							if (klassifikation != null)
							{
								var row = new GeoEnvironRow(klassifikation, niceString);
								strList.Add(row);
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				strList = null;
				logger.LogException("GeoEnvironRepository. Get: Failed.", exception);
				exceptionList = exception.Message;
			}

			if (strList != null && strList.Count < 1)
				strList = null;

			return strList;
		}

		private static bool HasAnyData(string vidensNiveau, string dato, string hasNuancering, string lokalitetsId, string bemaekning)
		{
			var hasData = !(vidensNiveau == EmptyChar1 && dato == EmptyChar1 && (hasNuancering == EmptyChar1 || hasNuancering == EmptyChar2) &&
			                lokalitetsId == EmptyChar1 && bemaekning == EmptyChar1);
			return hasData;
		}

		//Hent oplysninger via ejendom Hent oplysninger via matrikel
		//Kommunenr: 751 Ejendomsnr: Ejerlavskode: 2006353 Matrikelnr: 12ab
		//P04 relevant: Nej
		//P08 relevant: Nej
		//P09 relevant: Nej
		//P10 relevant: Nej
		//P11 relevant: Ja
		//  Kortlagt forurening :Ja Vidensniveau :V2 Dato: Nuancering:Ikke nuanceret endnu Lokalitets id: Bemrk:
		//
		//  Kortlagt forurening:Ja Vidensniveau:V2 Dato: Nuancering:Ikke nuanceret endnu Lokalitets id:751-00756-00 
		//  Bemrk: Region Midtjylland/tidligere Århus Amt har kortlagt ejendommen på vidensniveau 2 efter lov om forurenet jord. Der er udføt forureningsundersøgelse på ejendommen. som har påvist forurening. der overskrider gældende grænseværdier og som har begrundet. at ejendommen kortlægges. Jorden på ejendommen kan derfor ikke håndteres frit. ltilfælde af. atjord mskes bortgravet og ﬂyttet. skal jordﬂytningen anmeldes til Aarhus Kommune. Yderligere oplysninger kan fås ved henvendelse til Region Midtjylland. Miljø. e-mail: miljoe@ru.m1.dk. tlf.: 78 41 19 99 eller Natur og Miljø. Aarhus Kommune. tlf.: 89 40 45 22. mail:jordgruppen@mtm.aarhus.dk.
		//
		//  Kortlagt forurening:Ja Vidensniveau:V2 Dato: Nuancering:Ikke nuanceret endnu Lokalitets id:751-00756-01 
		//  Bemrk:Region Midtjylland/tidligere Arhus Amt har kortlagt ejendommen på vidensniveau 2 efter lov om forurenet jord. Der er udføt forureningsundersøgelse på ejendommen. som har påvist forurening. der overskrider gældende grænseværdier og som har begrundet. at ejendommen kortlægges. Jorden på ejendommen kan derfor ikke håndteres frit. ltilfælde af. atjord mskes bortgravet og ﬂyttet. skal jordﬂytningen anmeldes til Aarhus Kommune. Yderligere oplysninger kan fås ved henvendelse til Region Midtjylland. Miljø. e-mail: miljoe@ru.m1.dk. tlf.: 78 41 19 99 eller Natur og Miljø. Aarhus Kommune. tlf.: 89 40 45 22. mail:jordgruppen@mtm.aarhus.dk.
		//
		//P12 relevant :Ja
		//  Lettere forurenet :Ja Anden viden:Nej

		#endregion *** old stuff ***



	}
}
