using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Infrastructure.Common;
using Niras.Jordflytning.Infrastructure.DataAccess;
using Niras.Jordflytning.UnitTests.TestsSetup.DependencyResolution;


namespace Niras.Jordflytning.UnitTests.Controllers
{
  [TestFixture]
  public class OpretAnmeldelseTests
  {
    // Ninject kernel
    private IKernel _ninjectKernel;

    private ITransportoerBusiness transportoerBusiness;
    private IModtagerAnlaegBusiness modtagerAnlaegBusiness;
    private Mock<ITransportoerRepository> mockTransRepo;
    private Mock<IAnmeldelserRepository> mockAnmeldelseRepo;
    private Mock<IModtagerAnlaegRepository> mockModtagerAnlaeg;

    public OpretAnmeldelseTests()
    {

      // Init Ninject kernel
      _ninjectKernel = new StandardKernel
        (
        new ConfigModule()
        //new RepositoryModule()
        );
      _ninjectKernel.Bind<IUnitOfWork>().To<UnitOfWork>();



      var configService = _ninjectKernel.Get<IConfigService>();

      //Bind repositories
      _ninjectKernel.Bind<IRepository<Lastbil>>().To<GenericRepository<Lastbil>>().WithConstructorArgument("dataContext", configService.DataContext);
      //Bind<IModtagerAnlaegRepository>().To<ModtagerAnlaegRepository>().WithConstructorArgument("configService", configService);
      //Bind<IAnmeldelserRepository>().To<AnmeldelserRepository>().WithConstructorArgument("configService", configService);
      _ninjectKernel.Bind<IStatusBetalerRepository>().To<StatusBetalerRepository>().WithConstructorArgument("configService", configService);
      _ninjectKernel.Bind<IStatusAnmeldelseTypeRepository>().To<StatusAnmeldelseTypeRepository>().WithConstructorArgument("configService", configService);
      _ninjectKernel.Bind<IStatusAnmeldelseRepository>().To<StatusAnmeldelseRepository>().WithConstructorArgument("configService", configService);
      _ninjectKernel.Bind<IDokumentationRepository>().To<DokumentationRepository>().WithConstructorArgument("configService", configService);
      _ninjectKernel.Bind<IKonfigRepository>().To<KonfigRepository>().WithConstructorArgument("configService", configService);
      _ninjectKernel.Bind<IGraensevaerdierRepository>().To<GraensevaerdierRepository>().WithConstructorArgument("configService", configService);
      _ninjectKernel.Bind<IJordmodtagerRepository>().To<JordmodtagerRepository>().WithConstructorArgument("configService", configService);
			_ninjectKernel.Bind<IVognlaesRepository>().To<VognlaesRepository>().WithConstructorArgument("configService", configService);

       

      _ninjectKernel.Bind<IJordmodtagerBusiness>().To<JordmodtagerBusiness>();
      
      //Bind<IAnmeldelserRepository>().To<AnmeldelserRepository>().WithConstructorArgument("configService", configService);

      



      mockTransRepo = new Mock<ITransportoerRepository>();
      _ninjectKernel.Bind<ITransportoerRepository>().ToConstant(mockTransRepo.Object);

      mockAnmeldelseRepo = new Mock<IAnmeldelserRepository>();
      _ninjectKernel.Bind<IAnmeldelserRepository>().ToConstant(mockAnmeldelseRepo.Object);

      mockModtagerAnlaeg = new Mock<IModtagerAnlaegRepository>();
      _ninjectKernel.Bind<IModtagerAnlaegRepository>().ToConstant(mockModtagerAnlaeg.Object);

      _ninjectKernel.Bind<ITransportoerBusiness>().To<TransportoerBusiness>();
      transportoerBusiness = _ninjectKernel.Get<ITransportoerBusiness>();

      _ninjectKernel.Bind<IModtagerAnlaegBusiness>().To<ModtagerAnlaegBusiness>();
      modtagerAnlaegBusiness = _ninjectKernel.Get<IModtagerAnlaegBusiness>();


    }

