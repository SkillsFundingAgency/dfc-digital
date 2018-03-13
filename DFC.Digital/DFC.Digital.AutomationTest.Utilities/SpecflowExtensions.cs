using DFC.Digital.AutomationTest.Utilities;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;

namespace DFC.Digital.AutomationTest.Utilities
{
    public static class SpecflowExtensions
    {
        public static IEnumerable<JobProfileIndex> ToJobProfileSearchIndex(this Table table)
        {
            foreach (var item in table.Rows)
            {
                yield return new JobProfileIndex
                {
                    Title = item[nameof(JobProfileIndex.Title)],
                    IdentityField = item.GetConditionalData(nameof(JobProfileIndex.IdentityField), item[nameof(JobProfileIndex.Title)].ConvertToKey()),
                    AlternativeTitle = item.GetConditionalData(nameof(JobProfileIndex.AlternativeTitle), string.Empty)?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()),
                    Overview = item.GetConditionalData(nameof(JobProfileIndex.Overview), string.Empty),
                    JobProfileCategories = item.GetConditionalData(nameof(JobProfileIndex.JobProfileCategories), string.Empty)?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                    JobProfileSpecialism = item.GetConditionalData(nameof(JobProfileIndex.JobProfileSpecialism), string.Empty)?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                    HiddenAlternativeTitle = item.GetConditionalData(nameof(JobProfileIndex.HiddenAlternativeTitle), string.Empty)?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                    SalaryStarter = item.GetConditionalData<double>(nameof(JobProfileIndex.SalaryStarter), 0),
                    SalaryExperienced = item.GetConditionalData<double>(nameof(JobProfileIndex.SalaryExperienced), 0),
                    JobProfileCategoriesWithUrl = item.GetConditionalData(nameof(JobProfileIndex.JobProfileCategoriesWithUrl), string.Empty)?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                    Interests = item.GetConditionalData(nameof(JobProfileIndex.Interests), string.Empty)?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                    Enablers = item.GetConditionalData(nameof(JobProfileIndex.Enablers), string.Empty)?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                    EntryQualifications = item.GetConditionalData(nameof(JobProfileIndex.EntryQualifications), string.Empty)?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                    TrainingRoutes = item.GetConditionalData(nameof(JobProfileIndex.TrainingRoutes), string.Empty)?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                    PreferredTaskTypes = item.GetConditionalData(nameof(JobProfileIndex.PreferredTaskTypes), string.Empty)?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                    JobAreas = item.GetConditionalData(nameof(JobProfileIndex.JobAreas), string.Empty)?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                };
            }
        }

        public static IEnumerable<JobProfileIndex> CreateWithTitle(this int countOfDummies, string jobTitle)
        {
            for (int id = 0; id < countOfDummies; id++)
            {
                yield return new JobProfileIndex
                {
                    IdentityField = $"{jobTitle.ConvertToKey()}-{id}",
                    Title = jobTitle,
                    UrlName = $"{jobTitle.ConvertToKey()}-{id}"
                };
            }
        }

        public static SearchProperties ToSearchProperties(this Table table)
        {
            if (table == null || table.Rows.Count < 1)
            {
                throw new TestException("Empty or null table passed");
            }

            var interests = table.Rows[0]["Interests"];
            var trainingRoutes = table.Rows[0]["TrainingRoutes"];
            var enablers = table.Rows[0]["Enablers"];
            var entryqualifications = table.Rows[0]["EntryQualifications"];
            var jobAreas = table.Rows[0]["JobAreas"];
            var preferredTaskTypes = table.Rows[0]["PreferredTaskTypes"];

            var finalfilter = string.Empty;

            if (!string.IsNullOrWhiteSpace(interests))
            {
                finalfilter = $"{nameof(JobProfileIndex.Interests)}/any(t: search.in(t, '{interests}'))";
            }

            if (!string.IsNullOrWhiteSpace(trainingRoutes))
            {
                finalfilter += $"{(string.IsNullOrWhiteSpace(finalfilter) ? string.Empty : " or ")} {nameof(JobProfileIndex.TrainingRoutes)}/any(t: search.in(t, '{trainingRoutes}'))";
            }

            if (!string.IsNullOrWhiteSpace(entryqualifications))
            {
                finalfilter += $"{(string.IsNullOrWhiteSpace(finalfilter) ? string.Empty : " or ")}{nameof(JobProfileIndex.EntryQualifications)}/any(t: search.in(t, '{entryqualifications}'))";
            }

            if (!string.IsNullOrWhiteSpace(jobAreas))
            {
                finalfilter += $"{(string.IsNullOrWhiteSpace(finalfilter) ? string.Empty : " or ")}{nameof(JobProfileIndex.JobAreas)}/any(t: search.in(t, '{jobAreas}'))";
            }

            if (!string.IsNullOrWhiteSpace(preferredTaskTypes))
            {
                finalfilter += $"{(string.IsNullOrWhiteSpace(finalfilter) ? string.Empty : " or ")}{nameof(JobProfileIndex.PreferredTaskTypes)}/any(t: search.in(t, '{preferredTaskTypes}'))";
            }

            if (!string.IsNullOrWhiteSpace(enablers))
            {
                finalfilter = $"{finalfilter} {(string.IsNullOrWhiteSpace(finalfilter) ? string.Empty : " and ")}{nameof(JobProfileIndex.Enablers)}/any(t: search.in(t, '{enablers}'))";
            }

            var result = new SearchProperties
            {
                UseRawSearchTerm = true,
                Count = 100,
                FilterBy = finalfilter
            };

            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification ="String is only used for Azure index test")]
        internal static string ConvertToKey(this string rawKey)
        {
            Regex regex = new Regex(@"[ ()']");
            return regex.Replace(rawKey, match =>
            {
                return "-";
            }).TrimEnd('-').ToLowerInvariant();
        }

        private static T GetConditionalData<T>(this TableRow item, string field, T defaultValue)
        {
            return item.ContainsKey(field) ? (T)Convert.ChangeType(item[field], typeof(T)) : defaultValue;
        }
    }
}