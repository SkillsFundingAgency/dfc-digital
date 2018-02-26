using DFC.Digital.Data.Attributes;
using DFC.Digital.Data.Interfaces;
using Microsoft.Azure.Search;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DFC.Digital.Data.Model
{
    public class JobProfileIndex : IDigitalDataModel
    {
        [Key]
        public string IdentityField { get; set; }

        // [IsSearchable, IsFilterable, IsSortable, IsSuggestable]//, Analyzer(AnalyzerName.AsString.EnMicrosoft)]
        public string FilterableTitle { get; set; }

        // [IsSearchable, IsFilterable, IsSortable, IsSuggestable]//, Analyzer(AnalyzerName.AsString.EnMicrosoft)]
        public string FilterableAlternativeTitle { get; set; }

        [IsSearchable, IsFilterable, IsSortable, IsSuggestible]// , Analyzer(AnalyzerName.AsString.EnMicrosoft)]
        public string Title { get; set; }

        [IsSearchable, IsFilterable, IsSuggestible]// , Analyzer(AnalyzerName.AsString.EnMicrosoft)]
        public IEnumerable<string> AlternativeTitle { get; set; }

        public string Overview { get; set; }

        [IsFilterable, IsSortable, IsFacetable]
        public double SalaryStarter { get; set; }

        [IsFilterable, IsSortable, IsFacetable]
        public double SalaryExperienced { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "This is an application field of type string, last segment and is not a complete uri")]
        [IsFilterable, IsFacetable]
        public string UrlName { get; set; }

        [IsFilterable]
        public IEnumerable<string> JobProfileCategories { get; set; }

        [IsSearchable, IsFilterable]
        public IEnumerable<string> JobProfileSpecialism { get; set; }

        [IsSearchable, IsFilterable]
        public IEnumerable<string> HiddenAlternativeTitle { get; set; }

        public IEnumerable<string> JobProfileCategoriesWithUrl { get; set; }

        [IsFilterable]
        public IEnumerable<string> Interests { get; set; }

        [IsFilterable]
        public IEnumerable<string> Enablers { get; set; }

        [IsFilterable]
        public IEnumerable<string> EntryQualifications { get; set; }

        [IsFilterable]
        public IEnumerable<string> TrainingRoutes { get; set; }

        [IsFilterable]
        public IEnumerable<string> PreferredTaskTypes { get; set; }

        [IsFilterable]
        public IEnumerable<string> JobAreas { get; set; }
    }
}
