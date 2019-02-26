using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Models;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class CourseSearchConverterService : ICourseSearchConverter
    {
        public string BuildRedirectPathAndQueryString(string courseSearchResultsPage,
            TrainingCourseResultsViewModel trainingCourseResultsViewModel, string locationDistanceRegex)
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

        public string BuildSearchRedirectPathAndQueryString(string courseSearchResultsPage,
            CourseLandingViewModel courseLandingViewModel, string locationDistanceRegex)
        {

            var queryParameters = $"{courseSearchResultsPage}?";
            if (!string.IsNullOrWhiteSpace(courseLandingViewModel.SearchTerm) || !string.IsNullOrWhiteSpace(courseLandingViewModel.ProviderKeyword))
            { 
                if (!string.IsNullOrWhiteSpace(courseLandingViewModel.SearchTerm) &&
                    !string.IsNullOrWhiteSpace(courseLandingViewModel.ProviderKeyword))
                {
                    queryParameters = $"{queryParameters}searchTerm={courseLandingViewModel.SearchTerm}&prv={courseLandingViewModel.ProviderKeyword}";
                }
                else if (!string.IsNullOrWhiteSpace(courseLandingViewModel.SearchTerm))
                {
                    queryParameters = $"{queryParameters}searchTerm={courseLandingViewModel.SearchTerm}";
                }
                else
                {
                    queryParameters = $"{queryParameters}prv={courseLandingViewModel.ProviderKeyword}";
                }

                if (!string.IsNullOrWhiteSpace(courseLandingViewModel.QualificationLevel) && !courseLandingViewModel.QualificationLevel.Equals("0"))
                {
                    queryParameters = $"{queryParameters}&qualificationlevel={courseLandingViewModel.QualificationLevel}";
                }

                if (!string.IsNullOrWhiteSpace(courseLandingViewModel.Location))
                {
                    queryParameters = $"{queryParameters}&location={courseLandingViewModel.Location}";
                }

                queryParameters = $"{queryParameters}&StartDate=Anytime";

                if (!string.IsNullOrWhiteSpace(courseLandingViewModel.Location) &&
                    Regex.IsMatch(courseLandingViewModel.Location, locationDistanceRegex))
                {
                    queryParameters = $"{queryParameters}&distance={courseLandingViewModel.Distance}";
                }
            }

            return queryParameters;
        }

        public CourseSearchRequest GetCourseSearchRequest(string searchTerm, int recordsPerPage, string attendance, string studymode, string qualificationLevel, string distance, string dfe1619Funded, string pattern, string location, int page)
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
                Location = location
            };

            return request;
        }

        public string GetUrlEncodedString(string input)
        {
            return !string.IsNullOrWhiteSpace(input) ? HttpUtility.UrlEncode(input) : string.Empty;
        }

        public void SetupPaging(TrainingCourseResultsViewModel viewModel, CourseSearchResponse response, string pathQuery, int  recordsPerPage, string courseSearchResultsPage)
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

        public IEnumerable<SelectItem> GetFilterSelectItems(string propertyName, IEnumerable<string> sourceList,
            string value)
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
    }
}