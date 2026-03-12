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
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using Niras.Jordflytning.Infrastructure.Common;
using Niras.Jordflytning.UnitTests.TestsSetup.DependencyResolution;

namespace Niras.Jordflytning.UnitTests.Core.BusinessLogic
{
  [TestFixture]
  public class AdvisBussnessTest
  {
      // Ninject kernel
    private IKernel _ninjectKernel;
    private IAdviseringBusiness advisBusiness;
    
    public AdvisBussnessTest()
    {
    
      _ninjectKernel = new StandardKernel
         (
         new ConfigModule(),
         new RepositoryModule()
         );
      
      _ninjectKernel.Bind<IStatusAnmeldelseBusiness>().To<StatusAnmeldelseBusiness>();
      _ninjectKernel.Bind<IDokumentationBusiness>().To<DokumentationBusiness>();
      _ninjectKernel.Bind<IAdviseringBusiness>().To<AdviseringBusiness>();
      _ninjectKernel.Bind<IKonfigBusiness>().To<KonfigBusiness>();
      _ninjectKernel.Bind<IKodelisteBusiness>().To<KodelisteBusiness>();
      _ninjectKernel.Bind<IStatusBetalerBusiness>().To<StatusBetalerBusiness>();
      _ninjectKernel.Bind<IKommunikationBusiness>().To<KommunikationBusiness>();
      _ninjectKernel.Bind<IBetalerBusiness>().To<BetalerBusiness>();
      _ninjectKernel.Bind<IBrugereBusiness>().To<BrugereBusiness>();
      _ninjectKernel.Bind<ISecurityProvider>().To<WebSecurityProvider>();
      _ninjectKernel.Bind<IJordmodtagerBusiness>().To<JordmodtagerBusiness>();
      _ninjectKernel.Bind<IPdfBusiness>().To<PdfBusiness>();
      _ninjectKernel.Bind<IAlarmBusiness>().To<AlarmBusiness>();

      _ninjectKernel.Bind<IMailBusiness>().To<MailBusiness>();
      
      
      advisBusiness = _ninjectKernel.Get<IAdviseringBusiness>();
    }

    [Test]
    public void RemoveHttpLinkFromAdvis()
    {
      var input = "<a href=\"http:\\dr.dk\">HttP:\\Dr.dk</a>";
      
      var res = advisBusiness.AdvisUdenHttp(input);
      Assert.IsFalse(res.ToLower().Contains("http"));


    }
  }
}
