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
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.UnitTests.TestsSetup.DependencyResolution;

namespace Niras.Jordflytning.UnitTests.Core.BusinessLogic
{
  [TestFixture]
  public class StatusAnmeldelseBussnessTests
  {
    private IKernel _ninjectKernel;
    private IStatusAnmeldelseBusiness _statusAnmeldelseBusiness;
    private Mock<IStatusAnmeldelseRepository> mockStatusAnmeldelse;

    public StatusAnmeldelseBussnessTests()
    {
      _ninjectKernel = new StandardKernel
         (
         new ConfigModule(),
         new RepositoryModule()
         );

      mockStatusAnmeldelse = new Mock<IStatusAnmeldelseRepository>();
      _ninjectKernel.Bind<IStatusAnmeldelseRepository>().ToConstant(mockStatusAnmeldelse.Object);

      _ninjectKernel.Bind<IStatusAnmeldelseBusiness>().To<StatusAnmeldelseBusiness>();
      _statusAnmeldelseBusiness = _ninjectKernel.Get<IStatusAnmeldelseBusiness>();

    }

    [Test]
    public void GetStatusTypesForSagsbehandler()
    {

      var res = _statusAnmeldelseBusiness.GetStatusTypesForKommune();
      Assert.IsTrue(res != null);
      Assert.IsTrue(res.Count == 3);
      Assert.IsTrue(res[0].Kode == 6);
      Assert.IsTrue(res[1].Kode == 7);
      Assert.IsTrue(res[2].Kode == 8);
    }

    //[Test]
    //public void GetLastKommuneStatusExcludeGemTest()
    //{
    //  var s = new List<StatusAnmeldelse>();

    //  s.Add(new StatusAnmeldelse{
    //    Id= Guid.NewGuid(),
    //    StatusAnmeldelseType = new StatusAnmeldelseType{Id=Guid.NewGuid(),Kode = (short)EnumStatusAnmeldelse.Oprettet},
    //    Tid = new DateTime(2013,1,1)
    //  });

    //  s.Add(new StatusAnmeldelse
    //  {
    //    Id = Guid.NewGuid(),
    //    StatusAnmeldelseType = new StatusAnmeldelseType { Id = Guid.NewGuid(), Kode = (short)EnumStatusAnmeldelse.Gemt },
    //    Tid = new DateTime(2013, 1, 2)
    //  });
    //  s.Add(new StatusAnmeldelse
    //  {
    //    Id = Guid.NewGuid(),
    //    StatusAnmeldelseType = new StatusAnmeldelseType { Id = Guid.NewGuid(), Kode = (short)EnumStatusAnmeldelse.Afsendt },
    //    Tid = new DateTime(2013, 1, 3)
    //  });
    //  s.Add(new StatusAnmeldelse
    //  {
    //    Id = Guid.NewGuid(),
    //    StatusAnmeldelseType = new StatusAnmeldelseType { Id = Guid.NewGuid(), Kode = (short)EnumStatusAnmeldelse.UnderbehandlingAfKommunen },
    //    Tid = new DateTime(2013, 1, 4)
    //  });
    //  s.Add(new StatusAnmeldelse
    //  {
    //    Id = Guid.NewGuid(),
    //    StatusAnmeldelseType = new StatusAnmeldelseType { Id = Guid.NewGuid(), Kode = (short)EnumStatusAnmeldelse.Gemt },
    //    Tid = new DateTime(2013, 1, 5)
    //  });

    //  var laststatus = _statusAnmeldelseBusiness.GetLastStatusTypeForKommune(s);
    //  Assert.IsTrue(laststatus.Kode == (short)EnumStatusAnmeldelse.UnderbehandlingAfKommunen);
    //}

    //[Test]
    //public void GetLastJordmodtagerStatusExcludeGemTest()
    //{
    //  var s = new List<StatusAnmeldelse>();

    //  s.Add(new StatusAnmeldelse
    //  {
    //    Id = Guid.NewGuid(),
    //    StatusAnmeldelseType = new StatusAnmeldelseType { Id = Guid.NewGuid(), Kode = (short)EnumStatusAnmeldelse.Oprettet },
    //    Tid = new DateTime(2013, 1, 1)
    //  });

