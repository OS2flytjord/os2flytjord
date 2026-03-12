using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;

namespace Niras.Jordflytning.Core.BusinessLogic
{
	public abstract class BaseBusiness
	{
		private IUnitOfWork unitOfWork;
		protected BaseBusiness(IUnitOfWork uow)
		{
			unitOfWork = uow;
		}

		public void Update()
		{
			//unitOfWork.Commit();
		}
	}
}
