using Moq;
using Ninject;
using Ninject.Modules;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Niras.Jordflytning.Infrastructure.Common;
using Niras.Jordflytning.Infrastructure.DataAccess;

namespace Niras.Jordflytning.UnitTests.TestsSetup.DependencyResolution
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
      Bind<IRepository<Lastbil>>().To<GenericRepository<Lastbil>>().WithConstructorArgument("dataContext", configService.DataContext);
      Bind<IModtagerAnlaegRepository>().To<ModtagerAnlaegRepository>().WithConstructorArgument("configService", configService);
      //Bind<IAnmeldelserRepository>().To<AnmeldelserRepository>().WithConstructorArgument("configService", configService);
      Bind<IStatusBetalerRepository>().To<StatusBetalerRepository>().WithConstructorArgument("configService", configService);
      Bind<IStatusAnmeldelseRepository>().To<StatusAnmeldelseRepository>().WithConstructorArgument("configService", configService);
      Bind<IDokumentationRepository>().To<DokumentationRepository>().WithConstructorArgument("configService", configService);
      Bind<IKonfigRepository>().To<KonfigRepository>().WithConstructorArgument("configService", configService);
      Bind<IKommuneRepository>().To<KommuneRepository>().WithConstructorArgument("configService", configService);
      Bind<ILogRepository>().To<LogRepository>().WithConstructorArgument("configService", configService);
      Bind<IAdvisRepository>().To<AdvisRepository>().WithConstructorArgument("configService", configService);
      Bind<IBetalerRepository>().To<BetalerRepository>().WithConstructorArgument("configService", configService);
      Bind<IPersonRepository>().To<PersonRepository>().WithConstructorArgument("configService", configService);
      Bind<IBrugereRepository>().To<BrugereRepository>().WithConstructorArgument("configService", configService);
      Bind<Iwebpages_RolesRepository>().To<webpages_RolesRepository>().WithConstructorArgument("configService", configService);
      Bind<IPersonKommuneRepository>().To<PersonKommuneRepository>().WithConstructorArgument("configService", configService);
      Bind<IPersonJordmodtagerRepository>().To<PersonJordmodtagerRepository>().WithConstructorArgument("configService", configService);
      Bind<IJordmodtagerRepository>().To<JordmodtagerRepository>().WithConstructorArgument("configService", configService);
      Bind<ISagsbehandlerRepository>().To<SagsbehandlerRepository>().WithConstructorArgument("configService", configService);
      Bind<IBemyndigedeAnmeldereRepository>().To<BemyndigedeAnmeldereRepository>().WithConstructorArgument("configService", configService);
      Bind<IPdfServiceRepository>().To<PdfServiceRepository>();
      Bind<IAlarmRepository>().To<AlarmRepository>().WithConstructorArgument("configService", configService);
      Bind<IGraensevaerdierRepository>().To<GraensevaerdierRepository>().WithConstructorArgument("configService", configService);
      Bind<IVognlaesRepository>().To<VognlaesRepository>().WithConstructorArgument("configService", configService);
      Bind<ITransportoerRepository>().To<TransportoerRepository>().WithConstructorArgument("configService", configService);
      Bind<IFirmaoplysningerRepository>().To<FirmaoplysningerRepository>().WithConstructorArgument("configService", configService);
      Bind<IInteresantRepository>().To<InteresantRepository>().WithConstructorArgument("configService", configService);
      Bind<IBeskedRepository>().To<BeskedRepository>().WithConstructorArgument("configService", configService); 
      
      

      //Kodelister
      Bind<IDokumentationTypeRepository>().To<DokumentationTypeRepository>().WithConstructorArgument("configService", configService);
			Bind<IAndenOprindJordTypeRepository>().To<AndenOprindJordTypeRepository>().WithConstructorArgument("configService", configService);
			Bind<IOprindelsesstedKlassifikationTypeRepository>().To<OprindelsesstedKlassifikationTypeRepository>().WithConstructorArgument("configService", configService);
			Bind<IAffaldTypeRepository>().To<AffaldTypeRepository>().WithConstructorArgument("configService", configService);
			Bind<IJordKlassifikationTypeRepository>().To<JordKlassifikationTypeRepository>().WithConstructorArgument("configService", configService);
			Bind<IJordflytningTypeRepository>().To<JordflytningTypeRepository>().WithConstructorArgument("configService", configService);
			Bind<IStatusAnmeldelseTypeRepository>().To<StatusAnmeldelseTypeRepository>().WithConstructorArgument("configService", configService);
			Bind<IRepository<MiljoeklasseType>>().To<GenericRepository<MiljoeklasseType>>().WithConstructorArgument("dataContext", configService.DataContext);
      Bind<IForureningskomponentRepository>().To<ForureningskomponentRepository>().WithConstructorArgument("configService", configService);
      Bind<ILandsdelTypeRepository>().To<LandsdelTypeRepository>().WithConstructorArgument("configService", configService);
      


      Bind<IEnhedTypeRepository>().To<EnhedTypeRepository>().WithConstructorArgument("configService", configService);
      Bind<IJordanlaegTypeRepository>().To<JordanlaegTypeRepository>().WithConstructorArgument("configService", configService);
      Bind<IKommuneJordklassifikationRepository>().To<KommuneJordklassifikationRepository>().WithConstructorArgument("configService", configService);
      Bind<IAdvisTypeRepository>().To<AdvisTypeRepository>().WithConstructorArgument("configService", configService);
      Bind<IStatusStikproeveTypeRepository>().To<StatusStikproeveTypeRepository>().WithConstructorArgument("configService", configService);
      Bind<IKommunikationRepository>().To<KommunikationRepository>().WithConstructorArgument("configService", configService);
      


      Bind<IAnmeldelserRepository>().To<AnmeldelserRepository>().WithConstructorArgument("configService", configService);
      
      Bind<IUnitOfWork>().To<UnitOfWork>();
      
      

    }
  }
}
