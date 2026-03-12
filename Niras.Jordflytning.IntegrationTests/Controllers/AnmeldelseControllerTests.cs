using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Controllers;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.IntegrationTests.TestsSetup;
using Niras.Jordflytning.IntegrationTests.TestsSetup.DependencyResolution;
using Niras.Jordflytning.ViewModels.Anmeldelse;

namespace Niras.Jordflytning.IntegrationTests.Controllers
{
  [TestFixture]
  public class AnmeldelseControllerTests
  {
    private IKernel _ninjectKernel;
    private Mock<IPrincipal> _user;
    private Mock<IIdentity> _identity;
    private Mock<ISecurityProvider> _securityProvider;
    //private IModtagerAnlaegBusiness _modtagerAnlaegBusiness;
    //private ITransportoerBusiness _tranportoerBusiness;
    private IKodelisteBusiness _kodelisteBusiness;


    public AnmeldelseControllerTests()
    {
      // Init Ninject kernel
      _ninjectKernel = new StandardKernel
          (
            new ConfigModule(),
            new RepositoryModule()
          );

      _securityProvider = new Mock<ISecurityProvider>();

      _user = new Mock<IPrincipal>();
      _identity = new Mock<IIdentity>();
      _identity.Setup(i => i.Name).Returns("kve@niras.dk");
      _user.Setup(u => u.Identity).Returns(_identity.Object);

      _securityProvider.Setup(s => s.CurrentUser).Returns(_user.Object as IPrincipal);
      _securityProvider.Setup(s => s.GetUserId("kve@niras.dk")).Returns(7);
      _securityProvider.Setup(s => s.GetUserRoles("kve@niras.dk")).Returns(new List<string>());

      _ninjectKernel.Bind<ISecurityProvider>().ToConstant(_securityProvider.Object);

      _kodelisteBusiness=  TestSetupUtil.InjectKodelists(_ninjectKernel);

      //// create the mock http context
      //var fakeHttpContext = new Mock();

      //// create a mock session object and hook it up to the mock http context
      //var sessionMock = new HttpSessionMock { { "selectedYear", 2013 }, { "selectedMonth", 10 } };
      //var mockSessionState = new Mock();
      //fakeHttpContext.Setup(ctx => ctx.Session).Returns(sessionMock);

      _ninjectKernel.Bind<AnmeldelserController>().ToSelf();

      //_ninjectKernel.Bind<IStatusAnmeldelseBusiness>().To<StatusAnmeldelseBusiness>();
      //_statusAnmeldelseBusiness= _ninjectKernel.Get<IStatusAnmeldelseBusiness>();

      //_ninjectKernel.Bind<IAnmeldelserBusiness>().To<AnmeldelserBusiness>();
      //_anmeldelserBusiness = _ninjectKernel.Get<IAnmeldelserBusiness>();

    }

