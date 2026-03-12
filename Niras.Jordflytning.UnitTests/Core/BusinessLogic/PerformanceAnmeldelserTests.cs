using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Spatial;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using System.Linq;
using Niras.Jordflytning.Infrastructure.Common;
using Niras.Jordflytning.Infrastructure.DataAccess;
using Niras.Jordflytning.UnitTests.TestsSetup.DependencyResolution;

namespace Niras.Jordflytning.UnitTests.Core.BusinessLogic
{
  [TestFixture]
  public class PerfomanceAnmeldelserTests
  {

    private readonly IKernel _ninjectKernel;
    private readonly IAnmeldelserRepository _repo;
    private Anmeldelse _anmeldelse;

    private readonly IAnmeldelserBusiness _anmeldelserBusiness;


    public PerfomanceAnmeldelserTests()//public AnmeldelserTests(IAnmeldelserRepository repo)
    {
      //_repo = repo;
      _ninjectKernel = new StandardKernel
         (
         new ConfigModule(),
         new RepositoryModule()
         );
      try
      {
        _repo = _ninjectKernel.Get<IAnmeldelserRepository>();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        throw;
      }
      



      //_ninjectKernel.Bind<IStatusAnmeldelseBusiness>().To<StatusAnmeldelseBusiness>();
      //_ninjectKernel.Bind<IDokumentationBusiness>().To<DokumentationBusiness>();
      //_ninjectKernel.Bind<IAdviseringBusiness>().To<AdviseringBusiness>();
      //_ninjectKernel.Bind<IKonfigBusiness>().To<KonfigBusiness>();
      //_ninjectKernel.Bind<IKodelisteBusiness>().To<KodelisteBusiness>();
      //_ninjectKernel.Bind<IStatusBetalerBusiness>().To<StatusBetalerBusiness>();
      //_ninjectKernel.Bind<IKommunikationBusiness>().To<KommunikationBusiness>();
      //_ninjectKernel.Bind<IBetalerBusiness>().To<BetalerBusiness>();
      //_ninjectKernel.Bind<IBrugereBusiness>().To<BrugereBusiness>();
      //_ninjectKernel.Bind<ISecurityProvider>().To<WebSecurityProvider>();
      //_ninjectKernel.Bind<IJordmodtagerBusiness>().To<JordmodtagerBusiness>();
      //_ninjectKernel.Bind<IPdfBusiness>().To<PdfBusiness>();
      //_ninjectKernel.Bind<IAlarmBusiness>().To<AlarmBusiness>();
      //_ninjectKernel.Bind<IModtagerAnlaegBusiness>().To<ModtagerAnlaegBusiness>();
      //_ninjectKernel.Bind<ITransportoerBusiness>().To<TransportoerBusiness>();
      //_ninjectKernel.Bind<IMailBusiness>().To<MailBusiness>();

      //_ninjectKernel.Bind<ILogBusiness>().To<LogBusiness>();


      //_ninjectKernel.Bind<IAnmeldelserBusiness>().To<AnmeldelserBusiness>();
      //_anmeldelserBusiness = _ninjectKernel.Get<IAnmeldelserBusiness>();


    }

    /// <summary>
    /// Tester instantiering af Anmeldelse objekt
    /// </summary>
    [Test]
    public void AlmRead()
    {
      int i = 0;
      var t1 = DateTime.Now;
      var anmeldelser = _repo.Read();
      foreach (var anmeldelse in anmeldelser)
      {
        foreach (var status in anmeldelse.StatusAnmeldelse)
        {
          var id = status.StatusAnmeldelseType.Kode;
          
          //Console.WriteLine(id);
          i++;
        }
      }
      Console.WriteLine((DateTime.Now - t1).Milliseconds);
      Console.WriteLine("Antal status anmeldelser: " + anmeldelser.Count());
      Console.WriteLine("Antal status statusanmeldelser: " + i);

    }

    [Test]
    public void ReadIncludeStatusAnmeldelseTest()
    {
      int i = 0;
      var t1 = DateTime.Now;
      var anmeldelser = _repo.Read().Where(a=>a.Kommune.Kommunenr == 751);
      foreach (var anmeldelse in anmeldelser)
      {
        foreach (var status in anmeldelse.StatusAnmeldelse)
        {
          var id = status.StatusAnmeldelseType.Kode;
          //Console.WriteLine(id);
          i++;
        }
      }
      Console.WriteLine("Tid: " +(DateTime.Now - t1).Milliseconds);
      Console.WriteLine("Antal status anmeldelser: " + anmeldelser.Count());
      Console.WriteLine("Antal status statusanmeldelser: " + i);

    }


    [Test]
    public void SearchIncludeTest()
    {
      int i = 0;
      var t1 = DateTime.Now;
      var anmeldelser = _repo.Search(a => a.Kommune.Kommunenr == 751);

      foreach (var anmeldelse in anmeldelser)
      {
        foreach (var status in anmeldelse.StatusAnmeldelse)
        {
          var id = status.StatusAnmeldelseType.Kode;
          //Console.WriteLine(id);
          i++;
        }
      }
      Console.WriteLine("Tid: " + (DateTime.Now - t1).Milliseconds);
      Console.WriteLine("Antal status anmeldelser: " + anmeldelser.Count());
      Console.WriteLine("Antal status statusanmeldelser: " + i);

    }

  }
}
