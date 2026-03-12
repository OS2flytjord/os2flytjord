using System;
using System.Collections.ObjectModel;
using System.Data.Spatial;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Infrastructure.DataAccess;
using Niras.Jordflytning.Infrastructure.Services;

namespace Niras.Jordflytning.IntegrationTests.Core
{
	[TestFixture]
	public class MatrikelBusinessTests
	{

        private const int SpatialReference = 25832;

        // Ninject kernel
        private readonly IKernel _ninjectKernel;
        private readonly IMatrikelBusiness _business;

        public MatrikelBusinessTests()
		{
            // Init Ninject kernel
            _ninjectKernel = new StandardKernel(); 
            _ninjectKernel.Bind<IMatrikelBusiness>().To<MatrikelBusiness>();
            _ninjectKernel.Bind<IMatrikelRepository>().To<MatrikelRepository>();
            _ninjectKernel.Bind<IMatrikelOpslagRepository>().To<MatrikelOpslagRepository>();
            _business = _ninjectKernel.Get<IMatrikelBusiness>();
		}


		[Test]
		public void GetMatrikelFromPointTest()
		{
		    var wkt = GetPoint();
		    var list = _business.ReadMatrikler(wkt);
            Assert.IsNotNull(list);
            Assert.AreEqual(1, list.Count);
		}

        [Test]
        public void GetMatrikelFromPoint2Test()
        {
            var wkt = GetPoint2();
            var list = _business.ReadMatrikler(wkt);
            Assert.IsNotNull(list);
            Assert.AreEqual(1, list.Count);
        }

        [Test]
        public void GetMatrikelFromPolygonTest()
        {
            var wkt = GetPolygon();
            var list = _business.ReadMatrikler(wkt);
            Assert.IsNotNull(list);
            Assert.AreEqual(3, list.Count);
        }

        [Test]
        public void GetMatrikelFromPolygon2Test()
        {
            var wkt = GetPolygon2();
            var list = _business.ReadMatrikler(wkt);
            Assert.IsNotNull(list);
            Assert.AreEqual(1, list.Count);
        }

        private static string GetPoint()
        {
            const string wkt = "POINT(568124.29904483 6219190.04749136)";
            return wkt;
        }
        private static string GetPoint2()
        {
            const string wkt = "POINT(571152.48 6225014.4)";
            return wkt;
        }

	    private static string GetPolygon()
	    {
            const string inputGml = @"<gml:Polygon xmlns:gml=""http://www.opengis.net/gml"">" +
                                    @"<gml:exterior>" +
	                                @"<gml:LinearRing>" +
                                    @"<gml:posList>568400.655 6218890.654 568426.802 6218886.965 568426.85 6218898.159 568406.885 6218900.397 568405.457 6218900.446 568404.063 6218900.132 568402.796 6218899.472 568401.822 6218898.651 568401.056 6218897.634 568400.536 6218896.471 568400.289 6218895.222 568400.325 6218893.949 568400.655 6218890.654</gml:posList>" +
	                                @"</gml:LinearRing>" +
                                    @"</gml:exterior>" +
	                                @"</gml:Polygon>";

            var geom = DbGeometry.FromGml(inputGml, SpatialReference);
            
	        var returnWkt = geom.AsText();
	        //const string wkt = "POLYGON((559658 6275834,559659 6275834,559659 6275835,559658 6275835,559658 6275834))";
            return returnWkt;
	    }	    
        
        private static string GetPolygon2()
        {
            const string returnWkt = "POLYGON((571151.2000000001 6225205,571188.8 6225201.8,571181.6000000001 6225153.8,571147.2000000001 6225177,571151.2000000001 6225205))";
            return returnWkt;
        }
	}
}
