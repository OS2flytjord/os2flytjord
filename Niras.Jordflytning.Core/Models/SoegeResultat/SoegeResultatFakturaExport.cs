using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Niras.Jordflytning.Core.Models.SoegeResultat
{
	/// <summary>
	/// A row from a search result
	/// </summary>
	public class SoegeResultatFakturaExport
	{

		/// <summary>
		///Oprindelsessted
		///Postnr + PostDistrikt
		///Postnr
		///Sammensat postnr og distrikt af jordens oprindelsessted
		///String
		///8210 Århus V
		/// </summary>
		public string Postnr { get; set; }

		/// <summary>
		///	Person
		///Navn
		///TransportoerNavn
		///Navnet på transportøren
		///String
		///Jan Pedersen 
		/// </summary>
		public string TransportoerNavn { get; set; }

		/// <summary>
		/// Firmaoplysninger
		///Navn
		///TransportoerFirma
		///Navnet på transportørens firma
		///String 
		///Pedersen Transport Aps.
		/// </summary>
		public string TransportoerFirma { get; set; }

		/// <summary>
		/// Vognlaes
		///Id
		///VognlaesId
		///Intern id
		///Guid
		///D8D3BD7A-1217-4368-8B47-79C200324E4B
		/// </summary>
		public Guid VognlaesId { get; set; }

		/// <summary>
		///	Lastbil
		///Nummerplade
		///Lastbil
		///nummerpladen på lastbilen
		///String
		///xc 58 123
		/// </summary>
		public string Lastbil { get; set; }

		/// <summary>
		///Vognlaes
		///MaengdeAksler
		///MaengdeAksler
		///Jordmængde i aksler
		///Int
		///5
		/// </summary>
		public int MaengdeAksler { get; set; }

		/// <summary>
		/// Vognlaes
		///Maengde
		///MaengdeTons
		///Jordmængder i Tons
		///Int
		///30
		/// </summary>
		public string MaengdeTons { get; set; }

		/// <summary>
		/// Betaler
		///Id
		///BetalerId
		///Id på betaler
		///Guid
		///1DD60D51-8DD4-4199-BCFD-E221E566563B
		/// </summary>
		public Guid BetalerId { get; set; }

		/// <summary>
		///Person
		///Navn
		///BetalerNavn
		///Personnavn på betaler
		///String
		/// </summary>
		public string BetalerNavn { get; set; }

		/// <summary>
		/// Firmaoplysninger
		///Navn
		///BetalerFirma
		///Firmanavn på betaler
		///string
		/// </summary>
		public string BetalerFirma { get; set; }

		/// <summary>
		/// Firmaoplysninger
		///CVR
		///CVR
		///Firmaets CVR nummer
		///int
		///25798376
		/// </summary>
		public string Cvr { get; set; }

		/// <summary>
		///Firmaoplysninger
		///P nummer
		///Pnummer
		///Firmaets Pnummer
		///string
		///2
		/// </summary>
		public string Pnummer { get; set; }

		/// <summary>
		/// Anmelder
		///Id
		///AnmelderId
		///Id på anmelder
		///Guid
		///1768E3A6-E8F1-4277-B443-7B2F93F4CEBC
		/// </summary>
		public Guid AnmelderId { get; set; }

		/// <summary>
		///Person
		///Navn
		///AnmelderNavn
		///Personnavn på anmelder
		///string
		///Dennis Jensen
		/// </summary>
		public string AnmelderNavn { get; set; }

		/// <summary>
		///Firmaoplysninger
		///Navn
		///AnmelderFirma
		///Anmelder firma. (Felt kan være tomt)
		///string
		///Jord.Aps 
		/// </summary>
		public string AnmelderFirma { get; set; }

		/// <summary>
		///ModtagerAnlaeg
		///Id
		///ModtagerAnlaegId
		///Id på modtageranlaeg
		///Guid
		///C55EADEF-E2BF-4A76-A94A-129550D4FDF4
		/// </summary>
		public Guid ModtagerAnlaegId { get; set; }

		/// <summary>
		/// ModtagerAnlaeg
		///Loebenummer
		///ModtagerAnlaegNummer
		///Id på modtageranlaeg
		///int
		///1021
		/// </summary>
		public string ModtagerAnlaegNummer { get; set; }

		/// <summary>
		///ModtagerAnlaeg
		///Navn
		///ModtagerAnlaegNavn
		///Navn på modtageranlaeg
		///string
		///Århus Havn, Olie havnen, Ren jord
		/// </summary>
		public string ModtagerAnlaegNavn { get; set; }

		/// <summary>
		/// Dato for vognlæsset
		/// </summary>
		public string Dato { get; set; }

		public string Adresse { get; set; }

		public DateTime OrderByDato { get; set; }


		public string BeskedTilJordmodtager { get; set; }
		public string AnmeldersSagsnummer { get; set; }
		public string FakturaEmails { get; set; }
		public string EanNummer { get; set; }

		public string LoebeNummer { get; set; }


		private static String Escape(String s)
		{
			var retVal = "";
			if (!String.IsNullOrEmpty(s))
			{
				var sb = new StringBuilder();
				var needQuotes = false;
				foreach (var c in s.ToArray())
				{
					switch (c)
					{
						case '"':
							sb.Append("\\\"");
							needQuotes = true;
							break;
						case ' ':
							sb.Append(" ");
							needQuotes = true;
							break;
						case ',':
							sb.Append(",");
							needQuotes = true;
							break;
						case '\t':
							sb.Append("\\t");
							needQuotes = true;
							break;
						case '\n':
							sb.Append("\\n");
							needQuotes = true;
							break;
						default:
							sb.Append(c);
							break;
					}
				}
				if (needQuotes)
					return "\"" + sb + "\"";
				retVal= sb.ToString();
			}
			return retVal;
		}

		public void SerializeAsCsv(StreamWriter stream)
		{
			const string separator = ";";
			//const string newLine = "\n";
			var newLine = Environment.NewLine;
            
			stream.Write(Escape(OrderByDato.ToString(CultureInfo.InvariantCulture)));
			stream.Write(separator);
			stream.Write(Escape(LoebeNummer));
			stream.Write(separator);
			stream.Write(Escape(Adresse));
			stream.Write(separator);
			stream.Write(Escape(Postnr));
			stream.Write(separator);
			stream.Write(Escape(TransportoerNavn));
			stream.Write(separator);
			stream.Write(Escape(TransportoerFirma));
			stream.Write(separator);
			stream.Write(Escape(VognlaesId.ToString()));
			stream.Write(separator);
			stream.Write(Escape(Lastbil));
			stream.Write(separator);
			stream.Write(Escape(MaengdeAksler.ToString(CultureInfo.InvariantCulture)));
			stream.Write(separator);
			if (MaengdeTons != null) 
				stream.Write(Escape(MaengdeTons));
			stream.Write(separator);
			stream.Write(Escape(BetalerId.ToString()));
			stream.Write(separator);
			stream.Write(Escape(BetalerNavn));
			stream.Write(separator);
			stream.Write(Escape(BetalerFirma));
			stream.Write(separator);
			stream.Write(Escape(Cvr));
			stream.Write(separator);
			stream.Write(Escape(Pnummer));
			stream.Write(separator);
			stream.Write(Escape(AnmelderId.ToString()));
			stream.Write(separator);
			stream.Write(Escape(AnmelderNavn));
			stream.Write(separator);
			stream.Write(Escape(AnmelderFirma));
			stream.Write(separator);
			stream.Write(Escape(ModtagerAnlaegId.ToString()));
			stream.Write(separator);
			if (ModtagerAnlaegNummer != null) 
				stream.Write(Escape(ModtagerAnlaegNummer));
			stream.Write(separator);
			stream.Write(Escape(ModtagerAnlaegNavn));
			stream.Write(separator);
			if (BeskedTilJordmodtager != null)
				stream.Write(Escape(BeskedTilJordmodtager));
			stream.Write(separator);
			if (AnmeldersSagsnummer != null)
				stream.Write(Escape(AnmeldersSagsnummer)); 
			stream.Write(separator);
			if (FakturaEmails != null)
				stream.Write(Escape(FakturaEmails)); 
			stream.Write(separator);
			if (EanNummer != null)
				stream.Write(Escape(EanNummer));

			stream.Write(newLine);
		}

		public void SeralizeColnamesAsCsv(StreamWriter stream)
		{
			const string separator = ";";
			//const string newLine = "\n";
			var newLine = Environment.NewLine;

			stream.Write("Dato");
			stream.Write(separator);
			stream.Write("AnmeldelsesNummer");
			stream.Write(separator);
			stream.Write("Adresse");
			stream.Write(separator);
			stream.Write("Postnr");
			stream.Write(separator);
			stream.Write("TransportoerNavn");
			stream.Write(separator);
			stream.Write("TransportoerFirma");
			stream.Write(separator);
			stream.Write("VognlaesId");
			stream.Write(separator);
			stream.Write("Lastbil");
			stream.Write(separator);
			stream.Write("MaengdeAksler");
			stream.Write(separator);
			stream.Write("MaengdeTons");
			stream.Write(separator);
			stream.Write("BetalerId");
			stream.Write(separator);
			stream.Write("BetalerNavn");
			stream.Write(separator);
			stream.Write("BetalerFirma");
			stream.Write(separator);
			stream.Write("Cvr");
			stream.Write(separator);
			stream.Write("Pnummer");
			stream.Write(separator);
			stream.Write("AnmelderId");
			stream.Write(separator);
			stream.Write("AnmelderNavn");
			stream.Write(separator);
			stream.Write("AnmelderFirma");
			stream.Write(separator);
			stream.Write("ModtagerAnlaegId");
			stream.Write(separator);
			stream.Write("ModtagerAnlaegNummer");
			stream.Write(separator);
			stream.Write("ModtagerAnlaegNavn");
			stream.Write(separator);
			stream.Write("BeskedTilJordmodtager");
			stream.Write(separator);
			stream.Write("AnmeldersSagsnummer");
			stream.Write(separator);
			stream.Write("FakturaEmails");
			stream.Write(separator);
			stream.Write("EanNummer");

			stream.Write(newLine);
		}
	}
}
