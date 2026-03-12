using Ninject.Modules;
using Ninject.Web.Common;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Infrastructure.Services;

namespace Niras.Jordflytning.Infrastructure.DependencyResolution
{
	public class ConfigModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IConfigService>().To<ConfigService>().InRequestScope();
		}
	}
}