    [SetUp]
    public void Setup()
    {

    }

    [TearDown]
    public void TearDown()
    {

    }


    #region "Modtager og Transportør"

    [Test]
    public void TransportørFiltrering()
    {
      #region "Data"

      // Person


      //Transportør personer
      var pt1 = new Person { Aktiv = true, Id = Guid.NewGuid() };
      var pt2 = new Person { Aktiv = true, Id = Guid.NewGuid() };
      var pt3 = new Person { Aktiv = true, Id = Guid.NewGuid() };
      var pt4 = new Person { Aktiv = true, Id = Guid.NewGuid() };


      //Transportører
      var t1 = new Transportoer();
      var f1 = new Firmaoplysninger { Id = Guid.NewGuid(), Firmanavn = "f1" };
      t1.Id = new Guid();
      t1.Person = pt1;
      t1.Person.Firmaoplysninger = f1;

      var t2 = new Transportoer();
      var f2 = new Firmaoplysninger { Id = Guid.NewGuid(), Firmanavn = "f2" };
      t2.Id = new Guid();
      t2.Person = pt2;
      t2.Person.Firmaoplysninger = f2;

      var t3 = new Transportoer();
      var f3 = new Firmaoplysninger { Id = Guid.NewGuid(), Firmanavn = "f3" };
      t3.Id = new Guid();
      t3.Person = pt3;
      t3.Person.Firmaoplysninger = f3;

      var t4 = new Transportoer();
      var f4 = new Firmaoplysninger { Id = Guid.NewGuid(), Firmanavn = "f4" };
      t4.Id = new Guid();
      t4.Person = pt4;
      t4.Person.Firmaoplysninger = f4;
      t4.Person.Aktiv = false;


      //Anmelder
      var an1 = new Anmelder();
      an1.Id = new Guid("35E43FD9-5801-4139-8768-F141BF2FCDFE");
      var an2 = new Anmelder();
      an2.Id = new Guid("66DE106D-C9FA-4B27-9C2B-7ADF3C27FB57");

      //Anmeldelser

      //Anmelder 1 (an1)
      var a1 = new Anmeldelse();
      a1.Transportoer = t1;
      a1.Anmelder = an1;

      var a2 = new Anmeldelse();
      a2.Transportoer = t1;
      a2.Anmelder = an1;

      var a3 = new Anmeldelse();
      a3.Transportoer = t2;
      a3.Anmelder = an1;

      //Anmelder 2 (an2)
      var a11 = new Anmeldelse();
      a11.Transportoer = t1;
      a11.Anmelder = an2;

      var a22 = new Anmeldelse();
      a22.Transportoer = t1;
      a22.Anmelder = an2;

      var a33 = new Anmeldelse();
      a33.Transportoer = t3;
      a33.Anmelder = an2;
      #endregion

      //Test - Alle Aktive transportører, 
      IList<Transportoer> transportoers = new List<Transportoer>();
      transportoers.Add(t1);
      transportoers.Add(t2);
      transportoers.Add(t3);
      transportoers.Add(t4);
      mockTransRepo.Setup(f => f.Read()).Returns(transportoers);
      Assert.IsTrue(transportoerBusiness.ReadAktiveTransportoerer().Count == 3);


      //Test - Tidligere anvendte transportør
      IList<Anmeldelse> anmeldelses = new List<Anmeldelse>();
      anmeldelses.Add(a1);
      anmeldelses.Add(a2);
      anmeldelses.Add(a3);
      anmeldelses.Add(a11);
      anmeldelses.Add(a22);
      anmeldelses.Add(a33);

      mockAnmeldelseRepo.Setup(f => f.Read()).Returns(anmeldelses);
      var t_res1 = transportoerBusiness.ReadTidligereAktiveAnvendteTransportoerer(an1.Id);
      Assert.IsTrue(t_res1.Count == 2);
      Assert.IsTrue(t_res1.Contains(t1));
      Assert.IsTrue(t_res1.Contains(t2));
      Assert.IsTrue(!t_res1.Contains(t3));
      Assert.IsTrue(!t_res1.Contains(t4));

      var t_res2 = transportoerBusiness.ReadTidligereAktiveAnvendteTransportoerer(an2.Id);
      Assert.IsTrue(t_res2.Count == 2);
      Assert.IsTrue(t_res2.Contains(t1));
      Assert.IsTrue(!t_res2.Contains(t2));
      Assert.IsTrue(t_res2.Contains(t3));
      Assert.IsTrue(!t_res2.Contains(t4));

    }

