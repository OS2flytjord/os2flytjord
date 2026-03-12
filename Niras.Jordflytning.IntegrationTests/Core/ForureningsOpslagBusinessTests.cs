using System;
using System.Collections.ObjectModel;
using System.Data.Spatial;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Core.Models.JordForurening;
using Niras.Jordflytning.Infrastructure.DataAccess;
using Niras.Jordflytning.Infrastructure.Services;
using Niras.Jordflytning.IntegrationTests.TestsSetup;

namespace Niras.Jordflytning.IntegrationTests.Core
{
	[TestFixture]
	public class ForureningsOpslagBusinessTests
	{

		// Ninject kernel
		private readonly IKernel _ninjectKernel;
		private readonly IForureningsOpslagBusiness _business;


		private readonly Guid _aarhusGuid = new Guid("a15d5888-bc70-4206-871e-a47a0c05decc");

		public ForureningsOpslagBusinessTests()
		{
			// Init Ninject kernel
			_ninjectKernel = new StandardKernel();

			_ninjectKernel.Bind<IForureningsOpslagBusiness>().To<ForureningsOpslagBusiness>();

			TestSetupUtil.InjectKodelists(_ninjectKernel);

			_ninjectKernel.Bind<IMiljoePortalRepository>().To<MiljoePortalRepository>();
			_ninjectKernel.Bind<IGeoEnvironRepository>().To<GeoEnvironRepository>();

			_business = _ninjectKernel.Get<IForureningsOpslagBusiness>();
		}


		[Test]
		public void GetJordForureningTest()
		{
			const string matrikelNummer = "12ab";
			const string ejerlavNummer = "2006353";

			var oprSted = GetOprindelsesstedV1();
			oprSted.Matrikel = new Collection<Matrikel>();

			var mtr = new Matrikel();
			mtr.Ejerlav = ejerlavNummer;
			mtr.Matrikelnr = matrikelNummer;
			var aarhusGuid = _aarhusGuid;
			oprSted.Matrikel.Add(mtr);

			var opslag = _business.Read(oprSted, aarhusGuid);

			var hasAnmeldePligt = opslag.HasAnmeldePligt(0,1);//Ikke århus regel vedr. offentlig vej
			Assert.AreEqual(true, hasAnmeldePligt);
		}

		[Test]
		public void GetJordForureningjordklassTest()
		{
			const string matrikelNummer = "12ab";
			const string ejerlavNummer = "2006353";

			var oprSted = GetOprindelsesstedV1();
			oprSted.Matrikel = new Collection<Matrikel>();

			var mtr = new Matrikel();
			mtr.Ejerlav = ejerlavNummer;
			mtr.Matrikelnr = matrikelNummer;
			var aarhusGuid = _aarhusGuid;
			oprSted.Matrikel.Add(mtr);

			var areal = _business.Read(oprSted, aarhusGuid);

			var klassType = areal.GetMiljoePortalKlassifikation(0,1);//Ikke århus, jord fra ejendom

			Assert.IsNotNull(klassType, "Der burde være een klassifikation med typen 'analyse pligt'");
		}

    [Test]
    public void GetJordForureningjordklassOffentligVejAarhusTest()
    {
      const string matrikelNummer = "12ab";
      const string ejerlavNummer = "2006353";

      var oprSted = GetOprindelsesstedV1();
      oprSted.Matrikel = new Collection<Matrikel>();

      var mtr = new Matrikel();
      mtr.Ejerlav = ejerlavNummer;
      mtr.Matrikelnr = matrikelNummer;
      var aarhusGuid = _aarhusGuid;
      oprSted.Matrikel.Add(mtr);

      var areal = _business.Read(oprSted, aarhusGuid);

      var klassType = areal.GetMiljoePortalKlassifikation(751, 2);//århus, jord fra offentlig vej.

      Assert.IsNotNull(klassType, "Der burde være een klassifikation med typen 'analyse pligt'");
    }

