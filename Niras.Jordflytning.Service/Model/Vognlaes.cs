using System;

namespace Niras.Jordflytning.Service.Model
{
	public class Vognlaes
	{
		/// <summary>
		/// UniqueIndentifier (Alternativ string)
		/// </summary>
		public string AnmeldelsesId { get; set; }
		
		/// <summary>
		/// Dato og tid for, hvornår anmeldelsen bliver valideret.
		/// </summary>
		public DateTime Tid { get; set; }
		
		/// <summary>
		/// Jordmængden angivet i antal akseler
		/// </summary>
		public int JordmaengdeAksler { get; set; }
		
		/// <summary>
		/// True hvis næste vognlæs skal udtages til kontrol
		/// </summary>
		public bool Stikproeve { get; set; }
		
		/// <summary>
		/// Nummeret på båsen hvori jorden aflæses i. 
		/// Null hvis der ikke er registre-ret en 
		/// stikprøve på vognlæsset.
		/// </summary>	
		public int? StikproeveBaas { get; set; }
	}
}


