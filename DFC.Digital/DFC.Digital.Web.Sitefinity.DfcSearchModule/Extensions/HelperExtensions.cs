using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Sitefinity.Services.Search.Data;

namespace DFC.Digital.Web.Sitefinity.DfcSearchModule
{
    internal static class HelperExtensions
    {
        internal static IEnumerable<JobProfileIndex> ConvertToJobProfileIndex(this IEnumerable<IDocument> documents, IJobProfileIndexEnhancer jobProfileIndexEnhancer, IApplicationLogger applicationLogger)
        {
            var measure = Stopwatch.StartNew();
            Dictionary<string, JobProfileIndex> indexes = new Dictionary<string, JobProfileIndex>();

            var salaryPopulation = new List<Task<JobProfileSalary>>();
            var betaDocuments = documents.Where(d => Convert.ToBoolean(d.GetValue(nameof(JobProfile.IsImported)) ?? false) == false);
            foreach (var item in betaDocuments)
            {
                //TODO: Check and confirm that the removed FilterableTitle and FilterableAlternativeTitle are no longer used.
                var jobProfileIndex = new JobProfileIndex
                {
                    IdentityField = item.IdentityField.Value?.ToString(),
                    UrlName = item.GetValue(nameof(JobProfileIndex.UrlName))?.ToString(),
                    Title = item.GetValue(nameof(JobProfileIndex.Title))?.ToString(),
                    AlternativeTitle = item.GetValue(nameof(JobProfileIndex.AlternativeTitle))?.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()),
                    Overview = item.GetValue(nameof(JobProfileIndex.Overview))?.ToString(),
                    JobProfileCategories = item.GetValue(nameof(JobProfileIndex.JobProfileCategories)) as IEnumerable<string>,
                    JobProfileSpecialism = item.GetValue(nameof(JobProfileIndex.JobProfileSpecialism)) as IEnumerable<string>,
                    HiddenAlternativeTitle = item.GetValue(nameof(JobProfileIndex.HiddenAlternativeTitle)) as IEnumerable<string>,
                    WYDDayToDayTasks = item.GetValue(nameof(JobProfileIndex.WYDDayToDayTasks))?.ToString(),
                    CollegeRelevantSubjects = item.GetValue(nameof(JobProfileIndex.CollegeRelevantSubjects))?.ToString(),
                    UniversityRelevantSubjects = item.GetValue(nameof(JobProfileIndex.UniversityRelevantSubjects))?.ToString(),
                    ApprenticeshipRelevantSubjects = item.GetValue(nameof(JobProfileIndex.ApprenticeshipRelevantSubjects))?.ToString(),
                    CareerPathAndProgression = item.GetValue(nameof(JobProfileIndex.CareerPathAndProgression))?.ToString(),
                    WorkingPattern = item.GetValue(nameof(JobProfileIndex.WorkingPattern)) as IEnumerable<string>,
                    WorkingHoursDetails = item.GetValue(nameof(JobProfileIndex.WorkingHoursDetails)) as IEnumerable<string>,
                    WorkingPatternDetails = item.GetValue(nameof(JobProfileIndex.WorkingPatternDetails)) as IEnumerable<string>,
                    MinimumHours = Convert.ToDouble(item.GetValue(nameof(JobProfileIndex.MinimumHours))),
                    MaximumHours = Convert.ToDouble(item.GetValue(nameof(JobProfileIndex.MaximumHours)))
                };

                var isSalaryOverriden = Convert.ToBoolean(item.GetValue(nameof(JobProfile.IsLMISalaryFeedOverriden)));
                jobProfileIndexEnhancer.Initialise(jobProfileIndex, documents.Count() == 1);
                jobProfileIndexEnhancer.PopulateRelatedFieldsWithUrl();
                if (isSalaryOverriden)
                {
                    jobProfileIndex.SalaryStarter = Convert.ToDouble(item.GetValue(nameof(JobProfile.SalaryStarter)));
                    jobProfileIndex.SalaryExperienced = Convert.ToDouble(item.GetValue(nameof(JobProfile.SalaryExperienced)));
                }
                else
                {
                    if (!string.IsNullOrEmpty(jobProfileIndex.SocCode))
                    {
                        salaryPopulation.Add(jobProfileIndexEnhancer.PopulateSalary(jobProfileIndex.SocCode, jobProfileIndex.UrlName));
                    }
                }

                indexes.Add(jobProfileIndex.UrlName.ToLower(), jobProfileIndex);
            }

            var results = Task.Run(() => Task.WhenAll(salaryPopulation.ToArray())).GetAwaiter().GetResult();
            foreach (var idx in indexes)
            {
                var item = results.SingleOrDefault(r => r.JobProfileName.Equals(idx.Key, StringComparison.InvariantCultureIgnoreCase));
                if (item == null)
                {
                    applicationLogger.Warn($"WARN: Failed to get salary for '{idx.Key}'.");
                    continue;
                }

                idx.Value.SalaryStarter = item.StarterSalary;
                idx.Value.SalaryExperienced = item.SalaryExperienced;
            }

            applicationLogger.Info($"Took {measure.Elapsed} to complete converting to JP index. And got {results.Count()} salary info and {results.Count(r => r.StarterSalary == 0)} results that have salary missing. But {indexes.Values.Count(i => i.SalaryStarter == 0)} indexes missing salary information! from a total of {indexes.Values.Count()}");
            return indexes.Values;
        }
    }
}