		[Test]
		public void GetJordForureningPointTest()
		{
			const string matrikelNummer = "12ab";
			const string ejerlavNummer = "2006353";

			var oprSted = GetOprindelsesstedPoint();
			oprSted.Matrikel = new Collection<Matrikel>();

			var mtr = new Matrikel();
			mtr.Ejerlav = ejerlavNummer;
			mtr.Matrikelnr = matrikelNummer;
			var aarhusGuid = new Guid("a15d5888-bc70-4206-871e-a47a0c05decc");
			oprSted.Matrikel.Add(mtr);

			var areal = _business.Read(oprSted, aarhusGuid);
			var klassType = areal.GetMiljoePortalKlassifikation(0,1);

			Assert.AreEqual(true, areal.HasAnmeldePligt(0,1));//Ikke århus regel vedr. offentlig vej
			Assert.IsNotNull(klassType, "Der burde være een klassifikation");
		}


		[Test]
		public void GetJordForureningFailTest()
		{
			var aarhusGuid = new Guid();

			const string matrikelNummer = "12ab";
			const string ejerlavNummer = "2006353";

			var oprSted = GetOprindelsesstedPoint();
			oprSted.Matrikel = new Collection<Matrikel>();

			var mtr = new Matrikel();
			mtr.Ejerlav = ejerlavNummer;
			mtr.Matrikelnr = matrikelNummer;
			
			oprSted.Matrikel.Add(mtr);

			var areal = _business.Read(oprSted, aarhusGuid);
			var list = areal.GetResultList();

			var klassType = list.Find(e => e.Header.Contains("Systemfejl!"));

			Assert.IsNotNull(klassType, "Der burde være een klassifikation med typen 'Systemfejl'");
		}

		[Test]
		public void GetJordForureningFail2Test()
		{
			const string matrikelNummer = "12ab";
			const string ejerlavNummer = "2006353";

			var oprSted = GetOprindelsesstedPoint();
			oprSted.Matrikel = new Collection<Matrikel>();

			var mtr = new Matrikel();
			mtr.Ejerlav = ejerlavNummer;
			mtr.Matrikelnr = matrikelNummer;
			var aarhusGuid = new Guid();
			oprSted.Matrikel.Add(mtr);

			var areal = _business.Read(oprSted, aarhusGuid);

			var skalAnmeldes = areal.HasAnmeldePligt(0,1);//Ikke århus regel vedr. offentlig vej

			Assert.IsTrue(skalAnmeldes);
		}

		/// <summary>
		/// Can only be run, as a debug test
		/// </summary>
		[Ignore]
		[Test]
		public void GetJordForureningUnplugNetTest()
		{
			const string matrikelNummer = "12ab";
			const string ejerlavNummer = "2006353";

			var oprSted = GetOprindelsesstedV1();
			oprSted.Matrikel = new Collection<Matrikel>();

			var mtr = new Matrikel();
			mtr.Ejerlav = ejerlavNummer;
			mtr.Matrikelnr = matrikelNummer;
			var aarhusGuid = _aarhusGuid;
			oprSted.Matrikel.Add(mtr);

			// 1. set debug point here
			// 2. run
			// 3. Unplug net
			// 4. continue
			//var setDebugPointHere = true;

			var areal = _business.Read(oprSted, aarhusGuid);
			var list = areal.GetResultList();
			//var hasAnmmeldepligt = areal.HasAnmeldePligt();
			Assert.AreEqual(6, list.Count);
		}

		//Områdeklassificering: Analysekode: 1, Tekst: Analysefrit område (Kategori 1) 
		//Det er nødvendig at lave en anmeldelse.
		[Test]
		public void TestKlokkerfaldet6()
		{
			// Arrange
			const string matrikelNummer = "4kz";
			const string ejerlavNummer = "1020451";
			const string gmlPoint = @"<Point xmlns=""http://www.opengis.net/gml""><pos>571726 6224595</pos></Point>";

			var oprSted = CreateOprindelsesSted(gmlPoint, matrikelNummer, ejerlavNummer);

			// Act
			var areal = _business.Read(oprSted, _aarhusGuid);

			// Assert
			var skalAnmeldes = areal.HasAnmeldePligt(0,1);//Ikke århus regel vedr. offentlig vej
			Assert.IsTrue(skalAnmeldes);

			var kls = areal.GetMiljoePortalKlassifikation(0,1);//Ikke Århus, jord fraejendom
			Assert.IsNotNull(kls);
			Assert.AreEqual((short) ForureningsKlasse.JordLetForurenet, kls.Kode);
		}

