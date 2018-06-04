using DFC.Digital.Data.Model;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class JobProfileRelatedCareerConverter : IDynamicModuleConverter<JobProfileRelatedCareer>
    {
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        public JobProfileRelatedCareerConverter(IDynamicContentExtensions dynamicContentExtensions)
        {
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        public JobProfileRelatedCareer ConvertFrom(DynamicContent content)
        {
            return new JobProfileRelatedCareer
            {
                Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.Title)),
                ProfileLink = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(JobProfile.UrlName)),
            };
        }
    }
}