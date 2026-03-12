using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Infrastructure.DependencyResolution;

namespace Niras.Jordflytning.IntegrationTests.Core
{
  [TestFixture]
  public class TransportoerTests
  {

    // Ninject kernel
    private IKernel _ninjectKernel;
    private ITransportoerBusiness _transportoerBusiness;

    public TransportoerTests()
    {
      // Init Ninject kernel
      _ninjectKernel = new StandardKernel
          (
            new ConfigModule(),
            new RepositoryModule()
          );
      _ninjectKernel.Bind<ITransportoerBusiness>().To<TransportoerBusiness>();
      _transportoerBusiness = _ninjectKernel.Get<ITransportoerBusiness>();
      
    }

    [SetUp]
    public void Setup()
    { }

    [TearDown]
    public void TearDown()
    { }

    [Test]
    public void ReadAktiveTransportoererTest()
    {
      var k = _transportoerBusiness.ReadAktiveTransportoerer();
      Assert.IsTrue(k.Count>0);
    }

    [Test]
    public void ReadTransportoerTest()
    {
      var k = _transportoerBusiness.ReadAktiveTransportoerer();
      var kk = _transportoerBusiness.ReadTransportoer(k[0].Id);
      Assert.IsTrue(kk!=null);
      Assert.IsTrue(kk.Id == k[0].Id);
    }

  }
}
