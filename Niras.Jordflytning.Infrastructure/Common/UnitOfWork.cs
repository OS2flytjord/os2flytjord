using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using System;
using System.Data.Entity;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Library.Logging;
using System.Data.Entity.Infrastructure;

namespace Niras.Jordflytning.Infrastructure.Common
{
	public class UnitOfWork : IUnitOfWork
	{
		private static readonly ILogger logger = LogManager.Instance.GetLogger("Niras.Jordflytning.Core.BusinessLogi.UnitOfWorkc");
		private readonly DbContext _dataContext;
		private bool _disposed;

		public UnitOfWork(IConfigService configService)
		{
			_dataContext = configService.DataContext;
            ((IObjectContextAdapter)_dataContext).ObjectContext.CommandTimeout = 180;
		}

		public void Commit()
		{
			try
			{
				var test = _dataContext.ChangeTracker.Entries().Where(x => x.State == System.Data.EntityState.Modified || x.State == System.Data.EntityState.Added).Select(x => x.Entity).ToList();

        List<Object> validationFailed = _dataContext.ChangeTracker.Entries()
.Where(x => !x.GetValidationResult().IsValid && (x.State == System.Data.EntityState.Modified
        || x.State == System.Data.EntityState.Added))
.Select(x => x.Entity).ToList();

        if (validationFailed.Count > 0)
        {
          //Dette kode giver de objekter der er ændret eller vil blive tilføjet når SaveChanges() kaldes.
          List<Object> modifiedOrAddedEntities = _dataContext.ChangeTracker.Entries()
    .Where(x => x.State == System.Data.EntityState.Modified
          || x.State == System.Data.EntityState.Added)
    .Select(x => x.Entity).ToList();

          foreach (var o in modifiedOrAddedEntities)
          {
            var entryType = o.GetType().Name;
            var values = _dataContext.Entry(o).CurrentValues;

            var validationRes = _dataContext.Entry(o).GetValidationResult();
            foreach (var dbValidationError in validationRes.ValidationErrors)
            {
              logger.LogInfo(string.Format("Entity: {0}. DbValidationError: {1}",dbValidationError.PropertyName,dbValidationError.ErrorMessage)); 
            }

            foreach (var propertyName in values.PropertyNames)
            {
              logger.LogInfo(string.Format("Entity: {0}. Property {1}: has value {2}",entryType, propertyName, values[propertyName])); 
            }

            //Kode der kan rulle ændringer tilbage, således EF ikke bliver ubrugelige.
            switch (_dataContext.Entry(o).State)
            {
              case EntityState.Modified:
                _dataContext.Entry(o).State = EntityState.Unchanged;
                logger.LogInfo(string.Format("Entity: {0}. State changed from Modified to Unchanged", entryType)); 
                break;
              case EntityState.Deleted:
                _dataContext.Entry(o).Reload();
                logger.LogInfo(string.Format("Entity: {0}. State changed from Deleted to Reload", entryType)); 
                break;
              case EntityState.Added:
                _dataContext.Entry(o).State = EntityState.Detached;
                logger.LogInfo(string.Format("Entity: {0}. State changed from Added to Detached", entryType)); 
                break;
              default:
                break;
            }
          }

          throw new Exception("Fejl i database operation. Dataene blev ikke gemt, da objekterne er ikke valide.");

        }

				_dataContext.SaveChanges();
			}
      catch (DbEntityValidationException e)
      {
        foreach (var eve in e.EntityValidationErrors)
        {
          var s = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
          logger.LogException(s,e);

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
        List<Object> modifiedOrAddedEntities = _dataContext.ChangeTracker.Entries()
    .Where(x => x.State == System.Data.EntityState.Modified
          || x.State == System.Data.EntityState.Added)
    .Select(x => x.Entity).ToList();

			  if (modifiedOrAddedEntities != null)
			  {
          foreach (var o in modifiedOrAddedEntities)
          {
            var entryType = o.GetType().Name;
            var values = _dataContext.Entry(o).CurrentValues;

            foreach (var propertyName in values.PropertyNames)
            {
              logger.LogInfo(string.Format("Entity: {0}. Property {1}: has value {2}", entryType, propertyName, values[propertyName]));
            }

            //Kode der kan rulle ændringer tilbage, således EF ikke bliver ubrugelige.
            switch (_dataContext.Entry(o).State)
            {
              case EntityState.Modified:
                _dataContext.Entry(o).State = EntityState.Unchanged;
                logger.LogInfo(string.Format("Entity: {0}. State changed from Modified to Unchanged", entryType));
                break;
              case EntityState.Deleted:
                _dataContext.Entry(o).Reload();
                logger.LogInfo(string.Format("Entity: {0}. State changed from Deleted to Reload", entryType));
                break;
              case EntityState.Added:
                _dataContext.Entry(o).State = EntityState.Detached;
                logger.LogInfo(string.Format("Entity: {0}. State changed from Added to Detached", entryType));
                break;
              default:
                break;
            }
          }
			  }

			  logger.LogException(e);
        throw new Exception("Fejl i database operation",e);
			}
		}

		#region IDisposable implementation
	
    protected virtual void Dispose(bool disposing)
    {
			if (!_disposed)
			{
				if (disposing)
				{
					_dataContext.Dispose();
				}
			}
			_disposed = true;
    }

    public void Dispose()
    {
			Dispose(true);
      GC.SuppressFinalize(this);
    }   

		#endregion
	}
}
