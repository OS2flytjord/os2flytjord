using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Niras.Jordflytning.Core.Interfaces.Repository
{
	public interface IRepository<T>
	{
		T Create();
		void Delete(T entity);
		IEnumerable<T> Read();
		T Read(Guid id);
		void Add(T entity);
		IQueryable<T> Search(Expression<Func<T, bool>> predicate);
		void Reload(T entity);
	}
}
