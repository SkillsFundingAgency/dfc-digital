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
            var queryParameters = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(trainingCourseResultsViewModel.SearchTerm) || !string.IsNullOrWhiteSpace(trainingCourseResultsViewModel.CourseFiltersModel.ProviderKeyword))
            {
                //K=maths&location=HG1%205EZ&prv=Keith%20St%20Peters%20Limited&Attendance=Class&StartDate=2016-08-14
                // &Distance=5&Sort=distance&map=0&SearchId=3434
                if (!string.IsNullOrWhiteSpace(trainingCourseResultsViewModel.SearchTerm) &&
                    !string.IsNullOrWhiteSpace(trainingCourseResultsViewModel.CourseFiltersModel.ProviderKeyword))
                {
                    queryParameters.AppendFormat("searchTerm={0}&prv={1}", trainingCourseResultsViewModel.SearchTerm,
                        trainingCourseResultsViewModel.CourseFiltersModel.ProviderKeyword);
                }
                else if (!string.IsNullOrWhiteSpace(trainingCourseResultsViewModel.SearchTerm))
                {
                    queryParameters.AppendFormat("searchTerm={0}", trainingCourseResultsViewModel.SearchTerm);
                }
                else
                {
                    queryParameters.AppendFormat("prv={0}",
                        trainingCourseResultsViewModel.CourseFiltersModel.ProviderKeyword);
                }

                if (trainingCourseResultsViewModel.CourseFiltersModel.AttendanceMode.Any())
                {
                    queryParameters.AppendFormat("&attendance={0}",
                        string.Join(",", trainingCourseResultsViewModel.CourseFiltersModel.AttendanceMode));
                }

                if (trainingCourseResultsViewModel.CourseFiltersModel.QualificationLevel.Any())
                {
                    queryParameters.AppendFormat("&qualificationlevel={0}",
                        string.Join(",", trainingCourseResultsViewModel.CourseFiltersModel.QualificationLevel));
                }

                queryParameters.AppendFormat("&dfe1619Funded={0}",
                    trainingCourseResultsViewModel.CourseFiltersModel.AgeSuitability);

                queryParameters.AppendFormat("&location={0}",
                    trainingCourseResultsViewModel.CourseFiltersModel.Location);

                queryParameters.AppendFormat("&StartDate={0}", "Anytime");


                if (!string.IsNullOrWhiteSpace(trainingCourseResultsViewModel.CourseFiltersModel.Location) &&
                    !string.IsNullOrWhiteSpace(locationDistanceRegex))
                {
                    if (Regex.Matches(trainingCourseResultsViewModel.CourseFiltersModel.Location, locationDistanceRegex)
                            .Count > 0)
                    {
                        queryParameters.AppendFormat("&distance={0}",
                            trainingCourseResultsViewModel.CourseFiltersModel.Distance);
                    }
                }
                if (trainingCourseResultsViewModel.CourseFiltersModel.StudyMode.Any())
                {
                    queryParameters.AppendFormat("&studymode={0}",
                        string.Join(",", trainingCourseResultsViewModel.CourseFiltersModel.StudyMode));
                }

                queryParameters.AppendFormat("&pageNo={0}", "1");
            }

            return queryParameters.ToString();
        }

        public CourseSearchRequest GetCourseSearchRequest(string searchTerm, int recordsPerPage, string attendance, string studymode, string qualificationLevel, string distance, string dfe1619Funded, int page)
        {
            var request = new CourseSearchRequest
            {
                SearchTerm = searchTerm,
                RecordsPerPage = recordsPerPage,
                PageNumber = page
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