namespace Niras.Jordflytning.Core.Models.BomSystem
{

	public class KoereToej
	{
		/// <summary>
		/// Id på køretøjet
		/// </summary>
		public int KoeretoejsId { get; set; }
		
		/// <summary>
		/// Køretøjets fabrikat(mærke), fx Volvo, Mercedes etc.
		/// </summary>
		public string Fabrikat { get; set; }
		
		/// <summary>
		/// Køretøjets nummerplade (registreringsnummer)
		/// </summary>
		public string Nummerplade { get; set; }
		
		/// <summary>
		/// Køretøjets miljøklasse (Euro-1, Euro-2 etc.)
		/// </summary>
		public string Miljoeklasse { get; set; }

		/// <summary>
		/// Køretøjets Firma
		/// </summary>
		public string FirmaNavn { get; set; }

	}
}


