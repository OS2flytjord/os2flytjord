using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Infrastructure.DataAccess;

namespace Niras.Jordflytning.IntegrationTests.Core
{
  [TestFixture]
  public class PdfReposotoryTest
  {
    private readonly IPdfServiceRepository _repo;
    
    private readonly IKernel _ninjectKernel;

    public PdfReposotoryTest()
    {
      // Init Ninject kernel
      _ninjectKernel = new StandardKernel();
      _ninjectKernel.Bind<IPdfServiceRepository>().To<PdfServiceRepository>();

      _repo = _ninjectKernel.Get<IPdfServiceRepository>();
    }

    [Test]
    public void TestService()
    {
      Assert.IsTrue(_repo.UrlToPdf(Guid.NewGuid(), @"http://niras.dk"));
    }

  }
}
