using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.CourseSearchProvider.CourseSearchServiceApi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DFC.Digital.Service.CourseSearchProvider
{
    internal static class MessageConverter
    {
        internal static CourseListInput GetCourseListInput(string request)
        {
            var apiRequest = new CourseListInput
            {
                CourseListRequest = new CourseListRequestStructure
                {
                    CourseSearchCriteria = new SearchCriteriaStructure
                    {
                        APIKey = ConfigurationManager.AppSettings[Constants.CourseSearchApiKey],
                        SubjectKeyword = request,
                        EarliestStartDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        AttendanceModes = Convert.ToString(ConfigurationManager.AppSettings[Constants.CourseSearchAttendanceModes])?.Split(',')
                    },
                    RecordsPerPage = ConfigurationManager.AppSettings[Constants.CourseSearchPageSize],
                    PageNo = "1",
                    SortBy = SortType.A,
                    SortBySpecified = true
                }
            };

            return apiRequest;
        }

        internal static IEnumerable<Course> ConvertToCourse(this CourseListOutput apiResult)
        {
            var result = apiResult?.CourseListResponse?.CourseDetails?.Select(c =>
                new Course
                {
                    Title = c.Course.CourseTitle,
                    Location = (c.Opportunity.Item as VenueInfo)?.VenueAddress.Town,
                    ProviderName = c.Provider.ProviderName,
                    StartDate = Convert.ToDateTime(c.Opportunity.StartDate.Item),
                    CourseId = c.Course.CourseID
                });

            return result ?? Enumerable.Empty<Course>();
        }

        internal static IEnumerable<Course> ConvertToSearchCourse(this CourseListOutput apiResult)
        {
            var result = apiResult?.CourseListResponse?.CourseDetails?.Select(c =>
                new Course
                {
                    Title = c.Course.CourseTitle,
                    Location = (c.Opportunity.Item as VenueInfo)?.VenueAddress.Town,
                    ProviderName = c.Provider.ProviderName,
                    StartDateLabel = c.Opportunity.StartDate.Item,
                    CourseId = c.Course.CourseID,
                    AttendanceMode = c.Opportunity.AttendanceMode,
                    AttendancePattern = c.Opportunity.AttendancePattern,
                    QualificationLevel = c.Course.QualificationLevel,
                    StudyMode = c.Opportunity.StudyMode,
                    AdvancedLearnerLoansOffered = c.Provider.TFPlusLoans
                });

            return result ?? Enumerable.Empty<Course>();
        }

        internal static CourseDetails ConvertToCourseDetails(this CourseDetailOutput apiResult, string oppurtunityId)
        {
            var apiCourseDetail = apiResult.CourseDetails?.FirstOrDefault();

            if (apiCourseDetail == null)
            {
                return null;
            }

            return new CourseDetails
            {
                Title = apiCourseDetail.Course.CourseTitle,
                Description = apiCourseDetail.Course.CourseSummary,
                EntryRequirements = apiCourseDetail.Course.EntryRequirements,
                AssessmentMethod = apiCourseDetail.Course.AssessmentMethod,
                EquipmentRequired = apiCourseDetail.Course.EquipmentRequired,
                QualificationName = apiCourseDetail.Course.QualificationTitle,
                QualificationLevel = apiCourseDetail.Course.QualificationLevel,
                VenueDetails = new Venue
                {
                    EmailAddress = apiCourseDetail.Venue.FirstOrDefault()?.Email,
                    Location = new Address
                    {
                        AddressLine1 = apiCourseDetail.Venue.FirstOrDefault()?.VenueAddress.Address_line_1,
                        AddressLine2 = apiCourseDetail.Venue.FirstOrDefault()?.VenueAddress.Address_line_2,
                        County = apiCourseDetail.Venue.FirstOrDefault()?.VenueAddress.County,
                        Town = apiCourseDetail.Venue.FirstOrDefault()?.VenueAddress.Town,
                        Postcode = apiCourseDetail.Venue.FirstOrDefault()?.VenueAddress.PostCode,
                        Longitude = apiCourseDetail.Venue.FirstOrDefault()?.VenueAddress.Longitude,
                        Latitude = apiCourseDetail.Venue.FirstOrDefault()?.VenueAddress.Latitude,
                    },
                    PhoneNumber = apiCourseDetail.Venue.FirstOrDefault()?.Phone,
                    Website = apiCourseDetail.Venue.FirstOrDefault()?.Website,
                    VenueName = apiCourseDetail.Venue.FirstOrDefault()?.VenueName,
                    Fax = apiCourseDetail.Venue.FirstOrDefault()?.Fax,
                },
                ProviderDetails = new ProviderDetails
                {
                    EmailAddress = apiCourseDetail.Provider.Email,
                    AddressLine = apiCourseDetail.Provider.ProviderAddress.Address_line_1,
                    AddressLine2 = apiCourseDetail.Provider.ProviderAddress.Address_line_2,
                    Town = apiCourseDetail.Provider.ProviderAddress.Town,
                    County = apiCourseDetail.Provider.ProviderAddress.County,
                    PostCode = apiCourseDetail.Provider.ProviderAddress.PostCode,
                    Longitude = apiCourseDetail.Provider.ProviderAddress.Longitude,
                    Latitude = apiCourseDetail.Provider.ProviderAddress.Latitude,
                    PhoneNumber = apiCourseDetail.Provider.Phone,
                    Name = apiCourseDetail.Provider.ProviderName,
                    LearnerSatisfactionSpecified = apiCourseDetail.Provider.FEChoices_LearnerSatisfactionSpecified,
                    EmployerSatisfactionSpecified = apiCourseDetail.Provider.FEChoices_EmployerSatisfactionSpecified,
                    LearnerSatisfaction = apiCourseDetail.Provider.FEChoices_LearnerSatisfaction,
                    EmployerSatisfaction = apiCourseDetail.Provider.FEChoices_EmployerSatisfaction
                },
                Oppurtunities = GetOppurtunities(apiCourseDetail, oppurtunityId),
                CourseUrl = apiCourseDetail.Course.URL,
                CourseId = apiCourseDetail.Course.CourseID,
                Cost = !string.IsNullOrEmpty(oppurtunityId) ? apiCourseDetail.Opportunity.Where(op => op.OpportunityId == oppurtunityId).FirstOrDefault()?.Price : apiCourseDetail.Opportunity.FirstOrDefault()?.Price,
                StartDateLabel = !string.IsNullOrEmpty(oppurtunityId) ? apiCourseDetail.Opportunity.Where(op => op.OpportunityId == oppurtunityId).FirstOrDefault()?.StartDate.Item : apiCourseDetail.Opportunity.FirstOrDefault()?.StartDate.Item,
                AttendanceMode = !string.IsNullOrEmpty(oppurtunityId) ? apiCourseDetail.Opportunity.Where(op => op.OpportunityId == oppurtunityId).FirstOrDefault()?.AttendanceMode : apiCourseDetail.Opportunity.FirstOrDefault()?.AttendanceMode,
                AttendancePattern = !string.IsNullOrEmpty(oppurtunityId) ? apiCourseDetail.Opportunity.Where(op => op.OpportunityId == oppurtunityId).FirstOrDefault()?.AttendancePattern : apiCourseDetail.Opportunity.FirstOrDefault()?.AttendancePattern,
                StudyMode = !string.IsNullOrEmpty(oppurtunityId) ? apiCourseDetail.Opportunity.Where(op => op.OpportunityId == oppurtunityId).FirstOrDefault()?.StudyMode : apiCourseDetail.Opportunity.FirstOrDefault()?.StudyMode,
                Duration = $"{(!string.IsNullOrEmpty(oppurtunityId) ? apiCourseDetail.Opportunity.Where(op => op.OpportunityId == oppurtunityId).FirstOrDefault()?.Duration.DurationValue : apiCourseDetail.Opportunity.FirstOrDefault()?.Duration?.DurationValue)} {(!string.IsNullOrEmpty(oppurtunityId) ? apiCourseDetail.Opportunity.Where(op => op.OpportunityId == oppurtunityId).FirstOrDefault()?.Duration.DurationUnit : apiCourseDetail.Opportunity.FirstOrDefault()?.Duration?.DurationUnit)}"
            };
        }

        private static List<Oppurtunity> GetOppurtunities(CourseDetailStructure apiCourseDetail, string oppurtunityId)
        {
            List<Oppurtunity> oppurtunities = new List<Oppurtunity>();

            foreach (var oppurtunity in apiCourseDetail.Opportunity.Where(op => op.OpportunityId != oppurtunityId))
            {
                oppurtunities.Add(new Oppurtunity
                {
                    StartDate = oppurtunity.StartDate.Item,
                    OppurtunityId = oppurtunity.OpportunityId,
                    VenueName = apiCourseDetail.Venue.Where(x => x.VenueID.ToString() == oppurtunity.Items[0]).FirstOrDefault().VenueName
                });
            }

            return oppurtunities;
        }
    }
}