using System;
using System.Collections.Generic;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.SoegeResultat;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Core.BusinessLogic
{
  public class SoegBusinessBetaler : ISoegBusinessBetaler
  {
		private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.SoegBusinessBetaler");
		private readonly IStatusBetalerRepository _statusBetalerRepository;
		private readonly IPersonJordmodtagerRepository _personJordmodtagerRepository;


		public SoegBusinessBetaler(IStatusBetalerRepository statusBetalerRepository, IPersonJordmodtagerRepository personJordmodtagerRepository)
    {
			_statusBetalerRepository = statusBetalerRepository;
			_personJordmodtagerRepository = personJordmodtagerRepository;
    }

		#region *** Public methods ***

		/// <summary>
		/// Hent alle Betalerer som kræver handling
    /// </summary>
		public List<SoegeResultatBetaler> GetBetalerSomKrvHandling(Guid userId)
    {
      var startTime = DateTime.Now;

			var soegeResultatList = new List<SoegeResultatBetaler>();

			// hent alle jordmodtagerfirmaer, der er knyttet til denne person	
      //var jordModtagerList = (from j in _personJordmodtagerRepository.Read()
      //                       where j.PersonId == userId
      //                         select j).ToList();
      var jordModtagerList = (from j in _personJordmodtagerRepository.Search(x=>x.PersonId == userId) 
                              select j).ToList();
			
			//var guid = jordModtagerList.FirstOrDefault().JordmodtagerId;
			//var jordModtagerGuid = new Guid("92D7D7F1-A74A-4586-A01B-A19C575915F2");

			// hent alle betalere som er knyttet til jordmodtagerfirmaet
			if (jordModtagerList.Any())
			{
				var betalereList = new List<StatusBetaler>();

				foreach (var personJordmodtager in jordModtagerList)
				{
					var guid = personJordmodtager.JordmodtagerId;

					var list =
						from s in _statusBetalerRepository.Search(x=> x.JordmodtagerId == guid)
						select s;

					betalereList.AddRange(list);
				}

				var finalList = new List<StatusBetaler>();
				foreach (var statusBetaler in betalereList)
				{
					if (statusBetaler.Godkendt == null)
					{
						finalList.Add(statusBetaler);
					}
				}
				soegeResultatList = MapToSoegeResultatList(finalList);
			}

			var timeUsed = DateTime.Now - startTime;
			Logger.LogInfo("GetBetalerSomKrvHandling call time: " + timeUsed.TotalSeconds + " seconds.");
			return soegeResultatList;
    }

		#endregion *** Public methods ***


		#region *** Private methods ***

		private static List<SoegeResultatBetaler> MapToSoegeResultatList(List<StatusBetaler> statusBetalerList)
	  {
		  var size = 0;
		  if (statusBetalerList!=null)
		  {
			  size = statusBetalerList.Count;
		  }
			var soegeResultatList = new List<SoegeResultatBetaler>(size);

			if (statusBetalerList != null)
			  foreach (var statusBetaler in statusBetalerList)
			  {
					var soegeResultat = new SoegeResultatBetaler();
				  
					soegeResultat.JordmodtagerId = statusBetaler.JordmodtagerId;
					soegeResultat.Dato = statusBetaler.Redigeret.ToShortDateString();
				  soegeResultat.Bemaerkning = statusBetaler.Bemaerkning;

          if (statusBetaler.Betaler!=null && statusBetaler.Betaler.Person != null)
				  {
					  if (statusBetaler.BetalerId != null) 
							soegeResultat.BetalerId = (Guid) statusBetaler.BetalerId;
					  soegeResultat.BetalerNavn = statusBetaler.Betaler.Person.Navn + " " + statusBetaler.Betaler.Person.Efternavn;
						if (statusBetaler.Betaler.Person.Firmaoplysninger != null)
					  {
						  soegeResultat.FirmaNavn = statusBetaler.Betaler.Person.Firmaoplysninger.Firmanavn;
					  }				
				  }
				  soegeResultatList.Add(soegeResultat);
			  }
			soegeResultatList = (soegeResultatList.OrderBy(res => res.BetalerNavn)).ToList();
		  return soegeResultatList;
	  }


		#endregion *** Private methods ***
	}
}
