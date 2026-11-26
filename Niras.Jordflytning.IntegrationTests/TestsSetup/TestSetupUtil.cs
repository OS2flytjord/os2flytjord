using System;
using System.Linq;
using System.Reflection;
using Ninject;
using Ninject.Activation;
using Ninject.Activation.Providers;
using Ninject.Infrastructure;
using Ninject.Parameters;
using Ninject.Planning.Bindings;
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
		/// Tilføjer standard-bindinger for alle repositories, business-klasser og services.
		///
		/// Bindingerne er "implicitte" (lav prioritet) og tilføjes kun, hvis interfacet ikke allerede er bundet.
		/// Derfor kan en fixture frit binde sine egne implementeringer (fx mocks) - både før og efter dette kald,
		/// og med eller uden ConfigModule/RepositoryModule - uden "More than one matching bindings"-fejl.
		/// Interfaces uden eksplicit standard bindes til deres eneste implementering i Core/Infrastructure.
		/// </summary>
		public static IKodelisteBusiness InjectKodelists(IKernel ninjectKernel)
		{
			//Én DbContext for hele testen - svarer til InRequestScope i webappen.
			//Med en ny context pr. repository fejler EF med "referenced by multiple instances of IEntityChangeTracker".
			Standard(ninjectKernel, typeof(IConfigService), typeof(ConfigService), StandardScopeCallbacks.Singleton);
			var configService = ninjectKernel.Get<IConfigService>();

			
			Standard<IKodelisteBusiness, KodelisteBusiness>(ninjectKernel);
			
			Standard<IDokumentationTypeRepository, DokumentationTypeRepository>(ninjectKernel);		
			Standard<IKommuneRepository, KommuneRepository>(ninjectKernel);
			Standard<IOprindelsesstedKlassifikationTypeRepository, OprindelsesstedKlassifikationTypeRepository>(ninjectKernel);
			Standard<IAndenOprindJordTypeRepository, AndenOprindJordTypeRepository>(ninjectKernel);
			Standard<IAffaldTypeRepository, AffaldTypeRepository>(ninjectKernel);
			Standard<IJordKlassifikationTypeRepository, JordKlassifikationTypeRepository>(ninjectKernel);
			Standard<IJordflytningTypeRepository, JordflytningTypeRepository>(ninjectKernel);
			Standard<IGraensevaerdierRepository, GraensevaerdierRepository>(ninjectKernel);
			Standard<IEnhedTypeRepository, EnhedTypeRepository>(ninjectKernel);
			Standard<IStikproeveRepository, StikproeveRepository>(ninjectKernel);
			Standard<IModtagerAnlaegRepository, ModtagerAnlaegRepository>(ninjectKernel);
			Standard<IAnmeldelserRepository, AnmeldelserRepository>(ninjectKernel);
			Standard<IKommuneJordklassifikationRepository, KommuneJordklassifikationRepository>(ninjectKernel);
			Standard<IJordmodtagerRepository, JordmodtagerRepository>(ninjectKernel);
			Standard<IStatusAnmeldelseTypeRepository, StatusAnmeldelseTypeRepository>(ninjectKernel);
			Standard<IJordanlaegTypeRepository, JordanlaegTypeRepository>(ninjectKernel);			
			Standard<IDokumentationRepository, DokumentationRepository>(ninjectKernel);
			Standard<IStatusAnmeldelseRepository, StatusAnmeldelseRepository>(ninjectKernel);
			Standard<IPlanlagteStikproeverRepository, PlanlagteStikproeverRepository>(ninjectKernel);
			Standard<IKonfigRepository, KonfigRepository>(ninjectKernel);
			Standard<IBogholderOpslagstavleRepository, BogholderOpslagstavleRepository>(ninjectKernel);
			Standard<IAdvisTypeRepository, AdvisTypeRepository>(ninjectKernel);
			Standard<IAdvisRepository, AdvisRepository>(ninjectKernel);
			Standard<IStatusStikproeveTypeRepository, StatusStikproeveTypeRepository>(ninjectKernel);		
			Standard<IStatusBetalerRepository, StatusBetalerRepository>(ninjectKernel);
			Standard<IKommunikationRepository, KommunikationRepository>(ninjectKernel);
			Standard<IBetalerRepository, BetalerRepository>(ninjectKernel);
			Standard<IPersonRepository, PersonRepository>(ninjectKernel);
			Standard<IBrugereRepository, BrugereRepository>(ninjectKernel);
			Standard<Iwebpages_RolesRepository, webpages_RolesRepository>(ninjectKernel);
			Standard<IPersonKommuneRepository, PersonKommuneRepository>(ninjectKernel);
			Standard<IPersonJordmodtagerRepository, PersonJordmodtagerRepository>(ninjectKernel);
			Standard<ISagsbehandlerRepository, SagsbehandlerRepository>(ninjectKernel);
			Standard<IBemyndigedeAnmeldereRepository, BemyndigedeAnmeldereRepository>(ninjectKernel);
			Standard<IPdfServiceRepository, PdfServiceRepository>(ninjectKernel);
			Standard<IAlarmRepository, AlarmRepository>(ninjectKernel);
			Standard<ILogRepository, LogRepository>(ninjectKernel);
			Standard<IVognlaesRepository, VognlaesRepository>(ninjectKernel);
			Standard<ITransportoerRepository, TransportoerRepository>(ninjectKernel);
			Standard<ILastbilRepository, LastbilRepository>(ninjectKernel);
			Standard<ILandsdelTypeRepository, LandsdelTypeRepository>(ninjectKernel);
			Standard<IFirmaoplysningerRepository, FirmaoplysningerRepository>(ninjectKernel);
			Standard<IMiljoePortalRepository, MiljoePortalRepository>(ninjectKernel);
			Standard<IGeoEnvironRepository, GeoEnvironRepository>(ninjectKernel);
			Standard<IMatrikelRepository, MatrikelRepository>(ninjectKernel);
			Standard<IMatrikelOpslagRepository, MatrikelOpslagRepository>(ninjectKernel);
			Standard<IAnmelderRepository, AnmelderRepository>(ninjectKernel);
			Standard<IAnalyseDokumentRepository, AnalyseDokumentRepository>(ninjectKernel);
			Standard<IInteresantRepository, InteresantRepository>(ninjectKernel);
			Standard<IBeskedRepository, BeskedRepository>(ninjectKernel);
			
			Standard(ninjectKernel, typeof(IRepository<MiljoeklasseType>), typeof(GenericRepository<MiljoeklasseType>), null,
				new ConstructorArgument("dataContext", configService.DataContext));
			Standard(ninjectKernel, typeof(IRepository<Lastbil>), typeof(GenericRepository<Lastbil>), null,
				new ConstructorArgument("dataContext", configService.DataContext));

			Standard<ITransportoerBusiness, TransportoerBusiness>(ninjectKernel);
			Standard<ILogBusiness, LogBusiness>(ninjectKernel);
			Standard<IPdfBusiness, PdfBusiness>(ninjectKernel);
			Standard<IAlarmBusiness, AlarmBusiness>(ninjectKernel);
			Standard<IKommunikationBusiness, KommunikationBusiness>(ninjectKernel);
			Standard<IBrugereBusiness, BrugereBusiness>(ninjectKernel);
			Standard<IStatusBetalerBusiness, StatusBetalerBusiness>(ninjectKernel);
			Standard<IJordmodtagerBusiness, JordmodtagerBusiness>(ninjectKernel);
			Standard<IDokumentationBusiness, DokumentationBusiness>(ninjectKernel);
			Standard<IAnmeldelserBusiness, AnmeldelserBusiness>(ninjectKernel);
			Standard<IStatusAnmeldelseBusiness, StatusAnmeldelseBusiness>(ninjectKernel);
			Standard<IModtagerAnlaegBusiness, ModtagerAnlaegBusiness>(ninjectKernel);
			Standard<IAdviseringBusiness, AdviseringBusiness>(ninjectKernel);
			Standard<IBetalerBusiness, BetalerBusiness>(ninjectKernel);
			Standard<IMailBusiness, MailBusiness>(ninjectKernel);
			Standard<IKonfigBusiness, KonfigBusiness>(ninjectKernel);
			Standard<IBomSystemBusiness, BomSystemBusiness>(ninjectKernel);
			Standard<IPlanlagteStikproeverBusiness, PlanlagteStikproeveBusiness>(ninjectKernel);
			Standard<IStikproeveBusiness, StikproeveBusiness>(ninjectKernel);
			Standard<IVognlaesBusiness, VognlaesBusiness>(ninjectKernel);
			Standard<IStatusStikproeveBusiness, StatusStikproeveBusiness>(ninjectKernel);
			Standard<IPersonBusiness, PersonBusiness>(ninjectKernel);
			Standard<IForureningsOpslagBusiness, ForureningsOpslagBusiness>(ninjectKernel);
			Standard<IMatrikelBusiness, MatrikelBusiness>(ninjectKernel);
			Standard<ISoegBusiness, SoegBusiness>(ninjectKernel);
			Standard<IAnmelderBusiness, AnmelderBusiness>(ninjectKernel);
			Standard<IAnalyseDokumentBusiness, AnalyseDokumentBusiness>(ninjectKernel);
			Standard<IKommuneBusiness, KommuneBusiness>(ninjectKernel);

			Standard<ISecurityProvider, WebSecurityProvider>(ninjectKernel);
			Standard<IUnitOfWork, UnitOfWork>(ninjectKernel);

			//Alt hvad der ikke er nævnt ovenfor (nye repositories/business-klasser) bindes efter konvention.
			BindResterendeEfterKonvention(ninjectKernel);

			var bussinesKodeList = ninjectKernel.Get<IKodelisteBusiness>();
			return bussinesKodeList;

		}

		/// <summary>
		/// Tilføjer en standard-binding, hvis <typeparamref name="TService"/> ikke allerede er bundet.
		/// </summary>
		public static void Standard<TService, TImplementation>(IKernel kernel) where TImplementation : TService
		{
			Standard(kernel, typeof(TService), typeof(TImplementation), null);
		}

		/// <summary>
		/// Tilføjer en implicit (lav prioritet) binding, hvis servicen ikke allerede er bundet.
		/// En eksplicit binding, som en fixture laver bagefter, vinder over den implicitte.
		/// </summary>
		public static void Standard(IKernel kernel, Type service, Type implementation, Func<IContext, object> scope,
			params IParameter[] parameters)
		{
			if (kernel.GetBindings(service).Any())
				return;

			var binding = new Binding(service)
			{
				ProviderCallback = StandardProvider.GetCreationCallback(implementation),
				IsImplicit = true
			};
			if (scope != null)
				binding.ScopeCallback = scope;
			foreach (var p in parameters)
				binding.Parameters.Add(p);

			kernel.AddBinding(binding);
		}

		/// <summary>
		/// Binder alle Niras-interfaces fra Core/Infrastructure, som endnu ikke er bundet,
		/// til deres eneste konkrete implementering. Interfaces med flere eller ingen implementeringer springes over.
		/// </summary>
		private static void BindResterendeEfterKonvention(IKernel kernel)
		{
			var typer = new[] { typeof(AnmeldelserBusiness).Assembly, typeof(ConfigService).Assembly }
				.SelectMany(a =>
				{
					try { return a.GetTypes(); }
					catch (ReflectionTypeLoadException ex) { return ex.Types.Where(t => t != null).ToArray(); }
				})
				.ToList();

			var implementeringer = typer.Where(t => t.IsClass && !t.IsAbstract && !t.ContainsGenericParameters).ToList();
			var interfaces = typer.Where(t => t.IsInterface && !t.ContainsGenericParameters
			                                  && t.Namespace != null && t.Namespace.StartsWith("Niras.Jordflytning"));

			foreach (var service in interfaces)
			{
				var impl = implementeringer.Where(service.IsAssignableFrom).ToList();
				if (impl.Count == 1)
					Standard(kernel, service, impl[0], null);
			}
		}
	}
}
