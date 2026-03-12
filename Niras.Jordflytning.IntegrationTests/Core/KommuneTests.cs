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
  public class KommuneTests
  {

    // Ninject kernel
    private IKernel _ninjectKernel;
    private IKommuneBusiness _kommuneBusiness;

    public KommuneTests()
    {
      // Init Ninject kernel
      _ninjectKernel = new StandardKernel
          (
            new ConfigModule(),
            new RepositoryModule()
          );
      _ninjectKernel.Bind<IKommuneBusiness>().To<KommuneBusiness>();
      _kommuneBusiness = _ninjectKernel.Get<IKommuneBusiness>();
      
    }

    [SetUp]
    public void Setup()
    { }

    [TearDown]
    public void TearDown()
    { }

    [Test]
    public void ReadAktiveKommunerTest()
    {
      var k = _kommuneBusiness.ReadAktiveKommuner();
      Assert.IsTrue(k.Count>0);
    }

    [Test]
    public void ReadKommune()
    {
      var k = _kommuneBusiness.ReadAktiveKommuner();
      var kk = _kommuneBusiness.ReadKommune(k[0].Id);
      Assert.IsTrue(kk!=null);
      Assert.IsTrue(kk.Id == k[0].Id);
    }

  }
}
