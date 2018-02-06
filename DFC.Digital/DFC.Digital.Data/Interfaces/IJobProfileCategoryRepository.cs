using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Data.Interfaces
{
    public interface IJobProfileCategoryRepository
    {
        JobProfileCategory GetByUrlName(string categoryUrlName);

        IEnumerable<JobProfileCategory> GetByIds(IList<Guid> categoryIds);

        IEnumerable<JobProfile> GetRelatedJobProfiles(string category);

        IQueryable<JobProfileCategory> GetJobProfileCategories();
    }
}