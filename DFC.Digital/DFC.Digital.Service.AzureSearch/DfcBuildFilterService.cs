using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFC.Digital.Service.AzureSearch
{
    public class DfcBuildFilterService : IBuildSearchFilterService
    {
        public string BuildPreSearchFilters(PreSearchFiltersResultsModel preSearchFilterModel, IDictionary<string, PreSearchFilterLogicalOperator> indexFields)
        {
            var builder = new System.Text.StringBuilder();
            if (indexFields != null)
            {
                foreach (var field in indexFields)
                {
                    var validIndexField = typeof(JobProfileIndex).GetProperties()
                        .Any(property => property.Name.Equals(field.Key));

                    if (validIndexField)
                    {
                        var fieldFilter = preSearchFilterModel?.Sections?.FirstOrDefault(section =>
                            section.SectionDataTypes.Equals(field.Key, StringComparison.InvariantCultureIgnoreCase));
                        if (fieldFilter != null)
                        {
                            var notApplicableSelected = fieldFilter.Options.Any(opt => opt.ClearOtherOptionsIfSelected);
                            if (!notApplicableSelected)
                            {
                                var fieldValue = fieldFilter.SingleSelectOnly
                                    ? fieldFilter.SingleSelectedValue
                                    : string.Join(",", fieldFilter.Options.Where(opt => opt.IsSelected).Select(opt => opt.OptionKey));
                                if (!string.IsNullOrWhiteSpace(fieldValue))
                                {
                                    builder.Append($"{GetOperator(builder, field)} {field.Key}/{GetFilter(field, fieldValue)}");
                                }
                            }
                        }
                    }
                }
            }

            return builder.ToString().Trim();
        }

        private string GetFilter(KeyValuePair<string, PreSearchFilterLogicalOperator> field, string fieldValue)
        {
            return field.Value == PreSearchFilterLogicalOperator.Nand || field.Value == PreSearchFilterLogicalOperator.Nor ? $"all(t: not(search.in(t, '{fieldValue}'))) " : $"any(t: search.in(t, '{fieldValue}')) ";
        }

        private object GetOperator(StringBuilder builder, KeyValuePair<string, PreSearchFilterLogicalOperator> field)
        {
            if (builder.Length > 0)
            {
                var logicalOperator = field.Value;
                logicalOperator = logicalOperator == PreSearchFilterLogicalOperator.Nand ? PreSearchFilterLogicalOperator.And : field.Value;
                logicalOperator = logicalOperator == PreSearchFilterLogicalOperator.Nor ? PreSearchFilterLogicalOperator.Or : logicalOperator;
                return logicalOperator.ToString().ToLower();
            }

            return string.Empty;
        }
    }
}