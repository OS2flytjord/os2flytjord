using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
	public class LogRepository : GenericRepository<Log>, ILogRepository
	{
    public LogRepository(IConfigService configService)
      : base(configService)
		{

		}

	}
}
