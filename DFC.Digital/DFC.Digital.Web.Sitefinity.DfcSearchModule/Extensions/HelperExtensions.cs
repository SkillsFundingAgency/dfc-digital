using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Services.Search.Data;

namespace DFC.Digital.Web.Sitefinity.DfcSearchModule.Extensions
{
    internal static class HelperExtensions
    {
        internal static IEnumerable<JobProfileIndex> ConvertToJobProfileIndex(this IEnumerable<IDocument> documents, IJobProfileIndexEnhancer jobProfileIndexEnhancer, IAsyncHelper asyncHelper)
        {
            List<JobProfileIndex> indexes = new List<JobProfileIndex>();
            foreach (var item in documents)
            {
                var jobProfile = new JobProfileIndex
                {
                    IdentityField = item.IdentityField.Value?.ToString(),
                    UrlName = item.GetValue(nameof(JobProfileIndex.UrlName))?.ToString(),
                    FilterableTitle = item.GetValue(nameof(JobProfileIndex.Title))?.ToString().ToLowerInvariant(),
                    Title = item.GetValue(nameof(JobProfileIndex.Title))?.ToString(),
                    FilterableAlternativeTitle = item.GetValue(nameof(JobProfileIndex.AlternativeTitle))?.ToString().ToLowerInvariant(),
                    AlternativeTitle = item.GetValue(nameof(JobProfileIndex.AlternativeTitle))?.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()),
                    Overview = item.GetValue(nameof(JobProfileIndex.Overview))?.ToString(),
                    JobProfileCategories = item.GetValue(nameof(JobProfileIndex.JobProfileCategories)) as IEnumerable<string>,
                    JobProfileSpecialism = item.GetValue(nameof(JobProfileIndex.JobProfileSpecialism)) as IEnumerable<string>,
                    HiddenAlternativeTitle = item.GetValue(nameof(JobProfileIndex.HiddenAlternativeTitle)) as IEnumerable<string>
                };

                jobProfileIndexEnhancer.Initialise(jobProfile);
                jobProfile.SalaryRange = asyncHelper.Synchronise(() => jobProfileIndexEnhancer.GetSalaryRangeAsync());
                jobProfile = jobProfileIndexEnhancer.GetRelatedFieldsWithUrl(jobProfile);
                indexes.Add(jobProfile);
            }

            return indexes;
        }
    }
}