    //  s.Add(new StatusAnmeldelse
    //  {
    //    Id = Guid.NewGuid(),
    //    StatusAnmeldelseType = new StatusAnmeldelseType { Id = Guid.NewGuid(), Kode = (short)EnumStatusAnmeldelse.Gemt },
    //    Tid = new DateTime(2013, 1, 2)
    //  });
    //  s.Add(new StatusAnmeldelse
    //  {
    //    Id = Guid.NewGuid(),
    //    StatusAnmeldelseType = new StatusAnmeldelseType { Id = Guid.NewGuid(), Kode = (short)EnumStatusAnmeldelse.JordmodtagerAcceptererJorden },
    //    Tid = new DateTime(2013, 1, 3)
    //  });
    //  s.Add(new StatusAnmeldelse
    //  {
    //    Id = Guid.NewGuid(),
    //    StatusAnmeldelseType = new StatusAnmeldelseType { Id = Guid.NewGuid(), Kode = (short)EnumStatusAnmeldelse.JordmodtagerAfviserJorden },
    //    Tid = new DateTime(2013, 1, 4)
    //  });
    //  s.Add(new StatusAnmeldelse
    //  {
    //    Id = Guid.NewGuid(),
    //    StatusAnmeldelseType = new StatusAnmeldelseType { Id = Guid.NewGuid(), Kode = (short)EnumStatusAnmeldelse.Gemt },
    //    Tid = new DateTime(2013, 1, 5)
    //  });

    //  var laststatus = _statusAnmeldelseBusiness.GetLastStatusTypeForJordmodtager(s);
    //  Assert.IsTrue(laststatus.Kode == (short)EnumStatusAnmeldelse.JordmodtagerAfviserJorden);
    //}


    //[Test]
    //public void GetLastBetalerStatusExcludeGemTest()
    //{
    //  var s = new List<StatusAnmeldelse>();

    //  s.Add(new StatusAnmeldelse
    //  {
    //    Id = Guid.NewGuid(),
    //    StatusAnmeldelseType = new StatusAnmeldelseType { Id = Guid.NewGuid(), Kode = (short)EnumStatusAnmeldelse.Oprettet },
    //    Tid = new DateTime(2013, 1, 1)
    //  });

    //  s.Add(new StatusAnmeldelse
    //  {
    //    Id = Guid.NewGuid(),
    //    StatusAnmeldelseType = new StatusAnmeldelseType { Id = Guid.NewGuid(), Kode = (short)EnumStatusAnmeldelse.Gemt },
    //    Tid = new DateTime(2013, 1, 2)
    //  });
    //  s.Add(new StatusAnmeldelse
    //  {
    //    Id = Guid.NewGuid(),
    //    StatusAnmeldelseType = new StatusAnmeldelseType { Id = Guid.NewGuid(), Kode = (short)EnumStatusAnmeldelse.BetalerAccepteretBetalingen },
    //    Tid = new DateTime(2013, 1, 3)
    //  });
    //  s.Add(new StatusAnmeldelse
    //  {
    //    Id = Guid.NewGuid(),
    //    StatusAnmeldelseType = new StatusAnmeldelseType { Id = Guid.NewGuid(), Kode = (short)EnumStatusAnmeldelse.BetalerAfviserBetalingen },
    //    Tid = new DateTime(2013, 1, 4)
    //  });
    //  s.Add(new StatusAnmeldelse
    //  {
    //    Id = Guid.NewGuid(),
    //    StatusAnmeldelseType = new StatusAnmeldelseType { Id = Guid.NewGuid(), Kode = (short)EnumStatusAnmeldelse.Gemt },
    //    Tid = new DateTime(2013, 1, 5)
    //  });

    //  var laststatus = _statusAnmeldelseBusiness.GetLastStatusTypeForBetaler(s);
    //  Assert.IsTrue(laststatus.Kode == (short)EnumStatusAnmeldelse.BetalerAfviserBetalingen);
    //}



    [Test]
    public void GetStatusTypesForJordmodtager()
    {

      var res = _statusAnmeldelseBusiness.GetStatusTypesForJordmodtager();
      Assert.IsTrue(res != null);
      Assert.IsTrue(res.Count == 2);
      Assert.IsTrue(res[0].Kode == 9 | res[0].Kode == 10);
      Assert.IsTrue(res[1].Kode == 9 | res[1].Kode == 10);
      Assert.IsTrue(res[1].Kode != res[0].Kode );
      
    }

    [Test]
    public void GetStatusTypesForBetaler()
    {

      var res = _statusAnmeldelseBusiness.GetStatusTypesForBetaler();
      Assert.IsTrue(res != null);
      Assert.IsTrue(res.Count == 2);
      Assert.IsTrue(res[0].Kode == 4 | res[0].Kode == 5);
      Assert.IsTrue(res[1].Kode == 4 | res[1].Kode == 5);
      Assert.IsTrue(res[1].Kode != res[0].Kode);

    }
  }
}
