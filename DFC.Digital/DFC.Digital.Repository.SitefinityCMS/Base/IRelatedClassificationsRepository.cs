using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Base
{
    public interface IRelatedClassificationsRepository
    {
        IQueryable<string> GetRelatedClassifications(DynamicContent content, string relatedField, string taxonomyName);
    }
}
