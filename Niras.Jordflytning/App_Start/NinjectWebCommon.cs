using Niras.Jordflytning.Core.BusinessLogic.Interfaces;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Common;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.Models;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Niras.Jordflytning.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(Niras.Jordflytning.App_Start.NinjectWebCommon), "Stop")]

namespace Niras.Jordflytning.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Core.BusinessLogic;
    using Infrastructure.Common;
    using Infrastructure.DependencyResolution;

    using Ninject;
    using Ninject.Modules;
    using Ninject.Web.Common;
    using System.Web.Mvc;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {


            // Add data and infrastructure modules
            var modules = new List<INinjectModule>
                {
                    new ConfigModule(),
                    new RepositoryModule() 
                };

            kernel.Load(modules);

            // Get config service
            //var configService = kernel.Get<IConfigService>();

            // Bind local services
            kernel.Bind<IPersonBusiness>().To<PersonBusiness>();
            kernel.Bind<IModtagerAnlaegBusiness>().To<ModtagerAnlaegBusiness>();
            kernel.Bind<ITransportoerBusiness>().To<TransportoerBusiness>();
            kernel.Bind<IKommuneBusiness>().To<KommuneBusiness>();
            kernel.Bind<IKodelisteBusiness>().To<KodelisteBusiness>();
            kernel.Bind<IAnmeldelserBusiness>().To<AnmeldelserBusiness>();
            kernel.Bind<IBrugereBusiness>().To<BrugereBusiness>();
            //kernel.Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument("configService", configService); ;
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            kernel.Bind<ISecurityProvider>().To<WebSecurityProvider>();
            kernel.Bind<IJordmodtagerBusiness>().To<JordmodtagerBusiness>();
            kernel.Bind<IForureningsOpslagBusiness>().To<ForureningsOpslagBusiness>();
            kernel.Bind<IDokumentationBusiness>().To<DokumentationBusiness>();
            kernel.Bind<IGenericBusiness<Lastbil>>().To<GenericBusiness<Lastbil>>();
            kernel.Bind<IStatusAnmeldelseBusiness>().To<StatusAnmeldelseBusiness>();
            kernel.Bind<IBetalerBusiness>().To<BetalerBusiness>();
            kernel.Bind<IMatrikelBusiness>().To<MatrikelBusiness>();
            kernel.Bind<IKonfigBusiness>().To<KonfigBusiness>();
            kernel.Bind<IAnmelderBusiness>().To<AnmelderBusiness>();
            kernel.Bind<IForureningskomponentBusiness>().To<ForureningskomponentBusiness>();
            kernel.Bind<IDokumenterBusiness>().To<DokumenterBusiness>();
            kernel.Bind<IStikproeveBusiness>().To<StikproeveBusiness>();
            kernel.Bind<IPlanlagteStikproeverBusiness>().To<PlanlagteStikproeveBusiness>();
            kernel.Bind<IStatusBetalerBusiness>().To<StatusBetalerBusiness>();
            kernel.Bind<IBogholderOpslagstavleBusiness>().To<BogholderOpslagstavleBusiness>();
            kernel.Bind<IAdviseringBusiness>().To<AdviseringBusiness>();
            kernel.Bind<IJordforureningsopslagBusiness>().To<JordforureningsopslagBusiness>();
            kernel.Bind<IBomSystemBusiness>().To<BomSystemBusiness>();
            kernel.Bind<IAnalyseDokumentBusiness>().To<AnalyseDokumentBusiness>();
            kernel.Bind<IBemyndigedeAnmelderBusiness>().To<BemyndigedeAnmelderBusiness>();

            kernel.Bind<ISoegBusiness>().To<SoegBusiness>();
            kernel.Bind<ISoegAdminBusiness>().To<SoegAdminBusiness>();
            kernel.Bind<ISoegBusinessBetaler>().To<SoegBusinessBetaler>();
            kernel.Bind<ISoegBusinessOpslagsTavle>().To<SoegBusinessOpslagsTavle>();
            kernel.Bind<ISoegBusinessStikproeve>().To<SoegBusinessStikproeve>();
            kernel.Bind<ISoegBusinessAccepterJord>().To<SoegBusinessAccepterJord>();
            kernel.Bind<IStatusStikproeveBusiness>().To<StatusStikproeveBusiness>();
            kernel.Bind<IPdfBusiness>().To<PdfBusiness>();
            kernel.Bind<IMailBusiness>().To<MailBusiness>();

            kernel.Bind<IKommunikationBusiness>().To<KommunikationBusiness>();
            kernel.Bind<IAlarmBusiness>().To<AlarmBusiness>();
            kernel.Bind<ILogBusiness>().To<LogBusiness>();
            kernel.Bind<IVognlaesBusiness>().To<VognlaesBusiness>();
            kernel.Bind<IGebyrTidsregistreringBusiness>().To<GebyrTidsregistreringBusiness>();
            kernel.Bind<IOpgavetypeBusiness>().To<OpgavetypeBusiness>();

            kernel.Bind<IDawaOpslagBusiness>().To<DawaOpslagBusiness>();
        }
    }
}
