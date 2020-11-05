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

        public IEnumerable<KeyValuePair<string, PreSearchFilterLogicalOperator>> GetIndexFieldDefinitions(string indexFieldOperators)
        {
            var fields = indexFieldOperators.Split(',');

            var fieldDefinitions = new List<KeyValuePair<string, PreSearchFilterLogicalOperator>>();
            foreach (var field in fields)
            {
                var fieldDefinition = field.Split('|');
                if (fieldDefinition.Length == 2 && Enum.TryParse<PreSearchFilterLogicalOperator>(fieldDefinition[1], true, out var operand))
                {
                    fieldDefinitions.Add(new KeyValuePair<string, PreSearchFilterLogicalOperator>(fieldDefinition[0], operand));
                }
            }

            return fieldDefinitions;
        }

        public string GetSearchTerm(SearchProperties searchProperties, PreSearchFiltersResultsModel preSearchFiltersResultsModel, string[] searchFields)
        {
            var searchTerm = "*";
            var builder = new System.Text.StringBuilder();
            string usedSearchFields = string.Empty;

            if (searchFields == null)
            {
                return searchTerm;
            }

            foreach (var searchField in searchFields)
            {
                var fieldFilter = preSearchFiltersResultsModel.Sections?.FirstOrDefault(section =>
                        section.SectionDataTypes.Equals(searchField, StringComparison.InvariantCultureIgnoreCase));

                if (fieldFilter != null)
                {
                    var notApplicableSelected = fieldFilter.Options.Any(opt => opt.ClearOtherOptionsIfSelected);
                    if (!notApplicableSelected && !fieldFilter.SingleSelectOnly)
                    {
                        var fieldValue = string.Join(" + ", fieldFilter.Options.Where(opt => opt.IsSelected).Select(opt => opt.OptionKey));
                        if (!string.IsNullOrWhiteSpace(fieldValue))
                        {
                            builder.Append($"{fieldValue} + ");
                            usedSearchFields += $"{searchField},";
                        }
                    }
                }
            }

            if (builder.Length > 0)
            {
                searchProperties.SearchFields = usedSearchFields.TrimEnd(',').Split(',').ToList();
                searchTerm = builder.ToString();

                //remove the last " + "
                searchTerm = searchTerm.TrimEnd(' ', '+');
            }

            return searchTerm;
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