		//Områdeklassificering: Analysekode: 2, Tekst: Analysefrit område (Kategori 2) 
		//Det er nødvendig at lave en anmeldelse.
		[Test]
		public void TestKlokkervej19()
		{
			// Arrange
			const string matrikelNummer = "1fi";
			const string ejerlavNummer = "1020451";
			const string gmlPoint = @"<Point xmlns=""http://www.opengis.net/gml""><pos>572204 6224694</pos></Point>";

			var oprSted = CreateOprindelsesSted(gmlPoint, matrikelNummer, ejerlavNummer);

			// Act
			var areal = _business.Read(oprSted, _aarhusGuid);

			// Assert
      var skalAnmeldes = areal.HasAnmeldePligt(0, 1);//Ikke århus regel vedr. offentlig vej
			Assert.IsTrue(skalAnmeldes);

      var kls = areal.GetMiljoePortalKlassifikation(0, 1);//Ikke Århus, jord fraejendom
			Assert.IsNotNull(kls);
			Assert.AreEqual((short) ForureningsKlasse.JordLetForurenet, kls.Kode);
		}


		//Statuskode: 3, Status: Gældende / Vedtaget, Temanavn: Jordforurening V2 
		//Områdeklassificering: Analysekode: 3, Tekst: Område med krav om analyser 
		//Kortlagt forurening: Ja, Vidensniveau: V2, Dato: - , Nuancering: IkkeNuanceretEndnu, Lokalitets id: - , Bemærkninger: - 
		//Kortlagt forurening: Ja, Vidensniveau: V2, Dato: - , Nuancering: IkkeNuanceretEndnu, Lokalitets id: 751-00726-00, Bemærkninger: Region Midtjylland/tidligere Århus Amt har kortlagt ejendommen på vidensniveau 2 efter lov om forurenet jord. Der er udført forureningsundersøgelse på ejendommen, som har påvist forurening, der overskrider gældende grænseværdier og som har begrundet, at ejendommen kortlægges. Jorden på ejendommen kan derfor ikke håndteres frit. I tilfælde af, at jord ønskes bortgravet og flyttet, skal jordflytningen anmeldes til Aarhus Kommune. Yderligere oplysninger kan fås ved henvendelse til Region Midtjylland, Miljø, e-mail: miljoe@ru.rm.dk, tlf.: 78 41 19 99 eller Natur og Miljø, Aarhus Kommune, tlf.: 89 40 45 22, mail: jordgruppen@mtm.aarhus.dk. 
		//Det ER nødvendig at lave en anmeldelse.
		[Test]
		public void TestFrejasvej27()
		{
			// Arrange
			const string matrikelNummer = "17ey";
			const string ejerlavNummer = "1020451";
			const string gmlPoint = @"<Point xmlns=""http://www.opengis.net/gml""><pos>573181 6223808</pos></Point>";

			var oprSted = CreateOprindelsesSted(gmlPoint, matrikelNummer, ejerlavNummer);

			// Act
			var areal = _business.Read(oprSted, _aarhusGuid);

			// Assert
      var skalAnmeldes = areal.HasAnmeldePligt(0, 1);//Ikke århus regel vedr. offentlig vej
			Assert.IsTrue(skalAnmeldes);

      var kls = areal.GetMiljoePortalKlassifikation(0, 1);//Ikke Århus, jord fraejendom
			Assert.IsNotNull(kls);
			Assert.AreEqual((short) ForureningsKlasse.JordKraftigForurenet, kls.Kode);
		}