    [Test]
    public void ModtagerAnlaegFiltrering()
    {
      #region "Data"
      //Anmelder
      var an1 = new Anmelder();
      an1.Id = new Guid("35E43FD9-5801-4139-8768-F141BF2FCDFE");
      var an2 = new Anmelder();
      an2.Id = new Guid("66DE106D-C9FA-4B27-9C2B-7ADF3C27FB57");

      //ModtagerAnlaeg
      var g1 = DbGeometry.PointFromText("POINT(54001 64001)", 25832);
      var g2 = DbGeometry.PointFromText("POINT(54002 64002)", 25832);
      var g3 = DbGeometry.PointFromText("POINT(54003 64003)", 25832);

      var ma1 = new ModtagerAnlaeg { Id = new Guid(), Navn = "ma1", Geom = g1, Aktiv = true, AnvenderJF = true };
      var ma2 = new ModtagerAnlaeg { Id = new Guid(), Navn = "ma2", Geom = g2, Aktiv = true, AnvenderJF = true };
      var ma3 = new ModtagerAnlaeg { Id = new Guid(), Navn = "ma3", Geom = g3, Aktiv = true, AnvenderJF = true };


      //Anmeldelser
      //Anmelder 1 (an1)
      var a1 = new Anmeldelse();
      a1.Anmelder = an1;
      a1.ModtagerAnlaeg = ma1;

      var a2 = new Anmeldelse();
      a2.Anmelder = an1;
      a2.ModtagerAnlaeg = ma2;

      var a3 = new Anmeldelse();
      a3.Anmelder = an1;
      a3.ModtagerAnlaeg = ma2;

      //Anmelder 2 (an2)
      var a11 = new Anmeldelse();
      a11.Anmelder = an2;
      a11.ModtagerAnlaeg = ma1;

      var a22 = new Anmeldelse();
      a22.Anmelder = an2;
      a22.ModtagerAnlaeg = ma3;

      var a33 = new Anmeldelse();
      a33.Anmelder = an2;
      a33.ModtagerAnlaeg = ma3;

      #endregion

      IList<Anmeldelse> anmeldelses = new List<Anmeldelse>();
      anmeldelses.Add(a1);
      anmeldelses.Add(a2);
      anmeldelses.Add(a3);
      anmeldelses.Add(a11);
      anmeldelses.Add(a22);
      anmeldelses.Add(a33);

      mockAnmeldelseRepo.Setup(f => f.Read()).Returns(anmeldelses);

      //Tidligere anvendte modtagerAnlæg, AnvenderJF=True
      bool anvenderJF = true;

      var res1 = modtagerAnlaegBusiness.ReadTidligereAktiveAnvendteModtagerAnlaeg(an1.Id);
      var res2 = modtagerAnlaegBusiness.ReadTidligereAktiveAnvendteModtagerAnlaeg(an2.Id);
      Assert.IsTrue(res1.Count == 2);
      Assert.IsTrue(res2.Count == 2);

      Assert.IsTrue(res1.Contains(ma1));
      Assert.IsTrue(res1.Contains(ma2));
      Assert.IsTrue(!res1.Contains(ma3));

      Assert.IsTrue(res2.Contains(ma1));
      Assert.IsTrue(!res2.Contains(ma2));
      Assert.IsTrue(res2.Contains(ma3));

      //Modtageranlæg sorteret efter afstand til oprindelsessted, anvenderjf=true
      var modtagerAnlaegs = new List<ModtagerAnlaeg>();
      modtagerAnlaegs.Add(ma1);
      modtagerAnlaegs.Add(ma2);
      modtagerAnlaegs.Add(ma3);

      mockModtagerAnlaeg.Setup(f => f.Read()).Returns(modtagerAnlaegs);

      var g0 = DbGeometry.PointFromText("POINT(54000 64000)", 25832);
      var dist1 = modtagerAnlaegBusiness.ReadAktiveNaermesteModtagerAnlaeg(g0, anvenderJF, Guid.Empty, false);
      Assert.IsTrue(dist1.Count == 3);
      Assert.IsTrue(dist1[0] == ma1);
      Assert.IsTrue(dist1[1] == ma2);
      Assert.IsTrue(dist1[2] == ma3);

      var g4 = DbGeometry.PointFromText("POINT(54004 64004)", 25832);
      var dist2 = modtagerAnlaegBusiness.ReadAktiveNaermesteModtagerAnlaeg(g4, anvenderJF, Guid.Empty, false);
      Assert.IsTrue(dist2.Count == 3);
      Assert.IsTrue(dist2[2] == ma1);
      Assert.IsTrue(dist2[1] == ma2);
      Assert.IsTrue(dist2[0] == ma3);

      //AnvenderJF=False
      anvenderJF = false;
      ma2.AnvenderJF = false;
      ma3.AnvenderJF = false;

      //Tidligere anvendte modtagerAnlæg, AnvenderJF=False
      var res3 = modtagerAnlaegBusiness.ReadTidligereAktiveAnvendteModtagerAnlaeg(an1.Id, anvenderJF, Guid.Empty,false);
      var res4 = modtagerAnlaegBusiness.ReadTidligereAktiveAnvendteModtagerAnlaeg(an2.Id, anvenderJF, Guid.Empty, false);
      Assert.IsTrue(res3.Count == 1);
      Assert.IsTrue(res4.Count == 1);

      //Modtageranlæg sorteret efter afstand til oprindelsessted, anvenderjf=false
      var dist3 = modtagerAnlaegBusiness.ReadAktiveNaermesteModtagerAnlaeg(g0, anvenderJF, Guid.Empty, false);
      Assert.IsTrue(dist3.Count == 2);
      Assert.IsTrue(dist3[0] == ma2);
      Assert.IsTrue(dist3[1] == ma3);

      var dist4 = modtagerAnlaegBusiness.ReadAktiveNaermesteModtagerAnlaeg(g4, anvenderJF, Guid.Empty, false);
      Assert.IsTrue(dist4.Count == 2);
      Assert.IsTrue(dist4[1] == ma2);
      Assert.IsTrue(dist4[0] == ma3);
    }


