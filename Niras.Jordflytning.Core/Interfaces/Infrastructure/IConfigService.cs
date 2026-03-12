using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niras.Jordflytning.Core.Interfaces.Infrastructure
{
	public interface IConfigService : IDisposable
	{
		String ConnectionString { get; }
		DbContext DataContext { get; }
	}
}
