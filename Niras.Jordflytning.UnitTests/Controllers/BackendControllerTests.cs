using System;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Niras.Jordflytning.Areas.Backend.Controllers;
using Niras.Jordflytning.Core;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using Niras.Jordflytning.Entities;
using Niras.Jordflytning.ViewModels;

namespace Niras.Jordflytning.UnitTests.Controllers
{
	/// <summary>
	/// Tester venstremenuen i backend (DefaultBackendController.Index) for hver rolle.
	/// </summary>
	[TestFixture]
	[Niras.Jordflytning.IntegrationTests.TestsSetup.RulTilbageEfterTest]
	public class BackendControllerTests
	{
		private const string MenuGruppe = "Menu";
		private const string AdministrationGruppe = "Administration";
		private const string SoegUrl = "/Backend/Search/AdminSearch?soegetype=0";

		private Mock<IPrincipal> _user;
		private Mock<ISecurityProvider> _securityProvider;

		[SetUp]
		public void Setup()
		{
			_user = new Mock<IPrincipal>();
			_user.Setup(u => u.Identity).Returns(new Mock<IIdentity>().Object);

			_securityProvider = new Mock<ISecurityProvider>();
			_securityProvider.Setup(s => s.CurrentUser).Returns(_user.Object);
		}

		#region MenuTesting

		[Test]
		public void IndexShouldContainCorrectMenuItemsForMiljoemedarbejder()
		{
			AssertForsideOgSoeg(ApplicationConstants.MiljoemedarbejderRolle, "/Backend/FrontPages/StartMiljoemedarbejder");
		}

		[Test]
		public void IndexShouldContainCorrectMenuItemsForSagsbehandler()
		{
			AssertForsideOgSoeg(ApplicationConstants.SagsbehandlerRolle, "/Backend/FrontPages/StartSagsbehandler");
		}

		[Test]
		public void IndexShouldContainCorrectMenuItemsForLaboratorie()
		{
			AssertForsideOgSoeg(ApplicationConstants.LaboratorieRolle, "/Backend/FrontPages/StartLab");
		}

		[Test]
		public void IndexShouldContainCorrectMenuItemsForPrøvetager()
		{
			AssertForsideOgSoeg(ApplicationConstants.ProevetagerRolle, "/Backend/FrontPages/StartProevetager");
		}

		[Test]
		public void IndexShouldContainCorrectMenuItemsForPladsmand()
		{
			AssertForsideOgSoeg(ApplicationConstants.PladsmandRolle, "/Backend/FrontPages/StartPladsmand");
		}

		[Test]
		public void IndexShouldContainCorrectMenuItemsForBogholder()
		{
			AssertForsideOgSoeg(ApplicationConstants.BogholderRolle, "/Backend/FrontPages/StartBogholder");
		}

		[Test]
		public void IndexShouldContainCorrectMenuItemsForJordmodtagerAdmin()
		{
			var menu = GenerateIndexViewModel(ApplicationConstants.JordmodtagerAdminRolle).Menu;

			AssertMenupunkt(menu, AdministrationGruppe, "Brugere", "/Backend/Administration/AdminBrugere");
			AssertMenupunkt(menu, AdministrationGruppe, "Jordmodtageranlæg", "/Backend/Administration/AdminModtageAnlaeg");
			Assert.IsNull(Gruppe(menu, MenuGruppe), "JordmodtagerAdmin skal ikke have forside/søg");
		}

		[Test]
		public void IndexShouldContainCorrectMenuItemsForKommuneAdmin()
		{
			var menu = GenerateIndexViewModel(ApplicationConstants.KommuneAdminRolle).Menu;

			AssertMenupunkt(menu, AdministrationGruppe, "Brugere", "/Backend/Administration/AdminBrugere");
			AssertMenupunkt(menu, AdministrationGruppe, "Jordmodtagerfirmaer", "/Backend/Administration/AdminJordmodtager");
			AssertMenupunkt(menu, AdministrationGruppe, "Modtageanlæg", "/Backend/Administration/AdminModtagerAnlaeg");
			AssertMenupunkt(menu, AdministrationGruppe, "Fakturering", "/Backend/Administration/AdminFakturering");
			Assert.IsNull(Gruppe(menu, MenuGruppe), "KommuneAdmin skal ikke have forside/søg");
		}

