using System.Linq;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Infrastructure.DependencyResolution;

namespace Niras.Jordflytning.IntegrationTests.Core
{
  [TestFixture]
  public class KodelisteTests
  {

    // Ninject kernel
    private IKernel _ninjectKernel;
    private IKodelisteBusiness _kodelisteBusiness;
    private IKommuneBusiness _kommuneBusiness;
    
    public KodelisteTests()
    {
      // Init Ninject kernel
      _ninjectKernel = new StandardKernel
          (
            new ConfigModule(),
            new RepositoryModule()
          );
      _ninjectKernel.Bind<IKodelisteBusiness>().To<KodelisteBusiness>();
      _kodelisteBusiness = _ninjectKernel.Get<IKodelisteBusiness>();

      _ninjectKernel.Bind<IKommuneBusiness>().To<KommuneBusiness>();
      _kommuneBusiness= _ninjectKernel.Get<IKommuneBusiness>();     
    }

    [SetUp]
    public void Setup()
    { }

    [TearDown]
    public void TearDown()
    { }

    [Test]
    public void ReadAllKodelisterTest()
    {
      //Tjekker at alle kodelister kan hentes.

      var docTypes = _kodelisteBusiness.ReadAktiveDokumentationType();
      Assert.IsTrue(docTypes.Count > 0);

      if (docTypes.Count > 0)
      {
        var dt = _kodelisteBusiness.ReadDokumentationType(docTypes[0].Id);
        Assert.IsTrue(dt != null);
        Assert.IsTrue(dt.Id == docTypes[0].Id);
      }
    }


    [Test]
    public void ReadKommuneListTest()
    {
      var ks = _kodelisteBusiness.ReadAktiveKommune();
      Assert.IsTrue(ks.Count > 0);
    }

    [Test]
    public void ReadAktiveOprindelsesstedKlassifikationTypesTest()
    {
      var ks = _kodelisteBusiness.ReadAktiveOprindelsesstedKlassifikationTypes();
      Assert.IsTrue(ks.Count > 0);
    }


    [Test]
    public void ReadAktiveAndenOprindJordTypesTest()
    {
      var ks = _kodelisteBusiness.ReadAktiveAndenOprindJordTypes();
      Assert.IsTrue(ks.Count > 0);
    }

    [Test]
    public void ReadAktiveAffaldTypesTest()
    {
      var ks = _kodelisteBusiness.ReadAktiveAffaldTypes();
      Assert.IsTrue(ks.Count > 0);
    }

    [Test]
    public void ReadAktiveJordKlassifikationTypesForKommune()
    {
      var aarhus =
        (from k in _kommuneBusiness.ReadAktiveKommuner() where k.Navn == "Århus Kommune" select k).FirstOrDefault();
      var j = _kodelisteBusiness.ReadAktiveJordKlassifikationTypesForLandsdel(aarhus.Id);
      Assert.IsTrue(j.Count>0);
    }

    [Test]
    public void ReadAktiveJordflytningTypes()
    {
      var j = _kodelisteBusiness.ReadAktiveJordflytningTypes();
      Assert.IsTrue(j.Count>0);
    }

  }

}
