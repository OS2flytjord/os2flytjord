using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business
{
	public interface IGenericBusiness<T>
	{
		void Create(T entity);
		void SaveChanges();
		T Read(Guid id);
		IList<T> Read();
		void Delete(T entity);
		void Reload(T entity);
		IQueryable<T> Search(Expression<Func<T,bool>> predicate);

	}
}
