using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;

namespace DFC.Digital.Automation.Test.Utilities
{
    public static class SpecflowExtensions
    {
        public static IEnumerable<JobProfileIndex> ToJobProfileSearchIndex(this Table table)
        {
            foreach (var item in table.Rows)
            {
                yield return new JobProfileIndex
                {
                    FilterableTitle = item[nameof(JobProfileIndex.Title)].ToLowerInvariant(),
                    Title = item[nameof(JobProfileIndex.Title)],
                    IdentityField = item.GetConditionalData(nameof(JobProfileIndex.IdentityField), item[nameof(JobProfileIndex.Title)].ConvertToKey()),
                    FilterableAlternativeTitle = item.GetConditionalData(nameof(JobProfileIndex.AlternativeTitle)).ToLowerInvariant(),
                    AlternativeTitle = item.GetConditionalData(nameof(JobProfileIndex.AlternativeTitle))?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()),
                    Overview = item.GetConditionalData(nameof(JobProfileIndex.Overview)),
                    JobProfileCategories = item.GetConditionalData(nameof(JobProfileIndex.JobProfileCategories))?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                    JobProfileSpecialism = item.GetConditionalData(nameof(JobProfileIndex.JobProfileSpecialism))?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                    HiddenAlternativeTitle = item.GetConditionalData(nameof(JobProfileIndex.HiddenAlternativeTitle))?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                    SalaryStarter = item.GetConditionalData<double>(nameof(JobProfileIndex.SalaryStarter)),
                    SalaryExperienced = item.GetConditionalData<double>(nameof(JobProfileIndex.SalaryExperienced)),
                    JobProfileCategoriesWithUrl = item.GetConditionalData(nameof(JobProfileIndex.JobProfileCategoriesWithUrl))?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                    Interests = item.GetConditionalData(nameof(JobProfileIndex.Interests))?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                    Enablers = item.GetConditionalData(nameof(JobProfileIndex.Enablers))?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                    EntryQualifications = item.GetConditionalData(nameof(JobProfileIndex.EntryQualifications))?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                    TrainingRoutes = item.GetConditionalData(nameof(JobProfileIndex.TrainingRoutes))?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                    PreferredTaskTypes = item.GetConditionalData(nameof(JobProfileIndex.PreferredTaskTypes))?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries),
                    JobAreas = item.GetConditionalData(nameof(JobProfileIndex.JobAreas))?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
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
                    FilterableTitle = jobTitle.ToLowerInvariant(),
                    Title = jobTitle,
                    UrlName = $"{jobTitle.ConvertToKey()}-{id}"
                };
            }
        }

        public static SearchProperties ToSearchProperties(this Table table)
        {
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

        internal static string ConvertToKey(this string rawKey)
        {
            Regex regex = new Regex(@"[ ()']");
            return regex.Replace(rawKey, match =>
            {
                return "-";
            }).TrimEnd('-').ToLowerInvariant();
        }

        private static string GetConditionalData(this TableRow item, string field, string defaultValue = null)
        {
            return item.ContainsKey(field) ? item[field] : (defaultValue ?? string.Empty);
        }

        private static T GetConditionalData<T>(this TableRow item, string field)
        {
            return item.ContainsKey(field) ? (T)(object)item[field] : default(T);
        }
    }
}