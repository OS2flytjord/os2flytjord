
namespace Niras.Jordflytning.Service.Model
{
	public class BomAnmeldelse
	{
		/// <summary>
		/// UniqueIndentifier (Alternativ string)
		/// </summary>
		public string AnmeldelsesId { get; set; }
		
		/// <summary>
		/// True hvis aktiv
		/// </summary>
		public bool Aktiv { get; set; }
		
		/// <summary>
		/// Adgangskode til bommen
		/// </summary>
		public int BomAdgangskode { get; set; }
		
		/// <summary>
		/// True hvis næste vognlæs skal udtages til kontrol
		/// </summary>
		public bool Stikproeve { get; set; }
		
		/// <summary>
		/// 1=Rent jord
		/// 2=Let forurenet jord
		/// </summary>	
		public int Jordkategori { get; set; }
	}
}


