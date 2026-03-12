//using System;
//using Moq;
//using Ninject;
//using NUnit.Framework;
//using Niras.Jordflytning.Controllers;
//using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
//using Niras.Jordflytning.Core.Interfaces.Repository;
//using Niras.Jordflytning.Core.Models;
//using Niras.Jordflytning.Infrastructure.Common;
//using Niras.Jordflytning.Infrastructure.DataAccess;
//using Niras.Jordflytning.UnitTests.TestsSetup.DependencyResolution;
//using Niras.Jordflytning.ViewModels;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Principal;
//using System.Web.Mvc;

//namespace Niras.Jordflytning.UnitTests.Controllers
//{
//	[TestFixture]
//	public class DefaultControllerTests
//	{
//		// Ninject kernel
//		private IKernel _ninjectKernel;

//		private Mock<IPrincipal> _user;
//		private Mock<IIdentity> _identity;
//		private Mock<ISecurityProvider> _securityProvider;

//		public DefaultControllerTests()
//		{
//			//// Init Ninject kernel
//			//_ninjectKernel = new StandardKernel
//			//		(
//			//			new ConfigModule(),
//			//			new RepositoryModule()
//			//		);

//			//_securityProvider = new Mock<ISecurityProvider>();
			
//			//_user = new Mock<IPrincipal>();
//			//_identity = new Mock<IIdentity>();
//			//_user.Setup(u => u.Identity).Returns(_identity.Object);

//			//_securityProvider.Setup(s => s.CurrentUser).Returns(_user.Object as IPrincipal);
//			//_ninjectKernel.Bind<ISecurityProvider>().ToConstant(_securityProvider.Object);
//		}

//		[SetUp]
//		public void Setup()
//		{

//		}

//		[TearDown]
//		public void TearDown()
//		{
			
//		}

//		/// <summary>
//		/// Tests that a user is redirected to start page if login succeeds.
//		/// </summary>
//		[Test]
//		public void ShouldPresentStartPageAfterSuccesfullLogin()
//		{
//			//_securityProvider.Setup(s => s.Login("username", "password",false)).Returns(true);
//			//var loginModel = new LoginModel() { UserName = "username", Password = "password" };
//			//var defaultController = new DefaultController(_securityProvider.Object, null); // KFK: Ved ikke om denne null værdi er i orden.
//			//var loginResult = defaultController.FrontPage(loginModel) as RedirectToRouteResult;
//			//Assert.IsNotNull(loginResult, "User was not redirected after login");
//			//Object action, controller;
//			//loginResult.RouteValues.TryGetValue("action", out action);
//			//loginResult.RouteValues.TryGetValue("controller", out controller);
//			//Assert.AreEqual("Startside", action.ToString(), "User was not redirected to correct page");
//			//Assert.AreEqual("Default", controller.ToString(), "User was not redirected to correct page and controller");
//		}

//		/// <summary>
//		/// Tests that a user is presented to login page again if login fails.
//		/// </summary>
//		[Test]
//		public void ShouldPresentLoginPageAfterFailedLogin()
//		{
//			//_securityProvider.Setup(s => s.Login("username", "password", false)).Returns(false); // make sure login with these credentials fail
//			//var loginModel = new LoginModel() { UserName = "username", Password = "password" };
//			//var defaultController = new DefaultController(_securityProvider.Object, null); // KFK: Ved ikke om denne null værdi er i orden.
//			//var loginResult = defaultController.FrontPage(loginModel);// as RedirectToRouteResult;
//			//Assert.IsInstanceOfType(typeof(ViewResult), loginResult, "User was not presented to login page");
//		}
//	}
//}
