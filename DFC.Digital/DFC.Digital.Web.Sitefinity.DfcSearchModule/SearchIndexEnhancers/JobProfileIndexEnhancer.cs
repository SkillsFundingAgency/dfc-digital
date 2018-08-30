using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using Newtonsoft.Json;
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
        private readonly IApplicationLogger applicationLogger;
        private JobProfileIndex jobProfileIndex;

        public JobProfileIndexEnhancer(
            IJobProfileRepository jobProfileRepository,
            IJobProfileCategoryRepository jobProfileCategoryRepository,
            ISalaryService salaryService,
            ISalaryCalculator salaryCalculator,
            IApplicationLogger applicationLogger)
        {
            this.jobProfileRepository = jobProfileRepository;
            this.jobProfileCategoryRepository = jobProfileCategoryRepository;
            this.salaryService = salaryService;
            this.salaryCalculator = salaryCalculator;
            this.applicationLogger = applicationLogger;
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

        public Task PopulateSalary()
        {
            if (!string.IsNullOrEmpty(JobProfile.SOCCode))
            {
                //When there are no SOC code, leave the salary as default. Displayed as variable by the screen.
                return PopulateSalaryFromLMIAsync(JobProfile.SOCCode);
            }

            return Task.CompletedTask;
        }

        private async Task PopulateSalaryFromLMIAsync(string socCode)
        {
            applicationLogger.Trace($"BEGIN: Method '{nameof(PopulateSalaryFromLMIAsync)}' called from '{nameof(JobProfileIndexEnhancer)}' with parameters '{socCode}'.");
            JobProfileSalary salary = new JobProfileSalary();
            try
            {
                salary = await salaryService.GetSalaryBySocAsync(socCode);
                jobProfileIndex.SalaryStarter = Convert.ToDouble(salaryCalculator.GetStarterSalary(salary));
                jobProfileIndex.SalaryExperienced = Convert.ToDouble(salaryCalculator.GetExperiencedSalary(salary));
            }
            catch (Exception ex)
            {
                //If there is a failure for this profile, log and continue with other profiles
                applicationLogger.ErrorJustLogIt($"ERROR: Method '{nameof(PopulateSalaryFromLMIAsync)}' called from '{nameof(JobProfileIndexEnhancer)}' with parameters '{socCode}' failed with exception '{ex.Message}'.", ex);
            }
            finally
            {
                applicationLogger.Trace($"END: Method '{nameof(PopulateSalaryFromLMIAsync)}' called from '{nameof(JobProfileIndexEnhancer)}' with parameters '{socCode}' returned {JsonConvert.SerializeObject(salary)}.");
            }
        }

        private IEnumerable<string> GetJobProfileCategoriesWithUrl()
        {
            var categories = jobProfileCategoryRepository.GetByIds(JobProfile.JobProfileCategoryIdCollection);
            return categories.Select(c => $"{c.Title}|{c.Url}");
        }
    }
}