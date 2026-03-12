using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
	public class KonfigRepository : GenericRepository<Konfig>, IKonfigRepository
	{
    public KonfigRepository(IConfigService configService): base(configService)
		{

		}

	}
}
