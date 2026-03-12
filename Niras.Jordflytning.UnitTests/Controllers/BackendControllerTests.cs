using System;
using Moq;
using Ninject;
using NUnit.Framework;
using Niras.Jordflytning.Areas.Backend.Controllers;
using Niras.Jordflytning.Controllers;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Infrastructure.Common;
using Niras.Jordflytning.Infrastructure.DataAccess;
using Niras.Jordflytning.UnitTests.TestsSetup.DependencyResolution;
using Niras.Jordflytning.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using Niras.Jordflytning.Entities;


namespace Niras.Jordflytning.UnitTests.Controllers
{
	[TestFixture]
	public class 	BackendControllerTests

	{
		// Ninject kernel
		private IKernel _ninjectKernel;

		private Mock<IPrincipal> _user;
		private Mock<IIdentity> _identity;
		private Mock<ISecurityProvider> _securityProvider;

		public 	BackendControllerTests()
		{
			// Init Ninject kernel
			_ninjectKernel = new StandardKernel
					(
						new ConfigModule(),
						new RepositoryModule()
					);

			_securityProvider = new Mock<ISecurityProvider>();

		}

		[SetUp]
		public void Setup()
		{
			_user = new Mock<IPrincipal>();
			_identity = new Mock<IIdentity>();
			_user.Setup(u => u.Identity).Returns(_identity.Object);

			_securityProvider.Setup(s => s.CurrentUser).Returns(_user.Object as IPrincipal);
			_ninjectKernel.Bind<ISecurityProvider>().ToConstant(_securityProvider.Object);
		}

		[TearDown]
		public void TearDown()
		{

		}

		#region MenuTesting

