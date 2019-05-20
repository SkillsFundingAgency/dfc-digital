﻿using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class CourseSearchConverterService : ICourseSearchConverter
    {
        public string BuildRedirectPathAndQueryString(string courseSearchResultsPage, TrainingCourseResultsViewModel trainingCourseResultsViewModel, string locationDistanceRegex)
        {
            var queryParameters = $"{courseSearchResultsPage}?";
            if (!string.IsNullOrWhiteSpace(trainingCourseResultsViewModel.SearchTerm) || !string.IsNullOrWhiteSpace(trainingCourseResultsViewModel.CourseFiltersModel.ProviderKeyword))
            {
                if (!string.IsNullOrWhiteSpace(trainingCourseResultsViewModel.SearchTerm) &&
                    !string.IsNullOrWhiteSpace(trainingCourseResultsViewModel.CourseFiltersModel.ProviderKeyword))
                {
                    queryParameters = $"{queryParameters}searchTerm={trainingCourseResultsViewModel.SearchTerm}&prv={trainingCourseResultsViewModel.CourseFiltersModel.ProviderKeyword}";
                }
                else if (!string.IsNullOrWhiteSpace(trainingCourseResultsViewModel.SearchTerm))
                {
                    queryParameters = $"{queryParameters}searchTerm={trainingCourseResultsViewModel.SearchTerm}";
                }
                else
                {
                    queryParameters = $"{queryParameters}prv={trainingCourseResultsViewModel.CourseFiltersModel.ProviderKeyword}";
                }

                if (trainingCourseResultsViewModel.CourseFiltersModel.AttendanceMode.Any())
                {
                    queryParameters = $"{queryParameters}&attendance={string.Join(",", trainingCourseResultsViewModel.CourseFiltersModel.AttendanceMode)}";
                }

                if (trainingCourseResultsViewModel.CourseFiltersModel.QualificationLevel.Any())
                {
                    queryParameters = $"{queryParameters}&qualificationlevel={string.Join(",", trainingCourseResultsViewModel.CourseFiltersModel.QualificationLevel)}";
                }

                if (!string.IsNullOrWhiteSpace(trainingCourseResultsViewModel.CourseFiltersModel.AgeSuitability))
                {
                    queryParameters =
                        $"{queryParameters}&dfe1619Funded={trainingCourseResultsViewModel.CourseFiltersModel.AgeSuitability}";
                }

                if (!string.IsNullOrWhiteSpace(trainingCourseResultsViewModel.CourseFiltersModel.Location))
                {
                    queryParameters = $"{queryParameters}&location={trainingCourseResultsViewModel.CourseFiltersModel.Location}";
                }

                if (trainingCourseResultsViewModel.CourseFiltersModel.AttendancePattern.Any())
                {
                    queryParameters = $"{queryParameters}&pattern={string.Join(",", trainingCourseResultsViewModel.CourseFiltersModel.AttendancePattern)}";
                }

                queryParameters = $"{queryParameters}&StartDate=Anytime";

                if (!string.IsNullOrWhiteSpace(trainingCourseResultsViewModel.CourseFiltersModel.Location) &&
                    Regex.IsMatch(trainingCourseResultsViewModel.CourseFiltersModel.Location, locationDistanceRegex))
                {
                    queryParameters = $"{queryParameters}&distance={trainingCourseResultsViewModel.CourseFiltersModel.Distance}";
                }

                if (trainingCourseResultsViewModel.CourseFiltersModel.StudyMode.Any())
                {
                    queryParameters = $"{queryParameters}&studymode={string.Join(",", trainingCourseResultsViewModel.CourseFiltersModel.StudyMode)}";
                }

                queryParameters = $"{queryParameters}&page=1";
            }

            return queryParameters;
        }

        public string BuildSearchRedirectPathAndQueryString(string courseSearchResultsPage, CourseLandingViewModel courseLandingViewModel, string locationDistanceRegex)
        {
            var queryParameters = $"{courseSearchResultsPage}?";
            if (!string.IsNullOrWhiteSpace(courseLandingViewModel.StrippedCourseName) || !string.IsNullOrWhiteSpace(courseLandingViewModel.StrippedProviderKeyword))
            {
                if (!string.IsNullOrWhiteSpace(courseLandingViewModel.StrippedCourseName) &&
                    !string.IsNullOrWhiteSpace(courseLandingViewModel.StrippedProviderKeyword))
                {
                    queryParameters = $"{queryParameters}searchTerm={HttpUtility.HtmlEncode(courseLandingViewModel.CourseName)}&prv={HttpUtility.HtmlEncode(courseLandingViewModel.ProviderKeyword)}";
                }
                else if (!string.IsNullOrWhiteSpace(courseLandingViewModel.StrippedCourseName))
                {
                    queryParameters = $"{queryParameters}searchTerm={HttpUtility.HtmlEncode(courseLandingViewModel.CourseName)}";
                }
                else
                {
                    queryParameters = $"{queryParameters}prv={HttpUtility.HtmlEncode(courseLandingViewModel.ProviderKeyword)}";
                }

                if (!string.IsNullOrWhiteSpace(courseLandingViewModel.QualificationLevel) && !courseLandingViewModel.QualificationLevel.Equals("0"))
                {
                    queryParameters = $"{queryParameters}&qualificationlevel={courseLandingViewModel.QualificationLevel}";
                }

                if (!string.IsNullOrWhiteSpace(courseLandingViewModel.StrippedLocation))
                {
                    queryParameters = $"{queryParameters}&location={HttpUtility.HtmlEncode(courseLandingViewModel.Location)}";
                }

                if (!string.IsNullOrWhiteSpace(courseLandingViewModel.Dfe1619Funded))
                {
                    queryParameters = $"{queryParameters}&dfe1619Funded={courseLandingViewModel.Dfe1619Funded}";
                }
            }

            return queryParameters;
        }

        public CourseSearchRequest GetCourseSearchRequest(string searchTerm, int recordsPerPage, string attendance, string studymode, string qualificationLevel, string distance, string dfe1619Funded, string pattern, string location, string sortBy, int page)
        {
            float.TryParse(distance, out var localDistance);
            var request = new CourseSearchRequest
            {
                SearchTerm = searchTerm,
                RecordsPerPage = recordsPerPage,
                PageNumber = page,
                Attendance = attendance,
                StudyMode = studymode,
                QualificationLevel = qualificationLevel,
                Dfe1619Funded = dfe1619Funded,
                Distance = localDistance,
                AttendancePattern = pattern,
                Location = location,
                CourseSearchSortBy = GetSortBy(sortBy)
            };

            return request;
        }

        public string GetUrlEncodedString(string input)
        {
            return !string.IsNullOrWhiteSpace(input) ? HttpUtility.UrlEncode(input) : string.Empty;
        }

        public void SetupPaging(TrainingCourseResultsViewModel viewModel, CourseSearchResponse response, string pathQuery, int recordsPerPage, string courseSearchResultsPage)
        {
            viewModel.RecordsPerPage = recordsPerPage;
            viewModel.CurrentPageNumber = response.CurrentPage;
            viewModel.TotalPagesCount = response.TotalPages;
            viewModel.ResultsCount = response.TotalResultCount;

            if (viewModel.TotalPagesCount > 1 && viewModel.TotalPagesCount >= viewModel.CurrentPageNumber)
            {
                viewModel.PaginationViewModel.HasPreviousPage = viewModel.CurrentPageNumber > 1;
                viewModel.PaginationViewModel.HasNextPage = viewModel.CurrentPageNumber < viewModel.TotalPagesCount;
                viewModel.PaginationViewModel.NextPageUrl = new Uri($"{pathQuery}&page={viewModel.CurrentPageNumber + 1}", UriKind.RelativeOrAbsolute);
                viewModel.PaginationViewModel.NextPageUrlText = $"{viewModel.CurrentPageNumber + 1} of {viewModel.TotalPagesCount}";

                if (viewModel.CurrentPageNumber > 1)
                {
                    viewModel.PaginationViewModel.PreviousPageUrl = new Uri($"{pathQuery}{(viewModel.CurrentPageNumber == 2 ? string.Empty : $"&page={viewModel.CurrentPageNumber - 1}")}", UriKind.RelativeOrAbsolute);
                    viewModel.PaginationViewModel.PreviousPageUrlText = $"{viewModel.CurrentPageNumber - 1} of {viewModel.TotalPagesCount}";
                }
            }
        }

        public IEnumerable<SelectItem> GetFilterSelectItems(string propertyName, IEnumerable<string> sourceList, string value)
        {
            var selectList = new List<SelectItem>();
            var itemList = value?.Split(',');
            foreach (var sourceItem in sourceList)
            {
                var dataValues = sourceItem.Split(':');
                if (dataValues.Length == 2)
                {
                    selectList.Add(new SelectItem
                    {
                        Checked = itemList != null && itemList.Contains(dataValues[1].Trim()) ? "checked" : string.Empty,
                        Label = dataValues[0].Trim(),
                        Name = propertyName,
                        Id = $"{propertyName}{dataValues[1].Trim()}",
                        Value = dataValues[1].Trim()
                    });
                }
            }

            return selectList;
        }

        public OrderByLinks GetOrderByLinks(string searchUrl, CourseSearchSortBy courseSearchSortBy)
        {
            return new OrderByLinks
            {
                CourseSearchSortBy = courseSearchSortBy,
                OrderByRelevanceUrl = new Uri($"{searchUrl}&sortby=1", UriKind.RelativeOrAbsolute),
                OrderByDistanceUrl = new Uri($"{searchUrl}&sortby=2", UriKind.RelativeOrAbsolute),
                OrderByStartDateUrl = new Uri($"{searchUrl}&sortby=3", UriKind.RelativeOrAbsolute)
            };
        }

        public string GetActiveFilterOptions(CourseFiltersModel courseFiltersModel, string locationDistanceRegex)
        {
            var activeFilters = string.Empty;

            if (!string.IsNullOrWhiteSpace(courseFiltersModel.Location) &&
                Regex.IsMatch(courseFiltersModel.Location, locationDistanceRegex))
            {
                activeFilters = $"Within {courseFiltersModel.DistanceSelectedList.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Checked))?.Label} of {courseFiltersModel.Location}";
            }
            else if (!string.IsNullOrWhiteSpace(courseFiltersModel.Location))
            {
                activeFilters = courseFiltersModel.Location;
            }

            if (courseFiltersModel.AttendanceSelectedList.Any(x => !string.IsNullOrWhiteSpace(x.Checked)))
            {
                if (!string.IsNullOrWhiteSpace(activeFilters))
                {
                    activeFilters = $"{activeFilters},";
                }

                activeFilters =
                    $"{activeFilters} {string.Join(", ", courseFiltersModel.AttendanceSelectedList.Where(x => !string.IsNullOrWhiteSpace(x.Checked)).Select(lbl => lbl.Label))}";
            }

            if (courseFiltersModel.PatternSelectedList.Any(x => !string.IsNullOrWhiteSpace(x.Checked)))
            {
                if (!string.IsNullOrWhiteSpace(activeFilters))
                {
                    activeFilters = $"{activeFilters},";
                }

                activeFilters =
                    $"{activeFilters} {string.Join(", ", courseFiltersModel.PatternSelectedList.Where(x => !string.IsNullOrWhiteSpace(x.Checked)).Select(lbl => lbl.Label))}";
            }

            if (courseFiltersModel.QualificationSelectedList.Any(x => !string.IsNullOrWhiteSpace(x.Checked)))
            {
                if (!string.IsNullOrWhiteSpace(activeFilters))
                {
                    activeFilters = $"{activeFilters},";
                }

                activeFilters =
                    $"{activeFilters} {string.Join(", ", courseFiltersModel.QualificationSelectedList.Where(x => !string.IsNullOrWhiteSpace(x.Checked)).Select(lbl => lbl.Label))}";
            }

            if (courseFiltersModel.AgeSuitabilitySelectedList.Any(x => !string.IsNullOrWhiteSpace(x.Checked)))
            {
                if (!string.IsNullOrWhiteSpace(activeFilters))
                {
                    activeFilters = $"{activeFilters},";
                }

                activeFilters =
                    $"{activeFilters} {string.Join(", ", courseFiltersModel.AgeSuitabilitySelectedList.Where(x => !string.IsNullOrWhiteSpace(x.Checked)).Select(lbl => lbl.Label))}";
            }

            if (courseFiltersModel.StudyModeSelectedList.Any(x => !string.IsNullOrWhiteSpace(x.Checked)))
            {
                if (!string.IsNullOrWhiteSpace(activeFilters))
                {
                    activeFilters = $"{activeFilters},";
                }

                activeFilters =
                    $"{activeFilters} {string.Join(", ", courseFiltersModel.StudyModeSelectedList.Where(x => !string.IsNullOrWhiteSpace(x.Checked)).Select(lbl => lbl.Label))}";
            }

            return activeFilters;
        }

        private CourseSearchSortBy GetSortBy(string sortBy)
        {
            switch (sortBy)
            {
                case "2":
                    return CourseSearchSortBy.Distance;
                case "3":
                    return CourseSearchSortBy.StartDate;
                default:
                    return CourseSearchSortBy.Relevance;
            }
        }
    }
}