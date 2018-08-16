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
        internal static IEnumerable<JobProfileIndex> ConvertToJobProfileIndex(this IEnumerable<IDocument> documents, IJobProfileIndexEnhancer jobProfileIndexEnhancer, IAsyncHelper asyncHelper, IApplicationLogger applicationLogger)
        {
            var measure = Stopwatch.StartNew();
            List<JobProfileIndex> indexes = new List<JobProfileIndex>();

            List<Task> salaryPopulation = new List<Task>();
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
                    HiddenAlternativeTitle = item.GetValue(nameof(JobProfileIndex.HiddenAlternativeTitle)) as IEnumerable<string>
                };

                var isSalaryOverriden = Convert.ToBoolean(item.GetValue(nameof(JobProfile.IsLMISalaryFeedOverriden)));

                jobProfileIndexEnhancer.Initialise(jobProfileIndex, documents.Count() == 1);
                jobProfileIndexEnhancer.PopulateRelatedFieldsWithUrl();

                if (!isSalaryOverriden)
                {
                    salaryPopulation.Add(jobProfileIndexEnhancer.PopulateSalary());
                }

                indexes.Add(jobProfileIndex);
            }

            applicationLogger.Info($"Took {measure.Elapsed} to complete converting to JP index.");

            //asyncHelper.Synchronise(() => Task.WhenAll(salaryPopulation));
            return indexes;
        }
    }
}