		/// <summary>
		/// Tests that left menu contains correct items for Miljømedarbejder role.
		/// </summary>
		[Test]
		public void IndexShouldContainCorrectMenuItemsForMiljoemedarbejder() 
		{
			var menu = GenerateIndexViewModel("Miljømedarbejder", true).Menu;
			Assert.IsNotNull(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Start")), "Menu for Miljoemedarbejder does not contain Start item");
			Assert.AreEqual(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Start")).Url, "/Backend/Administration/StartMiljoemedarbejder", "URL for Sagsbehandler Start item is not correct");

			Assert.IsNotNull(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Søg")), "Menu for Miljoemedarbejder does not contain Søg item");
			Assert.AreEqual(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Søg")).Url, "/Backend/Administration/Search", "URL for Sagsbehandler Start item is not correct");
		}

		/// <summary>
		/// Tests that left menu contains correct items for Sagsbehandler role.
		/// </summary>
		[Test]
		public void IndexShouldContainCorrectMenuItemsForSagsbehandler()
		{
			var menu = GenerateIndexViewModel("Sagsbehandler", true).Menu;
			Assert.IsNotNull(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Start")), "Menu for Sagsbehandler does not contain Start item");
			Assert.AreEqual(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Start")).Url, "/Backend/Administration/StartSagsbehandler", "URL for Sagsbehandler Start item is not correct");

			Assert.IsNotNull(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Søg")), "Menu for Sagsbehandler does not contain Søg item");
			Assert.AreEqual(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Søg")).Url, "/Backend/Administration/Search", "URL for Sagsbehandler Search item is not correct");
			
		}

		/// <summary>
		/// Tests that left menu contains correct items for Laboratorie role.
		/// </summary>
		[Test]
		public void IndexShouldContainCorrectMenuItemsForLaboratorie()
		{
			var menu = GenerateIndexViewModel("Laboratorie", true).Menu;
			Assert.IsNotNull(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Start")), "Menu for Laboratorie does not contain Start item");
			Assert.AreEqual(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Start")).Url, "/Backend/Administration/StartLab", "URL for Laboratorie Start item is not correct");

			Assert.IsNotNull(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Søg")), "Menu for Laboratorie does not contain Søg item");
			Assert.AreEqual(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Søg")).Url, "/Backend/Administration/Search", "URL for Laboratorie Search item is not correct");
		}

		/// <summary>
		/// Tests that left menu contains correct items for Laboratorie role.
		/// </summary>
		[Test]
		public void IndexShouldContainCorrectMenuItemsForPrøvetager()
		{
			var menu = GenerateIndexViewModel("Prøvetager", true).Menu;
			Assert.IsNotNull(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Start")), "Menu for Prøvetager does not contain Start item");
			Assert.AreEqual(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Start")).Url, "/Backend/Administration/StartProevetager", "URL for Prøvetager Start item is not correct");

			Assert.IsNotNull(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Søg")), "Menu for Prøvetager does not contain Søg item");
			Assert.AreEqual(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Søg")).Url, "/Backend/Administration/Search", "URL for Prøvetager Search item is not correct");
		}
	
		[Test]
		public void IndexShouldContainCorrectMenuItemsForPladsmand()
		{
			var menu = GenerateIndexViewModel("Pladsmand", true).Menu;
			Assert.IsNotNull(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Start")), "Menu for Pladsmand does not contain Start item");
			Assert.AreEqual(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Start")).Url, "/Backend/Administration/StartLab", "URL for Pladsmand Start item is not correct");

			Assert.IsNotNull(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Søg")), "Menu for Pladsmand does not contain Søg item");
			Assert.AreEqual(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Søg")).Url, "/Backend/Administration/Search", "URL for Pladsmand Search item is not correct");
		}
		/// <summary>
		/// Tests that left menu contains correct items for Laboratorie role.
		/// </summary>
		[Test]
		public void IndexShouldContainCorrectMenuItemsForBogholder()
		{
			var menu = GenerateIndexViewModel("Bogholder", true).Menu;
			Assert.IsNotNull(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Start")), "Menu for Bogholder does not contain Start item");
			Assert.AreEqual(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Start")).Url, "/Backend/Administration/StartBogholder", "URL for Bogholder Start item is not correct");

			Assert.IsNotNull(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Søg")), "Menu for Bogholder does not contain Søg item");
			Assert.AreEqual(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Søg")).Url, "/Backend/Administration/Search", "URL for Bogholder Search item is not correct");
		}
				/// <summary>
		/// Tests that left menu contains correct items for Laboratorie role.
		/// </summary>
		[Test]
		public void IndexShouldContainCorrectMenuItemsForJordmodtagerAdmin()
		{
			var menu = GenerateIndexViewModel("JordmodtagerAdmin", true).Menu;
			Assert.IsNotNull(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Start")), "Menu for JordmodtagerAdmin does not contain Start item");
			Assert.AreEqual(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Start")).Url, "/Backend/Administration/StartMiljoemedarbejder", "URL for JordmodtagerAdmin Start item is not correct");
			Assert.IsNotNull(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Søg")), "Menu for JordmodtagerAdmin does not contain Søg item");
			Assert.AreEqual(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Søg")).Url, "/Backend/Administration/Search", "URL for JordmodtagerAdmin Search item is not correct");

			Assert.IsNotNull(menu.MenuItems[1].SubItems.FirstOrDefault(m => m.Text.Equals("Brugere")), "Menu for JordmodtagerAdmin does not contain Start item");
			Assert.AreEqual(menu.MenuItems[1].SubItems.FirstOrDefault(m => m.Text.Equals("Brugere")).Url, "/Backend/Administration/AdminBrugere", "URL for JordmodtagerAdmin Brugere item is not correct");
			Assert.IsNotNull(menu.MenuItems[1].SubItems.FirstOrDefault(m => m.Text.Equals("Modtageranlæg")), "Menu for JordmodtagerAdmin does not contain Jordmodtageranlæg item");
			Assert.AreEqual(menu.MenuItems[1].SubItems.FirstOrDefault(m => m.Text.Equals("Modtageranlæg")).Url, "/Backend/Administration/AdminModtageAnlaeg", "URL for JordmodtagerAdmin Jordmodtageranlæg item is not correct");
		}

		/// <summary>
		/// Tests that left menu contains correct items for Laboratorie role.
		/// </summary>
		[Test]
		public void IndexShouldContainCorrectMenuItemsForKommuneAdmin()
		{
			var menu = GenerateIndexViewModel("KommuneAdmin", true).Menu;
			Assert.IsNotNull(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Start")), "Menu for KommuneAdmin does not contain Start item");
			Assert.AreEqual(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Start")).Url, "/Backend/Administration/StartSagsbehandler", "URL for KommuneAdmin Start item is not correct");
			Assert.IsNotNull(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Søg")), "Menu for KommuneAdmin does not contain Søg item");
			Assert.AreEqual(menu.MenuItems[0].SubItems.FirstOrDefault(m => m.Text.Equals("Søg")).Url, "/Backend/Administration/Search", "URL for KommuneAdmin Search item is not correct");

			Assert.IsNotNull(menu.MenuItems[1].SubItems.FirstOrDefault(m => m.Text.Equals("Brugere")), "Menu for KommuneAdmin does not contain Brugere item");
			Assert.AreEqual(menu.MenuItems[1].SubItems.FirstOrDefault(m => m.Text.Equals("Brugere")).Url, "/Backend/Administration/AdminKommuneUsers", "URL for KommuneAdmin Brugere item is not correct");
			Assert.IsNotNull(menu.MenuItems[1].SubItems.FirstOrDefault(m => m.Text.Equals("Jordmodtagere")), "Menu for KommuneAdmin does not contain Jordmodtagere item");
			Assert.AreEqual(menu.MenuItems[1].SubItems.FirstOrDefault(m => m.Text.Equals("Jordmodtagere")).Url, "/Backend/Administration/AdminJordmodtager", "URL for JordmodtagerAdmin Jordmodtagere item is not correct");
		}

		#endregion

		#region helpers

		private IndexModel GenerateIndexViewModel(String role = null, Boolean isInRole = true)
		{
			if (!String.IsNullOrEmpty(role))
			{
				_user.Setup(u => u.IsInRole(role)).Returns(isInRole);
				_user.Setup(u => u.IsInRole("KommuneAdmin")).Returns(true);
			}
			var backendController = new Niras.Jordflytning.Areas.Backend.Controllers.DefaultBackendController(_securityProvider.Object,null);
			var viewResult = backendController.Index("") as ViewResult;
			Assert.IsNotNull(viewResult, "Backend index could not be rendered for " + role);
			Assert.IsInstanceOf(typeof(IndexModel), viewResult.Model, "Wrong viewmodel for " + role + " backend index");

			return viewResult.Model as IndexModel;
		}

		#endregion
	}
}
