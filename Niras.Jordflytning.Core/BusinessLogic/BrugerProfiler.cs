using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Niras.Jordflytning.Core.Interfaces.Business;
using Niras.Jordflytning.Core.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;

namespace Niras.Jordflytning.Core.BusinessLogic
{
	public class BrugerProfiler : IBrugerProfiler
	{
			private readonly IBrugerProfilerRepository _brugerProfilerRepository;

		public BrugerProfiler(IBrugerProfilerRepository brugerProfilerRepository)
		{
			_brugerProfilerRepository = brugerProfilerRepository;
		}

		public void Save(BrugerProfil brugerProfil)
		{
			_brugerProfilerRepository.Save(brugerProfil);	
		}

		public IEnumerable<BrugerProfil> Read()
		{
			return _brugerProfilerRepository.Read();
		}

	}
}
