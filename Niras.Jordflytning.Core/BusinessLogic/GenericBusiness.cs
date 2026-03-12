using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Business;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Niras.Jordflytning.Core.BusinessLogic
{
    public class GenericBusiness<TObject> : IGenericBusiness<TObject> where TObject : class
	{
		private IRepository<TObject> repo;
		private IUnitOfWork unitOfWork;

		public GenericBusiness(IRepository<TObject> repo, IUnitOfWork unitOfWork)
		{
			this.repo = repo;
			this.unitOfWork = unitOfWork;
		}

		public void Reload(TObject entity)
		{
			repo.Reload(entity);
		}

		public void Create(TObject entity)
		{
			var propertyInfo = entity.GetType().GetProperty("Id");
			if (propertyInfo != null)
			{
				var value = propertyInfo.GetValue(entity, null);
				if (value  as Guid? == Guid.Empty || value as Int32? == 0)
				{
					repo.Add(entity);
				}
			}
            SaveChanges();
		}

		public void SaveChanges()
		{
			repo.Commit();

			

			//unitOfWork.Commit();
		}

		public IList<TObject> Read()
		{
			return repo.Read().ToList();
		}

		public TObject Read(Guid id)
		{
			return repo.Read(id);
		}

		public IQueryable<TObject> Search(Expression<Func<TObject,bool>> predicate)
		{
			return repo.Search(predicate);
		}

		public void Delete(TObject objectToDelete)
		{
			repo.Delete(objectToDelete);
			SaveChanges();
		}
	}

}
