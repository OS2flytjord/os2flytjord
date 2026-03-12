using System;
using System.Collections.Generic;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Infrastructure.DependencyResolution;

namespace Niras.Jordflytning.IntegrationTests.Core
{
	[TestFixture]
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

		[Test]
		public void BetalerSoegSoegByBetalerNavnTest()
		{
			// Arrange
			//Act
			var resultatList = _business.GetBetalereBy("", "kve", 0, 0, Guid.Empty);
			//Assert 
			Assert.IsNotNull(resultatList);
			Assert.AreEqual(1, resultatList.Count);
		}

		[Test]
		public void BetalerSoegSoegByKerneKundeTest()
		{
			// Arrange
			//Act
			var resultatList = _business.GetBetalereBy("", "", 0, 1, Guid.Empty);
			//Assert 
			Assert.IsNotNull(resultatList);
			Assert.AreEqual(1, resultatList.Count);
		}

		[Test]
		public void BetalerSoegTest()
		{
			// Arrange
			//Act
			var resultatList = _business.GetBetalereBy("", "", 0, 1, _brugerId);
			//Assert 
			Assert.IsNotNull(resultatList);
			Assert.AreEqual(2, resultatList.Count);
		}

		#endregion *** Betaler søgning ***



	}
}
