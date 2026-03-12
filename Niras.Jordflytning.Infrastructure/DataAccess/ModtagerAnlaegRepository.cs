using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using System;
using System.Linq;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
	public class ModtagerAnlaegRepository : GenericRepository<ModtagerAnlaeg>, IModtagerAnlaegRepository
	{
    public ModtagerAnlaegRepository(IConfigService configService): base(configService)
		{
		}
	}
}
