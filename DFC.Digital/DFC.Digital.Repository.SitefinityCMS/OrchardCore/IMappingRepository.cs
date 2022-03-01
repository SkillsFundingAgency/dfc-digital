using DFC.Digital.Data.Model.OrchardCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Repository.SitefinityCMS.OrchardCore
{
    public interface IMappingRepository
    {
        void InsertMigrationMapping(Guid sitefinityId, string orchardCoreId, string contentType, string contentItemVersionId = "");

        IEnumerable<MigrationMapping> GetMigrationMappingBySitefinityId(Guid sitefinityId);
    }
}
