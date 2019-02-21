﻿using System.Collections.Generic;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Models;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public interface ICourseSearchConverter
    {
        string BuildRedirectPathAndQueryString(string courseSearchResultsPage, TrainingCourseResultsViewModel trainingCourseResultsViewModel, string locationDistanceRegex);

        string BuildSearchRedirectPathAndQueryString(string courseSearchResultsPage, CourseLandingViewModel courseLandingViewModel, string locationDistanceRegex);

        CourseSearchRequest GetCourseSearchRequest(string searchTerm, int recordsPerPage, string attendance, string studymode, string qualificationLevel, string distance, string dfe1619Funded, string pattern, string location, int page);
        string GetUrlEncodedString(string input);
        void SetupPaging(TrainingCourseResultsViewModel viewModel, CourseSearchResponse response, string searchTerm, int recordsPerPage, string courseSearchResultsPage);

        IEnumerable<SelectItem> GetFilterSelectItems(string propertyName, IEnumerable<string> sourceList, string value);
    }
}