using System.Collections.Generic;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.JordForurening;
using Niras.Jordflytning.Infrastructure.DataAccess;
using Niras.Jordflytning.IntegrationTests.TestsSetup;

namespace Niras.Jordflytning.IntegrationTests.Core
{
	[TestFixture]
	public class GeoEnvironRepositoryTests
	{
		private readonly IGeoEnvironRepository _repo;
		private readonly List<JordKlassifikationType> _jordKlassifikationTypeList;
		private IList<GeoEnvironKlassifikation> _geoEnvironKlassifikationList;
		private readonly IKodelisteBusiness _bussinesKodeList;

		private readonly IKernel _ninjectKernel;


		public GeoEnvironRepositoryTests()
		{
			// Init Ninject kernel
			_ninjectKernel = new StandardKernel();
			//_ninjectKernel.Bind<IGeoEnvironRepository>().To<GeoEnvironRepository>();
			
			_bussinesKodeList = TestSetupUtil.InjectKodelists(_ninjectKernel);

			_repo = _ninjectKernel.Get<IGeoEnvironRepository>();
			_jordKlassifikationTypeList = new List<JordKlassifikationType>();
		}

		[SetUp]
		public void Setup()
		{
			_geoEnvironKlassifikationList = null;

		}

		[TearDown]
		public void TearDown()
		{
		}


		[Test]
		public void GetMatrikelTest()
		{
			const string matrikelNummer = "12ab";
			const string ejerlavNummer = "2006353";
			const string ejerlavNavn = "";

			var strList = _repo.GetResult(ejerlavNummer, ejerlavNavn, matrikelNummer, _jordKlassifikationTypeList);

			Assert.IsNotNull(strList);
		}


		[Test]
		public void GetMatrikel2Test()
		{
			const string matrikelNummer = "12ab";
			const string ejerlavNummer = "2006353";
			string except;
			var strList = _repo.GetRows(ejerlavNummer, matrikelNummer, _geoEnvironKlassifikationList, out except);
			Assert.IsNotNull(strList);
		}


		[Test]
		public void GetElmsager5()
		{
			const string matrikelNummer = "23ef";
			const string ejerlavNummer = "1021151";
			string except;
			var strList = _repo.GetRows(ejerlavNummer, matrikelNummer, _geoEnvironKlassifikationList, out except);

			Assert.IsNull(strList);
		}

		[Test]
		public void GetLokesvej7()
		{
			const string matrikelNummer = "5bi";
			const string ejerlavNummer = "1020451";
			string except;
			var strList = _repo.GetRows(ejerlavNummer, matrikelNummer, _geoEnvironKlassifikationList, out except);

			Assert.IsNotNull(strList);
		}

				[Test]
		public void TestFrejasvej27()
		{
			// Arrange
			const string matrikelNummer = "17ey";
			const string ejerlavNummer = "1020451";
			string except;
			var strList = _repo.GetRows(ejerlavNummer, matrikelNummer, _geoEnvironKlassifikationList, out except);

			Assert.IsNotNull(strList);
		}
		/// <summary>
		/// Can only be run, as a debug test
		/// </summary>
		[Ignore]
		[Test]
		public void GetMatrikelFailWithNoConnectionTest()
		{
			const string matrikelNummer = "12ab";
			const string ejerlavNummer = "2006353";

			// 1. set debug point here
			// 2. run
			// 3. Unplug net
			// 4. continue
			
			//var setDebugPointHere = true;

			string except;
			var strList = _repo.Get(ejerlavNummer, matrikelNummer, _jordKlassifikationTypeList, out except);
			Assert.IsNotNull(except);
			Assert.IsNotNull(strList);
		}

		[Test]
		public void GetMatrikelWithoutPullotionTest()
		{
			const string matrikelNummer = "6gh";
			const string ejerlavNummer = "1020451";
			string except;
			var strList = _repo.Get(ejerlavNummer, matrikelNummer, _jordKlassifikationTypeList, out except);

			Assert.IsNull(strList);
		}

		[Test]
		public void GetMatrikelWithNoMatrikelTest()
		{
			const string matrikelNummer = "6ghee";
			const string ejerlavNummer = "1020451";
			string except;
			var strList = _repo.Get(ejerlavNummer, matrikelNummer, _jordKlassifikationTypeList, out except);

			Assert.IsNull(strList);
		}

		[Test]
		public void GetMatrikelOutsideAarhusTest()
		{
			const string matrikelNummer = "3c";
			const string ejerlavNummer = "51251";
			string except;
			var strList = _repo.Get(ejerlavNummer, matrikelNummer, _jordKlassifikationTypeList, out except);
			Assert.IsNull(except);
			Assert.IsNull(strList);
		}

		/// <summary>
		///
		/// </summary>
		[Test]
		public void GetMatrikelWithExceptionTest()
		{
			const string matrikelNummer = "6gh";
			const string ejerlavNummer = "";
			string except;
			var strList = _repo.Get(ejerlavNummer, matrikelNummer, _jordKlassifikationTypeList, out except);

			Assert.IsNotNull(except);
			Assert.IsNull(strList);
		}

		/// <summary>
		///
		/// </summary>
		[Test]
		public void GetMatrikelWithExceptionTest2()
		{
			const string matrikelNummer = "";
			const string ejerlavNummer = "1020451";
			string except;
			var strList = _repo.Get(ejerlavNummer, matrikelNummer, _jordKlassifikationTypeList, out except);

			Assert.IsNotNull(except);
			Assert.IsNull(strList);
		}
	}
}
