using System;
using System.Collections;
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
using Niras.Jordflytning.Infrastructure.DataAccess;
using Niras.Jordflytning.IntegrationTests.TestsSetup.DependencyResolution;


namespace Niras.Jordflytning.IntegrationTests.Core
{
  [TestFixture]
  public class StatusBetalerTests
  {
    // Ninject kernel
    private IKernel _ninjectKernel;

    private Mock<IStatusBetalerRepository> mockStatusBetaler;
    private IStatusBetalerBusiness statusBetalerBusiness;
    public StatusBetalerTests()
    {
      // Init Ninject kernel
      _ninjectKernel = new StandardKernel
        (
        new ConfigModule(),
        new RepositoryModule()
        );
      _ninjectKernel.Bind<IUnitOfWork>().To<UnitOfWork>();

      mockStatusBetaler = new Mock<IStatusBetalerRepository>();
      _ninjectKernel.Bind<IStatusBetalerRepository>().ToConstant(mockStatusBetaler.Object);

      _ninjectKernel.Bind<IStatusBetalerBusiness>().To<StatusBetalerBusiness>();
      statusBetalerBusiness = _ninjectKernel.Get<IStatusBetalerBusiness>();

    }
    
    //[Ignore]
    //[Test]
    //public void GetStatusBetalerForBetalerTest()
    //{
    //  //Mock data
    //  Guid gb1 = Guid.Parse("82EDC586-96E7-4644-ACEF-FC39E394E8C5");
    //  Guid gb2 = Guid.NewGuid();
    //  Guid gb3 = Guid.NewGuid();
    //  var b1 = new Betaler { Id = gb1 };
    //  var b2 = new Betaler { Id = gb2 };

    //  Guid gj1 = Guid.NewGuid();
    //  Guid gj2 = Guid.NewGuid();
    //  Guid gj3 = Guid.NewGuid();
    //  var j1 = new Jordmodtager { Id = gj1 };
    //  var j2 = new Jordmodtager { Id = gj2 };
    //  var j3 = new Jordmodtager { Id = gj3 };

    //  var sb_b1j1 = new StatusBetaler { Betaler = b1, Jordmodtager = j1, Godkendt = true };
    //  var sb_b1j2 = new StatusBetaler { Betaler = b1, Jordmodtager = j2, Godkendt = false };
    //  var sb_b1j3 = new StatusBetaler { Betaler = b1, Jordmodtager = j3 }; // ikke behandlet endnu af bogholder.

    //  //var sb_b2j1 = new StatusBetaler { Betaler = b2, Jordmodtager = j1, Godkendt = true };
    //  //var sb_b2j2 = new StatusBetaler { Betaler = b2, Jordmodtager = j2, Godkendt = false };
    //  //var sb_b2j3 = new StatusBetaler { Betaler = b2, Jordmodtager = j3 }; // ikke behandlet endnu af bogholder.

    //  var list = new List<StatusBetaler>();
    //  list.Add(sb_b1j1);
    //  list.Add(sb_b1j2);
    //  list.Add(sb_b1j3);
    //  IQueryable<StatusBetaler> sbList = new EnumerableQuery<StatusBetaler>(list);

    //  //sbList.Add(sb_b2j1);
    //  //sbList.Add(sb_b2j2);
    //  //sbList.Add(sb_b2j3);

    //  //Problemet er her. Vi ved ikke hvordan Search mockes.
    //  mockStatusBetaler.Setup(f => f.Search(x => x.Betaler.Id == gb1 && x.Godkendt.HasValue)).Returns(sbList);

    //  var sb = statusBetalerBusiness.GetStatusBetalerForBetaler(b1);
    //  Assert.IsTrue(sb.Contains(sb_b1j1));
    //  Assert.IsTrue(sb.Contains(sb_b1j2));
    //  Assert.IsTrue(!sb.Contains(sb_b1j3));


    //}


  }
}
