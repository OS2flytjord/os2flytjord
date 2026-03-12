using System.Data;
using System.Data.Entity.Validation;
using System.ServiceModel.Channels;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Niras.Jordflytning.Library.Logging;
using System.Data.Entity.Infrastructure;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
	public class GenericRepository<T> : IRepository<T> where T : class 
	{
		protected DbSet<T> DbSet;
		protected DbContext DataContext;

		private static readonly ILogger logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Infrastructure.DataAccess.GenericRepository");

		public GenericRepository(IConfigService configService)
		{
			DbSet = configService.DataContext.Set<T>();
			DataContext = configService.DataContext;
            ((IObjectContextAdapter)DataContext).ObjectContext.CommandTimeout = 180;
		}

		//internal DbSet<T> DbSet 
		//{ 
		//	get; 
		//	private set; 
		//}

		#region IRepository<T> Members

		public void Reload(T entity)
		{
			DataContext.Entry(entity).Reload();
		}

		public T Create()
		{
			return default(T);
		}

		public void Delete(T entity)
		{
			DbSet.Remove(entity);
		}

		public IEnumerable<T> Read()
		{
			return DbSet;
		}

		public T Read(Guid id)
		{
      //Dette kode giver de objekter der er ændret eller vil blive tilføjet når SaveChanges() kaldes.
      //List<Object> modifiedOrAddedEntities = DataContext.ChangeTracker.Entries().Where(x => x.State == System.Data.EntityState.Modified || x.State == System.Data.EntityState.Added).Select(x => x.Entity).ToList();

			return DbSet.Find(id);
		}

		public void Add(T entity)
		{
			DbSet.Add(entity);
		}

		public IQueryable<T> Search(Expression<Func<T, bool>> predicate)
		{
			return DbSet.Where(predicate);
		}

		public void Commit()
		{
			try
			{
				var test = DataContext.ChangeTracker.Entries().Where(x => x.State == System.Data.EntityState.Modified || x.State == System.Data.EntityState.Added).Select(x => x.Entity).ToList();

				List<Object> validationFailed = DataContext.ChangeTracker.Entries().Where(x => !x.GetValidationResult().IsValid && (x.State == System.Data.EntityState.Modified || x.State == System.Data.EntityState.Added)).Select(x => x.Entity).ToList();

				if (validationFailed.Count > 0)
				{
					//Dette kode giver de objekter der er ændret eller vil blive tilføjet når SaveChanges() kaldes.
					List<Object> modifiedOrAddedEntities = DataContext.ChangeTracker.Entries().Where(x => x.State == System.Data.EntityState.Modified || x.State == System.Data.EntityState.Added).Select(x => x.Entity).ToList();

					foreach (var o in modifiedOrAddedEntities)
					{
						var entryType = o.GetType().Name;
						var values = DataContext.Entry(o).CurrentValues;

						var validationRes = DataContext.Entry(o).GetValidationResult();
						foreach (var dbValidationError in validationRes.ValidationErrors)
						{
							logger.LogInfo(string.Format("Entity: {0}. DbValidationError: {1}", dbValidationError.PropertyName, dbValidationError.ErrorMessage));
						}

						foreach (var propertyName in values.PropertyNames)
						{
							logger.LogInfo(string.Format("Entity: {0}. Property {1}: has value {2}", entryType, propertyName, values[propertyName]));
						}

						//Kode der kan rulle ændringer tilbage, således EF ikke bliver ubrugelige.
            //switch (DataContext.Entry(o).State)
            //{
            //  case EntityState.Modified:
            //    DataContext.Entry(o).State = EntityState.Unchanged;
            //    logger.LogInfo(string.Format("Entity: {0}. State changed from Modified to Unchanged", entryType));
            //    break;
            //  case EntityState.Deleted:
            //    DataContext.Entry(o).Reload();
            //    logger.LogInfo(string.Format("Entity: {0}. State changed from Deleted to Reload", entryType));
            //    break;
            //  case EntityState.Added:
            //    DataContext.Entry(o).State = EntityState.Detached;
            //    logger.LogInfo(string.Format("Entity: {0}. State changed from Added to Detached", entryType));
            //    break;
            //  default:
            //    break;
            //}
					}

					throw new Exception("Fejl i database operation. Dataene blev ikke gemt, da objekterne er ikke valide.");

				}

				DataContext.SaveChanges();
			}
			catch (DbEntityValidationException e)
			{
				foreach (var eve in e.EntityValidationErrors)
				{
					var s = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
					logger.LogException(s, e);

					foreach (var ve in eve.ValidationErrors)
					{
						var ss = string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
						logger.LogException(ss, e);
					}
				}
				throw;
			}
			catch (Exception e)
			{
				//Kan dataene ikke gemmes pga mystiske fejl, prøver vi her at logge objekterne som er blevet ændret og rulle dem tilbage, så de ikke spærer for alle andre forsøg på at gemme i databasen.
				List<Object> modifiedOrAddedEntities = DataContext.ChangeTracker.Entries()
		.Where(x => x.State == System.Data.EntityState.Modified
					|| x.State == System.Data.EntityState.Added)
		.Select(x => x.Entity).ToList();

				if (modifiedOrAddedEntities != null)
				{
					foreach (var o in modifiedOrAddedEntities)
					{
						var entryType = o.GetType().Name;
						var values = DataContext.Entry(o).CurrentValues;

						foreach (var propertyName in values.PropertyNames)
						{
							logger.LogInfo(string.Format("Entity: {0}. Property {1}: has value {2}", entryType, propertyName, values[propertyName]));
						}

						//Kode der kan rulle ændringer tilbage, således EF ikke bliver ubrugelige.
            //switch (DataContext.Entry(o).State)
            //{
            //  case EntityState.Modified:
            //    DataContext.Entry(o).State = EntityState.Unchanged;
            //    logger.LogInfo(string.Format("Entity: {0}. State changed from Modified to Unchanged", entryType));
            //    break;
            //  case EntityState.Deleted:
            //    DataContext.Entry(o).Reload();
            //    logger.LogInfo(string.Format("Entity: {0}. State changed from Deleted to Reload", entryType));
            //    break;
            //  case EntityState.Added:
            //    DataContext.Entry(o).State = EntityState.Detached;
            //    logger.LogInfo(string.Format("Entity: {0}. State changed from Added to Detached", entryType));
            //    break;
            //  default:
            //    break;
            //}
					}
				}

				logger.LogException(e);
				throw new Exception("Fejl i database operation", e);
			}
		}

        public IEnumerable<T> ExecuteSqlQuery(string sqlQuery)
        {
            return DbSet.SqlQuery(sqlQuery);
        }
		#endregion
    }
}
