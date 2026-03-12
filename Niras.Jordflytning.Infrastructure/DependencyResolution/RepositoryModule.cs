using Ninject.Modules;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Infrastructure.DataAccess;

namespace Niras.Jordflytning.Infrastructure.DependencyResolution
{
    public class RepositoryModule : NinjectModule
    {
        public override void Load()
        {
            // Get config service
            //var configService = Kernel.Get<IConfigService>();
            // ;

            //Bind repositories
            Bind<IBrugereRepository>().To<BrugereRepository>();
            Bind<IPersonRepository>().To<PersonRepository>();
            Bind<ITransportoerRepository>().To<TransportoerRepository>();
            Bind<IModtagerAnlaegRepository>().To<ModtagerAnlaegRepository>();
            Bind<IAnmeldelserRepository>().To<AnmeldelserRepository>();
            Bind<IJordmodtagerRepository>().To<JordmodtagerRepository>();
            //Bind<IRepository<Lastbil>>().To<GenericRepository<Lastbil>>().WithConstructorArgument("dataContext", configService.DataContext);
            Bind<IRepository<Lastbil>>().To<GenericRepository<Lastbil>>();
            Bind<IStatusAnmeldelseRepository>().To<StatusAnmeldelseRepository>();
            Bind<IDokumentationRepository>().To<DokumentationRepository>();
            Bind<IBetalerRepository>().To<BetalerRepository>();
            Bind<IAnmelderRepository>().To<AnmelderRepository>();
            Bind<IStatusBetalerRepository>().To<StatusBetalerRepository>();
            Bind<Iwebpages_RolesRepository>().To<webpages_RolesRepository>();
            Bind<IPersonKommuneRepository>().To<PersonKommuneRepository>();
            Bind<IPersonJordmodtagerRepository>().To<PersonJordmodtagerRepository>();
            Bind<IDokumenterRepository>().To<DokumenterRepository>();
            Bind<ISagsbehandlerRepository>().To<SagsbehandlerRepository>();
            Bind<IStikproeveRepository>().To<StikproeveRepository>();
            Bind<IGraensevaerdierRepository>().To<GraensevaerdierRepository>();
            Bind<IJordanlaegTypeRepository>().To<JordanlaegTypeRepository>();
            Bind<IPlanlagteStikproeverRepository>().To<PlanlagteStikproeverRepository>();
            Bind<IEnhedTypeRepository>().To<EnhedTypeRepository>();
            Bind<IBogholderOpslagstavleRepository>().To<BogholderOpslagstavleRepository>();
            Bind<IJordforureningsopslagRepository>().To<JordforureningsopslagRepository>();
            Bind<IStatusStikproeveTypeRepository>().To<StatusStikproeveTypeRepository>();
            Bind<IStatusStikproeveRepository>().To<StatusStikproeveRepository>();
            Bind<IKommuneJordklassifikationRepository>().To<KommuneJordklassifikationRepository>();
            Bind<IBemyndigedeAnmeldereRepository>().To<BemyndigedeAnmeldereRepository>();
            Bind<IAdvisTypeRepository>().To<AdvisTypeRepository>();
            Bind<IAdvisRepository>().To<AdvisRepository>();
            Bind<IVognlaesRepository>().To<VognlaesRepository>();
            Bind<ILastbilRepository>().To<LastbilRepository>();
            Bind<IAnalyseDokumentRepository>().To<AnalyseDokumentRepository>();
            Bind<IFirmaoplysningerRepository>().To<FirmaoplysningerRepository>();
            Bind<IFaktureringRepository>().To<FaktureringRepository>();

            Bind<IKommunikationRepository>().To<KommunikationRepository>();
            Bind<IAlarmRepository>().To<AlarmRepository>();
            Bind<IBeskedRepository>().To<BeskedRepository>();
            Bind<ILogRepository>().To<LogRepository>();
            Bind<IInteresantRepository>().To<InteresantRepository>();
            Bind<IGebyrTidsregistreringRepository>().To<GebyrTidsregistreringRepository>();
            Bind<IOpgavetypeRepository>().To<OpgavetypeRepository>();



            //Kodetabeller
            Bind<IDokumentationTypeRepository>().To<DokumentationTypeRepository>();
            Bind<IAndenOprindJordTypeRepository>().To<AndenOprindJordTypeRepository>();
            Bind<IOprindelsesstedKlassifikationTypeRepository>().To<OprindelsesstedKlassifikationTypeRepository>();
            Bind<IAffaldTypeRepository>().To<AffaldTypeRepository>();
            Bind<IJordKlassifikationTypeRepository>().To<JordKlassifikationTypeRepository>();
            Bind<IJordflytningTypeRepository>().To<JordflytningTypeRepository>();
            Bind<IStatusAnmeldelseTypeRepository>().To<StatusAnmeldelseTypeRepository>();
            //Bind<IRepository<MiljoeklasseType>>().To<GenericRepository<MiljoeklasseType>>().WithConstructorArgument("dataContext", configService.DataContext);
            Bind<IRepository<MiljoeklasseType>>().To<GenericRepository<MiljoeklasseType>>();
            Bind<IForureningskomponentRepository>().To<ForureningskomponentRepository>();
            Bind<ILandsdelTypeRepository>().To<LandsdelTypeRepository>();


            //Andre tabeller
            Bind<IKommuneRepository>().To<KommuneRepository>();

            Bind<IGeoEnvironRepository>().To<GeoEnvironRepository>();
            Bind<IMiljoePortalRepository>().To<MiljoePortalRepository>();
            Bind<IMatrikelRepository>().To<MatrikelRepository>();
            Bind<IMatrikelOpslagRepository>().To<MatrikelOpslagRepository>();
            Bind<IKonfigRepository>().To<KonfigRepository>();
            Bind<IPdfServiceRepository>().To<PdfServiceRepository>();
            Bind<IKulturarvRepository>().To<KulturarvRepository>();
            Bind<IPlansystemRepository>().To<PlansystemRepository>();

        }
    }
}
