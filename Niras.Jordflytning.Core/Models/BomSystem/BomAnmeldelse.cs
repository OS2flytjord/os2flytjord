using System;

namespace Niras.Jordflytning.Core.Models.BomSystem
{

	public class BomAnmeldelse
	{
		/// <summary>
		/// Hvis QR koden ikke kan scannes, indtastes cifrene i 
		/// JordmodtageranlægId plus cifrene i Anmeldelseslbrnr i stedet. 
		/// AnmeldelsesId er ikke en GUID.
		/// </summary>
		public int Anmeldelseslbrnr { get; set; }
		
		/// <summary>
		/// Oprindelsesadresse for jorden
		/// </summary>
		public string Oprindelsesadresse { get; set; }

		/// <summary>
		/// True hvis godkendt
		/// </summary>
		public bool Godkendt { get; set; }
				
		/// <summary>
		/// (unik)	Id for jordmodtageranlægget
		/// </summary>
		public int JordmodtageranlægId { get; set; }
				
		/// <summary>
		/// True hvis jordmængden er overskredet
		/// </summary>
		public bool JordmaengdeOverskredet { get; set; }
				
		/// <summary>
		/// True hvis kørselsperioden er overskredet
		/// </summary>
		public bool KoerselsperiodeOverskredet { get; set; }
				
		/// <summary>
		/// True hvis vognlæsset skal lukkes ind, 
		/// selv om kørselsperioden er overskredet
		/// </summary>
		public bool KoerselsperiodeOverskredetSammedag { get; set; }

        /// <summary>
        /// Fornavn og efternavn på betaleren
        /// </summary>
        public string BetalerNavn { get; set; }		
		
		/// <summary>
		/// True hvis betaleren er godkendt
		/// </summary>
		public bool BetalerGodkendt { get; set; }
							
		/// <summary>
		/// True hvis betaleren er spærret.
		/// </summary>
		public bool BetalerSpaerret { get; set; }
						
		/// <summary>
		/// Angiver at næste vognlæs på denne anmeldelse, 
		/// skal udtages til stikprøvekontrol (dvs. Har en 
		/// planlagt sstikprøve)
		/// </summary>
		public bool UdtagNaesteLaesTilStikproeve { get; set; }	
					
		/// <summary>
		/// 1=Ren jord
		/// 2=Let forurenet jord
		/// </summary>	
		public int Jordtype { get; set; }

		/// <summary>
		/// Dato for hvornår kørsel starter
		/// </summary>
		public DateTime KoerselStartDato { get; set; }

		/// <summary>
		/// Dato for hvornår kørsel slutter
		/// </summary>
		public DateTime KoerselSlutDato { get; set; }

	}
}


