using System;
using System.Collections.Generic;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.IntegrationTests.TestsSetup.DependencyResolution;
using Niras.Jordflytning.IntegrationTests.MockData;

namespace Niras.Jordflytning.IntegrationTests.Core
{
	[TestFixture]
	[Niras.Jordflytning.IntegrationTests.TestsSetup.RulTilbageEfterTest]
	public class SoegAdminBusinessTests
	{
		// Ninject kernel
		private readonly IKernel _ninjectKernel;
		private readonly ISoegAdminBusiness _business;
		private readonly Guid _brugerId = new Guid("ec388bed-7884-452c-84dd-b7ccc29b69ca");

		private readonly Guid _aarhusGuid = new Guid("a15d5888-bc70-4206-871e-a47a0c05decc");

		public SoegAdminBusinessTests()
		{
			// Init Ninject kernel
			_ninjectKernel = new StandardKernel(new ConfigModule(), new RepositoryModule());
			_ninjectKernel.Bind<ISoegAdminBusiness>().To<SoegAdminBusiness>();
			_business = _ninjectKernel.Get<ISoegAdminBusiness>();
		}


		#region *** Betaler søgning ***

		// Betalersøgningen viser kun betalere hos brugerens egne jordmodtagere, så der søges som jordmodtagerens
		// administrator. Data: MockData\BetalereMock (Anne Anmelder og Bo Kollega er kernekunder, Tina Transport er ikke).

		[Test]
		public void BetalerSoegSoegByBetalerNavnTest()
		{
			//Act
			var resultatList = _business.GetBetalereBy("", "anmelder", 0, 0, Testdata.JordmodtagerAdminId);
			//Assert
			Assert.IsNotNull(resultatList);
			Assert.AreEqual(1, resultatList.Count);
		}

		[Test]
		public void BetalerSoegSoegByKerneKundeTest()
		{
			//Act - kerneKunde 2 = ikke kernekunde
			var resultatList = _business.GetBetalereBy("", "", 0, 2, Testdata.JordmodtagerAdminId);
			//Assert
			Assert.IsNotNull(resultatList);
			Assert.AreEqual(1, resultatList.Count);
		}

		[Test]
		public void BetalerSoegTest()
		{
			//Act - kerneKunde 1 = kernekunde
			var resultatList = _business.GetBetalereBy("", "", 0, 1, Testdata.JordmodtagerAdminId);
			//Assert
			Assert.IsNotNull(resultatList);
			Assert.AreEqual(2, resultatList.Count);
		}

		[Test]
		public void BetalerSoegUdenJordmodtagerTest()
		{
			//Act - anmelderen er ikke tilknyttet en jordmodtager og ser derfor ingen betalere
			var resultatList = _business.GetBetalereBy("", "", 0, 0, _brugerId);
			//Assert
			Assert.IsNotNull(resultatList);
			Assert.AreEqual(0, resultatList.Count);
		}

		#endregion *** Betaler søgning ***



	}
}
