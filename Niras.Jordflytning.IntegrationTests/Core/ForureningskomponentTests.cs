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
  public class ForureningskomponentTests
  {
    private IKernel _ninjectKernel;
    private IForureningskomponentBusiness forureningkomponenBusiness;

    public ForureningskomponentTests()
    {
      _ninjectKernel = new StandardKernel
         (
         new ConfigModule(),
         new RepositoryModule()
         );


      _ninjectKernel.Bind<IForureningskomponentBusiness>().To<ForureningskomponentBusiness>();
      forureningkomponenBusiness = _ninjectKernel.Get<IForureningskomponentBusiness>();
    }

    [Test]
    public void ReadAktiveForureningskomponenterTest()
    {
      var res = forureningkomponenBusiness.ReadAktiveForureningskomponenter();
      Assert.IsTrue(res!=null);
      Assert.IsTrue(res.Count>0);
    }

  }
}