		//Statuskode: 3, Status: Gældende / Vedtaget, Temanavn: Jordforurening V1 
		//Områdeklassificering: Analysekode: 3, Tekst: Område med krav om analyser 
		//Kortlagt forurening: Ja, Vidensniveau: V1, Dato: 28-06-2006, Nuancering: IkkeNuanceretEndnu, Lokalitets id: - , Bemærkninger: - 
		//Kortlagt forurening: Ja, Vidensniveau: V1, Dato: 28-06-2006, Nuancering: IkkeNuanceretEndnu, Lokalitets id: 751-02799-00, Bemærkninger: Region Midtjylland/tidligere Århus Amt har kortlagt ejendommen på vidensniveau 1 efter lov om forurenet jord. Kortlægningen er foretaget, idet aktiviteterne på ejendommen KAN have medført forurening. Yderligere oplysninger kan fås ved henvendelse til Region Midtjylland, Miljø, e-mail: miljoe@ru.rm.dk eller tlf.: 7841 1999. 
		//Det ER nødvendig at lave en anmeldelse.
		[Test]
		public void TestLokesvej7()
		{
			// Arrange
			const string matrikelNummer = "5bi";
			const string ejerlavNummer = "1020451";
			const string gmlPoint = @"<Point xmlns=""http://www.opengis.net/gml""><pos>572527 6223267</pos></Point>";

			var oprSted = CreateOprindelsesSted(gmlPoint, matrikelNummer, ejerlavNummer);

			// Act
			var areal = _business.Read(oprSted, _aarhusGuid);

			// Assert
      var skalAnmeldes = areal.HasAnmeldePligt(0, 1);//Ikke århus regel vedr. offentlig vej
			Assert.IsTrue(skalAnmeldes);

      var kls = areal.GetMiljoePortalKlassifikation(0, 1);//Ikke Århus, jord fraejendom
			Assert.IsNotNull(kls);
			Assert.AreEqual((short) ForureningsKlasse.JordKraftigForurenet, kls.Kode);
		}


		//<Point xmlns="http://www.opengis.net/gml"><pos>575245 6229653</pos></Point>
		//"1021151"
		//"23ef"
		//"Elmsager 5"
		//Det er IKKE nødvendig at lave en anmeldelse.
		[Test]
		public void TestElmsager5()
		{
			// Arrange
			const string matrikelNummer = "23ef";
			const string ejerlavNummer = "1021151";
			const string gmlPoint = @"<Point xmlns=""http://www.opengis.net/gml""><pos>575245 6229653</pos></Point>";

			var oprSted = CreateOprindelsesSted(gmlPoint, matrikelNummer, ejerlavNummer);

			// Act
			var areal = _business.Read(oprSted, _aarhusGuid);

			// Assert
      var skalAnmeldes = areal.HasAnmeldePligt(0, 1);//Ikke århus regel vedr. offentlig vej
			Assert.IsFalse(skalAnmeldes);

      var kls = areal.GetMiljoePortalKlassifikation(0, 1);//Ikke Århus, jord fraejendom
			Assert.IsNotNull(kls);
			Assert.AreEqual((short) ForureningsKlasse.JordRen, kls.Kode);
		}



		#region *** Private methods ***

		private static Oprindelsessted GetOprindelsesstedPoint()
		{
			var oprSted = new Oprindelsessted();
			const int srid = 25832;
			const string gmlPoint = @"<gml:Point xmlns:gml=""http://www.opengis.net/gml"">" +
			                        @"<gml:pos> 573940.74799999967 6223911.2320000008</gml:pos>" +
			                        @"</gml:Point>";

			oprSted.Geom = DbGeometry.FromGml(gmlPoint, srid);
			return oprSted;
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

		private static Oprindelsessted CreateOprindelsesSted(string gmlPoint, string matrikelNr, string ejerlav)
		{
			var oprSted = new Oprindelsessted();
			const int srid = 25832;
			oprSted.Geom = DbGeometry.FromGml(gmlPoint, srid);

			oprSted.Matrikel = new Collection<Matrikel>();
			var mtr = new Matrikel();
			mtr.Ejerlav = ejerlav;
			mtr.Matrikelnr = matrikelNr;
			oprSted.Matrikel.Add(mtr);
			return oprSted;
		}

		#endregion *** Private methods ***

	}
}
