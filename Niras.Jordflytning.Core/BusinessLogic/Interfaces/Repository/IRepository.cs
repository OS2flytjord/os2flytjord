using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository
{
	public interface IRepository<T>
	{
		T Create();
		void Delete(T entity);
		IEnumerable<T> Read();
		T Read(Guid id);
		void Add(T entity);
		IQueryable<T> Search(Expression<Func<T, bool>> predicate);
	    IEnumerable<T> ExecuteSqlQuery(string sqlQuery);
		void Reload(T entity);

		void Commit();
	}
}
