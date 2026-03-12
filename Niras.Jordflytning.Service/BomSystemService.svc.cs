using System;
using System.Collections.Generic;
using System.ServiceModel;
using Niras.Jordflytning.Library.Logging;
using Niras.Jordflytning.Service.Control;
using Niras.Jordflytning.Service.Model;

namespace Niras.Jordflytning.Service
{

	[ServiceContract]
	public interface IBomSystemService
	{
		[OperationContract]
		IList<BomAnmeldelse> GetAnmeldelser(string jordmodtagerAnlaegId, DateTime tidsStempel);

		[OperationContract]
		bool CreateVognlaes(IList<Vognlaes> vognlaesList );

		[OperationContract]
		int CreateStikproeve(Vognlaes vognlaes);
	}


	public class BomSystemService : IBomSystemService
	{
		private static readonly ILogger logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Service");


		/// <summary>
		/// Hent anmeldelser
		/// Modtageranlægid: (tekst string, 36 tegn)
		/// Tid (DateTime) 
		/// </summary>
		public IList<BomAnmeldelse> GetAnmeldelser(string jordmodtagerAnlaegId, DateTime tidsStempel )
		{
			var list = new List<BomAnmeldelse>();
			try
			{
				var control = new BomSystemControl();
				var retVal = control.GetBomAnmeldelser(jordmodtagerAnlaegId, tidsStempel);
				if (retVal!=null)
					list = retVal;
			}
			catch (Exception exception)
			{
				logger.LogException("GetAnmeldelser Failed. ", exception);
			}
			return list;
		}


		/// <summary>
		/// Funktion kaldes periodisk fx 4 gang i time med de vognlæs, 
		/// som er blevet valideret af bomsystemet. Hvis vognlæssene er 
		/// registreret returneres True og kan vognlæssene slettes i 
		/// bomsystemet.
		/// </summary>
		public bool CreateVognlaes(IList<Vognlaes> vognlaesList)
		{
			var allIsOk = false;
			try
			{
				var control = new BomSystemControl();
				var retVal = control.CreateVognlaes(vognlaesList);
				allIsOk = retVal;
			}
			catch (Exception exception)
			{
				logger.LogException("CreateVognlaes Failed. ", exception);
			}
			return allIsOk;
		}


		/// <summary>
		/// Når et vognlæs ved bommen er blevet udtaget til stikprøvekontrol 
		/// skal dette registreres i Jordflytning 2.0. 
		/// </summary>
		/// <param name="vognlaes"></param>
		/// <returns>Bås nummer</returns>
		public int CreateStikproeve(Vognlaes vognlaes)
		{
			var baasNr = 0;
			try
			{
				var control = new BomSystemControl();
				var retVal = control.CreateStikproeve(vognlaes);
				baasNr = retVal;
			}
			catch (Exception exception)
			{
				logger.LogException("CreateStikproeve Failed. ", exception);
			}

			return baasNr;
		}
	}

}
