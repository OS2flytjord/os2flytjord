using System;
using System.Data.Entity;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure
{
	public interface IConfigService : IDisposable
	{
		String ConnectionString { get; }
		DbContext DataContext { get; }
	}
}
