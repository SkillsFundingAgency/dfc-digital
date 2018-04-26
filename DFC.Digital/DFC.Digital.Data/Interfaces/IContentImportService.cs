using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface IContentImportService<T>
    {
        string Import(T bauJobProfile, Dictionary<string, string> propertyMappings, string changeComment, bool enforcePublishing, bool disableUpdate);

        string UpdateRelatedCareers(JobProfileImporting bauJobProfile, string changeComment, bool enforcePublishing);
    }
}
