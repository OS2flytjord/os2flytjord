using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.SoegeResultat;
using Niras.Jordflytning.Library.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Niras.Jordflytning.Core.BusinessLogic
{
    public class SoegBusinessAccepterJord : ISoegBusinessAccepterJord
  {
		private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogic.SoegBusinessAccepterJord");
		private readonly IAnmeldelserBusiness _anmeldelserBusiness;
		private readonly IStatusAnmeldelseBusiness _statusAnmeldelseBusiness;

		public SoegBusinessAccepterJord(IAnmeldelserBusiness anmeldelserBusiness, IStatusAnmeldelseBusiness statusAnmeldelseBusiness)
    {
			_anmeldelserBusiness = anmeldelserBusiness;
			_statusAnmeldelseBusiness = statusAnmeldelseBusiness;
    }

		#region *** Public methods ***

		/// <summary>
		/// Hent Anmeldelser der er klar til accept eller afvisning
    /// </summary>
		public List<SoegeResultatAccepterJord> GetAnmeldelserTilAccept(Guid userId)
    {
      var startTime = DateTime.Now;

			var anmeldelsesList = _anmeldelserBusiness.GetUsersAnmeldelser(userId);
      Logger.LogInfo("GetAnmeldelserTilAccept split 1 time: " + (DateTime.Now - startTime).TotalSeconds + " seconds.");
			var finalList = new List<Anmeldelse>();
			foreach (var anmeldelse in anmeldelsesList)
			{
				var latestJordmodtagerStatus = _statusAnmeldelseBusiness.GetLastStatusTypeForJordmodtager(anmeldelse.AnmeldelseEffektivStatus);
				if (latestJordmodtagerStatus != EnumStatusAnmeldelse.Ukendt) 
					continue;

				var latestKommuneStatus = _statusAnmeldelseBusiness.GetLastStatusTypeForKommune(anmeldelse.AnmeldelseEffektivStatus);
				if (latestKommuneStatus == EnumStatusAnmeldelse.Ukendt || latestKommuneStatus != EnumStatusAnmeldelse.GodkendtAfKommunen) 
					continue;

				var latestBetalerStatus = _statusAnmeldelseBusiness.GetLastStatusTypeForBetaler(anmeldelse.AnmeldelseEffektivStatus);
				if (latestBetalerStatus != EnumStatusAnmeldelse.Ukendt && latestBetalerStatus == EnumStatusAnmeldelse.BetalerAccepteretBetalingen)
					finalList.Add(anmeldelse);

            }
      Logger.LogInfo("GetAnmeldelserTilAccept split 2 time: " + (DateTime.Now - startTime).TotalSeconds+ " seconds.");
			var soegeResultatList = MapToSoegeResultatList(finalList);

      var timeUsed = DateTime.Now - startTime;
			Logger.LogInfo("GetAnmeldelserTilAccept call time: " + timeUsed.TotalSeconds + " seconds.");
			return soegeResultatList;
    }

		#endregion *** Public methods ***


		#region *** Private methods ***

	  private List<SoegeResultatAccepterJord> MapToSoegeResultatList(List<Anmeldelse> anmeldelsesList)
	  {
		  var size = 0;
		  if (anmeldelsesList != null)
		  {
			  size = anmeldelsesList.Count;
		  }
		  var soegeResultatList = new List<SoegeResultatAccepterJord>(size);

		  if (anmeldelsesList != null)
			  foreach (var anmeld in anmeldelsesList)
			  {
				  var soegeResultat = new SoegeResultatAccepterJord();

				  soegeResultat.AnmeldelseId = anmeld.Id;
				  soegeResultat.AnmeldLoebeNr = anmeld.Nummer.ToString(CultureInfo.InvariantCulture);

				  if (anmeld.BetalerId != null)
					  soegeResultat.BetalerId = (Guid) anmeld.BetalerId;

				  if (anmeld.Oprindelsessted != null)
					  soegeResultat.OprindelsesSted = 
							anmeld.Oprindelsessted.Adresse + ", " + 
							anmeld.Oprindelsessted.Postnummer + " " + 
							anmeld.Oprindelsessted.PostDistrikt;

				  if (anmeld.Jord != null && anmeld.Jord.ForventetJordmaengdeTon != null)
					  soegeResultat.JordMaengde = anmeld.Jord.ForventetJordmaengdeTon.ToString();

				  soegeResultat.KoerselsPeriode = anmeld.KoerselStartSlut;

				  if (anmeld.Anmelder != null && anmeld.Anmelder.Person != null)
					  soegeResultat.AnmelderNavn = anmeld.Anmelder.Person.Navn + " " + anmeld.Anmelder.Person.Efternavn;

				  if (anmeld.Betaler != null && anmeld.Betaler.Person != null)
					  soegeResultat.BetalerNavn = anmeld.Betaler.Person.Navn + " " + anmeld.Betaler.Person.Efternavn;

					if (_statusAnmeldelseBusiness.GetLastStatus(anmeld.AnmeldelseEffektivStatus, out _, out var statTid))
					  soegeResultat.SenesteStatusDato = statTid;

					soegeResultat.Status = _statusAnmeldelseBusiness.GetFormattedStatusListForGrids(anmeld);

				  soegeResultatList.Add(soegeResultat);
			  }

		  // sort by dato
		  soegeResultatList = (soegeResultatList.OrderByDescending(res => res.SenesteStatusDato)).ToList();

		  return soegeResultatList;
	  }




	  #endregion *** Private methods ***
	}
}
