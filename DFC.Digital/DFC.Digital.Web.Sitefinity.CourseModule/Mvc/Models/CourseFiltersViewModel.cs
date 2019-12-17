using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

using FAC = DFC.FindACourseClient.Models.ExternalInterfaceModels;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class CourseFiltersViewModel : FAC.CourseSearchFilters
    {
        public string Only1619CoursesText { get; set; }

        public string StartDateExampleText { get; set; }

        public string CourseHoursSectionText { get; set; }

        public string StartDateSectionText { get; set; }

        public string CourseTypeSectionText { get; set; }

        public string ApplyFiltersText { get; set; }

        public string StartDateDay { get; set; }

        public string StartDateMonth { get; set; }

        public string StartDateYear { get; set; }

        public string WithinText { get; set; }

        public string CourseHoursDisplayName => CourseHours?.First().GetAttribute<DisplayAttribute>().Name;

        public string CourseTypeDisplayName => CourseTypes?.First().GetAttribute<DisplayAttribute>().Name;

        public bool FiltersApplied => Only1619Courses || StartDate != FAC.StartDate.Anytime ||
                                      CourseHours?.First() != FAC.Enums.CourseHours.All || CourseTypes.First() != FAC.Enums.CourseType.All ||
                                      !string.IsNullOrWhiteSpace(Location) || !string.IsNullOrWhiteSpace(Provider);

        public string ActiveFiltersProvidedByText { get; set; }

        public string ActiveFiltersOfText { get; set; }

        public string ActiveFiltersWithinText { get; set; }

        public string ActiveFiltersOnly1619CoursesText { get; set; }

        public string ActiveFiltersSuitableForText { get; set; }

        public string ActiveFiltersStartingFromText { get; set; }

        public string ActiveFiltersCoursesText { get; set; }

        public string ActiveFiltersShowingText { get; set; }

        public string ActiveFiltersMilesText { get; set; }

        public string FilterProviderLabel { get; set; }

        public string FilterLocationLabel { get; set; }

        public string Only1619CoursesSectionText { get; set; }

        public bool IsDistanceLocation => !string.IsNullOrWhiteSpace(Location) && Regex.IsMatch(Location, Constants.CourseSearchLocationRegularExpression);
    }
}