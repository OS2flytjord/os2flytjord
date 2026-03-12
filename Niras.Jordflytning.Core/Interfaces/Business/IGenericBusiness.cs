using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Niras.Jordflytning.Core.Interfaces.Business
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
