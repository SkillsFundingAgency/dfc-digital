using DFC.Digital.Automation.Test.Utilities;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.AutomationTest.Utilities
{
    public class DummyJobProfileIndex
    {
        private DummyJobProfileIndex()
        {
        }

        public static JobProfileIndex GenerateJobProfileIndexDummy(string title)
        {
            return new JobProfileIndex
            {
                IdentityField = $"dummy{nameof(JobProfileIndex.FilterableTitle)}{title.ConvertToKey()}",
                FilterableTitle = $"dummy{nameof(JobProfileIndex.FilterableTitle)}{title.ConvertToKey()}".ToLowerInvariant(),
                Title = $"dummy{nameof(JobProfileIndex.Title)}{title.ConvertToKey()}",
                UrlName = $"dummy{nameof(JobProfileIndex.FilterableTitle)}{title.ConvertToKey()}",
                FilterableAlternativeTitle = $"dummy{nameof(JobProfileIndex.FilterableAlternativeTitle)}".ToLowerInvariant(),
                AlternativeTitle = $"dummy{nameof(JobProfileIndex.AlternativeTitle)}".Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()),
                Overview = $"dummy{nameof(JobProfileIndex.Overview)}",
                SalaryRange = $"dummy{nameof(JobProfileIndex.SalaryRange)}",
            };
        }

        public static IEnumerable<JobProfileIndex> GenerateJobProfileIndexDummyCollection(string title, int count)
        {
            for (int i = 1; i <= count; i++)
            {
                yield return new JobProfileIndex
                {
                    IdentityField = $"dummy{nameof(JobProfileIndex.FilterableTitle)}{title.ConvertToKey()}-{i}",
                    FilterableTitle = $"dummy{nameof(JobProfileIndex.FilterableTitle)}{title.ConvertToKey()}-{i}".ToLowerInvariant(),
                    Title = $"dummy{nameof(JobProfileIndex.Title)}{title.ConvertToKey()}-{i}",
                    UrlName = $"dummy{nameof(JobProfileIndex.FilterableTitle)}{title.ConvertToKey()}-{i}",
                    FilterableAlternativeTitle = $"dummy{nameof(JobProfileIndex.FilterableAlternativeTitle)}-{i}".ToLowerInvariant(),
                    AlternativeTitle = $"dummy{nameof(JobProfileIndex.AlternativeTitle)}-{i}".Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()),
                    Overview = $"dummy{nameof(JobProfileIndex.Overview)}-{i}",
                    SalaryRange = $"dummy{nameof(JobProfileIndex.SalaryRange)}-{i}",
                };
            }
        }

        public static IEnumerable<SearchResultItem<JobProfileIndex>> GenerateJobProfileResultItemDummyCollection(string title, int count, int page, bool useIndex = true)
        {
            int start = ((page - 1) * count) + 1;
            int end = page * count;

            for (int i = start; i <= end; i++)
            {
                yield return new SearchResultItem<JobProfileIndex>
                {
                    Rank = i,
                    ResultItem = new JobProfileIndex
                    {
                        IdentityField = $"dummy{nameof(JobProfileIndex.FilterableTitle)}{title.ConvertToKey()}{(useIndex ? i.ToString() : string.Empty)}",
                        FilterableTitle = $"dummy{nameof(JobProfileIndex.FilterableTitle)}{title.ConvertToKey()}{(useIndex ? i.ToString() : string.Empty)}".ToLowerInvariant(),
                        Title = $"dummy{nameof(JobProfileIndex.Title)}{title.ConvertToKey()}{(useIndex ? i.ToString() : string.Empty)}",
                        UrlName = $"dummy{nameof(JobProfileIndex.FilterableTitle)}{title.ConvertToKey()}{(useIndex ? i.ToString() : string.Empty)}",
                        FilterableAlternativeTitle = $"dummy{nameof(JobProfileIndex.FilterableAlternativeTitle)}{(useIndex ? i.ToString() : string.Empty)}".ToLowerInvariant(),
                        AlternativeTitle = $"dummy{nameof(JobProfileIndex.AlternativeTitle)}{(useIndex ? i.ToString() : string.Empty)}".Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()),
                        Overview = $"dummy{nameof(JobProfileIndex.Overview)}{(useIndex ? i.ToString() : string.Empty)}",
                        SalaryRange = $"dummy{nameof(JobProfileIndex.SalaryRange)}{(useIndex ? i.ToString() : string.Empty)}",
                    }
                };
            }
        }
    }
}