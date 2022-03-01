using DFC.Digital.Data.Model.OrchardCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DFC.Digital.Repository.SitefinityCMS.OrchardCore
{
    public class MappingRepository : IMappingRepository
    {
        public void InsertMigrationMapping(Guid sitefinityId, string orchardCoreId, string contentType, string contentItemVersionId = "")
        {
            string connectionString = "data source=.;UID=cds-sitefinity;PWD=cds-sitefinity;initial catalog=dfc-digital-sitefinity";

            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("MigrationToolMappingInsert", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SitefinityId", SqlDbType.UniqueIdentifier).Value = sitefinityId;
                cmd.Parameters.Add("@OrchardCoreId", SqlDbType.NVarChar, 450).Value = orchardCoreId;
                cmd.Parameters.Add("@ContentItemVersionId", SqlDbType.NVarChar, 450).Value = contentItemVersionId;
                cmd.Parameters.Add("@ContentType", SqlDbType.NVarChar, 450).Value = contentType;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        public IEnumerable<MigrationMapping> GetMigrationMappingBySitefinityId(Guid sitefinityId)
        {
            var migrationMappings = new List<MigrationMapping>();
            string connectionString = "data source=.;UID=cds-sitefinity;PWD=cds-sitefinity;initial catalog=dfc-digital-sitefinity";

            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("MigrationToolMappingGetBySitefinityId", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SitefinityId", SqlDbType.UniqueIdentifier).Value = sitefinityId;

                cn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    var migrationMapping = new MigrationMapping()
                    {
                        MappingId = (int)sdr["MappingId"],
                        SitefinityId = (Guid)sdr["SitefinityId"],
                        OrchardCoreId = (string)sdr["OrchardCoreId"],
                        ContentItemVersionId = sdr["ContentItemVersionId"] == null || sdr["ContentItemVersionId"] == DBNull.Value ? string.Empty : (string)sdr["ContentItemVersionId"],
                        ContentType = (string)sdr["ContentType"]
                    };
                    migrationMappings.Add(migrationMapping);
                }

                cn.Close();
            }

            return migrationMappings;
        }
    }
}