using Ninject;
using Niras.Jordflytning.Core.BusinessLogic;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Infrastructure.Common;
using Niras.Jordflytning.Infrastructure.DataAccess;
using Niras.Jordflytning.Infrastructure.Services;

namespace Niras.Jordflytning.IntegrationTests.TestsSetup
{
	public static class TestSetupUtil
	{

		/// <summary>
		/// Binding of the all missing bindings
		/// </summary>
		public static IKodelisteBusiness InjectKodelists(IKernel ninjectKernel)
		{
			ninjectKernel.Bind<IConfigService>().To<ConfigService>();
			var configService = ninjectKernel.Get<IConfigService>();

			
			ninjectKernel.Bind<IKodelisteBusiness>().To<KodelisteBusiness>();
			
			ninjectKernel.Bind<IDokumentationTypeRepository>().To<DokumentationTypeRepository>();		
			ninjectKernel.Bind<IKommuneRepository>().To<KommuneRepository>();
			ninjectKernel.Bind<IOprindelsesstedKlassifikationTypeRepository>().To<OprindelsesstedKlassifikationTypeRepository>();
			ninjectKernel.Bind<IAndenOprindJordTypeRepository>().To<AndenOprindJordTypeRepository>();
			ninjectKernel.Bind<IAffaldTypeRepository>().To<AffaldTypeRepository>();
			ninjectKernel.Bind<IJordKlassifikationTypeRepository>().To<JordKlassifikationTypeRepository>();
			ninjectKernel.Bind<IJordflytningTypeRepository>().To<JordflytningTypeRepository>();
			ninjectKernel.Bind<IGraensevaerdierRepository>().To<GraensevaerdierRepository>();
			ninjectKernel.Bind<IEnhedTypeRepository>().To<EnhedTypeRepository>();
			ninjectKernel.Bind<IStikproeveRepository>().To<StikproeveRepository>();
			ninjectKernel.Bind<IModtagerAnlaegRepository>().To<ModtagerAnlaegRepository>();
			ninjectKernel.Bind<IAnmeldelserRepository>().To<AnmeldelserRepository>();
			ninjectKernel.Bind<IKommuneJordklassifikationRepository>().To<KommuneJordklassifikationRepository>();
			ninjectKernel.Bind<IJordmodtagerRepository>().To<JordmodtagerRepository>();
			ninjectKernel.Bind<IStatusAnmeldelseTypeRepository>().To<StatusAnmeldelseTypeRepository>();
			ninjectKernel.Bind<IJordanlaegTypeRepository>().To<JordanlaegTypeRepository>();			
			ninjectKernel.Bind<IDokumentationRepository>().To<DokumentationRepository>();
			ninjectKernel.Bind<IStatusAnmeldelseRepository>().To<StatusAnmeldelseRepository>();
			ninjectKernel.Bind<IPlanlagteStikproeverRepository>().To<PlanlagteStikproeverRepository>();
			ninjectKernel.Bind<IKonfigRepository>().To<KonfigRepository>();
			ninjectKernel.Bind<IBogholderOpslagstavleRepository>().To<BogholderOpslagstavleRepository>();
			ninjectKernel.Bind<IAdvisTypeRepository>().To<AdvisTypeRepository>();
			ninjectKernel.Bind<IAdvisRepository>().To<AdvisRepository>();
			ninjectKernel.Bind<IStatusStikproeveTypeRepository>().To<StatusStikproeveTypeRepository>();		
			ninjectKernel.Bind<IStatusBetalerRepository>().To<StatusBetalerRepository>();
			ninjectKernel.Bind<IKommunikationRepository>().To<KommunikationRepository>();
			ninjectKernel.Bind<IBetalerRepository>().To<BetalerRepository>();
			ninjectKernel.Bind<IPersonRepository>().To<PersonRepository>();
			ninjectKernel.Bind<IBrugereRepository>().To<BrugereRepository>();
			ninjectKernel.Bind<Iwebpages_RolesRepository>().To<webpages_RolesRepository>();
			ninjectKernel.Bind<IPersonKommuneRepository>().To<PersonKommuneRepository>();
			ninjectKernel.Bind<IPersonJordmodtagerRepository>().To<PersonJordmodtagerRepository>();
			ninjectKernel.Bind<ISagsbehandlerRepository>().To<SagsbehandlerRepository>();
			ninjectKernel.Bind<IBemyndigedeAnmeldereRepository>().To<BemyndigedeAnmeldereRepository>();
			ninjectKernel.Bind<IPdfServiceRepository>().To<PdfServiceRepository>();
			ninjectKernel.Bind<IAlarmRepository>().To<AlarmRepository>();
			ninjectKernel.Bind<ILogRepository>().To<LogRepository>();
			ninjectKernel.Bind<IVognlaesRepository>().To<VognlaesRepository>();
			ninjectKernel.Bind<ITransportoerRepository>().To<TransportoerRepository>();
			ninjectKernel.Bind<ILastbilRepository>().To<LastbilRepository>();
			ninjectKernel.Bind<ILandsdelTypeRepository>().To<LandsdelTypeRepository>();
			ninjectKernel.Bind<IFirmaoplysningerRepository>().To<FirmaoplysningerRepository>();
			ninjectKernel.Bind<IMiljoePortalRepository>().To<MiljoePortalRepository>();
			ninjectKernel.Bind<IGeoEnvironRepository>().To<GeoEnvironRepository>();
			ninjectKernel.Bind<IMatrikelRepository>().To<MatrikelRepository>();
			ninjectKernel.Bind<IMatrikelOpslagRepository>().To<MatrikelOpslagRepository>();
			ninjectKernel.Bind<IAnmelderRepository>().To<AnmelderRepository>();
			ninjectKernel.Bind<IAnalyseDokumentRepository>().To<AnalyseDokumentRepository>();
			ninjectKernel.Bind<IInteresantRepository>().To<InteresantRepository>();
			ninjectKernel.Bind<IBeskedRepository>().To<BeskedRepository>();
			
			ninjectKernel.Bind<IRepository<MiljoeklasseType>>().To<GenericRepository<MiljoeklasseType>>().WithConstructorArgument("dataContext", configService.DataContext);
			ninjectKernel.Bind<IRepository<Lastbil>>().To<GenericRepository<Lastbil>>().WithConstructorArgument("dataContext", configService.DataContext);

			ninjectKernel.Bind<ITransportoerBusiness>().To<TransportoerBusiness>();
			ninjectKernel.Bind<ILogBusiness>().To<LogBusiness>();
			ninjectKernel.Bind<IPdfBusiness>().To<PdfBusiness>();
			ninjectKernel.Bind<IAlarmBusiness>().To<AlarmBusiness>();
			ninjectKernel.Bind<IKommunikationBusiness>().To<KommunikationBusiness>();
			ninjectKernel.Bind<IBrugereBusiness>().To<BrugereBusiness>();
			ninjectKernel.Bind<IStatusBetalerBusiness>().To<StatusBetalerBusiness>();
			ninjectKernel.Bind<IJordmodtagerBusiness>().To<JordmodtagerBusiness>();
			ninjectKernel.Bind<IDokumentationBusiness>().To<DokumentationBusiness>();
			ninjectKernel.Bind<IAnmeldelserBusiness>().To<AnmeldelserBusiness>();
			ninjectKernel.Bind<IStatusAnmeldelseBusiness>().To<StatusAnmeldelseBusiness>();
			ninjectKernel.Bind<IModtagerAnlaegBusiness>().To<ModtagerAnlaegBusiness>();
			ninjectKernel.Bind<IAdviseringBusiness>().To<AdviseringBusiness>();
			ninjectKernel.Bind<IBetalerBusiness>().To<BetalerBusiness>();
			ninjectKernel.Bind<IMailBusiness>().To<MailBusiness>();
			ninjectKernel.Bind<IKonfigBusiness>().To<KonfigBusiness>();
			ninjectKernel.Bind<IBomSystemBusiness>().To<BomSystemBusiness>();
			ninjectKernel.Bind<IPlanlagteStikproeverBusiness>().To<PlanlagteStikproeveBusiness>();
			ninjectKernel.Bind<IStikproeveBusiness>().To<StikproeveBusiness>();
			ninjectKernel.Bind<IVognlaesBusiness>().To<VognlaesBusiness>();
			ninjectKernel.Bind<IStatusStikproeveBusiness>().To<StatusStikproeveBusiness>();
			ninjectKernel.Bind<IPersonBusiness>().To<PersonBusiness>();
			ninjectKernel.Bind<IForureningsOpslagBusiness>().To<ForureningsOpslagBusiness>();
			ninjectKernel.Bind<IMatrikelBusiness>().To<MatrikelBusiness>();
			ninjectKernel.Bind<ISoegBusiness>().To<SoegBusiness>();
			ninjectKernel.Bind<IAnmelderBusiness>().To<AnmelderBusiness>();
			ninjectKernel.Bind<IAnalyseDokumentBusiness>().To<AnalyseDokumentBusiness>();
			ninjectKernel.Bind<IKommuneBusiness>().To<KommuneBusiness>();

			var securityProvider = ninjectKernel.TryGet<ISecurityProvider>();
			if (securityProvider == null)
			{
				ninjectKernel.Bind<ISecurityProvider>().To<WebSecurityProvider>();
			}

			ninjectKernel.Bind<IUnitOfWork>().To<UnitOfWork>();   

			var bussinesKodeList = ninjectKernel.Get<IKodelisteBusiness>();
			return bussinesKodeList;

		}


	}
}
