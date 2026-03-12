using Ninject.Modules;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Infrastructure.Services;

namespace Niras.Jordflytning.IntegrationTests.TestsSetup.DependencyResolution
{
	public class ConfigModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IConfigService>().To<ConfigService>().InSingletonScope();
		}
	}
}
