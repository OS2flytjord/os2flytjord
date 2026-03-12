using System;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.IntegrationTests.TestsSetup;

namespace Niras.Jordflytning.IntegrationTests.Core
{
	[TestFixture]
	public class SoegBusinessStikproeveTests
	{
		// Ninject kernel
		private readonly IKernel _ninjectKernel;
		private readonly ISoegBusinessStikproeve _business;

		private readonly Guid _brugerId = new Guid("ec388bed-7884-452c-84dd-b7ccc29b69ca");
		private readonly Guid _userGuid = new Guid("3E093B56-D1CD-42A3-9594-D765AFD46646");

		public SoegBusinessStikproeveTests()
		{
			_ninjectKernel = new StandardKernel();
			TestSetupUtil.InjectKodelists(_ninjectKernel); 
			_ninjectKernel.Bind<ISoegBusinessStikproeve>().To<SoegBusinessStikproeve>();	
			_business = _ninjectKernel.Get<ISoegBusinessStikproeve>();
		}

		[Test]
		public void GetAktuelleStikproeveListTest()
		{
			// Arrange

			//Act
			var resultatList = _business.GetAktuelleStikproeveList(_userGuid);

			//Assert 
			Assert.IsNotNull(resultatList);

			foreach (var stikpr in resultatList)
			{
				if (stikpr.LoebeNummer==0)
					Assert.Fail("Der skal være LoebeNummer");
				if (stikpr.StikproeveId == Guid.Empty)
					Assert.Fail("Der skal være id");
			}
		}

	}
}
