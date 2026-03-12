using System;
using System.Collections.Generic;

namespace Niras.Jordflytning.Core.Models.BomSystem
{
	public class BomAnmeldelseList
	{
		public BomAnmeldelseList()
		{
			AnmeldelseList = new List<BomAnmeldelse>();
		}

		/// <summary>
		/// Liste af anmeldelser
		/// </summary>
		public List<BomAnmeldelse> AnmeldelseList { get; set; }
		
		/// <summary>
		/// Tid for hentning
		/// </summary>
		public DateTime TimeStamp { get; set; }
		
		/// <summary>
		/// True hvis næste vognlæs skal udtages til kontrol
		/// Hvis de sidste 50 (antal kan variere for forskellige 
		/// modtageanlæg) vognlæs ikke har en stikprøve
		/// sættes denne til True.
		/// </summary>
		public bool Stikproeve { get; set; }

		/// <summary>
		/// Hvis der er sket en fejl sættes exception'en her.
		/// Ellers er den null.
		/// </summary>
		public string FejlBesked { get; set; }
	}
}


