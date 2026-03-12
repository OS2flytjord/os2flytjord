using System;
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
  public class JordModtagerBusinessTests
  {

    // Ninject kernel
    private readonly IKernel _ninjectKernel;
    private readonly IJordmodtagerBusiness _jordModtagerBusiness;

		public JordModtagerBusinessTests()
    {
      // Init Ninject kernel
      _ninjectKernel = new StandardKernel
          (
            new ConfigModule(),
            new RepositoryModule()
          );
			_ninjectKernel.Bind<IUnitOfWork>().To<UnitOfWork>();
			_ninjectKernel.Bind<IJordmodtagerBusiness>().To<JordmodtagerBusiness>();
			_jordModtagerBusiness = _ninjectKernel.Get<IJordmodtagerBusiness>();     
    }

    [SetUp]
    public void Setup()
    { }

    [TearDown]
    public void TearDown()
    { }

    [Test]
		public void GetJordModtagerListForUserTest()
    {
	    var personId = new Guid("8721e8a4-1574-4c49-b0b8-60460d491264");
			var list = _jordModtagerBusiness.GetJordModtagerListForUser(personId);

      Assert.IsTrue( list.Count == 2);
    }

  }
}
