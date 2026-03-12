using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Infrastructure.DependencyResolution;

namespace Niras.Jordflytning.IntegrationTests.Core
{
  [TestFixture]
  public class ModtagerAnlaegTests
  {

    // Ninject kernel
    private IKernel _ninjectKernel;
    private IModtagerAnlaegBusiness _modtagerAnlaegBusiness;

    public ModtagerAnlaegTests()
    {
      // Init Ninject kernel
      _ninjectKernel = new StandardKernel
          (
            new ConfigModule(),
            new RepositoryModule()
          );
      _ninjectKernel.Bind<IModtagerAnlaegBusiness>().To<ModtagerAnlaegBusiness>();
      _modtagerAnlaegBusiness = _ninjectKernel.Get<IModtagerAnlaegBusiness>();
			//_modtagerAnlaegBusiness.
      
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
      var k = _modtagerAnlaegBusiness.ReadAktiveModtagerAnlaeg();
      Assert.IsTrue(k.Count>0);
    }

    [Test]
    public void ReadTransportoerTest()
    {
      var k = _modtagerAnlaegBusiness.ReadAktiveModtagerAnlaeg();
      var kk = _modtagerAnlaegBusiness.Read(k[0].Id);
      Assert.IsTrue(kk!=null);
      Assert.IsTrue(kk.Id == k[0].Id);
    }

  }
}
