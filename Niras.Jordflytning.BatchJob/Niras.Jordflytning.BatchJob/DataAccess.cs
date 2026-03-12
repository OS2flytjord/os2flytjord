using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Spatial;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using Microsoft.SqlServer.Types;
using Niras.Jordflytning.Infrastructure.DataAccess;
using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Library.Logging;

namespace Niras.Jordflytning.BatchJob
{
    public static class DataAccess
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public static List<Tuple<Guid, string, string, string, string, Guid, int, Tuple<DbGeometry, DbGeometry, Guid, Guid>>> GetReviews(ILogger logger)
        {
            var reviews = new List<Tuple<Guid, string, string, string, string, Guid, int, Tuple<DbGeometry, DbGeometry, Guid, Guid>>>();

            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand("SP_BJ_SELECT_Anmeldelser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();
                        IDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            try
                            {
                                var anmeldelseId = dr["Id"] != DBNull.Value ? (Guid) dr["Id"] : Guid.Empty;
                                var ejerlav = dr["Ejerlav"] != DBNull.Value ? (string) dr["Ejerlav"] : "";
                                var ejerlavsNavn = dr["Ejerlavsnavn"] != DBNull.Value ? (string) dr["Ejerlavsnavn"] : "";
                                var matrikelNr = dr["Matrikelnr"] != DBNull.Value ? (string) dr["Matrikelnr"] : "";
                                var navn = dr["Navn"] != DBNull.Value ? (string) dr["Navn"] : "";
                                var kommuneId = dr["KommuneId"] != DBNull.Value ? (Guid) dr["KommuneId"] : Guid.Empty;
                                var kommuneNr = dr["Kommunenr"] != DBNull.Value ? (Int16) dr["Kommunenr"] : -1;
                                var oGeom = dr["OGeom"] != DBNull.Value
                                    ? DbGeometry.FromBinary(((SqlGeometry) dr["OGeom"]).STAsBinary().Buffer)
                                    : null;
                                var mGeom = dr["MGeom"] != DBNull.Value
                                    ? DbGeometry.FromBinary(((SqlGeometry) dr["MGeom"]).STAsBinary().Buffer)
                                    : null;
                                var jordKlassifikationTypeId = dr["JordKlassifikationTypeId"] != DBNull.Value
                                    ? (Guid) dr["JordKlassifikationTypeId"]
                                    : Guid.Empty;
                                var oprindelsesstedKlassifikationTypeId = dr["OprindelsesstedKlassifikationTypeId"] !=
                                                                          DBNull.Value
                                    ? (Guid) dr["OprindelsesstedKlassifikationTypeId"]
                                    : Guid.Empty;

                                reviews.Add(
                                    new Tuple
                                        <Guid, string, string, string, string, Guid, int,
                                            Tuple<DbGeometry, DbGeometry, Guid, Guid>>(anmeldelseId, ejerlav,
                                                ejerlavsNavn, matrikelNr, navn, kommuneId, kommuneNr,
                                                new Tuple<DbGeometry, DbGeometry, Guid, Guid>(oGeom, mGeom,
                                                    jordKlassifikationTypeId, oprindelsesstedKlassifikationTypeId)));
                            }
                            catch (Exception ex)
                            {
                                logger.LogException(ex);
                                Console.WriteLine(ex.ToString());
                            }
                        }

                        conn.Close();
                    }
                }
            }
            finally
            {
                if(conn != null)
                    conn.Close();
            }

            return reviews;
        }

        public static List<JordKlassifikationType> GetKommuneJordklassifikation()
        {
            List<JordKlassifikationType> result = new List<JordKlassifikationType>();
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand("SP_BJ_SELECT_KommuneJordklassifikation", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();
                        IDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            var kommuneId = dr["KommuneId"] != DBNull.Value ? (Guid)dr["KommuneId"] : Guid.Empty;
                            var kode = dr["Kode"] != DBNull.Value ? (short)dr["Kode"] : 0;
                            var jordKlassifikationTypeId = dr["JordKlassifikationTypeId"] != DBNull.Value ? (Guid)dr["JordKlassifikationTypeId"] : Guid.Empty;
                            var priotering = dr["Priotering"] != DBNull.Value ? (short) dr["Priotering"] : 0;
                            result.Add(new JordKlassifikationType() { Id = jordKlassifikationTypeId, Kode = (short)kode, LandsdelTypeId = kommuneId, Priotering = (short)priotering });
                        }

                        conn.Close();
                    }
                }
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }

            return result;
        }
        
        public static void UpdateReviews(Dictionary<Guid, Guid> reviews)
        {
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    // CLEAN UP OLD FLAGS!
                    using (var cmd = new SqlCommand("SP_BJ_UPDATE_ResetAnmeldelser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd = new SqlCommand("SP_BJ_UPDATE_Anmeldelse", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("Id", SqlDbType.UniqueIdentifier));
                        cmd.Parameters.Add(new SqlParameter("JFId", SqlDbType.UniqueIdentifier));

                        foreach (var review in reviews)
                        {
                            cmd.Parameters["Id"].Value = review.Key;
                            cmd.Parameters["JFId"].Value = review.Value;
                            cmd.ExecuteNonQuery();
                        }
                    }

                    conn.Close();
                }
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }
    }
}
