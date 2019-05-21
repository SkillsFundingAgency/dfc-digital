using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Models;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.CourseModule
{
    public class BuildQueryStringService : IBuildQueryStringService
    {
        public string BuildRedirectPathAndQueryString(string courseSearchResultsPage, TrainingCourseResultsViewModel trainingCourseResultsViewModel, string locationDistanceRegex)
        {
            var queryParameters = $"{courseSearchResultsPage}?";
            if (!string.IsNullOrWhiteSpace(trainingCourseResultsViewModel.SearchTerm) || !string.IsNullOrWhiteSpace(trainingCourseResultsViewModel.CourseFiltersModel.ProviderKeyword))
            {
                if (!string.IsNullOrWhiteSpace(trainingCourseResultsViewModel.SearchTerm) &&
                    !string.IsNullOrWhiteSpace(trainingCourseResultsViewModel.CourseFiltersModel.ProviderKeyword))
                {
                    queryParameters = $"{queryParameters}searchTerm={trainingCourseResultsViewModel.SearchTerm}&provider={trainingCourseResultsViewModel.CourseFiltersModel.ProviderKeyword}";
                }
                else if (!string.IsNullOrWhiteSpace(trainingCourseResultsViewModel.SearchTerm))
                {
                    queryParameters = $"{queryParameters}searchTerm={trainingCourseResultsViewModel.SearchTerm}";
                }
                else
                {
                    queryParameters = $"{queryParameters}provider={trainingCourseResultsViewModel.CourseFiltersModel.ProviderKeyword}";
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

                queryParameters = $"{queryParameters}&startDate=anytime";

                if (!string.IsNullOrWhiteSpace(trainingCourseResultsViewModel.CourseFiltersModel.Location) &&
                    !string.IsNullOrWhiteSpace(trainingCourseResultsViewModel.CourseFiltersModel.Distance) &&
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
            if (!string.IsNullOrWhiteSpace(courseLandingViewModel.SearchTerm) || !string.IsNullOrWhiteSpace(courseLandingViewModel.ProviderKeyword))
            {
                if (!string.IsNullOrWhiteSpace(courseLandingViewModel.SearchTerm) &&
                    !string.IsNullOrWhiteSpace(courseLandingViewModel.ProviderKeyword))
                {
                    queryParameters = $"{queryParameters}searchTerm={courseLandingViewModel.SearchTerm}&provider={courseLandingViewModel.ProviderKeyword}";
                }
                else if (!string.IsNullOrWhiteSpace(courseLandingViewModel.SearchTerm))
                {
                    queryParameters = $"{queryParameters}searchTerm={courseLandingViewModel.SearchTerm}";
                }
                else
                {
                    queryParameters = $"{queryParameters}provider={courseLandingViewModel.ProviderKeyword}";
                }

                if (!string.IsNullOrWhiteSpace(courseLandingViewModel.QualificationLevel) && !courseLandingViewModel.QualificationLevel.Equals("0"))
                {
                    queryParameters = $"{queryParameters}&qualificationlevel={courseLandingViewModel.QualificationLevel}";
                }

                if (!string.IsNullOrWhiteSpace(courseLandingViewModel.Location))
                {
                    queryParameters = $"{queryParameters}&location={courseLandingViewModel.Location}";
                }

                if (!string.IsNullOrWhiteSpace(courseLandingViewModel.Dfe1619Funded))
                {
                    queryParameters = $"{queryParameters}&dfe1619Funded={courseLandingViewModel.Dfe1619Funded}";
                }

                queryParameters = $"{queryParameters}&startDate=anytime";

                if (!string.IsNullOrWhiteSpace(courseLandingViewModel.Location) &&
                    !string.IsNullOrWhiteSpace(courseLandingViewModel.Distance) &&
                    Regex.IsMatch(courseLandingViewModel.Location, locationDistanceRegex))
                {
                    queryParameters = $"{queryParameters}&distance={courseLandingViewModel.Distance}";
                }
            }

            return queryParameters;
        }

        public string GetUrlEncodedString(string input)
        {
            return !string.IsNullOrWhiteSpace(input) ? HttpUtility.UrlEncode(input) : string.Empty;
        }
    }
}