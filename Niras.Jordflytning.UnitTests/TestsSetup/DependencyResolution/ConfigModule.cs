using Ninject.Modules;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Niras.Jordflytning.UnitTests.TestsSetup.DependencyResolution
{
	public class ConfigModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IConfigService>().To<ConfigService>();
		}
	}
}
