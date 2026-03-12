using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using System;
using System.Linq;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
	public class BrugereRepository : GenericRepository<BrugerProfil>, IBrugereRepository
	{
		private static readonly ILogger Logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Infrastructure.DataAccess.BrugereRepository");

		public BrugereRepository(IConfigService configService) : base(configService)
		{
		}

		public BrugerProfil Read(Int32 userId)
		{			
			BrugerProfil result = null;
			lock (DbSet)
			{
				try
				{
					if (DbSet != null)
					{
						result = DbSet.FirstOrDefault(x => x.BrugerId == userId);
					}
				}
				catch (Exception e)
				{
					Logger.LogException("Error in method: Read by userId", e);
					throw;
				}
			}
			return result;
		}

		public BrugerProfil Read(String userName)
		{
			BrugerProfil result;
			lock (DbSet)
			{
				try
				{
					result = Read().FirstOrDefault(u => u.BrugerNavn.ToLowerInvariant() == userName.ToLowerInvariant());
				}
				catch (Exception e)
				{
					Logger.LogException("Error in method: Read by username", e);
					throw;
				}
			}
			return result;
		}
	}
}
