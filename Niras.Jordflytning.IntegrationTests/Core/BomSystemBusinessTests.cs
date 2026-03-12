using System;
using System.Collections.Generic;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.Models.BomSystem;
using Niras.Jordflytning.IntegrationTests.TestsSetup;

namespace Niras.Jordflytning.IntegrationTests.Core
{
	[TestFixture]
	public class BomSystemBusinessTests
	{
		// Ninject kernel
		private readonly IKernel _ninjectKernel;
		private readonly IBomSystemBusiness _business;

		public BomSystemBusinessTests()
		{
			_ninjectKernel = new StandardKernel();
			TestSetupUtil.InjectKodelists(_ninjectKernel);
			_business = _ninjectKernel.Get<IBomSystemBusiness>();
		}

		[Test]
		public void GetBomAnmeldelserTest()
		{
			// Arrange
			var date = DateTime.Now;
			const int modtageAnlaegId = 1142;

			//Act
			var result = _business.GetBomAnmeldelser(modtageAnlaegId, date.AddDays(-40));

			//Assert 
			Assert.IsNotNull(result);

			foreach (var anmeldelse in result.AnmeldelseList)
			{

				if (anmeldelse.Anmeldelseslbrnr == 1206)
				{
					//var tt = 0;
				}

				if (anmeldelse.BetalerGodkendt)
				{
					//var tt = 3;
				}
				if (anmeldelse.BetalerSpaerret)
				{
					//var tt = 3;
				}
				if (anmeldelse.Anmeldelseslbrnr==0)
					Assert.Fail("Der skal være LoebeNummer");
				if (anmeldelse.JordmodtageranlægId == 0)
					Assert.Fail("Der skal være modtageAnlæg id");
			}
		}

		[Test]
		public void GetGetKoeretoejerTest()
		{
			// Arrange
			var date = DateTime.Now;

			//Act
			var result = _business.GetKoeretoejer(date.AddDays(-1000));

			//Assert 
			Assert.IsNotNull(result);

			foreach (var koereToej in result)
			{
			  if (koereToej.KoeretoejsId == 0)
					Assert.Fail("Der skal være id");
				if (koereToej.Miljoeklasse == "")
					Assert.Fail("Der skal være Miljoeklasse");

				if (koereToej.FirmaNavn == "")
					Assert.Fail("Der skal være et firma");
			}
		}

		[Test]
		public void GetCreateVognlaesTest()
		{
			// Arrange
			var list = new List<VognlaesBom>();
			var vognlaes = new VognlaesBom();
			vognlaes.AnmeldelsesId = 1426;
			vognlaes.JordmaengdeAksler = 4;

			// {bfebd3b8-ca80-41e6-8ef3-750928c4a990}
			vognlaes.LastbilId = 1048;
			vognlaes.Tid = DateTime.Now;
			//vognlaes.StikproeveBaas = 3;

			list.Add(vognlaes);
			//Act
			var result = _business.CreateVognlaes(list);

			//Assert 
			Assert.AreEqual(1, result.Count);
		}



		[Test]
		public void PlanlagtStikproeveTest()
		{
			// Arrange
			var date = DateTime.Now;
			const int modtageAnlaegId = 1142;

			//Act
			var result = _business.GetBomAnmeldelser(modtageAnlaegId, date.AddDays(-40));

			//Assert 
			Assert.IsNotNull(result);

			foreach (var anmeldelse in result.AnmeldelseList)
			{

				if (anmeldelse.Anmeldelseslbrnr == 1206)
				{
					//var tt = 0;
				}

				if (anmeldelse.BetalerGodkendt)
				{
					//var tt = 3;
				}
				if (anmeldelse.BetalerSpaerret)
				{
					//var tt = 3;
				}
				if (anmeldelse.Anmeldelseslbrnr == 0)
					Assert.Fail("Der skal være LoebeNummer");
				if (anmeldelse.JordmodtageranlægId == 0)
					Assert.Fail("Der skal være modtageAnlæg id");
			}
		}




		/// <summary>
		/// Advisering af Prøvetager
		/// Ved 20 båse på modtageranlægget, skal prøvetager adviseres, 
		/// når bås 10 eller  20 bliver fyldt med jord.
		/// Til et modtageranlæg som anvenderJF skal der oprettes 
		/// vognlæs og stikprøver til bås 10 eller 20 bliver fyldt med jord. 
		/// 
		/// Godkendelses kriteriet: Prøvetager skal modtage en mail.
		/// </summary>
		[Test]
		public void AdviseringAfProevetagerTest()
		{
			// Arrange
			var list = new List<VognlaesBom>();

			//Act
			//var result = _business.CreateVognlaes(list);

			//Assert 
			//Assert.AreEqual(true, result);
		}

		/// <summary>
		/// 1.1	Bås nummer konflikt
		/// Kan der opstå en konflikt, hvis en bås ikke når at blive 
		/// tømt på 2 prøvetagningsrunder, og hvordan retter man det?
		/// </summary>
		[Test]
		public void BaasNummerKonfliktTest()
		{
			// Arrange
			var list = new List<VognlaesBom>();

			//Act
			//var result = _business.CreateVognlaes(list);

			//Assert 
			//Assert.AreEqual(true, result);
		}


		/// <summary>
		///	1.1	Planlagt stikprøve
		/// Hvis der er planlagt en stikprøve på anmeldelsen skal der oprettes en stikprøve, når vognlæsset oprettes.
		/// 1.1.1	Test
		///Opret planlagtstikprøve til anmeldelse. 
		///Opret vognlæs til anmeldelsen
		///Godkendelses kriterier:
		///Der skal oprettes en stikprøve. Der skal være reference mellem planlagtstikprøve og stikprøven.

		/// </summary>
		[Test]
		public void PlanlagtStikproeveTest2()
		{
			// Arrange
			var list = new List<VognlaesBom>();

			//Act
			//var result = _business.CreateVognlaes(list);

			//Assert 
			//Assert.AreEqual(true, result);
		}

		/// <summary>
		///	Alarm ved x% jord kørt.
		/// Opret en anmeldelse, hvor der også oprettes en alarm.
		/// Opret x antal vognlæs til anmeldelsen, så jordmængden overskrider den i alarmen definerede % grænse.
		/// Godkendelses kriterier: 
		/// -	Personen skal modtage en mail
		/// -	Alarm.Udfoert skal være true efter at alarmen er trigget.
		/// </summary>
		[Test]
		public void AlarmVedXProcentJordKoertTest()
		{
			// Arrange
			var list = new List<VognlaesBom>();

			//Act
			//var result = _business.CreateVognlaes(list);

			//Assert 
			//Assert.AreEqual(true, result);
		}




//		3	ANMELDELSER
//3.1	Kriterier for aktive anmeldelser
//StatusAnmeldelseTyper: Ikke afsluttet, aktiv, 
//Kørselsperiode
//Jordmængde ikke overskredet
//Modtageranlæg anvenderJF: true
//LAHA: Opdaterer selv med flere kriterier.
//3.1.1	Test
//Opret flere forskellige anmeldelser.
//Godkendelse kriteriet:
//Det er kun de relevant anmeldelser som skal være med på outputtet til bomsy-stemet
		[Test]
		public void KriterierForAktiveAnmeldelserTest()
		{
			// Arrange
			var list = new List<VognlaesBom>();

			//Act
			//var result = _business.CreateVognlaes(list);

			//Assert 
			//Assert.AreEqual(true, result);
		}

	}
}
