using System;
using System.Collections.Generic;
using System.Data.Spatial;
using Ninject;
using NUnit.Framework;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.JordForurening;
using Niras.Jordflytning.Infrastructure.DataAccess;

namespace Niras.Jordflytning.IntegrationTests.Core
{
    [TestFixture]
    public class MiljoePortalTests
    {
        // Ninject kernel
        private readonly IKernel _ninjectKernel;
        private readonly IMiljoePortalRepository _miljoePortalRepository;

        public MiljoePortalTests()
        {
            _ninjectKernel = new StandardKernel();
            _ninjectKernel.Bind<IMiljoePortalRepository>().To<MiljoePortalRepository>();
            _miljoePortalRepository = _ninjectKernel.Get<IMiljoePortalRepository>();
        }


        [Test]
        public void TestOpslag()
        {
            // Arrange
            var oprSted = GetOprindelsesstedV1();

	        List<ForureningsOpslagResult> opslag = null;

            // Act
            try
            {
                var klassiList = GetKlassiList();
                opslag = _miljoePortalRepository.GetResult(oprSted, klassiList);
                //descList = opslag.GetForureningsDescriptionList();
            }
            catch (Exception exception)
            {
                Assert.Fail(exception.StackTrace);
            }
            // Assert
            Assert.IsNotNull(opslag);
            //Assert.IsNotNull(descList);
            //Assert.AreEqual(3, descList.Count);
        }

        [Test]
        public void TestOpslagFail()
        {
            // Arrange
            var oprSted = GetOprindelsesstedV1();

            ForureningsOpslag opslag = null;
            List<string> descList = null;
            List<string> expList = null;
            List<JordKlassifikationType> selectedKlassiList = null;

            // Act
            try
            {
								//var klassiList = new List<JordKlassifikationType>();
								//opslag = _miljoePortalRepository.Read(oprSted, klassiList);
								//descList = opslag.GetForureningsDescriptionList();
								//expList = opslag.GetExceptionList();
								//selectedKlassiList = opslag.GetMiljoePortalKlassifikationList();
            }
            catch (Exception exception)
            {
                Assert.Fail(exception.StackTrace);
            }
            // Assert
            Assert.IsNotNull(opslag);

            Assert.AreEqual(3, descList.Count);
            Assert.AreEqual(4, selectedKlassiList.Count);
            Assert.AreEqual(0, expList.Count);
        }


        private static Oprindelsessted GetOprindelsesstedV1()
        {
            var oprSted = new Oprindelsessted();
            const int srid = 25832;
            const string gmlPolygon = @"<gml:MultiSurface  xmlns:gml=""http://www.opengis.net/gml"">" +
                                      @"<gml:surfaceMember>" +
                                      @"<gml:Polygon>" +
                                      @"<gml:exterior>" +
                                      @"<gml:LinearRing>" +
                                      @"<gml:posList> 573940.74799999967 6223911.2320000008 573966.42599999998 6223887.0700000003 574011.71700000018 6223918.6070000008 573969.09900000039 6223980.1009999998 573935.65000000037 6223977.4189999998 573940.81699999981 6223911.3129999992 573940.74799999967 6223911.2320000008</gml:posList>" +
                                      @"</gml:LinearRing>" +
                                      @"</gml:exterior>" +
                                      @"</gml:Polygon>" +
                                      @"</gml:surfaceMember>" +
                                      @"</gml:MultiSurface>";

            oprSted.Geom = DbGeometry.FromGml(gmlPolygon, srid);
            return oprSted;
        }

        private static List<JordKlassifikationType> GetKlassiList()
        {
            var klassiList = new List<JordKlassifikationType>();

            var kl1 = new JordKlassifikationType();

            kl1.Navn = "Kraftigt forurenet";
            //kl1.KommuneId = new Guid("{9dcd3ea6-696d-4939-af01-fdb3369d3194}");
            kl1.Priotering = 1;

            var kl2 = new JordKlassifikationType();
            kl2.Navn = "Lettere forurenet"; 
            //kl2.KommuneId = new Guid("{9dcd3ea6-696d-4939-af01-fdb3369d3194}");
            kl2.Priotering = 2;

            var kl3 = new JordKlassifikationType();
            kl3.Navn = "Ren jord";
            //kl3.KommuneId = new Guid("{9dcd3ea6-696d-4939-af01-fdb3369d3194}");
            kl3.Priotering = 3;

            klassiList.Add(kl1);
            klassiList.Add(kl2);
            klassiList.Add(kl3);

            return klassiList;
        }
    }
}
