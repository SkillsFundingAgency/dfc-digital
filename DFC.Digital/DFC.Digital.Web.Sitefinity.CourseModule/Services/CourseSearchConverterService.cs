using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using DFC.Digital.Data.Model;

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
                //K=maths&location=HG1%205EZ&prv=Keith%20St%20Peters%20Limited&Attendance=Class&StartDate=2016-08-14
                // &Distance=5&Sort=distance&map=0&SearchId=3434
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

                queryParameters = $"{queryParameters}&dfe1619Funded={trainingCourseResultsViewModel.CourseFiltersModel.AgeSuitability}";

                if (!string.IsNullOrWhiteSpace(trainingCourseResultsViewModel.CourseFiltersModel.Location))
                {
                    queryParameters = $"{queryParameters}&location={trainingCourseResultsViewModel.CourseFiltersModel.Location}";
                }

                queryParameters = $"{queryParameters}&StartDate=Anytime";

                if (!string.IsNullOrWhiteSpace(trainingCourseResultsViewModel.CourseFiltersModel.Location) &&
                    !string.IsNullOrWhiteSpace(locationDistanceRegex))
                {
                    if (Regex.Matches(trainingCourseResultsViewModel.CourseFiltersModel.Location, locationDistanceRegex)
                            .Count > 0)
                    {
                        queryParameters = $"{queryParameters}&distance={trainingCourseResultsViewModel.CourseFiltersModel.Distance}";
                    }
                }

                if (trainingCourseResultsViewModel.CourseFiltersModel.StudyMode.Any())
                {
                    queryParameters = $"{queryParameters}&studymode={string.Join(",", trainingCourseResultsViewModel.CourseFiltersModel.StudyMode)}";
                }

                queryParameters = $"{queryParameters}&page=1";
            }

            return queryParameters;
        }

        public CourseSearchRequest GetCourseSearchRequest(string searchTerm, int recordsPerPage, string attendance, string studymode, string qualificationLevel, string distance, string dfe1619Funded, int page)
        {
            var request = new CourseSearchRequest
            {
                SearchTerm = searchTerm,
                RecordsPerPage = recordsPerPage,
                PageNumber = page,
                Attendance = attendance,
                StudyMode = studymode,
                QualificationLevel = qualificationLevel,
                Dfe1619Funded = dfe1619Funded,
                Distance = distance
            };

            return request;
        }

        public string GetUrlEncodedString(string input)
        {
            return !string.IsNullOrWhiteSpace(input) ? HttpUtility.UrlEncode(input) : string.Empty;
        }

        public void SetupPaging(TrainingCourseResultsViewModel viewModel, CourseSearchResponse response, string searchTerm, int  recordsPerPage, string courseSearchResultsPage)
        {
            viewModel.RecordsPerPage = recordsPerPage;
            viewModel.CurrentPageNumber = response.CurrentPage;
            viewModel.TotalPagesCount = response.TotalPages;
            viewModel.ResultsCount = response.TotalResultCount;

            if (viewModel.TotalPagesCount > 1 && viewModel.TotalPagesCount >= viewModel.CurrentPageNumber)
            {
                viewModel.PaginationViewModel.HasPreviousPage = viewModel.CurrentPageNumber > 1;
                viewModel.PaginationViewModel.HasNextPage = viewModel.CurrentPageNumber < viewModel.TotalPagesCount;
                viewModel.PaginationViewModel.NextPageUrl = new Uri($"{courseSearchResultsPage}?searchTerm={HttpUtility.UrlEncode(searchTerm)}&page={viewModel.CurrentPageNumber + 1}", UriKind.RelativeOrAbsolute);
                viewModel.PaginationViewModel.NextPageUrlText = $"{viewModel.CurrentPageNumber + 1} of {viewModel.TotalPagesCount}";

                if (viewModel.CurrentPageNumber > 1)
                {
                    viewModel.PaginationViewModel.PreviousPageUrl = new Uri($"{courseSearchResultsPage}?searchTerm={HttpUtility.UrlEncode(searchTerm)}{(viewModel.CurrentPageNumber == 2 ? string.Empty : $"&page={viewModel.CurrentPageNumber - 1}")}", UriKind.RelativeOrAbsolute);
                    viewModel.PaginationViewModel.PreviousPageUrlText = $"{viewModel.CurrentPageNumber - 1} of {viewModel.TotalPagesCount}";
                }
            }
        }
    }
}