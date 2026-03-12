using System;

namespace Niras.Jordflytning.Core.Models.BomSystem
{
	public class VognlaesBom
	{
		/// <summary>
		///  Loebenummer
		/// </summary>
		public int AnmeldelsesId { get; set; }
		
		/// <summary>
		/// Dato og tid for, hvornår anmeldelsen bliver valideret.
		/// </summary>
		public DateTime Tid { get; set; }

		/// <summary>
		/// Lastbilens id
		/// </summary>
		public int LastbilId { get; set; }

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


