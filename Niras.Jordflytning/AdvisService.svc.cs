using System;
using System.ServiceModel;
using Ninject;
using Niras.Jordflytning.App_Start;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning
{
  // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AdvisService" in code, svc and config file together.
  // NOTE: In order to launch WCF Test Client for testing this service, please select AdvisService.svc or AdvisService.svc.cs at the Solution Explorer and start debugging.

  [ServiceContract]
  public interface IAdvisService
  {
    
  }

  public class AdvisService : IAdvisService
  {
    private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.AdvisService");
    private readonly IKernel _ninjectKernel;

    public AdvisService()
    {
			_ninjectKernel = NinjectWebCommon.CreateKernel();      
    }
    
    
  }
}
