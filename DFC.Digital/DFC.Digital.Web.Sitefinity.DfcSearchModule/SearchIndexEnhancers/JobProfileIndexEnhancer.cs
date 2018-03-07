using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
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
        private JobProfileIndex jobProfileIndex;

        public JobProfileIndexEnhancer(
            IJobProfileRepository jobProfileRepository,
            IJobProfileCategoryRepository jobProfileCategoryRepository,
            ISalaryService salaryService,
            ISalaryCalculator salaryCalculator)
        {
            this.jobProfileRepository = jobProfileRepository;
            this.jobProfileCategoryRepository = jobProfileCategoryRepository;
            this.salaryService = salaryService;
            this.salaryCalculator = salaryCalculator;
        }

        public JobProfile JobProfile { get; private set; }

        public void Initialise(JobProfileIndex jobProfileIndex, bool isPublishing)
        {
            if (jobProfileIndex == null)
            {
                throw new ArgumentNullException(nameof(jobProfileIndex));
            }

            this.JobProfile = isPublishing ? jobProfileRepository.GetByUrlNameForSearchIndex(jobProfileIndex.UrlName) : jobProfileRepository.GetByUrlName(jobProfileIndex.UrlName);
            this.jobProfileIndex = jobProfileIndex;
        }

        public void PopulateRelatedFieldsWithUrl()
        {
            if (JobProfile != null)
            {
                jobProfileIndex.JobProfileCategoriesWithUrl = GetJobProfileCategoriesWithUrl();

                jobProfileIndex.Interests = JobProfile.RelatedInterests.ToList();
                jobProfileIndex.Enablers = JobProfile.RelatedEnablers.ToList();
                jobProfileIndex.EntryQualifications = JobProfile.RelatedEntryQualifications.ToList();
                jobProfileIndex.TrainingRoutes = JobProfile.RelatedTrainingRoutes.ToList();
                jobProfileIndex.JobAreas = JobProfile.RelatedJobAreas.ToList();
                jobProfileIndex.PreferredTaskTypes = JobProfile.RelatedPreferredTaskTypes.ToList();
            }
        }

        public Task PopulateSalary()
        {
            // Conversions taking place because sitefinity returns Decimal and Azure Search accepts Double fields
            if (JobProfile.IsLMISalaryFeedOverridden == true)
            {
                jobProfileIndex.SalaryStarter = Convert.ToDouble(JobProfile.SalaryStarter);
                jobProfileIndex.SalaryExperienced = Convert.ToDouble(JobProfile.SalaryExperienced);
            }
            else if (!string.IsNullOrEmpty(JobProfile.SOCCode))
            {
                //When there are no SOC code, leave the salary as default. Displayed as variable by the screen.
                //Mutate soc code
                var socCode = JobProfile.SOCCode;
                return Task.Run(() => PopulateSalaryFromLMIAsync(socCode));
            }

            return Task.CompletedTask;
        }

        private async Task PopulateSalaryFromLMIAsync(string socCode)
        {
            var salary = await salaryService.GetSalaryBySocAsync(socCode);
            jobProfileIndex.SalaryStarter = Convert.ToDouble(salaryCalculator.GetStarterSalary(salary));
            jobProfileIndex.SalaryExperienced = Convert.ToDouble(salaryCalculator.GetExperiencedSalary(salary));
        }

        private IEnumerable<string> GetJobProfileCategoriesWithUrl()
        {
            var categories = jobProfileCategoryRepository.GetByIds(JobProfile.JobProfileCategoryIdCollection);
            return categories.Select(c => $"{c.Title}|{c.Url}");
        }
    }
}