    /// <summary>
    /// Ensures that a anmeldelse can be saved
    /// </summary>
    [Test]
    public void ShouldSaveAnmeldelse()
    {
      // Arrange
      var controller = _ninjectKernel.Get<AnmeldelserController>();
      var route = new RouteData();
      //route.Values.Add("controller", "AnmeldelserController");
      //route.Values.Add("action", "Opret");

      var session = new Dictionary<string, object>();
      var matrikel = new Matrikel() { Ejerlav = "1020451", Ejerlavsnavn = "Hasle By, Hasle", Matrikelnr = "16o" };
      matrikel.Geom = DbGeometry.FromText("POLYGON ((571388.209 6225258.161, 571391.957 6225262.58, 571396.314 6225266.896, 571398.645 6225268.645, 571006.962 6225387.092, 570997.382 6225250.512, 570996.622 6225239.547, 571010.665 6225239.225, 571024.699 6225238.64, 571038.72 6225237.788, 571052.722 6225236.673, 571066.7 6225235.294, 571082.931 6225233.285, 571099.113 6225230.921, 571115.24 6225228.202, 571131.304 6225225.128, 571147.295 6225221.703, 571163.208 6225217.927, 571179.033 6225213.803, 571194.765 6225209.331, 571210.394 6225204.516, 571213.948 6225214.871, 571216.94 6225213.899, 571228.612 6225210.044, 571240.217 6225205.999, 571321.234 6225176.686, 571325.528 6225177.193, 571388.209 6225258.161))", 25832);
      var sessionMatrikler = new List<Matrikel>();
      sessionMatrikler.Add(matrikel);
      session["Matrikler"] = sessionMatrikler;

      controller.SetFakeControllerContext(route, session);

      var opretModel = new OpretModel();
      var command = "Gem";


      var anmelder = new Anmelder() { Id = new Guid("2BB90CF6-DEE4-45F2-AE65-EE103B8A9A43") };
      var betaler = new Betaler() { Id = new Guid("2BB90CF6-DEE4-45F2-AE65-EE103B8A9A43") };
      var jord = new Jord()
        {
          ForventetJordmaengdeTon = 1000,
          IntaktJord = false,
          JordflytningType = new JordflytningType() { Id = new Guid("1DB6CAC8-8335-49B1-A5AF-65DB18D7F9CB") },
          Jordproever = false,
          JordproeverFoer = true
        };

      var kommune = new Kommune() { Id = new Guid("A15D5888-BC70-4206-871E-A47A0C05DECC"), Kommunenr = 751, Navn = "Aarhus" };

      var oprindelsesSted = new Oprindelsessted() { Adresse = "Bautavej 1", PostDistrikt = "Aarhus V", Postnummer = 8210 };
      oprindelsesSted.OprindelsesstedKlassifikationType = new OprindelsesstedKlassifikationType() { Id = new Guid("BBB062F1-641A-45AD-BEDD-B0C126FCDFE6") };



      opretModel.Anmeldelse = new Anmeldelse()
        {
          Anmelder = anmelder,
          Betaler = betaler,
          Jord = jord,
          Kommune = kommune,
          Oprindelsessted = oprindelsesSted,
        };

      opretModel.ForureningOpslagKraeverAnmeldelse = true;
      opretModel.IndeholderAffald = false;
      opretModel.Jordhaandteringsplan = false;
      opretModel.MapStedLx = "571113";
      opretModel.MapStedLy = "6225161";
      opretModel.MapStedUx = "571313";
      opretModel.MapStedUy = "6225361";
      opretModel.MatrikelWkt = "";
      opretModel.ModtagerAnlaegFiltreringAnvenderJf = false;
      opretModel.OprindelsesKommuneAnvenderFlytJord = true;
      opretModel.OprindelsesstedWkt = "POINT(571213 6225261)";
      opretModel.SelectedJordklassifikationType = new Guid("1426C54A-6C06-4970-AC52-327D013751C7");//Kategori 1



      // Act
      var view = controller.Opret(opretModel, command) as PartialViewResult;
      var model = (OpretModel)view.Model;

      // Assert
      Assert.IsFalse(model.Anmeldelse.Id.Equals(Guid.Empty), "Anmeldelse blev ikke oprettet.");

    }

    
    [Test]
    public void ShouldAutogodkendAnmeldelse()
    {
      // Arrange
      var controller = _ninjectKernel.Get<AnmeldelserController>();
      var route = new RouteData();
      //route.Values.Add("controller", "AnmeldelserController");
      //route.Values.Add("action", "Opret");

      var session = new Dictionary<string, object>();
      //Matrikler
      var matrikel = new Matrikel() { Ejerlav = "1020451", Ejerlavsnavn = "Hasle By, Hasle", Matrikelnr = "16o" };
      matrikel.Geom = DbGeometry.FromText("POLYGON ((571388.209 6225258.161, 571391.957 6225262.58, 571396.314 6225266.896, 571398.645 6225268.645, 571006.962 6225387.092, 570997.382 6225250.512, 570996.622 6225239.547, 571010.665 6225239.225, 571024.699 6225238.64, 571038.72 6225237.788, 571052.722 6225236.673, 571066.7 6225235.294, 571082.931 6225233.285, 571099.113 6225230.921, 571115.24 6225228.202, 571131.304 6225225.128, 571147.295 6225221.703, 571163.208 6225217.927, 571179.033 6225213.803, 571194.765 6225209.331, 571210.394 6225204.516, 571213.948 6225214.871, 571216.94 6225213.899, 571228.612 6225210.044, 571240.217 6225205.999, 571321.234 6225176.686, 571325.528 6225177.193, 571388.209 6225258.161))", 25832);
      var sessionMatrikler = new List<Matrikel>();
      sessionMatrikler.Add(matrikel);
      session["Matrikler"] = sessionMatrikler;

      //Jordforureningopslag
      var jordforureningsopslag = new Jordforureningsopslag();
      jordforureningsopslag.JordKlassifikationType = _kodelisteBusiness.ReadJordKlassifikationType(new Guid("1426C54A-6C06-4970-AC52-327D013751C7"));
      jordforureningsopslag.Tid = DateTime.Now;
      session["Jordforureningsopslag"] = jordforureningsopslag;

      controller.SetFakeControllerContext(route, session);

      var opretModel = new OpretModel();
      var command = "Indsend";


      var anmelder = new Anmelder() { Id = new Guid("2BB90CF6-DEE4-45F2-AE65-EE103B8A9A43") };
      var betaler = new Betaler() { Id = new Guid("2BB90CF6-DEE4-45F2-AE65-EE103B8A9A43") };
      var jord = new Jord()
      {
        ForventetJordmaengdeTon = 1000,
        IntaktJord = false,
        JordflytningType = new JordflytningType() { Id = new Guid("1DB6CAC8-8335-49B1-A5AF-65DB18D7F9CB") },
        Jordproever = false,
        JordproeverFoer = true, 
        KoerselStart = DateTime.Now.Date,
        KoerselSlut = DateTime.Now.AddDays(7).Date,
        JordKlassifikationType = jordforureningsopslag.JordKlassifikationType

      };

      var kommune = new Kommune() { Id = new Guid("A15D5888-BC70-4206-871E-A47A0C05DECC"), Kommunenr = 751, Navn = "Aarhus" };

      var oprindelsesSted = new Oprindelsessted() { Adresse = "Bautavej 1", PostDistrikt = "Aarhus V", Postnummer = 8210 };
      oprindelsesSted.OprindelsesstedKlassifikationType = new OprindelsesstedKlassifikationType() { Id = new Guid("BBB062F1-641A-45AD-BEDD-B0C126FCDFE6") };

      opretModel.Anmeldelse = new Anmeldelse()
      {
        Anmelder = anmelder,
        Betaler = betaler,
        Jord = jord,
        Kommune = kommune,
        Oprindelsessted = oprindelsesSted,
      };

      opretModel.ForureningOpslagKraeverAnmeldelse = true;
      opretModel.IndeholderAffald = false;
      opretModel.Jordhaandteringsplan = false;
      opretModel.MapStedLx = "571113";
      opretModel.MapStedLy = "6225161";
      opretModel.MapStedUx = "571313";
      opretModel.MapStedUy = "6225361";
      opretModel.MatrikelWkt = "";
      opretModel.ModtagerAnlaegFiltreringAnvenderJf = false;
      opretModel.OprindelsesKommuneAnvenderFlytJord = true;
      opretModel.OprindelsesstedWkt = "POINT(571213 6225261)";
      opretModel.SelectedJordklassifikationType = new Guid("1426C54A-6C06-4970-AC52-327D013751C7"); //Kategori 1
      opretModel.ForureningOpslagJordklassifikationTypeId = new Guid("1426C54A-6C06-4970-AC52-327D013751C7");//Kategori 1
      
      //Modtageranlæaeg
      opretModel.SelectedModtagerAnlaegId = "B05F5607-6CF0-4D56-9671-0A78489C2E0B";//Reno Djurs I/S, Aktiv=1, Anvender JF=0

      //Transportør
      opretModel.SelectedTransportoerId = "ACF41B4B-63F4-4448-B4BB-139A17B851CB";

      //Betaler
      opretModel.SelectedBetalerIndex = 0; //Anmelder er betaler.

      
      // Act
      var view = controller.Opret(opretModel, command) as PartialViewResult;
      
      Assert.IsTrue(view==null); // hvis anmeldelsen indsendes, redirectes man til forsiden. Derfor er view null
      
    }
  }
}
