using System;
using System.Collections.Generic;
using System.ServiceModel;
using Ninject;
using Niras.Jordflytning.App_Start;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.Models.BomSystem;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning
{

	[ServiceContract]
	public interface IBomSystemService
	{
		[OperationContract]
		BomAnmeldelseList GetAnmeldelser(int modtagerAnlaegId, DateTime tidsStempel);

		[OperationContract]
		bool CreateVognlaes(IList<VognlaesBom> vognlaesList);

		[OperationContract]
		List<KoereToej> GetKoeretoejer(DateTime tidsStempel);
	}


	public class BomSystemService : IBomSystemService
	{
		private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.BomSystemService");
		private readonly IBomSystemBusiness _bomSystemBusiness;
		private readonly IKernel _ninjectKernel;

		/// <summary>
		/// Constructor
		/// </summary>
		public BomSystemService()
		{
			_ninjectKernel = NinjectWebCommon.CreateKernel();      
			_bomSystemBusiness = _ninjectKernel.Get<IBomSystemBusiness>();
		}

		/// <summary>
		/// Hent anmeldelser
		/// </summary>
		/// <param name="modtagerAnlaegId">Integer id på modtageranlægget</param>
		/// <param name="tidsStempel">DateTime.Now hos bomsystemet</param>
		/// <returns>Returnere med exception, hvis der er en fejl</returns>
		public BomAnmeldelseList GetAnmeldelser(int modtagerAnlaegId, DateTime tidsStempel)
		{
			BomAnmeldelseList list;
			try
			{
				list = _bomSystemBusiness.GetBomAnmeldelser(modtagerAnlaegId, tidsStempel);
			}
			catch (Exception exception)
			{
				list = new BomAnmeldelseList{ FejlBesked = exception.Message };
				Logger.LogException("GetAnmeldelser Failed for bomsystem. ModtagerAnlaegId:" + modtagerAnlaegId  , exception);
			}
			return list;
		}

		/// <summary>
		/// Funktion kaldes periodisk fx 4 gang i time med de vognlæs, 
		/// som er blevet valideret af bomsystemet. Hvis vognlæssene er 
		/// registreret returneres True og vognlæssene kan slettes i 
		/// bomsystemet.
		/// </summary>
		public bool CreateVognlaes(IList<VognlaesBom> vognlaesList)
		{
			var allIsOk = false;
			try
			{
				_bomSystemBusiness.CreateVognlaes(vognlaesList);
				allIsOk = true;
			}
			catch (Exception exception)
			{
				Logger.LogException("CreateVognlaes Failed. ", exception);
			}
			return allIsOk;
		}

		/// <summary>
		/// Giver de køretøjer der er kommet til siden sidste hentning
		/// </summary>
		/// <returns>Returnere null, hvis der er en fejl</returns>
		public List<KoereToej> GetKoeretoejer(DateTime tidsStempel)
		{
			List<KoereToej> list;
			try
			{
				list = _bomSystemBusiness.GetKoeretoejer(tidsStempel);
			}
			catch (Exception exception)
			{
				list = null;
				Logger.LogException("GetKoeretoejer Failed. ", exception);
			}
			return list;
		}
	}
}
