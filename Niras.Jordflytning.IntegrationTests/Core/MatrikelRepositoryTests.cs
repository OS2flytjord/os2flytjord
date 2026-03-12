using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Infrastructure.DataAccess;

namespace Niras.Jordflytning.IntegrationTests.Core
{
	[TestFixture]
	public class MatrikelRepositoryTests
	{
		private readonly IMatrikelRepository _repo;
		private readonly IKernel _ninjectKernel;

		public MatrikelRepositoryTests()
		{
			// Init Ninject kernel
			_ninjectKernel = new StandardKernel();
			_ninjectKernel.Bind<IMatrikelRepository>().To<MatrikelRepository>();
      _repo = _ninjectKernel.Get<IMatrikelRepository>();
		}

		[Test]
		public void GetMatrikelFromPoint()
		{
			const string wkt = "POINT(568124.29904483 6219190.04749136)";
			var res = _repo.GetListFromWfsService(wkt);

			Assert.IsTrue(res.Count == 1);
			Assert.IsNotNull(res[0].Geom);
			Assert.IsNotNullOrEmpty(res[0].Matrikelnr);
			Assert.IsNotNullOrEmpty(res[0].Ejerlav);
			Assert.IsNotNullOrEmpty(res[0].Sogn);
		}

		[Test]
		public void GetMatrikelFromPolygon()
		{
			const string wkt =
				"POLYGON ((568400.655 6218890.654, 568426.802 6218886.965, 568426.85 6218898.159, 568406.885 6218900.397, 568405.457 6218900.446, 568404.063 6218900.132, 568402.796 6218899.472, 568401.822 6218898.651, 568401.056 6218897.634, 568400.536 6218896.471, 568400.289 6218895.222, 568400.325 6218893.949, 568400.655 6218890.654))";
			var res = _repo.GetListFromWfsService(wkt);

			Assert.IsTrue(res.Count == 1);

		}

		[Test]
		public void GetMatrikelFromPolygon2()
		{
			const string wkt =
				"POLYGON((575110.2000000001 6226422.8,575101.4000000001 6226395.600000001,575127.4 6226387.600000001,575125.0000000001 6226420,575110.2000000001 6226422.8))";
			var res = _repo.GetListFromWfsService(wkt);

			Assert.IsTrue(res.Count == 5);

		}

		//  const string gmlPolygon =
		//@"<gml:MultiSurface  xmlns:gml=""http://www.opengis.net/gml"">" +
		//@"<gml:surfaceMember>" +
		//@"<gml:Polygon>" +
		//@"<gml:exterior>" +
		//@"<gml:LinearRing>" +
		//@"<gml:posList>568134.461526958,6219197.55134599 568112.303983957,6219198.29409571 568124.29904483,6219190.04749136 568134.461526958,6219197.55134599</gml:posList>" +
		//@"</gml:LinearRing>" +
		//@"</gml:exterior>" +
		//@"</gml:Polygon>" +
		//@"</gml:surfaceMember>" +
		//@"</gml:MultiSurface>";

		//  var filter2 =
		//@"&<ogc:Filter>" +
		//@"<ogc:BBOX>" +
		//@"<ogc:PropertyName>ugis:CG_GEOMETRY</ogc:PropertyName>" +
		//@"<gml:Box srsName=""EPSG:25832"" xmlns:gml=""http://www.opengis.net/gml"">" +
		//@"<gml:coordinates>567341.385308978,6218217.05230188 569451.627499489,6219520.3351021</gml:coordinates>" +
		//@"</gml:Box>" +
		//@"</ogc:BBOX>" +
		//@"</ogc:Filter>";

		//  var filter3 =
		//      @"&<ogc:Filter>" +
		//      @"<ogc:Contains>" +
		//      @"<ogc:PropertyName>ugis:CG_GEOMETRY</ogc:PropertyName>" +
		//      @"<gml:Point srsName=""EPSG:25832"" xmlns:gml=""http://www.opengis.net/gml"">" +
		//      @"<gml:coordinates>567341.385308978,6218217.05230188</gml:coordinates>" +
		//      @"</gml:Point>" +
		//      @"</ogc:Contains>" +
		//      @"</ogc:Filter>";


		//  var filter4 =
		//      @"&<ogc:Filter xmlns:ogc=""http://www.opengis.net/ogc"">" +
		//      @"<ogc:Overlaps>" +
		//      @"<ogc:PropertyName>ugis:CG_GEOMETRY</ogc:PropertyName>" +
		//      @"<gml:Point srsName=""EPSG:25832"" xmlns:gml=""http://www.opengis.net/gml"">" +
		//      @"<gml:pos>568134.461526958,6219197.55134599</gml:pos>" +
		//      @"</gml:Point>" +
		//      @"</ogc:Overlaps>" +
		//      @"</ogc:Filter>";

		//  var filter5 =
		//      @"&<ogc:Filter xmlns:ogc=""http://www.opengis.net/ogc"">" +
		//      @"<ogc:BBOX>" +
		//      @"<ogc:PropertyName>ugis:CG_GEOMETRY</ogc:PropertyName>" +
		//      @"<gml:Box srsName=""EPSG:25832"" xmlns:gml=""http://www.opengis.net/gml"">" +
		//      @"<gml:coordinates>568119,6219201 568120,6219202</gml:coordinates>" +
		//      @"</gml:Box>" +
		//      @"</ogc:BBOX>" +
		//      @"</ogc:Filter>";


		//  var tt = "568119.762085669,6219201.43772624 568127.555060564,6219206.25065728";

		//  const string inputGeometryGml =
		//      @"<gml:Point srsName=""EPSG:25832"" xmlns:gml=""http://www.opengis.net/gml"">" +
		//      @"<gml:pos decimal=""."" cs="","" ts="" "">568124.29904483,6219190.04749136</gml:pos>" +
		//      @"</gml:Point>";


	}
}
