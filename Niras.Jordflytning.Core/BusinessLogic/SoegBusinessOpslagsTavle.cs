using System;
using System.Collections.Generic;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.SoegeResultat;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Core.BusinessLogic
{
  public class SoegBusinessOpslagsTavle : ISoegBusinessOpslagsTavle
  {
		private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.SoegBusinessOpslagsTavle");
	  private readonly IJordmodtagerBusiness _jordmodtagerBusiness;


		public SoegBusinessOpslagsTavle(IJordmodtagerBusiness jordmodtagerBusiness)
    {
			_jordmodtagerBusiness = jordmodtagerBusiness;
    }

		#region *** Public methods ***

		/// <summary>
		/// Hent opslaf
    /// </summary>
		public List<SoegeResultatOpslagsTavle> GetOpslag(Guid brugerId)
    {
      var startTime = DateTime.Now;

			var jordmodtagerList = _jordmodtagerBusiness.GetJordModtagerListForUser(brugerId);
			var opslagList = new List<BogholderOpslagstavle>();

			foreach (var jordmodtager in jordmodtagerList)
			{
				var opsl = jordmodtager.BogholderOpslagstavle;
				opslagList.AddRange(opsl);
			}

			var soegeResultatList = MapToSoegeResultatList(opslagList);
      var timeUsed = DateTime.Now - startTime;
			Logger.LogInfo("GetOpslag call time: " + timeUsed.TotalSeconds + " seconds.");
			return soegeResultatList;
    }

		#endregion *** Public methods ***


		#region *** Private methods ***

		private static List<SoegeResultatOpslagsTavle> MapToSoegeResultatList(List<BogholderOpslagstavle> opslagList)
	  {
		  var size = 0;
		  if (opslagList!=null)
		  {
			  size = opslagList.Count;
		  }
			var soegeResultatList = new List<SoegeResultatOpslagsTavle>(size);
	  
			if (opslagList != null)
			  foreach (var opslag in opslagList)
			  {
					var opslagsTavle = new SoegeResultatOpslagsTavle();
				  opslagsTavle.NoteId = opslag.Id;
				  opslagsTavle.BetalerId = opslag.BetalerId;
				  opslagsTavle.Besked = opslag.Tekst;
				  opslagsTavle.AendretAfId = opslag.Person.Id;
				  opslagsTavle.AendretAfNavn = opslag.Person.Efternavn + " " + opslag.Person.Efternavn;
					opslagsTavle.BetalerNavn = opslag.Betaler.Person.Efternavn + " " + opslag.Betaler.Person.Navn;
					opslagsTavle.DatoAendret = opslag.Tid.ToShortDateString();
					opslagsTavle.SenesteDato = opslag.Tid;


					soegeResultatList.Add(opslagsTavle);
			  }
			// sort by dato
			soegeResultatList = (soegeResultatList.OrderByDescending(res => res.SenesteDato)).ToList();
		  return soegeResultatList;
	  }

		#endregion *** Private methods ***

  }
}
