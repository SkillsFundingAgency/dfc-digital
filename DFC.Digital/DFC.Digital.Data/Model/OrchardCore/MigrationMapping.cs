using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model.OrchardCore
{
    public class MigrationMapping
    {
        public int MappingId { get; set; }
        public Guid SitefinityId { get; set; }
        public string OrchardCoreId { get; set; }
        public string ContentType { get; set; }
    }
}
