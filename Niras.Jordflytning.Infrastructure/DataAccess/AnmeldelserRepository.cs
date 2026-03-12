using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Infrastructure;
using Niras.Jordflytning.Core.BusinessLogic.Interfaces.Repository;
using Niras.Jordflytning.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Niras.Jordflytning.Infrastructure.DataAccess
{
    public class AnmeldelserRepository : GenericRepository<Anmeldelse>, IAnmeldelserRepository
    {

        public AnmeldelserRepository(IConfigService configService)
          : base(configService)
        {
        }

        //public IEnumerable<Anmeldelse> ReadIncludeStatusAnmeldelse()
        //{
        //  //Eager loading
        //  //Eager loading is the process whereby a query for one type of entity also loads related entities as part of the query. Eager loading is achieved by use of the Include method
        //  //http://msdn.microsoft.com/en-us/data/jj574232.aspx

        //  return DbSet.Include("StatusAnmeldelse.StatusAnmeldelseType");
        //}

        public new void Reload(Anmeldelse entity)
        {
            DataContext.Entry(entity).Reload();

            var context = ((IObjectContextAdapter)DataContext).ObjectContext;
            var refreshableObjects = DataContext.ChangeTracker.Entries().Select(c => c.Entity).ToList();
            context.Refresh(RefreshMode.StoreWins, refreshableObjects);
        }

        public IEnumerable<Anmeldelse> ReadIncludeVognlaesStikproeve()
        {
            return DbSet.Include(x => x.AnmeldelseEffektivStatus).Include("Vognlaes.Stikproeve.StatusStikproeve.StatusStikproeveType");
        }

        public new Anmeldelse Read(Guid id)
        {
            return DbSet.Include(x => x.AnmeldelseEffektivStatus).FirstOrDefault(x => x.Id == id);
        }

        public new IEnumerable<Anmeldelse> Read()
        {
            return DbSet.Include(x => x.AnmeldelseEffektivStatus);
        }

        public new IQueryable<Anmeldelse> Search(Expression<Func<Anmeldelse, bool>> predicate)
        {
            return DbSet.Include(x => x.AnmeldelseEffektivStatus).Where(predicate);
        }

        #region Bypass EF Search!
        public IEnumerable<Anmeldelse> SearchBypassEF(Guid? KommuneId, string ModtagerAnlaegXmlList, DateTime? foerDate, DateTime? efterDate, decimal? loebeNr, Guid? modtagerId, Guid? transportoerId, Guid? anmelderId, string forureningsKategori)
        {
            var sqlQuery = new StringBuilder();
            sqlQuery.Append("exec dbo.[SP_SELECT_Anmeldelse_Soegning] ");

            var parameters = new List<SqlParameter>();
            if (KommuneId.HasValue)
            {
                sqlQuery.Append("@KommuneId ");
                parameters.Add(new SqlParameter("KommuneId", KommuneId.Value));
                sqlQuery.Append(",@ModtagerAnlaegXmlList ");
                parameters.Add(new SqlParameter("ModtagerAnlaegXmlList", DBNull.Value));
            }

            if (ModtagerAnlaegXmlList != string.Empty)
            {
                sqlQuery.Append("@KommuneId ");
                parameters.Add(new SqlParameter("KommuneId", DBNull.Value));

                sqlQuery.Append(",@ModtagerAnlaegXmlList ");
                parameters.Add(new SqlParameter("ModtagerAnlaegXmlList", ModtagerAnlaegXmlList));
            }

            sqlQuery.Append(",@FoerDate ");
            if (foerDate.HasValue)
            {
                parameters.Add(new SqlParameter("FoerDate", foerDate.Value));
            }
            else
            {
                parameters.Add(new SqlParameter("FoerDate", DBNull.Value));
            }

            sqlQuery.Append(",@EfterDate ");
            if (efterDate.HasValue)
            {
                parameters.Add(new SqlParameter("EfterDate", efterDate.Value));
            }
            else
            {
                parameters.Add(new SqlParameter("EfterDate", DBNull.Value));
            }

            sqlQuery.Append(",@LoebeNr ");
            if (loebeNr.HasValue && loebeNr.Value > 0)
            {
                parameters.Add(new SqlParameter("LoebeNr", loebeNr.Value));
            }
            else
            {
                parameters.Add(new SqlParameter("LoebeNr", DBNull.Value));
            }

            sqlQuery.Append(",@ModtagerId ");
            if (modtagerId.HasValue && modtagerId.Value != Guid.Empty)
            {
                parameters.Add(new SqlParameter("ModtagerId", modtagerId.Value));
            }
            else
            {
                parameters.Add(new SqlParameter("ModtagerId", DBNull.Value));
            }

            sqlQuery.Append(",@TransportoerId ");
            if (transportoerId.HasValue && transportoerId.Value != Guid.Empty)
            {
                parameters.Add(new SqlParameter("TransportoerId", transportoerId.Value));
            }
            else
            {
                parameters.Add(new SqlParameter("TransportoerId", DBNull.Value));
            }

            sqlQuery.Append(",@AnmelderId ");
            if (anmelderId.HasValue && anmelderId.Value != Guid.Empty)
            {
                parameters.Add(new SqlParameter("AnmelderId", anmelderId.Value));
            }
            else
            {
                parameters.Add(new SqlParameter("AnmelderId", DBNull.Value));
            }

            sqlQuery.Append(",@Forureningskategori ");
            if (forureningsKategori != String.Empty)
            {
                parameters.Add(new SqlParameter("Forureningskategori", forureningsKategori));
            }
            else
            {
                parameters.Add(new SqlParameter("Forureningskategori", DBNull.Value));
            }

            return DbSet.SqlQuery(sqlQuery.ToString(), parameters.ToArray());
        }
        #endregion

        public IEnumerable<Anmeldelse> SearchInclude(Expression<Func<Anmeldelse, bool>> predicate, EnumIncludeTables includeTables)
        {
            //Eager loading
            //Eager loading is the process whereby a query for one type of entity also loads related entities as part of the query. Eager loading is achieved by use of the Include method
            //http://msdn.microsoft.com/en-us/data/jj574232.aspx

            string strInclude = "";
            switch (includeTables)
            {
                //case EnumIncludeTables.StatusAnmeldelse_StatusAnmeldelseType:
                //  strInclude = "StatusAnmeldelse.StatusAnmeldelseType";
                //  break;

                case EnumIncludeTables.Vognlaes_Stikproeve_StatusStikproeve_StatusStikproeveType:
                    strInclude = "Vognlaes.Stikproeve.StatusStikproeve.StatusStikproeveType";
                    break;
                default:
                    break;
            }

            return DbSet.Where(predicate).Include(x => x.AnmeldelseEffektivStatus).Include(strInclude);
        }

    }
}
