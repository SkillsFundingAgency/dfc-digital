using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Digital.Web.Sitefinity.DfcSearchModule.SearchIndexEnhancers
{
    public class JobProfileIndexEnhancer : IJobProfileIndexEnhancer
    {
        private readonly IJobProfileRepository jobProfileRepository;
        private readonly IJobProfileCategoryRepository jobProfileCategoryRepository;
        private readonly ISalaryService salaryService;
        private readonly ISalaryCalculator salaryCalculator;

        public JobProfileIndexEnhancer(IJobProfileRepository jobProfileRepository, IJobProfileCategoryRepository jobProfileCategoryRepository, ISalaryService salaryService, ISalaryCalculator salaryCalculator)
        {
            this.jobProfileRepository = jobProfileRepository;
            this.jobProfileCategoryRepository = jobProfileCategoryRepository;
            this.salaryService = salaryService;
            this.salaryCalculator = salaryCalculator;
        }

        public JobProfile JobProfile { get; private set; }

        public void Initialise(JobProfileIndex jobProfileIndex)
        {
           this.JobProfile = jobProfileRepository.GetByUrlName(jobProfileIndex.UrlName);
        }

        public JobProfileIndex GetRelatedFieldsWithUrl(JobProfileIndex jobProfileIndex)
        {
            if (JobProfile != null)
            {
                jobProfileIndex.JobProfileCategoriesWithUrl = GetJobProfileCategoriesWithUrl(JobProfile);

                jobProfileIndex.Interests = JobProfile.RelatedInterests;
                jobProfileIndex.Enablers = JobProfile.RelatedEnablers;
                jobProfileIndex.EntryQualifications = JobProfile.RelatedEntryQualifications;
                jobProfileIndex.TrainingRoutes = JobProfile.RelatedTrainingRoutes;
                jobProfileIndex.JobAreas = JobProfile.RelatedJobAreas;
                jobProfileIndex.PreferredTaskTypes = JobProfile.RelatedPreferredTaskTypes;
            }

            return jobProfileIndex;
        }

        public async Task<string> GetSalaryRangeAsync()
        {
            if (JobProfile.IsLmiSalaryFeedOverriden.HasValue && JobProfile.IsLmiSalaryFeedOverriden.Value != true)
            {
                var salary = await salaryService.GetSalaryBySocAsync(JobProfile.SocCode);
                JobProfile.SalaryStarter = salaryCalculator.GetStarterSalary(salary);
                JobProfile.SalaryExperienced = salaryCalculator.GetExperiencedSalary(salary);
            }

            return !JobProfile.SalaryStarter.HasValue || !JobProfile.SalaryExperienced.HasValue || JobProfile.SalaryStarter.Value.Equals(0) || JobProfile.SalaryExperienced.Value.Equals(0)
                ? null
                : string.Format(new CultureInfo("en-GB", false), "{0:C0} to {1:C0}", JobProfile.SalaryStarter, JobProfile.SalaryExperienced);
        }

        private IEnumerable<string> GetJobProfileCategoriesWithUrl(JobProfile jobProfile)
        {
            var categories = jobProfileCategoryRepository.GetByIds(jobProfile.JobProfileCategoryIdCollection);
            return categories.Select(c => $"{c.Title}|{c.Url}");
        }
    }
}