using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.AutomationTest.Utilities
{
    public static class DummyJobProfileIndex
    {
        public static JobProfileIndex GenerateJobProfileIndexDummy(string title)
        {
            return new JobProfileIndex
            {
                IdentityField = $"dummy{title.ConvertToKey()}",
                Title = $"dummy{nameof(JobProfileIndex.Title)}{title.ConvertToKey()}",
                AlternativeTitle = $"dummy{nameof(JobProfileIndex.AlternativeTitle)}"
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()),
                Overview = $"dummy{nameof(JobProfileIndex.Overview)}",
                SalaryStarter = 10,
                SalaryExperienced = 10
            };
        }

        public static IEnumerable<JobProfileIndex> GenerateJobProfileIndexDummyCollection(string title, int count)
        {
            for (int i = 1; i <= count; i++)
            {
                yield return new JobProfileIndex
                {
                    IdentityField = $"dummy{title.ConvertToKey()}-{i}",
                    Title = $"dummy{nameof(JobProfileIndex.Title)}{title.ConvertToKey()}-{i}",
                    UrlName = $"dummy{title.ConvertToKey()}-{i}",
                    AlternativeTitle = $"dummy{nameof(JobProfileIndex.AlternativeTitle)}-{i}".Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()),
                    Overview = $"dummy{nameof(JobProfileIndex.Overview)}-{i}",
                    SalaryStarter = 10,
                    SalaryExperienced = 10
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
                        IdentityField = $"dummy{title.ConvertToKey()}{(useIndex ? i.ToString() : string.Empty)}",
                        Title = $"dummy{nameof(JobProfileIndex.Title)}{title.ConvertToKey()}{(useIndex ? i.ToString() : string.Empty)}",
                        UrlName = $"dummy{title.ConvertToKey()}{(useIndex ? i.ToString() : string.Empty)}",
                        AlternativeTitle = $"dummy{nameof(JobProfileIndex.AlternativeTitle)}{(useIndex ? i.ToString() : string.Empty)}".Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()),
                        Overview = $"dummy{nameof(JobProfileIndex.Overview)}{(useIndex ? i.ToString() : string.Empty)}",
                        SalaryStarter = 10,
                        SalaryExperienced = 10
                    }
                };
            }
        }

        public static IEnumerable<SearchResultItem<JobProfileIndex>> GenerateJobProfileResultItemDummyCollectionWithOrderPicker(string title, bool useIndex = true)
        {
            return new List<SearchResultItem<JobProfileIndex>>
            {
                new SearchResultItem<JobProfileIndex>
                {
                    Rank = 1,
                    ResultItem = new JobProfileIndex
                    {
                        IdentityField = $"dummy{title.ConvertToKey()}{(useIndex ? "1" : string.Empty)}",
                        Title = $"Packer",
                        UrlName = $"dummy{title.ConvertToKey()}{(useIndex ? "1" : string.Empty)}",
                        AlternativeTitle = new[] { "Order packer", "order picker" },
                        Overview = $"dummy{nameof(JobProfileIndex.Overview)}{(useIndex ? "1" : string.Empty)}",
                        SalaryStarter = 10,
                        SalaryExperienced = 10
                    }
                },
                new SearchResultItem<JobProfileIndex>
                {
                    Rank = 2,
                    ResultItem = new JobProfileIndex
                    {
                        IdentityField = $"dummy{title.ConvertToKey()}{(useIndex ? "2" : string.Empty)}",
                        Title = $"Order Picker",
                        UrlName = $"dummy{title.ConvertToKey()}{(useIndex ? "2" : string.Empty)}",
                        AlternativeTitle = new[] { "Online personal shopper, warehouse assistant" },
                        Overview = $"dummy{nameof(JobProfileIndex.Overview)}{(useIndex ? "2" : string.Empty)}",
                        SalaryStarter = 10,
                        SalaryExperienced = 10
                    }
                }
            };
        }
    }
}
