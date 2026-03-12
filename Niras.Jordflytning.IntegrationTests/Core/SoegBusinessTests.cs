using System;
using System.Collections.Generic;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Infrastructure.DependencyResolution;
using Niras.Jordflytning.IntegrationTests.TestsSetup;

namespace Niras.Jordflytning.IntegrationTests.Core
{
	[TestFixture]
	public class SoegBusinessTests
	{
		// Ninject kernel
		private readonly IKernel _ninjectKernel;
		private readonly ISoegBusiness _business;
		private readonly Guid _brugerId = new Guid("ec388bed-7884-452c-84dd-b7ccc29b69ca");

		private readonly Guid _aarhusGuid = new Guid("a15d5888-bc70-4206-871e-a47a0c05decc");

		public SoegBusinessTests()
		{
			// Init Ninject kernel
			_ninjectKernel = new StandardKernel();
			TestSetupUtil.InjectKodelists(_ninjectKernel);

			_ninjectKernel.Bind<ISoegBusiness>().To<SoegBusiness>();
			_business = _ninjectKernel.Get<ISoegBusiness>();
		}

		[Test]
		public void GetFemsendteKommuneTest()
		{
			// Arrange

			//Act
			var resultatList = _business.GetFremSendteAnmeldelser(_aarhusGuid, true);

			//Assert 
			Assert.IsNotNull(resultatList);

			foreach (var anmeldelse in resultatList)
			{
				if (anmeldelse.SenesteStatusId == (decimal)EnumStatusAnmeldelse.Oprettet)
					Assert.Fail("Der må ikke være  oprettet i ikke fremsendtlisten");

				if (anmeldelse.SenesteStatus == "")
					Assert.Fail("Der må ikke være tomme statuser i listen");
			}
		}

		[Test]
		public void GetAnmeldelserDerKraeverHandlingTest()
		{
			// Arrange
			//Act
			var resultatList = _business.GetAnmeldelserDerKraeverHandling(_aarhusGuid);

			//Assert 
			Assert.IsNotNull(resultatList);

			foreach (var anmeldelse in resultatList)
			{
				if (anmeldelse.SenesteStatusId == (decimal)EnumStatusAnmeldelse.Oprettet)
					Assert.Fail("Der må ikke være  oprettet i ikke fremsendtlisten");

				if (anmeldelse.SenesteStatus == "")
					Assert.Fail("Der må ikke være tomme statuser i listen");
			}
		}

		[Test]
		public void GetAnmeldelserDerKraeverHandling2Test()
		{
			//Abildgade 10, burde komme med på listen "Kræver handling"
			//login på udvikling med tok@niras.dk. Kode 1234

			// Arrange
			//Act
			var resultatList = _business.GetAnmeldelserDerKraeverHandling(_aarhusGuid);

			//Assert 
			Assert.IsNotNull(resultatList);

			foreach (var anmeldelse in resultatList)
			{
				if (anmeldelse.SenesteStatusId == (decimal) EnumStatusAnmeldelse.Oprettet)
					Assert.Fail("Der må ikke være  oprettet i ikke fremsendtlisten");

				if (anmeldelse.SenesteStatus == "")
					Assert.Fail("Der må ikke være tomme statuser i listen");
			}
		}


		[Test]
		public void GetMineAnnmeldelserTest()
		{
			// 94bdf401-f2c4-4243-b5d7-2ffd3701f5ee

			// tom 
			// 9c368a61-dfdc-48cc-9390-7880972591d3

			// Arrange
			var brugerGuid = new Guid("EC388BED-7884-452C-84DD-B7CCC29B69CA");
			//Act
			var resultatList = _business.GetMineAnmeldelser(brugerGuid);

			//Assert 
			Assert.IsNotNull(resultatList);

			foreach (var anmeldelse in resultatList)
			{
				if (anmeldelse.SenesteStatusId == (decimal)EnumStatusAnmeldelse.Oprettet)
					Assert.Fail("Der må ikke være  oprettet i ikke fremsendtlisten");

				if (anmeldelse.SenesteStatus == "")
					Assert.Fail("Der må ikke være tomme statuser i listen");
			}
		}


		[Test]
		public void GetMineAnnmeldelser2Test()
		{
			// tom 
			// 9c368a61-dfdc-48cc-9390-7880972591d3

			// Arrange
			var brugerGuid = new Guid("9c368a61-dfdc-48cc-9390-7880972591d3");
			//Act
			var resultatList = _business.GetMineAnmeldelser(brugerGuid);

			// anmeldelse der burde være der:
			//94bdf401-f2c4-4243-b5d7-2ffd3701f5ee

			//Assert 
			Assert.IsNotNull(resultatList);

			foreach (var anmeldelse in resultatList)
			{
				if (anmeldelse.SenesteStatusId == (decimal)EnumStatusAnmeldelse.Oprettet)
					Assert.Fail("Der må ikke være  oprettet i ikke fremsendtlisten");

				if (anmeldelse.SenesteStatus == "")
					Assert.Fail("Der må ikke være tomme statuser i listen");
			}
		}


