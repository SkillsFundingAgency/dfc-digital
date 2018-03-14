using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;

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
                                    builder.Append($"{(builder.Length > 0 ? field.Value.ToString().ToLower() : string.Empty)} {field.Key}/any(t: search.in(t, '{fieldValue}')) ");
                                }
                            }
                        }
                    }
                }
            }

            return builder.ToString().Trim();
        }
    }
}