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

        public JobProfileOverloadForSearch JobProfile { get; private set; }

        public void Initialise(JobProfileIndex initialiseJobProfileIndex, bool isPublishing)
        {
            if (initialiseJobProfileIndex == null)
            {
                throw new ArgumentNullException(nameof(initialiseJobProfileIndex));
            }

            JobProfile = jobProfileRepository.GetByUrlNameForSearchIndex(initialiseJobProfileIndex.UrlName, isPublishing);
            jobProfileIndex = initialiseJobProfileIndex;
            PopulateCustomFields();
        }

        public void PopulateRelatedFieldsWithUrl()
        {
            if (JobProfile != null)
            {
                jobProfileIndex.JobProfileCategoriesWithUrl = GetJobProfileCategoriesWithUrl();
            }
        }

        public async Task<JobProfileSalary> PopulateSalary(string socCode, string jobProfileUrlName)
        {
            applicationLogger.Trace($"BEGIN: Method '{nameof(PopulateSalary)}' called from '{nameof(JobProfileIndexEnhancer)}' with parameters '{socCode}'.");
            JobProfileSalary salary = new JobProfileSalary();
            try
            {
                salary = await salaryService.GetSalaryBySocAsync(socCode);
                salary.JobProfileName = jobProfileUrlName;
                salary.StarterSalary = Convert.ToDouble(salaryCalculator.GetStarterSalary(salary));
                salary.SalaryExperienced = Convert.ToDouble(salaryCalculator.GetExperiencedSalary(salary));

                if (salary.StarterSalary == 0 || salary.SalaryExperienced == 0)
                {
                    applicationLogger.Warn($"ERROR: Method '{nameof(PopulateSalary)}' called from '{nameof(JobProfileIndexEnhancer)}' with parameters '{socCode}' failed to get salary '{JsonConvert.SerializeObject(salary)}'.");
                }
            }
            catch (Exception ex)
            {
                //If there is a failure for this profile, log and continue with other profiles
                applicationLogger.ErrorJustLogIt($"ERROR: Method '{nameof(PopulateSalary)}' called from '{nameof(JobProfileIndexEnhancer)}' with parameters '{socCode}' failed with exception '{ex.Message}'.", ex);
            }
            finally
            {
                salary.JobProfileName = jobProfileUrlName;
                applicationLogger.Trace($"END: Method '{nameof(PopulateSalary)}' called from '{nameof(JobProfileIndexEnhancer)}' with parameters '{socCode}' returned {JsonConvert.SerializeObject(salary)}.");
            }

            return salary;
        }

        private IEnumerable<string> GetJobProfileCategoriesWithUrl()
        {
            var categories = jobProfileCategoryRepository.GetByIds(JobProfile.JobProfileCategoryIdCollection);
            return categories.Select(c => $"{c.Title}|{c.Url}");
        }

        private void PopulateCustomFields()
        {
            if (JobProfile != null)
            {
                jobProfileIndex.SocCode = JobProfile.SOCCode;
                jobProfileIndex.CareerPathAndProgression = JobProfile.CareerPathAndProgression;
                jobProfileIndex.WydDayToDayTasks = JobProfile.WYDDayToDayTasks;
                jobProfileIndex.CollegeRelevantSubjects = JobProfile.CollegeRelevantSubjects;
                jobProfileIndex.UniversityRelevantSubjects = JobProfile.UniversityRelevantSubjects;
                jobProfileIndex.ApprenticeshipRelevantSubjects = JobProfile.ApprenticeshipRelevantSubjects;
            }
        }
    }
}