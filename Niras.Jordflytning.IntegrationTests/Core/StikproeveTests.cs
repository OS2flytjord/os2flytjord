using System;
using System.Collections.Generic;
using Ninject;
using NUnit.Framework;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Infrastructure.Common;
using Niras.Jordflytning.Infrastructure.DependencyResolution;
using System.Linq;

namespace Niras.Jordflytning.IntegrationTests.Core
{
  [TestFixture]

  public class StikproeveTests
  {
    // Ninject kernel
    private readonly IKernel _ninjectKernel;
    private readonly IStikproeveBusiness _stikBusi;
    private readonly IStatusStikproeveBusiness _statusBusi;
    private readonly IAdviseringBusiness _adviseringBusiness;


    public StikproeveTests()
    {
      // Init Ninject kernel
      _ninjectKernel = new StandardKernel
          (
            new ConfigModule(),
            new RepositoryModule()
          );

      _ninjectKernel.Bind<IStikproeveBusiness>().To<StikproeveBusiness>();
      _ninjectKernel.Bind<IUnitOfWork>().To<UnitOfWork>();
      _ninjectKernel.Bind<IStatusStikproeveBusiness>().To<StatusStikproeveBusiness>();
      _stikBusi = _ninjectKernel.Get<IStikproeveBusiness>();
      _statusBusi = _ninjectKernel.Get<IStatusStikproeveBusiness>();

      _ninjectKernel.Bind<IKodelisteBusiness>().To<KodelisteBusiness>();
      _ninjectKernel.Bind<IKommunikationBusiness>().To<KommunikationBusiness>();
      _ninjectKernel.Bind<IStatusBetalerBusiness>().To<StatusBetalerBusiness>();
      _ninjectKernel.Bind<IMailBusiness>().To<MailBusiness>();

      _ninjectKernel.Bind<IAdviseringBusiness>().To<AdviseringBusiness>();
      _adviseringBusiness = _ninjectKernel.Get<IAdviseringBusiness>();

      

      
        
        


    }

    [SetUp]
    public void Setup()
    {

    }

    [TearDown]
    public void TearDown()
    {

    }


    /// <summary>
    /// 
    /// </summary>
    [Test]
    public void CreateStikproeve()
    {
      // Arrange
      Stikproeve stikproeve = new Stikproeve();


      stikproeve.InternBemaerkning = "itrnbmk";

      // Act
      _stikBusi.Create(stikproeve);

      // Assert
      Assert.IsNotNull(stikproeve);
      Assert.IsTrue(stikproeve.Id != Guid.Empty);
      Assert.IsTrue(stikproeve.Nummer > 0);
      Assert.IsTrue("itrnbmk" == stikproeve.InternBemaerkning);
    }

    /// <summary>
    /// 
    /// </summary>
    [Test]
    public void CreateStatusStikproeve()
    {
      // Arrange
      Stikproeve stikproeve = new Stikproeve();
      Person person = new Person();
      person.Navn = "navn";
      person.Adresse = "adresse";
      person.Efternavn = "efternavn";
      person.Email = "segseg@segseaf11234g.dk";
      stikproeve.InternBemaerkning = "itrnbmk";

      // Act
      _stikBusi.Create(stikproeve);
      stikproeve.StatusStikproeve.Add(_statusBusi.CreateStatus(EnumStatusStikproeve.Planlagt, person));
      _stikBusi.SaveChanges();
      // Assert
      Assert.IsTrue(stikproeve.StatusStikproeve.First().Id != Guid.Empty);


      //Act part II
      stikproeve.StatusStikproeve.Add(_statusBusi.CreateStatus(EnumStatusStikproeve.AnalyseAfvistPgaAffald, person));
      stikproeve.StatusStikproeve.Add(_statusBusi.CreateStatus(EnumStatusStikproeve.AnalyseGodkendt, person));


      //Assert part II
      Assert.IsTrue(_statusBusi.GodkendtAfvistUnderbehandling(stikproeve.StatusStikproeve.ToList()).StatusStikproeveType.Kode == 8);
    }



    [Test]
    public void SkalProevetagerAdviseresTest()
    {
      ModtagerAnlaeg m = new ModtagerAnlaeg();
      m.AnvenderJF = true;
      m.AntalBaase = 20;
      var s = new Stikproeve();

      s.Baas = 9;
      Assert.IsFalse(_stikBusi.SkalProevetagerAdviseres(s, m));

      s.Baas = 10;
      Assert.IsTrue(_stikBusi.SkalProevetagerAdviseres(s, m));

      s.Baas = 19;
      Assert.IsFalse(_stikBusi.SkalProevetagerAdviseres(s, m));

      s.Baas = 20;
      Assert.IsTrue(_stikBusi.SkalProevetagerAdviseres(s, m));


      m.AntalBaase = 19;
      s.Baas = 19;
      Assert.IsTrue(_stikBusi.SkalProevetagerAdviseres(s, m));

      s.Baas = 9;
      Assert.IsTrue(_stikBusi.SkalProevetagerAdviseres(s, m));

      s.Baas = 10;
      Assert.IsFalse(_stikBusi.SkalProevetagerAdviseres(s, m));


    }

    [Ignore]
    [Test]
    public void AdviserProevetagerTest()
    {
      ModtagerAnlaeg m = new ModtagerAnlaeg();
      m.AnvenderJF = true;
      m.AntalBaase = 20;
      m.Navn = "Test modtageranlæg";
      m.Jordmodtager = new Jordmodtager() { Id = new Guid("B3081EB9-20B0-4E0E-AE8D-B09B8EDBA04A") };

      var s = new Stikproeve();
      s.Baas = 10;

      var skalProevetagerAdvisers = _stikBusi.SkalProevetagerAdviseres(s, m);
      var proevetagers = new List<Person>();
      proevetagers.Add(new Person() { Email = "kve@niras.dk" });
      if (skalProevetagerAdvisers)
      {
        _adviseringBusiness.SendBeskedTilProevetagerTidTilJordproever(m, proevetagers,s);
      }
    }


  }

}

