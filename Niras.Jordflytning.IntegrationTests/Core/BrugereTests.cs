using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using NUnit.Framework;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Infrastructure.Common;
using Niras.Jordflytning.Infrastructure.DependencyResolution;
using Niras.Jordflytning.IntegrationTests.TestsSetup;

namespace Niras.Jordflytning.IntegrationTests.Core
{
	[TestFixture]
	[InitializeSimpleMembership]
	public class BrugereTests
	{
		// Ninject kernel
		private readonly IKernel _ninjectKernel;
		private readonly ISecurityProvider _securityProvider;
		private readonly IBrugereBusiness _brugerBusiness;
	  private readonly IAdviseringBusiness _adviseringBusiness;

		private const String TestsUserName = "unittest@jordflytning.dk";
		private const String TestsPassword = "UT_01";
		private Guid _organisationsId = new Guid("f06cbca7-8154-4e44-bcea-0d8cdcab6bb3");

		public BrugereTests()
		{
			// Init Ninject kernel
			_ninjectKernel = new StandardKernel
					(
						new ConfigModule(),
						new RepositoryModule()
					);
      _ninjectKernel.Bind<IAdviseringBusiness>().To<AdviseringBusiness>();
			_ninjectKernel.Bind<ISecurityProvider>().To<WebSecurityProvider>();
			_ninjectKernel.Bind<IUnitOfWork>().To<UnitOfWork>();
			_ninjectKernel.Bind<IBrugereBusiness>().To<BrugereBusiness>();
      _ninjectKernel.Bind<IKodelisteBusiness>().To<KodelisteBusiness>();
      _ninjectKernel.Bind<IStatusBetalerBusiness>().To<StatusBetalerBusiness>();

      
      _securityProvider = _ninjectKernel.Get<ISecurityProvider>();

			_brugerBusiness = _ninjectKernel.Get<IBrugereBusiness>();

      
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
		public void ReadJormodtagereTest()
		{
			// Arrange
			var userId = new Guid("8721e8a4-1574-4c49-b0b8-60460d491264");

			// Act
			var persJorMdt = _brugerBusiness.GetUsersJordModtOrganisationer(userId);
			var persKomm = _brugerBusiness.GetUsersKommuneOrganisationer(userId);

			// Assert
			Assert.IsNotNull(persJorMdt);
			Assert.IsNotNull(persKomm);
		}		
		
		
		/// <summary>
		/// Tests that a user profile can be created in database.
		/// </summary>
		[Test]
		public void SendEmailTest()
		{
			// Arrange
			var userProfile = new BrugerProfil();
			userProfile.PasswordClearText = "1234";
			userProfile.BrugerNavn = "kve@niras.dk";

			const string activationToken = "ql4ED-5bHztg7ZowGxIx3g2";

			// Act

			_adviseringBusiness.SendAktivationEmail(userProfile, activationToken);

			// Assert
		}


		/// <summary>
		/// Tests that a user profile can be created in database.
		/// </summary>
		[Test]
		public void ShouldCreateUserProfile()
		{
			// Arrange
			String userName = DateTime.Now.Ticks + "@jordflytning.dk";

			// Act
			var created = _securityProvider.CreateUserAndAccount(userName, TestsPassword);

			// Assert
			Assert.IsNotNullOrEmpty(created, "User profile was not created");
			var userId = _securityProvider.GetUserId(userName);
			Assert.IsTrue(userId > 0, "User profile could not be retrieved after save.");
		}

		[Test]
		public void ShouldUpdateUserRoles()
		{
			// Arrange
			String userName = DateTime.Now.Ticks + "@jordflytning.dk";
			var userProfile = new BrugerProfil() { BrugerNavn = userName, PasswordClearText = TestsPassword };
			String[] roles = new String[] { "Prøvetager" };
			Person person = new Person();
			person.Email = userName;
			person.Navn = "TestFornavn";
			person.Efternavn = "TestEfternavn";
			person.Adresse = "TestAdresse";
			person.By = "TestBy";

			userProfile.Person = person;
			// Act
			var created = _brugerBusiness.Create(userProfile, roles, "2CDB9F35-5C1C-4206-BDF4-6B2C147A48FD");
			var user = _brugerBusiness.Read(userName);
			Assert.IsTrue(user.webpages_Roles.Count == 1);
			_brugerBusiness.AddUserToRole(userName,"KommuneAdmin");
//			_securityProvider.UpdateUserRoles(userName, new List<string>{"KommuneAdmin", "Prøvetager", "Sagsbehandler"});
//			_brugerBusiness.Reload(user);
			user = _brugerBusiness.Read(userName);
			Assert.IsTrue(user.webpages_Roles.Count == 2);
			_brugerBusiness.RemoveUserFromRole(userName, "Prøvetager");
			Assert.IsTrue(user.webpages_Roles.Count == 1);

		}

		[Test]
		public void ShouldAddNewUserToKommune()
		{
			// Arrange
			String userName = DateTime.Now.Ticks + "@jordflytning.dk";
			var userProfile = new BrugerProfil() { BrugerNavn = userName, PasswordClearText = TestsPassword };
			String[] roles = new String[] { "Prøvetager" };
			Person person = new Person();
			person.Email = userName;
			person.Navn = "TestFornavn";
			person.Efternavn = "TestEfternavn";
			person.Adresse = "TestAdresse";
			person.By = "TestBy";

			userProfile.Person = person;
			// Act

			var created = _brugerBusiness.Create(userProfile, roles, "2CDB9F35-5C1C-4206-BDF4-6B2C147A48FD");
			var user = _brugerBusiness.Read(userName);

			user = _brugerBusiness.Read(userName);

			Assert.IsTrue(user.Person.PersonKommune.Count == 1);
			Assert.IsTrue(user.Person.PersonKommune.First().KommuneId == new Guid("2CDB9F35-5C1C-4206-BDF4-6B2C147A48FD"));
		}

		[Test]
		public void ShouldAddUserToKommune()
		{
			// Arrange
			String userName = DateTime.Now.Ticks + "@jordflytning.dk";
			var userProfile = new BrugerProfil() { BrugerNavn = userName, PasswordClearText = TestsPassword };
			String[] roles = new String[] { "Prøvetager" };
			Person person = new Person();
			person.Email = userName;
			person.Navn = "TestFornavn";
			person.Efternavn = "TestEfternavn";
			person.Adresse = "TestAdresse";
			person.By = "TestBy";

			userProfile.Person = person;
			// Act

			var created = _brugerBusiness.Create(userProfile, roles);
			var user = _brugerBusiness.Read(userName);

			PersonKommune pk = new PersonKommune();
			pk.KommuneId = new Guid("2CDB9F35-5C1C-4206-BDF4-6B2C147A48FD");
			
			user.Person.PersonKommune.Add(pk);
			_brugerBusiness.SaveChanges();

			Assert.IsTrue(user.Person.PersonKommune.Count == 1);
			Assert.IsTrue(user.Person.PersonKommune.First().KommuneId == new Guid("2CDB9F35-5C1C-4206-BDF4-6B2C147A48FD"));
			Assert.IsTrue(user.Person.PersonKommune.First().Id != Guid.Empty);
		}

		/// <summary>
		/// Tests that a user can be created in database.
		/// </summary>
		[Test]
		public void ShouldCreateUser()
		{
			// Arrange
			String userName = DateTime.Now.Ticks + "@jordflytning.dk";
			var userProfile = new BrugerProfil() { BrugerNavn = userName, PasswordClearText = TestsPassword };
			String[] roles = new String[] { "Prøvetager" };
			Person person = new Person();
			person.Email = userName;
			person.Navn = "TestFornavn";
			person.Efternavn = "TestEfternavn";
			person.Adresse = "TestAdresse";

			//userProfile.Person.Add(person);
			// Act
			var created = _brugerBusiness.Create(userProfile, roles);

			// Assert
			Assert.IsTrue(created, "User was not created");
			var user = _brugerBusiness.Read(userName);
			Assert.IsNotNull(user, "User could not be retrieved after save.");
			Assert.AreEqual(userName, user.BrugerNavn, "User name was not saved");
			Assert.AreEqual(roles.Length, user.Roles.Count, "Incorrect number of roles");
			Assert.AreEqual(roles[0], user.Roles[0], "Incorrect role was assigned");
		}

		/// <summary>
		/// Tests that person data can be saved in database.
		/// </summary>
		[Test]
		public void ShouldSavePersonInformation()
		{
			var personBusiness = new GenericBusiness<Person>(_ninjectKernel.Get<IPersonRepository>(), _ninjectKernel.Get<IUnitOfWork>());

			String userName = DateTime.Now.Ticks + "@jordflytning.dk";
			Person person = new Person();
			person.Email = userName;
			person.Navn = "TestFornavn";
			person.Efternavn = "TestEfternavn";
			person.Adresse = "TestAdresse";

			personBusiness.Create(person);
			Assert.AreNotEqual(Guid.Empty, person.Id, "Id not generated");

		}

		/// <summary>
		/// Tests that a user can be created in database.
		/// </summary>
		[Test]
		public void ShouldCreateAndUpdateUser()
		{
			// Arrange
			String userName = DateTime.Now.Ticks + "@jordflytning.dk";
			var userProfile = new BrugerProfil() { BrugerNavn = userName, PasswordClearText = TestsPassword };
			String[] roles = new String[] { "Prøvetager" };
			Person person = new Person();
			person.Email = userName;
			person.Navn = "TestFornavn";
			person.Efternavn = "TestEfternavn";
			person.Adresse = "TestAdresse";

			userProfile.Person = person;
			// Act
			var created = _brugerBusiness.Create(userProfile, roles);

			var user = _brugerBusiness.Read(userName);

			//Person[] parray = new Person[10];
			//user.Person.CopyTo(parray,0);
			//Person p = parray[0];
			user.Person.Telefon = 12345678;
			user.Person.Mobiltelefon = 12345678;

			 _brugerBusiness.SaveChanges();

			var updatedUser = _brugerBusiness.Read(user.BrugerNavn);
			//parray = new Person[2];
			//updatedUser.Person.CopyTo(parray, 0);
			//var updatedP = parray[0];
			
			//p.Telefon = 12345678;
			//p.Mobiltelefon = 12345678;

			Assert.AreEqual(updatedUser.Person.Telefon, user.Person.Telefon, "Telephone number is incorrect");
			Assert.AreEqual(updatedUser.Person.Mobiltelefon, user.Person.Mobiltelefon, "Cellphone number is not correct");
		}

		/// <summary>
		/// Tests that a user can be created in database.
		/// </summary>
		[Test]
		public void ShouldCreateAndUpdateUserWithTransportoer()
		{
			// Arrange
			String userName = DateTime.Now.Ticks + "@jordflytning.dk";
			var userProfile = new BrugerProfil() { BrugerNavn = userName, PasswordClearText = TestsPassword };
			String[] roles = new String[] { "Prøvetager" };
			Person person = new Person();
			person.Email = userName;
			person.Navn = "TestFornavn";
			person.Efternavn = "TestEfternavn";

			person.Adresse = "TestAdresse";
			person.Transportoer = new Transportoer();
			

			userProfile.Person = person;
			// Act
			var created = _brugerBusiness.Create(userProfile, roles);

			var user = _brugerBusiness.Read(userName);	

			person.Telefon = 12345678;
			person.Mobiltelefon = 12345678;
			_brugerBusiness.SaveChanges();

			var updatedUser = _brugerBusiness.Read(user.BrugerNavn);

			Assert.AreEqual(updatedUser.Person.Telefon, person.Telefon, "Telephone number is incorrect");
			Assert.AreEqual(updatedUser.Person.Mobiltelefon, person.Mobiltelefon, "Cellphone number is not correct");

		}


		///// <summary>
		///// Tests that a user profile can be retrieved from database by the user name.
		///// </summary>
		//[Test]
		//public void ShouldReadUserProfileFromUserName()
		//{
		//	// Arrange
			
		//	// Act
		//	var userProfile = _brugerBusiness.Read(testsUserName);
		//	// Assert
		//	Assert.IsNotNull(userProfile, "User profile could not be retrieved by user name");
		//	Assert.AreEqual(testsUserName, userProfile.BrugerNavn, "Incorrect user was retrieved by user name");
		//	Assert.IsNullOrEmpty(userProfile.PasswordClearText, "Password in clear text should not be able to be retrieved from database.");
		//}

		//[Test]
		//public void Test()
		//{
		//	var business = new GenericBusiness<Person>(_ninjectKernel.Get<IPersonRepository>(), _ninjectKernel.Get<IUnitOfWork>());
		//	var p = new Person();
		//	p.Email = "unittest@niras.dk";
		//	p.Navn = "TestFornavn";
		//	p.Efternavn = "TestEfternavn";
		//	p.Adresse = "TestAdresse";
	

		//	business.Create(p);
		//}
	}
}