		[Test]
		public void IndexShouldContainCorrectMenuItemsForMasterAdmin()
		{
			var menu = GenerateIndexViewModel(ApplicationConstants.MasterAdminRolle).Menu;

			AssertMenupunkt(menu, "Support", "Brugersupport", "/Backend/Support/SkiftEmail");
			AssertMenupunkt(menu, "Support", "Ændringslog", "/Backend/Support/Aendringslog");
		}

		[Test]
		public void IndexWithSeveralRolesShouldContainSoegOnceAndForsideForEachRole()
		{
			var menu = GenerateIndexViewModel(ApplicationConstants.SagsbehandlerRolle, ApplicationConstants.MiljoemedarbejderRolle).Menu;

			var gruppe = Gruppe(menu, MenuGruppe);
			Assert.IsNotNull(gruppe, "Menu-gruppen mangler");
			Assert.AreEqual(1, gruppe.SubItems.Count(m => m.Text == "Søg"), "Søg skal kun være i menuen én gang");
			AssertMenupunkt(menu, MenuGruppe, "Forside (" + ApplicationConstants.MiljoemedarbejderRolle + ")", "/Backend/FrontPages/StartMiljoemedarbejder");
			AssertMenupunkt(menu, MenuGruppe, "Forside (" + ApplicationConstants.SagsbehandlerRolle + ")", "/Backend/FrontPages/StartSagsbehandler");
		}

		#endregion

		#region helpers

		/// <summary>Roller med forside har gruppen "Menu" med "Søg" og "Forside (rolle)".</summary>
		private void AssertForsideOgSoeg(string rolle, string forsideUrl)
		{
			var menu = GenerateIndexViewModel(rolle).Menu;

			AssertMenupunkt(menu, MenuGruppe, "Søg", SoegUrl);
			AssertMenupunkt(menu, MenuGruppe, "Forside (" + rolle + ")", forsideUrl);
			Assert.IsNull(Gruppe(menu, AdministrationGruppe), rolle + " skal ikke have administrationsmenu");
		}

		private static MenuItem Gruppe(Menu menu, string gruppe)
		{
			return menu.MenuItems.FirstOrDefault(m => m.Text == gruppe);
		}

		private static void AssertMenupunkt(Menu menu, string gruppe, string tekst, string url)
		{
			var g = Gruppe(menu, gruppe);
			Assert.IsNotNull(g, "Menuen mangler gruppen '" + gruppe + "'");

			var punkt = g.SubItems.FirstOrDefault(m => m.Text == tekst);
			Assert.IsNotNull(punkt, "Gruppen '" + gruppe + "' mangler punktet '" + tekst + "'");
			Assert.AreEqual(url, punkt.Url, "Forkert URL for '" + tekst + "'");
		}

		private IndexModel GenerateIndexViewModel(params string[] roller)
		{
			foreach (var rolle in roller)
			{
				var r = rolle;
				_user.Setup(u => u.IsInRole(r)).Returns(true);
			}

			var backendController = new DefaultBackendController(_securityProvider.Object, null);
			var viewResult = backendController.Index("") as ViewResult;
			Assert.IsNotNull(viewResult, "Backend index could not be rendered for " + string.Join(", ", roller));
			Assert.IsInstanceOf(typeof(IndexModel), viewResult.Model, "Wrong viewmodel for " + string.Join(", ", roller) + " backend index");

			return (IndexModel)viewResult.Model;
		}

		#endregion
	}
}
