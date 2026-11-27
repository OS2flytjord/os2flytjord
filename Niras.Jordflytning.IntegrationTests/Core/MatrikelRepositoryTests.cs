using NUnit.Framework;
using Ninject;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Infrastructure.DataAccess;
using Niras.Jordflytning.IntegrationTests.TestsSetup;

namespace Niras.Jordflytning.IntegrationTests.Core
{
	[TestFixture]
	[Niras.Jordflytning.IntegrationTests.TestsSetup.RulTilbageEfterTest]
	public class MatrikelRepositoryTests
	{
		private readonly IMatrikelRepository _repo;
		private readonly IKernel _ninjectKernel;

		public MatrikelRepositoryTests()
		{
			// Init Ninject kernel
			_ninjectKernel = new StandardKernel();
			_ninjectKernel.Bind<IMatrikelRepository>().To<MatrikelRepository>();
			TestSetupUtil.InjectKodelists(_ninjectKernel); //Øvrige afhængigheder (IConfigService m.fl.)
      _repo = _ninjectKernel.Get<IMatrikelRepository>();
		}

		

	}
}
