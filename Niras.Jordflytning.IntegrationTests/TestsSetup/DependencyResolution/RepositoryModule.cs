using Moq;
using Ninject;
using Ninject.Modules;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using System.Collections.Generic;
using Niras.Jordflytning.Infrastructure.DataAccess;

namespace Niras.Jordflytning.IntegrationTests.TestsSetup.DependencyResolution
{

  public class RepositoryModule : NinjectModule
  {
    public override void Load()
    {
      // Init users
      var users = new List<BrugerProfil>
						{
							new BrugerProfil { BrugerId = 1, BrugerNavn = "bruger1@jordflytning.dk" },
							new BrugerProfil { BrugerId = 2, BrugerNavn = "bruger2@jordflytning.dk" },
							new BrugerProfil { BrugerId = 3, BrugerNavn = "bruger3@jordflytning.dk" },
						};

      // Set up mock user repository
      var mockUsersRep = new Mock<IBrugereRepository>();
      mockUsersRep.Setup(m => m.Read()).Returns(users);
      //mockUsersRep.Setup(m => m.Create("bruger1@jordflytning.dk", "12345")).Returns(users[0]);


      var configService = Kernel.Get<IConfigService>();

      //Bind repositories
      //Bind<IRepository<Lastbil>>().To<GenericRepository<Lastbil>>().WithConstructorArgument("dataContext", configService.DataContext);
			//Bind<IJordmodtagerRepository>().To<JordmodtagerRepository>().WithConstructorArgument("configService", configService);
      //Bind<IAnmeldelserRepository>().To<AnmeldelserRepository>().WithConstructorArgument("configService", configService);
      //Bind<IStatusAnmeldelseTypeRepository>().To<StatusAnmeldelseTypeRepository>().WithConstructorArgument("configService", configService);
			

      

    }
  }
}
