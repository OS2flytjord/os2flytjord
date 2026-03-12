using System;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.IntegrationTests.TestsSetup;

namespace Niras.Jordflytning.IntegrationTests.Core
{
	[TestFixture]
	public class AlarmBusinessTest
	{
		// Ninject kernel
		private readonly IKernel _ninjectKernel;
		private readonly IAlarmBusiness _alarmBusiness;
		private readonly IAnmeldelserBusiness _anmeldelserBusiness;

		// anmeldelsenummer 1885
		//private readonly Guid _anmeldelseGuid = new Guid("F4850722-5F29-4E7D-930C-DA610D0F8C64");
    
    private readonly Guid _anmeldelseGuid = new Guid("580618E0-C248-46E4-AB8D-42B6FC48E7F4");
    

		public AlarmBusinessTest()
		{
			_ninjectKernel = new StandardKernel();
			TestSetupUtil.InjectKodelists(_ninjectKernel);
			_alarmBusiness = _ninjectKernel.Get<IAlarmBusiness>();
			_anmeldelserBusiness = _ninjectKernel.Get<IAnmeldelserBusiness>();

		}

		[Test]
		public void CreateAlarmMaengdeKoertJordTest()
		{
			// Arrange
			// get anmeldelse med en alarm
			var anmeldelse = _anmeldelserBusiness.Read(_anmeldelseGuid);
			//Act
			_alarmBusiness.SendKoertJordAlarmer(anmeldelse);

			//Assert 
			Assert.IsEmpty("");
		}


	}
}
