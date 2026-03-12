using Ninject;
using NUnit.Framework;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Infrastructure.DependencyResolution;

namespace Niras.Jordflytning.IntegrationTests.Core
{
	[TestFixture]
	
	public class PersonTests
	{
		// Ninject kernel
		private readonly IKernel _ninjectKernel;
		private readonly IConfigService _configService;
		private readonly IPersonRepository _personRepo;
		//private IBrugereBusiness _brugerBusiness;

		private const int TestsUserId = 7;
		//private const String testsPassword = "UT_01";

		public PersonTests()
		{
			// Init Ninject kernel
			_ninjectKernel = new StandardKernel
					(
						new ConfigModule(),
						new RepositoryModule()
					);

			_configService = _ninjectKernel.Get<IConfigService>();
			_personRepo = _ninjectKernel.Get<IPersonRepository>();
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
		/// Tests that a user profile can be created in database.
		/// </summary>
		[Test]
		public void ShouldCreatePerson()
		{

			//GenericRepository<Person> _personRepo = new GenericRepository<Person>(_configService.DataContext);
			var p = new Person();
			p.Adresse = "tralala 123";
			p.Aktiv = true;
			p.BrugerId = TestsUserId;
			p.Navn = "UT Navn";
			p.Efternavn = "Efternavn";
			p.Email = "test@test.dk";

			//Transportoer transportoer = new Transportoer();
			//transportoer.Person = p;
			//p.Transportoer.Add(transportoer);

			_personRepo.Add(p);
			_configService.DataContext.SaveChanges();
			// Act
			//var created = repo.Read();

			// Assert
			//	Assert.IsTrue(created.id != Guid.Empty, "Person has empty guid");
			//var userId = _securityProvider.GetUserId(userName);
			//Assert.IsTrue(userId > 0, "User profile could not be retrieved after save.");
		}


	}
}
