using System;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Infrastructure.DependencyResolution;

namespace Niras.Jordflytning.IntegrationTests.Core
{
  [TestFixture]
  public class StatusAnmeldelseBusinessTests
  {

    // Ninject kernel
    private IKernel _ninjectKernel;
    private IStatusAnmeldelseBusiness _statusAnmeldelseBusiness;
    //private Mock<IPrincipal> _user;
    //private Mock<IIdentity> _identity;
    //private Mock<ISecurityProvider> _securityProvider;

    public StatusAnmeldelseBusinessTests()
    {
      // Init Ninject kernel
      _ninjectKernel = new StandardKernel
          (
            new ConfigModule(),
            new RepositoryModule()
          );
      _ninjectKernel.Bind<IStatusAnmeldelseBusiness>().To<StatusAnmeldelseBusiness>();
      _statusAnmeldelseBusiness = _ninjectKernel.Get<IStatusAnmeldelseBusiness>();


      //_user = new Mock<IPrincipal>();
      //_identity = new Mock<IIdentity>();
      //_user.Setup(u => u.Identity).Returns(_identity.Object);
      //_securityProvider.Setup(s => s.CurrentUser).Returns(_user.Object as IPrincipal);
      //_ninjectKernel.Bind<ISecurityProvider>().ToConstant(_securityProvider.Object);
      //_securityProvider = new Mock<ISecurityProvider>();


    }

    [Test]
    public void CreateStatusOprettetTest()
    {
      var sa = _statusAnmeldelseBusiness.CreateStatus(EnumStatusAnmeldelse.Oprettet,null);
      Assert.IsTrue(sa.StatusAnmeldelseType.Navn.ToLower().Contains("anmeldelse oprettet"));
      Assert.IsTrue(sa.Person==null);
      Assert.IsTrue(sa.Tid.Date == DateTime.Now.Date);
      Assert.IsTrue(sa.Tid.Hour== DateTime.Now.Hour);

      sa = _statusAnmeldelseBusiness.CreateStatus(EnumStatusAnmeldelse.Gemt, null);
      Assert.IsTrue(sa.StatusAnmeldelseType.Navn.ToLower().Contains("gemt"));
      Assert.IsTrue(sa.Person == null);
      Assert.IsTrue(sa.Tid.Date == DateTime.Now.Date);
      Assert.IsTrue(sa.Tid.Hour == DateTime.Now.Hour);

      sa = _statusAnmeldelseBusiness.CreateStatus(EnumStatusAnmeldelse.Afsendt, null);
      Assert.IsTrue(sa.StatusAnmeldelseType.Navn.ToLower().Contains("afsendt"));
      Assert.IsTrue(sa.Person == null);
      Assert.IsTrue(sa.Tid.Date == DateTime.Now.Date);
      Assert.IsTrue(sa.Tid.Hour == DateTime.Now.Hour);
    }

  }
}
