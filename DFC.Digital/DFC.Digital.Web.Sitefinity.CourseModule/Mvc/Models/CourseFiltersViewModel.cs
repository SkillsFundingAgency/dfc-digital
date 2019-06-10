using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class CourseFiltersViewModel : CourseSearchFilters
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

        public string LocationRegex { get; set; } = @"^([bB][fF][pP][oO]\s{0,1}[0-9]{1,4}|[gG][iI][rR]\s{0,1}0[aA][aA]|[a-pr-uwyzA-PR-UWYZ]([0-9]{1,2}|([a-hk-yA-HK-Y][0-9]|[a-hk-yA-HK-Y][0-9]([0-9]|[abehmnprv-yABEHMNPRV-Y]))|[0-9][a-hjkps-uwA-HJKPS-UW])\s{0,1}[0-9][abd-hjlnp-uw-zABD-HJLNP-UW-Z]{2})$";

        public bool IsDistanceLocation => !string.IsNullOrWhiteSpace(Location) &&
                                          Regex.Matches(Location, LocationRegex).Count > 0 && !Distance.Equals(default(float));

        public bool IsValidStartDateFrom =>
            StartDate == StartDate.SelectDateFrom && !StartDateFrom.Equals(DateTime.MinValue);

        public string CourseHoursDisplayName => CourseHours.GetAttribute<DisplayAttribute>().Name;

        public string CourseTypeDisplayName => CourseType.GetAttribute<DisplayAttribute>().Name;
    }
}