		[Test]
		public void GetaktiveTest()
		{
			// Arrange
			var brugerId = _brugerId;
			//Act
			var resultatList = _business.GetAktiveAnmeldelser(brugerId, false);

			//Assert 
			Assert.IsNotNull(resultatList);

			foreach (var anmeldelse in resultatList)
			{
				//Console.WriteLine(anmeldelse.AnmeldelseId + ", " + anmeldelse.SenesteStatusId + ", " + anmeldelse.SenesteStatus + ", "+ anmeldelse.Jordmodtager);
				if (anmeldelse.SenesteStatusId != (int)EnumStatusAnmeldelse.GodkendtAfKommunen)
					if (anmeldelse.SenesteStatusId != (int)EnumStatusAnmeldelse.JordmodtagerAcceptererJorden)
						Assert.Fail("Uha-da. Den skal være godkendt af alle.");
			}
		}

		[Test]
		public void GetIkkeFemsendteTest()
		{
			// Arrange

			//Act
			var resultatList = _business.GetIkkeFremSendteAnmeldelser(_brugerId, false);

			//Assert 
			Assert.IsNotNull(resultatList);

			foreach (var anmeldelse in resultatList)
			{
				//Console.WriteLine(anmeldelse.AnmeldelseId + ", " + anmeldelse .SenesteStatusId + ", " + anmeldelse.SenesteStatus + ", "+ anmeldelse.Jordmodtager);
				if (anmeldelse.SenesteStatusId == (decimal)EnumStatusAnmeldelse.Afsendt)
					Assert.Fail("Der må ikke være afsendet i ikke fremsendtlisten");
				if (anmeldelse.SenesteStatusId == (decimal)EnumStatusAnmeldelse.GodkendtAfKommunen)
					Assert.Fail("Der må ikke være GodkendtAfKommune i ikke fremsendtlisten");
			}
		}

		[Test]
		public void GetAfsluttedeTest()
		{
			// Arrange

			//Act
			var resultatList = _business.GetAfsluttedeAnmeldelser(_brugerId);

			//Assert 
			Assert.IsNotNull(resultatList);

			foreach (var anmeldelse in resultatList)
			{
				//Console.WriteLine(anmeldelse.AnmeldelseId + ", " + anmeldelse.SenesteStatusId + ", " + anmeldelse.SenesteStatus + ", " + anmeldelse.Jordmodtager);

				if (anmeldelse.SenesteStatusId != (decimal)EnumStatusAnmeldelse.Afsluttet)
					Assert.Fail("Der må ikke være andet end Afsluttet i ikke fremsendtlisten");

				if (anmeldelse.SenesteStatus == "")
					Assert.Fail("Der må ikke være tomme statuser i listen");
			}
		}

		[Test]
		public void GetFremsendteTest()
		{
			// Arrange
			//Act
			var resultatList = _business.GetFremSendteAnmeldelser(_brugerId, false);

			//Assert 
			Assert.IsNotNull(resultatList);

			foreach (var anmeldelse in resultatList)
			{
				Console.WriteLine(anmeldelse.AnmeldelseId + ", " + anmeldelse.SenesteStatusId + ", " + anmeldelse.SenesteStatus + ", " + anmeldelse.Jordmodtager);

				if (anmeldelse.SenesteStatusId == (decimal)EnumStatusAnmeldelse.Oprettet)
					Assert.Fail("Der må ikke være  oprettet i ikke fremsendtlisten");

				if (anmeldelse.SenesteStatus == "")
					Assert.Fail("Der må ikke være tomme statuser i listen");
			}
		}

		[Test]
		public void GetallTest()
		{
			// Arrange

			//Act
			//var resultatList = _business.GetAllAnmeldelser(_brugerId);

			////Assert 
			//Assert.IsNotNull(resultatList);

			//foreach (var anmeldelse in resultatList)
			//{
			//	Console.WriteLine(anmeldelse.AnmeldelseId + ", " + anmeldelse.SenesteStatusId + ", " + anmeldelse.SenesteStatus + ", " + anmeldelse.Jordmodtager);
			//}
		}



