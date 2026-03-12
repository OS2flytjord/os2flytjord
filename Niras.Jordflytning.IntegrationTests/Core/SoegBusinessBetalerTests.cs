using System;
using System.Collections.Generic;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Infrastructure.DependencyResolution;

namespace Niras.Jordflytning.IntegrationTests.Core
{
	[TestFixture]
	public class SoegBusinessBetalerTests
	{
		// Ninject kernel
		private readonly IKernel _ninjectKernel;
		private readonly ISoegBusinessBetaler _business;

		private readonly Guid _brugerId = new Guid("ec388bed-7884-452c-84dd-b7ccc29b69ca");
		private readonly Guid _aarhusGuid = new Guid("a15d5888-bc70-4206-871e-a47a0c05decc");

		public SoegBusinessBetalerTests()
		{
			// Init Ninject kernel
			_ninjectKernel = new StandardKernel(new ConfigModule(), new RepositoryModule());
			_ninjectKernel.Bind<ISoegBusinessBetaler>().To<SoegBusinessBetaler>();
			_business = _ninjectKernel.Get<ISoegBusinessBetaler>();
		}



		[Test]
		public void GetBetalerSomKrvHandlingTest()
		{
			// Arrange
			var betalerGuid = new Guid("EC388BED-7884-452C-84DD-B7CCC29B69CA");
			var jordModtagerGuid = new Guid("92D7D7F1-A74A-4586-A01B-A19C575915F2");

			var userGuid = new Guid("3E093B56-D1CD-42A3-9594-D765AFD46646");



			//Act
			var resultatList = _business.GetBetalerSomKrvHandling(userGuid);

			//Assert 
			Assert.IsNotNull(resultatList);

			foreach (var betaler in resultatList)
			{
				if (String.IsNullOrEmpty(betaler.FirmaNavn))
					Assert.Fail("Der skal være firmanavn");	
			
				if (String.IsNullOrEmpty(betaler.BetalerNavn))
					Assert.Fail("Der skal være betalernavn");		
		
				if (String.IsNullOrEmpty(betaler.Dato))
					Assert.Fail("Der skal være dato");			
	
				if (betaler.BetalerId == Guid.Empty)
					Assert.Fail("Der skal være id");

			}
		}


	}
}
