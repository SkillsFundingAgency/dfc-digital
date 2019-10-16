using DFC.Digital.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.Taxonomies.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public interface IFlatTaxonomyRepository : IQueryRepository<FlatTaxon>, IDisposable
    {
    }
}
