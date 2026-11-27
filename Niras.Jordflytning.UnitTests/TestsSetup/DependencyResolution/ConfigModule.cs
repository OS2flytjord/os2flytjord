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
			//Én DbContext pr. kernel - svarer til InRequestScope i webappen. Med flere contexts fejler EF med
			//"referenced by multiple instances of IEntityChangeTracker", og transaktionen pr. test kræver én forbindelse ad gangen.
			Bind<IConfigService>().To<ConfigService>().InSingletonScope();
		}
	}
}
