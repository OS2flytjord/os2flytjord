using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.IntegrationTests.MockData;
using Niras.Jordflytning.IntegrationTests.TestsSetup;

namespace Niras.Jordflytning.IntegrationTests.Core
{
	[TestFixture]
	[Niras.Jordflytning.IntegrationTests.TestsSetup.RulTilbageEfterTest]
	public class AlarmBusinessTest
	{
		// Ninject kernel
		private IKernel _ninjectKernel;
		private IAlarmBusiness _alarmBusiness;
		private IAnmeldelserBusiness _anmeldelserBusiness;
		private FakeMailBusiness _mail;

		[SetUp]
		public void Setup()
		{
			// Ny kernel (og DbContext) pr. test - testens ændringer rulles tilbage efter hver test.
			_mail = new FakeMailBusiness();
			_ninjectKernel = new StandardKernel();
			_ninjectKernel.Bind<IMailBusiness>().ToConstant(_mail);
			TestSetupUtil.InjectKodelists(_ninjectKernel);
			_alarmBusiness = _ninjectKernel.Get<IAlarmBusiness>();
			_anmeldelserBusiness = _ninjectKernel.Get<IAnmeldelserBusiness>();
		}

		[TearDown]
		public void TearDown()
		{
			_ninjectKernel.Dispose();
		}

		[Test]
		public void CreateAlarmMaengdeKoertJordTest()
		{
			// Arrange - MockData\KoertJordMock: 30 af 100 ton kørt, alarm til anmelderen ved 25 %
			var anmeldelse = _anmeldelserBusiness.Read(Testdata.AnmeldelseAktivId);
			Assert.IsNotNull(anmeldelse);
			var alarm = anmeldelse.Alarm.Single(a => a.Id == Testdata.AlarmId);
			Assert.IsNull(alarm.Udfoert, "Alarmen må ikke være sendt på forhånd");

			//Act
			_alarmBusiness.SendKoertJordAlarmer(anmeldelse);

			//Assert
			CollectionAssert.AreEqual(new[] { Testdata.AnmelderEmail }, _mail.Modtagere, "Alarmen skal sendes til anmelderen");
			Assert.IsNotNull(alarm.Udfoert, "Alarmen skal markeres som sendt");
		}

		[Test]
		public void AlarmSendesIkkeIgenNaarDenErUdfoert()
		{
			var anmeldelse = _anmeldelserBusiness.Read(Testdata.AnmeldelseAktivId);
			anmeldelse.Alarm.Single(a => a.Id == Testdata.AlarmId).Udfoert = DateTime.Now.AddDays(-1);

			_alarmBusiness.SendKoertJordAlarmer(anmeldelse);

			CollectionAssert.IsEmpty(_mail.Modtagere);
		}

		private class FakeMailBusiness : IMailBusiness
		{
			public readonly List<string> Modtagere = new List<string>();

			public void SendEmail(string emailSubject, string emailContent, string emailTo)
			{
				Modtagere.Add(emailTo);
			}
		}
	}
}