    [Test]
    public void ModaterAnlaegFiltreringPaaJordklassifikationType()
    {

      #region "Data"
      //Anmelder
      var an1 = new Anmelder();
      an1.Id = new Guid("35E43FD9-5801-4139-8768-F141BF2FCDFE");
      var an2 = new Anmelder();
      an2.Id = new Guid("66DE106D-C9FA-4B27-9C2B-7ADF3C27FB57");

      //ModtagerAnlaeg
      var g1 = DbGeometry.PointFromText("POINT(54001 64001)", 25832);
      var g2 = DbGeometry.PointFromText("POINT(54002 64002)", 25832);
      var g3 = DbGeometry.PointFromText("POINT(54003 64003)", 25832);

      var jk1 = new JordKlassifikationType { Id = Guid.NewGuid() };
      var jk23 = new JordKlassifikationType { Id = Guid.NewGuid() };

      var ma1 = new ModtagerAnlaeg { Id = new Guid(), Navn = "ma1", Geom = g1, Aktiv = true, AnvenderJF = true,JordKlassifikationType = jk1};
      var ma2 = new ModtagerAnlaeg { Id = new Guid(), Navn = "ma2", Geom = g2, Aktiv = true, AnvenderJF = true, JordKlassifikationType = jk23 };
      var ma3 = new ModtagerAnlaeg { Id = new Guid(), Navn = "ma3", Geom = g3, Aktiv = true, AnvenderJF = true, JordKlassifikationType = jk23 };


      //Anmeldelser
      //Anmelder 1 (an1)
      var a1 = new Anmeldelse();
      a1.Anmelder = an1;
      a1.ModtagerAnlaeg = ma1;

      var a2 = new Anmeldelse();
      a2.Anmelder = an1;
      a2.ModtagerAnlaeg = ma2;

      var a3 = new Anmeldelse();
      a3.Anmelder = an1;
      a3.ModtagerAnlaeg = ma2;

      //Anmelder 2 (an2)
      var a11 = new Anmeldelse();
      a11.Anmelder = an2;
      a11.ModtagerAnlaeg = ma1;

      var a22 = new Anmeldelse();
      a22.Anmelder = an2;
      a22.ModtagerAnlaeg = ma3;

      var a33 = new Anmeldelse();
      a33.Anmelder = an2;
      a33.ModtagerAnlaeg = ma3;

      #endregion

      IList<Anmeldelse> anmeldelses = new List<Anmeldelse>();
      anmeldelses.Add(a1);
      anmeldelses.Add(a2);
      anmeldelses.Add(a3);
      anmeldelses.Add(a11);
      anmeldelses.Add(a22);
      anmeldelses.Add(a33);

      mockAnmeldelseRepo.Setup(f => f.Read()).Returns(anmeldelses);


      //Modtageranlæg sorteret efter afstand til oprindelsessted, anvenderjf=true
      var modtagerAnlaegs = new List<ModtagerAnlaeg>();
      modtagerAnlaegs.Add(ma1);
      modtagerAnlaegs.Add(ma2);
      modtagerAnlaegs.Add(ma3);

      mockModtagerAnlaeg.Setup(f => f.Read()).Returns(modtagerAnlaegs);


      //Read modtagerAnlaeg med de forskellige funktioner, hvor vi anvender jordklassifikationer som filter

      //ReadAktiveModtagerAnlaeg
     // Assert.IsTrue(modtagerAnlaegBusiness.ReadAktiveModtagerAnlaeg(true, jk1.Id, false).Count == 1);
     // Assert.IsTrue(modtagerAnlaegBusiness.ReadAktiveModtagerAnlaeg(true, jk23.Id, false).Count == 2);

      //ReadTidligereAktiveAnvendteModtagerAnlaeg
      Assert.IsTrue(modtagerAnlaegBusiness.ReadTidligereAktiveAnvendteModtagerAnlaeg(an1.Id, true, jk1.Id, false).Count == 1);
      Assert.IsTrue(modtagerAnlaegBusiness.ReadTidligereAktiveAnvendteModtagerAnlaeg(an2.Id, true, jk23.Id, false).Count == 1);
      
      //ReadAktiveNaermesteModtagerAnlaeg
      var g0 = DbGeometry.PointFromText("POINT(54000 64000)", 25832);
      Assert.IsTrue(modtagerAnlaegBusiness.ReadAktiveNaermesteModtagerAnlaeg(g0, true, jk1.Id, false).Count == 1);
      Assert.IsTrue(modtagerAnlaegBusiness.ReadAktiveNaermesteModtagerAnlaeg(g0, true, jk23.Id, false).Count == 2);

    }

    #endregion

  }
}
