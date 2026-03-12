using Niras.Jordflytning.Core;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Infrastructure.Common;
using System;
using System.Configuration;
using System.Data.Entity;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Infrastructure.Services
{
    public class ConfigService : Disposable, IConfigService
	{
		private DbContext dataContext;

		public Guid UniqueId { get; set; }

		public ConfigService()
		{
			UniqueId = Guid.NewGuid();
		}

		protected override void DisposeCore()
		{
			if (dataContext != null)
			{
				dataContext.Dispose();
				dataContext = null;
			}
		}
		
		public string ConnectionString
		{
			get
			{
				string cnString = null;
				var cnSettings = ConfigurationManager.ConnectionStrings["DefaultConnection"];
				if (cnSettings != null)
				{
					cnString = cnSettings.ConnectionString;
				}
				return cnString;
			}
		}

		public DbContext DataContext
		{
			get
			{
                return dataContext ?? (dataContext = new JordflytningEntities("DefaultConnection") ); // should not be necessary, since Ninject runs this class "InRequestScope", thus only one instance is created per HTTP request
			}
		}
	}
}
