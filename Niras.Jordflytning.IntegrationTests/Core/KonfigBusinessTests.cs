using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Infrastructure.Common;
using Niras.Jordflytning.Infrastructure.DependencyResolution;

namespace Niras.Jordflytning.IntegrationTests.Core
{
  [TestFixture]
  public class KonfigBusinessTests
  {
    // Ninject kernel
    private IKernel _ninjectKernel;
    private IKonfigBusiness _konfigBusiness;
    private IKommuneBusiness _kommuneBusiness;

    public KonfigBusinessTests()
    {
      _ninjectKernel = new StandardKernel
          (
            new ConfigModule(),
            new RepositoryModule()
          );

      _ninjectKernel.Bind<IUnitOfWork>().To<UnitOfWork>();
      _ninjectKernel.Bind<IKonfigBusiness>().To<KonfigBusiness>();
      _konfigBusiness = _ninjectKernel.Get<IKonfigBusiness>();

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
    public void ReadKonfigTest()
    {
      var komId = (from kom in  _kommuneBusiness.ReadAktiveKommuner().Where(f=>f.Navn=="Aarhus") select kom.Id).FirstOrDefault();
      
      var k = _konfigBusiness.ReadKommuneKonfig("abekattestreger",komId);
      Assert.IsTrue(string.IsNullOrEmpty(k));

      Assert.IsTrue(!string.IsNullOrEmpty(_konfigBusiness.ReadKommuneKonfig(EnumKonfigKey.InfoOpretAnmeldelseStedKommuneSpecifik.ToString(),komId)));
    }
  }
}
