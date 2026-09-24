using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Net;
using System.Text;
using System.Xml.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
  public class MatrikelRepository : GenericRepository<Matrikel>, IMatrikelRepository
  {
    private static readonly ILogger logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Infrastructure.DataAccess.MatrikelRepository");
    private const int SpatialReference = 25832;

    public MatrikelRepository(IConfigService configService)
      : base(configService)
    {
    }    
  }
}
