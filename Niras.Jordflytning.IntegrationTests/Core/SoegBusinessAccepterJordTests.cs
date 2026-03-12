using System;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.IntegrationTests.TestsSetup;

namespace Niras.Jordflytning.IntegrationTests.Core
{
	[TestFixture]
	public class SoegBusinessAccepterJordTests
	{
		// Ninject kernel
		private readonly IKernel _ninjectKernel;
		private readonly ISoegBusinessAccepterJord _business;

		private readonly Guid _brugerId = new Guid("00beb664-b1f0-443f-b5a5-339735af48c6"); // miljømedarbejder på prod


		public SoegBusinessAccepterJordTests()
		{
			// Init Ninject kernel
			_ninjectKernel = new StandardKernel();
			TestSetupUtil.InjectKodelists(_ninjectKernel);
			_ninjectKernel.Bind<ISoegBusinessAccepterJord>().To<SoegBusinessAccepterJord>();
			_business = _ninjectKernel.Get<ISoegBusinessAccepterJord>();
		}

		[Test]
		public void GetAnmeldelserTilAcceptTest()
		{
			// Arrange

			//Act
			var resultatList = _business.GetAnmeldelserTilAccept(_brugerId);

			//Assert 
			Assert.IsNotNull(resultatList);

			foreach (var anmeldelse in resultatList)
			{
				if (anmeldelse.AnmeldLoebeNr == 1206.ToString())
				{
					Assert.Fail("Fejl. Den skal ikke med.");
				}		
			}
		}

	}
}
