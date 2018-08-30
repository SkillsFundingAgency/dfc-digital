using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Digital.Web.Sitefinity.DfcSearchModule
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

        public void Initialise(JobProfileIndex initialiseJobProfileIndex, bool isPublishing)
        {
            if (initialiseJobProfileIndex == null)
            {
                throw new ArgumentNullException(nameof(initialiseJobProfileIndex));
            }

            this.JobProfile = jobProfileRepository.GetByUrlNameForSearchIndex(initialiseJobProfileIndex.UrlName, isPublishing);
            this.jobProfileIndex = initialiseJobProfileIndex;
        }

        public void PopulateRelatedFieldsWithUrl()
        {
            if (JobProfile != null)
            {
                jobProfileIndex.JobProfileCategoriesWithUrl = GetJobProfileCategoriesWithUrl();
            }
        }

        public void PopulateSalary()
        {
            if (!string.IsNullOrEmpty(JobProfile.SOCCode))
            {
                //When there are no SOC code, leave the salary as default. Displayed as variable by the screen.
                PopulateSalaryFromLmiAsync(JobProfile.SOCCode);
            }
        }

        private void PopulateSalaryFromLmiAsync(string socCode)
        {
            var salary = Task.Run(() => salaryService.GetSalaryBySocAsync(socCode)).Result;
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