		[Test]
		public void SoegByKunMine()
		{
			// Arrange

			//Act
			var resultatList = _business.GetAnmeldelserBy(null, null, null, null, null, null, null, _brugerId,  true, false);
			//Assert 
			Assert.IsNotNull(resultatList);
			Assert.Greater(resultatList.Count, 1, "Der burde være mere end een.");
		}


		[Test]
		public void SoegByLoebeNrTest()
		{
			// Arrange
			//Act
			var resultatList = _business.GetAnmeldelserBy(1000, "", null, null, null, null, null, _brugerId,  true, false);
			//Assert 
			Assert.IsNotNull(resultatList);
			Assert.AreEqual(1, resultatList.Count);
		}

		[Test]
		public void SoegByAdresseTest()
		{
			// Arrange
			const string adresse = "Baun";
			//Act
			var resultatList = _business.GetAnmeldelserBy(null, adresse, null, null, null, null, null, _brugerId, true, false);
			//Assert 
			Assert.IsNotNull(resultatList);
			foreach (var soegeResultat in resultatList)
			{
				if (!soegeResultat.OprindelsesSted.Contains(adresse))
				{
					Assert.Fail("Søgeresultatet indeholder ikke søgestrengen");
				}
			}
			Assert.Greater(resultatList.Count, 1, "Der burde være mere end een.");
		}


		[Test]
		public void SoegByFoerDateTest()
		{
			// Arrange
			var date = DateTime.Now;
			//Act
			var resultatList = _business.GetAnmeldelserBy(null, "", date, null, null, null, null, _brugerId, true, false);
			//Assert 
			Assert.IsNotNull(resultatList);
			foreach (var soegeResultat in resultatList)
			{
				Console.WriteLine(soegeResultat.AnmeldelseId + ", " + soegeResultat.SenesteStatusId + ", " + soegeResultat.SenesteStatus + ", " + soegeResultat.Jordmodtager);
			}
			Assert.Greater(resultatList.Count, 1, "Der burde være mere end een.");
		}

		[Test]
		public void SoegByFoerDateTest2()
		{
			// Arrange
			var date = DateTime.Now;
			date = date.AddMonths(-10);
			//Act
			var resultatList = _business.GetAnmeldelserBy(null, "", date, null, null, null, null, _brugerId, true, false);
			//Assert 
			Assert.IsNotNull(resultatList);
			foreach (var soegeResultat in resultatList)
			{
				Console.WriteLine(soegeResultat.AnmeldelseId + ", " + soegeResultat.SenesteStatusId + ", " + soegeResultat.SenesteStatus + ", " + soegeResultat.Jordmodtager);
			}
			Assert.AreEqual(resultatList.Count, 0, "Der burde ikke være nogen");
		}

		[Test]
		public void SoegByEfterDateTest()
		{
			// Arrange
			var date = DateTime.Now;
			date = date.AddMonths(-10);
			//Act
			var resultatList = _business.GetAnmeldelserBy(null, "", null, date, null, null, null, _brugerId, true, false);
			//Assert 
			Assert.IsNotNull(resultatList);
			foreach (var soegeResultat in resultatList)
			{
				Console.WriteLine(soegeResultat.AnmeldelseId + ", " + soegeResultat.SenesteStatusId + ", " + soegeResultat.SenesteStatus + ", " + soegeResultat.Jordmodtager);
			}
			Assert.Greater(resultatList.Count, 1, "Der burde være mere end een.");
		}

		// løbenummer
		//Oprindelsessted
		// før
		// efter

		//Modtager
		//Transportør
		// anmelder


		[Test]
		[Ignore]
		public void GetaktiveNotExistingUserTest()
		{
			// Arrange
			var brugerId = new Guid();
			//Act
			var anmeldList = _business.GetAktiveAnmeldelser(brugerId, false);

			//Assert 
			// Assert.IsNotNull(anmeldList);
			// Assert.AreEqual(0, anmeldList.Count, "Der må ikke komme et resultat når det er en ukendt bruger");
		}

		[Test]
		[Ignore]
		public void GetaktiveDublicatesTest()
		{
			// Arrange
			var brugerId = new Guid("ec388bed-7884-452c-84dd-b7ccc29b69ca");

			//Act
			//var anmeldList = _business.GetAllAnmeldelser(brugerId);

			////Assert 
			//Assert.IsNotNull(anmeldList);

			//foreach (var anmeldelse in anmeldList)
			//{
			//		var counter = 1;
			//		foreach (var anmeldelseChck in anmeldList)
			//		{
			//				if (anmeldelse.Id == anmeldelseChck.Id)
			//				{
			//						if (counter > 1)
			//						{
			//								Assert.Fail("Der er anmeldelse dubletter i listen");
			//						}             
			//						counter++;
			//				}
			//		}
			//}
		}

	}
}
