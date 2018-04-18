using DFC.Digital.Data.Model;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class JobProfileRelatedCareerConverter : IDynamicModuleConverter<JobProfileRelatedCareer>
    {
        public JobProfileRelatedCareer ConvertFrom(DynamicContent content)
        {
            return new JobProfileRelatedCareer
            {
                Title = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.Title)),
                ProfileLink = content?.GetValueOrDefault<Lstring>(nameof(JobProfile.UrlName)),
            };
        }
    }
}