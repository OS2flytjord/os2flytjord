using System;
using System.Collections.Generic;
using System.Linq;
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
using Niras.Jordflytning.UnitTests.TestsSetup.DependencyResolution;

namespace Niras.Jordflytning.UnitTests.Core.BusinessLogic
{
  [TestFixture]
  public class BetalerBusinessTests
  {
    // Ninject kernel
    private IKernel _ninjectKernel;
    private IBetalerBusiness betalerBusiness;
    private Mock<IBetalerRepository> mockBetalerRepo;

    public BetalerBusinessTests()
    {
      _ninjectKernel = new StandardKernel
         (
         new ConfigModule(),
         new RepositoryModule()
         );
      _ninjectKernel.Bind<IUnitOfWork>().To<UnitOfWork>();

      mockBetalerRepo = new Mock<IBetalerRepository>();
      _ninjectKernel.Bind<IBetalerRepository>().ToConstant(mockBetalerRepo.Object);

      _ninjectKernel.Bind<IBetalerBusiness>().To<BetalerBusiness>();
      betalerBusiness = _ninjectKernel.Get<IBetalerBusiness>();
    }

    [Test]
    public void ReadBetalerTest()
    {
      Guid g = Guid.NewGuid();
      Guid gg = Guid.NewGuid();

      mockBetalerRepo.Setup(x => x.Read(g)).Returns((Betaler) null);
     var b = betalerBusiness.ReadBetaler(g);
     
      Assert.IsTrue(b.Id != gg);
      Assert.IsTrue(b.Id==g);

      var mb = new Betaler {Id = gg};
      mockBetalerRepo.Setup(x => x.Read(g)).Returns(mb);

      var bb = betalerBusiness.ReadBetaler(gg);
      Assert.IsTrue(bb.Id == gg);
      


    }

   

  }
}
